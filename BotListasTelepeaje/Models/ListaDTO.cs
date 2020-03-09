using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotListasTelepeaje.Models
{
    public class ListaDTO
    {
        public string file { get; set; }
        public DateTime creationTime { get; set; }
        public string extension { get; set; }
        public bool delay { get; set; }

    }
}
