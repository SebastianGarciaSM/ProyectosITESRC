using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace ControlAlumnos
{
    /// <summary>
    /// Lógica de interacción para vndAgregarAlumno.xaml
    /// </summary>
    public partial class vndAgregarAlumno : Window
    {
        public vndAgregarAlumno()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnSeleccionarFoto_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog abrir = new OpenFileDialog();
            //abrir.Filter = "Todos los archivos de imagenes | *.jpg";
            //MemoryStream archivo = new MemoryStream();
            //if (abrir.ShowDialog() == true)
            //{
            //    BitmapImage b = new BitmapImage();
            //    string ruta = abrir.FileName;
            //    FileStream stream = new FileStream(abrir.FileName, FileMode.Open);
            //    b.BeginInit();
            //    b.CacheOption = BitmapCacheOption.OnLoad;
            //    b.StreamSource = stream;
            //    b.EndInit();
            //    b.Freeze();
            //    imgFoto.Source = b;
            //    txtRuta.Text = abrir.FileName;
            //    stream.Close();
            //}
        }

        private void imgFoto_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Filter = "Imagen JPG|*.jpg|Imagen PNG|*.png";
            if (abrir.ShowDialog() == true)
            {
                txtRuta.Text = abrir.FileName;
                imgFoto.Source = new BitmapImage(new Uri(abrir.FileName));
            }
        }
    }
}
