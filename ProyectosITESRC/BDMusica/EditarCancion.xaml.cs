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
using System.Windows.Shapes;

namespace BDMusica
{
    /// <summary>
    /// Lógica de interacción para EditarCancion.xaml
    /// </summary>
    public partial class EditarCancion : Window
    {
        public EditarCancion()
        {
            InitializeComponent();
        }
        AdministracionCanciones ac = new AdministracionCanciones();
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAceptat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
                this.Close();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCategoria.ItemsSource = ac.ObtenerListaCategorias();
            cmbArtista.ItemsSource = ac.ObtenerListaArtistas();
        }
    }
}
