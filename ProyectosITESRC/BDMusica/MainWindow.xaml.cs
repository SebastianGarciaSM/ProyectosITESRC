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

namespace BDMusica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        AdministracionCanciones ac = new AdministracionCanciones();
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            Cancion c = new Cancion();
            AgregarCancion agregarCancion = new AgregarCancion();
            agregarCancion.DataContext = c;
            agregarCancion.ShowDialog();
            bool? resultado = agregarCancion.DialogResult;

            if (resultado == true)
            {
                ac.Agregar(c);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtaCanciones.ItemsSource = ac.ListaCanciones;
        }

        private void ImgEditar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cancion c = ((FrameworkElement)sender).DataContext as Cancion;
            if (MessageBox.Show("¿Desea editar la canción seleccionada?", "Atención", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                EditarCancion ec = new EditarCancion();
                ec.DataContext = c;
                ec.ShowDialog();
                
                ac.Editar(c);
            }

        }

        private void ImgEliminar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cancion c = ((FrameworkElement)sender).DataContext as Cancion;
            if (MessageBox.Show("¿Desea eliminar la canción seleccionada?", "Atención", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                ac.Eliminar(c);
            }
        }
    }
}
