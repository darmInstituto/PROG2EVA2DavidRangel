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
    public partial class PantallaLog : Form
    {
        public PantallaLog()
        {
            InitializeComponent();
        }

        string ruta = Application.StartupPath + @"\archivo\VIGIADAVIDRANGEL.txt";
        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();
            StreamReader sr = new StreamReader(ruta);
            String lectura;
            lectura = sr.ReadLine();
            dataGridView1.ColumnCount = 5;
            while (lectura != null)
            {
                string[] registro = lectura.Split(';');

                if (registro[0] == "rut")
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
            if (ValidacionRut.validarRut(textBox2.Text) == true)
            {
                dataGridView1.Rows.Clear();
                StreamReader sr = new StreamReader(ruta);
                String lectura;
                lectura = sr.ReadLine();
                
                while (lectura != null)
                {
                    string[] registro = lectura.Split(';');

                    if (registro[0] == "rut")
                    {
                        dataGridView1.Columns[0].HeaderText = registro[0];
                        dataGridView1.Columns[1].HeaderText = registro[1];
                        dataGridView1.Columns[2].HeaderText = registro[2];
                        dataGridView1.Columns[3].HeaderText = registro[3];
                        dataGridView1.Columns[4].HeaderText = registro[4];
                    }
                    else
                    {
                        if (registro[0] == textBox2.Text)
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
            dataGridView1.ColumnCount = 5;
        }

        private void PantallaLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea limpiar toda la informacion?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string headers = "rut;inicioSesion;finSesion;accion;accionF\n";
                File.WriteAllText(ruta, headers);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
