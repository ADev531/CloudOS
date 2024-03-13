using CosmosTTF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudOSFont
    {
        public string Name { get; private set; }
        public float Size { get; private set; }

        public CloudOSFont(string Name, int Size) 
        { 
            this.Name = Name;
            this.Size = Size;
        }

        public CloudOSFont(string Name, float Size)
        {
            this.Name = Name;
            this.Size = Size;
        }
    }
}
