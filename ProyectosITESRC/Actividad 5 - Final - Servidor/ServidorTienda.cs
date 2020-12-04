using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace Actividad_5___Final___Servidor
{
    public class ServidorTienda
    {
        public CatalogoRopa CatRopa { get; set; } = new CatalogoRopa();
        HttpListener server;
        Dispatcher current;

        public ServidorTienda()
        {
            current = Dispatcher.CurrentDispatcher;
            server = new HttpListener();
            server.Prefixes.Add("http://*:1000/practica5/");
            server.Start();
            server.BeginGetContext(OnContext, null);
        }

        private void OnContext(IAsyncResult ar)
        {
            var context = server.EndGetContext(ar);
            server.BeginGetContext(OnContext, null);

            if(context.Request.Url.LocalPath == "/practica5/ropanueva")
            {
                if (context.Request.HttpMethod == "GET")
                {
                    var datos = JsonConvert.SerializeObject(CatRopa.Prendas);
                    byte[] buffer = Encoding.UTF8.GetBytes(datos);
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = 200;
                }
                else
                {
                    if (context.Request.ContentType.StartsWith("application/json") && context.Request.ContentLength64 > 0)
                    {
                        StreamReader reader = new StreamReader(context.Request.InputStream);
                        string datos = reader.ReadToEnd();
                        var ropa = JsonConvert.DeserializeObject<Cargamento>(datos);
                        current.Invoke(new Action(() =>
                        {
                            if (context.Request.HttpMethod == "POST")
                            {
                                CatRopa.Agregar(ropa);

                            }
                            else if (context.Request.HttpMethod == "PUT")
                            {
                                CatRopa.Editar(ropa);
                            }
                            else if (context.Request.HttpMethod == "DELETE")
                            {
                                CatRopa.Eliminar(ropa);
                            }
                        }));
                        context.Response.StatusCode = 200;
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
            }
            else
            {
                context.Response.StatusCode = 404;
            }
            context.Response.Close();
        }
    }
}
