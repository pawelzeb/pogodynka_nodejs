using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Pogodynka.Utils
{

    public class InternetCheck
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool IsConnected()
        {
            return InternetGetConnectedState(out _, 0);
        }
    }

}
