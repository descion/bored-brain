using BoredBrain;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private Board board;

        public MainWindow() {
            InitializeComponent();

            this.board = this.CreateTestboard();
            this.board.OnBoardChanged += this.OnBoardChanged;
            this.ConstructBoard();
        }

        private void Clear() {
            this.MainPanel.Children.Clear();
        }

        private void ConstructBoard() {
            SelectField columnField = (SelectField)this.board.Structure.GetFieldById(this.board.ColumnField);

            for (int i = 0; i < columnField.PossibleValues.Count; i++) {
                this.CreateColumn(columnField.Id, columnField.PossibleValues[i]);
            }
        }

        private Column CreateColumn(Guid fieldId, string fieldValue) {
            Column c1 = new Column(fieldId, fieldValue);
            c1.SetValue(Grid.ColumnProperty, 0);

            MainPanel.Children.Add(c1);

            for (int i = 0; i < this.board.Cards.Count; i++) {
                c1.AddCard(new CardElement(this.board.Cards[0]));
            }

            return c1;
        }

        private void OnBoardChanged() {
            this.board.Save();
            this.Clear();
            this.ConstructBoard();
        }

        private Board CreateTestboard() {
            //if (Directory.Exists("TestFolder")) {
            //    Directory.Delete("TestFolder", true);
            //}

            //Directory.CreateDirectory("TestFolder");

            Board b = new Board("TestFolder");

            b.Load();

            //Field test1 = new TextField() {
            //    Name = "FirstTestField"
            //};

            //b.Structure.AddField(test1);

            //Field test2 = new MultiselectField() {
            //    Name = "SecondTestField"
            //};

            //b.Structure.AddField(test2);

            //SimpleSelectField test3 = new SimpleSelectField() {
            //    Name = "Status"
            //};

            //test3.PossibleValues.Add("ToDo");
            //test3.PossibleValues.Add("In Progress");
            //test3.PossibleValues.Add("Blocked");
            //test3.PossibleValues.Add("Done");

            //b.Structure.AddField(test3);
            //b.ColumnField = test3.Id;

            //Card newCard = b.CreateCard();

            //newCard.Title = "My first Card!";
            //newCard.Content = "Main content Stuff with all the nice things that you need!\n[] Done!";

            //newCard.SetField(test1.Id, "This is my field1 value.");
            //newCard.SetField(test2.Id, new string[] { "Tag1", "Tag2", "Tag3" });
            //newCard.SetField(test3.Id, test3.PossibleValues[1]);

            //b.AddCard(newCard);

            //b.Save();

            return b;
        }
    }
}
