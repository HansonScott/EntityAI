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
        }

        internal void LoadEntity(Entity e)
        {
            this.CurrentEntity = e;
            LoadEntityDetails();
        }

        private void LoadEntityDetails()
        {
            // dgvAttributes;
            // dgvSenses;
            // dgvNeeds;
            // dgvActions;
        }
    }
}
