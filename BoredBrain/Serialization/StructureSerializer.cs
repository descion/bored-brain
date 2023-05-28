using BoredBrain.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace BoredBrain.Serialization {
    public static class StructureSerializer {

        //---------------------------------------------------------------------------

        private class StructureJSON {
            public List<FieldSerializer.FieldJSON> Fields { get; set; }
        }

        //---------------------------------------------------------------------------

        public static string Serialize(Structure structure) {

            StructureJSON structureJson = new StructureJSON() {
                Fields = new List<FieldSerializer.FieldJSON>()
            };

            for (int i = 0; i < structure.Fields.Count; i++) {
                structureJson.Fields.Add(FieldSerializer.ToJSONType(structure.Fields[i]));
            }

            structure.IsModified = false;

            return JsonSerializer.Serialize(structureJson);
        }

        //---------------------------------------------------------------------------

        public static Structure Deserialize(string structureJsonString) {
            StructureJSON structureJson = JsonSerializer.Deserialize<StructureJSON>(structureJsonString);

            Structure structure = new Structure();

            for (int i = 0; i < structureJson.Fields.Count; i++) {
                structure.AddField(FieldSerializer.FromJSONType(structureJson.Fields[i]));
            }

            structure.IsModified = false;

            return structure;
        }
    }
}
