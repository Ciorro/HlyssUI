using HlyssUI.Styling.ValuePresets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Styling
{
    public class StyleClassContainer
    {
        private Dictionary<StyleState, StyleClass> _classStates = new Dictionary<StyleState, StyleClass>();

        public void AddClassState(StyleClass styleClass, StyleState state)
        {
            if (!_classStates.ContainsKey(state))
            {
                _classStates.Add(state, styleClass);
            }
            else
            {
                _classStates[state].Combine(styleClass);
            }
        }

        public string GetValue(string name, StyleState state = StyleState.Default)
        {
            if (state == StyleState.Disabled)
            {
                if (_classStates.ContainsKey(StyleState.Disabled))
                {
                    var value = _classStates[StyleState.Disabled].GetValue(name);
                    if (value != null)
                        return value;
                }

                state = StyleState.Default;
            }

            if (state == StyleState.Pressed)
            {
                if (_classStates.ContainsKey(StyleState.Pressed))
                {
                    var value = _classStates[StyleState.Pressed].GetValue(name);
                    if (value != null)
                        return value;
                }
                
                state = StyleState.Hovered;
            }

            if (state == StyleState.Hovered)
            {
                if (_classStates.ContainsKey(StyleState.Hovered))
                {
                    var value = _classStates[StyleState.Hovered].GetValue(name);
                    if (value != null)
                        return value;
                }
                
                state = StyleState.Default;
            }
            
            if (_classStates.ContainsKey(StyleState.Default))
                return _classStates[StyleState.Default].GetValue(name);
            
            return null;
        }
    }
}
