using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Backend.Kernel;
using EndlessLegendEditor.DataObject;

namespace EndlessLegendEditor
{
    public partial class Form1 : Form
    {
        private string filePath = string.Empty;

        private ZipSave ZipSave;

        private XElement Document;

        private List<Empire> Empires;

        private Empire Current;

        private Hero Hero;

        private Army Army;

        private Unit Unit;

        private List<TextBox> SRControls = new List<TextBox>();

        private List<TextBox> LRControls = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Endless Legend Save|*.zip"
            };
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }
            if (string.IsNullOrWhiteSpace(filePath)) return;
            Parse();
            InitView();
        }

        private void Parse()
        {
            ZipSave = new ZipSave(filePath);
            Document = ZipSave.Document;
            //Document = XElement.Load(filePath);
            Empires = (from XElement e in Document.XPathSelectElement("./Game/Empires").Elements("MajorEmpire")
                       select new Empire(e)).ToList();
        }

        private void InitView()
        {
            Current = null;
            Hero = null;
            selectEmpire.Items.Clear();
            selectHero.Items.Clear();
            int i = 0;
            foreach (var e in Empires)
            {
                selectEmpire.Items.Add(new { Text = e.Name, Value = i });
            }
            SRControls = new List<TextBox>
            {
                txtSR1,txtSR2,txtSR3,txtSR4,txtSR5,txtSR6
            };
            LRControls = new List<TextBox>
            {
                txtLR1,txtLR2,txtLR3,txtLR4,txtLR5,txtLR6,txtLR7,txtLR8,txtLR9,txtLR10,txtLR11,txtLR12,txtLR13,txtLR14,txtLR15
            };
        }

        private void LoadEmpire()
        {
            txtAccount.Text = Current.Account.Value;
            txtPower.Text = Current.Power.Value;
            var i = 0;
            foreach (var r in Current.StrategicResources)
            {
                SRControls[i].Text = r.Value;
                i++;
            }
            i = 0;
            foreach (var r in Current.LuxuryResources)
            {
                LRControls[i].Text = r.Value;
                i++;
            }
            txtOrb.Text = Current.Perals.Value;
            txtNextPlanTurn.Text = Current.NextPlanTurn.Value;
            selectMilitaryPlan.SelectedIndex = Current.Plans[0].Level;
            selectKnowledgePlan.SelectedIndex = Current.Plans[1].Level;
            selectForeignPlan.SelectedIndex = Current.Plans[2].Level;
            selectEconomyPlan.SelectedIndex = Current.Plans[3].Level;
            selectArmy.DisplayMember = "Name";
            selectArmy.ValueMember = "Name";
            selectArmy.DataSource = Current.Armies;
        }

        private void LoadHero()
        {
            if (Hero == null) return;
            gaList.DataSource = null;
            maList.DataSource = null;
            oaList.DataSource = null;
            maList.DataSource = Ability.GetMilitaryAbilities().ToList();
            maList.DisplayMember = "Value";
            maList.ValueMember = "Key";
            gaList.DataSource = Ability.GetGovermentAbilities().ToList();
            gaList.DisplayMember = "Value";
            gaList.ValueMember = "Key";

            var i = 0;
            foreach (KeyValuePair<string, string> item in Ability.GetMilitaryAbilities())
            {
                if (Hero.Abilities.Any(x => x.Value == item.Key))
                {
                    maList.SetItemChecked(i, true);
                }
                i++;
            }
            i = 0;
            foreach (KeyValuePair<string, string> item in Ability.GetGovermentAbilities())
            {
                if (Hero.Abilities.Any(x => x.Value == item.Key))
                {
                    gaList.SetItemChecked(i, true);
                }
                i++;
            }
            var others =
                Hero.Abilities.Where(
                    x =>
                        !Ability.GetMilitaryAbilities().ContainsKey(x.Value) &&
                        !Ability.GetGovermentAbilities().ContainsKey(x.Value)).Select(x => x.Value).ToList();
            oaList.DataSource = others;
            for (var j = 0; j < oaList.Items.Count; j++)
            {
                oaList.SetItemChecked(j, true);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateEmpire();
            UpdateHero();
            ZipSave.Save();
            //Document.Save(filePath);
            MessageBox.Show("Success!");

        }

        void UpdateEmpire()
        {
            if (Current == null) return;
            try
            {
                decimal val;
                if (decimal.TryParse(txtAccount.Text, out val))
                {
                    Current.Account.Value = txtAccount.Text;
                }
                if (decimal.TryParse(txtPower.Text, out val))
                {
                    Current.Power.Value = txtPower.Text;
                }
                var i = 0;
                foreach (var r in SRControls)
                {
                    int count;
                    if (int.TryParse(r.Text, out count))
                    {
                        Current.StrategicResources[i].Value = r.Text;
                    }
                    i++;
                }
                i = 0;
                foreach (var r in LRControls)
                {
                    int count;
                    if (int.TryParse(r.Text, out count))
                    {
                        Current.LuxuryResources[i].Value = r.Text;
                    }
                    i++;
                }
                Current.Perals.Value = txtOrb.Text;
                UpdatePlan();
                if (Army != null && Army.Leader != null)
                {
                    Army.Leader.Exp.Value = txtLeaderExp.Text.ToDecimal().ToString();
                    Army.Leader.SkillPoint.Value = txtSkillpoints.Text.ToInt().ToString();
                }
                if (Unit != null)
                {
                    Unit.Exp.Value = txtUnitExp.Text.ToDecimal().ToString();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        void UpdatePlan()
        {
            Current.Plans[0].Level = selectMilitaryPlan.SelectedIndex.ToInt();
            Current.Plans[1].Level = selectKnowledgePlan.SelectedIndex.ToInt();
            Current.Plans[2].Level = selectForeignPlan.SelectedIndex.ToInt();
            Current.Plans[3].Level = selectEconomyPlan.SelectedIndex.ToInt();
        }

        void UpdateHero()
        {
            if (Hero == null) return;
            try
            {
                Hero.ClearAbilities();
                foreach (KeyValuePair<string, string> item in maList.CheckedItems)
                {
                    Hero.AddAbility(item.Key, 2);
                }
                foreach (KeyValuePair<string, string> item in gaList.CheckedItems)
                {
                    Hero.AddAbility(item.Key, 2);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void selectEmpire_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reset();
            Current = Empires[selectEmpire.SelectedIndex];
            Hero = null;
            LoadEmpire();

            foreach (var h in Current.Heroes)
            {
                selectHero.Items.Add(new { Text = h.Name, Value = h.Name });
            }
        }

        private void selectHero_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hero = Current.Heroes[selectHero.SelectedIndex];
            LoadHero();
        }

        private void selectArmy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Current == null) return;
            Army = Current.Armies.FirstOrDefault(x => x.Name == selectArmy.SelectedValue);
            if (Army != null)
            {
                selectUnit.DataSource = Army.Units.Select(x => x.Name).ToList();
                if (Army.Leader != null)
                {
                    lbLeaderPrototype.Text = Army.Leader.Name;
                    lbLeaderLv.Text = Army.Leader.Level.ToString();
                    txtLeaderExp.Text = Army.Leader.Exp.Value;
                    txtSkillpoints.Text = Army.Leader.SkillPoint.Value;
                }
                selectUnit.SelectedIndex = -1;
            }
            else
            {
                selectUnit.DataSource = null;
            }
        }

        private void selectUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Army == null) return;
            Unit = Army.Units.FirstOrDefault(x => x.Name == selectUnit.SelectedValue);
            if (Unit != null)
            {
                lbUnitName.Text = Unit.Name;
                lbUnitPrototype.Text = Unit.Prototype;
                lbUnitLv.Text = Unit.Level.ToString();
                txtUnitExp.Text = Unit.Exp.Value;
            }
        }

        private void Reset()
        {
            selectHero.Items.Clear();
            selectArmy.DataSource = null;
            selectUnit.DataSource = null;
        }
    }
}
