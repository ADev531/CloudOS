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
    public class TextLabel : Control
    {
        public TextLabel()
        {
            AutoSizing = true;
        }

        protected override Size _getAutoSize()
        {
            return new Size(Text.GetTTFWidth(Font.Name, Font.Size), (int)Font.Size);
        }

        protected override void _onupdateinner(int x, int y, Canvas c)
        {
            //Point pos = new(x + Position.X, y + Position.Y + Font.Size);
            Point pos = new(x + Position.X, y + Position.Y);
            if (AutoSizing)
            {
                CloudFontManager.DrawStringTTF(Color, Text, Font.Name, (int)Font.Size, pos);
            } else
            {
                CloudFontManager.DrawStringTTFChecked(Color, Text, Font.Name, (int)Font.Size, pos, Size.Width);
            }
        }
    }
}
