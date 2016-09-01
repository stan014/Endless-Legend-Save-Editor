using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Backend.Kernel;

namespace EndlessLegendEditor.DataObject
{
    public class Army : XmlProperty
    {
        public Leader Leader { get; set; }

        public List<Unit> Units { get; set; }

        public Army(XElement e)
            : base(e)
        {
            if (e.Attribute("UserDefinedName") != null)
            {
                Name = e.Attribute("UserDefinedName").Value;
            }
            Leader = Leader.GetLeader(e.Element("Unit")); 
            var units =e.XPathSelectElements("./Units/Unit");
            Units = (from ue in units select new Unit(ue)).ToList();
 
        }
    }
}
