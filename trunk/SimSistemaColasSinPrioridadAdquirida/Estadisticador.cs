using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimSistemaColasSinPrioridadAdquirida
{
    class Estadisticador
    {
        public List<Minuto> dominioTiempo;
        public int contadorTiempo;
        public Estadisticador() {
            dominioTiempo = new List<Minuto>();
            contadorTiempo=-1;
        }
        public void ingresarMinuto(Double TM, int WL0, int WL1, int WL2, int SS){
            if (contadorTiempo == (TM - 1))
            {
                dominioTiempo.Add(new Minuto(TM, WL0, WL1, WL2, SS));
                contadorTiempo++;
            }
            else {
                for (int i = (int)dominioTiempo[dominioTiempo.Count - 1].TM+1; i < TM; i++)//Estrapolación
                {
                    dominioTiempo.Add(new Minuto(i, dominioTiempo[dominioTiempo.Count-1].WL0, WL1, WL2, SS));
                }
                dominioTiempo.Add(new Minuto(TM, WL0, WL1, WL2, SS));
            }
        
        }
    }
}
