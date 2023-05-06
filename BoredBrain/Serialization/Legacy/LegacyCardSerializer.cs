using BoredBrain.Models;
using System;
using System.IO;
using System.Text;

namespace BoredBrain.Serialization {

    public static class LegacyCardSerializer {

        private const string CONTENT_START = ":::CONTENTSTART:::";

        public static string Serialize(Card card) {
            StringBuilder cardContent = new StringBuilder();

            cardContent.AppendLine(card.Id.ToString());
            cardContent.AppendLine(card.Title);

            for (int i = 0; i < card.Structure.Fields.Count; i++) {
                Field currentField = card.Structure.Fields[i];

                object currentFieldValue = currentField.GetDefaultValue();

                if (card.Fields.ContainsKey(currentField)) {
                    currentFieldValue = card.Fields[currentField];
                }

                string fieldValue = currentField.ConvertValueToString(currentFieldValue);
                cardContent.AppendLine(currentField.Name + ":" + fieldValue);
            }

            cardContent.AppendLine(CONTENT_START);
            cardContent.Append(card.Content);

            return cardContent.ToString();
        }

        public static Card Deserialize(string cardString, Structure structure) {
            Card card = new Card(structure);

            using (StringReader contentReader = new StringReader(cardString)) {

                card.Id = Guid.Parse(contentReader.ReadLine());
                card.Title = contentReader.ReadLine();

                string currentLine = contentReader.ReadLine();

                while (currentLine != CONTENT_START) {

                    string fieldName = currentLine.Substring(0, currentLine.IndexOf(':'));
                    string fieldContent = currentLine.Substring(currentLine.IndexOf(':') + 1);

                    for (int i = 0; i < card.Structure.Fields.Count; i++) {
                        Field field = card.Structure.Fields[i];

                        if (field.Name == fieldName) {
                            object fieldValue = field.ConvertStringToValue(fieldContent);
                            card.Fields.Add(field, fieldValue);
                        }
                    }

                    currentLine = contentReader.ReadLine();
                }

                card.Content = contentReader.ReadToEnd();
            }

            return card;
        }
    }
}
