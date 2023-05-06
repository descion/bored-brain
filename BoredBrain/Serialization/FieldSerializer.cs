using BoredBrain.Models;
using System;
using System.Collections.Generic;

namespace BoredBrain {
    public static class FieldSerializer {

        public class FieldJSON {

            public string Type { get; set; }

            public string Name { get; set; }

            public List<string> PossibleValues { get; set; }

            public bool ShowOnCard { get; set; }
        }

        public static FieldJSON ToJSONType(Field field) {

            return new FieldJSON() {
                Type = field.Type.ToString(),
                Name = field.Name,
                PossibleValues = field.PossibleValues,
                ShowOnCard = field.ShowOnCard
            };
        }

        public static Field FromJSONType(FieldJSON fieldJson) {
            Field field = FieldFactory.CreateField((FieldType)Enum.Parse(typeof(FieldType), fieldJson.Type));

            field.Name = fieldJson.Name;
            field.PossibleValues = fieldJson.PossibleValues;
            field.ShowOnCard = fieldJson.ShowOnCard;

            return field;
        }
    }
}
