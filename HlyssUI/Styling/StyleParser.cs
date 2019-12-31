using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace HlyssUI.Styling
{
    internal static class StyleParser
    {
        internal static XmlNodeList GetClasses(string xmlStyleSheet)
        {
            XmlDocument styleSheet = new XmlDocument();
            styleSheet.LoadXml(xmlStyleSheet);

            return styleSheet.SelectSingleNode("/hss").ChildNodes;
        }

        internal static XmlNode StringToNode(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            return xml.DocumentElement;
        }
    }
}
