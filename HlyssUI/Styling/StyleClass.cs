using HlyssUI.Styling.ValuePresets;
using System;
using System.Collections.Generic;
using System.Xml;

namespace HlyssUI.Styling
{
    public class StyleClass
    {
        private List<StyleValue> _values = new List<StyleValue>();
        public StyleState State { get; private set; }
        public string Name { get; private set; }

        public StyleClass(string classStr)
        {
            Load(StyleParser.StringToNode(classStr));
        }

        public StyleClass(XmlNode classXml)
        {
            Load(classXml);
        }

        public StyleClass() { }

        public string GetValue(string name)
        {
            foreach (var value in _values)
            {
                if (value.Name == name)
                    return value.Value;
            }

            return string.Empty;
        }

        public bool Contains(string name)
        {
            foreach (var value in _values)
            {
                if (value.Name == name)
                    return true;
            }

            return false;
        }

        public void SetValue(string name, string value)
        {
            if (Contains(name))
            {
                foreach (var val in _values)
                {
                    if (val.Name == name)
                        val.Value = value;
                }
            }
            else
            {
                StyleValue val = StyleValueResolver.Get(name, value);
                if (val != null)
                    _values.Add(val);
            }
        }

        private void Load(XmlNode node)
        {
            if (node.Attributes["state"] != null)
            {
                State = (StyleState)Enum.Parse(typeof(StyleState), node.Attributes["state"].InnerText, true);
            }

            if (node.Attributes["name"] != null)
            {
                Name = node.Attributes["name"].InnerText;
            }

            foreach (XmlNode prop in node.ChildNodes)
            {
                StyleValue value = StyleValueResolver.Get(prop.Name, prop.InnerText);

                if (value != null)
                    _values.Add(value);
            }
        }
    }
}
