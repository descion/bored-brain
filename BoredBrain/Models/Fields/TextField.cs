namespace BoredBrain.Models {
    public class TextField : Field {
        public override FieldType Type => FieldType.Text;

        public override object ConvertStringToValue(string valueString) {
            return valueString;
        }

        public override object GetDefaultValue() {
            return string.Empty;
        }

        public override string ConvertValueToString(object value) {
            return (string)value;
        }
    }
}
