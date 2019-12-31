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
            _classStates.Add(state, styleClass);
        }

        public string GetValue(string name, StyleState state = StyleState.Default)
        {
            if (state == StyleState.Disabled)
            {
                if (_classStates.ContainsKey(StyleState.Disabled))
                    return _classStates[StyleState.Disabled].GetValue(name);
            }

            if (state == StyleState.Pressed)
            {
                if (_classStates.ContainsKey(StyleState.Pressed))
                    return _classStates[StyleState.Pressed].GetValue(name);
                else
                    state = StyleState.Hovered;
            }

            if (state == StyleState.Hovered)
            {
                if (_classStates.ContainsKey(StyleState.Hovered))
                    return _classStates[StyleState.Hovered].GetValue(name);
                else
                    state = StyleState.Default;
            }
            
            if (_classStates.ContainsKey(StyleState.Default))
                return _classStates[StyleState.Default].GetValue(name);

            StyleValue styleValue = StyleValueResolver.Get(name);

            if (styleValue != null) 
                return styleValue.Value;
            else return null;
        }

        //public StyleClass GetClass(StyleState state)
        //{
        //    //Get class based on state availability

        //    if (state == StyleState.Disabled)
        //    {
        //        if (_classStates.ContainsKey(StyleState.Disabled))
        //            return _classStates[StyleState.Disabled];
        //        else
        //            return new StyleClass();
        //    }

        //    if (state == StyleState.Pressed)
        //    {
        //        if (_classStates.ContainsKey(StyleState.Pressed))
        //            return _classStates[StyleState.Pressed];
        //        else
        //            state = StyleState.Hovered;
        //    }

        //    if (state == StyleState.Hovered)
        //    {
        //        if (_classStates.ContainsKey(StyleState.Hovered))
        //            return _classStates[StyleState.Hovered];
        //        else
        //            state = StyleState.Default;
        //    }


        //    if (_classStates.ContainsKey(StyleState.Default))
        //        return _classStates[StyleState.Default];
        //    else
        //        return new StyleClass();
        //}
    }
}
