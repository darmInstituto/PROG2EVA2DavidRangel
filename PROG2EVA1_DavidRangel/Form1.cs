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
            string rut = Op.ponerMinusculas(textBox1.Text);
            
            if (Op.validarRut(rut) == true)
            {
                JuegoMemoria form2 = new JuegoMemoria(rut.PadLeft(10, '0'));
                form2.ShowDialog();
            }
            textBox1.Clear();
        }

        string ruta = Application.StartupPath + @"\archivo\VIGIADAVIDRANGEL.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\imagenes\fondorut.jpg");
            //! 18-05: se determina si existe o no el archivo
            bool existe = File.Exists(ruta);
            
            if (existe)
            {
                StreamReader sr = new StreamReader(ruta);
                string lectura;
                lectura = sr.ReadLine();
                //! 18-05: si los encabezados están malos, se limipia el archivo
                if (lectura != "clave;inicioSesion;finSesion;accion;accionF")
                {
                    string headers = "clave;inicioSesion;finSesion;accion;accionF\n";
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
                //! 18-05: si no existe, se crea
                string headers = "clave;inicioSesion;finSesion;accion;accionF\n";
                File.WriteAllText(ruta, headers);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();       
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PantallaLog pantallaLog = new PantallaLog();
            pantallaLog.ShowDialog();                    
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Perfiles perfiles = new Perfiles();
            perfiles.ShowDialog();
        }
    }
}
