using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components.Interfaces
{
    public interface ISelectable
    {
        public delegate void SelectedHandler(object sender);
        public event SelectedHandler OnSelect;

        public delegate void UnselectedHandler(object sender);
        public event UnselectedHandler OnUnselect;

        public bool IsSelected { get; set; }
    }
}
