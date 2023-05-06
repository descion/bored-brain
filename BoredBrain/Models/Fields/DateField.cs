namespace BoredBrain.Models {
    public class DateField : Field {
        public override FieldType Type => FieldType.Date;

        public override object ConvertStringToValue(string valueString) {
            return valueString;
        }

        public override string ConvertValueToString(object value) {
            return (string)value;
        }

        public override object GetDefaultValue() {
            return "";
        }

        public override object Validate(object value) {
            return value;
        }
    }
}
