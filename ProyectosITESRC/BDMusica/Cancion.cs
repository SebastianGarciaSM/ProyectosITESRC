using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMusica
{
    public class Cancion
    {
        public int IdCancion { get; set; }
        public string NombreCancion { get; set; }
        public string Duracion { get; set; }
        public string Album { get; set; }
        public int IdCategoria { get; set; }
        public int IdArtista { get; set; }
    }
}
