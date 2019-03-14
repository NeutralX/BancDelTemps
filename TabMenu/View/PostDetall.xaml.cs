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
    /// Interaction logic for PostDetall.xaml
    /// </summary>
    public partial class PostDetall : Window
    {
        public PostDetall(Post post)
        {
            InitializeComponent();
            DataContext = new PostDetallViewModel(post);
        }
    }
}
