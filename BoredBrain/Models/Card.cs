using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public class Card {

        //---------------------------------------------------------------------------

        public Guid Id { get; set; }

        public Structure Structure { get; set; }

        private string title;
        public string Title { get { return this.title; } set { this.title = value; this.OnDataChanged?.Invoke(this); } }

        private string content;
        public string Content { get { return this.content; } set { this.content = value; this.OnDataChanged?.Invoke(this); } }

        public Dictionary<Field, object> Fields { get; set; }

        //---------------------------------------------------------------------------

        public delegate void OnCardDataChangedDelegate(Card c);

        public event OnCardDataChangedDelegate OnDataChanged;

        //---------------------------------------------------------------------------

        public Card(Structure structure) {
            this.Id = Guid.NewGuid();
            this.Structure = structure;
            this.Fields = new Dictionary<Field, object>();
        }

        //---------------------------------------------------------------------------

        public void SetFieldValue(Field field, object value) {
            this.Fields[field] = value;
            this.OnDataChanged?.Invoke(this);
        }

        //---------------------------------------------------------------------------

        public object GetFieldValue(Field field) {
            if (this.Fields.ContainsKey(field)) {
                return this.Fields[field];
            }

            return field.GetDefaultValue();
        }
    }
}
