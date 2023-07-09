using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.IO;
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
            rut = "";
            nivel = 2;
        }
        public Perfiles(string rut, int nivel)
        {
            this.rut = rut;
            this.nivel = nivel;
            InitializeComponent();
        }

        string rut;
        string rutaBDD = "Server=127.0.0.1;User=root;Database=BDDPROG2DavidRangel;password=''";
        int nivel;
        TextBox[] arrTextBoxs;
        private void Perfiles_Load(object sender, EventArgs e)
        {
            arrTextBoxs = new TextBox[5] { txtRut, txtNombre, txtPaterno, txtMaterno, txtNivel };
                            
            if (nivel == 2)
            {
                groupBox1.Visible = false;
                txtBusqueda.Visible = false;
                btnEliminar.Visible = false;
                button1.Visible = false;
                btnModificar.Visible = false;
                label6.Visible = false;
                txtNivel.Visible = false;
                dataGridView1.Visible = false;
                this.Size = new System.Drawing.Size(420, 296);
                btnIngresar.Location = new System.Drawing.Point(165, 180);
            }
            else
            {
                groupBox1.Visible = true;
                txtBusqueda.Visible = true;

                mostrarTodoToolStripMenuItem.Visible = true;
                btnEliminar.Visible = true;
                button1.Visible = true;
                btnModificar.Visible = true;
                label6.Visible = true;
                txtNivel.Visible = true;
                dataGridView1.Visible = true;
                
                            
            }
            actualizarTabla();
        }

        void actualizarTabla()
        {
            MySqlConnection con = new MySqlConnection(rutaBDD);

            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select * from PERFILESDavidRangel");
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            dataGridView1.DataSource = datos;

            dataGridView1.Columns[0].HeaderText = "Rut";
            dataGridView1.Columns[1].HeaderText = "Nombre";
            dataGridView1.Columns[2].HeaderText = "Apellido Pat";
            dataGridView1.Columns[3].HeaderText = "Apellido Mat";
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
            if (nivel == 2)
            {
                txtNivel.Text = "2";
            }
            MySqlConnection con = new MySqlConnection(rutaBDD);
            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select * from PERFILESDavidRangel");
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
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
                int nivelIngresar = nivel != 2 ? int.Parse(txtNivel.Text) : 2;

                bool usuarioExiste = false;

                if (Op.validarRut(rut)) // Rut es correcto?
                {
                    foreach (DataRow fila in datos.Rows)
                    {
                        if (fila[0].ToString() == rut)
                        {
                            usuarioExiste = true;
                            MessageBox.Show("Usuario ya existe en la BDD");
                        }
                    }

                    if (!usuarioExiste)
                    {
                        con.Open();
                        datos = new DataTable();
                        setencia = String.Format("insert into PERFILESDavidRangel (rut, nombre, appat, apmat, clave, nivel) values ('{0}','{1}','{2}','{3}','{4}',{5})", rut,nombre,paterno,materno,clave,nivelIngresar);
                        dataAdapter = new MySqlDataAdapter(setencia, con);
                        dataAdapter.Fill(datos);
                        con.Close();
                        MessageBox.Show("Usuario ingresado");
                        actualizarTabla();
                    }
                }
                arrTextBoxs.ToList().ForEach((x) => x.Clear());
            }
            else
            {
                MessageBox.Show("Campos vacíos!");
            }
        }
            
        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eliminarRegistro(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
        }

        void eliminarRegistro(string clave)
        {
            if (MessageBox.Show("¿Seguro desea eliminar este perfil? Se van a eliminar el registro de sus acciones", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {             
                if (clave.Substring(3) == this.rut)
                {
                    MessageBox.Show("No te puedes borrar a ti mismo");
                }
                else
                {
                    MySqlConnection con = new MySqlConnection(rutaBDD);

                    con.Open();
                    DataTable datos = new DataTable();
                    string setencia = String.Format("delete from ACCIONESDavidRangel where clave = '{0}'", clave);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
                    dataAdapter.Fill(datos);
                    con.Close();
                   
                    con.Open();
                    datos = new DataTable();
                    setencia = String.Format("delete from PERFILESDavidRangel where clave = '{0}'", clave);
                    dataAdapter = new MySqlDataAdapter(setencia, con);
                    dataAdapter.Fill(datos);
                    con.Close();
                    actualizarTabla();
                }
            }
            arrTextBoxs.ToList().ForEach((x) => x.Clear());
            txtBusqueda.Clear();
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

                    MySqlConnection con = new MySqlConnection(rutaBDD);
                    con.Open();
                    DataTable datos = new DataTable();
                    string setencia = String.Format("select * from PERFILESDavidRangel");
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
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
                            setencia = String.Format("select * from ACCIONESDavidRangel where clave = (select clave from PERFILESDavidRangel where rut = '{0}')", rut);
                            dataAdapter = new MySqlDataAdapter(setencia, con);
                            dataAdapter.Fill(datos);
                            con.Close();

                            if (datos.Rows.Count > 0)
                            {
                                con.Open();
                                DataTable eliminarDatos = new DataTable();
                                setencia = String.Format("delete from ACCIONESDavidRangel where clave = (select clave from PERFILESDavidRangel where rut = '{0}')", rut);
                                dataAdapter = new MySqlDataAdapter(setencia, con);
                                dataAdapter.Fill(eliminarDatos);
                                con.Close();

                                con.Open();
                                DataTable modificarDatos = new DataTable();
                                setencia = String.Format("update PERFILESDavidRangel set nombre='{1}', appat='{2}',apmat='{3}',clave='{4}',nivel={5} where rut = '{0}'", rut, nombre, paterno, materno, clave, nivelIngresar);
                                dataAdapter = new MySqlDataAdapter(setencia, con);
                                dataAdapter.Fill(modificarDatos);
                                con.Close();

                                foreach (DataRow fila in datos.Rows)
                                {
                                    fila[1] = clave;
                                    con.Open();

                                    DataTable insertarDatos = new DataTable();
                                    setencia = String.Format("SET IDENTITY_INSERT accionesdavidrangel on insert into ACCIONESDavidRangel (num, clave, iniciosesion, finsesion, accion, accionf) values " +
                                        "({0},'{1}', '{2}', '{3}', '{4}', '{5}') SET IDENTITY_INSERT accionesdavidrangel off", fila[0], fila[1], fila[2], fila[3], fila[4], fila[5]);
                                    dataAdapter = new MySqlDataAdapter(setencia, con);
                                    dataAdapter.Fill(insertarDatos);
                                    con.Close();
                                }                   
                                formatearCampoClave();
                            }
                            else
                            {
                                con.Open();
                                DataTable modificarDatos = new DataTable();
                                setencia = String.Format("update PERFILESDavidRangel set nombre='{1}', appat='{2}',apmat='{3}',clave='{4}',nivel={5} where rut = '{0}'", rut, nombre, paterno, materno, clave, nivelIngresar);
                                dataAdapter = new MySqlDataAdapter(setencia, con);
                                dataAdapter.Fill(modificarDatos);
                                con.Close();
                            }

                            MessageBox.Show("Usuario modificado");
                            actualizarTabla();

                            if (rut == this.rut && nivelIngresar == 2)
                            {
                                MessageBox.Show("Ya no tienes permisos");
                                this.DialogResult = DialogResult.No;
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

        

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(rutaBDD);
            con.Open();
            DataTable datos = new DataTable();
            string setencia = String.Format("select * from PERFILESDavidRangel where clave = '{0}'", txtBusqueda.Text);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            if (datos.Rows.Count > 0)
            {
                eliminarRegistro(txtBusqueda.Text);
            }
            else
            {
                MessageBox.Show("Perfil no existe. Revise las mayúsculas o minúsculas.");
            }
            arrTextBoxs.ToList().ForEach((x) => x.Clear());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(rutaBDD);
            con.Open();
            DataTable datos = new DataTable();

            string selects = claveOApPat ?
                "select * from PERFILESDavidRangel where appat like '%{0}%'"
                :
                "select * from PERFILESDavidRangel where clave = '{0}'";

            string setencia = String.Format(selects, txtBusqueda.Text);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
            dataAdapter.Fill(datos);
            con.Close();

            if (datos.Rows.Count > 0)
            {
                colocarDatosTabla(datos);
            }
            else
            {
                MessageBox.Show("Perfiles no existen");
            }
            arrTextBoxs.ToList().ForEach((x) => x.Clear());
        }

        private void mostrarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actualizarTabla();
        }

        void colocarDatosTabla(DataTable datos)
        {
            dataGridView1.DataSource = datos;
            dataGridView1.Columns[0].HeaderText = "Rut";
            dataGridView1.Columns[1].HeaderText = "Nombre";
            dataGridView1.Columns[2].HeaderText = "Apellido Pat";
            dataGridView1.Columns[3].HeaderText = "Apellido Mat";
            dataGridView1.Columns[4].HeaderText = "Clave";
            dataGridView1.Columns[5].HeaderText = "Nivel";

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].ContextMenuStrip = contextMenuStrip1;
            }
        }

        string rutaArchivo = @"C:\TXTS\VIGIADAVIDRANGEL.txt";
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


            MySqlConnection con = new MySqlConnection(rutaBDD);
            con.Open();
            DataTable perfiles = new DataTable();
            string setencia = String.Format("select Rut, Nombre, ApPat, ApMat from PERFILESDavidRangel");
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(setencia, con);
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

                    if ((rut == registro[0]) || (registro[0].Length == 13 && rut == registro[0].Substring(3)))
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

        //true -> ApPat,
        //false -> clave
        bool claveOApPat = true;
        private void claveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            claveToolStripMenuItem.BackColor = Color.Gray;
            claveToolStripMenuItem.ForeColor = Color.White;

            ApPatToolStripMenuItem.BackColor = Color.White;
            ApPatToolStripMenuItem.ForeColor = Color.Black;
            claveOApPat = false;
        }

        private void ApPatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            claveOApPat = true;
            ApPatToolStripMenuItem.BackColor = Color.Gray;
            ApPatToolStripMenuItem.ForeColor = Color.White;

            claveToolStripMenuItem.BackColor = Color.White;
            claveToolStripMenuItem.ForeColor = Color.Black;
        }
    }
}
