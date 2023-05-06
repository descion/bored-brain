using BoredBrain.Models;
using BoredBrain.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BoredBrain.Views {
    /// <summary>
    /// Interaction logic for StructureView.xaml
    /// </summary>
    public partial class StructureView : UserControl {

        private Board board;

        private StructureViewModel structureViewModel;

        public event OnClose OnClose;

        public StructureView(Board board) {
            InitializeComponent();

            this.board = board;
            this.structureViewModel = new StructureViewModel(board);
            this.DataContext = structureViewModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            this.structureViewModel.Apply();
            this.OnClose?.Invoke(true);
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.OnClose?.Invoke(false);
        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            string fieldName = this.NewFieldName.Text;
            string fieldType = this.NewFieldType.Text;

            this.structureViewModel.AddField(
                new FieldDefinition() {
                    Name = fieldName,
                    Type = fieldType,
                    PossibleValues = new ObservableCollection<string>(),
                }
            );

            this.NewFieldName.Text = "";
            this.NewFieldType.SelectedIndex = 0;
        }
    }
}
