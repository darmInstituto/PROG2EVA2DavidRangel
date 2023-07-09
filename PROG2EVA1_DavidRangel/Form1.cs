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
using MySql.Data.MySqlClient;

namespace PROG2EVA1_DavidRangel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string rutaArchivo = @"C:\TXTS\VIGIADAVIDRANGEL.txt";
        string rutaBDD = "Server=127.0.0.1;User=root;Database=BDDPROG2DavidRangel;password=''";

        private void button1_Click(object sender, EventArgs e)
        {
            string rut = Op.ponerMinusculas(txtRut.Text.PadLeft(10,'0'));
            string clave = txtClave.Text;

            if (Op.validarRut(rut) == true)
            {
                MySqlConnection con = new MySqlConnection(rutaBDD);
                con.Open();
                DataTable datos = new DataTable();
                string setencia = String.Format("select nivel from PERFILESDavidRangel where rut='{0}'", rut);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
                dataAdapter.Fill(datos);
                con.Close();

                if (datos.Rows.Count > 0)
                {
                    int nivel = int.Parse(datos.Rows[0][0].ToString());
                    if (nivel == 2)
                    {
                        JuegoMemoria juegoMemoria = new JuegoMemoria(rut);
                        juegoMemoria.ShowDialog();
                    }
                    else
                    {
                        Menu menu = new Menu(rut, nivel);
                        menu.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Tiene que registrar su rut (sign up)");
                }           
            }
            txtRut.Clear();
        }


        

        private void Form1_Load(object sender, EventArgs e)
        {           
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\imagenes\fondorut.jpg");
            //! 18-05: se determina si existe o no el archivo
            bool existe = File.Exists(rutaArchivo);
            
            if (existe)
            {
                StreamReader sr = new StreamReader(rutaArchivo);
                string lectura;
                lectura = sr.ReadLine();
                //! 18-05: si los encabezados están malos, se limipia el archivo
                if (lectura != "clave;inicioSesion;finSesion;accion;accionF")
                {
                    string headers = "clave;inicioSesion;finSesion;accion;accionF\n";
                    sr.Close();
                    File.WriteAllText(rutaArchivo, headers);
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
                File.WriteAllText(rutaArchivo, headers);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();       
        }

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Perfiles perfiles = new Perfiles();
            perfiles.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Creditos creditos = new Creditos();
            creditos.ShowDialog();
        }
    }
}
