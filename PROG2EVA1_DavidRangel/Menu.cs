using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }
        string rut;
        int nivel;
        public Menu(string rut, int nivel)
        {
            this.rut = rut;
            InitializeComponent();
            this.nivel = nivel;
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Perfiles perfiles = new Perfiles(rut, nivel);
            var result = perfiles.ShowDialog();
            
            if (result == DialogResult.No)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PantallaLog pantallaLog = new PantallaLog();
            pantallaLog.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JuegoMemoria juegoMemoria = new JuegoMemoria(rut);
            juegoMemoria.ShowDialog();
        }
    }
}
