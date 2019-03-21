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
using BancDelTemps.Model.Class;
using BancDelTemps.ViewModel;

namespace BancDelTemps.View
{
    /// <summary>
    /// Interaction logic for Informacio.xaml
    /// </summary>
    public partial class Informacio : Window
    {
        public Informacio(Object ob)
        {
            InitializeComponent();
            DataContext = new InformacioViewModel(ob);
        }

        
    }
}
