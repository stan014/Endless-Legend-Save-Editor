using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EndlessLegendEditor.DataObject
{
    public class Empire : XmlProperty
    {
        public XmlProperty Account { get; set; }

        public XmlProperty Power { get; set; }

        public List<XmlProperty> StrategicResources { get; set; }

        public List<XmlProperty> LuxuryResources { get; set; }

        public List<Hero> Heroes { get; set; }

        public List<EmpirePlan> Plans { get; set; }

        public List<Army> Armies { get; set; }

        public XmlProperty NextPlanTurn { get; set; }

        public XmlProperty Perals { get; set; }

        public Empire(XElement e)
            : base(e)
        {
            Name = e.Element("SimulationObject").Attribute("Name").Value;
            var properties = e.Element("SimulationObject").Element("Properties").Elements("Property");
            Account = new XmlProperty(properties.First(x => x.Attribute("Name").Value == "BankAccount"));
            Power = new XmlProperty(properties.First(x => x.Attribute("Name").Value == "EmpirePointStock"));
            StrategicResources = properties.Where(x => x.Attribute("Name").Value.StartsWith("Strategic"))
                    .Select(x => new XmlProperty(x)).ToList();
            Perals = new XmlProperty(properties.First(x => x.Attribute("Name").Value == "OrbStock"));
            LuxuryResources = properties.Where(x => x.Attribute("Name").Value.StartsWith("Luxury"))
                    .Select(x => new XmlProperty(x)).ToList();
            Heroes =
                e.XPathSelectElement("./Agencies/DepartmentOfDefense/UnitDesigns/Hidden")
                    .Elements("UnitDesign")
                    .Where(
                        x =>
                            x.Elements("SimulationDescriptorReference")
                                .Any(y => y.Attribute("Name").Value == "UnitHero"))
                    .Select(x => new Hero(x))
                    .ToList();
            var empirePlans = e.XPathSelectElement("./Agencies/DepartmentOfPlanificationAndDevelopment/EmpirePlans");
            NextPlanTurn = new XmlProperty(empirePlans.Element("EmpirePlanChoiceRemainingTurn"));
            Plans = empirePlans.Element("CurrentEmpirePlan").Elements("KeyValuePair")
                    .Select(x => new EmpirePlan(x))
                    .ToList();
            Armies = e.XPathSelectElement("./Agencies/DepartmentOfDefense/Armies")
                .Elements("Army").Select(x => new Army(x)).ToList();
        }
    }
}
