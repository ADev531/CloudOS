using CloudOSLib;
using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Cosmos.Core.INTs;

namespace CloudOSPlugs
{
    [Plug(Target = typeof(Cosmos.Core.INTs))]
    public class INTs
    {
        public static void HandleException(uint EIP, string Description, string Name, ref IRQContext ctx, uint addr = 0)
        {
            const string xHex = "0123456789ABCDEF";

            string ctxinterrupt = "";
            ctxinterrupt = ctxinterrupt + xHex[(int)((ctx.Interrupt >> 4) & 0xF)];
            ctxinterrupt = ctxinterrupt + xHex[(int)(ctx.Interrupt & 0xF)];

            string lastsknowaddress = "";

            if (addr != 0)
            {
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 28) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 24) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 20) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 16) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 12) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 8) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)((addr >> 4) & 0xF)];
                lastsknowaddress = lastsknowaddress + xHex[(int)(addr & 0xF)];
            }

            int line = 0;

            CloudOSGraphics.canvas.Clear(Color.Black);
            CloudOSGraphics.canvas.DrawString("CloudOS System Error", PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.DrawString("-- Start of Error Report", PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.DrawString("Error name : " + Name, PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.DrawString("Error description : " + Description, PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.Display();
            Heap.Collect();
            if (lastsknowaddress != "")
            {
                CloudOSGraphics.canvas.DrawString("Last known address : " + lastsknowaddress, PCScreenFont.Default, Color.White, 0, 64);
            }
            CloudOSGraphics.canvas.Display();
            CloudOSGraphics.canvas.DrawString("-- End of Error Report", PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.DrawString("System Halted. Press any key to restart.", PCScreenFont.Default, Color.White, 0, line++ * 16);
            CloudOSGraphics.canvas.Display();
            while (!KeyboardManager.KeyAvailable) ;
            CPU.Reboot();
        }
    }
}
