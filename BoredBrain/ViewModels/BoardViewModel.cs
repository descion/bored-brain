using BoredBrain.Models;
using BoredBrain.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace BoredBrain.ViewModels {

    class BoardViewModel : INotifyPropertyChanged {

        //---------------------------------------------------------------------------

        private string name;
        public string Name { 
            get {
                return this.name;
            }
            set {
                this.name = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private string columnField;
        public string ColumnField {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColumnField"));
            }
        }

        private string categoryField;
        public string CategoryField {
            get {
                return this.categoryField;
            }
            set {
                this.categoryField = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CategoryField"));
            }
        }

        public ObservableCollection<CategoryViewModel> Categories { get; set; }

        private ObservableCollection<string> selectFields;
        public ObservableCollection<string> SelectFields { 
            get {
                return this.selectFields;
            }
            set {
                this.selectFields = value;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectFields"));
            }
        }

        private ObservableCollection<string> columnValues;
        public ObservableCollection<string> ColumnValues {
            get {
                return this.columnValues;
            }
            set {
                this.columnValues = value;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColumnValues"));
            }
        }

        private BoardView boardView;

        public event PropertyChangedEventHandler PropertyChanged;
        
        //---------------------------------------------------------------------------


        public BoardViewModel(BoardView boardView) {
            this.Categories = new ObservableCollection<CategoryViewModel>();
            this.SelectFields = new ObservableCollection<string>();
            this.boardView = boardView;
        }

        //---------------------------------------------------------------------------

        public void LoadBoard(Board board) {

            this.Name = new DirectoryInfo(board.Path).Name;

            this.Categories.Clear();
            this.columnField = board.ColumnField.Name;
            this.categoryField = board.CategoryField?.Name;

            this.ColumnValues = new ObservableCollection<string>(board.ColumnField.PossibleValues);

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
