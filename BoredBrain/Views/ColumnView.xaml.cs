using BoredBrain.Models;
using BoredBrain.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for ColumnView.xaml
    /// </summary>
    public partial class ColumnView : UserControl {

        private CardDropPreview dropPreview;

        public ColumnView() {
            InitializeComponent();
        }

        private void Card_Drop(object sender, DragEventArgs e) {
            ColumnViewModel viewModel = ((ColumnViewModel)this.DataContext);
            Card card = (Card)e.Data.GetData(DataFormats.Serializable);

            card.SetFieldValue(viewModel.Board.ColumnField, viewModel.Headline);

            if (viewModel.Board.CategoryField != null) {
                card.SetFieldValue(viewModel.Board.CategoryField, viewModel.Category);
            }

            viewModel.Board.MoveToEnd(card);

            e.Handled = true;
        }

        private void Card_DragEnter(object sender, DragEventArgs e) {
            Card card = (Card)e.Data.GetData(DataFormats.Serializable);
            this.dropPreview = new CardDropPreview(card);
            this.CardContainer.Children.Add(this.dropPreview);
        }

        private void Card_DragLeave(object sender, DragEventArgs e) {
            this.CardContainer.Children.Remove(this.dropPreview);
            this.dropPreview = null;
        }
    }
}
