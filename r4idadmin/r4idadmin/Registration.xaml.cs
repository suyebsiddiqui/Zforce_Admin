using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Text.RegularExpressions;

namespace r4idadmin
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(primaryemail.Text.Length == 0 || secondaryemail.Text.Length == 0)
            {
                MessageBox.Show("Enter an Email");
                primaryemail.Focus();
                secondaryemail.Focus();
            }else if((!Regex.IsMatch(primaryemail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")) || (!Regex.IsMatch(secondaryemail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))) {
                MessageBox.Show("Enter a valid email");
            }
            else
            {
                string Firstname = firstname.Text;
                string Middlename = middlename.Text;
                string Lastname = lastname.Text;
                string Primaryemail = primaryemail.Text;
                string Secondaryemail = secondaryemail.Text;
                string Username = username.Text;
                string Password = password.Text;
                string Confirmpassword = confirmpassword.Text;
                string Addressline1 = addressline1.Text;
                string Addressline2 = addressline2.Text;
                string City = city.Text;
                string State = state.Text;
                string Country = country.Text;
                string Pincode = pincode.Text;
                string Primarymobile = primarymobile.Text;
                string Secondarymobile = secondarymobile.Text;
                if (Password.Length == 0)
                {
                    MessageBox.Show("Enter password");
                    password.Focus();
                }
                else if (Confirmpassword.Length == 0)
                {
                    MessageBox.Show("Enter Confirm Password");
                    confirmpassword.Focus();
                }
                else if (Password != Confirmpassword)
                {
                    MessageBox.Show("Confirm Password must match Password");
                    confirmpassword.Focus();
                }
                else
                {
                    try {
                        SqlConnection con = new SqlConnection(@"Data Source=ARIJIT\SQLEXPRESS;Initial Catalog=R4ID;Integrated Security=True");
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select count(*) from [User]", con);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        cmd.CommandType = CommandType.Text;
                        adapter.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        int count = ds.Tables[0].Rows.Count;
                        string Userid = "R4ID" + Username.ToString();
                        string Userrole = "Player", Creatorid = "cr001", Cafeid = "ca001", Walletid = "walletid", Lastlogin = "lastlogincaf", Uniqueidtype = "uniqueidtype", Uniqueidnum = "number";
                        cmd.CommandText = "Insert into [User] values ('" + Userid + "','" + Username + "' , '" + Password + "', '"+Userrole+"', '" + Firstname + "','" + Middlename + "', '" + Lastname + "', '" + Primaryemail + "', '" + Secondaryemail + "', '" + Primarymobile + "', '" + Secondarymobile + "', '" + Addressline1 + "', '" + Addressline2 + "', '" + City + "', '" + State + "', '" + Country + "','" + Pincode + "', '"+Creatorid+"','"+Cafeid+"','"+Walletid+"','"+Lastlogin+"','"+Uniqueidtype+"','"+Uniqueidnum+"')";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Successfully Registered");
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

                

            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
