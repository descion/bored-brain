using BoredBrain.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public static class BoardSerializer {

        public static void Save(Board board) {

            board.Validate();

            if (!Directory.Exists(board.Path)) {
                Directory.CreateDirectory(board.Path);
            }

            StringBuilder boardFileContent = new StringBuilder();
            boardFileContent.AppendLine(board.Id.ToString());
            boardFileContent.AppendLine(".bbs");
            boardFileContent.AppendLine(board.ColumnField != null ? board.ColumnField.Name : "");
            boardFileContent.AppendLine(board.CategoryField != null ? board.CategoryField.Name : "");

            File.WriteAllText(Path.Combine(board.Path, ".bbs"), StructureSerializer.Serialize(board.Structure));
            File.WriteAllText(Path.Combine(board.Path, ".bbb"), boardFileContent.ToString());

            for (int i = 0; i < board.Cards.Count; i++) {
                File.WriteAllText(Path.Combine(board.Path, board.Cards[i].Id.ToString() + ".bbc"), CardSerializer.Serialize(board.Cards[i]));
            }
        }

        //---------------------------------------------------------------------------

        public static void Load(Board board) {

            string boardFile = Directory.GetFiles(board.Path, ".bbb")[0];

            using (StreamReader boardReader = new StreamReader(boardFile)) {
                string boardId = boardReader.ReadLine();
                string structurePath = boardReader.ReadLine();

                Structure boardStructure = StructureSerializer.Deserialize(File.ReadAllText(Path.Combine(board.Path, structurePath)));
                board.Structure = boardStructure;

                board.ColumnField = board.Structure.GetFieldByName(boardReader.ReadLine());
                board.CategoryField = board.Structure.GetFieldByName(boardReader.ReadLine());

                board.Id = Guid.Parse(boardId);
            }

            string[] cardFiles = Directory.GetFiles(board.Path, "*.bbc");

            for (int i = 0; i < cardFiles.Length; i++) {
                board.AddCard(CardSerializer.Deserialize(File.ReadAllText(cardFiles[i]), board.Structure));
            }
        }
    }

    public class Board {

        //---------------------------------------------------------------------------

        public Guid Id { get; set; }

        public string Path { get; set; }

        public Structure Structure { get; set; }

        public Field ColumnField { get; set; }

        public Field CategoryField { get; set; }

        public List<Card> Cards { get; private set; }

        public List<Card> ArchivedCards { get; private set; }

        //---------------------------------------------------------------------------

        public delegate void OnBoardChangedDelegate();

        public event OnBoardChangedDelegate OnBoardChanged;

        //---------------------------------------------------------------------------

        public Board(string path) {
            this.Id = Guid.NewGuid();
            this.Path = path;
            this.Structure = new Structure();
            this.Cards = new List<Card>();
        }

        //---------------------------------------------------------------------------

        public Board(string path, Guid id, Structure structure) : this(path) {
            this.Id = id;
            this.Structure = structure;
        }

        //---------------------------------------------------------------------------

        public Card CreateCard() {
            return new Card(this.Structure);
        }

        //---------------------------------------------------------------------------

        public void AddCard(Card card) {
            this.Cards.Add(card);
            card.OnDataChanged += this.OnCardChanged;
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public void RemoveCard(Card card) {
            this.Cards.Remove(card);
            this.ArchivedCards.Add(card);
        }

        //---------------------------------------------------------------------------

        private void OnCardChanged(Card c) {
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public List<Card> GetCardsFiltered(Field field, object value) {
            return this.Cards.FindAll(
                (Card c) => { 
                    return c.GetFieldValue(field).Equals(value); 
                }
            );
        }

        //---------------------------------------------------------------------------

        public void Validate() {
            for (int itCards = 0; itCards < this.Cards.Count; itCards++) {
                this.Cards[itCards].Validate();
            }
        }
    }
}
