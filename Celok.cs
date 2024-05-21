using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelettravel2024
{
    class Celok
    {
        public int celok_id { get; set; }
        public string celok_nev { get; set; }
        public string celok_kep { get; set; }
        public string celok_kultura_honap{ get; set; }
        public int celok_orszag { get; set; }


        public bool KetSzo()
        {
            return celok_nev.Contains(" ");
        }
    }

  
}
