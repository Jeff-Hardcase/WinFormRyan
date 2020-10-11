using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormRyan
{
    public partial class frmTransactions : Form
    {
        public frmTransactions()
        {
            InitializeComponent();
        }

        private void frmTransactions_Load(object sender, EventArgs e)
        {
            BindTransactionsGrid();
        }

        private void BindTransactionsGrid(string typeFilter = null)
        {
            var connString = Properties.Settings.Default.TransactBitConnectionString;
            var sqlText = @"SELECT [Time (UTC)]
                          ,[Type]
                          ,[Symbol]
                          ,[Specification]
                          ,[USD Amount]
                          ,[Trading Fee (USD)]
                          ,[USD Balance]
                          ,[BTC Amount]
                          ,[BTC Balance]
                          ,[ETH Amount]
                          ,[ETH Balance]
                      FROM[TransactBit].[dbo].[Transactions]";

            SqlParameter sqlParameter = null;

            if(!string.IsNullOrEmpty(typeFilter))
            {
                sqlText += " WHERE [Type] = @Type";

                sqlParameter = new SqlParameter
                {
                    ParameterName = "@Type",
                    Value = typeFilter
                };
            }

            var conn = new SqlConnection(connString);
            var cmd = new SqlCommand(sqlText, conn);

            cmd.CommandType = CommandType.Text;

            if (sqlParameter != null)
                cmd.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();

            da.Fill(ds);

            dgTransactions.DataSource = ds.Tables[0];
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            BindTransactionsGrid(txtType.Text);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindTransactionsGrid();
        }
    }
}
