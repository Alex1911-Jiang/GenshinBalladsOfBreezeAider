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
                btnStart.Text = "开始自动演奏(F10)";
                lblStatus.Text = $"已找到原神进程，未开始自动演奏";
                lblStatus.ForeColor = Color.Black;
                FindGenshinProcess();
                return;
            }
            else
            {
                debugIndex = 1;
                working = true;
                lblStatus.Text = $"已开始自动演奏";
                lblStatus.ForeColor = Color.Green;
                btnStart.Text = "停止自动演奏(F10)";
            }
            Task.Run(() =>
            {
                Dictionary<Keys, DateTime> dicKeysNextPressTime = new Dictionary<Keys, DateTime>();

                int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * DpiScaleX);
                int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);

                bool wHolding = false, sHolding = false, aHolding = false, dHolding = false, iHolding = false, kHolding = false, jHolding = false, lHolding = false;

                (double aTailX, double aTailY) = Rotate(376 +  190, 731, 376 , 731, -45);
                (double sTailX, double sTailY) = Rotate(573 + 190, 819, 573, 819, -60);
                (double dTailX, double dTailY) = Rotate(787  + 190, 870, 787 , 870, -75);
                (double jTailX, double jTailY) = Rotate(1133 + 190, 870, 1133, 870, -105);
                (double kTailX, double kTailY) = Rotate(1347 + 190, 819, 1347, 819, -120);
                (double lTailX, double lTailY) = Rotate(1545 + 190, 731, 1545, 731, -135);

                double aScaleX = 376 / 1920.0;
                double sScaleX = 573 / 1920.0;
                double dScaleX = 787 / 1920.0;
                double jScaleX = 1133 / 1920.0;
                double kScaleX = 1347 / 1920.0;
                double lScaleX = 1545 / 1920.0;
                double aScaleY = 731 / 1080.0;
                double sScaleY = 819 / 1080.0;
                double dScaleY = 870 / 1080.0;
                double jScaleY = 870 / 1080.0;
                double kScaleY = 819 / 1080.0;
                double lScaleY = 731 / 1080.0;

                double aScaleTailX = aTailX / 1920.0;
                double sScaleTailX = sTailX / 1920.0;
                double dScaleTailX = dTailX / 1920.0;
                double jScaleTailX = jTailX / 1920.0;
                double kScaleTailX = kTailX / 1920.0;
                double lScaleTailX = lTailX / 1920.0;

                double aScaleTailY = aTailY / 1080.0;
                double sScaleTailY = sTailY / 1080.0;
                double dScaleTailY = dTailY / 1080.0;
                double jScaleTailY = jTailY / 1080.0;
                double kScaleTailY = kTailY / 1080.0;
                double lScaleTailY = lTailY / 1080.0;

                while (working)
                {
                    Bitmap memoryImage = new Bitmap(width, height);
                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, new Size(width, height));

                    #region -- 镜花听世/风物之歌 --
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.253125, 0.37685, Keys.W, 0.2526, 0.461, ref wHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.253125, 0.823148, Keys.S, 0.253125, 0.773, ref sHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.127083, 0.6, Keys.A, 0.165625, 0.616, ref aHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.378125, 0.6, Keys.D, 0.340625, 0.616, ref dHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.74739583, 0.37685, Keys.I, 0.7475, 0.461, ref iHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.74739583, 0.823148, Keys.K, 0.7475, 0.773, ref kHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.62135416, 0.6, Keys.J, 0.659, 0.616, ref jHolding);
                    //StartAutoPressKeyAndRecordCD(dicKeysNextPressTime, memoryImage, 0.872917, 0.6, Keys.L, 0.835, 0.616, ref lHolding);
                    #endregion -- 镜花听世/风物之歌 --

                    #region -- 荒 泷 极 上 盛 世 豪 鼓 大 祭 典 --
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, aScaleX, aScaleY , aScaleTailX, aScaleTailY, Keys.A, ref aHolding);
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, sScaleX, sScaleY , sScaleTailX, sScaleTailY, Keys.S, ref sHolding);
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, dScaleX, dScaleY , dScaleTailX, dScaleTailY, Keys.D, ref dHolding);
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, jScaleX, jScaleY , jScaleTailX, jScaleTailY, Keys.J, ref jHolding);
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, kScaleX, kScaleY , kScaleTailX, kScaleTailY, Keys.K, ref kHolding);
                    StartAutoPressKeyAndRecordCD2(dicKeysNextPressTime, memoryImage, lScaleX, lScaleY , lScaleTailX, lScaleTailY, Keys.L, ref lHolding);
                    #endregion -- 荒 泷 极 上 盛 世 豪 鼓 大 祭 典 --

                    memoryGraphics.Dispose();
                    memoryImage.Dispose();
                }
            });
        }

        private void btnDebugScreenshot_Click(object sender, EventArgs e) => SaveDebugImage();

        [Obsolete]
        private void StartAutoPressKeyAndRecordCD(Dictionary<Keys, DateTime> dicKeysNextPressTime, Bitmap bmp, double scaleX, double scaleY, Keys key, double scaleHoldX, double scaleHoldY, ref bool holding)
        {
            if (dicKeysNextPressTime.ContainsKey(key))
            {
                if (dicKeysNextPressTime[key] < DateTime.Now)
                    dicKeysNextPressTime[key] = StartAutoPressKey(bmp, scaleX, scaleY, key, scaleHoldX, scaleHoldY, ref holding);
            }
            else
                dicKeysNextPressTime.Add(key, StartAutoPressKey(bmp, scaleX, scaleY, key, scaleHoldX, scaleHoldY, ref holding));
        }

        private void StartAutoPressKeyAndRecordCD2(Dictionary<Keys, DateTime> dicKeysNextPressTime, Bitmap bmp, double scaleX, double scaleY, double scaleTailX, double scaleTailY, Keys key, ref bool holding)
        {
            if (dicKeysNextPressTime.ContainsKey(key))
            {
                if (dicKeysNextPressTime[key] < DateTime.Now)
                    dicKeysNextPressTime[key] = StartAutoPressKey2(bmp, scaleX, scaleY, scaleTailX, scaleTailY, key, ref holding);
            }
            else
                dicKeysNextPressTime.Add(key, StartAutoPressKey2(bmp, scaleX, scaleY, scaleTailX, scaleTailY, key, ref holding));
        }

        private (double x, double y) Rotate(double targetX, double targetY, double centerX, double centerY, double angle)
        {
            double x = (targetX - centerX) * Math.Cos(Math.PI / 180.0 * angle) - (targetY - centerY) * Math.Sin(Math.PI / 180.0 * angle) + centerX;
            double y = (targetX - centerX) * Math.Sin(Math.PI / 180.0 * angle) + (targetY - centerY) * Math.Cos(Math.PI / 180.0 * angle) + centerY;
            return (x, y);
        }

        private int debugIndex = 1;
        private void SaveDebugImage()
        {
            int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * DpiScaleX);
            int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);
            Size size = new Size(width, height);
            Bitmap memoryImage = new Bitmap(width, height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, size);

            #region -- 镜花听世/风物之歌 --
            #region -- 点按判定位置 --
            //DrawDebugLine(0.253125, 0.37685, memoryImage, Keys.W);
            //DrawDebugLine(0.253125, 0.823148, memoryImage, Keys.S);
            //DrawDebugLine(0.127083, 0.6, memoryImage, Keys.A);
            //DrawDebugLine(0.378125, 0.6, memoryImage, Keys.D);
            //DrawDebugLine(0.74739583, 0.37685, memoryImage, Keys.I);
            //DrawDebugLine(0.74739583, 0.823148, memoryImage, Keys.J);
            //DrawDebugLine(0.62135416, 0.6, memoryImage, Keys.K);
            //DrawDebugLine(0.872917, 0.6, memoryImage, Keys.L);
            #endregion -- 点按判定位置 --

            #region -- 长按判定位置 --
            //DrawDebugLine(0.2526, 0.461 - 0.15, memoryImage, Keys.W);
            //DrawDebugLine(0.165625, 0.616 - 0.15, memoryImage, Keys.S);
            //DrawDebugLine(0.253125, 0.773 - 0.15, memoryImage, Keys.A);
            //DrawDebugLine(0.340625, 0.616 - 0.15, memoryImage, Keys.D);
            //DrawDebugLine(0.7475, 0.461 - 0.15, memoryImage, Keys.I);
            //DrawDebugLine(0.659, 0.616 - 0.15, memoryImage, Keys.J);
            //DrawDebugLine(0.7475, 0.773 - 0.15, memoryImage, Keys.K);
            //DrawDebugLine(0.835, 0.616 - 0.15, memoryImage, Keys.L);
            #endregion -- 长按判定位置 --
            #endregion -- 镜花听世/风物之歌 --

            #region -- 荒 泷 极 上 盛 世 豪 鼓 大 祭 典 --
            DrawDebugLine(376 / 1920.0, 731 / 1080.0, memoryImage, Keys.A);
            DrawDebugLine(573 / 1920.0, 819 / 1080.0, memoryImage, Keys.S);
            DrawDebugLine(787 / 1920.0, 870 / 1080.0, memoryImage, Keys.D);
            DrawDebugLine(1133 / 1920.0, 870 / 1080.0, memoryImage, Keys.J);
            DrawDebugLine(1347 / 1920.0, 819 / 1080.0, memoryImage, Keys.K);
            DrawDebugLine(1545 / 1920.0, 731 / 1080.0, memoryImage, Keys.L);

            //(double aX, double aY) = Rotate(376 + 190, 731, 376, 731, -45);
            //(double sX, double sY) = Rotate(573 + 190, 819, 573, 819, -60);
            //(double dX, double dY) = Rotate(787 + 190, 870, 787, 870, -75);
            //(double jX, double jY) = Rotate(1133 + 190, 870, 1133, 870, -105);
            //(double kX, double kY) = Rotate(1347 + 190, 819, 1347, 819, -120);
            //(double lX, double lY) = Rotate(1545 + 190, 731, 1545, 731, -135);

            //DrawDebugLine(aX / 1920.0,  aY / 1080.0, memoryImage, Keys.A);
            //DrawDebugLine(sX / 1920.0,  sY / 1080.0, memoryImage, Keys.S);
            //DrawDebugLine(dX / 1920.0,  dY / 1080.0, memoryImage, Keys.D);
            //DrawDebugLine(jX  / 1920.0, jY / 1080.0, memoryImage, Keys.J);
            //DrawDebugLine(kX  / 1920.0, kY / 1080.0, memoryImage, Keys.K);
            //DrawDebugLine(lX  / 1920.0, lY / 1080.0, memoryImage, Keys.L);
            #endregion -- 荒 泷 极 上 盛 世 豪 鼓 大 祭 典 --

            memoryImage.Save(Application.StartupPath + @$"\Screenshot{debugIndex++}.jpg");

            void DrawDebugLine(double scaleX, double scaleY, Bitmap highDpiScreenshot, Keys key)
            {
                int xw = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
                int yw = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;
                for (int i = xw - 25; i < xw + 25; i++)
                    highDpiScreenshot.SetPixel(i, yw, Color.Red);
                for (int i = yw - 25; i < yw + 25; i++)
                    highDpiScreenshot.SetPixel(xw, i, Color.Red);
                Graphics g = Graphics.FromImage(highDpiScreenshot);
                g.DrawString(key.ToString(), new Font("宋体", 12), Brushes.Red, xw, yw);
                g.Dispose();
            }
        }

        [Obsolete]
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
                //弹起
                int xHold = (int)Math.Round(genshinWindowWdith * scaleHoldX) + genshinWindowX;
                int yHold = (int)Math.Round(genshinWindowHeight * scaleHoldY) + genshinWindowY;
                int yTail = (int)Math.Round(genshinWindowHeight * (scaleHoldY - 0.15)) + genshinWindowY;

                var colorHold = bmp.GetPixel(xHold, yHold);
                var colorTail = bmp.GetPixel(xHold, yTail);

                if (colorHold.R > 240 && colorHold.G > 210 && colorHold.B > 210 && colorTail.B < 250)
                {
                    Task.Run(() => keybd_event(byteKey, code, 2, 0));
                    holding = false;
                    if (debugTextBox.Visible)
                        BeginInvoke(new Action(() => debugTextBox.AppendText($"弹起按键:{key} ----{DateTime.Now}\r\n")));
                    //SaveDebugImage()");
                    return DateTime.Now;
                }
            }
            else
            {
                //点按
                int x = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
                int y = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;

                Color color = bmp.GetPixel(x, y);
                if (color.R > 240 && color.G > 210 && color.G < 240 && color.B > 50 && color.B < 80)
                {
                    keybd_event(byteKey, code, 0, 0);
                    if (debugTextBox.Visible)
                        BeginInvoke(new Action(() => debugTextBox.AppendText($"点按按键{key}----{DateTime.Now}\r\n")));
                    Task.Delay(50).ContinueWith(_ => keybd_event(byteKey, code, 2, 0));
                    //SaveDebugImage();
                    return DateTime.Now.AddMilliseconds(200);
                }

                //长按
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
                    //SaveDebugImage();
                    return DateTime.Now.AddMilliseconds(500);
                }
            }
            return DateTime.Now;
        }


        private DateTime StartAutoPressKey2(Bitmap bmp, double scaleX, double scaleY, double scaleTailX, double scaleTailY, Keys key, ref bool holding)
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

            //点按
            int x = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
            int y = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;

            Color color = bmp.GetPixel(x, y);
            //if (holding)
            //{
            //    //弹起
            //    int xTail = (int)Math.Round(genshinWindowWdith * scaleTailX) + genshinWindowX;
            //    int yTail = (int)Math.Round(genshinWindowHeight * scaleTailY) + genshinWindowY;
            //    Color colorTail = bmp.GetPixel(xTail, yTail);
            //    if (colorTail.B > 245)
            //    {
            //        Task.Run(() => keybd_event(byteKey, code, 2, 0));
            //        holding = false;
            //        if (debugTextBox.Visible)
            //            BeginInvoke(new Action(() => debugTextBox.AppendText($"弹起按键:{key} ----{DateTime.Now}\r\n")));
            //        //SaveDebugImage();
            //        return DateTime.Now.AddMilliseconds(100);
            //    }
            //}
            //else 
            //if (color.R > 160 && color.G < 190 && color.B > 220)
            //{
            //    keybd_event(byteKey, code, 2, 0);
            //    Task.Run(() => keybd_event(byteKey, code, 1, 0));
            //    holding = true;

            //    if (debugTextBox.Visible)
            //        BeginInvoke(new Action(() => debugTextBox.AppendText($"按住按键:{key} ----{DateTime.Now}\r\n")));
            //    //SaveDebugImage();
            //    return DateTime.Now.AddMilliseconds(200);
            //}
            //else 
            if (color.B < 160)
            {
                keybd_event(byteKey, code, 0, 0);
                if (debugTextBox.Visible)
                    BeginInvoke(new Action(() => debugTextBox.AppendText($"点按按键{key}----{DateTime.Now}\r\n")));
                Task.Delay(20).ContinueWith(_ => keybd_event(byteKey, code, 2, 0));
                //SaveDebugImage();
                return DateTime.Now.AddMilliseconds(100);
            }
            return DateTime.Now;
        }
    }
}
