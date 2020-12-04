using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad_5___Final___Servidor
{
    public class Cargamento:INotifyPropertyChanged
    {
        private string codigo;
        private string nomProd;
        private string talla;
        private int cantidad;
        private string fechaLlegada;

        public string Codigo 
        {
            get => codigo; 
            set
            {
                codigo = value;
                LanzarEvento("Codigo");
            }
        }
        public string NombreProducto 
        {
            get => nomProd;
            set
            {
                nomProd = value;
                LanzarEvento("NombreProducto");
            }
        }
        public string Talla 
        {
            get => talla;
            set
            {
                talla = value;
                LanzarEvento("Talla");
            }
        }
        public int Cantidad 
        {
            get => cantidad;
            set
            {
                cantidad = value;
                LanzarEvento("Cantidad");
            }
        }
        public string FechaLlegada 
        {
            get => fechaLlegada;
            set
            {
                fechaLlegada = value;
                LanzarEvento("FechaLlegada");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void LanzarEvento(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
