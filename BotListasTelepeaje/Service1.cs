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
            timer.Interval = 300000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        public void Inicio()
        {

            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }
        private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();
            using (var client = new HttpClient())
            {
                var ReceiveLastLista = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));

                if (ReceiveLastLista.IsSuccessStatusCode)
                {
                    //ListasObject listasObject;
                    var contenido = await ReceiveLastLista.Content.ReadAsStringAsync();
                    contenido.ToString();
                    MyProperty = JsonConvert.DeserializeObject<List<ListasObjectDTO>>(contenido);

                    ChecarWebServices();
                    ChecarListasServidor();
                    ChecarListas();
                    //return null;
                }
                else
                {
                    //return null;
                    //algo fallo
                }
            }
        }
        //private async Task<ListaDTO> DoWork(object state)
        //{


        //}
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
                        Bot.SendTextMessageAsync(-431912689, "No se han enviado cruces desde *" + dto.webService.date + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        //webService.date = dto.webService.date;
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(-431912689, "Atrasado el envio de cruces de la plaza: *" + dto.caseta + "*Con fecha: *" + dto.webService.date + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
            }
            //throw new NotImplementedException();
        }
        private void ChecarListasServidor()
        {

            throw new NotImplementedException();
        }



        public async void ChecarListas()
        {

            using (var client = new HttpClient())
            {
                var ReceiveLastLista = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));

                if (ReceiveLastLista.IsSuccessStatusCode)
                {
                    //ListasObject listasObject;
                    var contenido = await ReceiveLastLista.Content.ReadAsStringAsync();
                    contenido.ToString();
                    var objResponse1 = JsonConvert.DeserializeObject<List<ListasObjectDTO>>(contenido);
                    foreach (var dto in objResponse1)
                    {

                        //listas
                        if (dto.lista == null)
                        {
                            //Mando un nullo por que no hay conexion
                            //await Bot.SendTextMessageAsync(-431912689, "No hay conexion en la plaza *" + dto.caseta + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);

                        }
                        else
                        {
                            if (Convert.ToDateTime(dto.lista.creationTime) < DateTime.Now.AddMinutes(-40))
                            {
                                {
                                    await Bot.SendTextMessageAsync(-431912689, "Cruces de la plaza: *" + dto.caseta + "*Con fecha: *" + dto.lista.creationTime + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);

                                }
                            }
                            if (dto.listaServidor == null)
                            {
                                //Mando un nullo por que no hay conexion
                                //await Bot.SendTextMessageAsync(-431912689, "No hay conexion en la plaza *" + dto.caseta + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);

                            }
                            else
                            {
                                if (Convert.ToDateTime(dto.lista.creationTime) < DateTime.Now.AddMinutes(-40))
                                {
                                    await Bot.SendTextMessageAsync(-431912689, "Cruces de la plaza: *" + dto.caseta + "*Con fecha: *" + dto.listaServidor.creationTime + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);


                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnStop()
        {
            //WriteToFile("WriteToFileHostedService: Process Stopped");

        }
    }
}
