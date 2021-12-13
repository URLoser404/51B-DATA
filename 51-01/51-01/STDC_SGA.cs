using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _51_01
{
    public partial class STDC_SGA : Form
    {
        DataTable dt_user;
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=51;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public DataTable sql(string command)
        {
            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public STDC_SGA(DataTable dt_user)
        {
            InitializeComponent();
            this.dt_user = dt_user;
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            try
            {
                sql($"insert into [SystemLog] values({dt_user.Rows[0]["id"]},2,GETDATE())");
                Form f = new Login();
                f.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
