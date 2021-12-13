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
    public partial class STDC_SA : Form
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
        public STDC_SA(DataTable dt_user)
        {
            InitializeComponent();
            this.dt_user = dt_user;
        }
        private void STDC_SA_Load(object sender, EventArgs e)
        {
            dgv_systemLog.DataSource = sql("select [SystemLog].id,time,[User].id user_id,[user].account user_account,Action.name from SystemLog  " +
                "inner join Action " +
                "on SystemLog.action_id = Action.id " +
                "inner join[user] " +
                "on SystemLog.user_id = [user].id");
            dgv_user.DataSource = sql("select [user].id,[user].account,[user].password,[user].errorCount,[user].status,[PermissionArea].name permissionArea,[PermissionUser].name permissionUser from [User] " +
                "inner join[PermissionArea] on[user].permissionArea_id = [PermissionArea].id " +
                "inner join[PermissionUser] on[User].permissionUser_id = [PermissionUser].id");
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgv_user_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var tmp = dgv_user.SelectedRows[0];
                tb_account.Text = tmp.Cells["account"].Value.ToString();
                tb_password.Text = tmp.Cells["password"].Value.ToString();

                Dictionary<string, string> permissionUser = new Dictionary<string, string>
                    {
                        { "SGA","STDC_SGA"},
                        { "SSA","STDC_SSA" },
                        { "BD","STDC_BD" },
                        { "TRA","CM_TRA" },
                        { "TD","CM_TD" },
                        { "PG","TR_PG" },
                        { "SA","STDC_SA" }
                    };
                cb_role.Text = permissionUser[tmp.Cells["permissionUser"].Value.ToString()];
                tb_errorCount.Text = tmp.Cells["errorCount"].Value.ToString();
                tb_status.Text = tmp.Cells["status"].Value.ToString();
            }
            catch
            {
                
            }
        }

        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> permissionArea = new Dictionary<string, string>
                    {
                        { "STDC_SGA","1"},
                        { "STDC_SSA","1"},
                        { "STDC_BD","1"},
                        { "CM_TRA","2"},
                        { "CM_TD","2"},
                        { "TR_PG","3"},
                        { "STDC_SA","1"}
                    };
                Dictionary<string, string> permissionUser = new Dictionary<string, string>
                    {
                        { "STDC_SGA","1"},
                        { "STDC_SSA","2"},
                        { "STDC_BD","3"},
                        { "CM_TRA","4"},
                        { "CM_TD","5"},
                        { "TR_PG","6"},
                        { "STDC_SA","7"}
                    };

                sql("update [user]  " +
                    $"set password = '{tb_password.Text}', " +
                    $"errorCount = {tb_errorCount.Text}, " +
                    $"status = '{tb_status.Text}', " +
                    $"permissionArea_id = {permissionArea[cb_role.Text]}, " +
                    $"permissionUser_id = {permissionUser[cb_role.Text]} " +
                    $"where id = {dgv_user.SelectedRows[0].Cells["id"].Value}");
                STDC_SA_Load(sender, e);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_account = sql("select [user].id,[user].account,[user].password,[user].errorCount,[user].status,[PermissionArea].name permissionArea,[PermissionUser].name permissionUser from [User] " +
                    "inner join[PermissionArea] on[user].permissionArea_id = [PermissionArea].id " +
                    "inner join[PermissionUser] on[User].permissionUser_id = [PermissionUser].id " +
                    $"where[user].account = '{tb_account.Text}'");
                if (dt_account.Rows.Count == 0)
                {
                    Dictionary<string, string> permissionArea = new Dictionary<string, string>
                    {
                        { "STDC_SGA","1"},
                        { "STDC_SSA","1"},
                        { "STDC_BD","1"},
                        { "CM_TRA","2"},
                        { "CM_TD","2"},
                        { "TR_PG","3"},
                        { "STDC_SA","1"}
                    };
                    Dictionary<string, string> permissionUser = new Dictionary<string, string>
                    {
                        { "STDC_SGA","1"},
                        { "STDC_SSA","2"},
                        { "STDC_BD","3"},
                        { "CM_TRA","4"},
                        { "CM_TD","5"},
                        { "TR_PG","6"},
                        { "STDC_SA","7"}
                    };

                    sql($"insert into [user] values({permissionArea[cb_role.Text]},{permissionUser[cb_role.Text]},'{tb_account.Text}','{tb_password.Text}',0,1)");

                    STDC_SA_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("account is already exist");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                sql("update [user]  " +
                    $"set status = 0 " +
                    $"where id = {dgv_user.SelectedRows[0].Cells["id"].Value}");
                STDC_SA_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
