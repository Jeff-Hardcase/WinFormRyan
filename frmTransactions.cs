using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormRyan.Models;

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
            BindAircraftGrid();
        }

        
        private void BindAircraftGrid(IEnumerable<FilterDetails> filters = null)
        {
            var connString = Properties.Settings.Default.AircraftConnectionString;
            var sqlText = @"SELECT * From AircraftValues";

            List<SqlParameter> paramList = new List<SqlParameter>();

            if (filters != null)
            {
                var count = 0;

                foreach(var filter in filters)
                {
                    sqlText += (count == 0) ? " WHERE" : " AND";
                    sqlText += $" [{filter.ColumnName}] LIKE '%' + @{filter.ColumnName}Value + '%'";

                    paramList.Add(
                        new SqlParameter
                        {
                            ParameterName = $"@{filter.ColumnName}Value",
                            Value = filter.FilterValue
                        }
                    );

                    count++;
                }
            }

            var conn = new SqlConnection(connString);
            var cmd = new SqlCommand(sqlText, conn)
            {
                CommandType = CommandType.Text
            };

            foreach (var param in paramList)
            {
                cmd.Parameters.Add(param);
            }

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();

            da.Fill(ds);

            dgTransactions.DataSource = ds.Tables[0];
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var filterList = new List<FilterDetails>();

            foreach(Control control in panelFilters.Controls)
            {
                if(control is TextBox)
                {
                    if(!string.IsNullOrEmpty(control.Text))
                    {
                        var thisFilter = new FilterDetails 
                        { 
                            ColumnName = control.Tag.ToString(), 
                            FilterValue = control.Text
                        };

                        filterList.Add(thisFilter);
                    }
                }
            }

            BindAircraftGrid(filterList);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (Control control in panelFilters.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = string.Empty;
                }
            }

            BindAircraftGrid();
        }

    }
}
