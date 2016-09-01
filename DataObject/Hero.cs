using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EndlessLegendEditor.DataObject
{
    public class Hero : XmlProperty
    {
        public int Level { get; set; }

        public List<Ability> Abilities { get; set; }

        public Hero(XElement e)
            : base(e)
        {
            Name = e.Attribute("Name").Value.Replace("UnitProfile",string.Empty);
            DisplayName = new XmlProperty(e.Element("UserDefinedName"));
            Abilities = e.Elements("ProfileAbilityReferences").Select(x => new Ability(x)).ToList();
        }

        public void AddAbility(string name, int lv)
        {
            var xe = new XElement("ProfileAbilityReferences");
            xe.Add(new XAttribute("Name", name));
            xe.Add(new XAttribute("Level", lv));
            Element.Add(xe);
        }

        public void ClearAbilities()
        {
            foreach (var a in Abilities)
            {
                a.Element.Remove();
            }
            Abilities.Clear(); ;
        }
    }
}
