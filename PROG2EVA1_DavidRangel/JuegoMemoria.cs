using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{

    public partial class JuegoMemoria : Form
    {
        public JuegoMemoria()
        {
            InitializeComponent();
        }
        int[,] matriz;
        PictureBox[,] OnePiece;
        int contadorBtn2 = 0;
        //! 18-05: se cambió el tipo de dato que almacena la lista
        List<int> seleccionados = new List<int>(); // se crea lista tipo int con los seleccionados.

        //! 18-05: variable global rut para recibir el rut del form1
        string rut;

        //! 18-05: objeto global que va a recibir todos los cambios
        ClaseEvalua2DavidRangel usuarioActual;

        //! 18-05: lista de objetos del tipo de la clase creada
        List<ClaseEvalua2DavidRangel> ListaEvalua2DavidRangel;

        //! 18-05: ruta del archivo txt
        string ruta = @"C:\TXTS\VIGIADAVIDRANGEL.txt";

        //! 18-05: numero de coincidencias conseguidas
        int coincidencias = 0;
         
        //! 18-05: sobrecarga del constructor del formulario JuegoMemoria
        public JuegoMemoria(string rut)
        {
            InitializeComponent();
            //! 18-05: se asgina el parametro recibido del constructor en la variable global
            this.rut = rut;           
        }

        /*! 18-05: 
         * Esta funcion se encarga de enviar el evento hacia la lista
         * recibe una accion y un booleano que determina si es el fin de sesion
         * si finSesion es igual a true, el atributo finSesion del usuarioActual se setea a Now
         * si es false, se deja como estaba
         */
        public void enviarEvento(string accion, bool finSesion)
        {
            if (finSesion)
            {
                usuarioActual.setFinSesion(DateTime.Now);
            }
            usuarioActual.setAccion(accion);
            usuarioActual.setAccionF(DateTime.Now);

            //! 18-05: se crea un nuevo objeto a partir de los cambios realizados en el usuario actual, y se aniade a la lista
            ListaEvalua2DavidRangel.Add(new ClaseEvalua2DavidRangel(usuarioActual.getRut(), usuarioActual.getInicio(), usuarioActual.getFin(), usuarioActual.getAccion(), usuarioActual.getAccionF()));
        }

        //! 18-05: funcion para cargar la informacion de la lista hacia el txt
        public void cargarInformacion()
        {
            StreamWriter sw = File.AppendText(ruta);
            //! 18-05: foreach para recorrer toda la lista 
            foreach (ClaseEvalua2DavidRangel usuario in ListaEvalua2DavidRangel)
            {      
                string fechaFin;
                //! 18-05: Si la fecha finSesion tiene el valor minimo, no se guarda nada en el txt
                if (usuario.getFin() == DateTime.MinValue)
                {
                    fechaFin = "";
                }
                else
                {
                    fechaFin = Op.parseDateTime(usuario.getFin(), 1);
                }
                /*! 18-05: 
                 * Format es una funcion que permite colocar los valores de varias variables dentro de un texto
                 * sin tener que utilizar el signo + de concatenacion
                 * en donde está el {0} -> se coloca usuario.getRut()
                 * {1} -> usuario.getInicio() 
                 * {2} -> fechaFin y así sucesivamente
                 */
               
                string registro = String.Format("{0};{1};{2};{3};{4}", usuario.getRut(), Op.parseDateTime(usuario.getInicio(), 1), fechaFin, usuario.getAccion(), Op.parseDateTime(usuario.getAccionF(), 1));
                //! 18-05: Se escribe el registro en el txt
                sw.WriteLine(registro);
            }
            sw.Close();
        }

        //! 18-05: El boton cerrar sesion llama al evento Close
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Thread thread;
        //! 18-05: Cuando Close es llamado, se envia el cierre de sesion y carga la informacion, se devuelve al form1
        private void JuegoMemoria_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }

            enviarEvento("Cierre de sesion", true);
            cargarInformacion();           
        }

        private void JuegoMemoria_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\imagenes\fondojuego.jpg");

            /*! 18-05: 
             * Cuando se carga formulario, se instancia la lista
             * Se instancia el usuario acual con el rut recibido del form1 y la fecha de inicio de sesion
             */
            ListaEvalua2DavidRangel = new List<ClaseEvalua2DavidRangel>();
            usuarioActual = new ClaseEvalua2DavidRangel(this.rut, DateTime.Now);

            enviarEvento("Inicio de sesion", false);

            //una vez que pase de la validacion, acá se crean las 2 matrices.
            matriz = new int[4, 4];
            OnePiece = new PictureBox[4, 4]
            {
                { pictureBox1, pictureBox2, pictureBox3, pictureBox4},
                { pictureBox5, pictureBox6, pictureBox7, pictureBox8 },
                { pictureBox9, pictureBox10, pictureBox11, pictureBox12 },
                { pictureBox13, pictureBox14, pictureBox15, pictureBox16 }
            };

            for (int fila = 0; fila < OnePiece.GetLength(0); fila++)
            {
                for (int columna = 0; columna < OnePiece.GetLength(1); columna++)
                {
                    OnePiece[fila, columna].Click += new EventHandler(picClick); 
                    OnePiece[fila, columna].Enabled = false; // inhabilitamos los pictureBox, para que al momento de click no presente error.
                }
            }
            label1.Visible = false;      
        }


        
        private void button2_Click(object sender, EventArgs e)
        {
            //! 18-05: Se resetea coincidencias porque se reinicia la partida cuando se click otra vez a Jugar
            coincidencias = 0;
            //! 18-05: Sumamos contador de clicks para el boton Jugar
            contadorBtn2++;
            //! 18-05: Si la cantidad de clicks es >= 2 quiere decir que reinició, sino está empezando una
            if (contadorBtn2 >= 2)
            {
                enviarEvento("Jugador reinicio la partida", false);
                
            }
            else
            {
                enviarEvento("Jugador comenzo una partida", false);             
            }
            

            label1.Text = "";
            
            Random random = new Random();
            
            int[] repetidos = new int[16]; //creacion array repetidos
            int posicion = 0; //variable para manejar la posicion y luego comparar.


            for (int fila = 0; fila < matriz.GetLength(0); fila++)
            {
                for (int columna = 0; columna < matriz.GetLength(1); columna++)
                {
                    int contador = 0;
                    int numAleatorio = 0;
                    bool cumple2veces = false;

                    do
                    {
                        contador = 0;
                        numAleatorio = random.Next(1, 9);

                        for (int i = 0; i < repetidos.Length; i++) //recorre array de repetidos
                        {
                            if (repetidos[i] == numAleatorio) //compara si numero que esta en i posicion de repetidos es igual a numAleatorio.
                            {
                                contador++;
                            }
                        }
                        if (contador >= 2)
                        {
                            cumple2veces = true;
                        }
                        else
                        {
                            cumple2veces = false;
                        }

                    } while (cumple2veces == true); 

                    repetidos[posicion] = numAleatorio;
                    posicion++;

                    matriz[fila, columna] = numAleatorio;

                    label1.Text += matriz[fila, columna].ToString().PadLeft(5) + " ";
                }
                label1.Text += "\n";
            }

            for (int fila = 0; fila < OnePiece.GetLength(0); fila++) 
            {
                for (int columna = 0; columna < OnePiece.GetLength(1); columna++)
                {
                    string image = Application.StartupPath + @"\imagenes\default.jpg"; //busca una imagen y la guarda en variable ruta.

                    OnePiece[fila, columna].Image = Image.FromFile(image); //carga la imagen en cada pictureBox.
                    OnePiece[fila, columna].Tag = false; //el .tag lo ulitizamos para controlar si se puede seleccionar la imagen.
                    OnePiece[fila, columna].Enabled = true;

                }
            }
            seleccionados.Clear(); // limpia la lista seleccionados
        }

        private void picClick(object sender, EventArgs e)
        {

            PictureBox picClickeado = (PictureBox)sender;
            int filaClick = 0;
            int colClick = 0;

            for (int fila = 0; fila< OnePiece.GetLength(0); fila++)
            {
                for (int col = 0; col < OnePiece.GetLength(1); col++)
                {
                    if (picClickeado.Name == OnePiece[fila, col].Name) 
                        /*cuando el jugador haga click, se guardará el nombre y lo comparara con el nombre de la matriz One Piece
                         */
                    {
                        filaClick = fila;
                        colClick = col;

                       /*guardará la posicion de click en filaclick & columnaClick*/
                    }
                }
            }


            if (Convert.ToBoolean(OnePiece[filaClick, colClick].Tag) == false)
            {
                //! 18-05: Antes este valor era un string, se cambió para un int. Cabe destacar que la variable numero guarda el numero clickeado
                int numero = matriz[filaClick, colClick];
                /*! 18-05: 
                 * Array de los nombres de los personajes, ordenados de la misma manera como están las imagenes en la carpeta
                 * 0(1.jpg) -> Luffy
                 * 1(2.jpg) -> Zoro
                 * 2(3.jpg) -> Nami
                 * ...
                 */
                string[] personajes = { "Luffy", "Zoro", "Nami", "Usopp", "Sanji", "Chopper", "Nico Robin", "Franky"};
                
                //! 18-05: La carta jugada será el personaje ubicado en la posicion que indique el número-1 seleccionado
                string cartaJugada = personajes[numero-1];
                enviarEvento("Jugador hizo click a " + cartaJugada + ". En la posicion " + filaClick + ", " + colClick, false);

                string image = Application.StartupPath + @"\imagenes\" + numero + ".jpg"; // busca imagen con nombre de numero.
                OnePiece[filaClick, colClick].Image = Image.FromFile(image); //inserta la imagen


                OnePiece[filaClick, colClick].Tag = true; // cambia el control a True por ende queda visible y con la imagen que corresponde.
                seleccionados.Add(numero);// se agrega a la lista seleccionados.
                
                //! 18-05: si la cantidad de seleccionados llega a 2
                if (seleccionados.Count == 2)
                {
                    
                    if (seleccionados[0] == seleccionados[1]) //compara en lista seleccionados la primera posicion con la segunda
                    {
                        //! 18-05: si coinciden, se cuenta 
                        coincidencias++;
                        enviarEvento("Jugador encontro una pareja", false);

                        //! 18-05: si se encuentran todas las coincidencias
                        if (coincidencias == 8)
                        {
                            enviarEvento("Jugador encontro todas las parejas", false);
                        }
                    }
                    else
                    {
                        enviarEvento("Jugador se equivoco", false);

                        int num1 = seleccionados[0];
                        int num2 = seleccionados[1];

                        /*! 19-05:
                         * Thread es una clase que crea y controla un hilo.
                         * su constructor recibe una funcion (no su llamada) que va a ejecutar cuando se inicie (Start)
                         * en este caso, se colocó una función flecha que va a llamar a la función colocarImagenesDefault
                         */
                        thread = new Thread(() => colocarImagenesDefault(num1, num2));
                        thread.Start();

                        
                    }
                    seleccionados.Clear(); //limpiamos lista seleccionados.
                }
            }
        }  
        
        

        //! 19-05: Metodo para voltear las imagenes a su imagen default. Se utiliza el thread.sleep para detener la ejecucion del
        //! proceso (durante 200 milisegundos), de esta manera se le da ese tiempo al usuario para ver la segunda carta que le dio click
        void colocarImagenesDefault(int num1, int num2)
        {
            Thread.Sleep(200);

            for (int fila = 0; fila < OnePiece.GetLength(0); fila++)
            {
                for (int columna = 0; columna < OnePiece.GetLength(1); columna++)
                {
                    if (num1 == matriz[fila, columna] || num2 == matriz[fila, columna])
                    {
                        // en este for se busca identificar el segun click erroneo del jugador, a través de la comparación.
                        OnePiece[fila, columna].Tag = false; //cambia el control a false
                        OnePiece[fila, columna].Image = Image.FromFile(Application.StartupPath + @"\imagenes\default.jpg"); //inserta la imagen "default"
                    }
                }
            }
        }
        
    }
}
