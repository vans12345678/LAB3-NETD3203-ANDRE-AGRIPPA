/*
 * Name: Andre Agrippa
 * Date: 11/05/2020
 * Course: NETD 3202
 * Purpose: To create share entries, view summary and view entries
 * File: MainWindow.xaml.cs
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAB3_NETD3203_ANDRE_AGRIPPA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Initialize MainWindow contents
            InitializeComponent();
            ViewSummary();
        }

        private void btnCreateEntry_Click(object sender, RoutedEventArgs e)
        {
            //Gets date from date picker
            string date = dtpDateSelected.SelectedDate.ToString();

            //If buyer name is not empty
            if (txtBuyerName.Text != string.Empty)
            {
                //If number of shares is not empty and can be parsed into integer 
                if (txtNumberOfShares.Text != string.Empty && int.TryParse(txtNumberOfShares.Text, out int shares))
                {
                    //Chose share type based on radio button selection
                    string selectedShare;
                    if (radPreferred.IsChecked == true)
                    {
                        selectedShare = "Preferred";
                    }
                    else
                    {
                        selectedShare = "Common";
                    }
                    try
                    {
                        //Connect to db
                        string connectString = Properties.Settings.Default.connect_string;
                        SqlConnection conn = new SqlConnection(connectString);
                        conn.Open();

                        //Select number of shares based on if common or preferred
                        string selectionQuery = "";
                        if (selectedShare == "Common")
                        {
                            selectionQuery = "SELECT numCommonShares FROM NumberOfShares";
                        }
                        else
                        {
                            selectionQuery = "SELECT numPreferredShares FROM NumberOfShares";
                        }

                        //Change sum of shares after user bought shares
                        SqlCommand secondCommand = new SqlCommand(selectionQuery, conn);
                        int availableShares = Convert.ToInt32(secondCommand.ExecuteScalar());
                        availableShares -= shares;

                        //If buyer tries to buy more shares than available
                        if (availableShares < 0)
                        {
                            MessageBox.Show("Sorry, not enough shares available.");
                            txtNumberOfShares.SelectAll();
                            txtNumberOfShares.Focus();

                        }
                        
                        else
                        {
                             //Insert user record in CreateEntry db
                             string insert = "INSERT INTO [CreateEntry](buyerName, shares, datePurchased, shareType) VALUES('" + txtBuyerName.Text + "', '" + shares + "', '" +
                                        date + "', '" + selectedShare + "')";

                            //Execute command
                            SqlCommand command = new SqlCommand(insert, conn);
                            command.ExecuteNonQuery();
                            string updateQuery = "";

                            //Update number of common or preferred shares
                            if (selectedShare == "Common")
                            {
                                updateQuery = "UPDATE NumberOfShares SET numCommonShares = '" + availableShares + "' ";
                                SqlCommand sqlUpdateShareCommand = new SqlCommand(updateQuery, conn);
                                sqlUpdateShareCommand.ExecuteScalar();
                            }
                            else
                            {
                                updateQuery = "UPDATE NumberOfShares SET numPreferredShares = '" + availableShares + "' ";
                                SqlCommand sqlUpdateShareCommand = new SqlCommand(updateQuery, conn);
                                sqlUpdateShareCommand.ExecuteScalar();
                            } 
                            MessageBox.Show("Successfully added equipment entry");
                        }
                        
                        //Close db connection
                        conn.Close();

                        //reset all fields
                    }
                    //Display exception in message box
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                        throw;
                    }
                }
                else
                {
                    //Number of shares is empty
                    MessageBox.Show("Number of shares must be a non-empty numerical value");
                    txtNumberOfShares.SelectAll();
                    txtNumberOfShares.Focus();
                }
            }
            else
            {
                //buyer name is empty
                MessageBox.Show("Buyer name cannot be empty");
                txtBuyerName.SelectAll();
                txtNumberOfShares.Focus();
            }
        }
        
        private void ViewSummary()
        {
            try
            {
                /*
                 * SQL Commands for displaying # of common shares sold, # of preferred shares sold, revenue generated,
                 * common shares available and preferred shares available. 
                 */
                
                string sqlCommonSharesSoldQuery = "SELECT SUM (shares) FROM CreateEntry WHERE shareType ='Common'";
                string sqlPreferredSharesSoldQuery = "SELECT SUM (shares) FROM CreateEntry WHERE shareType ='Preferred'";
                string sqlCommonSharesAvailable = "SELECT numCommonShares FROM NumberOfShares";
                string sqlPreferredSharesAvailable = "SELECT numPreferredShares FROM NumberOfShares";
                string sqlRevenueGenerated = "SELECT datePurchased, shares, shareType FROM CreateEntry";

                //Connect to db
                string connectString = Properties.Settings.Default.connect_string;
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();

                //Run the sql commands
                SqlCommand commonSharesSoldSqlCommand = new SqlCommand(sqlCommonSharesSoldQuery, conn);
                SqlCommand preferredSharesSoldSqlCommand = new SqlCommand(sqlPreferredSharesSoldQuery, conn);
                SqlCommand commonSharesAvailableCommand = new SqlCommand(sqlCommonSharesAvailable, conn);
                SqlCommand preferredSharesAvailableCommand = new SqlCommand(sqlPreferredSharesAvailable, conn);
                SqlCommand revenueGeneratedCommand = new SqlCommand(sqlRevenueGenerated, conn);

                //Get values
                int soldCommonShares = Convert.ToInt32(commonSharesSoldSqlCommand.ExecuteScalar());
                int soldPreferredShares = Convert.ToInt32(preferredSharesSoldSqlCommand.ExecuteScalar());
                int commonAvailable = Convert.ToInt32(commonSharesAvailableCommand.ExecuteScalar());
                int preferredAvailable = Convert.ToInt32(preferredSharesAvailableCommand.ExecuteScalar());
                

                //Create datatable and display on datagrid
                SqlDataAdapter sda = new SqlDataAdapter(revenueGeneratedCommand);
                DataTable dt = new DataTable("DateTable");
                sda.Fill(dt);

                //Total revenue for all shares
                int totalRevenue = 0;

                //For each row found in Data Table
                foreach (DataRow row in dt.Rows)
                {
                    //Get total amount of days from beginning date
                    DateTime selectedDate = DateTime.Parse(row["datePurchased"].ToString());
                    int day = Convert.ToInt32((selectedDate - new DateTime(1990, 1, 1)).TotalDays);
                    Random rnd = new Random(day);
                    int id = 0;

                    //Gets the price per share depending if it's common or preferred
                    if (row["shareType"].ToString() == "Common")
                    {
                        id = rnd.Next(1, 50);
                    }
                    else
                    { 
                        id = rnd.Next(1, 200);
                    }
                   
                    //Calculate total revenue
                    totalRevenue += int.Parse(row["shares"].ToString()) * id;
                }

                //Output to text blocks
                txtCommonSharesAvailable.Text = commonAvailable.ToString();
                txtCommonSharesSold.Text = soldCommonShares.ToString();
                txtPreferredSharesSold.Text = soldPreferredShares.ToString();
                txtPreferredSharesAvailable.Text = preferredAvailable.ToString();
                txtRevenueGenerated.Text = totalRevenue.ToString();

            }
            //Catch any exceptions and display in message box
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }
        }
        private void tiViewEntries_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //Connect to db
                string connectString = Properties.Settings.Default.connect_string;
                SqlConnection conn = new SqlConnection(connectString);
                conn.Open();

                //sql query to select all fields from create entry
                string selectAll = "SELECT * FROM CreateEntry";
                SqlCommand command = new SqlCommand(selectAll, conn);

                //Create datatable and display on datagrid
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Entries");

                sda.Fill(dt);

                viewEntriesGrid.ItemsSource = dt.DefaultView;
            }
            //If contents cannot be displayed, display exception
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
