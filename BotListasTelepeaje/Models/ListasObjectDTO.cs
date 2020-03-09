using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotListasTelepeaje.Models
{
    public class ListasObjectDTO
    {
        public string caseta { get; set; }
        public ListaDTO lista { get; set; }
        public WebServiceDTO webService { get; set; }
        public ListaServidorDTO listaServidor { get; set; }
    }
}
