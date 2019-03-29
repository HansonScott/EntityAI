using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityAI;

namespace EntitySimulator
{
    public partial class Form_Entity_Viewer : Form
    {
        private Entity CurrentEntity;

        private bool ShouldRefresh = true;
        Timer RefreshTimer;

        public Form_Entity_Viewer()
        {
            InitializeComponent();
            SetupGrids();
            SetupTimer();
        }

        private void SetupTimer()
        {
            RefreshTimer = new Timer();
            RefreshTimer.Interval = 1000;
            RefreshTimer.Tick += T_Tick;
        }

        private void SetupGrids()
        {
            dgvAttributes.Columns.Add("Attribute", "Attribute");
            dgvAttributes.Columns.Add("Value", "Value");

            dgvSenses.Columns.Add("Sense", "Sense");
            dgvSenses.Columns.Add("Value", "Value");

            dgvNeeds.Columns.Add("Need", "Need");
            dgvNeeds.Columns.Add("Value", "Value");

            dgvOpportunities.Columns.Add("Opportunity", "Opportunity");
            dgvOpportunities.Columns.Add("Value", "Value");

            dgvSolutions.Columns.Add("Solution", "Solution");
            dgvSolutions.Columns.Add("Value", "Value");

            dgvOpportunitySolutions.Columns.Add("Solution", "Solution");
            dgvOpportunitySolutions.Columns.Add("Value", "Value");

            dgvInventory.Columns.Add("Item", "Item");
            dgvInventory.Columns.Add("Description", "Description");

            dgvActions.Columns.Add("Action", "Action");
            dgvActions.Columns.Add("Target", "Target");
            dgvActions.Columns.Add("Item", "Item");
            dgvActions.Columns.Add("Status", "Status");
        }

        internal void LoadEntity(Entity e)
        {
            this.CurrentEntity = e;
            LoadEntityDetails();
        }

        private void LoadEntityDetails()
        {
            // dgvAttributes;
            dgvAttributes.Rows.Clear();
            foreach (CoreAttribute c in this.CurrentEntity.coreAttributes)
            {
                dgvAttributes.Rows.Add(c.Name, c.CurrentValue.ToString("F2"));
            }

            // dgvSenses;
            dgvSenses.Rows.Clear();
            foreach(Sensor s in this.CurrentEntity.senses.sensors)
            {
                dgvSenses.Rows.Add(s.Name, s.CurrentValue.ToString("F2"));
            }

            // dgvNeeds;
            dgvNeeds.Rows.Clear();
            foreach(EntityNeed n in this.CurrentEntity.CurrentNeeds)
            {
                dgvNeeds.Rows.Add(n.Name, n.Urgency);
            }

            // dgvOpportunities;
            dgvOpportunities.Rows.Clear();
            foreach (EntityNeed n in this.CurrentEntity.CurrentOpportunities)
            {
                dgvOpportunities.Rows.Add(n.Name, n.Urgency);
            }

            // dgvSolutions;
            dgvSolutions.Rows.Clear();
            foreach(Solution s in this.CurrentEntity.CurrentSolutions)
            {
                dgvSolutions.Rows.Add(s.SolutionState.ToString(), s.Description);
            }

            // dgvOpportunitySolutions;
            dgvOpportunitySolutions.Rows.Clear();
            foreach (Solution s in this.CurrentEntity.CurrentOpportunitySolutions)
            {
                dgvOpportunitySolutions.Rows.Add(s.SolutionState.ToString(), s.Description);
            }

            // dgvInventory
            dgvInventory.Rows.Clear();
            foreach(EntityResource item in this.CurrentEntity.Inventory.Items)
            {
                dgvInventory.Rows.Add(item.RType.ToString(), item.Quantity);
            }

            // dgvActions;
            dgvActions.Rows.Clear();
            foreach(EntityAction e in this.CurrentEntity.actions.ActionQueue)
            {
                string a = e.ability.Name;
                string t = string.Empty;
                if(e.Target != null)
                {
                    if(e.Target is EntityResource)
                    {
                        t = (e.Target as EntityResource).RType.ToString();
                    }
                    else
                    {
                        t = e.Target.ToString();
                    }
                }
                string i = string.Empty;
                if(e.Item != null)
                {
                    if(e.Item is EntityResource)
                    {
                        i = (e.Item as EntityResource).RType.ToString();
                    }
                    else
                    {
                        i = e.Item.ToString();
                    }
                }
                string s = e.ActionState.ToString();
                dgvActions.Rows.Add(a, t, i, s);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadEntityDetails();
            this.tableLayoutPanel1.Refresh();
        }

        private void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            ShouldRefresh = cbAutoRefresh.Checked;
        }

        private void Form_Entity_Viewer_Load(object sender, EventArgs e)
        {
            RefreshTimer.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if(ShouldRefresh)
            {
                LoadEntityDetails();
                this.tableLayoutPanel1.Refresh();
            }
        }

        private void Form_Entity_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShouldRefresh = false;
        }
    }
}
