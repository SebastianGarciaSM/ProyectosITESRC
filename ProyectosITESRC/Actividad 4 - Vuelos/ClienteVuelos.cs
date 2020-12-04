using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Policy;

using System.Runtime.Remoting.Channels;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Actividad_4___Vuelos
{
    public class ClienteVuelos
    {
        public CatalogoVuelos Vuelos { get; set; } = new CatalogoVuelos();

        HttpClient client;

        public ClienteVuelos()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://vuelos.itesrc.net/");
        }

        public async void AgregarVuelo(VueloDatos vuelo)
        {
            var json = JsonConvert.SerializeObject(vuelo);
            var result = await client.PostAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }

        public async void EditarVuelo(VueloDatos vuelo)
        {
            var json = JsonConvert.SerializeObject(vuelo);
            var result = await client.PutAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }

        public async void EliminarVuelo(VueloDatos vuelo)
        {
            var json = JsonConvert.SerializeObject(vuelo);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, "/Tablero");
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.SendAsync(message);
            result.EnsureSuccessStatusCode();
        }

        public delegate void movimiento();
        public event movimiento AlActualizar;
        public IEnumerable<VueloDatos> Model { get; set; }

        public async void Get()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("http://vuelos.itesrc.net/Tablero");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Model = JsonConvert.DeserializeObject<IEnumerable<VueloDatos>>(jsonString);
                AlActualizar?.Invoke();
            }
        }
    }
}
