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
            var LSTABINTWorking = await CheckListas();


        }
        //private void WriteToFile(string message)
        //{
        //    var path = $@"C:\Prueba.txt";
        //    using (StreamWriter writer = new StreamWriter(path, append: true))
        //    {
        //        writer.WriteLine(message);
        //    }
        //}

        //        Meteorologia meteorologia;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://weathers.co/api.php?city=Madrid");
        //using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
        //using (Stream stream = response.GetResponseStream())
        //using (StreamReader reader = new StreamReader(stream))
        //{
        //    var json = reader.ReadToEnd();
        //    meteorologia = JsonConvert.DeserializeObject<Meteorologia>(json);
        //}
        //Console.WriteLine("La temperatura en Madrid es: " + meteorologia.Data.Temperature);
        public async Task<object> CheckListas()
        {
            //ListasObject listasObject;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://pc004.sytes.net:182/HistorialPlaza/Irapuato");
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    var json = reader.ReadToEnd();
            //    listasObject = JsonConvert.DeserializeObject<ListasObject>(json);
            //}
            //VerifyListasService(listasObject.lista.creationTime.ToString());
            //listasObject.listaServidor.creationTime;
            using (var client = new HttpClient())
            {
                //HttpResponseMessage response = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));

                var ReceiveLastLista = await client.GetAsync(new Uri("http://pc004.sytes.net:182/HistorialPlaza/Irapuato"));

                if (ReceiveLastLista.IsSuccessStatusCode)
                {
                    var content = await ReceiveLastLista.Content.ReadAsStringAsync();
                    //return VerifyListasService(content);
                    var contenido = await ReceiveLastLista.Content.ReadAsStringAsync();
                    return VerifyListasService(await ReceiveLastLista.Content.ReadAsStringAsync());

                }



                return true;
                //return VerifyLSTABINTSevice(await ReceiveLastLista.Content.ReadAsStringAsync());
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

        //public class Lista
        //{
        //    public string file { get; set; }
        //    public DateTime creationTime { get; set; }
        //    public string extension { get; set; }
        //    public bool delay { get; set; }
        //}

        //public class WebService
        //{
        //    public DateTime date { get; set; }
        //    public bool delay { get; set; }
        //}

        //public class ListaServidor
        //{
        //    public string fileName { get; set; }
        //    public DateTime creationTime { get; set; }
        //    public string fileSize { get; set; }
        //    public bool delay { get; set; }
        //}

        //public class ListasObject
        //{
        //    public string caseta { get; set; }
        //    public Lista lista { get; set; }
        //    public WebService webService { get; set; }
        //    public ListaServidor listaServidor { get; set; }
        //}
    }
}
