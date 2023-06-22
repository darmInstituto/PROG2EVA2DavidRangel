using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{
    public partial class PantallaLog : Form
    {
        public PantallaLog()
        {
            InitializeComponent();
        }

        string rutaArchivo = Application.StartupPath + @"\archivo\VIGIADAVIDRANGEL.txt";
        string rutaBDD;
        private void button2_Click(object sender, EventArgs e)
        {
            /*! 18-05:
             * Se limpia el dataGridView
             * Se le colocan 5 columnas
             * Se realiza la lectura de la primera linea
             * esa linea se divide en ';' por lo que se utiliza split para convertir la linea en un arreglo tomando el ';' como separacion de cada celda
             * mientras la linea sea diferente de null sigue leyendo
             * si el uno de los registros tiene escrito "rut", quiere decir que es el encabezado de la tabla
             * El dataGridView1.Rows.Add() recibe por parametro un arreglo
             * por lo tanto el arreglo obtenido del split se agrega como una fila del dataGridView
             */
            dataGridView1.Rows.Clear();
            StreamReader sr = new StreamReader(rutaArchivo);
            String lectura;
            lectura = sr.ReadLine();
            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] == "clave")
                {
                    dataGridView1.Columns[0].HeaderText = registro[0];
                    dataGridView1.Columns[1].HeaderText = registro[1];
                    dataGridView1.Columns[2].HeaderText = registro[2];
                    dataGridView1.Columns[3].HeaderText = registro[3];
                    dataGridView1.Columns[4].HeaderText = registro[4];
                }
                else
                {
                    dataGridView1.Rows.Add(registro);
                }
                lectura = sr.ReadLine();
            }
            sr.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //! 18-05: si rut no es correcto no avanza
            if (Op.validarRut(textBox2.Text) == true)
            {
                dataGridView1.Rows.Clear();
                StreamReader sr = new StreamReader(rutaArchivo);
                String lectura;
                lectura = sr.ReadLine();
                //! misma logica que se explicó más arriba
                while (lectura != null)
                {
                    string[] registro = lectura.Split(';');

                    if (registro[0] == "clave")
                    {
                        dataGridView1.Columns[0].HeaderText = registro[0];
                        dataGridView1.Columns[1].HeaderText = registro[1];
                        dataGridView1.Columns[2].HeaderText = registro[2];
                        dataGridView1.Columns[3].HeaderText = registro[3];
                        dataGridView1.Columns[4].HeaderText = registro[4];
                    }
                    else
                    {
                        //! 18-05: si la primera celda (rut) del registro corresponde al mismo rut que se escribió, se muestra
                        if (registro[0] == Op.ponerMinusculas(textBox2.Text.PadLeft(10, '0')))
                        {
                            dataGridView1.Rows.Add(registro);
                        }
                    }
                    lectura = sr.ReadLine();
                }
                sr.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PantallaLog_Load(object sender, EventArgs e)
        {
            string rutaAplicacion = Application.StartupPath;
            int diferencia = rutaAplicacion.Length - 9;
            string bdd = rutaAplicacion.Remove(diferencia, 9);
            rutaBDD = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + bdd + "BDDPROG2DavidRangel.mdf\";Integrated Security=True";
            dataGridView1.ColumnCount = 5;
        }

        private void PantallaLog_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea limpiar toda la informacion?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string headers = "clave;inicioSesion;finSesion;accion;accionF\n";
                File.WriteAllText(rutaArchivo, headers);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            formatearCampoClave();

            StreamReader sr = new StreamReader(rutaArchivo);
            String lectura;
            lectura = sr.ReadLine();

            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] != "clave")
                {
                    if (registro[2] == "")
                    {
                        registro[2] = Op.parseDateTime(DateTime.MinValue, 1);
                    }

                    insertarRegistroEnBDD(registro[0], registro[1], registro[2], registro[3], registro[4]);
                }

                lectura = sr.ReadLine();
            }
            sr.Close();
            MessageBox.Show("Registros subidos a la BDD");
        } 
        

        void insertarRegistroEnBDD(string clave, string inicio, string fin, string accion, string accionF)
        {
            SqlConnection con = new SqlConnection(rutaBDD);
            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select rut from PERFILESDavidRangel");
            SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            string rut = clave.Substring(3);
            bool existeRut = false;
            foreach (DataRow fila in datos.Rows)
            {
                if(rut == fila[0].ToString())
                {
                    existeRut = true;
                }
            }

            if (existeRut)
            {
                con.Open();
                datos = new DataTable();
                setencia = String.Format("select clave, iniciosesion, finsesion, accion, accionf from ACCIONESDavidRangel where clave='{0}'", clave);
                dataAdapter = new SqlDataAdapter(setencia, con);
                dataAdapter.Fill(datos);
                con.Close();

                bool existeRegistro = false;


                foreach (DataRow fila in datos.Rows)
                {
                    /*
                     MessageBox.Show(parseDateTime(inicio) + "\n" + parseDateTime((DateTime)fila[1]));
                    MessageBox.Show(parseDateTime(fin) + "\n" + parseDateTime((DateTime)fila[2]));
                    MessageBox.Show(parseDateTime(accionF) + "\n" + parseDateTime((DateTime)fila[4]));*/

                    bool igualClave = clave == fila[0].ToString();
                    bool igualInicio = inicio == fila[1].ToString();
                    bool igualFin = fin == fila[2].ToString();
                    bool igualAccion = accion == fila[3].ToString();
                    bool igualAccionF = accionF == fila[4].ToString();

                    if (igualClave && igualInicio && igualFin && igualAccion && igualAccionF)
                    {
                        existeRegistro = true;
                    }
                }

                if (!existeRegistro)
                {
                    con.Open();
                    datos = new DataTable();
                    setencia = String.Format("insert into ACCIONESDavidRangel (clave, iniciosesion, finsesion, accion, accionf) values " +
                        "('{0}', '{1}', '{2}', '{3}', '{4}')", clave, inicio, fin, accion, accionF);

                    dataAdapter = new SqlDataAdapter(setencia, con);
                    dataAdapter.Fill(datos);
                    con.Close();
                }              
            }
        }

        

        void formatearCampoClave()
        {
            StreamReader sr = new StreamReader(rutaArchivo);
            String lectura;
            lectura = sr.ReadLine();
            List<string[]> matriz = new List<string[]>();
            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] != "clave")
                {
                    matriz.Add(registro);
                }
                
                lectura = sr.ReadLine();
            }
            sr.Close();


            SqlConnection con = new SqlConnection(rutaBDD);
            con.Open();
            DataTable perfiles = new DataTable();
            string setencia = String.Format("select Rut, Nombre, ApPat, ApMat from PERFILESDavidRangel");
            SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
            dataAdapter.Fill(perfiles);
            con.Close();

            //Recorro la informacion del txt
            foreach (string[] registro in matriz)
            {
                //busco si el rut que está en el txt está en la BDD
                foreach (DataRow fila in perfiles.Rows)
                {
                    string nombre = fila[1].ToString();
                    string paterno = fila[2].ToString();
                    string materno = fila[3].ToString();
                    string rut = fila[0].ToString();

                    if (rut == registro[0])
                    {
                        registro[0] = nombre[0].ToString() + paterno[0].ToString() + materno[0].ToString() + rut;
                    }
                }
            }

            string headers = "clave;inicioSesion;finSesion;accion;accionF\n";
            File.WriteAllText(rutaArchivo, headers);


            StreamWriter sw = File.AppendText(rutaArchivo);
            //! 18-05: foreach para recorrer toda la lista 
            foreach (string[] registro in matriz)
            {
                string fila = String.Format("{0};{1};{2};{3};{4}", registro[0], registro[1], registro[2], registro[3], registro[4]);

                sw.WriteLine(fila);
            }
            sw.Close();
        }
    }
}
