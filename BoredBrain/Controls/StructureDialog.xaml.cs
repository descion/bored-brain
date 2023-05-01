using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoredBrain {
    /// <summary>
    /// Interaction logic for StructureDialog.xaml
    /// </summary>
    public partial class StructureDialog : UserControl {

        private Board board;

        private List<StructureField> fields;

        private Action onSaveAction;

        private string newColumnField;

        public StructureDialog() {
            InitializeComponent();

        }

        public void Close() {
            this.Visibility = Visibility.Hidden;
        }

        public void Open(Board board, Action OnSaveAction) {

            this.Fields.Children.Clear();

            this.onSaveAction = OnSaveAction;
            this.board = board;
            this.fields = new List<StructureField>();

            this.CreateFields();

            this.Visibility = Visibility.Visible;
        }

        private void CreateFields() {

            for (int i = 0; i < this.board.Structure.Fields.Count; i++) {
                Field currentField = this.board.Structure.Fields[i];

                FieldDefinition fieldDefinition = new FieldDefinition() {
                    name = currentField.Name,
                    type = currentField.Type.ToString(),
                    possibleValues = new List<string>()
                };

                if(currentField.Type == FieldType.Select) {
                    SelectField selectField = currentField as SelectField;

                    fieldDefinition.possibleValues = new List<string>(selectField.PossibleValues);
                }

                this.CreateField(fieldDefinition);
            }
            
        }

        private void CreateField(FieldDefinition definition) {
            StructureField newField = new StructureField(definition);

            this.fields.Add(newField);
            this.Fields.Children.Add(newField);

            newField.OnDelete += this.OnDeleteField;
            newField.OnSetColumn += this.OnSetAsColumnField;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < this.fields.Count; i++) {
                StructureField field = this.fields[i];

                Field existingField = this.board.Structure.GetFieldByName(field.Definition.name);

                if (existingField == null) {
                    FieldType type = (FieldType)Enum.Parse(typeof(FieldType), field.Definition.type);

                    Field newField = FieldFactory.CreateField(type);
                    newField.Name = field.Definition.name;
                    
                    if(newField is SelectField) {
                        (newField as SelectField).PossibleValues = field.Definition.possibleValues;
                    }

                    this.board.Structure.AddField(newField);
                }else if(existingField is SelectField) {
                    (existingField as SelectField).PossibleValues = field.Definition.possibleValues;
                }
            }

            if(this.newColumnField != null && this.board.ColumnField.Name != this.newColumnField) {
                this.board.ColumnField = this.board.Structure.GetFieldByName(this.newColumnField);
            }

            this.onSaveAction?.Invoke();
            this.Close();
        }

        private void NewField_Click(object sender, RoutedEventArgs e) {
            string fieldName = this.NewFieldName.Text;
            string fieldType = ((ComboBoxItem)this.NewFieldType.SelectedItem).Content as string;

            FieldDefinition newField = new FieldDefinition() {
                name = fieldName,
                type = fieldType,
                possibleValues = new List<string>()
            };

            this.CreateField(newField);

            this.NewFieldName.Text = "";
            this.NewFieldType.SelectedIndex = 0;
        }

        private void OnDeleteField(StructureField field) {
            this.fields.Remove(field);
            this.Fields.Children.Remove(field);
            field.OnDelete -= this.OnDeleteField;
            field.OnSetColumn -= this.OnSetAsColumnField;
        }

        private void OnSetAsColumnField(StructureField field) {
            this.newColumnField = field.Definition.name;
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
