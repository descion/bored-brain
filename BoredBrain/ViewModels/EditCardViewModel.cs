using BoredBrain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoredBrain.ViewModels {

    public class FieldDefinitionWrapper {
        public FieldDefinition Definition { get; set; }
    }

    public class FieldDefinition {
        public string Name { get; set; }
        public string Type { get; set; }
        public ObservableCollection<string> PossibleValues { get; set; }
        public object Value { get; set; }

        public Visibility PossibleValuesVisiblity { get {
                return this.Type == "Select" ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }

    public class EditCardViewModel {

        private bool showDelete;

        public string Title { get; set; }

        public string Content { get; set; }

        public List<FieldDefinitionWrapper> Fields { get; set; }

        public Visibility DeleteVisibility { get {
                return this.showDelete ? Visibility.Visible : Visibility.Collapsed; 
            } 
        }

        private Card card;

        public EditCardViewModel(Card card, bool showDelete) {
            this.showDelete = showDelete;
            this.Fields = new List<FieldDefinitionWrapper>();

            this.Title = card.Title;
            this.Content = card.Content;

            for (int itFields = 0; itFields < card.Structure.Fields.Count; itFields++) {
                Field currentField = card.Structure.Fields[itFields];

                FieldDefinition definition = new FieldDefinition() {
                    Name = currentField.Name,
                    Type = currentField.Type.ToString(),
                    PossibleValues = new ObservableCollection<string>(currentField.PossibleValues),
                    Value = card.GetFieldValue(currentField)
                };

                this.Fields.Add(new FieldDefinitionWrapper() { Definition = definition });
            }

            this.card = card;
        }

        public void Save() {
            this.card.Title = this.Title;
            this.card.Content = this.Content;

            for (int itFields = 0; itFields < this.Fields.Count; itFields++) {
                FieldDefinition definition = this.Fields[itFields].Definition;

                this.card.SetFieldValue(this.card.Structure.GetFieldByName(definition.Name), definition.Value);
            }
        }
    }
}
