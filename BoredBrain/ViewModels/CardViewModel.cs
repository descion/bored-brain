using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.ViewModels {
    public class CardViewModel {

        public Card Card { get; set; }

        public Action<Card> OnEditCard { get; set; }

        public Action<Card, Card, CardMoveMode> OnMoveCard { get; set; }

        public CardViewModel(Card card, Action<Card> OnEditCard, Action<Card, Card, CardMoveMode> OnMoveCard) {
            this.Card = card;
            this.OnEditCard = OnEditCard;
            this.OnMoveCard = OnMoveCard;
        }        
    }
}
