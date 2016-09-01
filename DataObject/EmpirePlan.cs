using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Backend.Kernel;

namespace EndlessLegendEditor.DataObject
{
    public class EmpirePlan : XmlProperty
    {
        private readonly XmlAttribute v;

        public int Level
        {
            get
            {
                return v.Value[v.Value.Length - 1].ToInt();
            }
            set
            {
                v.Value = "EmpirePlanDefinition" + Name + value;
            }
        }

        public EmpirePlan(XElement e)
            : base(e)
        {
            v = new XmlAttribute(e, "Value");
            Name = e.Attribute("Key").Value.Substring(10);
        }
    }
}
