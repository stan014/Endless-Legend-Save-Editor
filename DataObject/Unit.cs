using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Backend.Kernel;

namespace EndlessLegendEditor.DataObject
{
    public class Unit : XmlProperty
    {
        public int Level
        {
            get
            {
                if (Properties.ContainsKey("Level"))
                {
                    return Properties["Level"].Value.ToInt();
                }
                return 0;
            }
        }

        public XmlProperty Exp
        {
            get
            {
                if (Properties.ContainsKey("Experience"))
                {
                    return Properties["Experience"];
                }
                return new XmlProperty(null);
            }
        }

        public string Prototype { get; set; }

        public Dictionary<string, XmlProperty> Properties { get; set; }

        public Unit(XElement e)
            : base(e)
        {
            Properties = new Dictionary<string, XmlProperty>();
            Prototype = e.Attribute("UnitDesignName").Value;
            Name = e.Element("SimulationObject").Attribute("Name").Value;
            var properties = e.XPathSelectElements("./SimulationObject/Properties/Property");
            foreach (var p in properties)
            {
                Properties.Add(p.Attribute("Name").Value, new XmlProperty(p));
            }
        }
    }

    public class Leader : Unit
    {
        public XmlProperty SkillPoint
        {
            get
            {
                if (Properties.ContainsKey("MaximumSkillPoints"))
                {
                    return Properties["MaximumSkillPoints"];
                }
                return new XmlProperty(null);
            }
        }
        private Leader(XElement e)
            : base(e)
        {
            Name += string.Format(" [{0}]", e.Attribute("UnitDesignName").Value);
        }

        public static Leader GetLeader(XElement e)
        {
            if (e == null) return null;
            return new Leader(e);
        }
    }
}
