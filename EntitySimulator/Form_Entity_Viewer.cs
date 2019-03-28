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

        public Form_Entity_Viewer()
        {
            InitializeComponent();
            SetupGrids();
        }

        private void SetupGrids()
        {
            dgvAttributes.Columns.Add("Attribute", "Attribute");
            dgvAttributes.Columns.Add("Value", "Value");

            dgvSenses.Columns.Add("Sense", "Sense");
            dgvSenses.Columns.Add("Value", "Value");

            dgvNeeds.Columns.Add("Need", "Need");
            dgvNeeds.Columns.Add("Value", "Value");

            dgvActions.Columns.Add("Action", "Action");
            dgvActions.Columns.Add("Target", "Target");
            dgvActions.Columns.Add("Item", "Item");
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
                dgvNeeds.Rows.Add(n.Name, "");
            }

            // dgvActions;
            dgvActions.Rows.Clear();
            foreach(EntityAction e in this.CurrentEntity.actions.ActionQueue)
            {
                dgvActions.Rows.Add(e.ability.Name, e.Target?.ToString(), e.Item?.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadEntityDetails();
            this.splitContainer1.Refresh();
        }
    }
}
