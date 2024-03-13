using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CosmosTTF;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public static class CloudFontManager
    {
        [ManifestResourceStream(ResourceName = "CloudOSLib.SystemFonts.ARIAL.TTF")]
        public static byte[] ArialFont;

        [ManifestResourceStream(ResourceName = "CloudOSLib.SystemFonts.NotoSansKR.ttf")]
        public static byte[] NotoSans_KR;

        public static Dictionary<string, TTFFont> ttfFonts;
        static CGSSurface surface;
        static Canvas canvas;

        public static void Load(Canvas c)
        {
            ttfFonts = new()
            {
                { "Arial", new(ArialFont) },
                { "fallback_font_", new(ArialFont) },
                { "Noto Sans KR", new(NotoSans_KR) }
            };
            surface = new CGSSurface(c);
            canvas = c;
        }

        /// <summary>
        /// Direct call of WriteString. if possible, use WriteString instead.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="str"></param>
        /// <param name="fontname"></param>
        /// <param name="px"></param>
        /// <param name="pos"></param>
        public static void DrawStringTTF(Color color, string str, string fontname, int px, Point pos)
        {
            TTFFont ttffont;
            if (ttfFonts.ContainsKey(fontname))
            {
                ttffont = ttfFonts[fontname];
            }
            else
            {
                ttffont = ttfFonts["fallback_font_"];
            }

            ttffont.DrawToSurface(surface, px, pos.X, (int)(pos.Y + px / 1.5), str, color);
        }

        /// <summary>
        /// Writes text.
        /// </summary>
        /// <param name="color">Color of text</param>
        /// <param name="str">Text</param>
        /// <param name="font">Font</param>
        /// <param name="pos">Position of Text</param>
        public static void WriteText(Color color, string str, CloudOSFont font, Point pos)
        {
            DrawStringTTF(color, str, font.Name, (int)font.Size, pos);
        }

        /// <summary>
        /// Writes text with Maxmium size.
        /// </summary>
        /// <param name="color">Color of text</param>
        /// <param name="str">Text</param>
        /// <param name="font">Font</param>
        /// <param name="pos">Position of Text</param>
        /// <param name="maxSize">Maxmium size of Text</param>
        public static void WriteText(Color color, string str, CloudOSFont font, Point pos, Size maxSize)
        {
            DrawStringTTFChecked(color, str, font.Name, (int)font.Size, pos, maxSize.Width, maxSize.Height);
        }

        /// <summary>
        /// Direct call of WriteString with Maxmium size. if possible, use WriteString instead.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="str"></param>
        /// <param name="fontname"></param>
        /// <param name="px"></param>
        /// <param name="pos"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        public static void DrawStringTTFChecked(Color color, string str, string fontname, int px, Point pos, int maxWidth = int.MaxValue, int maxHeight = int.MaxValue)
        {
            TTFFont ttffont;
            if (ttfFonts.ContainsKey(fontname))
            {
                ttffont = ttfFonts[fontname];
            }
            else
            {
                ttffont = ttfFonts["fallback_font_"];
            }

            int x = pos.X;
            int y = pos.Y + px;

            int dX = 0;
            int dY = 0;
            foreach (Rune rune in str)
            {
                if (rune.ToString()[0] == '\n')
                {
                    dX = 0;
                    dY += px;
                } 
                else
                {
                    var result = ttffont.RenderGlyphAsBitmap(rune, color, px);
                    if (result.HasValue)
                    {
                        canvas.DrawImageAlpha(result.Value.bmp, x + dX, y + dY + result.Value.offY);
                        dX += result.Value.offX;
                    }
                }

                if (dX >= maxWidth)
                {
                    dX = 0;
                    dY += px;
                }

                if (dY >= maxHeight)
                {
                    return;
                }
            }
        }

        public static int GetTTFWidth(this string String, string name, float size)
        {
            TTFFont ttffont;
            if (ttfFonts.ContainsKey(name))
            {
                ttffont = ttfFonts[name];
            }
            else
            {
                ttffont = ttfFonts["fallback_font_"];
            }
            return ttffont.CalculateWidth(String, size);
        }
    }
}
