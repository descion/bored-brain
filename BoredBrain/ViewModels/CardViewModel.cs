using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoredBrain.ViewModels {
    public class CardViewModel {

        //---------------------------------------------------------------------------

        public Card Card { get; set; }

        public ObservableCollection<FieldDefinition> PreviewFields { get; set; }

        public Action<Card> OnEditCard { get; set; }

        public Action<Card, Card, CardMoveMode> OnMoveCard { get; set; }

        //---------------------------------------------------------------------------

        public CardViewModel(Card card, Action<Card> OnEditCard, Action<Card, Card, CardMoveMode> OnMoveCard) {
            this.Card = card;
            this.OnEditCard = OnEditCard;
            this.OnMoveCard = OnMoveCard;

            this.PreviewFields = new ObservableCollection<FieldDefinition>();

            foreach (KeyValuePair<Field, object> field in this.Card.Fields) {
                if (field.Key.ShowOnCard) {
                    this.PreviewFields.Add(new FieldDefinition() {
                        Name = field.Key.Name,
                        Type = field.Key.Type.ToString(),
                        Value = field.Value
                    });
                }
            }
        }        
    }
}
