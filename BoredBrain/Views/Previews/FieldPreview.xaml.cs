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

namespace BoredBrain.Views.Previews {
    /// <summary>
    /// Interaction logic for FieldPreview.xaml
    /// </summary>
    public partial class FieldPreview : UserControl {
        
        private FieldDefinition definition;

        //---------------------------------------------------------------------------

        public FieldPreview() {
            InitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        //---------------------------------------------------------------------------

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            this.definition = (FieldDefinition)this.DataContext;

            switch (this.definition.Type) {
                case "Text":
                    TextFieldPreview textInput = new TextFieldPreview();
                    this.Main.Children.Add(textInput);
                    textInput.DataContext = this.definition;
                    break;
                case "Number":
                    NumberFieldPreview numberInput = new NumberFieldPreview();
                    this.Main.Children.Add(numberInput);
                    numberInput.DataContext = this.definition;
                    break;
                case "Select":
                    SelectFieldPreview selectInput = new SelectFieldPreview();
                    this.Main.Children.Add(selectInput);
                    selectInput.DataContext = this.definition;
                    break;
                case "Multiselect":

                    MultiselectViewModel multiselectViewModel = new MultiselectViewModel(this.definition);

                    MultiselectPreview multiselectInput = new MultiselectPreview();
                    this.Main.Children.Add(multiselectInput);
                    multiselectInput.DataContext = multiselectViewModel;
                    break;
                case "Date":
                    if (!string.IsNullOrEmpty((string)this.definition.Value)) {
                        DatePreview dateInput = new DatePreview();
                        this.Main.Children.Add(dateInput);
                        dateInput.DataContext = new DateViewModel() { Definition = this.definition };
                    }
                    break;
                default:
                    break;
            }

            this.DataContextChanged -= this.OnDataContextChanged;
        }
    }
}
