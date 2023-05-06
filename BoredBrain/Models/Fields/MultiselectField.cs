using System;
using System.Collections.Generic;
using System.Text;

namespace BoredBrain.Models {
    public class MultiselectField : Field {

        //---------------------------------------------------------------------------

        private const char SERIALIZATION_SEPARATOR = ';';
        public override FieldType Type => FieldType.Multiselect;

        //---------------------------------------------------------------------------

        public override object ConvertStringToValue(string valueString) {
            return valueString.Split(new char[] { SERIALIZATION_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
        }

        //---------------------------------------------------------------------------

        public override object GetDefaultValue() {
            return new string[0];
        }

        //---------------------------------------------------------------------------

        public override string ConvertValueToString(object value) {
            string[] valueArray = (string[])value;

            StringBuilder valueStringBuilder = new StringBuilder();
            for (int i = 0; i < valueArray.Length; i++) {
                valueStringBuilder.Append(valueArray[i]);
                valueStringBuilder.Append(SERIALIZATION_SEPARATOR);
            }

            return valueStringBuilder.ToString();
        }

        //---------------------------------------------------------------------------

        public override object Validate(object value) {
            List<string> valueList = new List<string>((string[])value);

            for (int i = valueList.Count-1; i >= 0; i--) {
                if (!this.PossibleValues.Contains(valueList[i])) {
                    valueList.RemoveAt(i);
                }
            }

            return valueList.ToArray();
        }
    }
}
