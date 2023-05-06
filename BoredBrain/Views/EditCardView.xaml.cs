using BoredBrain.Models;
using BoredBrain.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BoredBrain.Views {


    public delegate void OnClose(bool success);

    /// <summary>
    /// Interaction logic for EditCardView.xaml
    /// </summary>
    public partial class EditCardView : UserControl {

        private Card card;
        private EditCardViewModel editCardViewModel;

        private Action<Card> OnClose;
        private Action<Card> OnSave;
        private Action<Card> OnDelete;

        public EditCardView(Card card, Action<Card> OnClose, Action<Card> OnSave, Action<Card> OnDelete) {
            InitializeComponent();

            this.card = card;

            this.OnClose = OnClose;
            this.OnSave = OnSave;
            this.OnDelete = OnDelete;

            this.editCardViewModel = new EditCardViewModel(card, OnDelete != null);
            this.DataContext = this.editCardViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            this.OnClose?.Invoke(this.card);
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            this.editCardViewModel.Save();
            this.OnSave?.Invoke(this.card);
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult confirmResult = MessageBox.Show("This will move the card to the archive folder. Are you sure?", "Confirm delete", MessageBoxButton.YesNo);

            if (confirmResult == MessageBoxResult.Yes) {
                this.OnDelete?.Invoke(this.card);
            }
        }
    }
}
