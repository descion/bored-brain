﻿using System;
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
    /// Interaction logic for CardElement.xaml
    /// </summary>
    public partial class CardElement : UserControl {

        public Column Column { get; set; }
        public  Card Card { get; private set; }
        
        public CardElement(Card card) {
            InitializeComponent();

            this.Card = card;

            this.Title.Content = this.Card.Title;
        }

        private void Card_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this.Card), DragDropEffects.Move);
            }
        }
    }
}
