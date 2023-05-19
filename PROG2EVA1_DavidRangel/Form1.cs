using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rut = textBox1.Text;

            if (ValidacionRut.validarRut(rut) == true)
            {
                JuegoMemoria form2 = new JuegoMemoria(rut);
                form2.Show();
                this.Hide();
            }         
        }

        string ruta = Application.StartupPath + @"\archivo\VIGIADAVIDRANGEL.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\imagenes\fondorut.jpg");

            bool existe = File.Exists(ruta);
            
            if (existe)
            {
                StreamReader sr = new StreamReader(ruta);
                string lectura;
                lectura = sr.ReadLine();
                if (lectura != "rut;inicioSesion;finSesion;accion;accionF")
                {
                    string headers = "rut;inicioSesion;finSesion;accion;accionF\n";
                    sr.Close();
                    File.WriteAllText(ruta, headers);
                }
                else
                {
                    sr.Close();
                }
                
            }
            else
            {
                string headers = "rut;inicioSesion;finSesion;accion;accionF\n";
                File.WriteAllText(ruta, headers);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
                    
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PantallaLog pantallaLog = new PantallaLog();
            pantallaLog.Show();          
            this.Hide();
        }
    }
}
