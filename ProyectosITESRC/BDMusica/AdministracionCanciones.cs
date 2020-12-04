using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using MySql.Data.MySqlClient;

namespace BDMusica
{
    public class AdministracionCanciones
    {
        public ObservableCollection<Cancion> ListaCanciones { get; set; }

        private ObservableCollection<Artista> listaArtistas;
        public ObservableCollection<Artista> ListaArtistas
        {
            get { return listaArtistas; }
        }
        public ObservableCollection<Artista> ObtenerListaArtistas()
        {
            Conectar();
            comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select * from artista";
            listaArtistas = new ObservableCollection<Artista>();
            lector = comando.ExecuteReader();
            while (lector.Read())
            {
                Artista a = new Artista()
                {
                    IdArtista = Convert.ToInt32(lector["idArtista"]),
                    NombreArtista = (string)lector["nombre"],
                    ApMaterno = (string)lector["apMaterno"],
                    ApPaterno = (string)lector["apPaterno"],
                    PaisOrigen = Convert.ToInt32(lector["paisOrigen"])
                };
                listaArtistas.Add(a);
            }
            lector.Close();
            return listaArtistas;
        }

        private ObservableCollection<Categoria> listaCategorias;
        public ObservableCollection<Categoria> ListaCategorias
        {
            get { return listaCategorias; }
        }
        public ObservableCollection<Categoria> ObtenerListaCategorias()
        {
            Conectar();
            comando = new MySqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select * from categoria";
            listaCategorias = new ObservableCollection<Categoria>();
            lector = comando.ExecuteReader();
            while (lector.Read())
            {
                Categoria c = new Categoria()
                {
                    IdCategoria=Convert.ToInt32(lector["idCategoria"]),
                    NombreCategoria=(string)lector["nombre"]
                };
                listaCategorias.Add(c);
            }
            lector.Close();
            return listaCategorias;
        }


        private MySqlConnection conexion;

        private MySqlCommand comando;

        private MySqlDataReader lector;

        public void Conectar()
        {
            conexion = new MySqlConnection("server=localhost;user=root;password=root;database=musica");
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }
        }

        ~AdministracionCanciones()
        {
            if (conexion.State != ConnectionState.Closed)
                conexion.Close();
        }

        public AdministracionCanciones()
        {

            //  //

            Conectar();
            comando = new MySqlCommand();
            comando.Connection = conexion;
            //comando.CommandText = "select * from cancion";
            
            ListaCanciones = new ObservableCollection<Cancion>();
            lector = comando.ExecuteReader();
            while (lector.Read())
            {
                Cancion c = new Cancion()
                {
                    IdCancion = (int)lector["idCancion"],
                    NombreCancion=(string)lector["nombre"],
                    Duracion=(string)lector["duracion"],
                    Album=(string)lector["album"],
                    IdCategoria=(int)lector["idCategoria"],
                    IdArtista=(int)lector["idArtista"]
                };
                ListaCanciones.Add(c);
            }
            lector.Close();
        }

        public void Agregar(Cancion c)
        {
            comando = new MySqlCommand(String.Format("select count(*) from cancion where nombre='{0}'", c.NombreCancion), conexion);
            int resultado = Convert.ToInt32(comando.ExecuteScalar());
            if (resultado > 0)
            {
                throw new ArgumentException("Esta canción ya se encuentra registrada.");
            }
            comando.CommandText = string.Format ("insert into cancion(nombre, duracion, album, idCategoria, idArtista) values('{0}','{1}','{2}',{3},{4})", c.NombreCancion, c.Duracion, c.Album, c.IdCategoria, c.IdArtista);
            comando.ExecuteNonQuery();
            ListaCanciones.Add(c);
        }

        public void Eliminar(Cancion c)
        {
            comando = new MySqlCommand(String.Format("select count(*) from cancion where nombre='{0}'", c.NombreCancion), conexion);

            int resultado = Convert.ToInt32(comando.ExecuteScalar());
            if (resultado > 0)
            {
                comando.CommandText = string.Format("delete from cancion where idCancion={0}", c.IdCancion);
                comando.ExecuteNonQuery();
                ListaCanciones.Remove(c);
            }
            else
            {
                throw new ArgumentException("La canción no se encuentra en la base de datos.");
            }
        }

        public void Editar(Cancion c)
        {
            comando = new MySqlCommand(String.Format("select count(*) from cancion where nombre='{0}'", c.NombreCancion), conexion);
            int resultado = Convert.ToInt32(comando.ExecuteScalar());
            comando.CommandText = string.Format("update cancion set nombre='{0}', duracion='{1}', album='{2}', idCategoria={3}, idArtista={4} where idCancion={5}", c.NombreCancion, c.Duracion, c.Album, c.IdCategoria, c.IdArtista, c.IdCancion);
            comando.ExecuteNonQuery();
        }
    } 
}
