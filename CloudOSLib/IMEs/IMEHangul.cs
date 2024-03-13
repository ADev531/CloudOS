using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib.IMEs
{
    public class IMEHangul : CloudOSIME
    {
        Dictionary<char, char> keyMap = new()
        {
            { 'q', 'ㅂ' },
            { 'w', 'ㅈ' },
            { 'e', 'ㄷ' },
            { 'r', 'ㄱ' },
            { 't', 'ㅅ' },
            { 'y', 'ㅛ' },
            { 'u', 'ㅕ' },
            { 'i', 'ㅑ' },
            { 'o', 'ㅐ' },
            { 'p', 'ㅔ' },
            { 'a', 'ㅁ' },
            { 's', 'ㄴ' },
            { 'd', 'ㅇ' },
            { 'f', 'ㄹ' },
            { 'g', 'ㅎ' },
            { 'h', 'ㅗ' },
            { 'j', 'ㅓ' },
            { 'k', 'ㅏ' },
            { 'l', 'ㅣ' },
            { 'z', 'ㅋ' },
            { 'x', 'ㅌ' },
            { 'c', 'ㅊ' },
            { 'v', 'ㅍ' },
            { 'b', 'ㅠ' },
            { 'n', 'ㅜ' },
            { 'm', 'ㅡ' },
            { 'Q', 'ㅃ' },
            { 'W', 'ㅉ' },
            { 'E', 'ㄸ' },
            { 'R', 'ㄲ' },
            { 'T', 'ㅆ' },
            { 'O', 'ㅒ' },
            { 'P', 'ㅖ' },
        };

        public override string GetIMEName()
        {
            return "한글 입력기";
        }

        public override string GetShortIMEName()
        {
            return "한";
        }

        public override char GetKeyAsIME(KeyEvent key)
        {
            if (keyMap.ContainsKey(key.KeyChar))
            {
                return keyMap[key.KeyChar];
            } 
            else
            {
                return base.GetKeyAsIME(key);
            }
        }
    }
}
