using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public class Board {

        //---------------------------------------------------------------------------

        public Guid Id { get; set; }

        public string Path { get; set; }

        public Structure Structure { get; set; }

        public Field ColumnField { get; set; }

        public Field CategoryField { get; set; }

        public LinkedList<Card> Cards { get; private set; }

        public List<Card> ArchivedCards { get; private set; }

        //---------------------------------------------------------------------------

        public delegate void OnBoardChangedDelegate();

        public event OnBoardChangedDelegate OnBoardChanged;

        //---------------------------------------------------------------------------

        public Board(string path) {
            this.Id = Guid.NewGuid();
            this.Path = path;
            this.Structure = new Structure();
            this.Cards = new LinkedList<Card>();
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
            this.Cards.AddLast(card);
            card.OnDataChanged += this.OnCardChanged;
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public void MoveToEnd(Card card) {
            this.Cards.Remove(card);
            this.Cards.AddLast(card);
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public void MoveCard(Card cardToMove, Card referenceCard, CardMoveMode mode) {
            cardToMove.SetFieldValue(this.ColumnField, referenceCard.GetFieldValue(this.ColumnField));
            this.Cards.Remove(cardToMove);

            if (mode == CardMoveMode.Before) {
                this.Cards.AddBefore(this.Cards.Find(referenceCard), cardToMove);
            }
            else {
                this.Cards.AddAfter(this.Cards.Find(referenceCard), cardToMove);
            }

            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public void RemoveCard(Card card) {
            this.Cards.Remove(card);
            this.ArchivedCards.Add(card);
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        private void OnCardChanged(Card c) {
            this.OnBoardChanged?.Invoke();
        }

        //---------------------------------------------------------------------------

        public List<Card> GetCardsFiltered(Field field, object value) {

            List<Card> filteredCards = new List<Card>();

            foreach (Card card in this.Cards) {
                if (card.GetFieldValue(field).Equals(value)) {
                    filteredCards.Add(card);
                }
            }

            return filteredCards;
        }

        //---------------------------------------------------------------------------

        public void Validate() {
            foreach (Card card in this.Cards) {
                card.Validate();
            }
        }
    }
}
