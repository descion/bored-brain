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

        public CardViewModel(Card card, Action<Card> OnEditCard) {
            this.Card = card;
            this.OnEditCard = OnEditCard;
        }        
    }
}
