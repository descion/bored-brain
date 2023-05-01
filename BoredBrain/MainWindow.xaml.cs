﻿using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace BoredBrain {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private Board board;

        private Card cardInEditMode;

        public MainWindow() {
            InitializeComponent();

            this.InitializeBoard("SavedBoard");
            
        }

        //---------------------------------------------------------------------------

        private void InitializeBoard(string path) {
            if(this.board != null) {
                this.board.OnBoardChanged -= this.OnBoardChanged;
            }

            if (!File.Exists(Path.Combine(path, ".bbb"))) {
                
                Board templateBoard = new Board(path);
                SimpleSelectField statusField = new SimpleSelectField() {
                    Name = "Status"
                };

                templateBoard.Structure.AddField(statusField);
                templateBoard.ColumnField = statusField;

                templateBoard.Save();
            }

            this.board = new Board(path);
            this.board.Load();

            this.board.OnBoardChanged += this.OnBoardChanged;
            this.ConstructBoard();
        }

        //---------------------------------------------------------------------------

        private void ConstructBoard() {
            this.MainPanel.Children.Clear();
            SelectField columnField = (SelectField)this.board.ColumnField;

            for (int i = 0; i < columnField.PossibleValues.Count; i++) {
                this.CreateColumn(columnField, columnField.PossibleValues[i]);
            }
        }

        //---------------------------------------------------------------------------

        private Column CreateColumn(Field field, string fieldValue) {
            Column c1 = new Column(field, fieldValue);
            c1.SetValue(Grid.ColumnProperty, 0);

            MainPanel.Children.Add(c1);

            for (int i = 0; i < this.board.Cards.Count; i++) {
                CardElement newCardElement = new CardElement(this.board.Cards[i]);
                c1.AddCard(newCardElement);
                newCardElement.OnEditCard += this.EditCard;
            }

            return c1;
        }

        //---------------------------------------------------------------------------

        private void OnBoardChanged() {
            this.board.Save();
            this.ConstructBoard();
        }

        //---------------------------------------------------------------------------

        private void CreateColumn_Click(object sender, RoutedEventArgs e) {

            List<InputDefinition> inputs = new List<InputDefinition>() {
                new InputDefinition() {
                    name = this.board.ColumnField.Name,
                    type = FieldType.Text,
                    value = ""
                }
            };

            this.Dialog.Open("Create Column", "Create", inputs, this.OnCreateColumn);
        }

        //---------------------------------------------------------------------------

        private void CreateCard_Click(object sender, RoutedEventArgs e) {
            this.Dialog.Open("Create Card", "Create", this.GetInputDefinitionsForCard(), this.OnCreateCard);
        }

        //---------------------------------------------------------------------------

        private void CreateBoard_Click(object sender, RoutedEventArgs e) {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog()) {

                folderDialog.ShowDialog();
                string boardPath = folderDialog.SelectedPath;

                this.InitializeBoard(boardPath);
            }
        }

        //---------------------------------------------------------------------------

        private void OpenBoard_Click(object sender, RoutedEventArgs e) {
            using (OpenFileDialog openDialog = new OpenFileDialog()) {
                openDialog.Filter = "Bored Brain Board Files (*.bbb)|*.bbb";
                if (openDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel) {
                    this.InitializeBoard(new FileInfo(openDialog.FileName).DirectoryName);
                }
            }

        }

        //---------------------------------------------------------------------------

        private void OnCreateCard(List<InputDefinition> fields) {

            Card newCard = this.board.CreateCard();

            this.ApplyValuesToCard(newCard, fields);

            this.board.AddCard(newCard);
        }

        //---------------------------------------------------------------------------

        private void OnSaveCard(List<InputDefinition> fields) {
            this.ApplyValuesToCard(this.cardInEditMode, fields);
        }

        //---------------------------------------------------------------------------

        private void OnCreateColumn(List<InputDefinition> fields) {
            string newColumn = (string)fields[0].value;

            ((SelectField)this.board.ColumnField).PossibleValues.Add(newColumn);
            this.board.Save();
            this.OnBoardChanged();
        }

        //---------------------------------------------------------------------------

        private void EditCard(Card card) {
            this.cardInEditMode = card;
            this.Dialog.Open("Edit Card", "Save", this.GetInputDefinitionsForCard(card), this.OnSaveCard);
        }

        //---------------------------------------------------------------------------

        private List<InputDefinition> GetInputDefinitionsForCard(Card c = null) {
            List<InputDefinition> inputs = new List<InputDefinition>() {
                new InputDefinition() {
                    name = "Title",
                    type = FieldType.Text,
                    value = c != null? c.Title : ""
                },

                new InputDefinition() {
                    name = "Content",
                    type = FieldType.Text,
                    value = c != null? c.Content : ""
                }
            };

            for (int i = 0; i < this.board.Structure.Fields.Count; i++) {
                Field currentField = this.board.Structure.Fields[i];

                InputDefinition input = null;

                switch (currentField.Type) {
                    case FieldType.Text:
                        input = new InputDefinition() {
                            name = currentField.Name,
                            type = currentField.Type,
                            value = c != null ? c.GetFieldValue(this.board.Structure.GetFieldByName(currentField.Name)) : ""
                        };
                        break;
                    case FieldType.Number:
                        input = new InputDefinition() {
                            name = currentField.Name,
                            type = currentField.Type,
                            value = c != null ? c.GetFieldValue(this.board.Structure.GetFieldByName(currentField.Name)) : 0
                        };
                        break;
                    case FieldType.Select:
                        List<string> possibleValues = ((SelectField)currentField).PossibleValues;

                        input = new SelectInputDefinition() {
                            name = currentField.Name,
                            type = currentField.Type,
                            value = c != null ? c.GetFieldValue(this.board.Structure.GetFieldByName(currentField.Name)) : (possibleValues.Count > 0 ? possibleValues[0] : ""),
                            possibleValues = possibleValues
                        };
                        break;
                    case FieldType.Multiselect:
                        List<string> possibleMultiValues = ((SelectField)currentField).PossibleValues;

                        input = new SelectInputDefinition() {
                            name = currentField.Name,
                            type = currentField.Type,
                            value = c != null ? c.GetFieldValue(this.board.Structure.GetFieldByName(currentField.Name)) : new string[0],
                            possibleValues = possibleMultiValues
                        };
                        break;
                    case FieldType.Date:
                        break;
                    default:
                        break;
                }

                inputs.Add(input);
            }

            return inputs;
        }

        //---------------------------------------------------------------------------

        private void ApplyValuesToCard(Card card, List<InputDefinition> fields) {
            InputDefinition titleInput = fields.Find((InputDefinition d) => { return d.name == "Title"; });
            fields.Remove(titleInput);
            InputDefinition contentInput = fields.Find((InputDefinition d) => { return d.name == "Content"; });
            fields.Remove(contentInput);

            card.Title = (string)titleInput.value;
            card.Content = (string)contentInput.value;

            for (int i = 0; i < fields.Count; i++) {
                card.SetFieldValue(this.board.Structure.GetFieldByName(fields[i].name), fields[i].value);
            }
        }

        //---------------------------------------------------------------------------

        private void EditStructure_Click(object sender, RoutedEventArgs e) {
            this.StructureDialog.Open(this.board, this.OnBoardChanged);
        }
    }
}
