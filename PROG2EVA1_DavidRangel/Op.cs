using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1_DavidRangel
{
    static internal class Op
    {

        //! 18 - 05:
        //! Se creó esta clase estática que va a contener el método validarRut
        //! No se pueden crear instancias de una clase estática
        //! Se creó para que el método validarRut esté en el Form1 y PantallaLog
     
        public static bool validarRut(string rut)
        {
            int[] constantes = { 3, 2, 7, 6, 5, 4, 3, 2 };
            bool rutValido = false;
            while (rut.Length < 10)
            {
                rut = "0" + rut;
            }

            bool caracteresCorrectos = validarCaracteres(rut);

            if (caracteresCorrectos)
            {
                if (rut.Length > 10)
                {
                    MessageBox.Show("No puede ingresar más de 10 caracteres");
                }
                else
                {
                    double suma = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int valorActual = int.Parse(rut[i].ToString());
                        suma += constantes[i] * valorActual;
                    }
                    double division = suma / 11.0;
                    double decimales = division - (int)division;
                    double digitoNumerico = Math.Round(11 - (11 * decimales));
                    char digito;
                    if (digitoNumerico == 11)
                    {
                        digito = '0';
                    }
                    else if (digitoNumerico == 10)
                    {
                        digito = 'k';
                    }
                    else
                    {
                        digito = Convert.ToChar(digitoNumerico.ToString());
                    }

                    if (digito == rut[9])
                    {

                        rutValido = true;
                    }
                    else if (digito == 'k' && rut[9] == 'K')
                    {

                        rutValido = true;
                    }
                    else
                    {

                        MessageBox.Show("Rut incorrecto. El digito verificador tiene que ser: " + digito);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe colocar guión\nDebe colocar caracteres entre 0-9, o K (mayuscula o minuscula) si es el digito verificador");
            }
            return rutValido;
        }

        public static bool validarCaracteres(string rut)
        {
            bool correcto = true;

            if (rut[8] != '-')
            {
                correcto = false;
            }
            else
            {
                for (int i = 0; i < rut.Length; i++)
                {
                    if (i != 8)
                    {
                        char charActual = rut[i];
                        if (!((charActual >= '0' && charActual <= '9') ||
                            (i == 9 && (charActual == 'k' || charActual == 'K'))))
                        {
                            correcto = false;
                        }
                    }
                }
            }

            return correcto;
        }

        public static string ponerMinusculas(string texto)
        {
            string nuevoTexto = "";
            foreach (char caracter in texto)
            {
                char caracterEnMayuscula = ' ';
                if (caracter >= 65 && caracter <= 90)
                {
                    caracterEnMayuscula = Convert.ToChar((int)caracter + 32);
                }
                else
                {
                    caracterEnMayuscula = caracter;
                }
                nuevoTexto += caracterEnMayuscula;
            }
            return nuevoTexto;
        }

        public static string parseDateTime(DateTime dateTime, int op)
        {
            string fecha = "";
            if (op == 1)
            {
                fecha = dateTime.Year + "-" + dateTime.Month.ToString("00") + "-" + dateTime.Day.ToString("00") + " " + dateTime.Hour.ToString("00") + ":" + dateTime.Minute.ToString("00") + ":" + dateTime.Second.ToString("00") + "." + (dateTime.Millisecond).ToString("000");
            }else if (op == 2)
            {
                fecha = dateTime.ToString("dd-MM-yyyy hh:mm:ss.fff");
            }
            else if (op == 3)
            {
                fecha = dateTime.ToString("dd-MM-yyyy hh:mm:ss");
            }
            return fecha;
        }
    }
}
