using BoredBrain.ViewModels;
using MaterialDesignThemes.Wpf;
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

namespace BoredBrain {
    /// <summary>
    /// Interaction logic for MultiselectInput.xaml
    /// </summary>
    public partial class MultiselectInput {

        private MultiselectViewModel multiselectViewModel;

        public MultiselectInput(MultiselectViewModel multiselectViewModel) {
            InitializeComponent();

            this.multiselectViewModel = multiselectViewModel;
            this.DataContext = multiselectViewModel;
        }

        private void Value_DeleteClick(object sender, RoutedEventArgs e) {
            this.multiselectViewModel.RemoveValue((string)((Chip)sender).Content);
            e.Handled = true;
        }

        private void Value_Selected(object sender, RoutedEventArgs e) {
            if(this.ValueSelector.SelectedItem == null) {
                return;
            }

            string chosenValue = (string)this.ValueSelector.SelectedItem;
            this.multiselectViewModel.AddValue(chosenValue);
            this.ValueSelector.SelectedIndex = -1;
        }
    }
}
