using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace SimSistemaColasSinPrioridadAdquirida
{
    class Simulador
    {
        public int tiposClientes; // la cantidad de tipos de clientes diferentes que puede haber en el sistema
        public enum Servidor { desocupado = 0, ocupado=1 };
        public Double TM;// = hora de la simulación
        //public Double AT;// = tiempo programado para la siguiente llegada
        public Double [] AT;// = tiempo programado para la siguiente llegada de cada cliente
        //public Double DT;// = tiempo programada para la siguiente salida
        public Double [] DT;// = tiempo programada para la siguiente salida de cada cliente
        public int SS;// = estado del empleado (1 = ocupado, O = desocupado)
        //public int WL;// = longitud de la cola de espera
        public int []WL;// = longitud de la cola de espera de cada cliente
        public int MX;// = longitud, en unidades de tiempo, de una corrida de simulación
        public Double IT;// = Tiempo entre llegadas
        public Double rho;
        public int clientesColas;
        public int contadorClientes;
        Reportador reportador;
        public int contadorEventos;
        Queue q = new Queue();
        Queue colaClientes = new Queue();
        public List<Cliente> historialClientes;
        public Random r;
        public Estadisticador estadisticador;
        Double mu;
        Double lambdaBuena;
        public Simulador()
        {
            tiposClientes = 3;
            historialClientes = new List<Cliente>();
            estadisticador = new Estadisticador();
            r = new Random();
            TM = 0;

            //AT = 0; //DESPUES VA A HABER QUE QUITAR ESTE Y DEJAR SOLO LOS OTROS 3
            AT = new Double[tiposClientes];
            for (int a = 0; a < (tiposClientes); a++)
            {
                AT[a] = 0;
            }

            //DT = 999999; //DESPUES VA A HABER QUE QUITAR ESTE Y DEJAR SOLO LOS OTROS 3
            DT = new Double[tiposClientes]; 
            for(int i = 0; i < (tiposClientes); i++){
                DT[i] = 999999;
            }

            SS = 0;

            //WL = 0; //DESPUES VA A HABER QUE QUITAR ESTE Y DEJAR SOLO LOS OTROS 3
            WL = new int[tiposClientes];
            for (int b = 0; b < (tiposClientes); b++)
            {
                WL[b] = 0;
            }

            MX = 10;
            IT = 0;
            reportador = new Reportador("reporte1.txt");
        }
        public void correr() {
            
            while(TM<=MX){
                contadorEventos++;
                //if ( AT < DT)//si llega alguien antes que salga el siguiente
                int ATminimo = calcularMinimo(AT);
                int DTminimo = calcularMinimo(DT);
                if ( AT[ATminimo] < DT[DTminimo] )//si llega alguien antes que salga el siguiente
                {//procesar llegada
                    contadorClientes = contadorClientes + 1;
                    TM = AT[ATminimo];
                    if ((int)Servidor.desocupado == SS)
                    {
                        SS = (int)Servidor.ocupado;
                        //Generar ST
                        DT[ATminimo] = TM + generarST();
                    }
                    else {//Servidor ocupado
                        WL[ATminimo] = WL[ATminimo] +1;
                        
                        clientesColas = clientesColas + 1;
//                        q.Enqueue(contadorClientes);
//                        colaClientes.Enqueue(new Cliente(contadorClientes,-1,TM));

                    }
                    //Generar IT
                    AT[ATminimo] = TM + generarIT(ATminimo);
//                    escribirNuevoEvento(contadorEventos, "Llegad", contadorClientes, TM, SS, ATminimo, ATminimo, ATminimo);
                }
                else { //procesar salida
                    //TM = DT;
                    if (WL[0] > 0)
                    {//si hay clientes en cola 1
                        WL[0] = WL[0] - 1;
                        //Generar ST
                        DT[0] = TM + generarST();
//                        escribirNuevoEvento(contadorEventos, "Salida", (int)q.Dequeue(), TM, SS, 0, 0, 0);
//                        Cliente clienteTerminandoCola = (Cliente)colaClientes.Dequeue();
//                        clienteTerminandoCola.setTMFinal(TM);
//                        historialClientes.Add(clienteTerminandoCola);
                    }
                    else if (DT[0] == 999999){//No hay clientes en cola 1 pero cliente es tipo 1
                        SS = (int)Servidor.desocupado;
                        DT[1] = 999999;
//                        escribirNuevoEvento(contadorEventos, "Salida", contadorClientes, TM, SS, 0, 0, 0);
                    }
                    else if (WL[1] > 0)
                    {//si hay clientes en cola 2
                        WL[1] = WL[1] - 1;
                        //Generar ST
                        DT[1] = TM + generarST();
//                        escribirNuevoEvento(contadorEventos, "Salida", (int)q.Dequeue(), TM, SS, 1, 1, 1);
//                        Cliente clienteTerminandoCola = (Cliente)colaClientes.Dequeue();
//                        clienteTerminandoCola.setTMFinal(TM);
//                        historialClientes.Add(clienteTerminandoCola);
                    }
                    else if (DT[1] == 999999)
                    {//No hay clientes en cola 1 pero cliente es tipo 1
                        SS = (int)Servidor.desocupado;
                        DT[1] = 999999;
//                        escribirNuevoEvento(contadorEventos, "Salida", contadorClientes, TM, SS, 1, 1, 1);
                    }
                    else if (WL[2] > 0)
                    {//si hay clientes en cola 2
                        WL[2] = WL[2] - 1;
                        //Generar ST
                        DT[2] = TM + generarST();
//                        escribirNuevoEvento(contadorEventos, "Salida", (int)q.Dequeue(), TM, SS, 2, 2, 2);
//                        Cliente clienteTerminandoCola = (Cliente)colaClientes.Dequeue();
//                        clienteTerminandoCola.setTMFinal(TM);
//                        historialClientes.Add(clienteTerminandoCola);
                    }
                    else if (DT[2] == 999999)
                    {//No hay clientes en cola 1 pero cliente es tipo 1
                        SS = (int)Servidor.desocupado;
                        DT[2] = 999999;
//                        escribirNuevoEvento(contadorEventos, "Salida", contadorClientes, TM, SS, 2, 2, 2);
                    }
                }
            }
            reportador.close();
//            Double totalPersonasCola=0;
//            Double totalPersonasEnSistema=0;
//            for (int i = 0; i < estadisticador.dominioTiempo.Count; i++ )
//            {
//                totalPersonasCola += estadisticador.dominioTiempo[i].WL;
//                totalPersonasEnSistema += estadisticador.dominioTiempo[i].WL + estadisticador.dominioTiempo[i].SS;
//            }
//            totalPersonasCola = totalPersonasCola / estadisticador.dominioTiempo.Count;
//            totalPersonasEnSistema = totalPersonasEnSistema / estadisticador.dominioTiempo.Count;
//            rho=lambdaBuena/mu;
//            Double FactorEscala=rho;
            //MessageBox.Show("Lq (Discreto): " + totalPersonasCola * (1 + rho) + "\n Lq (continuo):" + (Math.Pow(rho, 2) / (1 - rho)) + "\nRho: " + rho + "\nL (Discreto): " + totalPersonasEnSistema * (1 + rho) + "\n L (Continuo): " + (rho/ (1 - rho)));
//            Double tiempoPromedioEnCola = calcularTiempoEnCola();
//            Double LqContinuo = (Math.Pow(rho, 2) / (1 - rho));
//            Double WqContinuo = LqContinuo / lambdaBuena;
//            MessageBox.Show("Lq (Discreto): " + totalPersonasCola + "\n Lq (continuo):" + LqContinuo + "\nRho: " + rho + "\nL (Discreto): " + totalPersonasEnSistema + "\n L (Continuo): " + (rho / (1 - rho)) + "\nTiempo promedio en cola (Discreto): " + tiempoPromedioEnCola + "\nTiempo promedio en cola (Continuo): " + WqContinuo);
           // Console.WriteLine("Personas esperadas en cola (Lq): " + totalPersonasCola);
        }
        public void escribirNuevoEvento(int contadorEventos, String tipo, int contadorClientes, Double TM, int SS, int indiceWL, int indiceAT, int indiceDT){
            //reportador.escribirNuevoEvento(contadorEventos, tipo, contadorClientes, TM, SS, WL[indiceWL], AT[indiceAT], DT[indiceDT]);
            estadisticador.ingresarMinuto(TM, WL[0],WL[1],WL[2], SS);
        }
        private Double generarST(){
            //---------esto va determinado por el usuario (numero de clientes x unidad de tiempo)-----------
            mu = 100;
            //----------------------------------------------------------------------------------------------
            Double ws = 1 / mu;
            return ws;//ws=1/μ
        }
        private Double generarIT(int indice)//           1/λ
        {
            lambdaBuena=85;
       //     Double diferencia = lambdaBuena / 10;
            Double rand =  (r.Next(0,10099000)/10000000.0);
            //Double rand = (r.Next(0, 10190000) / 10000000.0);
             //rand = (r.NextDouble());
            if (rand==0)
            {
                rand = 0.000000001;
               // MessageBox.Show("alto");
            }
            //Double valor = (Math.Log(rand,2.745) / (lambdaBuena * -1)) ;
             Double valor = (Math.Log(rand) / (lambdaBuena * -1));
            
            return valor;
  
        }
        public Double calcularTiempoEnCola(){
            Double tiempoPromedioEnCola = 0;
            for (int i = 0; i<historialClientes.Count; i++ )
            {
                tiempoPromedioEnCola = tiempoPromedioEnCola+historialClientes[i].tiempoEsperaCola;
            }
            tiempoPromedioEnCola = tiempoPromedioEnCola / historialClientes.Count;
            return tiempoPromedioEnCola;
        }
            //Double lambda = 0.4;
            //return (1 / lambda);

        private int calcularMinimo(double [] x){
            int minimo;
            minimo = -1;
            if((x[0]<=x[1])&&(x[0]<=x[2])){
                minimo = 0;
            }
            else if ((x[1]<x[0])&&(x[1]<=x[2])){
                minimo = 1;
            }
            else if((x[2]<x[0])&&(x[2]<x[1])){
                minimo = 2;
            }
            return(minimo);
        }
    }
}
