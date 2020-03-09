using HlyssUI.Styling;
using HlyssUI.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HlyssUI
{
    public class HlyssApplication
    {
        private Dictionary<string, HlyssForm> _forms = new Dictionary<string, HlyssForm>();

        public static void InitializeStyles()
        {
            StyleBank.LoadFromString(Properties.Resources.DefaultStyle);
        }

        public static void LoadDefaultTheme()
        {
            ThemeManager.LoadFromString(Properties.Resources.DefaultTheme);
            ThemeManager.SetTheme("light");
        }

        public void RegisterForm(string name, HlyssForm form)
        {
            if (!_forms.ContainsKey(name))
            {
                form.Application = this;
                _forms.Add(name, form);
            }
        }

        public void RegisterAndShow(HlyssForm form)
        {
            string id = Guid.NewGuid().ToString();

            form.Application = this;
            _forms.Add(id, form);

            form.Closed += (_) => 
            {
                _forms.Remove(id);
            };

            GetForm(id).Show();
        }

        public void UnregisterForm(string name)
        {
            if (_forms.ContainsKey(name))
                _forms.Remove(name);
        }

        public void UpdateAllForms()
        {
            UpdateForms(_forms.Keys.ToArray());
        }

        public void UpdateForms(params string[] names)
        {
            foreach (var name in names)
            {
                if (_forms.ContainsKey(name) && _forms[name].IsOpen)
                    _forms[name].Update();
            }
        }

        public void DrawAllForms()
        {
            DrawForms(_forms.Keys.ToArray());
        }

        public void DrawForms(params string[] names)
        {
            foreach (var name in names)
            {
                if (_forms.ContainsKey(name) && _forms[name].IsOpen)
                    _forms[name].Draw();
            }
        }

        public HlyssForm GetForm(string name)
        {
            return _forms[name];
        }

        public bool IsAnyFormRunning()
        {
            bool isAnyFormRunning = false;

            foreach (var form in _forms.Values)
            {
                if (form.IsOpen)
                    isAnyFormRunning = true;
            }

            return isAnyFormRunning;
        }
    }
}
