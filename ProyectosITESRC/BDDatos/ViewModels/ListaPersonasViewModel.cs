using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BDDatos.Models;
using BDDatos.Repositories;
using GalaSoft.MvvmLight.Command;

namespace BDDatos.ViewModels
{
    public class ListaPersonasViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<persona> Personas { get; set; } = new ObservableCollection<persona>();
        private int hombres;

        public int Hombres
        {
            get { return hombres; }
            set
            {
                hombres = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hombres"));
            }
        }
        private int mujeres;

        public int Mujeres
        {
            get { return mujeres; }
            set
            {
                mujeres = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mujeres"));
            }
        }


        public ICommand Eliminar { get; set; }
        public Func<string, bool> Confirmar;

        public event PropertyChangedEventHandler PropertyChanged;

        public ListaPersonasViewModel()
        {
            Eliminar = new RelayCommand<persona>(EliminarPersona);
        }

        public void EliminarPersona(persona p) //Para comunicar con el 
        {
            if(p!=null)
            {
                if(Confirmar?.Invoke(p.nombre)==true)
                {
                    PersonasRepository per = new PersonasRepository();
                    per.Delete(p);
                    Cargar();
                }
            }
        }

        public void Cargar()
        {
            
            PersonasRepository datosPersona = new PersonasRepository();
            Personas.Clear();
            foreach (var item in datosPersona.GetAll())
            {
                Personas.Add(item);
            }
            Hombres = Personas.Count(x => x.genero == 0);
            Mujeres = Personas.Count(x => x.genero == 1);
        }
    }
}
