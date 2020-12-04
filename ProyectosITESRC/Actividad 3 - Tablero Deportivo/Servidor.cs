using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Windows.Threading;
using System.Security.Policy;
using System.Net.Http;

namespace Actividad_3___Tablero_Deportivo
{
    public class Servidor
    {
        HttpListener listener;
        Dispatcher dispatcher;
        public Datos Datos { get; set; } = new Datos();

        public Servidor()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:1000/actividad3/");
            listener.Start();
            dispatcher = Dispatcher.CurrentDispatcher;
            listener.BeginGetContext(RecibirPeticiones, null);
        }

        private void RecibirPeticiones(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(RecibirPeticiones, null);

            var url = context.Request.Url.LocalPath;
            if(url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 2, 1);
            }

            if (context.Request.HttpMethod == "GET" && url == "/actividad3")
            {
                var index = System.IO.File.ReadAllBytes("index.html");
                
                context.Response.ContentType = "text/html";
                context.Response.OutputStream.Write(index, 0, index.Length);
                context.Response.OutputStream.Close();
            }
            else if(context.Request.HttpMethod=="POST" && url=="/actividad3")
            {
                StreamReader stream = new StreamReader(context.Request.InputStream);
                string variables = stream.ReadToEnd();
                var datos = HttpUtility.ParseQueryString(variables);
                IncrementarDato(datos["dato"]);
                context.Response.StatusCode = 200;
                context.Response.Redirect("/actividad3");
            }
            else
            {
                context.Response.StatusCode = 404;
            }
            context.Response.Close();
        }


        public void IncrementarDato(string dato)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                if(dato=="Local")
                {
                    Datos.DatoLocal++;
                }
                else if (dato == "Visita")
                {
                    Datos.DatoVisita++;
                }
                else if (dato == "Faltas Local")
                {
                    Datos.DatoFaltasLocal++;
                }
                else if (dato == "Faltas Visita")
                {
                    Datos.DatoFaltasVisita++;
                }
                else
                {
                    Datos.DatoPeriodo++;
                }
            }));
        }
    }
}
