using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace _51_01
{
    public partial class Login : Form
    {
        SqlConnection cn = new SqlConnection("Data Source=.;Initial Catalog=51;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public DataTable sql(string command)
        {
            SqlDataAdapter da = new SqlDataAdapter(command, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public Login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_account = sql("select [user].id,[user].account,[user].password,[user].errorCount,[user].status,[PermissionArea].name permissionArea,[PermissionUser].name permissionUser from [User] " +
                    "inner join[PermissionArea] on[user].permissionArea_id = [PermissionArea].id " +
                    "inner join[PermissionUser] on[User].permissionUser_id = [PermissionUser].id " +
                    $"where[user].account = '{tb_account.Text}'");
                if (dt_account.Rows.Count > 0)
                {
                    DataTable dt_password = sql("select [user].id,[user].account,[user].password,[user].errorCount,[user].status,[PermissionArea].name permissionArea,[PermissionUser].name permissionUser from [User] " +
                        "inner join[PermissionArea] on[user].permissionArea_id = [PermissionArea].id " +
                        "inner join[PermissionUser] on[User].permissionUser_id = [PermissionUser].id " +
                        $"where[user].account = '{tb_account.Text}' " +
                        $"and [user].password = '{tb_password.Text}'");
                    if (dt_password.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt_password.Rows[0]["errorCount"]) < 3)
                        {
                            sql($"update [user] set errorCount = 0 where id = {dt_password.Rows[0]["id"]} ");
                            sql($"insert into [SystemLog] values({dt_password.Rows[0]["id"]},1,GETDATE())");
                            Form f = new Form();
                            switch (dt_password.Rows[0]["permissionUser"])
                            {
                                case "SA":
                                    f =  new STDC_SA(dt_password);
                                    break;
                                case "SSA":
                                    break;
                                case "SGA":
                                    f = new STDC_SGA(dt_password);
                                    break;
                                case "BD":
                                    f = new STDC_BD(dt_password);
                                    break;
                                case "TRA":
                                    break;
                                case "TD":
                                    break;
                                case "PD":
                                    break;
                            }
                            f.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("error count >= 3");
                        }
                    }
                    else
                    {
                        sql($"update [user] set errorCount+=1 where id = {dt_account.Rows[0]["id"]} ");
                        MessageBox.Show("password wrong");
                    }
                }
                else
                {
                    MessageBox.Show("accout not found");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_signup_Click(object sender, EventArgs e)
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
                        { "TR_PG","3"},
                        { "CM_TD","2" },
                        { "STDC_BD","1" }
                    };
                    Dictionary<string, string> permissionUser = new Dictionary<string, string>
                    {
                        { "TR_PG","6"},
                        { "CM_TD","5" },
                        { "STDC_BD","3" }
                    };
                    
                    sql($"insert into [user] values({permissionArea[cb_role.Text]},{permissionUser[cb_role.Text]},'{tb_account.Text}','{tb_password.Text}',0,1)");

                    MessageBox.Show("sign up done");
                }
                else
                {
                    MessageBox.Show("account is already exist");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
