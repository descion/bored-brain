using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain {

    public enum FieldType {
        Text,
        Number,
        Select,
        Multiselect,
        Date
    }

    public abstract class Field {

        private const string SERIALIZATION_SEPARATOR = ":::";

        public string Name { get; set; }
        public abstract FieldType Type { get; }

        public string Serialize() {
            return this.Type.ToString() + SERIALIZATION_SEPARATOR + this.Name + SERIALIZATION_SEPARATOR + this.SerializeData();
        }

        public void Deserialize(string fieldDefinition) {
            string[] contents = fieldDefinition.Split(new string[] { SERIALIZATION_SEPARATOR }, StringSplitOptions.None);

            this.Name = contents[1];

            this.DeserializeData(contents[2]);
        }

        protected virtual string SerializeData() { return string.Empty; }

        protected virtual void DeserializeData(string data) { }

        public abstract string SerializeValue(object value);

        public abstract object DeserializeValue(string valueString);

    }

    public class TextField : Field {
        public override FieldType Type => FieldType.Text;

        public override object DeserializeValue(string valueString) {
            return valueString;
        }

        public override string SerializeValue(object value) {
            return (string)value;
        }
    }

    public abstract class SelectField : Field {

        private const char VALUE_SEPARATOR = ';';
        
        public List<string> PossibleValues { get; protected set; }

        public SelectField() {
            this.PossibleValues = new List<string>();
        }

        protected override string SerializeData() {
            StringBuilder dataBuilder = new StringBuilder();

            for (int i = 0; i < this.PossibleValues.Count; i++) {
                dataBuilder.Append(this.PossibleValues[i]);
                dataBuilder.Append(VALUE_SEPARATOR);
            }

            return dataBuilder.ToString();
        }

        protected override void DeserializeData(string data) {
            string[] splitData = data.Split(new char[] { VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            this.PossibleValues = new List<string>(splitData);
        }
    }

    public class MultiselectField : SelectField {

        private const char SERIALIZATION_SEPARATOR = ';';
        public override FieldType Type => FieldType.Multiselect;

        public override object DeserializeValue(string valueString) {
            return valueString.Split(new char[] { SERIALIZATION_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string SerializeValue(object value) {
            string[] valueArray = (string[])value;

            StringBuilder valueStringBuilder = new StringBuilder();
            for (int i = 0; i < valueArray.Length; i++) {
                valueStringBuilder.Append(valueArray[i]);
                valueStringBuilder.Append(SERIALIZATION_SEPARATOR);
            }

            return valueStringBuilder.ToString();
        }
    }

    public class SimpleSelectField : SelectField {
        public override FieldType Type => FieldType.Select;

        public override object DeserializeValue(string valueString) {
            return valueString;
        }

        public override string SerializeValue(object value) {
            return (string)value;
        }
    }

    public static class FieldFactory {
        public static Field CreateField(string fieldDefinition) {
            string[] contents = fieldDefinition.Split(new string[] { ":::" }, StringSplitOptions.None);

            FieldType type = (FieldType)Enum.Parse(typeof(FieldType), contents[0]);
            Field f = null;

            switch (type) {
                case FieldType.Text:
                    f = new TextField();
                    break;
                case FieldType.Multiselect:
                    f =  new MultiselectField();
                    break;
                case FieldType.Select:
                    f = new SimpleSelectField();
                    break;
            }

            if(f != null) {
                f.Deserialize(fieldDefinition);
            }

            return f;
        }
    }
}
