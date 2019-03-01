using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityAI
{
    public partial class Form1 : Form
    {
        private const string fCreateDatabase = "Create Database";
        private const string fCreateTable = "Create Table";
        private const string fLoadTable = "Load Table";
        private const string fSelectTable = "Select from Table";

        public Form1()
        {
            InitializeComponent();
            PopulateFunctions();
        }

        private void PopulateFunctions()
        {
            comboBox1.Items.Add(fCreateDatabase);
            comboBox1.Items.Add(fCreateTable);
            comboBox1.Items.Add(fLoadTable);
            comboBox1.Items.Add(fSelectTable);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            DAC dac = new DAC();

            switch (comboBox1.Text)
            {
                case fCreateDatabase:
                    Output("Creating database");
                    dac.CreateFile();
                    Output("Creating database - complete");
                    break;
                case fCreateTable:
                    Output("Creating table");
                    dac.CreateTable();
                    Output("Creating table - complete");
                    break;
                case fLoadTable:
                    Output("inserting data");
                    dac.InsertData();
                    Output("inserting table - complete");
                    break;
                case fSelectTable:
                    Output("selecting data");
                    DataSet ds = dac.SelectData();
                    SetData(ds);
                    Output("selecting data - complete");
                    break;
                default:
                    break;
            }
        }

        public void SetData(DataSet ds)
        {
            if(ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        public void Output(string msg)
        {
            tbOutput.AppendText(DateTime.Now.ToString("HH:mm:ss"));
            tbOutput.AppendText(": ");
            tbOutput.AppendText(msg);
            tbOutput.AppendText(Environment.NewLine);
        }
    }
}
