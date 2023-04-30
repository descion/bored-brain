using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain {
    public class Card {

        private const string CONTENT_START = ":::CONTENTSTART:::";

        public Guid Id { get; private set; }

        private Structure structure;

        private string title;
        public string Title { get { return this.title; } set { this.title = value; this.OnDataChanged?.Invoke(this); } }

        private string content;
        public string Content { get { return this.content; } set { this.content = value; this.OnDataChanged?.Invoke(this); } }

        private Dictionary<Guid, object> Fields { get; set; }

        public delegate void OnCardDataChangedDelegate(Card c);

        public event OnCardDataChangedDelegate OnDataChanged;

        public Card(Structure structure) {
            this.Id = Guid.NewGuid();
            this.structure = structure;
            this.Fields = new Dictionary<Guid, object>();
        }

        public void SetField(Guid fieldId, object value) {
            this.Fields[fieldId] = value;
            this.OnDataChanged?.Invoke(this);
        }

        public object GetField(Guid fieldId) {
            return this.Fields[fieldId];
        }

        public string Serialize() {
            StringBuilder cardContent = new StringBuilder();

            cardContent.AppendLine(this.Id.ToString());
            cardContent.AppendLine(this.Title);

            for (int i = 0; i < this.structure.Fields.Count; i++) {
                Field currentField = this.structure.Fields[i];

                string fieldValue = "";

                if (this.Fields.ContainsKey(currentField.Id)) {
                    fieldValue = currentField.SerializeValue(this.Fields[currentField.Id]);
                }

                cardContent.AppendLine(currentField.Id+ ":" + fieldValue);
            }

            cardContent.AppendLine(CONTENT_START);
            cardContent.Append(this.Content);

            return cardContent.ToString();
        }

        public void Deserialize(string cardString) {
            using (StringReader contentReader = new StringReader(cardString)) { 

                this.Id = Guid.Parse(contentReader.ReadLine());
                this.Title = contentReader.ReadLine();

                string currentLine = contentReader.ReadLine();

                while (currentLine != CONTENT_START) {

                    Guid fieldId = Guid.Parse(currentLine.Substring(0, currentLine.IndexOf(':')));
                    string fieldContent = currentLine.Substring(currentLine.IndexOf(':') + 1);

                    for (int i = 0; i < this.structure.Fields.Count; i++) {
                        Field field = this.structure.Fields[i];

                        if (field.Id == fieldId) {
                            object fieldValue = field.DeserializeValue(fieldContent);
                            this.Fields.Add(field.Id, fieldValue);
                        }
                    }

                    currentLine = contentReader.ReadLine();
                }

                this.Content = contentReader.ReadToEnd();
            }
        }
    }
}
