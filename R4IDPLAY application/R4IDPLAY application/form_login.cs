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

namespace R4IDPLAY_application
{
    public partial class form_login : Form
    {
        string connectionString = @"Data Source=ARIJIT\SQLEXPRESS;Initial Catalog=R4ID;Integrated Security=True";
        public form_login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_login_Click(object sender, EventArgs e)
        {
            if(txt_username.Text == "" || txt_password.Text == "")
            {
                MessageBox.Show("Please provide Username and Password");
                return;
            }
            try {
                SqlConnection sqlCon = new SqlConnection(connectionString);
                sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("select * from [User] where username=@usrname and password=@pass", sqlCon);
                sqlcmd.Parameters.AddWithValue("@usrname", txt_username.Text);
                sqlcmd.Parameters.AddWithValue("@pass", txt_password.Text);
                SqlDataAdapter adpt = new SqlDataAdapter(sqlcmd);
                DataSet ds = new DataSet();
                adpt.Fill(ds);
                sqlCon.Close();
                int count = ds.Tables[0].Rows.Count;

                if (count == 1)
                {
                    MessageBox.Show("Login Successful");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Login");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
