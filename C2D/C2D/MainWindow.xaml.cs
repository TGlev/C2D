using C2D.Shared;
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
using C2D.Shared.Models;

namespace C2D
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ((App)Application.Current).MainWindow = this;

            if(App.UserSettings != null)
            {           
                txt_Username.Text = App.UserSettings.Username;
                txt_Password.Password = App.UserSettings.Password;
                txt_ID.Password = App.UserSettings.ClientID;
                txt_Secret.Password = App.UserSettings.ClientSecret;
                txt_Extension.Text = App.UserSettings.Extension;
            }
        }

        private async void Next_Clicked(object sender, RoutedEventArgs e)
        {
            if(!Utils.HasText(txt_ID) || !Utils.HasText(txt_Secret) || !Utils.HasText(txt_Username) || !Utils.HasText(txt_Extension))
            {
                MessageBox.Show("Alle velden dienen ingevuld te worden.");
                return;
            }
            
            ToggleAllFields(false);

            var User = new User
            {
                Username = txt_Username.Text,
                Password = txt_Password.Password,
                ClientID = txt_ID.Password,
                ClientSecret = txt_Secret.Password,
                Extension = txt_Extension.Text
            };

            var response = await NetworkManager.CheckLogin(User);
            ToggleAllFields(true);

            if(response == null || response.access_token == null)
            {
                MessageBox.Show("Onjuiste waarden! Probeer het opnieuw.");
                return;
            }

            var UserSettings = App.UserSettings;
            if (UserSettings == null)
                UserSettings = new User();

            UserSettings.Username = txt_Username.Text;
            UserSettings.Password = txt_Password.Password;
            UserSettings.ClientID = txt_ID.Password;
            UserSettings.ClientSecret = txt_Secret.Password;
            UserSettings.Extension = txt_Extension.Text;
            UserSettings.AccessToken = response.access_token;
            UserSettings.RefreshToken = response.refresh_token;
            App.DatabaseManager.SaveUser(UserSettings);

            var page = new ContactListWindow();
            page.Show();
            App.IsLoggedIn = true;

            ((App)Application.Current).CreateContextMenu();

            this.Close();
            ((App)Application.Current).MainWindow = page;
        }

        private void ToggleAllFields(bool value)
        {
            txt_ID.IsEnabled = value;
            txt_Secret.IsEnabled = value;
            txt_Username.IsEnabled = value;
            txt_Password.IsEnabled = value;
            txt_Extension.IsEnabled = value;
            btn_Next.IsEnabled = value;
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
