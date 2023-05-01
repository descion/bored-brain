using BoredBrain.Models;
using System.IO;
using System.Text;

namespace BoredBrain.Serialization {
    public static class StructureSerializer {
        public static string Serialize(Structure structure) {
            StringBuilder fieldBuilder = new StringBuilder();

            for (int i = 0; i < structure.Fields.Count; i++) {
                fieldBuilder.AppendLine(FieldSerializer.Serialize(structure.Fields[i]));
            }

            return fieldBuilder.ToString();
        }

        public static Structure Deserialize(string structureDefinition) {
            Structure structure = new Structure();

            using (StringReader definitionReader = new StringReader(structureDefinition)) {
                string currentLine = definitionReader.ReadLine();
                while (currentLine != null) {
                    Field f = FieldSerializer.Deserialize(currentLine);
                    structure.Fields.Add(f);

                    currentLine = definitionReader.ReadLine();
                }
            }

            return structure;
        }
    }
}
