using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkHelper;

namespace Kelettravel2024
{
    class Kapcsolatfelvetel
    {
        public int id { get; set; }
        public string nev { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string megjegyzes { get; set; }

        public bool Hianyos
        {
            get
            {
                if (nev == "" || email == "" || telefon == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

   
}
