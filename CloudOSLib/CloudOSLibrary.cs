using Cosmos.Core.Memory;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudOSLibrary
    {
        CloudVFSManager vfsManager;
        public CloudOSWindowSystem windowsSystem;
        public SystemCall calls;
        public KernelClassDebugger debugger;

        public CloudOSLibrary()
        {
            debugger = new("CloudOS Kernel library");
            debugger.Send("CloudOSLibrary Loading");

            RAT.MinFreePages = 200;
            //CloudOSPCI.Init();
            //debugger.Send("Waiting for PCI Load");
            //while (!CloudOSPCI.PCIInitComplete) { }
            //debugger.Send("PCI Load End");

            Canvas canvas = new VBECanvas(new(800, 600, ColorDepth.ColorDepth32));

            CloudFontManager.Load(canvas);

            debugger.Send("Starting Process System");

            CloudOSProcess.Start();

            vfsManager = new CloudVFSManager();
            windowsSystem = new CloudOSWindowSystem(canvas);
            calls = new SystemCall(windowsSystem);
        }

        public List<string> GetClassDebuggerNames()
        {
            List<string> debuggerNames = new();

            foreach (KernelClassDebugger debugger in KernelClassDebugger.Instances)
            {
                debuggerNames.Add(debugger.Name);
            }

            return debuggerNames;
        }

        public KernelClassDebugger GetClassDebugger(string name)
        {
            KernelClassDebugger classDebugger = KernelClassDebugger.Instances[0];
            bool found = false;

            foreach (KernelClassDebugger debugger in KernelClassDebugger.Instances)
            {
                if (debugger.Name == name)
                {
                    classDebugger = debugger;
                    found = true;
                } 
            }

            if (found == false)
            {
                throw new Exception("KernelClassDebugger not found");
            }
            return classDebugger;
        }
    }
}
