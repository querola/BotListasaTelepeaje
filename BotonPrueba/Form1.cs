using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;

namespace BotonPrueba
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void Iniciar_Click(object sender, EventArgs e)
        {
            BotListasTelepeaje.Service1 service = new BotListasTelepeaje.Service1();
            service.Inicio();
            Iniciar.Enabled = false;
        }
    }
}
