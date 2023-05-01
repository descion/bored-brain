using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoredBrain.Models {

    public class Structure {

        //---------------------------------------------------------------------------

        public List<Field> Fields { get; private set; }

        //---------------------------------------------------------------------------

        public Structure() {
            this.Fields = new List<Field>();
        }

        //---------------------------------------------------------------------------

        public void AddField(Field f) {
            this.Fields.Add(f);
        }

        //---------------------------------------------------------------------------

        public Field GetFieldByName(string name) {
            return this.Fields.Find((Field f) => { return f.Name == name; });
        }
    }
}
