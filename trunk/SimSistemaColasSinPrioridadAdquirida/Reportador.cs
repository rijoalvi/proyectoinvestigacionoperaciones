using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimSistemaColasSinPrioridadAdquirida
{
    class Reportador
    {
        public TextWriter file;

        public Reportador(String fileName)
        {
            file = new StreamWriter(fileName);
            file.WriteLine("Final del Evento\t Tipo de Evento\t Cliente Número\t TM\t\t\t\t SS\t WL\t AT (next)\t DT");
        }
        public void escribirNuevoEvento(Double numeroEvento, String tipoEvento, Double clienteNumero, Double TM, Double SS, Double WL, Double AT, Double DT)
        {
            file.WriteLine(numeroEvento + "\t\t\t " + tipoEvento + "\t\t " + clienteNumero + "\t\t " + TM + "\t\t\t\t  " + SS + "\t " + WL + "\t " + AT + "\t\t " + DT);
        }
        public void close()
        {
            file.Close();
        }
    }
}
