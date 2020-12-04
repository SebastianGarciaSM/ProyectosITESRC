using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDDatos.Models;

namespace BDDatos.Repositories
{
    public class PersonasRepository
    {
        bddatosEntities contexto = new bddatosEntities();
        public IEnumerable<persona> GetAll()
        {
            return contexto.persona.OrderBy(x => x.nombre);
        }

        public void Add(persona p)
        {
            //Vienen validados los datos
            contexto.persona.Add(p);
            contexto.SaveChanges();
        }

        public void Delete(persona p)
        {
            //Verificar que p no sea null
            //Si está en la bd borrarlo
            var per = contexto.persona.FirstOrDefault(x=>x.idPersona==p.idPersona);
            if (per!=null)
            {
                contexto.persona.Remove(per);
                contexto.SaveChanges();
            }
        }

        public void Update(persona p)
        {
            var per = contexto.persona.FirstOrDefault(x => x.idPersona == p.idPersona);
            if (per != null)
            {
                per.nombre = p.nombre;
                per.genero = p.genero;
                per.edad = p.edad;
                contexto.SaveChanges();
            }
        }

        public persona GetByID(int id)
        {
            return contexto.persona.FirstOrDefault(x => x.idPersona == id);
        }
    }
}
