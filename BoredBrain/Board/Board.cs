using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain {
    public class Board {

        private Guid id;
        private string path;

        public Structure Structure { get; set; }

        public Guid ColumnField { get; set; }
        public Guid CategoryField { get; set; }

        public List<Card> Cards { get; private set; }

        public delegate void OnBoardChangedDelegate();

        public event OnBoardChangedDelegate OnBoardChanged;

        public Board(string path) {
            this.id = Guid.NewGuid();
            this.path = path;
            this.Structure = new Structure();
            this.Cards = new List<Card>();
        }

        public Board(string path, Guid id, Structure structure) : this(path) {
            this.id = id;
            this.Structure = structure;
        }

        public Card CreateCard() {
            return new Card(this.Structure);
        }

        public void AddCard(Card card) {
            this.Cards.Add(card);
            card.OnDataChanged += this.OnCardChanged;
        }

        private void OnCardChanged(Card c) {
            this.OnBoardChanged?.Invoke();
        }

        public void Save() {

            if (!Directory.Exists(this.path)) {
                Directory.CreateDirectory(this.path);
            }

            StringBuilder boardFileContent = new StringBuilder();
            boardFileContent.AppendLine(this.id.ToString());
            boardFileContent.AppendLine(".bbs");
            boardFileContent.AppendLine(this.ColumnField.ToString());
            boardFileContent.AppendLine(this.CategoryField.ToString());

            File.WriteAllText(Path.Combine(this.path, ".bbs"), this.Structure.Serialize());
            File.WriteAllText(Path.Combine(this.path, ".bbb"), boardFileContent.ToString());

            for (int i = 0; i < this.Cards.Count; i++) {
                File.WriteAllText(Path.Combine(this.path, this.Cards[i].Id.ToString() + ".bbc"), this.Cards[i].Serialize());
            }
        }

        public void Load() {

            string boardFile = Directory.GetFiles(this.path, ".bbb")[0];

            using (StreamReader boardReader = new StreamReader(boardFile)) {
                string boardId = boardReader.ReadLine();
                string structurePath = boardReader.ReadLine();
                this.ColumnField = Guid.Parse(boardReader.ReadLine());
                this.CategoryField = Guid.Parse(boardReader.ReadLine());

                Structure boardStructure = new Structure();
                boardStructure.Deserialize(File.ReadAllText(Path.Combine(this.path, structurePath)));

                this.id = Guid.Parse(boardId);
                this.Structure = boardStructure;
            }

            string[] cardFiles = Directory.GetFiles(this.path, "*.bbc");

            for (int i = 0; i < cardFiles.Length; i++) {
                Card c = new Card(this.Structure);

                c.Deserialize(File.ReadAllText(cardFiles[i]));

                this.AddCard(c);
            }
        }
    }
}
