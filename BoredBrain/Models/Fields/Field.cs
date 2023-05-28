using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public abstract class Field {

        public delegate void OnFieldChangedDelegate(Field field);

        public event OnFieldChangedDelegate OnFieldChanged;

        public string Name { get; set; }

        public abstract FieldType Type { get; }

        private List<string> possibleValues;
        public ReadOnlyCollection<string> PossibleValues { 
            get { 
                return this.possibleValues.AsReadOnly(); 
            }
            set {
                this.possibleValues = new List<string>(value);
            }
        }

        public bool ShowOnCard { get; set; }

        public Field() {
            this.possibleValues = new List<string>();
        }

        public void SetPossibleValues(List<string> possibleValues) {
            this.possibleValues = possibleValues;
            this.OnFieldChanged?.Invoke(this);
        }

        public void AddPossibleValue(string value) {
            this.possibleValues.Add(value);
            this.OnFieldChanged?.Invoke(this);
        }

        public abstract string ConvertValueToString(object value);

        public abstract object ConvertStringToValue(string valueString);

        public abstract object GetDefaultValue();

        public abstract object Validate(object value);

    }
}
