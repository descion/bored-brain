using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BoredBrain.Serialization {

    public static class LegacyBoardSerializer {

        //---------------------------------------------------------------------------

        private const string CARD_LIST = "CARDLIST";

        //---------------------------------------------------------------------------

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

            File.WriteAllText(Path.Combine(board.Path, ".bbs"), LegacyStructureSerializer.Serialize(board.Structure));            

            List<string> cardOrder = new List<string>();
            foreach(Card card in board.Cards) {
                File.WriteAllText(Path.Combine(board.Path, card.Id.ToString() + ".bbc"), LegacyCardSerializer.Serialize(card));
                cardOrder.Add(card.Id.ToString());
            }

            boardFileContent.AppendLine(CARD_LIST);
            for (int i = 0; i < cardOrder.Count; i++) {
                boardFileContent.AppendLine(cardOrder[i]);
            }

            File.WriteAllText(Path.Combine(board.Path, ".bbb"), boardFileContent.ToString());
        }

        //---------------------------------------------------------------------------

        public static void Load(Board board) {

            string boardFile = Directory.GetFiles(board.Path, ".bbb")[0];

            List<string> cardOrder = new List<string>();

            using (StreamReader boardReader = new StreamReader(boardFile)) {
                string boardId = boardReader.ReadLine();
                string structurePath = boardReader.ReadLine();

                Structure boardStructure = LegacyStructureSerializer.Deserialize(File.ReadAllText(Path.Combine(board.Path, structurePath)));
                board.Structure = boardStructure;

                board.ColumnField = board.Structure.GetFieldByName(boardReader.ReadLine());
                board.CategoryField = board.Structure.GetFieldByName(boardReader.ReadLine());

                board.Id = Guid.Parse(boardId);

                string currentLine = boardReader.ReadLine();

                while(currentLine != null) {
                    if(currentLine != CARD_LIST) {
                        cardOrder.Add(currentLine);
                    }

                    currentLine = boardReader.ReadLine();
                }           
            }

            string[] cardFiles = Directory.GetFiles(board.Path, "*.bbc");

            Dictionary<string, Card> cards = new Dictionary<string, Card>();

            for (int i = 0; i < cardFiles.Length; i++) {
                Card currentCard = LegacyCardSerializer.Deserialize(File.ReadAllText(cardFiles[i]), board.Structure);
                cards.Add(currentCard.Id.ToString(), currentCard);
            }

            for (int i = 0; i < cardOrder.Count; i++) {
                if (cards.ContainsKey(cardOrder[i])) {
                    board.AddCard(cards[cardOrder[i]]);
                    cards.Remove(cardOrder[i]);
                }
            }

            foreach (KeyValuePair<string, Card> unorderedCard in cards) {
                board.AddCard(unorderedCard.Value);
            }
        }
    }
}
