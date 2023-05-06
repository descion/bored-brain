using BoredBrain.Models;
using BoredBrain.Serialization;
using BoredBrain.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl {

        private Board board;

        private BoardViewModel boardVM;

        private EditCardView currentCardView;

        public BoardView() {
            InitializeComponent();


            this.boardVM = new BoardViewModel(this);
            this.DataContext = this.boardVM;

            this.InitializeBoard("SavedBoard");
        }

        private void InitializeBoard(string path) {
            if (this.board != null) {
                this.board.OnBoardChanged -= this.OnBoardChanged;
            }

            if (!File.Exists(Path.Combine(path, ".bbb"))) {

                Board templateBoard = new Board(path);
                SimpleSelectField statusField = new SimpleSelectField() {
                    Name = "Status",
                    PossibleValues = new List<string> {
                        "None"
                    }
                };

                templateBoard.Structure.AddField(statusField);
                templateBoard.ColumnField = statusField;

                BoardSerializer.Save(templateBoard);
            }

            this.board = new Board(path);
            BoardSerializer.Load(this.board);

            this.board.OnBoardChanged += this.OnBoardChanged;
            this.boardVM.LoadBoard(this.board);
        }

        private void OnBoardChanged() {
            BoardSerializer.Save(this.board);
            this.boardVM.LoadBoard(this.board);
        }

        private void CreateBoard_Click(object sender, RoutedEventArgs e) {
            using (System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog()) {

                folderDialog.ShowDialog();
                string boardPath = folderDialog.SelectedPath;

                this.InitializeBoard(boardPath);
            }
        }

        private void OpenBoard_Click(object sender, RoutedEventArgs e) {
            using (System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog()) {
                openDialog.Filter = "Bored Brain Board Files (*.bbb)|*.bbb";
                if (openDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel) {
                    this.InitializeBoard(new FileInfo(openDialog.FileName).DirectoryName);
                }
            }
        }

        private void EditStructure_Click(object sender, RoutedEventArgs e) {
            StructureView addView = new StructureView(this.board);

            this.MainPanel.Children.Add(addView);

            addView.OnClose += (bool success) => {
                
                this.MainPanel.Children.Remove(addView);

                this.OnBoardChanged();
            };
        }

        private void CreateColumn_Click(object sender, RoutedEventArgs e) {
            AddColumnView addView = new AddColumnView();

            this.MainPanel.Children.Add(addView);

            addView.OnClose = (bool success) => {
                if (success) {
                    this.board.ColumnField.PossibleValues.Add(addView.GetName());
                }

                this.MainPanel.Children.Remove(addView);

                this.OnBoardChanged();
            };
        }

        private void CreateCard_Click(object sender, RoutedEventArgs e) {
            Card card = this.board.CreateCard();

            this.currentCardView = new EditCardView(card, this.OnCloseEditDialog, this.OnCreateCard, null);
            this.MainPanel.Children.Add(this.currentCardView);
        }

        private void OnCreateCard(Card card) {
            this.board.AddCard(card);
            this.CloseEditDialog();
        }


        private void OnCloseEditDialog(Card card) {
            this.CloseEditDialog();
        }

        private void OnDeleteCard(Card card) {
            this.board.RemoveCard(card);
            this.CloseEditDialog();
        }

        private void CloseEditDialog() {
            this.MainPanel.Children.Remove(this.currentCardView);
            this.OnBoardChanged();
        }

        public void EditCard(Card card) {
            
            this.currentCardView = new EditCardView(card, this.OnCloseEditDialog, this.OnCloseEditDialog, this.OnDeleteCard);
            this.MainPanel.Children.Add(this.currentCardView);
        }

        public void MoveCard(Card cardToMove, Card referenceCard, CardMoveMode mode) {
            this.board.MoveCard(cardToMove, referenceCard, mode);
        }

        private void ColumnField_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            this.board.ColumnField = this.board.Structure.GetFieldByName(this.boardVM.ColumnField);
            this.OnBoardChanged();
        }

        private void CategoryField_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.board.CategoryField = this.board.Structure.GetFieldByName(this.boardVM.CategoryField);
            this.OnBoardChanged();
        }

        private void ClearCategory_Click(object sender, RoutedEventArgs e) {
            this.board.CategoryField = null;
            this.CategoryField.SelectedIndex = -1;
            this.OnBoardChanged();
        }
    }
}
