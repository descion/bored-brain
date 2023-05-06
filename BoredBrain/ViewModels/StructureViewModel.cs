using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain.ViewModels {
    public class StructureViewModel {

        //---------------------------------------------------------------------------

        public ObservableCollection<FieldDefinition> Fields { get; set; }

        private Structure structure;

        //---------------------------------------------------------------------------

        public StructureViewModel(Board board) {
            this.structure = board.Structure;

            this.Fields = new ObservableCollection<FieldDefinition>();

            for (int itFields = 0; itFields < structure.Fields.Count; itFields++) {
                Field currentField = structure.Fields[itFields];

                FieldDefinition definition = new FieldDefinition() {
                    Name = currentField.Name,
                    Type = currentField.Type.ToString(),
                    PossibleValues = new ObservableCollection<string>(currentField.PossibleValues),
                    ShowOnCard = currentField.ShowOnCard,
                };

                if(currentField != board.ColumnField && currentField != board.CategoryField) {
                    definition.Delete = this.DeleteField;
                }

                this.Fields.Add(definition);
            }
        }

        //---------------------------------------------------------------------------

        public void AddField(FieldDefinition definition) {
            definition.Delete = this.DeleteField;
            this.Fields.Add(definition);
        }

        //---------------------------------------------------------------------------

        private void DeleteField(FieldDefinition definition) {
            this.Fields.Remove(definition);
        }

        //---------------------------------------------------------------------------

        public void Apply() {

            for (int itFields = 0; itFields < this.Fields.Count; itFields++) {
                Field existingField = this.structure.Fields.Find((Field f) => { return f.Name == this.Fields[itFields].Name; });

                if(existingField == null) {
                    Field f = FieldFactory.CreateField((FieldType)Enum.Parse(typeof(FieldType), this.Fields[itFields].Type));
                    f.Name = this.Fields[itFields].Name;
                    f.PossibleValues = new List<string>(this.Fields[itFields].PossibleValues);
                    f.ShowOnCard = this.Fields[itFields].ShowOnCard;

                    this.structure.AddField(f);
                }
                else {
                    existingField.PossibleValues = new List<string>(this.Fields[itFields].PossibleValues);
                    existingField.ShowOnCard = this.Fields[itFields].ShowOnCard;
                }
            }

            for (int i = 0; i < this.structure.Fields.Count; i++) {
                bool hasField = this.Fields.Any((FieldDefinition f) => { return f.Name == this.structure.Fields[i].Name; });

                if(!hasField) {
                    this.structure.RemoveField(this.structure.Fields[i]);
                }
            }
        }
    }
}
