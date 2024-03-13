using Cosmos.System;
using Cosmos.System.Graphics;
using CosmosTTF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib.Controls
{
    public class InputBox : Control
    {
        public InputBox() 
        {
            BorderStyle = BorderStyle.Line;
            BackgroundColor = Color.White;
            AutoSizing = false;
        }

        public override void _innerkeydown(KeyEvent keyevent)
        {
            if (IsFocused)
            {
                if (keyevent.Key == ConsoleKeyEx.Backspace)
                {
                    if (Text.Length != 0)
                    {
                        Text = Text.Remove(Text.Length - 1);
                    }
                } else
                {
                    Text += keyevent.KeyChar;
                }
            }
        }

        protected override void _beforeupdate()
        {
            if (Size.Height > Font.Size)
            {
                Size = new(Size.Width, (int)Font.Size);
            }
        }

        protected override Size _getAutoSize()
        {
            return new Size(Text.GetTTFWidth(Font.Name, Font.Size), (int)Font.Size);
        }

        protected override void _onupdateinner(int x, int y, Canvas c)
        {
            //Point pos = new(x + Position.X, y + Position.Y - (Font.Size + (Font.Size / 2)));
            Point pos = new(x + Position.X, y + Position.Y);
            if (AutoSizing)
            {
                CloudFontManager.DrawStringTTF(Color, Text, Font.Name, (int)Font.Size, pos);
            }
            else
            {
                CloudFontManager.DrawStringTTFChecked(Color, Text, Font.Name, (int)Font.Size, pos, Size.Width, (int)Font.Size);
            }
        }
    }
}
