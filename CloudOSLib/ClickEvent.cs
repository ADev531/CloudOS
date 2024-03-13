using CloudOSLib.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class ClickEvent
    {
        public int X = 0;
        public int Y = 0;
        public MouseButtonState ButtonState;

        public ClickEvent(int x, int y, MouseButtonState buttonState)
        {
            X = x;
            Y = y;
            ButtonState = buttonState;
        }
    }
}
