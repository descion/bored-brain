﻿using BoredBrain.Models;
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
    /// <summary>
    /// Interaction logic for CardDropPreview.xaml
    /// </summary>
    public partial class CardDropPreview : UserControl {

        //---------------------------------------------------------------------------

        private CardViewModel previewCard;

        //---------------------------------------------------------------------------

        public CardDropPreview(Card previewCard) {
            InitializeComponent();

            this.previewCard = new CardViewModel(previewCard, null, null);
            this.DataContext = this.previewCard;
        }

    }
}
