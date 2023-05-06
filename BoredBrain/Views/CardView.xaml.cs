using BoredBrain.Models;
using BoredBrain.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : UserControl {

        //---------------------------------------------------------------------------

        private bool mouseDown;

        private CardViewModel card;

        private CardDropPreview dropPreview;

        //---------------------------------------------------------------------------

        public CardView() {
            InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;
        }

        //---------------------------------------------------------------------------

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            this.card = ((CardWrapper)this.DataContext).CardViewModel;
            this.DataContextChanged -= this.OnDataContextChanged;
        }

        //---------------------------------------------------------------------------

        private void Card_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.mouseDown = true;
            }
        }

        //---------------------------------------------------------------------------

        private void Card_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed && this.mouseDown) {
                this.Visibility = Visibility.Collapsed;
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this.card.Card), DragDropEffects.Move);
            }
        }

        //---------------------------------------------------------------------------

        private void Card_MouseUp(object sender, MouseButtonEventArgs e) {
            if (this.mouseDown && e.ChangedButton == MouseButton.Left) {
                this.card.OnEditCard?.Invoke(this.card.Card);
            }

            this.mouseDown = false;
        }

        //---------------------------------------------------------------------------

        private void Card_Drop(object sender, DragEventArgs e) {
            Card card = (Card)e.Data.GetData(DataFormats.Serializable);

            if (card.Id != this.card.Card.Id) {
                if (this.IsDropBefore(e)) {
                    this.card.OnMoveCard(card, this.card.Card, CardMoveMode.Before);
                }
                else {
                    this.card.OnMoveCard(card, this.card.Card, CardMoveMode.After);
                }
            }

            e.Handled = true;

        }

        //---------------------------------------------------------------------------

        private void Card_DragEnter(object sender, DragEventArgs e) {
            Card card = (Card)e.Data.GetData(DataFormats.Serializable);
            this.dropPreview = new CardDropPreview(card);

            if (IsDropBefore(e)) {
                this.CardContainer.Children.Insert(0, this.dropPreview);
            }
            else {
                this.CardContainer.Children.Add(this.dropPreview);
            }

            e.Handled = true;
        }

        //---------------------------------------------------------------------------

        private void Card_DragLeave(object sender, DragEventArgs e) {
            this.CardContainer.Children.Remove(this.dropPreview);
            this.dropPreview = null;
            e.Handled = true;
        }

        //---------------------------------------------------------------------------

        private bool IsDropBefore(DragEventArgs e) {
            return e.GetPosition(this.Card).Y < this.Card.ActualHeight * 0.5f;
        }

        //---------------------------------------------------------------------------

        private void Card_QueryContinueDrag(object sender, QueryContinueDragEventArgs e) {
            if (e.EscapePressed) {
                e.Action = DragAction.Cancel;
                this.Visibility = Visibility.Visible;

                if(this.dropPreview != null) {
                    this.CardContainer.Children.Remove(this.dropPreview);
                    this.dropPreview = null;
                }
            }
        }

    }
}
