using Cosmos.Core;
using Cosmos.HAL;
using Cosmos.HAL.BlockDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public static class CloudOSPCI
    {
        static KernelClassDebugger debugger = new("CloudOS PCI");

        public static bool PCIInitComplete = false;

        public static void Init()
        {
            int i = 0;
            PCI.Setup();
            AHCI.Wait(1000);
            ACPI.Start(true, true);
            Cosmos.Core.Bootstrap.Init();
            Cosmos.Core.Global.Init();
            
            foreach (PCIDevice dev in PCI.Devices)
            {
                debugger.Send($"PCI #0{i}=> VendorID: {dev.VendorID} |  BAR0: {dev.BAR0}  | ProgIf: {dev.ProgIF}  ||  BUS!: {dev.bus} | func!: {dev.function} | slot!: {dev.slot} ||  Class: {dev.ClassCode}  | Sub: {dev.Subclass} | CMD: {GetCMD(dev.Command)}");

                if (dev.ClassCode == 1 && dev.Subclass == 6)
                {
                    AHCI_DISK disk = new();
                    disk.Init(i, dev);
                }
                i++;
            }
            PCIInitComplete = true;
        }

        private static string GetCMD(PCIDevice.PCICommand type)
        {
            switch (type)
            {
                case PCIDevice.PCICommand.Wait:
                    return "Wait";
                case PCIDevice.PCICommand.VGA_Pallete:
                    return "VGA_PALLETE";
                case PCIDevice.PCICommand.Special:
                    return "Special";
                case PCIDevice.PCICommand.SERR:
                    return "SERR";
                case PCIDevice.PCICommand.Parity:
                    return "Parity";
                case PCIDevice.PCICommand.Memory:
                    return "Memory";
                case PCIDevice.PCICommand.Master:
                    return "Master";
                case PCIDevice.PCICommand.IO:
                    return "I/O";
                case PCIDevice.PCICommand.Invalidate:
                    return "Invalidate";
                case PCIDevice.PCICommand.Fast_Back:
                    return "Fast_Back 512";
                default:
                    return "Unknown CMD";
            }
        }
    }
}
