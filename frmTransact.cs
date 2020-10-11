using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormRyan
{
    public partial class frmTransact : Form
    {
        public frmTransact()
        {
            InitializeComponent();
        }

        private void transactionsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.transactionsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.transactBitDataSet);

        }

        private void frmTransact_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'transactBitDataSet.Transactions' table. You can move, or remove it, as needed.
            this.transactionsTableAdapter.Fill(this.transactBitDataSet.Transactions);

        }

        private void fillByTypeToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.transactionsTableAdapter.FillByType(this.transactBitDataSet.Transactions, typeToolStripTextBox.Text);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
