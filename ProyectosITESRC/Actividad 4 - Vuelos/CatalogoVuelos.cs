using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Actividad_4___Vuelos
{
    public class CatalogoVuelos
    {
        public ObservableCollection<VueloDatos> Vuelos { get; set; } = new ObservableCollection<VueloDatos>();

        public void AgregarVuelo(VueloDatos v)
        {
            Vuelos.Add(v);
            Guardar();
        }

        public void EditarVuelo(VueloDatos v)
        {
            Vuelos.Add(v);
            Guardar();
        }

        public void EliminarVuelo(VueloDatos v)
        {
            Vuelos.Add(v);
            Guardar();
        }

        public void Guardar()
        {
            string datosVuelos = JsonConvert.SerializeObject(Vuelos);
            File.WriteAllText("vuelos.json", datosVuelos);
        }

        private void Cargar()
        {
            if(File.Exists("vuelos.json"))
            {
                var vueloNuevo = JsonConvert.DeserializeObject<ObservableCollection<VueloDatos>>(File.ReadAllText("vuelos.json"));

                foreach (var v in vueloNuevo)
                {
                    Vuelos.Add(v);
                }
            }
        }

        public CatalogoVuelos()
        {
            Cargar();
        }
    }
}
