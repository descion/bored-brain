using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BoredBrain.Serialization {
    public static class BoardSerializer {

        //---------------------------------------------------------------------------

        private const string BOARD_FILE = "board.json";
        private const string STRUCTURE_FILE = "structure.json";
        private const string CARD_FOLDER = "cards";

        //---------------------------------------------------------------------------

        private class BoardJSON {
            public string ID { get; set; }
            public string StructureFilePath { get; set; }
            public string ColumnField { get; set; }
            public string CategoryField { get; set; }
            public List<string> CardOrder { get; set; }
        }

        //---------------------------------------------------------------------------

        public static void Save(Board board) {
            BoardJSON boardJson = new BoardJSON() {
                ID = board.Id.ToString(),
                ColumnField = board.ColumnField?.Name,
                CategoryField = board.CategoryField?.Name,
                CardOrder = new List<string>()
            };

            Directory.CreateDirectory(board.Path);
            Directory.CreateDirectory(Path.Combine(board.Path, CARD_FOLDER));

            foreach (Card card in board.Cards) {
                File.WriteAllText(Path.Combine(board.Path, CARD_FOLDER, card.Id.ToString() + ".json"), CardSerializer.Serialize(card));
                boardJson.CardOrder.Add(card.Id.ToString());
            }

            File.WriteAllText(Path.Combine(board.Path, STRUCTURE_FILE), StructureSerializer.Serialize(board.Structure));
            File.WriteAllText(Path.Combine(board.Path, BOARD_FILE), JsonSerializer.Serialize(boardJson));
        }

        //---------------------------------------------------------------------------

        public static void Load(Board board) {

            if(File.Exists(Path.Combine(board.Path, ".bbb"))) {
                LegacyBoardSerializer.Load(board);

                Directory.Delete(board.Path, true);
                Save(board);
            }

            BoardJSON boardJson = JsonSerializer.Deserialize<BoardJSON>(File.ReadAllText(Path.Combine(board.Path, BOARD_FILE)));
            board.Structure  = StructureSerializer.Deserialize(File.ReadAllText(Path.Combine(board.Path, STRUCTURE_FILE)));
            board.Id = new Guid(boardJson.ID);
            board.ColumnField = board.Structure.GetFieldByName(boardJson.ColumnField);
            board.CategoryField = board.Structure.GetFieldByName(boardJson.CategoryField);

            string[] cardFiles = Directory.GetFiles(Path.Combine(board.Path, CARD_FOLDER), "*.json");

            Dictionary<string, Card> cards = new Dictionary<string, Card>();

            for (int i = 0; i < cardFiles.Length; i++) {
                Card currentCard = CardSerializer.Deserialize(File.ReadAllText(cardFiles[i]), board.Structure);
                cards.Add(currentCard.Id.ToString(), currentCard);
            }

            for (int i = 0; i < boardJson.CardOrder.Count; i++) {
                if (cards.ContainsKey(boardJson.CardOrder[i])) {
                    board.AddCard(cards[boardJson.CardOrder[i]]);
                    cards.Remove(boardJson.CardOrder[i]);
                }
            }

            foreach (KeyValuePair<string, Card> unorderedCard in cards) {
                board.AddCard(unorderedCard.Value);
            }
        }
    }
}
