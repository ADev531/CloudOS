using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmosTTF;
using IL2CPU.API.Attribs;
using CloudOSLib.Controls;
using Cosmos.HAL;
using Global = Cosmos.HAL.Global;
using CloudOSLib.IMEs;

namespace CloudOSLib
{
    public class CloudOSWindow
    {
        public int x = 0;
        public int y = 0;
        public int dx = 0;
        public int dy = 0;
        public int width = 500;
        public int height = 250;
        public string text = "Window";
        public List<Control> controls = new();
        public bool Focus = false;
    }

    public class CloudOSWindowSystem
    {
        Canvas canvas;
        public List<CloudOSWindow> windows = new();
        public KernelClassDebugger windowSystemDebugger = new("CloudOS Window System");
        public bool enableCursor = false;
        public static string SystemFont = "Noto Sans KR";
        public bool MenuVisible = false;
        public bool MenuDelay = false;

        [ManifestResourceStream(ResourceName = "CloudOSLib.Resources.adevcloud.bmp")]
        public static byte[] ADevCloud;

        [ManifestResourceStream(ResourceName = "CloudOSLib.Resources.mousecursor.bmp")]
        public static byte[] Cursor;

        [ManifestResourceStream(ResourceName = "CloudOSLib.Resources.prebetabackground.bmp")]
        public static byte[] sys_background;

        [ManifestResourceStream(ResourceName = "CloudOSLib.Resources.glasstitlebar.bmp")]
        public static byte[] glass_titlebar_bytes;

        [ManifestResourceStream(ResourceName = "CloudOSLib.Resources.glassclosebtn.bmp")]
        public static byte[] glass_closebtn_bytes;

        Bitmap ADevCloudIcon = new(ADevCloud);
        Bitmap MouseCursor = new(Cursor);
        Bitmap Background = new(sys_background);
        Bitmap GlassTitleBar = new(glass_titlebar_bytes);
        Bitmap GlassCloseBtn = new(glass_closebtn_bytes);

        public bool EnableUI = true;

        public int Width {
            get
            {
                return (int)canvas.Mode.Width;
            } 
        }

        public int Height
        {
            get
            {
                return (int)canvas.Mode.Height;
            }
        }
        public CloudOSWindowSystem(Canvas c) 
        {
            windowSystemDebugger.Send("Starting CloudOS WindowSystem");
            canvas = c;

            CloudOSGraphics.canvas = canvas;
            
            MouseManager.ScreenWidth = (uint)Width;
            MouseManager.ScreenHeight = (uint)Height;
            MouseManager.X = MouseManager.ScreenWidth / 2;
            MouseManager.Y = MouseManager.ScreenHeight / 2;
            MouseManager.ResetScrollDelta();

            windowSystemDebugger.Send("Registering Processes");
            CloudOSProcess.CreateSystemProcess((CloudOSProcess process) =>
            {
                Update();
                process.Running = false;
            });

            CloudOSProcess.CreateSystemProcess((CloudOSProcess process) =>
            {
                if (MenuDelay == true)
                {
                    MenuDelay = false;
                }
                process.Running = false;
            });

            windowSystemDebugger.Send("Loading IME System");
            CloudOSIME.RegisterIME(new IMEHangul());
        }

