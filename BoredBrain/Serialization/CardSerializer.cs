using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BoredBrain.Serialization {
    public static class CardSerializer {

        //---------------------------------------------------------------------------

        private class CardJSON {
            public string ID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public Dictionary<string, string> FieldValues { get; set; }
        }

        //---------------------------------------------------------------------------

        public static string Serialize(Card card) {
            CardJSON cardJson = new CardJSON(){
                ID = card.Id.ToString(),
                Title = card.Title,
                Content = card.Content,
                FieldValues = new Dictionary<string, string>()
            };

            for (int i = 0; i < card.Structure.Fields.Count; i++) {
                Field currentField = card.Structure.Fields[i];

                object currentFieldValue = currentField.GetDefaultValue();

                if (card.Fields.ContainsKey(currentField)) {
                    currentFieldValue = card.Fields[currentField];
                }

                string fieldValue = currentField.ConvertValueToString(currentFieldValue);
                cardJson.FieldValues.Add(currentField.Name, fieldValue);
            }

            return JsonSerializer.Serialize(cardJson);
        }

        //---------------------------------------------------------------------------

        public static Card Deserialize(string cardJsonString, Structure structure) {
            CardJSON cardJson = JsonSerializer.Deserialize<CardJSON>(cardJsonString);

            Card card = new Card(structure) {
                Id = new Guid(cardJson.ID),
                Title = cardJson.Title,
                Content = cardJson.Content
            };

            foreach(KeyValuePair<string, string> fieldValue in cardJson.FieldValues) {
                Field field = structure.GetFieldByName(fieldValue.Key);
                
                if (field != null) {
                    card.Fields.Add(field, field.ConvertStringToValue(fieldValue.Value));
                }
            }

            return card;
        }
    }
}
