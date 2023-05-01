using BoredBrain.Models;
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
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : UserControl {

        public ColumnView() {
            InitializeComponent();
        }

        private void Card_Drop(object sender, DragEventArgs e) {
            ColumnViewModel viewModel = ((ColumnViewModel)this.DataContext);

            ((Card)e.Data.GetData(DataFormats.Serializable)).SetFieldValue(viewModel.Board.ColumnField, viewModel.Headline);
        }
    }
}
