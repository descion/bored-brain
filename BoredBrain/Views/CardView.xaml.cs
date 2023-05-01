using BoredBrain.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl {

        private CardViewModel card;

        public CardView() {
            InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            this.card = ((CardWrapper)this.DataContext).CardViewModel;
            this.DataContextChanged -= this.OnDataContextChanged;
        }

        private void Card_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this.card.Card), DragDropEffects.Move);
            }
        }

        private void Card_MouseUp(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.card.OnEditCard?.Invoke(this.card.Card);
            }
        }
    }
}
