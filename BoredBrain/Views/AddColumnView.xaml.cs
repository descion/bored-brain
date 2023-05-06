using BoredBrain.ViewModels;
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

namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : UserControl {

        private AddColumnViewModel addColumnViewModel;
        
        public Action<bool> OnClose { get; set; }

        public AddColumnView() {
            InitializeComponent();

            this.addColumnViewModel = new AddColumnViewModel();
            this.DataContext = this.addColumnViewModel;
        }



        public string GetName() {
            return this.addColumnViewModel.Name;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.OnClose?.Invoke(false);
        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            this.OnClose?.Invoke(true);
        }
    }
}
