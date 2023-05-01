using System.Collections.Generic;
using System.Windows.Controls;

namespace BoredBrain {
    /// <summary>
    /// Interaction logic for StructureField.xaml
    /// </summary>
    /// 

    public class FieldDefinition {
        public string name;
        public string type;
        public List<string> possibleValues;
    }

    public partial class StructureField : UserControl {

        public FieldDefinition Definition { get; set; }

        public delegate void OnDeleteDelegate(StructureField field);
        public delegate void OnSetColumnDelegate(StructureField field);

        public event OnDeleteDelegate OnDelete;
        public event OnSetColumnDelegate OnSetColumn;


        public StructureField(FieldDefinition definition) {
            InitializeComponent();

            this.Definition = definition;

            this.Name.Content = this.Definition.name;
            this.Type.Content = this.Definition.type;

            this.FillOptions();
        }

        private void FillOptions() {
            this.PossibleValues.Children.Clear();

            for (int i = 0; i < this.Definition.possibleValues.Count; i++) {
                Label valueLabel = new Label();
                valueLabel.Content = this.Definition.possibleValues[i];

                this.PossibleValues.Children.Add(valueLabel);
            }
        }

        public FieldDefinition GetDefinition() {
            return this.Definition;
        }

        private void AddOption_Click(object sender, System.Windows.RoutedEventArgs e) {
            string newOption = this.NewOption.Text;

            this.Definition.possibleValues.Add(newOption);
            this.FillOptions();
        }


        private void Column_Click(object sender, System.Windows.RoutedEventArgs e) {
            this.OnSetColumn?.Invoke(this);
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e) {
            this.OnDelete?.Invoke(this);
        }
    }
}
