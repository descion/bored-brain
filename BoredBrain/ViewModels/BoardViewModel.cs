using BoredBrain.Models;
using BoredBrain.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.ViewModels {

    class BoardViewModel : INotifyPropertyChanged {

        public ObservableCollection<ColumnWrapper> Columns { get; set; }

        public string ColumnField { get; set; }

        public string CategoryField { get; set; }

        private ObservableCollection<string> selectFields;
        public ObservableCollection<string> SelectFields { get {
                return this.selectFields;
            }
            set {
                this.selectFields = value;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectFields"));
            }
        }

        private BoardView boardView;

        public event PropertyChangedEventHandler PropertyChanged;

        public BoardViewModel(BoardView boardView) {
            this.Columns = new ObservableCollection<ColumnWrapper>();
            this.SelectFields = new ObservableCollection<string>();
            this.boardView = boardView;
        }

        public void LoadBoard(Board board) {

            this.Columns.Clear();

            Field columnField = board.ColumnField;
            this.ColumnField = columnField.Name;
            this.CategoryField = board.CategoryField?.Name;

            for (int itValues = 0; itValues < columnField.PossibleValues.Count; itValues++) {

                List<Card> cardsInColumn = board.GetCardsFiltered(columnField, columnField.PossibleValues[itValues]);

                List<CardWrapper> cardWrapperList = new List<CardWrapper>();

                for (int itCards = 0; itCards < cardsInColumn.Count; itCards++) {
                    Card currentCard = cardsInColumn[itCards];

                    cardWrapperList.Add(new CardWrapper() { CardViewModel = new CardViewModel(currentCard, this.OnEditCard) });
                }

                ColumnViewModel columnVM = new ColumnViewModel() {
                    Headline = columnField.PossibleValues[itValues],
                    Cards = cardWrapperList,
                    Board = board
                };

                this.Columns.Add(new ColumnWrapper() { ColumnViewModel = columnVM });
            }


            List<string> selectFields = new List<string>();

            for (int i = 0; i < board.Structure.Fields.Count; i++) {
                if(board.Structure.Fields[i].Type == FieldType.Select) {
                    selectFields.Add(board.Structure.Fields[i].Name);
                }
            }

            this.SelectFields = new ObservableCollection<string>(selectFields);
        }

        private void OnEditCard(Card card) {
            this.boardView.EditCard(card);
        }
    }
}
