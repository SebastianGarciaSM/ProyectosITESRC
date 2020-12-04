using System;
using System.Collections.Generic;
using System.IO;
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
using LibraryAlumnos;

namespace ControlAlumnos
{ 
    
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListAl.Abrir();
            itcAlumnos.ItemsSource = ListAl.ListaAlumnos;
        }
        ListadoAlumnos ListAl = new ListadoAlumnos();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            ListAl.Guardar();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vndAgregarAlumno vndA = new vndAgregarAlumno();
                Alumnos a = new Alumnos();
                vndA.DataContext = a;
                if (vndA.ShowDialog() == true)
                {
                    ListAl.CrearAlumno(a, vndA.txtRuta.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MIEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vndEditar vndM = new vndEditar();
                Alumnos a = new Alumnos();
                a = ((MenuItem)sender).DataContext as Alumnos;
                vndM.DataContext = a;
                //var pd = ((FrameworkElement)sender).DataContext as Alumnos;
                //vndM.DataContext = pd;
                //if (vndM.ShowDialog() == true)
                //{
                //    ListAl.Editar();
                //}
                if (vndM.ShowDialog() == true)
                {
                    var t = ((BitmapImage)vndM.imgFoto.Source).UriSource.LocalPath;
                    ListAl.Editar(a, ((BitmapImage)vndM.imgFoto.Source).UriSource.LocalPath);
                    itcAlumnos.ItemsSource = null;
                    itcAlumnos.ItemsSource = ListAl.ListaAlumnos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MIEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Deseas eliminar el alumno seleccionado?", "Alumno a eliminar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var pd = ((FrameworkElement)sender).DataContext as Alumnos;
                ListAl.EliminarAlumno(pd);
                MessageBox.Show("Alumno eliminado.");
            }
        }
    }
}
