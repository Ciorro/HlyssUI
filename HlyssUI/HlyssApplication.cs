using HlyssUI.Styling;
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
            StyleBank.LoadFromString(Encoding.UTF8.GetString(Properties.Resources.DefaultStyle));
        }

        public void RegisterForm(string name, HlyssForm form)
        {
            if (!_forms.ContainsKey(name))
                _forms.Add(name, form);
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
    }
}
