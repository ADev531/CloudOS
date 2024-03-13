using CloudOSLib.Controls;
using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public enum Focus
    {
        None,
        Focus,
        Unfocus
    }

    public class ClickManager
    {
        public static Focus CheckClick(int X, int Y, int width, int height, MouseButtonState btn)
        {
            if (MouseManager.X >= X && MouseManager.X <= X + width && MouseManager.Y >= Y && MouseManager.Y <= Y + height && btn == MouseButtonState.LeftClick)
            {
                return Focus.Focus;
            }
            else
            {
                if (btn != MouseButtonState.None)
                {
                    return Focus.Unfocus;
                }
            }
            return Focus.None;
        }
    }
}
