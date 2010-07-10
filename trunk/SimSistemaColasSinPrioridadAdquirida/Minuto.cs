using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSistemaColasSinPrioridadAdquirida
{
    class Minuto
    {
        public Double TM;
        public int WL0;
        public int WL1;
        public int WL2;
        public int SS;
        public Minuto(Double TM, int WL0, int WL1, int WL2, int SS) {
            this.TM = TM;
            this.WL0 = WL0;
            this.WL1 = WL1;
            this.WL2 = WL2;
            this.SS = SS;
        }
        public override string ToString()
        {
            return "(TM," + TM + ")(WL0," + WL0 + ")(WL1," + WL1 + ")(WL2," + WL2 + ")";
        }
    }
}
