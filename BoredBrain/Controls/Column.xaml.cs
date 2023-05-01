using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Column.xaml
    /// </summary>
    public partial class Column : UserControl {

        private Field field;
        private string fieldValue;

        public Column(Field field, string fieldValue) {
            InitializeComponent();

            this.field = field;
            this.fieldValue = fieldValue;
            this.Title.Content = fieldValue;
        }

        public void AddCard(CardElement c) {
            if((string)c.Card.GetFieldValue(this.field) == this.fieldValue) {
                c.Column = this;
                this.Cards.Children.Add(c);
            }
        }

        public void RemoveCard(CardElement c) {
            this.Cards.Children.Remove(c);
        }

        private void Grid_Drop(object sender, DragEventArgs e) {
            Card card = (Card)e.Data.GetData(DataFormats.Serializable);
            card.SetFieldValue(this.field, this.fieldValue);
        }
    }
}
