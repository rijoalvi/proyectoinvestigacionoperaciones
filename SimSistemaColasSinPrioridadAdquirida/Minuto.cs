using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSistemaColasSinPrioridadAdquirida
{
    class Minuto
    {
        public Double TM;
        public int WL;
        public int SS;
        public Minuto(Double TM, int WL, int SS) {
            this.TM = TM;
            this.WL = WL;
            this.SS = SS;
        }
        public override string ToString()
        {
            return "(TM,"+TM+")(WL,"+WL+")";
        }
    }
}
