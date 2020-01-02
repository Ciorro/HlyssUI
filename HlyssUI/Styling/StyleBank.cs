using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace HlyssUI.Styling
{
    public static class StyleBank
    {
        private static Dictionary<string, StyleClassContainer> _classes = new Dictionary<string, StyleClassContainer>();

        public static void LoadFromString(string hss)
        {
            XmlNodeList classes = StyleParser.GetClasses(hss);

            foreach (XmlNode styleClassNode in classes)
            {
                if (styleClassNode is XmlElement)
                {

                    StyleClass styleClass = new StyleClass(styleClassNode);

                    if (!_classes.ContainsKey(styleClass.Name))
                    {
                        _classes.Add(styleClass.Name, new StyleClassContainer());
                    }

                    _classes[styleClass.Name].AddClassState(styleClass, styleClass.State);
                }
            }
        }

        public static void LoadFromFile(string filename)
        {
            LoadFromString(File.ReadAllText(filename));
        }

        public static void AddClass(string classStr)
        {

        }

        public static StyleClassContainer GetClass(string name)
        {
            //stanelo na tym jak szukac odpowiedniego stylu (pressed hover def disabled)

            if (_classes.ContainsKey(name))
                return _classes[name];
            else
                return new StyleClassContainer();
        }
    }
}