        public void DrawWindow(CloudOSWindow window, MouseButtonState btn, KeyEvent? key)
        { 
            int y = window.y + 20;

            Focus titleBarFocus = ClickManager.CheckClick(window.x, window.y, window.width - 20, y, btn);

            if (titleBarFocus == Focus.Focus && window.Focus == false)
            {
                window.Focus = true;
                window.dx = (int)MouseManager.X - window.x;
                window.dy = (int)MouseManager.Y - window.y;
            }
            else if ((titleBarFocus == Focus.Unfocus || titleBarFocus == Focus.None) && window.Focus == true)
            {
                window.Focus = false;
            }

            if (window.Focus == true)
            {
                window.x = (int)MouseManager.X - window.dx;
                window.y = (int)MouseManager.Y - window.dy;
            }

            canvas.DrawRectangle(Color.Black, window.x - 1, y - 1, window.width + 2, window.height + 2);
            
            if (EnableUI)
            {
                for (int i = 0; i < window.width - 20; i++)
                {
                    canvas.DrawImageAlpha(GlassTitleBar, window.x + i, y);
                }
                Heap.Collect();
            } 
            else
            {
                canvas.DrawFilledRectangle(Color.Blue, window.x, y, window.width, 20);
            }

            if (EnableUI)
            {
                canvas.DrawImageAlpha(GlassCloseBtn, window.x + window.width - 20, y);
                Heap.Collect();
            } 
            else
            {
                canvas.DrawFilledRectangle(Color.Red, window.x + window.width - 20, y, 20, 20);
            }
            canvas.DrawString("X", PCScreenFont.Default, Color.White, window.x + window.width - 16, y + 4);
            CloudFontManager.DrawStringTTF(Color.White, window.text, SystemFont, 20, new Point(window.x, y));
            canvas.DrawFilledRectangle(Color.White, window.x, y + 20, window.width, window.height - 20);

            Focus closeBtnFocus = ClickManager.CheckClick(window.x + window.width - 20, y, 20, 20, btn);

            if (closeBtnFocus == Focus.Focus)
            {
                windows.Remove(window);
                return;
            }

            foreach (Control c in window.controls)
            {
                c.Update(canvas, btn, window.x, y + 20);
                if (key != null)
                {
                    c.KeyDown(key);
                }
            }
        }

        public void Update()
        {
            canvas.Clear(Color.FromArgb(160, 198, 255));

            canvas.DrawImage(Background, (int)(((int)canvas.Mode.Width / 2) - (Background.Width / 2)), (int)(((int)canvas.Mode.Height / 2) - (Background.Height / 2)));

            MouseButtonState btn = MouseButtonState.None;

            if (MouseManager.MouseState != MouseState.None)
            {
                switch (MouseManager.MouseState)
                {
                    case MouseState.Left:
                        btn = MouseButtonState.LeftClick;
                        break;
                    case MouseState.Right:
                        btn = MouseButtonState.RightClick;
                        break;
                    case MouseState.Middle:
                        btn = MouseButtonState.MiddleClick;
                        break;
                    default:
                        btn = MouseButtonState.None;
                        break;
                }
            }

            KeyEvent? key = null;

            if (KeyboardManager.KeyAvailable)
            {
                key = KeyboardManager.ReadKey();

                if (key.Modifiers == ConsoleModifiers.Control && key.KeyChar == ' ')
                {
                    CloudOSIME.SwitchIME();
                }

                key.KeyChar = CloudOSIME.GetKeyChar(key);
            }

            for (int i = windows.Count - 1; i >= 0; i--)
            {
                CloudOSWindow window = windows[i];
                DrawWindow(window, btn, key);
            }

            if (EnableUI)
            {
                Heap.Collect();
                // Taskbar
                canvas.DrawFilledRectangle(Color.Gray, 0, 0, (int)canvas.Mode.Width, 20);

                // Taskbar items
                CloudFontManager.DrawStringTTF(Color.White, CloudOSIME.GetShortNameofCurrentIME(), SystemFont, 15, new(50, 5));

                string Time = DateTime.Now.ToString("tt hh:mm ");

                CloudFontManager.DrawStringTTF(Color.White, Time, SystemFont, 15, new Point((int)canvas.Mode.Width - Time.GetTTFWidth(SystemFont, 15), 5));

                // Menu Button
                canvas.DrawFilledRectangle(Color.White, (int)canvas.Mode.Width / 2 - 20, 0, 20, 20);
                canvas.DrawImageAlpha(ADevCloudIcon, (int)canvas.Mode.Width / 2 - 20, 0);
                Focus _menubtnfocus = ClickManager.CheckClick((int)canvas.Mode.Width / 2 - 20, 0, 20, 20, btn);

                if (_menubtnfocus == Focus.Focus && !MenuDelay)
                {
                    MenuVisible = !MenuVisible;
                    MenuDelay = true;
                }

                // Menu

                if (MenuVisible)
                {
                    canvas.DrawFilledRectangle(Color.Gray, (int)canvas.Mode.Width / 2 - 30, 20, (int)canvas.Mode.Width - ((int)canvas.Mode.Width / 2 - 30), (int)canvas.Mode.Height / 2);
                }
            }

            if (enableCursor)
            {
                canvas.DrawImageAlpha(MouseCursor, (int)MouseManager.X, (int)MouseManager.Y);
            }

            Heap.Collect();
            canvas.Display();
        }
    }
}
