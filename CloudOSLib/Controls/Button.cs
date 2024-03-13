using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib.Controls
{
    public class Button : Control
    {
        /// <summary>
        /// Event of clicking.
        /// </summary>
        public CloudEventHandle<ClickEvent> OnClick = new();

        public Button()
        {
            BorderStyle = BorderStyle.Line;
            BackgroundColor = Color.White;
            AutoSizing = false;
        }

        protected override void _click(int x, int y, MouseButtonState state)
        {
            OnClick.InvokeAll(new(x, y, state));
        }

        protected override Size _getAutoSize()
        {
            return new Size(Text.GetTTFWidth(Font.Name, Font.Size), (int)Font.Size);
        }

        protected override void _onupdateinner(int x, int y, Canvas c)
        {
            Point pos = new(x + Position.X, y + Position.Y);
            if (AutoSizing)
            {
                CloudFontManager.DrawStringTTF(Color, Text, Font.Name, (int)Font.Size, pos);
            }
            else
            {
                CloudFontManager.DrawStringTTFChecked(Color, Text, Font.Name, (int)Font.Size, pos, Size.Width);
            }
        }
    }
}
