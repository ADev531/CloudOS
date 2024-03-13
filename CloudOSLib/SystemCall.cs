using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cosmos.Core.INTs;

namespace CloudOSLib
{
    public class SystemCall
    {
        CloudOSWindowSystem coswSys;
        public SystemCall(CloudOSWindowSystem wSystem) 
        {
            coswSys = wSystem;
            SetIntHandler(0x80, IRQHandler);
        }

        public void IRQHandler(ref IRQContext context)
        {
            CloudOSWindow window = new CloudOSWindow();
            window.text = "YAY!";
            coswSys.windows.Add(window);
        }
    }
}
