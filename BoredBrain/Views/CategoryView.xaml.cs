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
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl {

        //---------------------------------------------------------------------------

        public CategoryView() {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------

        private void Category_Drop(object sender, DragEventArgs e) {
            CategoryViewModel viewModel = ((CategoryViewModel)this.DataContext);

            Card card = (Card)e.Data.GetData(DataFormats.Serializable);
            card.SetFieldValue(viewModel.Category, viewModel.CategoryValue);
            
        }
    }
}
