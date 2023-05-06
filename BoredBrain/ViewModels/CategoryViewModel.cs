using BoredBrain.Models;
using BoredBrain.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoredBrain.ViewModels {
    class CategoryViewModel {

        public Field Category { get; set; }

        public string CategoryValue { get; set; }

        public Action<Card> EditCard { get; set; }
        public Action<Card, Card, CardMoveMode> MoveCard { get; set; }

        public ObservableCollection<ColumnViewModel> Columns { get; set; }

        public Visibility CategoryHeadlineVisibility {
            get {
                return this.CategoryValue != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public CategoryViewModel(string categoryValue) {
            this.Columns = new ObservableCollection<ColumnViewModel>();
            this.CategoryValue = categoryValue;
        }

        public void LoadBoard(Board board) {

            this.Category = board.CategoryField;

            this.Columns.Clear();

            Field columnField = board.ColumnField;

            for (int itValues = 0; itValues < columnField.PossibleValues.Count; itValues++) {

                List<Card> cardsInColumn = board.GetCardsFiltered(columnField, columnField.PossibleValues[itValues]);

                List<CardWrapper> cardWrapperList = new List<CardWrapper>();

                for (int itCards = 0; itCards < cardsInColumn.Count; itCards++) {
                    Card currentCard = cardsInColumn[itCards];

                    if (this.CategoryValue == null || currentCard.GetFieldValue(this.Category).Equals(this.CategoryValue)) {
                        cardWrapperList.Add(new CardWrapper() { CardViewModel = new CardViewModel(currentCard, this.OnEditCard, this.OnMoveCard) });
                    }
                }

                ColumnViewModel columnVM = new ColumnViewModel() {
                    Headline = columnField.PossibleValues[itValues],
                    Cards = cardWrapperList,
                    Board = board,
                    Category = this.CategoryValue
                };

                this.Columns.Add(columnVM);
            }

        }


        private void OnEditCard(Card card) {
            this.EditCard?.Invoke(card);
        }

        private void OnMoveCard(Card cardToMove, Card referenceCard, CardMoveMode mode) {
            this.MoveCard?.Invoke(cardToMove, referenceCard, mode);
        }
    }
}
