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

namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for FieldView.xaml
    /// </summary>
    public partial class FieldView : UserControl {

        //---------------------------------------------------------------------------

        public FieldView() {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------

        private void AddPossibleValue_Click(object sender, RoutedEventArgs e) {
            FieldDefinition definition = (FieldDefinition)this.DataContext;

            definition.PossibleValues.Add(this.NewPossibleValue.Text);

            this.NewPossibleValue.Text = "";
        }

        //---------------------------------------------------------------------------

        private void PossibleValue_DeleteClick(object sender, RoutedEventArgs e) {
            Chip possibleValueSender = (Chip)sender;
            FieldDefinition definition = (FieldDefinition)this.DataContext;

            definition.PossibleValues.Remove(possibleValueSender.Content as string);
        }

        //---------------------------------------------------------------------------

        private void Delete_Click(object sender, RoutedEventArgs e) {
            FieldDefinition definition = (FieldDefinition)this.DataContext;
            definition.DoDelete();
        }
    }
}
