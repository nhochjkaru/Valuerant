using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Valuerant
{
    public partial class Main : Form
    {
        DateTime startdt;
        UserActivityHook actHook;
        private static bool shoot_on = true;
        private static int gunmod = 0;

        bool isRunning = false;
        bool isDone = false;

        int fovX = 100;
        int fovY = 100;
        bool showFOV = false;

        int monitor = 0;
        float zoom = 1;
        int xSize;
        int ySize;
        int colorVariation = 25;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        System.DateTime lastShot = System.DateTime.Now;
        int msShootTime = 225;
        public Main()
        {
            InitializeComponent();
            startdt = DateTime.Now;
            actHook = new UserActivityHook();
         
            actHook.KeyUp += new KeyEventHandler(MyKeyUp);
            this.Text = Utils.RandomString(16);
            this.Name = Utils.RandomString(16);
            this.ShowIcon = false;
        }

        Thread mainThread;
        private void Main_load(object sender, EventArgs e)
        {
            mainThread = new Thread(() => {
                Update();
            });
            mainThread.Start();
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
           
                mainThread.Abort();
            
            
            base.OnHandleDestroyed(e);
        }
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        [DllImport("USER32.dll")]
        static extern short GetKeyState(int nVirtKey);
        private const UInt16 VK_LBUTTON = 0x01;//left mouse button
   
        async void Update()
        {
            while (true)
            {
                
                if (!isRunning)
                {
                    if(DateTime.Now > startdt.AddDays(2))
                    {
                            System.Windows.Forms.Application.Exit();
                    }
                  
                }
                var mainColor = Color.FromArgb(0xffff82);//0x9A0000 red // purple 0xA224A2 // purple 2 0xaf2eaf// vang mu sac do 0xffff82


                if (true)
                {
                    
                    const int triggerFovX = 13;//15 test ok voi gia tri nay
                    const int triggerFovY = 15;//20 ok
                    var l = PixelSearch(new Rectangle((xSize - triggerFovX) / 2, (ySize - triggerFovY) / 2, triggerFovX, triggerFovY), mainColor, colorVariation, false);


                    if (l.Length > 0)
                    {
                        if (System.DateTime.Now.Subtract(lastShot).TotalMilliseconds < msShootTime)
                        {
                            continue;
                        }
                        else
                        {
                            lastShot = System.DateTime.Now;
                        }
                        shoot_on = Control.IsKeyLocked(Keys.NumLock);
                        //shoot_on = true;


                        if ((gunmod == 0 && shoot_on))
                        {
                            int res = GetKeyState(VK_LBUTTON);

                            if (res < 0)
                            {
                                mouse_event(true ? (MOUSEEVENTF_LEFTDOWN) : 0x0001, 0, 0, 0, UIntPtr.Zero);
                                //Thread.Sleep(100);
                                //mouse_event(true ? (MOUSEEVENTF_LEFTDOWN) : 0x0001, 0, 0, 0, UIntPtr.Zero);
                            }
                            else
                            {
                                mouse_event(true ? (MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP) : 0x0001, 0, 0, 0, UIntPtr.Zero);
                            }

                            //Thread.Sleep(300);
                            // mouse_event(true ? (MOUSEEVENTF_LEFTUP) : 0x0001, 0, 0, 0, UIntPtr.Zero);
                            continue;

                        }

                        if (gunmod == 1 && shoot_on)
                        {
                            var z = PixelSearch(new Rectangle(xSize - 10, (ySize - triggerFovY) / 2, 2, 2), Color.FromArgb(14, 14, 14), 40, true);

                            if (z.Length > 0)
                            { // IF NOT ERROR
                                mouse_event(true ? (MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP) : 0x0001, 0, 0, 0, UIntPtr.Zero);
                                continue;
                            }

                        }
                    }
                }
              else//aimbot // ko work
                {

                    const int maxCount = 5;
                    const int size = 60;
                    const int closeSize = 10;

                    var l = PixelSearch(new Rectangle((xSize - fovX) / 2, (ySize - fovY) / 2, fovX, fovY), mainColor, colorVariation,false);
                    if (l.Length > 0)
                    {
                        try
                        {
                            bool pressDown = false;
                            /*if (isTriggerbot)
                            {
                                for (int i = 0; i < l.Length; i++)
                                {
                                    if ((new Vector2(l[i].X, l[i].Y) - new Vector2(xSize / 2, ySize / 2)).Length() < closeSize)
                                    {
                                        pressDown = true;
                                        break;
                                    }
                                }
                            }*/

                            var q = l.OrderBy(t => t.Y).ToArray();

                            List<Vector2> forbidden = new List<Vector2>();

                            for (int i = 0; i < q.Length; i++)
                            {
                                Vector2 current = new Vector2(q[i].X, q[i].Y);
                                if (forbidden.Where(t => (t - current).Length() < size || Math.Abs(t.X - current.X) < size).Count() < 1)
                                { // TO NOT PLACE POINTS AT THE BODY
                                    forbidden.Add(current);
                                    if (forbidden.Count > maxCount)
                                    {
                                        break;
                                    }
                                }
                            }

                            var closes = forbidden.Select(t => (t - new Vector2(xSize / 2, ySize / 2))).OrderBy(t => t.Length()).ElementAt(0) + new Vector2(1, offsetY);

                            Move((int)(closes.X * (float)speed), (int)(closes.Y * (float)speed), pressDown);
                        }
                        catch (Exception _ex)
                        {
                            Console.WriteLine("Main Ex." + _ex);
                        }
                    }

                }
                Thread.Sleep(10);
            }
        }
        decimal speed = 1;
        int offsetY = 10;
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        public void Move(int xDelta, int yDelta, bool pressDown = false)
        {

            mouse_event((uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),xDelta, yDelta, 0, UIntPtr.Zero);

            //Thread.Sleep(2000);
        }
        public Point[] PixelSearch(Rectangle rect, Color Pixel_Color, int Shade_Variation, bool zoom)
        {
            ArrayList points = new ArrayList();
            Bitmap RegionIn_Bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);

            if (monitor >= Screen.AllScreens.Length)
            {
                monitor = 0;
                UpdateUI();
            }

            int xOffset = Screen.AllScreens[monitor].Bounds.Left;
            int yOffset = Screen.AllScreens[monitor].Bounds.Top;

            using (Graphics GFX = Graphics.FromImage(RegionIn_Bitmap))
            {
                GFX.CopyFromScreen(rect.X + xOffset, rect.Y + yOffset, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }
            //if (zoom) thu check open awm zoom
            //{
            //    RegionIn_Bitmap.Save(Application.StartupPath + "\\img.Bmp", ImageFormat.Bmp);
            //    Color pixel = RegionIn_Bitmap.GetPixel(1, 1);
            //    var a = 1;
            //}
            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);
                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                    points.Add(new Point(x + rect.X, y + rect.Y));
                    }
                }
            }
            RegionIn_Bitmap.Dispose();
            return (Point[])points.ToArray(typeof(Point));
        }
        private void MyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString().ToUpper() == "NUMPAD1")
            {
                gunmod = 0;
            }
            if (e.KeyData.ToString().ToUpper() == "NUMPAD2")
            {
                gunmod = 1;
            }
        }

        
        private void Start_click(object sender, EventArgs e)
        {
            msShootTime = (int)FireRateNum.Value;
            isRunning = !isRunning;
            
            UpdateUI();


            if (isRunning)
            {
                StartBtt.Text = "Stop";
            }
            else
            {
                StartBtt.Text = "Start";
            }


        }

        void UpdateUI()
        {
            StartBtt.Text = isRunning ? "Stop" : "Start";
            UpdateDisplayInformation();
        }
        void UpdateDisplayInformation()
        {
            zoom = GetScalingFactor();
            Screen current = CurrentScreen();
            bool prim = current.Primary;
            xSize = (int)(current.Bounds.Width * (prim ? zoom : 1));
            ySize = (int)(current.Bounds.Height * (prim ? zoom : 1));
        }
        
        public Screen CurrentScreen()
        {
            return Screen.AllScreens[monitor];
        }
        private float GetScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
        private void MonitorChanged(object sender, EventArgs e)
        {
            monitor++;
            if (monitor >= Screen.AllScreens.Length)
            {
                monitor = 0;
            }
            UpdateUI();
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }


    }
}
