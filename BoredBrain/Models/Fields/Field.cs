using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public abstract class Field {

        public string Name { get; set; }

        public abstract FieldType Type { get; }

        public List<string> PossibleValues { get; set; }

        public Field() {
            this.PossibleValues = new List<string>();
        }

        public abstract string ConvertValueToString(object value);

        public abstract object ConvertStringToValue(string valueString);

        public abstract object GetDefaultValue();

        public abstract object Validate(object value);

    }
}
