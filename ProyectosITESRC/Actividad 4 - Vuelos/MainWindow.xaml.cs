using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Actividad_4___Vuelos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClienteVuelos cliente = new ClienteVuelos();
        VueloDatos datos = new VueloDatos();

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = datos;
            cliente.Get();
            cliente.AlActualizar += Cv_AlActualizar;
            btnAgregar.IsEnabled = false;
        }

        private void Cv_AlActualizar()
        {
            dtgVuelos.ItemsSource = cliente.Model;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cliente.AgregarVuelo(datos);
                btnAgregar.IsEnabled = false;
                txtDestino.Text = txtHora.Text = txtVuelo.Text = cmbEstado.Text = "";
                cliente.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                datos.Hora = txtHora.Text;
                datos.Vuelo = txtVuelo.Text;
                datos.Destino = txtDestino.Text;
                datos.Estado = cmbEstado.Text;
                
                cliente.EditarVuelo(datos);
                cliente.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(dtgVuelos.SelectedIndex != -1)
            {
                try
                {
                    VueloDatos vuelo = new VueloDatos();
                    vuelo = dtgVuelos.SelectedItem as VueloDatos;
                    if(MessageBox.Show($"¿Deseas eliminar el vuelo {vuelo.Vuelo}?", "Alerta", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                    {
                        cliente.EliminarVuelo(vuelo);
                        txtDestino.Text = txtHora.Text = txtVuelo.Text = cmbEstado.Text = "";
                        cliente.Get();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Elige un renglón para eliminarlo.", "Alerta", MessageBoxButton.OK);
            }
            
        }

        private void dtgVuelos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgVuelos.SelectedItem != null)
            {
                datos = dtgVuelos.SelectedItem as VueloDatos;
                txtHora.Text = datos.Hora;
                txtDestino.Text = datos.Destino;
                txtVuelo.Text = datos.Vuelo;
                cmbEstado.Text = datos.Estado;
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = datos;
            btnAgregar.IsEnabled = true;
        }
    }
}
