using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.ViewModels {
    public class StructureViewModel {
        public ObservableCollection<FieldDefinitionWrapper> Fields { get; set; }

        private Structure structure;

        public StructureViewModel(Structure structure) {
            this.structure = structure;

            this.Fields = new ObservableCollection<FieldDefinitionWrapper>();

            for (int itFields = 0; itFields < structure.Fields.Count; itFields++) {
                Field currentField = structure.Fields[itFields];

                FieldDefinition definition = new FieldDefinition() {
                    Name = currentField.Name,
                    Type = currentField.Type.ToString(),
                    PossibleValues = new ObservableCollection<string>(currentField.PossibleValues)
                };

                this.Fields.Add(new FieldDefinitionWrapper() { Definition = definition });
            }
        }

        public void Apply() {
            for (int itFields = 0; itFields < this.Fields.Count; itFields++) {
                Field existingField = this.structure.Fields.Find((Field f) => { return f.Name == this.Fields[itFields].Definition.Name; });

                if(existingField == null) {
                    Field f = FieldFactory.CreateField((FieldType)Enum.Parse(typeof(FieldType), this.Fields[itFields].Definition.Type));
                    f.Name = this.Fields[itFields].Definition.Name;
                    f.PossibleValues = new List<string>(this.Fields[itFields].Definition.PossibleValues);

                    this.structure.AddField(f);
                }
                else {
                    existingField.PossibleValues = new List<string>(this.Fields[itFields].Definition.PossibleValues);
                }
            }
        }
    }
}
