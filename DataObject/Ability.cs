using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EndlessLegendEditor.DataObject
{
    public class Ability : XmlProperty
    {
        public string Level
        {
            get { return Element.Attribute("Level").Value; }
            set { Element.Attribute("Level").SetValue(value); }
        }

        public bool Enable { get; set; }

        public Ability(XElement e)
            : base(e)
        {
            Value = Element.Attribute("Name").Value;
            var abilities = GetAbilities();
            if (abilities.ContainsKey(Value))
            {
                Name = abilities[Value];
            }
            else
            {
                Name = Value;
            }
            Enable = true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as Ability;
            if (t == null) return false;
            return t.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static Dictionary<string, string> GetAbilities()
        {
            return GetGovermentAbilities().Union(GetMilitaryAbilities()).ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<string, string> GetMilitaryAbilities()
        {
            return new Dictionary<string, string>
            {
                {"UnitAbilityGeneralOverall","軍隊強化"},
                {"UnitAbilityGeneralDamage","軍隊傷害強化"},
                {"UnitAbilityGeneralDefense","軍隊防禦強化"},
                {"UnitAbilityGeneralAttack","軍隊攻擊強化"},
                {"UnitAbilityGeneralHealth","軍隊生命強化"},
                {"UnitAbilityGeneralInitiative","軍隊主動性強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
                //{"UnitAbilityGeneralOverall","軍隊強化"},
            };
        }

        public static Dictionary<string, string> GetGovermentAbilities()
        {
            return new Dictionary<string, string>
            {
                {"UnitAbilityInitialBoostScience","初始科學強化"},
                {"UnitAbilityInitialBoostFood","初始食物強化"},
                {"UnitAbilityInitialBoostIndustry","初始工業強化"},
                {"UnitAbilityInitialBoostPrestige","初始權力強化"},
                {"UnitAbilityInitialBoostDust","初始星塵強化"},
                {"UnitAbilityIndustryPopulationEfficiency","工業成長強化"},
                {"UnitAbilityDustPopulationEfficiency","星塵成長強化"},
                {"UnitAbilitySciencePopulationEfficiency","科學成長強化"},
                {"UnitAbilityFoodPopulationEfficiency","食物成長強化"},
                {"UnitAbilityPrestigePopulationEfficiency","權力成長強化"},
            };
        }
    }
}
