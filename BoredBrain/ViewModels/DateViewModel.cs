using System;
using System.Globalization;

namespace BoredBrain.ViewModels {
    public class DateViewModel {

        public FieldDefinition Definition { get; set; }

        public DateTime? DateValue {
            get {
                if (string.IsNullOrEmpty((string)this.Definition.Value)) {
                    return null;
                }

                return DateTime.Parse((string)this.Definition.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }
            set {
                Definition.Value = value?.ToString("O");
            }
        }

        public string DateString {
            get {
                if (string.IsNullOrEmpty((string)this.Definition.Value)) {
                    return "";
                }
                
                return this.DateValue?.ToShortDateString();
            }
        }
    }
}
