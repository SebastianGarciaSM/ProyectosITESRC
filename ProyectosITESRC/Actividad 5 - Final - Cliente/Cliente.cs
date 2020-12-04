using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Actividad_5___Final___Cliente
{
    public class Cliente
    {
        HttpClient cliente = new HttpClient();

        public Cliente()
        {
            cliente.BaseAddress = new Uri("http://localhost:1000/");
        }

        public async void Agregar(Datos d)
        {
            var json = JsonConvert.SerializeObject(d);
            var result = await cliente.PostAsync("practica5/ropanueva", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }

        public async void Editar(Datos d)
        {
            var json = JsonConvert.SerializeObject(d);
            var result = await cliente.PutAsync("practica5/ropanueva", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }

        public async void Eliminar(Datos d)
        {
            var json = JsonConvert.SerializeObject(d);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, "practica5/ropanueva");
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await cliente.SendAsync(message);
            result.EnsureSuccessStatusCode();
        }
    }
}
