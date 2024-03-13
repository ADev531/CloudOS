using Cosmos.System;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib.Controls
{
    public enum BorderStyle
    {
        None,
        Line
    }

    public enum MouseButtonState
    {
        None,
        WheelUp,
        WheelDown,
        LeftClick,
        MiddleClick,
        RightClick
    }

    public abstract class Control
    {
        /// <summary>
        /// Gets or sets text of this control.
        /// </summary>
        public string Text;

        /// <summary>
        /// Gets or sets this control is visible.
        /// </summary>
        public bool Visible { get; }

        /// <summary>
        /// Position/Location of this control.
        /// </summary>
        public Point Position;

        protected Size _size;

        /// <summary>
        /// Size of this control.
        /// </summary>
        public Size Size
        {
            get
            {
                if (AutoSizing)
                {
                    return _getAutoSize();
                } else
                {
                    return _size;
                }
            }

            set
            {
                if (!AutoSizing)
                {
                    _size = value;
                }
            }
        }

        /// <summary>
        /// Child controls of this control.
        /// </summary>
        public List<Control> Controls;

        /// <summary>
        /// Font of this control.
        /// </summary>
        public CloudOSFont Font = new(CloudOSWindowSystem.SystemFont, 12);

        /// <summary>
        /// Color of this control.
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// Background color of this control.
        /// </summary>
        public Color BackgroundColor = Color.Transparent;

        /// <summary>
        /// Gets this control is focused.
        /// </summary>
        public bool IsFocused { get; protected set; } = false;

        /// <summary>
        /// Border style of this control.
        /// </summary>
        public BorderStyle BorderStyle = BorderStyle.None;

        /// <summary>
        /// Gets or Sets this control is auto sizing.
        /// </summary>
        public bool AutoSizing = false;

        /// <summary>
        /// Mouse down event.
        /// </summary>
        public CloudEventHandle<object?> MouseDown = new();

        bool mouseDown = false;

        protected abstract Size _getAutoSize();

        public Control()
        {
            Text = "Control";
            Visible = true;
            Position = new();
            _size = new();
            Controls = new List<Control>();
        }

        protected abstract void _onupdateinner(int x, int y, Canvas c);
        protected virtual void _beforeupdate()
        {
            // do nothing.
        }

        /// <summary>
        /// Event of clicking.
        /// </summary>
        /// <param name="x">Mouse X.</param>
        /// <param name="y">Mouse Y.</param>
        /// <param name="state">Mouse Button State.</param>
        public void Click(int x, int y, MouseButtonState state)
        {
            _click(x, y, state);
        }
        
        protected virtual void _click(int x, int y, MouseButtonState state)
        {
            // do nothing
        }

        /// <summary>
        /// Remove focus of this control.
        /// </summary>
        public void Unfocus()
        {
            IsFocused = false;
        }

        /// <summary>
        /// Runs KeyDown Event.
        /// </summary>
        /// <param name="keyevent">Key.</param>
        public void KeyDown(KeyEvent keyevent)
        {
            foreach (Control control in Controls)
            {
                control.KeyDown(keyevent);
            }
            _innerkeydown(keyevent);
        } 

        public virtual void _innerkeydown(KeyEvent key)
        {
            // do nothing
        }

        /// <summary>
        /// Redraws control.
        /// </summary>
        /// <param name="x">Offset X.</param>
        /// <param name="y">Offset Y.</param>
        /// <param name="canvas">Canvas object.</param>
        /// <param name="btn">Mouse button state.</param>
        public void Update(Canvas canvas, MouseButtonState btn, int x = 0, int y = 0)
        {
            _beforeupdate();
            int X = x + Position.X;
            int Y = y + Position.Y;

            if (Visible)
            {
                if (BackgroundColor != Color.Transparent)
                {
                    canvas.DrawFilledRectangle(BackgroundColor, X, Y, Size.Width, Size.Height);
                }

                switch (BorderStyle)
                {
                    case BorderStyle.Line:
                        canvas.DrawRectangle(Color.Black, X, Y, Size.Width, Size.Height);
                        break;
                }

                Focus focus = ClickManager.CheckClick(X, Y, Size.Width, Size.Height, btn);

                if (focus == Focus.Focus)
                {
                    mouseDown = true;
                    IsFocused = true;
                } 
                else if (focus == Focus.Unfocus)
                {
                    Unfocus();
                }
                else if (focus == Focus.None && mouseDown)
                {
                    mouseDown = false;
                    Click((int)MouseManager.X, (int)MouseManager.Y, btn);
                }

                _onupdateinner(X, Y, canvas);

                foreach (Control c in Controls)
                {
                    c.Update(canvas, btn, X, Y);
                }
            }
        }
    }
}
