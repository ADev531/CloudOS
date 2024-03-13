using Cosmos.Core.Memory;
using System.Runtime.ConstrainedExecution;

namespace CloudOSLib
{
    public static class CloudOSExecuter
    {
        public static void Run(string filename)
        {
            unsafe
            {
                fixed (byte* ptr = File.ReadAllBytes(filename))
                {
                    // todo : create code
                    var app = (delegate*<void>)ptr;
                    app();
                }
            }
        }
    }
}