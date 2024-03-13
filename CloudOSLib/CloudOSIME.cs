using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudOSIME
    {
        static CloudOSIME? SelectedIME = null;
        static CloudOSIME _fallbackime = new();
        static Queue<CloudOSIME> IMEList = new();

        static CloudOSIME()
        {
            SelectedIME = new();
        }

        public virtual string GetIMEName()
        {
            return "English IME";
        }

        public virtual string GetShortIMEName()
        {
            return "EN";
        }
        
        public virtual char GetKeyAsIME(KeyEvent key)
        {
            return key.KeyChar;
        }

        public static void RegisterIME(CloudOSIME IME)
        {
            IMEList.Enqueue(IME);
        }

        public static void SwitchIME()
        {
            if (IMEList.Count > 0)
            {
                if (SelectedIME != null)
                {
                    IMEList.Enqueue(SelectedIME);
                }

                SelectedIME = IMEList.Dequeue();
            }
        }

        public static char GetKeyChar(KeyEvent key)
        {
            if (SelectedIME != null)
            {
                return SelectedIME.GetKeyAsIME(key);
            }
            else
            {
                return _fallbackime.GetKeyAsIME(key);
            }
        }

        public static string GetShortNameofCurrentIME()
        {
            if (SelectedIME != null)
            {
                return SelectedIME.GetShortIMEName();
            } 
            else
            {
                return "FALLBACK";
            }
        }
    }
}
