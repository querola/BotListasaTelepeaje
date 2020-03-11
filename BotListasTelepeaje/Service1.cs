using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Text;
using Telegram.Bot;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using BotListasTelepeaje.Models;

namespace BotListasTelepeaje
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer = null;
        public List<ListasObjectDTO> MyProperty { get; set; }
        private static readonly TelegramBotClient Bot = new TelegramBotClient("1135151562:AAGZemcnKdHL6uCSmN6Tiu9Zfs-SZ4VNHcg");
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 600000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        public void Inicio()
        {

            timer = new System.Timers.Timer();
            timer.Interval = 600000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

            
            timer.Enabled = false;
            timer.Stop();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://pc004.sytes.net:182/HistorialPlaza/Irapuato");
                MyProperty = JsonConvert.DeserializeObject<List<ListasObjectDTO>>(json);
            }
            ChecarWebServices();
            ChecarListasServidor();
            ChecarListasSQL();
            timer.Enabled = true;
            timer.Start();
            }
            catch (Exception Ex)
            {
                Bot.SendTextMessageAsync(-364639169, "Oh oh, algo salió mal con el bot que monitorea los servicios, que ironía :( : " + Ex.StackTrace);
                timer.Enabled = true;
                timer.Start();
     
            }
        }
        private void ChecarWebServices()
        {

            foreach (var dto in MyProperty)
            {
                WebServiceDTO webService = new WebServiceDTO();
                //Meto las propiedades del dto recorrido en el usuario
                if (dto.webService == null)
                {
                    //Mando un nullo por que no hay conexion
                    Bot.SendTextMessageAsync(-431912689, "No hay conexion en la plaza *" + dto.caseta + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                else
                {
                    //validar que el web service este actualizado
                    if (Convert.ToDateTime(dto.webService.date) < DateTime.Now.AddMinutes(-20))
                    {
                        Bot.SendTextMessageAsync(-431912689, "Atrasado el envio de cruces de la plaza: *" + dto.caseta + "* ultimo registro enviado: *" + dto.webService.date + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);

                        //Bot.SendTextMessageAsync(-431912689, "No se han enviado cruces desde *" + dto.webService.date + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        //webService.date = dto.webService.date;
                    }                    
                }
            }
        }
        private void ChecarListasServidor()
        {
            foreach (var dto in MyProperty)
            {           
                //Meto las propiedades del dto recorrido en el usuario
                if (dto.listaServidor == null)
                {
                    //Mando un nullo por que no hay conexion
                    //Bot.SendTextMessageAsync(-431912689, "No hay conexion en la plaza *" + dto.caseta + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                else
                {
                    //validar que el web service este actualizado
                    if (Convert.ToDateTime(dto.listaServidor.creationTime) < DateTime.Now.AddMinutes(-35))
                    {
                        Bot.SendTextMessageAsync(-431912689, "Atrasado las listas en el servidor: *" + dto.caseta + "* ultima lista creada a las: *" + dto.listaServidor.creationTime + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
            }
        }



        public  void ChecarListasSQL()
        {

            foreach (var dto in MyProperty)
            {
                //Meto las propiedades del dto recorrido en el usuario
                if (dto.lista == null)
                {
                    //Mando un nullo por que no hay conexion
                    //Bot.SendTextMessageAsync(-431912689, "No hay conexion en la plaza *" + dto.caseta + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                else
                {
                    //validar que el web service este actualizado
                    if (Convert.ToDateTime(dto.lista.creationTime) < DateTime.Now.AddMinutes(-35))
                    {
                        if (dto.caseta.ToString() != "Tepotzotlan")
                        {
                            Bot.SendTextMessageAsync(-431912689, "No se estan generando nuevas listas en la plaza: *" + dto.caseta + "* ultima lista creada fue a las: *" + dto.lista.creationTime + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);

                        }
                    }
                }
            }


        }

        protected override void OnStop()
        {
            //WriteToFile("WriteToFileHostedService: Process Stopped");
            File.WriteAllText(@"C:\temporal\LSTABINTBotStopped.txt", "Se detuvo");
        }
    }
}
