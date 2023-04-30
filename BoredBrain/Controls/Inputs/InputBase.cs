using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoredBrain {
    public abstract class InputBase : UserControl {

        protected InputDefinition definition;

        public InputBase(InputDefinition definition) {
            
            this.definition = definition;
        }

        public abstract void ApplyValue();
    }
}
