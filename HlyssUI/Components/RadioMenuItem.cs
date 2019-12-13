using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class RadioMenuItem : MenuItem
    {
        public delegate void MarkHandler(object sender, bool isToggled);
        public event MarkHandler Marked;

        private bool _marked = false;

        public bool IsMarked
        {
            get { return _marked; }
            set
            {
                if (value != _marked)
                {
                    if (value)
                    {
                        Icon = Graphics.Icons.AngleRight;
                        UnmarkOthers();
                    }
                    else
                    {
                        Icon = Graphics.Icons.Empty;
                    }

                    _marked = value;
                    Marked?.Invoke(this, value);
                }
            }
        }

        public RadioMenuItem(string label = "") : base(label) { }

        public override void OnClicked()
        {
            base.OnClicked();
            IsMarked = !IsMarked;
        }

        private void UnmarkOthers()
        {
            if (Parent == null)
                return;

            foreach (var parentChild in Parent.Children)
            {
                if (parentChild.GetType() == GetType() && parentChild != this)
                {
                    ((RadioMenuItem)parentChild).IsMarked = false;
                }
            }
        }
    }
}
