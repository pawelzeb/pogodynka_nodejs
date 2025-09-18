using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka
{
    internal interface IAlertObserver
    {
        public void Call(string sCity, string sMsg);
    }
}
