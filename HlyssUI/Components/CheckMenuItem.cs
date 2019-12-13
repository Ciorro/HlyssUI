using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class CheckMenuItem : MenuItem
    {
        public delegate void CheckHandler(object sender, bool isChecked);
        public event CheckHandler Checked;

        private bool _checked = false;

        public bool IsChecked
        {
            get { return _checked; }
            set
            {
                if (value != _checked)
                {
                    if (value)
                    {
                        Icon = Graphics.Icons.Check;
                    }
                    else
                    {
                        Icon = Graphics.Icons.Empty;
                    }

                    _checked = value;
                    Checked?.Invoke(this, value);
                }
            }
        }

        public CheckMenuItem(string label = "") : base(label) { }

        public override void OnClicked()
        {
            base.OnClicked();
            IsChecked = !IsChecked;
        }
    }
}
