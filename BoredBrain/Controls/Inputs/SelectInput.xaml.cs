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
    /// Interaction logic for SelectInput.xaml
    /// </summary>
    public partial class SelectInput : InputBase {

        public SelectInput(InputDefinition definition) : base(definition) {
            InitializeComponent();

            this.Label.Content = definition.name;

            SelectInputDefinition selectDefinition = (SelectInputDefinition)definition;

            for (int i = 0; i < selectDefinition.possibleValues.Count; i++) {
                ComboBoxItem valueItem = new ComboBoxItem();
                valueItem.Content = selectDefinition.possibleValues[i];

                valueItem.IsSelected = selectDefinition.possibleValues[i] == (string)definition.value;

                this.Value.Items.Add(valueItem);
            }
        }

        public override void ApplyValue() {
            this.definition.value = ((ComboBoxItem)this.Value.SelectedItem).Content;
        }
    }
}
