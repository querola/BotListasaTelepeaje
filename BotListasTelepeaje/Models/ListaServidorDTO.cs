using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotListasTelepeaje.Models
{
    public class ListaServidorDTO
    {
        public string fileName { get; set; }
        public DateTime creationTime { get; set; }
        public string fileSize { get; set; }
        public bool delay { get; set; }
    }
}
