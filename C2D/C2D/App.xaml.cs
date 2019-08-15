
using C2D.Properties;
using C2D.Shared.Models;
using C2D.Shared.Models.Responses;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace C2D
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DatabaseManager DatabaseManager { get; set; }
        public static User UserSettings { get; set; }

        public static CallControlWindow CallControlWindow;
        public static Call ActiveCall = null;

        public System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        private static System.Threading.Mutex _mutex = null;

        public static bool IsLoggedIn;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            AppCenter.Start("83c694cd-7de0-4536-a606-e58251e96c26", typeof(Analytics), typeof(Crashes));

            StartTimer();
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            DatabaseManager = new DatabaseManager();
            var Settings = DatabaseManager.GetUser();
            UserSettings = Settings;
            if (App.UserSettings == null)
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            else
            {
                var User = new User
                {
                    Username = Settings.Username,
                    Password = Settings.Password,
                    ClientID = Settings.ClientID,
                    ClientSecret = Settings.ClientSecret,
                    Extension = Settings.Extension
                };

                var response = await NetworkManager.CheckLogin(User);

                if (response == null || response.access_token == null)
                {
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                    return;
                }

                App.UserSettings.AccessToken = response.access_token;
                App.UserSettings.RefreshToken = response.refresh_token;
                App.DatabaseManager.SaveUser(UserSettings);
                MainWindow = new ContactListWindow();
                IsLoggedIn = true;
            }
            UserSettings = DatabaseManager.GetUser();
            MainWindow.Closing += MainWindow_Closing;

            using (var mgr =  UpdateManager.GitHubUpdateManager("https://github.com/TGlev/C2D"))
            {
                var result = await mgr.Result.UpdateApp();
                if(result?.Version.ToString() != null)
                    MessageBox.Show("Er is een update geïnstalleerd! Start de applicatie opnieuw op om deze te activeren.");
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, "C2DHaceel", out bool createdNew);

            if (!createdNew)
                Application.Current.Shutdown();
            else
                Exit += CloseMutexHandler;

            base.OnStartup(e);
            _notifyIcon.Click += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = C2D.Properties.Resources.AppIcon;
            _notifyIcon.Visible = true;
            
            CreateContextMenu();
        }

        protected virtual void CloseMutexHandler(object sender, EventArgs e)
        {
            _mutex?.Close();
        }

        public void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Telefoonboek").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Instellingen").Click += (s, e) => ShowSettingsWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Afsluiten").Click += (s, e) => ExitApplication();
        }

        public void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        public void ShowMainWindow()
        {
            if(!IsLoggedIn)
            {
                new MainWindow().Show();
                return;
            }

            if (MainWindow == null && !IsLoggedIn)
                return;
            else if (MainWindow == null && IsLoggedIn)
                MainWindow = new ContactListWindow();

            if (MainWindow.Title == "Log In")
                MainWindow = new ContactListWindow();
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        public void ShowTestWindow()
        {
            new CallControlWindow(null).Show();
        }

        public void ShowSettingsWindow()
        {
            new SettingsWindow().Show();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

        public static void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private static async void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsLoggedIn)
                return;

            var activeCalls = await NetworkManager.GetActiveCalls() ?? new List<GetCallsResponse>();
            Console.WriteLine(activeCalls.Count);
            if (activeCalls.Count > 0 && ActiveCall == null)
            {
                ActiveCall = activeCalls[0].convert();
                CallControlWindow = new CallControlWindow(ActiveCall);
                CallControlWindow.Show();
            }
            else if (activeCalls.Count == 0 && ActiveCall != null)
            {
                CallControlWindow.Close();
                CallControlWindow = null;
                ActiveCall = null;
            }
        }
    }
}
