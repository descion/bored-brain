using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.ViewModels {
    public class MultiselectViewModel {

        public ObservableCollection<string> AvailableValues { get; set; }

        public FieldDefinition Field { get; set; }

        public ObservableCollection<string> Values { get; set; }

        public MultiselectViewModel(FieldDefinition field) {
            this.Field = field;

            this.Values = new ObservableCollection<string>((string[])field.Value);

            if (field.PossibleValues != null) {
                this.AvailableValues = new ObservableCollection<string>(field.PossibleValues);

                for (int itValue = 0; itValue < this.Values.Count; itValue++) {
                    this.AvailableValues.Remove(this.Values[itValue]);
                }
            }
        }

        public void AddValue(string value) {
            this.Values.Add(value);
            this.AvailableValues.Remove(value);

            this.Field.Value = this.Values.ToArray();
        }

        public void RemoveValue(string value) {
            this.Values.Remove(value);
            this.AvailableValues.Add(value);

            this.Field.Value = this.Values.ToArray();
        }
    }
}
