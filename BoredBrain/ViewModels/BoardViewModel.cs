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

        public string ColumnField { get; set; }

        public string CategoryField { get; set; }

        public ObservableCollection<CategoryViewModel> Categories { get; set; }

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
            this.Categories = new ObservableCollection<CategoryViewModel>();
            this.SelectFields = new ObservableCollection<string>();
            this.boardView = boardView;
        }

        public void LoadBoard(Board board) {
            this.Categories.Clear();
            this.CategoryField = null;

            this.ColumnField = board.ColumnField.Name;
            this.CategoryField = board.CategoryField?.Name;

            if (board.CategoryField != null) {
                for (int i = 0; i < board.CategoryField.PossibleValues.Count; i++) {
                    CategoryViewModel currentCategory = new CategoryViewModel(board.CategoryField.PossibleValues[i]) {
                        EditCard = this.boardView.EditCard,
                        MoveCard = this.boardView.MoveCard
                    };

                    currentCategory.LoadBoard(board);
                    this.Categories.Add(currentCategory);
                }
            }
            else {

                CategoryViewModel defaultCategory = new CategoryViewModel(null) {
                    EditCard = this.boardView.EditCard,
                    MoveCard = this.boardView.MoveCard
                };

                defaultCategory.LoadBoard(board);
                this.Categories.Add(defaultCategory);
            }

            List<string> selectFields = new List<string>();

            for (int i = 0; i < board.Structure.Fields.Count; i++) {
                if(board.Structure.Fields[i].Type == FieldType.Select) {
                    selectFields.Add(board.Structure.Fields[i].Name);
                }
            }

            this.SelectFields = new ObservableCollection<string>(selectFields);
        }
    }
}
