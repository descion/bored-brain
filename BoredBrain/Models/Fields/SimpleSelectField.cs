namespace BoredBrain.Models {
    public class SimpleSelectField : Field {
        public override FieldType Type => FieldType.Select;

        //---------------------------------------------------------------------------

        public override string ConvertValueToString(object value) {
            if (this.PossibleValues.Count > 0 && !this.PossibleValues.Contains((string)value)) {
                return this.PossibleValues[0];
            }

            return (string)value;
        }

        //---------------------------------------------------------------------------

        public override object ConvertStringToValue(string valueString) {
            return valueString;
        }

        //---------------------------------------------------------------------------

        public override object GetDefaultValue() {
            if(this.PossibleValues.Count > 0) {
                return this.PossibleValues[0];
            }

            return string.Empty;
        }

        //---------------------------------------------------------------------------

        public override object Validate(object value) {
            if (this.PossibleValues.Count > 0 && !this.PossibleValues.Contains((string)value)) {
                return this.PossibleValues[0];
            }

            return value;
        }

    }
}
