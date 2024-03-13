using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalDebugger = Cosmos.Debug.Kernel.Debugger;

namespace CloudOSLib
{
    public class KernelClassDebugger
    {
        List<string> Message = new List<string>();
        public string Name;
        public static List<KernelClassDebugger> Instances = new();
        ExternalDebugger extDebug;

        public KernelClassDebugger(string name) 
        {
            Name = name;
            Instances.Add(this);
            extDebug = new(name);
            extDebug.Send("KernelClassDebugger " + name);
        }

        public void Send(string msg)
        {
            Message.Add(msg);
            extDebug.Send(msg);
        }

        public List<string> GetMessages()
        {
            return Message;
        }
    }
}
