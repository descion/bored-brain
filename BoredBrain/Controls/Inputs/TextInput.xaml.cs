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
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInput : InputBase {

        public TextInput(InputDefinition definition) : base(definition) {
            InitializeComponent();

            this.Label.Content = definition.name;
            this.Value.Text = (string)definition.value;
        }

        public override void ApplyValue() {
            this.definition.value = this.Value.Text;
        }
    }
}
