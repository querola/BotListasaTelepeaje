using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using Telegram.Bot;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using BotListasTelepeaje.Models;

namespace BotListasTelepeaje
{
    public partial class Service1 : ServiceBase, IDisposable
    {
        private Timer _timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            //return Task.CompletedTask;
        }
        public void Inicio()
        {

            _timer = new Timer(
                DoWork,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10000));
            //return Task.CompletedTask;
        }
        private async void DoWork(object state)
        {
            //WriteToFile("WriteToFileHostedService: Process Start" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            var LSTABINTWorking = await ChecarListas();


        }   
        public async Task<ListaDTO> ChecarListas()
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
                        //Creo el usuario que vamos a añadir en ret
                        //Usuario user = new Usuario();
                        WebServiceDTO webService = new WebServiceDTO();
                        //Meto las propiedades del dto recorrido en el usuario
                        if (dto.webService == null)
                        {

                        }
                        else
                        {
                            webService.date = dto.webService.date;
                        }
                                                         
                        //Añado el usuario
                        //ret.Add(user);
                    }
                    //listasObject = JsonConvert.DeserializeObject<ListasObject>(contenido);
                    //return VerifyListasService(await ReceiveLastLista.Content.ReadAsStringAsync());
                }
            }
            return new ListaDTO()
            {

            };
        }           
        public async Task<object> CheckListas()
        {
            using (var client = new HttpClient())
            {
                //HttpResponseMessage response = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));
                var ReceiveLastLista = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));
                if (ReceiveLastLista.IsSuccessStatusCode)
                {                  
                    //return VerifyListasService(content);
                    var contenido = await ReceiveLastLista.Content.ReadAsStringAsync();
                    return VerifyListasService(await ReceiveLastLista.Content.ReadAsStringAsync());
                }
                return true;             
            }

        }
        public bool VerifyListasService(string DateLastLista)
        {
            if (Convert.ToDateTime(DateLastLista) < DateTime.Now.AddMinutes(-30))
            {
                //Bot.SendTextMessageAsync(-364639169, "Las LSTABINT no han sido actualizadas desde *" + DateLastLista + "*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void OnStop()
        {
            //WriteToFile("WriteToFileHostedService: Process Stopped");
            _timer?.Change(Timeout.Infinite, 0);
        }



        //public class WebService
        //{
        //    public DateTime date { get; set; }
        //    public bool delay { get; set; }
        //}



        //public class ListasObject
        //{
        
        //}
    }
}
