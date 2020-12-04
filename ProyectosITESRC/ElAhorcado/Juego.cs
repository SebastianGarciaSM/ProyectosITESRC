using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ElAhorcado
{
    public enum Categoria { Comidas, Oficios, Paises, Razas}

    public enum Comando { NombreEnviado, CategoriaEnviada, PalabraEnviada, LetraEnviada, ImagenEnviada, JugadaEnviada}

    public class Juego : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Jugador1 { get; set; } = "Jugador";
        public string Jugador2 { get; set; }

        public string Letra { get; set; }

        public string IP { get; set; } = "localhost";

        public bool MainWindowVisible { get; set; } = true;

        public string Mensaje { get; set; }

        public bool PuedeJugar { get; set; }

        public string CategoriaSeleccionada { get; set; }
        public string Imagen { get; set; }

        public ICommand JugarCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand EnviarLetraCommand { get; set; }

        WebSocket webSocket;
        HttpListener servidor;
        ClientWebSocket cliente;
        Dispatcher currentDispatcher;
        Lobby lobby;


        public Juego()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
            IniciarCommand = new RelayCommand<bool>(IniciarPartida);
            EnviarLetraCommand = new RelayCommand(Intentar);
        }


        private void CrearPartida()
        {
            servidor = new HttpListener();
            servidor.Prefixes.Add("http://*:1000/ahorcado/");
            servidor.Start();
            servidor.BeginGetContext(OnContext, null);

            Mensaje = "Esperando que se conecte un adversario...";

            //SeleccionarCategoria(CategoriaSeleccioanda);

            Actualizar();
        }

        public async Task ConectarPartida()
        {
            cliente = new ClientWebSocket();
            await cliente.ConnectAsync(new Uri($"ws://{IP}:1000/ahorcado/"), CancellationToken.None);
            webSocket = cliente;
            RecibirComando();
        }

        private async void OnContext(IAsyncResult ar)
        {
            var context = servidor.EndGetContext(ar);

            if (context.Request.IsWebSocketRequest)
            {
                var listener = await context.AcceptWebSocketAsync(null);
                webSocket = listener.WebSocket;
                CambiarMensaje("Cliente aceptado. Esperando información del jugador.");

                //Enviar mis datos al cliente
                EnviarComando(new DatoEnviado { Comando = Comando.CategoriaEnviada, Dato = CategoriaSeleccionada });
                EnviarComando(new DatoEnviado { Comando = Comando.NombreEnviado, Dato = Jugador1 });
                SeleccionarCategoria();
                EnviarComando(new DatoEnviado { Comando = Comando.PalabraEnviada, Dato = palabra });
                ConvertirRayitas();

                Imagen = $"Images/Vidas/ahorcado{errores}.jpg";
                EnviarComando(new DatoEnviado { Comando = Comando.ImagenEnviada, Dato = Imagen });
                EnviarComando(new DatoEnviado { Comando = Comando.LetraEnviada, Dato = Letra });

                RecibirComando();
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                servidor.BeginGetContext(OnContext, null);
            }
        }

        private async void IniciarPartida(bool tipoPartida)
        {
            try
            {
                MainWindowVisible = false;
                lobby = new Lobby();
                lobby.Closing += Lobby_Closing;
                lobby.DataContext = this;
                lobby.Show();
                Actualizar();

                if (tipoPartida == true)
                {
                    CrearPartida();
                }
                else
                {
                    Jugador2 = Jugador1;
                    Jugador1 = null;
                    Mensaje = "Intentando conectar con el servidor en " + IP;
                    Actualizar("Mensaje");
                    await ConectarPartida();
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Actualizar();
            }
        }




        private void Lobby_Closing(object sender, CancelEventArgs e)
        {
            MainWindowVisible = true;
            Actualizar("MainWindowVisible");
            if (servidor != null)
            {
                servidor.Stop();
                servidor = null;
            }
        }

        private async void EnviarComando(DatoEnviado datos)
        {
            byte[] buffer;
            var json = JsonConvert.SerializeObject(datos);
            buffer = Encoding.UTF8.GetBytes(json);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }



        VentanaJuego juego;
        private async void RecibirComando()
        {
            try
            {
                byte[] buffer = new byte[1024];

                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        juego.Close();
                        return;
                    }
                    string datosRecibidos = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var comando = JsonConvert.DeserializeObject<DatoEnviado>(datosRecibidos);

                    if (cliente != null) //Soy cliente
                    {
                        switch (comando.Comando)
                        {

                            case Comando.NombreEnviado:
                                Jugador1 = (string)comando.Dato;
                                CambiarMensaje("Conectado con el jugador " + Jugador1);
                                _ = currentDispatcher.BeginInvoke(new Action(() =>
                                {
                                    //AQUÍ DEBERÍA DE ESPERAR A QUE EL SERVIDOR ELIGA UNA CATEGORÍA
                                    //SeleccionarCategoria();

                                    EnviarComando(new DatoEnviado { Comando = Comando.NombreEnviado, Dato = Jugador2 });

                                    lobby.Hide();
                                    juego = new VentanaJuego();
                                    juego.Title = "CLIENTE";
                                    juego.DataContext = this;

                                    PuedeJugar = true;

                                    CambiarMensaje("Ingresa las letras para adivinar la palabra.");
                                    //EnviarComando(new DatoEnviado { Comando = Comando.LetraEnviada, Dato = Letra });
                                    juego.ShowDialog();
                                    lobby.Show();
                                }));

                                break;

                            case Comando.CategoriaEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    CategoriaSeleccionada = (string)comando.Dato;
                                }));
                                break;

                            case Comando.PalabraEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    palabra = (string)comando.Dato;
                                    p = palabra;
                                    ConvertirRayitas();
                                    Actualizar();
                                }));
                                break;
                            case Comando.ImagenEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    Imagen = (string)comando.Dato;
                                    Imagen = $"Images/Vidas/ahorcado{errores}.jpg";
                                    Actualizar();
                                }));
                                break;
                            case Comando.LetraEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    VerificarGanadorAsync();
                                    Letra = (string)comando.Dato;
                                    Intentar();
                                    Actualizar();
                                }));
                                break;
                            case Comando.JugadaEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    PalabraJugador1 = (string)comando.Dato;
                                    Actualizar();
                                }));
                                VerificarGanadorAsync();
                                break;

                        }
                    }
                    else //Soy servidor
                    {
                        switch (comando.Comando)
                        {
                            case Comando.NombreEnviado:
                                Jugador2 = (string)comando.Dato;
                                CambiarMensaje("Conectado con el jugador " + Jugador2);
                                _ = currentDispatcher.BeginInvoke(new Action(() =>
                                {
                                    CambiarMensaje("Ingresa las letras para adivinar la palabra.");
                                    lobby.Hide();
                                    juego = new VentanaJuego();
                                    juego.Title = "SERVIDOR";
                                    juego.DataContext = this;

                                    PuedeJugar = true;
                                    //CambiarMensaje("Seleccione la opción con la cual quiera jugar");
                                    juego.ShowDialog();
                                    lobby.Show();

                                }));
                                
                                break;

                            case Comando.JugadaEnviada:
                                currentDispatcher.Invoke(new Action(() =>
                                {
                                    PalabraJugador2 = (string)comando.Dato;
                                    Actualizar();
                                }));
                                VerificarGanadorAsync();
                                
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (webSocket.State == WebSocketState.Aborted)
                {
                    juego.Close();
                    lobby.Close();
                    MainWindowVisible = true;
                    Actualizar("MainWindowVisible");
                }
                else
                    CambiarMensaje(ex.Message);
            }
        }



        void Actualizar(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void CambiarMensaje(string mensaje)
        {
            currentDispatcher.Invoke(new Action(() =>
            {
                Mensaje = mensaje;
                Actualizar();
            }));
        }



        private string palabra;
        private string p;
        public int errores;
        private char[] rayitas;

        public int Errores { get { return errores; } }
        public string Palabra
        {
            get { return palabra; }
            set { palabra = value; }
        }

        public string P
        {
            get { return p; }
            set { p = value; }
        }
        public void SeleccionarCategoria()
        {
            errores = 0;
            var app = App.Current.GetType().Assembly;
            StreamReader fs = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}//{CategoriaSeleccionada}.txt");
            //StreamReader fs = new StreamReader(app.GetManifestResourceStream($"AplicacionesClienteServidor_Unidad3.{CategoriaSeleccionada}.txt"));
            List<string> temp = new List<string>();
            string s = null;
            while ((s = fs.ReadLine()) != null)
            {
                temp.Add(s);
            }
            Random r = new Random();
            palabra = temp[r.Next(0, temp.Count)];
            p = palabra;


        }

        public void ConvertirRayitas()
        {
            rayitas = new char[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                if (char.IsLetter(p[i]))
                    rayitas[i] = '_';
                else
                    rayitas[i] = p[i];
            }

            P = string.Join(" ", rayitas);
        }

        public string PalabraJugador1 { get; set; }
        public string PalabraJugador2 { get; set; }
        public string PJugador1 { get; set; }
        public string PJugador2 { get; set; }

        public void Intentar()
        {

            try
            {
                if (palabra.Any(x => x == char.Parse(Letra)))
                {
                    for (int i = 0; i < palabra.Length; i++)
                    {
                        if (palabra[i] == char.Parse(Letra))
                        {
                            rayitas[i] = char.Parse(Letra);
                        }
                    }
                    //string palabraConvertida = new string(rayitas);
                    P = string.Join(" ", rayitas);
                    //P = palabraConvertida;
                    CambiarMensaje("¡Letra adivinada!");
                }
                //if (!P.Contains("_"))
                //{
                //    PuedeJugar = false;
                //    CambiarMensaje("¡Has ganado la partida! :)");
                //}
                if (!palabra.Any(x => x == char.Parse(Letra)))
                {
                    errores++;
                    Imagen = $"Images/Vidas/ahorcado{errores}.jpg";
                    CambiarMensaje("Letra equivocada :(");

                    if (errores > 5)
                    {
                        PuedeJugar = false;
                        CambiarMensaje($"Has perdido la partida. La palabra correcta es {Palabra}.");
                    }

                }
            }
            catch
            {
                CambiarMensaje("Ingresa una letra a adivinar.");
            }

            if (cliente != null) //Soy cliente
            {
                PalabraJugador2 = P;
                EnviarComando(new DatoEnviado { Comando = Comando.JugadaEnviada, Dato = PalabraJugador2 });
                Actualizar();
            }
            else //Soy servidor
            {

                PalabraJugador1 = P;
                EnviarComando(new DatoEnviado { Comando = Comando.JugadaEnviada, Dato = PalabraJugador1 });
                Actualizar();
            }

            VerificarGanadorAsync();
        }

        private void VerificarGanadorAsync()
        {
            if (PalabraJugador1 != null && PalabraJugador2 != null)
            {
                //bool ganaJugador1 = (!PalabraJugador1.Contains("_") && PalabraJugador2.Contains("_"));

                if (!PalabraJugador1.Contains("_") && PalabraJugador2.Contains("_"))
                {
                    PuedeJugar = false;
                    CambiarMensaje($"¡{Jugador1} ha ganado la partida!");
                }
                if (PalabraJugador1.Contains("_") && !PalabraJugador2.Contains("_"))
                {
                    PuedeJugar = false;
                    CambiarMensaje($"¡{Jugador2} ha ganado la partida!");
                }
            }

            //if (SeleccionJugador1 != null && SeleccionJugador2 != null)
            //{
            //    if (SeleccionJugador1 == SeleccionJugador2)
            //    {
            //        CambiarMensaje("Hubo un empate, nadie gana puntos");
            //    }
            //    else
            //    {
            //        bool ganaJugador1 = (SeleccionJugador1 == Opcion.PAPEL && SeleccionJugador2 == Opcion.PIEDRA)
            //              || (SeleccionJugador1 == Opcion.PIEDRA && SeleccionJugador2 == Opcion.TIJERAS)
            //              || (SeleccionJugador1 == Opcion.TIJERAS && SeleccionJugador2 == Opcion.PAPEL);

            //        if (ganaJugador1)
            //        {
            //            PuntosJugador1++;
            //            CambiarMensaje("Gano " + Jugador1);
            //        }
            //        else
            //        {
            //            PuntosJugador2++;
            //            CambiarMensaje("Gano " + Jugador2);
            //        }
            //    }

            //    if (PuntosJugador1 < 3 && PuntosJugador2 < 3)
            //    {
            //        await Task.Delay(3000);
            //        CambiarMensaje("Preparando el siguiente turno....");
            //        await Task.Delay(2000);
            //        SeleccionJugador1 = null;
            //        SeleccionJugador2 = null;
            //        PuedeJugar = true;
            //        CambiarMensaje("Seleccione la opción con la cual quiera jugar");

            //    }
            //    else
            //    {
            //        await Task.Delay(3000);
            //        CambiarMensaje("El juego ha terminado. Gano " + ((PuntosJugador1 > PuntosJugador2) ? Jugador1 : Jugador2));
            //    }
            //}
        }
        public class DatoEnviado
        {
            public Comando Comando { get; set; }
            public object Dato { get; set; }
        }
    }
}
