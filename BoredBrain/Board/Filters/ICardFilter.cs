using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoredBrain {
    interface ICardFilter {

        bool Keep(Card card);

    }
}
