using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace r4idadmin
{
   
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class dashboard : Window
    {

        
        ListCollectionView CollectionViewList;
        DataTable DataTableFiltered;

        private readonly TimeSpan filterDelay = TimeSpan.FromMilliseconds(350);
        private DelayAction delayedAction;

        String WalletId = null;
        DBConnect Dbconnect;

        MySqlConnection mcon;
        MySqlCommand mcd;
        MySqlDataReader mdr;
        string s;

        DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
        ObservableCollection<string> stDispatcher = new ObservableCollection<string>();



        void dispTimer_Tick(object sender, EventArgs e)
        {
            LoadMonitor();

        }

        public dashboard()
        {
            InitializeComponent();
            Dbconnect = new DBConnect();
            mcon = new MySqlConnection(Dbconnect.connection_global);

            Recharge_Amt_re.IsReadOnly = true;
            button1_re.IsEnabled = false;
            txt_Balance.IsReadOnly = true;

            dispTimer.Interval = TimeSpan.FromSeconds(30);
            dispTimer.Tick += new EventHandler(dispTimer_Tick);
            dispTimer.IsEnabled = true;
            stDispatcher.Clear();
            


        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            dataGrid.ItemsSource = LoadDataGrid().DefaultView;
            //dataGrid.Columns[0].Visibility = Visibility.Hidden;
            SetStatusColumn();
        }

        public void LoadMonitor()
        {
            //dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = LoadDataGrid().DefaultView;
            //dataGrid.Columns[0].Visibility = Visibility.Hidden;
            //SetStatusColumn();
        }

        private DataTable LoadDataGrid()
        {
            DataTable dt = new DataTable();
            DataTableFiltered = new DataTable();

            List<List<string>> alist = new List<List<string>>() {  };

            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Machine Name", typeof(string));
            dt.Columns.Add("Current User ID", typeof(string));
            dt.Columns.Add("Current Amount (INR)", typeof(string));
            dt.Columns.Add("Potential Customer", typeof(string));
            dt.Columns.Add("Active Game", typeof(string));

            try
            {

                mcon.Open();
                s = "select * from monitor_dummy";
                mcd = new MySqlCommand(s, mcon);
                mdr = mcd.ExecuteReader();
               // Boolean flag = false;
                while (mdr.Read())
                {
                    dt.Rows.Add(mdr.GetString("Stauts"), mdr.GetString("MachineName"), mdr.GetString("UserId"), mdr.GetString("CurrentAmount"), mdr.GetString("PotentialCustomer"), mdr.GetString("ActiveGame"));
                    alist.Add(new List<string>() { mdr.GetString("Stauts"), mdr.GetString("MachineName"), mdr.GetString("UserId"), mdr.GetString("CurrentAmount"), mdr.GetString("PotentialCustomer"), mdr.GetString("ActiveGame") });
                   // flag = true;

                }
                /*
                if(flag == false)
                {

                }
                */

                bool isEmpty = !alist.Any();
                if (isEmpty)
                {
                    // error message
                    dt.Rows.Add("NA", "NA", "NA", "NA", "NA", "NA");
                    alist.Add(new List<string>() { "NA", "NA", "NA", "NA", "NA", "NA" });
                }
                else
                {
                    // show grid
                }

                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mdr.Close();
                mcon.Close();
            }


            




            /*
            dt.Rows.Add("Active", "Optimus Prime", "suyeb", "100", "Yes", "Dota");
            dt.Rows.Add("Free", "Iron Man", "rohit", "200", "Yes", "CS");
            dt.Rows.Add("Crash", "The Rock", "kewal", "300", "Yes", "CS");
            dt.Rows.Add("Crash", "Captain America", "Ben", "400", "No", "Dota");
            dt.Rows.Add("Active", "Spider Man", "Kaila", "500", "No", "CS");
            */

            DataTableFiltered.Columns.Add("Status", typeof(string));
            DataTableFiltered.Columns.Add("Column1", typeof(string));
            DataTableFiltered.Columns.Add("Column2", typeof(string));
            DataTableFiltered.Columns.Add("Column3", typeof(string));
            DataTableFiltered.Columns.Add("Column4", typeof(string));
            DataTableFiltered.Columns.Add("Column5", typeof(string));

            /*
            List<string> list1 = new List<string>() { "Active", "Optimus Prime", "suyeb", "100", "Yes", "Dota" };
            List<string> list2 = new List<string>() { "Free", "Iron Man", "rohit", "200", "Yes", "CS" };
            List<string> list3 = new List<string>() { "Crash", "The Rock", "kewal", "300", "Yes", "CS" };
            List<string> list4 = new List<string>() { "Crash", "Captain America", "Ben", "400", "No", "Dota" };
            List<string> list5 = new List<string>() { "Active", "Spider Man", "Kaila", "500", "No", "CS" };
            */

            // List<List<string>> alist = new List<List<string>>() { list1, list2, list3, list4, list5 };
            //alist.Add(list1);


            IList results = (IList)alist;

            CollectionViewList = new ListCollectionView(results);

            return dt;
        }

        public bool ContainsIt(object value)
        {
            if (DataTableFiltered.Columns.Count > 1)
            {
                //There is more than 1 column in DataGrid
                List<string> DataGridRowList = (List<string>)value;
                foreach (object item in DataGridRowList)
                {
                    if (item != null)
                    {
                        if (item.ToString().ToLower().Contains(textBox.Text.ToLower())) return true;
                    }
                }
            }
            else
            {
                //There is a single column in the DataGrid
                if (value.ToString().ToLower().Contains(textBox.Text.ToLower())) return true;
            }
            return false;
        }

        public void FilterIt()
        {
            int count = 0;
            DataTableFiltered.Clear();
            dataGrid.ItemsSource = null;

            foreach (List<string> row in CollectionViewList)
            {
                DataTableFiltered.Rows.Add(row[0], row[1], row[2], row[3], row[4]);
                count++;
            }

            dataGrid.ItemsSource = DataTableFiltered.DefaultView;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text != "")
            {
                if (CollectionViewList.CanFilter)
                {
                    CollectionViewList.Filter = new Predicate<object>(ContainsIt);

                    if (delayedAction == null)
                    {
                        delayedAction = DelayAction.Initialize(() => FilterIt());
                    }
                    delayedAction.Wait(filterDelay);
                }
                else
                {
                    CollectionViewList.Filter = null;
                }
            }
            else
            {
                CollectionViewList.Filter = null;
                FilterIt();
            }
        }

        public void SetStatusColumn()
        {
            DataGridTemplateColumn statusColumn = new DataGridTemplateColumn { CanUserReorder = false, Width = 85, CanUserSort = false }; ;
            statusColumn.Header = "Status";
            statusColumn.CellTemplateSelector = new DataTemplateSelectorBase();
            dataGrid.Columns.Insert(0, statusColumn);
        }

        private void recharge(object sender, RoutedEventArgs e)
	       {
	            string value = "";
	            decimal old_Recharge_Amt_re_value;
	            decimal new_Recharge_Amt_re_value = Convert.ToDecimal(Recharge_Amt_re.Text.Trim());
	            if (Recharge_Amt_re.Text.Length == 0)
	            {
	                MessageBox.Show("Recharge Amount can not be BLANK");
                Recharge_Amt_re.Focus();
	                return;
	            }
	            else if (new_Recharge_Amt_re_value< 0)
	            {
	                MessageBox.Show("Recharge Amount can not be NEGATIVE");
	                Recharge_Amt_re.Focus();
	                return;
	            }
	            else if (new_Recharge_Amt_re_value == 0)
	            {
	                MessageBox.Show("Recharge Amount can not be ZERO");
	                Recharge_Amt_re.Focus();
	                return;
	            }
	            else if ((!Regex.IsMatch(Recharge_Amt_re.Text, @"^\d*\d?((50)|(0))\.?((0)|(00))?$")))
	            {
	                MessageBox.Show("Enter Amount multiples of Rs. 50");
	                Recharge_Amt_re.Focus();
	                return;
	            }


            value = Dbconnect.getBalance(WalletId);
	            
	            try
	            {
	                old_Recharge_Amt_re_value = Convert.ToDecimal(value);
	                string input = WalletId;
	                decimal udated_amount = new_Recharge_Amt_re_value + old_Recharge_Amt_re_value;
                    MessageBox.Show("Final value: "+udated_amount.ToString());
                mcon.Open();
	                s = "Update wallet set WalletOfflieAmount = " + "'" + udated_amount + "'" + "where WalletId = " + "'" + input + "'" + "";
                
                mcd = new MySqlCommand(s, mcon);
	                //mdr = mcd.ExecuteReader();
	                mcd.ExecuteNonQuery();
                txt_Balance.Text = Dbconnect.getBalance(input);
            }
            

                catch (Exception ex)
	            {
	                MessageBox.Show(ex.Message);
	            }
	            finally
	            {
	                mdr.Close();
	                mcon.Close();
	            }
            
	            MessageBox.Show("Recharge Successful");
	
	
	
	        }

       


        private void search(object sender, RoutedEventArgs e)
        {
            //User user = new User();
            
            if (uname_re.Text == "")
            {
                MessageBox.Show("Please provide Username");

        uname_re.Focus();
        return;
            }

            try
            {
                //to enable the recharge button and amount textbox
                Recharge_Amt_re.IsReadOnly = false;
                button1_re.IsEnabled = true;
                txt_charge_per_hour.Text = Convert.ToString(50);
                txt_charge_per_hour.IsEnabled = false;
                txt_playing_hours_remaining.IsEnabled = false;


                string input = uname_re.Text.Trim();
                mcon.Open();
                s = "select * from user where UserName=" + "'" + input + "'";
                mcd = new MySqlCommand(s, mcon);
                mdr = mcd.ExecuteReader();
                if (mdr.Read())
                {
                    fname_re.Text = mdr.GetString("FirstName");
                    lname_re.Text = mdr.GetString("LastName");
                    tb_email_re.Text = mdr.GetString("Email1");
                    tb_cell_re.Text = mdr.GetString("Mobile1");
                    WalletId = mdr.GetString("WalletId");

                    txt_Balance.Text = Dbconnect.getBalance(WalletId);

                }
                else
                {

                    Recharge_Amt_re.IsReadOnly = true;
                    button1_re.IsEnabled = false;
                    MessageBox.Show("NO DATA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mdr.Close();
                mcon.Close();
            }

        }

        
        private void dash_tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Country_txt.Text = "INDIA";
            Country_txt.IsEnabled = false;
            String Username = null, Password1 = null,Password2=null, Fname = null, Lname = null, Pemail = null, Pmobile = null, Add1 = null, Add2 = null, City = null, State = null, Pincode = null, Country = null;
            Username = uname_re1.Text.ToString();
            Password1 = Passwrd1.Password.ToString();
            Password2 = Passwrd2.Password.ToString();
            Fname = Fname_txt.Text.ToString();
            Lname = Lname_txt.Text.ToString();
            Pemail = Email_txt.Text.ToString();
            Pmobile = Mobile_txt.Text.ToString();

            Add1 = Add1_txt.Text.ToString();
            if (!Add2_txt.Text.ToString().Equals(""))
            {
                Add2 = Add2_txt.Text.ToString();
            }

            City = City_txt.Text.ToString();
            State = State_txt.Text.ToString();
            Country = Country_txt.Text.ToString();
            Pincode = Pincode_txt.Text.ToString();



            //Check Constraints.
            int flag = 1;

            while (flag == 1)
            {



                if (Username == "" || Password1 == "" || Password2 == "" || Fname == "" || Lname == "" || Pemail == "" || Pmobile == "" || Add1 == "" || City == "" || State == "" || Pincode == "" || Country == "")
                {
                    MessageBox.Show("Please Enter details in all the *(Mandatory) fields");
                    return;
                }
                if (Regex.IsMatch(Username, @"^[a-zA-Z0-9_]{5,20}$") == false)
                {
                    MessageBox.Show("Enter correct Username");
                    return;
                }

                if (!Password1.Equals(Password2))
                {
                    MessageBox.Show("Passwords not matching");
                    return;
                }

                if (Password1.Length < 5)
                {
                    MessageBox.Show("Password length is short (Length should be more than 5 character)");
                    return;
                }

                if (Regex.IsMatch(Fname, @"^[a-zA-Z]+$") == false)
                {
                    MessageBox.Show("Enter Correct First Name");
                    return;
                }

                if (Regex.IsMatch(Lname, @"^[a-zA-Z]+$") == false)
                {
                    MessageBox.Show("Enter Correct Last Name");
                    return;
                }



                try
                {
                    if (Regex.IsMatch(Pmobile, @"^([7-9]{1})([0-9]{9})$") == false)
                    {
                        MessageBox.Show("Enter valid 10 digits Mobile Number");
                        return;
                    }
                }
                catch (System.NullReferenceException)
                {
                    MessageBox.Show("Enter valid 10 digits Mobile Number");

                }

                if (Regex.IsMatch(Pemail, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
                {
                    MessageBox.Show("Enter valid Email Id Email");
                    return;
                }

                if (Regex.IsMatch(Pincode, @"^[1-9][0-9]{5}$") == false)
                {
                    MessageBox.Show("Enter correct Pin");
                    return;
                }

                if (Regex.IsMatch(City, @"^[a-zA-Z]+$") == false)
                {
                    MessageBox.Show("Enter Correct city");
                    return;
                }

                if (Regex.IsMatch(State, @"^[a-zA-Z]+$") == false)
                {
                    MessageBox.Show("Enter Correct State");
                    return;
                }

                //Set true value for loop termination
                flag = 0;
            }

            if (Dbconnect.CheckUsername(Username))
            {
                MessageBox.Show("Username Exists");
                return;
            }
            if (Dbconnect.CheckEmail1(Pemail))
            {
                MessageBox.Show("Email Exists");
                return;
            }

            if (Dbconnect.CheckMobile(Pmobile))
            {
                MessageBox.Show("Mobile number Exists");
                return;
            }

        
        User user = new User();
            user.Username = Username;
            user.Password = Password1;
            user.Fname = Fname;
            user.Lname = Lname;
            user.Email = Pemail;
            user.mobile = Pmobile;
            user.Add1 = Add1;
            user.Add2 = Add2;
            user.City = City;
            user.State = State;
            user.Pincode = Pincode;
            user.Country = Country;
            if (Dbconnect.Register(user))
            {
                MessageBox.Show("User registered");
               
            }


        }

        private void list_pc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void computerlist_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {

            mac.Text = GetMacAddress(ip.Text.ToString());
           
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(info.Document.ContentStart,
        info.Document.ContentEnd);

            if (ip.Text.Equals(""))
            {
                MessageBox.Show("Ip address not present");
                return;
            }
            if (mac.Text.Equals(""))
            {
                MessageBox.Show("MAC Address not present");
                return;
            }
            if (Workstation_Name.Equals(""))
            {
                MessageBox.Show("Please provide PC name");
                return;
            }
            if(info.Equals("") && textRange.Text.Length<50)
            {
                MessageBox.Show("Please provide info Max length should be 50 character");
                return;
            }

            //check if ip is already present in database
            if (Dbconnect.IsIPPresent(ip.Text))
            {
                MessageBox.Show("Pc already added");
                return;
            }
          else
            {

                if (Dbconnect.AddPc(ip.Text, mac.Text, Workstation_Name.Text, textRange.Text))
                    MessageBox.Show("Pc Added");
                else
                    MessageBox.Show("Unable to add PC");
                               
            }
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            dispTimer.Stop();
            this.Close();
            log.Show();
        }

        public string GetMacAddress(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "not found";
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            uname_re.Text = string.Empty;
            fname_re.Text = string.Empty;
            lname_re.Text = string.Empty;
            tb_email_re.Text = string.Empty;
            tb_cell_re.Text = string.Empty;
            Recharge_Amt_re.Text = string.Empty;
            txt_Balance.Text = string.Empty;
            Recharge_Amt_re.IsReadOnly = true;
            button1_re.IsEnabled = false;

        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void getdata(object sender, RoutedEventArgs e)
        {
            
            MySqlDataReader reader;
            reader = Dbconnect.getPcDetails();
            if (reader.HasRows)
            {
               
                DataTable dt = new DataTable();
                dt.Load(reader);
                //DataGridView
            }
        }

        private void Add_hours(object sender, RoutedEventArgs e)
        {
            var regex = new Regex(@"^*[0-9,\.]+$");
            if(!regex.IsMatch(txt_playing_hours_Add.Text))
            {
                MessageBox.Show("Enter valid playing hours");
                return;
            }
            int charge = Convert.ToInt32(txt_charge_per_hour.Text);
            int hours = Convert.ToInt32(txt_playing_hours_Add.Text);
            int total = charge * hours;
            int balance = Convert.ToInt32(txt_Balance.Text);
            if (total < balance)
            {
                MessageBox.Show("Insufficient account balance");
                return;
            }
            else
                balance = balance - total;


        }
    }

    public class User
    {
        public String UserId;
        public String Username;
        public String Password;
        public String Fname;
        public String Lname;
        public String Email;
        public String mobile;
        public String Add1;
        public String Add2;
        public String City;
        public String State;
        public String Pincode;
        public String Country;
        public String WalletId;
    }
}

    
    
 
    


