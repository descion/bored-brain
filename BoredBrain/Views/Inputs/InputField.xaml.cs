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

namespace BoredBrain.Views.Inputs {
    /// <summary>
    /// Interaction logic for InputField.xaml
    /// </summary>
    public partial class InputField : UserControl {

        //---------------------------------------------------------------------------

        private FieldDefinition definition;

        //---------------------------------------------------------------------------

        public InputField() {
            InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;
        }

        //---------------------------------------------------------------------------

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            this.definition = (FieldDefinition)this.DataContext;

            switch (this.definition.Type) {
                case "Text":
                    TextInput textInput = new TextInput();
                    this.Main.Children.Add(textInput);
                    textInput.DataContext = this.definition;
                    break;
                case "Number":
                    NumberInput numberInput = new NumberInput();
                    this.Main.Children.Add(numberInput);
                    numberInput.DataContext = this.definition;
                    break;
                case "Select":
                    SelectInput selectInput = new SelectInput();
                    this.Main.Children.Add(selectInput);
                    selectInput.DataContext = this.definition;
                    break;
                case "Multiselect":
                    break;
                default:
                    break;
            }

            this.DataContextChanged -= this.OnDataContextChanged;
        }
    }
}
