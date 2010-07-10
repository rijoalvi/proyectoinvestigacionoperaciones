using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimSistemaColasSinPrioridadAdquirida
{
    class Cliente
    {
        public int ID;
        public int tipo;
        public Double TMInicio;
        private Double TMFinal;
        public Double tiempoEsperaCola;
        public Cliente(int ID, int tipo, Double TMInicio) {
            this.ID = ID;
            this.tipo = tipo;
            this.TMInicio = TMInicio;
        }
        public void setTMFinal(Double TMFinal) { 
            this.TMFinal=TMFinal;
            this.tiempoEsperaCola = TMFinal - TMInicio;
            if (tiempoEsperaCola<=0)
            {
                MessageBox.Show("Error en tiempo en cola");
            }
        }
    }
}
