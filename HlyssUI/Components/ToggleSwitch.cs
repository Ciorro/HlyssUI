﻿using HlyssUI.Layout;
using HlyssUI.Themes;

namespace HlyssUI.Components
{
    public class ToggleSwitch : Component
    {
        public bool Toggled
        {
            get { return _toggled; }
            set
            {
                _toggled = value;
                toggle();
            }
        }

        public string Label
        {
            get { return _label.Text; }
            set
            {
                _label.Text = value;
            }
        }

        private bool _toggled;
        private Panel _body;
        private Panel _toggle;
        private Label _label;

        public ToggleSwitch(string label = "")
        {
            Layout = LayoutType.Row;
            _body = new Panel();
            _toggle = new Panel();
            _label = new Label(label);

            Autosize = true;
        }

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            _body.Width = "40px";
            _body.Height = "20px";
            _body.MarginRight = "5px";
            _body.CoverParent = false;
            AddChild(_body);

            _toggle.Width = "12px";
            _toggle.Height = "12px";
            _toggle.MarginLeft = "4px";
            _toggle.MarginTop = "4px";
            _toggle.CoverParent = false;
            _toggle.Style.Round = true;
            _body.AddChild(_toggle);
            _toggle.Name = "toggle";

            _label.Margin = "2px";
            _label.CoverParent = false;
            AddChild(_label);

            Style.Round = true;
            CascadeStyle = true;
            Toggled = false;
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            if (Toggled)
                Style["primary"] = Style.GetDarker(Theme.GetColor("accent"), 20);
            else
                Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 20);

            StyleChanged = true;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            if (Toggled)
                Style["primary"] = Theme.GetColor("accent");
            else
                Style["primary"] = Theme.GetColor("primary");

            StyleChanged = true;
        }

        public override void OnPressed()
        {
            base.OnPressed();
            if (Toggled)
                Style["primary"] = Style.GetDarker(Theme.GetColor("accent"), 40);
            else
                Style["primary"] = Style.GetDarker(Theme.GetColor("primary"), 40);

            StyleChanged = true;
        }

        public override void OnClicked()
        {
            base.OnClicked();
            Toggled = !Toggled;
        }

        private void toggle()
        {
            if(_toggled)
            {
                _toggle.MarginLeft = "24px";
                _toggle.Style["primary"] = Theme.GetColor("ffffff");
                _toggle.Style["secondary"] = Theme.GetColor("ffffff");

                Style["primary"] = Theme.GetColor("accent");
                Style["secondary"] = Theme.GetColor("accent");
            }
            else
            {
                _toggle.MarginLeft = "4px";
                _toggle.Style["primary"] = Theme.GetColor("secondary");
                _toggle.Style["secondary"] = Theme.GetColor("secondary");

                Style["primary"] = Theme.GetColor("primary");
                Style["secondary"] = Theme.GetColor("secondary");
            }

            StyleChanged = true;
        }
    }
}