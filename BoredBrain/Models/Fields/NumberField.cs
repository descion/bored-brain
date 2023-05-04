namespace BoredBrain.Models {
    public class NumberField : Field {
        public override FieldType Type => FieldType.Number;

        public override object ConvertStringToValue(string valueString) {
            return int.Parse(valueString);
        }

        public override object GetDefaultValue() {
            return 0;
        }

        public override string ConvertValueToString(object value) {
            return value.ToString();
        }

        public override object Validate(object value) {
            return value;
        }
    }
}
