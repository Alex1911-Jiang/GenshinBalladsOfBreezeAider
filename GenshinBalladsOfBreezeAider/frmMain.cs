using System;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinBalladsOfBreezeAider
{
    public partial class frmMain : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32")]
        private static extern int GetSystemMetrics(int nIndex);


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

        private int genshinWindowX = 0;
        private int genshinWindowY = 0;
        private int genshinWindowWdith = 0;
        private int genshinWindowHeight = 0;
        private bool working = false;

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
            FindGenshinProcess();
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

                            float dpiX = DpiScaleX;
                            float dpiY = DpiScaleY;

                            if (windowRect.X < -16000)  //全屏
                            {
                                genshinWindowX = 0;
                                genshinWindowY = 0;
                                genshinWindowWdith = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * dpiX);
                                genshinWindowHeight = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * DpiScaleY);

                                lblWindowType.Text = "全屏";
                                lblWindowLocation.Text = $"(0,0)";
                                lblWindowSize.Text = $"{genshinWindowWdith}×{genshinWindowHeight}";
                            }
                            else  //窗口化
                            {
                                Rectangle tempRect = new Rectangle(windowRect.X, windowRect.Y, windowRect.Width - windowRect.X, windowRect.Height - windowRect.Y);
                                Rectangle rect = new Rectangle(tempRect.X + (tempRect.Width - clientRect.Width) - 3, tempRect.Y + (tempRect.Height - clientRect.Height) - 3, clientRect.Width, clientRect.Height);

                                if (dpiX > 1 || DpiScaleY > 1)
                                {
                                    rect = new Rectangle((int)Math.Round(rect.X * dpiX), (int)Math.Round(rect.Y * dpiY), (int)Math.Round(rect.Width * dpiX), (int)Math.Round(rect.Height * dpiY));
                                }

                                genshinWindowX = rect.X;
                                genshinWindowY = rect.Y;
                                genshinWindowWdith = rect.Width;
                                genshinWindowHeight = rect.Height;

                                lblWindowType.Text = "窗口化";
                                lblWindowLocation.Text = $"({genshinWindowX},{genshinWindowY})";
                                lblWindowSize.Text = $"{genshinWindowHeight}×{genshinWindowHeight}";

                            }
                            btnStart.Enabled = true;
                        }
                        catch
                        {
                        }
                    }));
                }
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
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
                float dpiX = DpiScaleX;
                float dpiY = DpiScaleY;

                while (working)
                {
                    int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * dpiX);
                    int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * dpiY);
                    Size s = new Size(width, height);
                    Bitmap memoryImage = new Bitmap(width, height);
                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

                    StartAutoPressKey(memoryImage, 0.253125, 0.37685, Keys.W);
                    StartAutoPressKey(memoryImage, 0.253125, 0.823148, Keys.S);
                    StartAutoPressKey(memoryImage, 0.127083, 0.6, Keys.A);
                    StartAutoPressKey(memoryImage, 0.378125, 0.6, Keys.D);
                    StartAutoPressKey(memoryImage, 0.74739583, 0.37685, Keys.I);
                    StartAutoPressKey(memoryImage, 0.74739583, 0.823148, Keys.K);
                    StartAutoPressKey(memoryImage, 0.62135416, 0.6, Keys.J);
                    StartAutoPressKey(memoryImage, 0.872917, 0.6, Keys.L);

                    memoryGraphics.Dispose();
                    memoryImage.Dispose();
                }
            });
        }

        bool wReady, sReady, aReady, dReady, iReady, kReady, jReady, lReady;

        private void SaveDebugImage()
        {
            float dpiX = DpiScaleX;
            float dpiY = DpiScaleY;

            int width = (int)Math.Round(Screen.PrimaryScreen.Bounds.Width * dpiX);
            int height = (int)Math.Round(Screen.PrimaryScreen.Bounds.Height * dpiY);
            Size s = new Size(width, height);
            Bitmap memoryImage = new Bitmap(width, height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

            DrawDebugLine(0.253125, 0.37685, memoryImage);
            DrawDebugLine(0.253125, 0.823148, memoryImage);
            DrawDebugLine(0.127083, 0.6, memoryImage);
            DrawDebugLine(0.378125, 0.6, memoryImage);
            DrawDebugLine(0.74739583, 0.37685, memoryImage);
            DrawDebugLine(0.74739583, 0.823148, memoryImage);
            DrawDebugLine(0.62135416, 0.6, memoryImage);
            DrawDebugLine(0.872917, 0.6, memoryImage);

            memoryImage.Save(Application.StartupPath + @"\text.jpg");
        }

        private void DrawDebugLine(double scaleX, double scaleY, Bitmap highDpiScreenshot)
        {
            int xw = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
            int yw = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;
            for (int i = 0; i < highDpiScreenshot.Width; i++)
            {
                highDpiScreenshot.SetPixel(i, yw, Color.Red);
            }
            for (int i = 0; i < highDpiScreenshot.Height; i++)
            {
                highDpiScreenshot.SetPixel(xw, i, Color.Red);
            }
        }

        private void StartAutoPressKey(Bitmap bmp, double scaleX, double scaleY, Keys key)
        {
            bool getReady = key switch
            {
                Keys.W => wReady,
                Keys.S => sReady,
                Keys.A => aReady,
                Keys.D => dReady,
                Keys.I => iReady,
                Keys.K => kReady,
                Keys.J => jReady,
                Keys.L => lReady,
                _ => throw new Exception()
            };

            int x = (int)Math.Round(genshinWindowWdith * scaleX) + genshinWindowX;
            int y = (int)Math.Round(genshinWindowHeight * scaleY) + genshinWindowY;
            PressKey(ref getReady, bmp, x, y, key);

            switch (key)
            {
                case Keys.W:
                    wReady = getReady;
                    break;
                case Keys.S:
                    sReady = getReady;
                    break;
                case Keys.A:
                    aReady = getReady;
                    break;
                case Keys.D:
                    dReady = getReady;
                    break;
                case Keys.I:
                    iReady = getReady;
                    break;
                case Keys.K:
                    kReady = getReady;
                    break;
                case Keys.J:
                    jReady = getReady;
                    break;
                case Keys.L:
                    lReady = getReady;
                    break;
            }
        }

        private byte GetScancode(Keys key) => key switch
        {
            Keys.W => Convert.ToByte("11"),
            Keys.S => Convert.ToByte("1F"),
            Keys.A => Convert.ToByte("1E"),
            Keys.D => Convert.ToByte("20"),
            Keys.I => Convert.ToByte("17"),
            Keys.K => Convert.ToByte("25"),
            Keys.J => Convert.ToByte("24"),
            Keys.L => Convert.ToByte("26"),
            _ => throw new Exception()
        };

        private void PressKey(ref bool getReady, Bitmap bmp, int x, int y, Keys key)
        {
            Color color = bmp.GetPixel(x, y);
            if (!getReady)
            {
                if (color.R < 100 && color.G < 100 && color.B < 100)
                {
                    getReady = true;
                }
            }
            if (getReady)
            {
                if (color.R == 255 && color.G > 170 && color.G < 230 && color.B > 60 && color.B < 70)
                {
                    byte byteKey = (byte)key;
                    //getReady = false;  //连击不会显示黑色圈导致判定有问题, 现只用作判断是否开始音游
                    //byte code = GetScancode(key);
                    keybd_event(byteKey, 0, 0, 0);
                    //WinIo.MykeyDown(byteKey);
                    //Invoke(new Action(() => debugTextBox.AppendText($"按下按键{((Keys)key).ToString()} ----{DateTime.Now}\r\n")));
                    Task.Delay(50).ContinueWith(_ =>
                    {
                        keybd_event(byteKey, 0, 2, 0);
                        //WinIo.MykeyUp(byteKey);
                    });
                }
            }
        }
    }
}
