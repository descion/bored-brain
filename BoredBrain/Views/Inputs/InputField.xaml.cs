using BoredBrain.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
                    MultiselectViewModel multiselectViewModel = new MultiselectViewModel(this.definition);
                    MultiselectInput multiselectInput = new MultiselectInput(multiselectViewModel);
                    this.Main.Children.Add(multiselectInput);
                    break;
                case "Date":
                    DateInput dateInput = new DateInput();
                    this.Main.Children.Add(dateInput);
                    dateInput.DataContext =new DateViewModel() { Definition = this.definition };
                    break;
                default:
                    break;
            }

            this.DataContextChanged -= this.OnDataContextChanged;
        }
    }
}
