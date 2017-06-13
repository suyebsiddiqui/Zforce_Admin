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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace r4idadmin
{           
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DBConnect Dbconnect;
        public Login()
        {
            Dbconnect = new DBConnect();
            InitializeComponent();
        }
        Registration reg = new Registration();

        private void but_login_Click(object sender, RoutedEventArgs e)
        {
            

            String Username = txt_username.Text;
            String Password = txt_paswword.Password;
            if (txt_username.Text == "" || txt_paswword.Password == "")
            {
                MessageBox.Show("Please provide Username and Password");
                return;
            }
            try
            {
               
                
                if (Dbconnect.AuthenticateUser(Username, Password))
                {
                    this.Close();
                    System.Media.SystemSounds.Exclamation.Play();
                    dashboard dash = new dashboard();
                    dash.Show();

                }
                else
                {
                   
                    MessageBox.Show("Invalid Username/Password");
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }




        }
        private void register_click(object sender, RoutedEventArgs e)
        {
            reg.Show();
            Close();
        }

       
    }

    class DBConnect
    {
        private MySqlConnection sqlConnection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public string connection_global;

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            //server = "192.168.0.2";
            server = "localhost";
            database = "cafe_manager";
            uid = "root";
            password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection_global = connectionString;
            sqlConnection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }

        }

        public bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool AuthenticateUser(String Username, String Password)
        {
            string query = "SELECT * from Admin where UserName=" + "'" + Username + "'" + " And" + " Pass=" + "'" + Password + "'" + "";

            //Open connection
            if (this.OpenConnection() == true)
            {
                
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                  
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                
                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;
        }

        public string getBalance(String walletId)
        {
            String balance = null;
            string query = "select WalletOfflieAmount from wallet where WalletId=" + "'" + walletId + "'";
            //Open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand mcd1 = new MySqlCommand(query, sqlConnection);
                MySqlDataReader mdr1 = mcd1.ExecuteReader();
                if (mdr1.Read())
                {

                    balance = mdr1.GetString("WalletOfflieAmount");
                }
                else
                    MessageBox.Show("No Wallet details found");
                
            }
            this.CloseConnection();
            return balance;
            

        }

        public bool IsIPPresent(String Ip)
        {
            string query = "SELECT * from workstationDetails where Ip=" + "'" + Ip + "'" + "AND Isactive=" + 1 + "";


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;
        }

        internal bool AddPc(string ip, string mac, string name, string info)
        {
            String query = "INSERT INTO workstationDetails (MachineName, Ip, MacAddress, MachineInfo,Isactive) VALUES(" + "'" + name + "'" + "," + "'" + ip + "'" + "," + "'" + mac + "'" + "," + "'" + info + "'" + "," + 1 + ")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
                return true;
            }
            return false;
        }

        internal MySqlDataReader getPcDetails()
        {
            string query = "SELECT * from workstationDetails where Isactive=" + 1 + "";


            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();


                //close Connection
                this.CloseConnection();


                return dataReader;
            }
            return null;
        }

        internal bool CheckUsername(string username)
        {
            string query = "SELECT * from user where UserName=" + "'" + username + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;

        }

        internal bool CheckEmail1(string Email)
        {
            string query = "SELECT * from user where Email1=" + "'" + Email + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;

        }

        internal bool CheckMobile(string Mobile)
        {
            string query = "SELECT * from user where Mobile1=" + "'" + Mobile + "'" + "";
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                if (dataReader.HasRows)
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                else
                {
                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();
                    return false;
                }
            }
            return false;

        }

        internal bool Register(User user)
        {

            String Getusercount = "select count(UserId) from user";
            String Getcafename = "select CafeName from cafe";
            //Create a list to store the result
            int usercount = 0;
            String cafename = null;
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd2 = new MySqlCommand(Getusercount, sqlConnection);
                usercount = Convert.ToInt32(cmd2.ExecuteScalar());
                //MySqlDataReader dataReader1 = cmd2.ExecuteReader();
                //while(dataReader1.HasRows)
                //{
                //  usercount = dataReader1["count"].ToString();
                //}
                MessageBox.Show(usercount.ToString());
                //close Connection
                MySqlCommand cmd1 = new MySqlCommand(Getcafename, sqlConnection);

                MySqlDataReader dataReader = cmd1.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    cafename = dataReader["CafeName"].ToString();
                    MessageBox.Show(cafename);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed

            }
            usercount++;


            String UserId = cafename + "u" + usercount;
            MessageBox.Show(UserId);
            String WalletId = cafename + "w" + usercount;

            String query = "INSERT INTO user(UserId,UserName,Pass,FirstName, LastName, Email1, Mobile1, AddressLine1, AddressLine2, City, State, Country, Pincode,IsUserLoggedIn,WalletId) VALUES(" + "'" + UserId + "'" + "," + "'" + user.Username + "'" + "," + "'" + user.Password + "'" + "," + "'" + user.Fname + "'" + "," + "'" + user.Lname + "'" + "," + "'" + user.Email + "'" + "," + "'" + user.mobile + "'" + "," + "'" + user.Add1 + "'" + "," + "'" + user.Add2 + "'" + "," + "'" + user.City + "'" + "," + "'" + user.State + "'" + "," + "'" + user.Country + "'" + "," + "'" + user.Pincode + "'" + "," + 0 + "," + "'" + WalletId + "'" + ")";
            String query1 = "Insert into wallet (WalletId,WalletOfflieAmount) Values (" + "'" + WalletId + "'" + "," + "'" + 0 + "'" + ")";
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

                //Execute command
                cmd.ExecuteNonQuery();


                //close connection
                this.CloseConnection();
                this.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand(query1, sqlConnection);
                cmd1.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }

            return false;
        }
    }
    }


