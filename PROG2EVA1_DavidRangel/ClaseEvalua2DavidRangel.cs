using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG2EVA1_DavidRangel
{
    internal class ClaseEvalua2DavidRangel
    {
        private string rut;
        private DateTime inicioSesion;
        private DateTime finSesion;
        private string accion;
        private DateTime accionF;

        public ClaseEvalua2DavidRangel(string rut, DateTime inicioSesion, DateTime finSesion, string accion, DateTime accionF)
        {
            this.rut = rut;
            this.inicioSesion = inicioSesion;
            this.finSesion= finSesion;
            this.accion = accion;
            this.accionF = accionF;
        }

        public ClaseEvalua2DavidRangel(string rut, DateTime inicioSesion)
        {
            this.rut = rut;
            this.inicioSesion = inicioSesion;
            this.finSesion = DateTime.MinValue;
            this.accion = "";
            this.accionF = DateTime.MinValue;
        }

        public string getRut() { return this.rut; }
        public DateTime getInicio() { return this.inicioSesion; }
        public DateTime getFin() { return this.finSesion; }
        public string getAccion() { return this.accion; }
        public DateTime getAccionF() { return this.accionF; }

        public void setRut(string p) { this.rut = p; }
        public void setInicioSesion(DateTime p) { this.inicioSesion = p; }
        public void setFinSesion(DateTime p) { this.finSesion = p; }
        public void setAccion(string p) { this.accion = p; }
        public void setAccionF(DateTime p) { this.accionF = p; }

    }
}
