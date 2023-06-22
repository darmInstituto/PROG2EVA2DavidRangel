using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{
    public partial class Perfiles : Form
    {
        public Perfiles()
        {
            InitializeComponent();
        }
        string ruta;
        int nivel = 1;
        TextBox[] arrTextBoxs;
        private void Perfiles_Load(object sender, EventArgs e)
        {
            arrTextBoxs = new TextBox[5] { txtRut, txtNombre, txtPaterno, txtMaterno, txtNivel };
            string rutaAplicacion = Application.StartupPath;
            int diferencia = rutaAplicacion.Length - 9;
            string rutaBDD = rutaAplicacion.Remove(diferencia, 9);
            ruta = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"" + rutaBDD + "BDDPROG2DavidRangel.mdf\";Integrated Security=True";
            if (nivel == 1)
            {
                btnModificar.Visible = false;
                label6.Visible = false;
                txtNivel.Visible = false;
                dataGridView1.Visible = false;
                this.Size = new System.Drawing.Size(334, 296);
                btnIngresar.Location = new System.Drawing.Point(165, 180);
            }
            else
            {
                this.Size = new System.Drawing.Size(717, 336);
            }
            actualizarTabla();
        }

        void actualizarTabla()
        {
            SqlConnection con = new SqlConnection(ruta);

            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select * from PERFILESDavidRangel");
            SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            dataGridView1.DataSource = datos;

            dataGridView1.Columns[0].HeaderText = "Rut";
            dataGridView1.Columns[1].HeaderText = "Nombre";
            dataGridView1.Columns[2].HeaderText = "Apellido Paterno";
            dataGridView1.Columns[3].HeaderText = "Apellido Materno";
            dataGridView1.Columns[4].HeaderText = "Clave";
            dataGridView1.Columns[5].HeaderText = "Nivel";

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
            }

            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (nivel == 1)
            {
                txtNivel.Text = "1";
            }
            SqlConnection con = new SqlConnection(ruta);
            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select * from PERFILESDavidRangel");
            SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            bool camposVacios = false;

            
            foreach (TextBox textBox in arrTextBoxs)
            {
                if (textBox.Text == String.Empty)
                {
                    camposVacios = true;
                }
            }

            if (!camposVacios)
            {
                string rut = Op.ponerMinusculas(txtRut.Text.PadLeft(10, '0'));
                string nombre = txtNombre.Text;
                string paterno = txtPaterno.Text;
                string materno = txtMaterno.Text;
                
                string clave = nombre[0].ToString() + paterno[0].ToString() + materno[0].ToString() + rut;
                int nivelIngresar = nivel == 2 ? int.Parse(txtNivel.Text) : 1;

                bool usuarioExiste = false;

                if (Op.validarRut(rut)) // Rut es correcto?
                {
                    foreach (DataRow fila in datos.Rows)
                    {
                        if (fila[0].ToString() == rut)
                        {
                            usuarioExiste = true;
                            MessageBox.Show("Usuario ya ingresado");
                        }
                    }

                    if (!usuarioExiste)
                    {
                        con.Open();
                        datos = new DataTable();
                        setencia = String.Format("insert into PERFILESDavidRangel (rut, nombre, appat, apmat, clave, nivel) values ('{0}','{1}','{2}','{3}','{4}',{5})", rut,nombre,paterno,materno,clave,nivelIngresar);
                        dataAdapter = new SqlDataAdapter(setencia, con);
                        dataAdapter.Fill(datos);
                        con.Close();
                        MessageBox.Show("Usuario ingresado");
                        actualizarTabla();
                    }
                }
                arrTextBoxs.ToList().ForEach((x) => x.Clear());
            }


        }

        private void validarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rut = Op.ponerMinusculas(toolStripTextBox1.Text.PadLeft(10, '0'));
            toolStripTextBox1.Text = rut;
            if (rut != String.Empty)
            {
                SqlConnection con = new SqlConnection(ruta);

                con.Open();
                DataTable datos = new DataTable();
                string setencia = String.Format("select rut, nivel from PERFILESDavidRangel");
                SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
                dataAdapter.Fill(datos);
                con.Close();

                foreach (DataRow fila in datos.Rows)
                {
                    if (fila[0].ToString() == rut)
                    {
                        nivel = int.Parse(fila[1].ToString());
                    }
                }

                if (nivel == 2)
                {
                    btnModificar.Visible = true;
                    label6.Visible = true;
                    txtNivel.Visible = true;
                    dataGridView1.Visible = true;
                    this.Size = new System.Drawing.Size(717, 336);
                    btnIngresar.Location = new System.Drawing.Point(78, 204);
                    MessageBox.Show("Permisos de Admin");
                    toolStripTextBox1.Enabled = false;
                    validarToolStripMenuItem.Enabled = false;
                    volverToolStripMenuItem.Visible = true;
                }
                else
                {
                    MessageBox.Show("Error");
                    toolStripTextBox1.Clear();
                }
            }
        }

        private void volverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nivel == 2)
            {
                btnModificar.Visible = false;
                label6.Visible = false;
                txtNivel.Visible = false;
                dataGridView1.Visible = false;
                this.Size = new System.Drawing.Size(334, 296);
                btnIngresar.Location = new System.Drawing.Point(165, 180);
                nivel = 1;
                toolStripTextBox1.Enabled = true;
                validarToolStripMenuItem.Enabled = true;
                volverToolStripMenuItem.Visible = false;
                toolStripTextBox1.Clear();
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro desea eliminar este registro?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string rut = (dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

                if (rut == toolStripTextBox1.Text)
                {
                    MessageBox.Show("No te puedes borrar a ti mismo");
                }
                else
                {
                    SqlConnection con = new SqlConnection(ruta);
                    con.Open();
                    DataTable datos = new DataTable();
                    string setencia = String.Format("delete from PERFILESDavidRangel where rut = '{0}'", rut);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
                    dataAdapter.Fill(datos);
                    con.Close();
                    actualizarTabla();
                }                          
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {               
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                for (int i = 0; i < fila.Cells.Count; i++)
                {
                    DataGridViewCell celda = fila.Cells[i];

                    if (i != 4)
                    {
                        if (i == 5)
                        {
                            arrTextBoxs[4].Text = celda.Value.ToString();
                        }
                        else
                        {
                            arrTextBoxs[i].Text = celda.Value.ToString();
                        }
                    }                   
                }
            }
        }

        private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arrTextBoxs.ToList().ForEach((x) => x.Clear());
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {          
            bool camposVacios = false;


            foreach (TextBox textBox in arrTextBoxs)
            {
                if (textBox.Text == String.Empty)
                {
                    camposVacios = true;
                }
            }

            if (!camposVacios)
            {
                string rut = Op.ponerMinusculas(txtRut.Text.PadLeft(10, '0'));
                string nombre = txtNombre.Text;
                string paterno = txtPaterno.Text;
                string materno = txtMaterno.Text;

                string clave = nombre[0].ToString() + paterno[0].ToString() + materno[0].ToString() + rut;
                int nivelIngresar = int.Parse(txtNivel.Text);


                if (Op.validarRut(rut)) // Rut es correcto?
                {
                    bool usuarioExiste = false;

                    SqlConnection con = new SqlConnection(ruta);
                    con.Open();
                    DataTable datos = new DataTable();
                    string setencia = String.Format("select * from PERFILESDavidRangel");
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(setencia, con);
                    dataAdapter.Fill(datos);
                    con.Close();

                    foreach (DataRow fila in datos.Rows)
                    {
                        if (fila[0].ToString() == rut)
                        {
                            usuarioExiste = true;
                        }
                    }
                    
                    if (usuarioExiste)
                    {
                        bool confirmacion = MessageBox.Show("¿Seguro que desea modificar este perfil " + rut + "?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes;

                        if (confirmacion)
                        {
                            con.Open();
                            datos = new DataTable();
                            setencia = String.Format("update PERFILESDavidRangel set nombre='{1}', appat='{2}',apmat='{3}',clave='{4}',nivel={5} where rut = '{0}'", rut, nombre, paterno, materno, clave, nivelIngresar);
                            dataAdapter = new SqlDataAdapter(setencia, con);
                            dataAdapter.Fill(datos);
                            con.Close();
                            MessageBox.Show("Usuario modificado");
                            actualizarTabla();

                            bool modificasTuPerfil = rut == toolStripTextBox1.Text;
                            if (modificasTuPerfil && nivelIngresar == 1)
                            {
                                MessageBox.Show("Ya no tienes permisos de admin");
                                volverToolStripMenuItem.PerformClick();
                            }
                        }
                    }
                    else if(!usuarioExiste)
                    {
                        MessageBox.Show("Rut no existe");
                    }
                }
                arrTextBoxs.ToList().ForEach((x) => x.Clear());
            }
        }
    }
}
