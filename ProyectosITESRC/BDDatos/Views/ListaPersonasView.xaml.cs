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
using BDDatos.ViewModels;

namespace BDDatos.Views
{
    /// <summary>
    /// Lógica de interacción para ListaPersonasView.xaml
    /// </summary>
    public partial class ListaPersonasView : Window
    {
        ListaPersonasViewModel lp = new ListaPersonasViewModel();
        public ListaPersonasView()
        {
            InitializeComponent();
            lp.Cargar();
            this.DataContext = lp;
            lp.Confirmar = Confirmar;
        }

        public bool Confirmar(string personaElimnar)
        {
            return MessageBox.Show($"¿Seguro que deseas eliminar a {personaElimnar}?", "Atención", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
        }
    }
}
