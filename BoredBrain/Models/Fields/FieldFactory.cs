using System;

namespace BoredBrain.Models {
    public static class FieldFactory {

        //---------------------------------------------------------------------------

        public static Field CreateField(string fieldDefinition) {
            string[] contents = fieldDefinition.Split(new string[] { ":::" }, StringSplitOptions.None);

            FieldType type = (FieldType)Enum.Parse(typeof(FieldType), contents[0]);
            Field f = CreateField(type);

            return f;
        }

        //---------------------------------------------------------------------------

        public static Field CreateField(FieldType type) {
            
            Field f = null;

            switch (type) {
                case FieldType.Text:
                    f = new TextField();
                    break;
                case FieldType.Multiselect:
                    f = new MultiselectField();
                    break;
                case FieldType.Select:
                    f = new SimpleSelectField();
                    break;
                case FieldType.Number:
                    f = new NumberField();
                    break;
                case FieldType.Date:
                    f = new DateField();
                    break;
            }

            return f;
        }
    }
}
