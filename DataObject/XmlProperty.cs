using System.Xml.Linq;

namespace EndlessLegendEditor.DataObject
{
    public class XmlProperty
    {
        public XElement Element { get; set; }

        public XmlProperty DisplayName { get; set; }

        public string Name { get; set; }

        public string Value
        {
            get
            {
                if (Element == null) return string.Empty;
                return Element.Value;
            }
            set
            {
                if (Element == null) return;
                Element.SetValue(value);
            }
        }

        public XmlProperty(XElement e)
        {
            Element = e;
        }
    }
}
