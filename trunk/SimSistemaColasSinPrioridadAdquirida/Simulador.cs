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
        Queue colaClientes = new Queue();
        public List<Cliente> historialClientes;
        public Random r;
        public Estadisticador estadisticador;
        Double mu;
        Double lambdaBuena;
        public Simulador()
        {
            historialClientes = new List<Cliente>();
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
                        colaClientes.Enqueue(new Cliente(contadorClientes,-1,TM));

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
                        Cliente clienteTerminandoCola = (Cliente)colaClientes.Dequeue();
                        clienteTerminandoCola.setTMFinal(TM);
                        historialClientes.Add(clienteTerminandoCola);
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
            Double tiempoPromedioEnCola = calcularTiempoEnCola();
            Double LqContinuo = (Math.Pow(rho, 2) / (1 - rho));
            Double WqContinuo = LqContinuo / lambdaBuena;
            MessageBox.Show("Lq (Discreto): " + totalPersonasCola + "\n Lq (continuo):" + LqContinuo + "\nRho: " + rho + "\nL (Discreto): " + totalPersonasEnSistema + "\n L (Continuo): " + (rho / (1 - rho)) + "\nTiempo promedio en cola (Discreto): " + tiempoPromedioEnCola + "\nTiempo promedio en cola (Continuo): " + WqContinuo);
           // Console.WriteLine("Personas esperadas en cola (Lq): " + totalPersonasCola);
        }
        public void escribirNuevoEvento(int contadorEventos, String tipo, int contadorClientes, Double TM, int SS, int WL, Double AT, Double DT){
            //reportador.escribirNuevoEvento(contadorEventos, tipo, contadorClientes, TM, SS, WL, AT, DT);
            estadisticador.ingresarMinuto(TM, WL, SS);
        }
        private Double generarST(){
            mu = 100;
            Double ws = 1 / mu;
            return ws;//ws=1/μ
        }
        private Double generarIT()//           1/λ
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
    }
}
