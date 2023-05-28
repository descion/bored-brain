using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public class Structure {

        //---------------------------------------------------------------------------

        public bool IsModified { get; set; }

        //---------------------------------------------------------------------------

        private List<Field> fields;
        public ReadOnlyCollection<Field> Fields { 
            get {
                return this.fields.AsReadOnly();
            }
        }

        //---------------------------------------------------------------------------

        public Structure() {
            this.fields = new List<Field>();
        }

        //---------------------------------------------------------------------------

        public void AddField(Field f) {
            this.fields.Add(f);
            f.OnFieldChanged += this.OnFieldChanged;
            this.SetModified();
        }

        //---------------------------------------------------------------------------

        public void RemoveField(Field f) {
            this.fields.Remove(f);
            f.OnFieldChanged -= this.OnFieldChanged;
            this.SetModified();
        }

        //---------------------------------------------------------------------------

        public Field GetFieldByName(string name) {
            return this.fields.Find((Field f) => { return f.Name == name; });
        }

        //---------------------------------------------------------------------------

        private void OnFieldChanged(Field field) {
            this.SetModified();
        }

        //---------------------------------------------------------------------------

        private void SetModified() {
            this.IsModified = true;
        }
    }
}
