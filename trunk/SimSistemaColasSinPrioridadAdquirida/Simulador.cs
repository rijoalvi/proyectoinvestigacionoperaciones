using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SimSistemaColasSinPrioridadAdquirida
{
    class Simulador
    {
        public enum Servidor { desocupado = 0, ocupado=1 };
        public Double TM;// = hora de la simulación
        public Double AT;// = tiempo programado para la siguiente llegada
        public Double DT;// = tiempo programada para la siguiente salida
        public int SS;// = estado del empleado (1 = ocupado, O = desocupado)
        public int WL;// = longitud de la cola de espera
        public int MX;// = longitud, en unidades de tiempo, de una corrida de simulación
        public Double IT;// = Tiempo entre llegadas
        public Double rho;
        public int clientesColas;
        public int contadorClientes;
        Reportador reportador;
        public int contadorEventos;
        Queue q = new Queue();
        public Random r;
        public Estadisticador estadisticador;
        Double mu;
        Double lambdaBuena;
        public Simulador()
        {
            estadisticador = new Estadisticador();
            r = new Random();
            TM = 0;
            AT = 0;
            DT = 999999;
            SS = 0;
            WL = 0;
            MX = 100000;
            IT = 0;
            reportador = new Reportador("reporte1.txt");
        }
        public void correr() {
            
            while(TM<=MX){
                contadorEventos++;
                if (AT < DT)//si llega alguien antes que salga el siguiente
                {//procesar llegada
                    contadorClientes = contadorClientes + 1;
                    TM = AT;
                    if ((int)Servidor.desocupado == SS)
                    {
                        SS = (int)Servidor.ocupado;
                        //Generar ST
                        DT = TM + generarST();
                    }
                    else {//Servidor ocupado
                        WL = WL + 1;
                        clientesColas = clientesColas + 1;
                        q.Enqueue(contadorClientes);
                    }
                    //Generar IT
                    AT = TM + generarIT();
                    escribirNuevoEvento(contadorEventos, "Llegad", contadorClientes, TM, SS, WL, AT, DT);
                }
                else { //procesar salida
                    TM = DT;
                    if (WL > 0)
                    {//si hay clientes en cola
                        WL = WL - 1;
                        //Generar ST
                        DT = TM + generarST();
                        escribirNuevoEvento(contadorEventos, "Salida", (int)q.Dequeue(), TM, SS, WL, AT, DT);
                    }
                    else {//No hay clientes en cola
                        SS = (int)Servidor.desocupado;
                        DT = 999999;
                        escribirNuevoEvento(contadorEventos, "Salida", contadorClientes, TM, SS, WL, AT, DT);
                    }
                }
            }
            reportador.close();
            Double totalPersonasCola=0;
            Double totalPersonasEnSistema=0;
            for (int i = 0; i < estadisticador.dominioTiempo.Count; i++ )
            {
                totalPersonasCola += estadisticador.dominioTiempo[i].WL;
                totalPersonasEnSistema += estadisticador.dominioTiempo[i].WL + estadisticador.dominioTiempo[i].SS;
            }
            totalPersonasCola = totalPersonasCola / estadisticador.dominioTiempo.Count;
            totalPersonasEnSistema = totalPersonasEnSistema / estadisticador.dominioTiempo.Count;
            rho=lambdaBuena/mu;
            Double FactorEscala=rho;
            //MessageBox.Show("Lq (Discreto): " + totalPersonasCola * (1 + rho) + "\n Lq (continuo):" + (Math.Pow(rho, 2) / (1 - rho)) + "\nRho: " + rho + "\nL (Discreto): " + totalPersonasEnSistema * (1 + rho) + "\n L (Continuo): " + (rho/ (1 - rho)));
            MessageBox.Show("Lq (Discreto): " + totalPersonasCola  + "\n Lq (continuo):" + (Math.Pow(rho, 2) / (1 - rho)) + "\nRho: " + rho + "\nL (Discreto): " + totalPersonasEnSistema  + "\n L (Continuo): " + (rho / (1 - rho)));
           // Console.WriteLine("Personas esperadas en cola (Lq): " + totalPersonasCola);
        }
        public void escribirNuevoEvento(int contadorEventos, String tipo, int contadorClientes, Double TM, int SS, int WL, Double AT, Double DT){
          //  reportador.escribirNuevoEvento(contadorEventos, tipo, contadorClientes, TM, SS, WL, AT, DT);
            estadisticador.ingresarMinuto(TM, WL, SS);
        }
        private Double generarST(){
            mu = 10;
            Double ws = 1 / mu;
            return ws;//ws=1/μ
        }
        private Double generarIT()//           1/λ
        {
            lambdaBuena=5;
       //     Double diferencia = lambdaBuena / 10;
            Double rand = 0;// (r.Next(0,9999999)/10000000.0);
             rand = (r.NextDouble());
            /*if (rand==0)
            {
               // rand = 0.0000001;
                MessageBox.Show("alto");
            }*/
            Double valor = (Math.Log(rand,2.732) / (lambdaBuena * -1)) ;
            return valor;
           /* if(rand<10){
                return 1;
            }
            else if(rand<20){
                return 2;
            }
            else if(rand<30){
                return 3;
            }
            else if((rand<40)){
                return 4;
            }
            else if ((rand < 50))
            {
                return 5;
            }
            else if ((rand < 60))
            {
                return 6;
            }
            else if ((rand < 70))
            {
                return 7;
            }
            else if ((rand < 80))
            {
                return 8;
            }
            else if ((rand < 90))
            {
                return 9;
            }
            else if ((rand <100))
            {
                return 10;
            }
            //tiempo promedio entre llegadas 1/λ=5.5 alpha 2/11=0.1818
            return -1;*/
        }
            //Double lambda = 0.4;
            //return (1 / lambda);
    }
}
