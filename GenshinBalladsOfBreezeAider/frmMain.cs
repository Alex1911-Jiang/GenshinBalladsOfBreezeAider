using System;
using System.Drawing;
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
        public static extern bool GetClientRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32")]
        public static extern int GetSystemMetrics(int nIndex);

        private IntPtr hwndGenshin = IntPtr.Zero;

        private int genshinWindowX = 0;
        private int genshinWindowY = 0;
        private int genshinWindowWdith = 0;
        private int genshinWindowHeight = 0;
        private bool working = false;

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
                            if (windowRect.X < -16000)  //全屏
                            {
                                genshinWindowX = 0;
                                genshinWindowY = 0;
                                genshinWindowWdith = Screen.PrimaryScreen.Bounds.Width;
                                genshinWindowHeight = Screen.PrimaryScreen.Bounds.Height;

                                lblWindowType.Text = "全屏";
                                lblWindowLocation.Text = $"(0,0)";
                                lblWindowSize.Text = $"{Screen.PrimaryScreen.Bounds.Width}×{Screen.PrimaryScreen.Bounds.Height}";
                            }
                            else  //窗口化
                            {
                                Rectangle tempRect = new Rectangle(windowRect.X, windowRect.Y, windowRect.Width - windowRect.X, windowRect.Height - windowRect.Y);
                                Rectangle rect = new Rectangle(tempRect.X + (tempRect.Width - clientRect.Width) - 3, tempRect.Y + (tempRect.Height - clientRect.Height) - 3, clientRect.Width, clientRect.Height);

                                lblWindowType.Text = "窗口化";
                                lblWindowLocation.Text = $"({rect.X},{rect.Y})";
                                lblWindowSize.Text = $"{rect.Width}×{rect.Height}";

                                genshinWindowX = rect.X;
                                genshinWindowY = rect.Y;
                                genshinWindowWdith = rect.Width;
                                genshinWindowHeight = rect.Height;
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
                while (working)
                {
                    Size s = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Bitmap memoryImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
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
            PressKey(ref getReady, bmp, x, y, (byte)key);

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

        private void PressKey(ref bool getReady, Bitmap bmp, int x, int y, byte key)
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
                    //getReady = false;  //连击不会显示黑色圈导致判定有问题, 现只用作判断是否开始音游
                    keybd_event(key, 0, 0, 0);
                    Task.Delay(50).ContinueWith(_ =>
                    {
                        keybd_event(key, 0, 2, 0);
                    });
                }
            }
        }
    }
}
