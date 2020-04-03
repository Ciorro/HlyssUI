using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HlyssUI.Utils
{
    public class Observer<T>
    {
        T variable;

        public Observer(T initVariable)
        {
            variable = initVariable;
        }

        public bool Changed(T variable)
        {
            bool changed = !this.variable.Equals(variable);
            this.variable = variable;

            return changed;
        }
    }
}
