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
    /// Lógica de interacción para AgregarCancion.xaml
    /// </summary>
    public partial class AgregarCancion : Window
    {
        AdministracionCanciones ac = new AdministracionCanciones();
        public AgregarCancion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCategoria.ItemsSource = ac.ObtenerListaCategorias();
            //cmbCategoria.DisplayMemberPath = "NombreCategoria";
            //cmbCategoria.SelectedValuePath = "IdCategoria";
            cmbArtista.ItemsSource = ac.ObtenerListaArtistas();
            //cmbArtista.DisplayMemberPath = "NombreArtista";
        }

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
            catch(Exception m)
            {
                MessageBox.Show(m.Message);
            }
            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
