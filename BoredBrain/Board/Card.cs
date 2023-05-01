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

        private Dictionary<Field, object> Fields { get; set; }

        public delegate void OnCardDataChangedDelegate(Card c);

        public event OnCardDataChangedDelegate OnDataChanged;

        public Card(Structure structure) {
            this.Id = Guid.NewGuid();
            this.structure = structure;
            this.Fields = new Dictionary<Field, object>();
        }

        public void SetFieldValue(Field field, object value) {
            this.Fields[field] = value;
            this.OnDataChanged?.Invoke(this);
        }

        public object GetFieldValue(Field field) {
            if (this.Fields.ContainsKey(field)) {
                return this.Fields[field];
            }

            return field.GetDefault();
        }

        public string Serialize() {
            StringBuilder cardContent = new StringBuilder();

            cardContent.AppendLine(this.Id.ToString());
            cardContent.AppendLine(this.Title);

            for (int i = 0; i < this.structure.Fields.Count; i++) {
                Field currentField = this.structure.Fields[i];

                object currentFieldValue = currentField.GetDefault();

                if (this.Fields.ContainsKey(currentField)) {
                    currentFieldValue = this.Fields[currentField];
                }

                string fieldValue = currentField.SerializeValue(currentFieldValue);
                cardContent.AppendLine(currentField.Name + ":" + fieldValue);
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

                    string fieldName = currentLine.Substring(0, currentLine.IndexOf(':'));
                    string fieldContent = currentLine.Substring(currentLine.IndexOf(':') + 1);

                    for (int i = 0; i < this.structure.Fields.Count; i++) {
                        Field field = this.structure.Fields[i];

                        if (field.Name == fieldName) {
                            object fieldValue = field.DeserializeValue(fieldContent);
                            this.Fields.Add(field, fieldValue);
                        }
                    }

                    currentLine = contentReader.ReadLine();
                }

                this.Content = contentReader.ReadToEnd();
            }
        }
    }
}
