using Cosmos.HAL.BlockDevice.Ports;
using Cosmos.HAL.BlockDevice;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.HAL;

namespace CloudOSLib
{
    public class CloudVFSManager
    {
        KernelClassDebugger debugger = new("CloudOS VFS");
        CosmosVFS vfs;

        public CloudVFSManager() 
        {
            debugger.Send("Loading SATA Devices...");

            if (!CloudOSPCI.PCIInitComplete)
            {
                CloudOSPCI.Init();
            }

            List<BlockDevice> satadevices = SATA.Devices;

            // Temp vfs until CloudFS/CloudVFS is created
            debugger.Send("Loading VFS...");

            vfs = new();
            VFSManager.RegisterVFS(vfs);

            debugger.Send("Loading SATA VFS...");
            foreach (var device in satadevices)
            {
                Disk disk = new(device);

                if (!vfs.Disks.Contains(disk))
                {
                    debugger.Send("Adding disk to VFS");
                    vfs.Disks.Add(disk);
                }
            }

            Directory.SetCurrentDirectory("0:\\");
        }
    }
}
