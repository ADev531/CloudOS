using Cosmos.HAL;
using Cosmos.System.Graphics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudOSProcess
    {
        static List<CloudOSProcess> Processes = new();
        static List<CloudOSProcess> RunningProcesses = new();

        public bool Running = false;
        public bool Removing = false;
        public Action<CloudOSProcess> Method;
        
        public CloudOSProcess(Action<CloudOSProcess> method)
        {
            Method = method;
        }

        public void Run()
        {
            Running = true;
            Method.Invoke(this);
        }

        public static void Start()
        {
            Processes = new();

            Global.PIT.RegisterTimer(new(UpdateProcesses, 100, 0));
        }

        static void UpdateProcesses()
        {
            CloudOSProcess process = Processes[0];

            Processes.Remove(process);

            process.Run();

            CloudOSProcess running = RunningProcesses[0];

            if (!running.Running)
            {
                if (!running.Removing)
                {
                    Processes.Add(running);
                }
            } 
            else
            {
                RunningProcesses.Add(running);
            }
        }

        public static void CreateProcess(Action<CloudOSProcess> process)
        {
            CreateSystemProcess(process);
        }

        public static void CreateSystemProcess(Action<CloudOSProcess> process)
        {
            Processes.Add(new(process));
        }
    }
}
