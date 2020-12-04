using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad_3___Tablero_Deportivo
{
    public class Datos:INotifyPropertyChanged
    {
        private int local;
        public int DatoLocal
        {
            get { return local; }
            set
            {
                local = value;
                OnPropertyChanged("DatoLocal");
            }
        }

        private int visita;
        public int DatoVisita
        {
            get { return visita; }
            set
            {
                visita = value;
                OnPropertyChanged("DatoVisita");
            }
        }

        private int periodo;
        public int DatoPeriodo
        {
            get { return periodo; }
            set
            {
                periodo = value;
                OnPropertyChanged("DatoPeriodo");
            }
        }

        private int faltasLocal;
        public int DatoFaltasLocal
        {
            get { return faltasLocal; }
            set
            {
                faltasLocal = value;
                OnPropertyChanged("DatoFaltasLocal");
            }
        }

        private int faltasVisita;
        public int DatoFaltasVisita
        {
            get { return faltasVisita; }
            set
            {
                faltasVisita = value;
                OnPropertyChanged("DatoFaltasVisita");
            }
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
