using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BoredBrain {

    public static class LegacyFieldSerializer {

        private const string SERIALIZATION_SEPARATOR = ":::";
        private const char VALUE_SEPARATOR = ';';

        public static string Serialize(Field field) {
            return field.Type.ToString() + SERIALIZATION_SEPARATOR + field.Name + SERIALIZATION_SEPARATOR + SerializeValues(field.PossibleValues) + SERIALIZATION_SEPARATOR + field.ShowOnCard;
        }

        private static string SerializeValues(List<string> values) {
            StringBuilder dataBuilder = new StringBuilder();

            for (int i = 0; i < values.Count; i++) {
                dataBuilder.Append(values[i]);
                dataBuilder.Append(VALUE_SEPARATOR);
            }

            return dataBuilder.ToString();
        }

        public static Field Deserialize(string fieldDefinition) {
            string[] contents = fieldDefinition.Split(new string[] { SERIALIZATION_SEPARATOR }, StringSplitOptions.None);

            Field field = FieldFactory.CreateField(fieldDefinition);
            field.Name = contents[1];
            field.PossibleValues = DeserializeValues(contents[2]);

            if(contents.Length > 3) {
                field.ShowOnCard = bool.Parse(contents[3]);
            }

            return field;
        }

        private static List<string> DeserializeValues(string valueString) { 
            string[] splitData = valueString.Split(new char[] { VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

            return new List<string>(splitData);
        }
    }
}
