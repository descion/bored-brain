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

    public class InputDefinition {
        public string name;
        public FieldType type;
        public object value;
    }

    public class SelectInputDefinition : InputDefinition {
        public List<string> possibleValues;
    }


    /// <summary>
    /// Interaction logic for CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : UserControl {

        private Action<List<InputDefinition>> onCreateCallback;

        private List<InputDefinition> fields;

        private List<InputBase> createdInputs;

        public CreateDialog() {
            InitializeComponent();

            this.createdInputs = new List<InputBase>();
        }

        public void Open(string headline, string buttonText, List<InputDefinition> fields, Action<List<InputDefinition>> onCreateCallback) {
            this.DialogHeadline.Content = headline;
            this.CreateButton.Content = buttonText;
            this.fields = fields;
            this.onCreateCallback = onCreateCallback;

            this.fields = fields;

            for (int i = 0; i < this.fields.Count; i++) {
                switch (this.fields[i].type) {
                    case FieldType.Text:
                        this.AddTextInput(this.fields[i]);
                        break;
                    case FieldType.Number:
                        this.AddNumberInput(this.fields[i]);
                        break;
                    case FieldType.Select:
                        this.AddSelectInput(this.fields[i]);
                        break;
                    case FieldType.Multiselect:
                        this.AddMultiselectInput(this.fields[i]);
                        break;
                    case FieldType.Date:
                        break;
                    default:
                        break;
                }
            }

            this.Visibility = Visibility.Visible;
        }

        private void AddTextInput(InputDefinition definition) {

            TextInput input = new TextInput(definition);
            this.Inputs.Children.Add(input);
            this.createdInputs.Add(input);
        }


        private void AddNumberInput(InputDefinition definition) {
            NumberInput input = new NumberInput(definition);
            this.Inputs.Children.Add(input);
            this.createdInputs.Add(input);
        }

        private void AddSelectInput(InputDefinition definition) {
            SelectInput input = new SelectInput(definition);
            this.Inputs.Children.Add(input);
            this.createdInputs.Add(input);
        }


        private void AddMultiselectInput(InputDefinition definition) {
            MultiselectInput input = new MultiselectInput(definition);
            this.Inputs.Children.Add(input);
            this.createdInputs.Add(input);
        }

        public void Close() {
            this.Visibility = Visibility.Hidden;
            this.Inputs.Children.Clear();
            this.fields = null;
            this.createdInputs.Clear();
        }


        private void CloseDialog_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < this.createdInputs.Count; i++) {
                this.createdInputs[i].ApplyValue();
            }

            this.onCreateCallback(new List<InputDefinition>(this.fields));
            this.Close();
        }
    }
}
