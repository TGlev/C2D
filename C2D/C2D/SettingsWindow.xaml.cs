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
using C2D.Shared.Models;

namespace C2D
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            var settings = App.DatabaseManager.GetUser();
            txt_ID.Password = settings.ClientID;
            txt_Secret.Password = settings.ClientSecret;
            txt_Username.Password = settings.Username;
            txt_Password.Password = settings.Password;
            txt_Extension.Password = settings.Extension;
        }

        private async void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            var User = new User
            {
                Username = txt_Username.Password,
                Password = txt_Password.Password,
                ClientID = txt_ID.Password,
                ClientSecret = txt_Secret.Password,
                Extension = txt_Extension.Password
            };

            var response = await NetworkManager.CheckLogin(User);

            if (response == null || response.access_token == null)
            {
                MessageBox.Show("Onjuiste waarden! Probeer het opnieuw.");
                return;
            }

            var UserSettings = App.UserSettings;
            if (UserSettings == null)
                UserSettings = new User();

            UserSettings.Username = txt_Username.Password;
            UserSettings.Password = txt_Password.Password;
            UserSettings.ClientID = txt_ID.Password;
            UserSettings.ClientSecret = txt_Secret.Password;
            UserSettings.Extension = txt_Extension.Password;
            UserSettings.AccessToken = response.access_token;
            UserSettings.RefreshToken = response.refresh_token;
            App.DatabaseManager.SaveUser(UserSettings);

            MessageBox.Show("Gelukt! Wijzingen opgeslagen.");
            this.Close();
        }
    }
}
