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



namespace Creds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyButton.IsEnabled = false;
            Creds.TestCredentials.DSServicesClient client;
            if(this.RegionBox.Text == "OC")
            {
                client = new Creds.TestCredentials.DSServicesClient("BasicHttpBinding_DSServices_OC");
            }
            else if(this.RegionBox.Text == "OC-Test")
            {
                client = new Creds.TestCredentials.DSServicesClient("BasicHttpBinding_DSServices_OCTest");
            }
            else if(this.RegionBox.Text == "US")
            {
                client = new Creds.TestCredentials.DSServicesClient("BasicHttpBinding_DSServices_US");
            }
            else {
                //OMG CREDS
                client = new Creds.TestCredentials.DSServicesClient("OMG creds");
            }
                     
            var user = new Creds.TestCredentials.User();
            user.Username = Convert.ToString(this.UserNameText.Text);
            user.Password = Convert.ToString(this.PasswordText.Text);
            var cp = Convert.ToString(this.ContractPrefixText.Text);

            
            try
            {
                //instantiation before sending
                Creds.TestCredentials.RequestGetPaymentHistoryByStatusForDateRange apiCall = new Creds.TestCredentials.RequestGetPaymentHistoryByStatusForDateRange();
                apiCall.User = user;
                apiCall.StartDate = new DateTime(2018, 08, 15);
                apiCall.EndDate = new DateTime(2018, 08, 17);

                var resp = "";
                if (this.RegionBox.Text == "US")
                {
                    var response = await client.GetPaymentHistoryByStatusForDateRangeAsync(apiCall);
                    resp = response.ResponseNotes[0].Note.ToString();

                }
                else if(this.RegionBox.Text == "OC-Test")
                {
                    var response = await client.GetPaymentHistoryByStatusForDateRangeAsync(apiCall);
                    resp = response.ResponseNotes[0].Note.ToString();
                }
                else
                {
                    apiCall.ContractPrefix = cp;
                    var response = await client.GetPaymentHistoryByStatusForDateRangeAsync(apiCall);
                    resp = response.ResponseNotes[0].Note.ToString();
                }
                MessageBox.Show(resp);
                ApplyButton.IsEnabled = true;
            }
            catch
            {
                
            }

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.UserNameText.Text = "";
            this.PasswordText.Text = "";
            this.ContractPrefixText.Text = "";
        }
    }
}
