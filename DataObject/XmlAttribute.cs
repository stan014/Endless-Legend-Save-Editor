using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EndlessLegendEditor.DataObject
{
    public class XmlAttribute
    {
        private readonly string a;

        public XElement Element { get; set; }

        public XmlProperty DisplayName { get; set; }

        public string Name { get; set; }

        public string Value
        {
            get
            {
                if (Element == null) return string.Empty;
                return Element.Attribute(a).Value;
            }
            set
            {
                if (Element == null) return;
                Element.Attribute(a).SetValue(value);
            }
        }

        public XmlAttribute(XElement e, string name)
        {
            a = name;
            Element = e;
        }
    }
}
