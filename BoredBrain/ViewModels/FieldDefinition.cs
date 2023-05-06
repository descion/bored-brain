using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BoredBrain.ViewModels {
    public class FieldDefinition {

        //---------------------------------------------------------------------------

        public string Name { get; set; }
        public string Type { get; set; }
        public ObservableCollection<string> PossibleValues { get; set; }
        public object Value { get; set; }

        public bool ShowOnCard { get; set; }

        public Action<FieldDefinition> Delete { get; set; }

        public Visibility PossibleValuesVisiblity { 
            get {
                return this.Type == "Select" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility DeleteVisibility {
            get {
                return this.Delete != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        //---------------------------------------------------------------------------

        public void DoDelete() {
            this.Delete?.Invoke(this);
        }
    }
}
