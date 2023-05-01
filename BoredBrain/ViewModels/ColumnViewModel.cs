using BoredBrain.Models;
using System.Collections.Generic;

namespace BoredBrain.ViewModels {

    public class ColumnViewModel {
        public string Headline { get; set; }

        public List<CardWrapper> Cards { get; set; }

        public Board Board { get; set; }
    }
}
