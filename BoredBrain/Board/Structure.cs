using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain {
    public class Structure {

        public List<Field> Fields { get; private set; }

        public Structure() {
            this.Fields = new List<Field>();
        }

        public void AddField(Field f) {
            this.Fields.Add(f);
        }

        public Field GetFieldByName(string name) {
            return this.Fields.Find((Field f) => { return f.Name == name; });
        }

        public string Serialize() {
            StringBuilder fieldBuilder = new StringBuilder();

            for (int i = 0; i < this.Fields.Count; i++) {
                fieldBuilder.AppendLine(this.Fields[i].Serialize());
            }

            return fieldBuilder.ToString();
        }

        public void Deserialize(string structureDefinition) {
            using (StringReader definitionReader = new StringReader(structureDefinition)) {
                string currentLine = definitionReader.ReadLine();
                while(currentLine != null) { 
                    Field f = FieldFactory.CreateField(currentLine);
                    this.Fields.Add(f);

                    currentLine = definitionReader.ReadLine();
                }
            }
        }
    }
}
