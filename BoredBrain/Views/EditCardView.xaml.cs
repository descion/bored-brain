using BoredBrain.Models;
using BoredBrain.ViewModels;
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

namespace BoredBrain.Views {


    public delegate void OnClose(bool success);

    public interface IClose {
        event OnClose OnClose;
    }

    /// <summary>
    /// Interaction logic for EditCardView.xaml
    /// </summary>
    public partial class EditCardView : UserControl, IClose {

        private EditCardViewModel editCardViewModel;

        public event OnClose OnClose;

        public EditCardView(Card card) {
            InitializeComponent();

            this.editCardViewModel = new EditCardViewModel(card);
            this.DataContext = this.editCardViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.OnClose?.Invoke(false);
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            this.editCardViewModel.Save();
            this.OnClose?.Invoke(true);
        }
    }
}
