using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Actividad_5___Final___Servidor
{
    public class CatalogoRopa
    {
        public ObservableCollection<Cargamento> Prendas { get; set; } = new ObservableCollection<Cargamento>();

        public void Agregar(Cargamento c)
        {
            Prendas.Add(c);
            Guardar();
        }

        public void Editar(Cargamento c)
        {
            var car = Prendas.FirstOrDefault(x => x.Codigo == c.Codigo);
            if (car!= null)
            {
                car.Cantidad = c.Cantidad;
                car.FechaLlegada = c.FechaLlegada;
                car.NombreProducto = c.NombreProducto;
                car.Talla = c.Talla;
                Guardar();
            }
        }

        public void Eliminar(Cargamento c)
        {
            var car = Prendas.FirstOrDefault(x => x.Codigo == c.Codigo);
            if (car!=null)
            {
                Prendas.Remove(car);
                Guardar();
            }
        }

        public void Guardar()
        {
            string datos = JsonConvert.SerializeObject(Prendas);
            File.WriteAllText("cargamento.json", datos);
        }

        public void Cargar()
        {
            if (File.Exists("cargamento.json"))
            {
                var nuevo = JsonConvert.DeserializeObject<ObservableCollection<Cargamento>>
                    (File.ReadAllText("cargamento.json"));

                foreach (var item in nuevo)
                {
                    Prendas.Add(item);
                }
            }
        }

        public CatalogoRopa()
        {
            Cargar();
        }

    }
}
