using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinBalladsOfBreezeAider
{
    public partial class frmMain : Form
    {
        private int genshinWindowX = 0;
        private int genshinWindowY = 0;
        private int genshinWindowWdith = 0;
        private int genshinWindowHeight = 0;
        private bool working = false;

        #region -- 按键钩子 --
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        #endregion --按键钩子 --

        #region -- 窗口钩子 --
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        const int HORZRES = 8;
        const int VERTRES = 10;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;

        private IntPtr hwndGenshin = IntPtr.Zero;
        #endregion -- 窗口钩子 --

        #region -- 按键监听钩子 --
        protected delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        protected static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        protected static extern int UnhookWindowsHookEx(int idHook);

        [StructLayout(LayoutKind.Sequential)]
        protected class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        protected int _handleToHook;
        protected HookProc _hookCallback;
        #endregion -- 按键监听钩子 --

        public static float DpiScaleX
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                float ScaleX = (float)GetDeviceCaps(hdc, DESKTOPHORZRES) / (float)GetDeviceCaps(hdc, HORZRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }

        public static float DpiScaleY
        {
            get
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                float ScaleY = (float)(float)GetDeviceCaps(hdc, DESKTOPVERTRES) / (float)GetDeviceCaps(hdc, VERTRES);
                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleY;
            }
        }

        public frmMain() => InitializeComponent();

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
#if DEBUG
            btnDebugScreenshot.Visible = true;
            debugTextBox.Visible = true;
#endif
            _hookCallback = new HookProc(HookCallbackProcedure);
            _handleToHook = SetWindowsHookEx(13, _hookCallback, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

            FindGenshinProcess();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            UnhookWindowsHookEx(_handleToHook);
            Process.GetCurrentProcess().Kill();
            base.OnClosing(e);
        }

        private int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {
            KeyboardHookStruct keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            if (wParam == 0x100 || wParam == 0x104)  //KeyDown事件
            {
                switch (keyboardHookStruct.vkCode)
                {
                    case (int)Keys.F12:
                        SaveDebugImage();
                        break;
                    case (int)Keys.F11:
                        Invoke(new Action(() => debugTextBox.AppendText($"{DateTime.Now.ToString("HH:mm:ss fff")}\r\n")));
                        break;
                    case (int)Keys.F10:
                        Start();
                        break;
                }
            }
            return 0;
        }

        private void FindGenshinProcess()
        {
            lblStatus.Text = $"未找到原神进程";
            lblStatus.ForeColor = Color.Red;
            Task.Run(() =>
            {
                while (!working)
                {
                    while (hwndGenshin == IntPtr.Zero)
                    {
                        hwndGenshin = FindWindow("UnityWndClass", "原神");
                        Task.Delay(100).Wait();
                    }

                    GetWindowRect(hwndGenshin, out Rectangle windowRect);
                    GetClientRect(hwndGenshin, out Rectangle clientRect);

                    Invoke(new Action(() =>
                    {
                        try
                        {
                            if (!working)
                            {
                                lblStatus.Text = $"已找到原神进程，未开始自动演奏";
                                lblStatus.ForeColor = Color.Black;
                            }

                            if (windowRect.X < -16000 || (windowRect.X == 0 && windowRect.Y == 0 && windowRect.Width == Screen.PrimaryScreen.Bounds.Width && windowRect.Height == Screen.PrimaryScreen.Bounds.Height))  //全屏
                            {
                                genshinWindowX = 0;
                                genshinWindowY = 0;
                                genshinWindowWdith = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * DpiScaleX);
                                genshinWindowHeight = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);

                                lblWindowType.Text = "全屏";
                                lblWindowLocation.Text = $"(0,0)";
                            }
                            else  //窗口化
                            {
                                Rectangle tempRect = new Rectangle(windowRect.X, windowRect.Y, windowRect.Width - windowRect.X, windowRect.Height - windowRect.Y);
                                Rectangle rect = new Rectangle(tempRect.X + (tempRect.Width - clientRect.Width) - 3, tempRect.Y + (tempRect.Height - clientRect.Height) - 3, clientRect.Width, clientRect.Height);

                                if (DpiScaleX > 1 || DpiScaleY > 1)
                                    rect = new Rectangle((int)Math.Round(rect.X * DpiScaleX), (int)Math.Round(rect.Y * DpiScaleY), (int)Math.Round(rect.Width * DpiScaleX), (int)Math.Round(rect.Height * DpiScaleY));

                                genshinWindowX = rect.X;
                                genshinWindowY = rect.Y;
                                genshinWindowWdith = rect.Width;
                                genshinWindowHeight = rect.Height;

                                lblWindowType.Text = "窗口化";
                                lblWindowLocation.Text = $"({genshinWindowX},{genshinWindowY})";
                            }

                            lblDpi.Text = $"{DpiScaleX * 100}%";

                            if (genshinWindowWdith / 16 * 9 == genshinWindowHeight)
                            {
                                lblRatio.Text = "16:9";
                                lblRatio.ForeColor = Color.Black;
                                lblWarring.Text = "";

                                if (DpiScaleX != 1)
                                    lblWarring.Text = "您显示设置-更改文本、应用等项目的大小不是100%，可能在坐标换算中产生偏移。";
                            }
                            else
                            {
                                if (genshinWindowWdith / 16 * 10 == genshinWindowHeight)
                                    lblRatio.Text = "16:10";
                                else if (genshinWindowWdith / 4 * 3 == genshinWindowHeight)
                                    lblRatio.Text = "4:3";
                                else if (genshinWindowWdith / 5 * 4 == genshinWindowHeight)
                                    lblRatio.Text = "5:4";
                                else
                                    lblRatio.Text = "未知";
                                lblRatio.ForeColor = Color.Red;
                                lblWarring.Text = "尚未适配当前分辨率，请切换到长宽比为16:9的分辨率";
                            }

                            lblWindowSize.Text = $"{genshinWindowWdith}×{genshinWindowHeight}";

                            btnStart.Enabled = true;
                        }
                        catch
                        {
                        }
                    }));
                    Task.Delay(100).Wait();
                }
            });
        }

        private void btnStart_Click(object sender, EventArgs e) => Start();

        private void Start()
        {
            if (working)
            {
                working = false;
                btnStart.Text = "开始自动演奏";
                lblStatus.Text = $"已找到原神进程，未开始自动演奏";
                lblStatus.ForeColor = Color.Black;
                FindGenshinProcess();
                return;
            }
            else
            {
                working = true;
                lblStatus.Text = $"已开始自动演奏";
                lblStatus.ForeColor = Color.Green;
                btnStart.Text = "停止自动演奏";
            }
            Task.Run(() =>
            {
                Dictionary<Keys, DateTime> dicKeysNextPressTime = new Dictionary<Keys, DateTime>();

                int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * DpiScaleX);
                int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);
                Size size = new Size(width, height);

                bool wHolding = false, sHolding = false, aHolding = false, dHolding = false, iHolding = false, kHolding = false, jHolding = false, lHolding = false;

                while (working)
                {
                    Bitmap memoryImage = new Bitmap(width, height);
                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);

                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.253125, 0.37685, Keys.W, 0.2526, 0.461, ref wHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.253125, 0.823148, Keys.S, 0.253125, 0.773, ref sHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.127083, 0.6, Keys.A, 0.165625, 0.616, ref aHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.378125, 0.6, Keys.D, 0.340625, 0.616, ref dHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.74739583, 0.37685, Keys.I, 0.7475, 0.461, ref iHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.74739583, 0.823148, Keys.K, 0.7475, 0.773, ref kHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.62135416, 0.6, Keys.J, 0.659, 0.616, ref jHolding);
                    StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.872917, 0.6, Keys.L, 0.835, 0.616, ref lHolding);

                    memoryGraphics.Dispose();
                    memoryImage.Dispose();
                }
            });
        }

        private void btnDebugScreenshot_Click(object sender, EventArgs e) => SaveDebugImage();

        private void StartAutoPressKeyAndRecordCD(Dictionary<Keys, DateTime> dicKeysNextPressTime, Bitmap bmp, double scaleX, double scaleY, Keys key, double scaleHoldX, double scaleHoldY, ref bool holding)
        {
            if (dicKeysNextPressTime.ContainsKey(key))
            {
                if (dicKeysNextPressTime[key] < DateTime.Now || holding)
                    dicKeysNextPressTime[key] = StartAutoPressKey(bmp, scaleX, scaleY, key, scaleHoldX, scaleHoldY, ref holding);
            }
            else
                dicKeysNextPressTime.Add(key, StartAutoPressKey(bmp, scaleX, scaleY, key, scaleHoldX, scaleHoldY, ref holding));
        }

        private void SaveDebugImage()
        {
            int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * DpiScaleX);
            int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);
            Size s = new Size(width, height);
            Bitmap memoryImage = new Bitmap(width, height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

            #region -- 点按判定位置 --
            //DrawDebugLine(0.253125, 0.37685, memoryImage);
            //DrawDebugLine(0.253125, 0.823148, memoryImage);
            //DrawDebugLine(0.127083, 0.6, memoryImage);
            //DrawDebugLine(0.378125, 0.6, memoryImage);
            //DrawDebugLine(0.74739583, 0.37685, memoryImage);
            //DrawDebugLine(0.74739583, 0.823148, memoryImage);
            //DrawDebugLine(0.62135416, 0.6, memoryImage);
            //DrawDebugLine(0.872917, 0.6, memoryImage);
            #endregion -- 点按判定位置 --

            #region -- 长按判定位置 --
            //DrawDebugLine(0.2526, 0.461 - 0.15, memoryImage);
            //DrawDebugLine(0.165625, 0.616 - 0.15, memoryImage);
            //DrawDebugLine(0.253125, 0.773 - 0.15, memoryImage);
            //DrawDebugLine(0.340625, 0.616 - 0.15, memoryImage);
            //DrawDebugLine(0.7475, 0.461 - 0.15, memoryImage);
            //DrawDebugLine(0.659, 0.616 - 0.15, memoryImage);
            //DrawDebugLine(0.7475, 0.773 - 0.15, memoryImage);
            //DrawDebugLine(0.835, 0.616 - 0.15, memoryImage);
            #endregion -- 长按判定位置 --

            memoryImage.Save(Application.StartupPath + @"\Screenshot.jpg");
        }

        private void DrawDebugLine(double scaleX, double scaleY, Bitmap highDpiScreenshot)
        {
            int xw = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
            int yw = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;
            for (int i = 0; i < highDpiScreenshot.Width; i++)
                highDpiScreenshot.SetPixel(i, yw, Color.Red);
            for (int i = 0; i < highDpiScreenshot.Height; i++)
                highDpiScreenshot.SetPixel(xw, i, Color.Red);
        }

        private DateTime StartAutoPressKey(Bitmap bmp, double scaleX, double scaleY, Keys key, double scaleHoldX, double scaleHoldY, ref bool holding)
        {
            byte byteKey = (byte)key;
            byte code = key switch
            {
                Keys.W => Convert.ToByte(0x11),
                Keys.S => Convert.ToByte(0x1F),
                Keys.A => Convert.ToByte(0x1E),
                Keys.D => Convert.ToByte(0x20),
                Keys.I => Convert.ToByte(0x17),
                Keys.K => Convert.ToByte(0x25),
                Keys.J => Convert.ToByte(0x24),
                Keys.L => Convert.ToByte(0x26),
                _ => throw new Exception()
            };

            if (holding)
            {
                int xHold = (int)Math.Round(genshinWindowWdith * scaleHoldX) + genshinWindowX;
                int yHold = (int)Math.Round(genshinWindowHeight * scaleHoldY) + genshinWindowY;
                int yTail = (int)Math.Round(genshinWindowHeight * (scaleHoldY - 0.15)) + genshinWindowY;

                var colorHold = bmp.GetPixel(xHold, yHold);
                var colorTail = bmp.GetPixel(xHold, yTail);

                if (colorHold.R > 240 && colorHold.G > 220 && colorHold.B > 210 && colorTail.B < 250 )
                {
                    keybd_event(byteKey, code, 2, 0);
                    holding = false;
                    if (debugTextBox.Visible)
                        BeginInvoke(new Action(() => debugTextBox.AppendText($"弹起按键:{key} ----{DateTime.Now}\r\n")));
                    return DateTime.Now;
                }
            }
            else
            {
                int x = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
                int y = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;

                Color color = bmp.GetPixel(x, y);
                if (color.R > 240 && color.G > 210 && color.G < 240 && color.B > 50 && color.B < 80)
                {
                    keybd_event(byteKey, code, 0, 0);
                    if (debugTextBox.Visible)
                        BeginInvoke(new Action(() => debugTextBox.AppendText($"点按按键{key}----{DateTime.Now}\r\n")));
                    Task.Delay(50).ContinueWith(_ => keybd_event(byteKey, code, 2, 0));
                    return DateTime.Now.AddMilliseconds(200);
                }

                int xHold = (int)Math.Round(genshinWindowWdith * scaleHoldX) + genshinWindowX;
                int yHold = (int)Math.Round(genshinWindowHeight * scaleHoldY) + genshinWindowY;
                int yTail = (int)Math.Round(genshinWindowHeight * (scaleHoldY - 0.15)) + genshinWindowY;

                var colorHold = bmp.GetPixel(xHold, yHold);
                var colorTail = bmp.GetPixel(xHold, yTail);

                if (colorHold.R > 240 && colorHold.G > 240 && colorHold.B > 230 && colorTail.B == 255)
                {
                    keybd_event(byteKey, code, 1, 0);
                    holding = true;
                    if (debugTextBox.Visible)
                        BeginInvoke(new Action(() => debugTextBox.AppendText($"按住按键:{key} ----{DateTime.Now}\r\n")));
                    return DateTime.Now.AddMinutes(500);
                }
            }
            return DateTime.Now;
        }
    }
}
