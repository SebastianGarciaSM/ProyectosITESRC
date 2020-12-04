using LibraryAlumnos;
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
    /// Lógica de interacción para vndEditar.xaml
    /// </summary>
    public partial class vndEditar : Window
    {
        public vndEditar()
        {
            InitializeComponent();
        }
        OpenFileDialog abrir = new OpenFileDialog();
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var t = abrir.FileName;
            if (t == null)
            {
                string tempo = $"{Directory.GetCurrentDirectory()}\\Fotos\\" + $"{ ((this.DataContext) as Alumnos).NumeroDeControl}.jpg";
                imgFoto.Source = new BitmapImage(new Uri(tempo));
            }
            DialogResult = true;
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
