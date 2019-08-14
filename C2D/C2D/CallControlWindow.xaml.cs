using C2D.Shared.Models;
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

namespace C2D
{
    /// <summary>
    /// Interaction logic for CallControlWindow.xaml
    /// </summary>
    public partial class CallControlWindow : Window
    {

        private Call Call;
        private Contact Contact;

        private bool IsOnHold;

        public CallControlWindow(Call call)
        {
            InitializeComponent();

            if (call == null)
                return;

            this.Call = call;

            StartTimer();
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        public void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private async void LoadData()
        {
            //lbl_Company.Content = Contact.CompanyName;
            //lbl_Phone.Content = Call.ToNumber;
            if (Call.Direction == "inbound")
            {
                Contact = await NetworkManager.GetContactByNumber(Call.CallerId);
                lbl_Phone.Content = Call.CallerId;
                if (Contact == null)
                {
                    lbl_Company.Content = "Onbekend bedrijf";
                    lbl_Contact.Content = "Onbekende contactpersoon";
                    btn_Add.Visibility = Visibility.Visible;
                    return;
                }
                btn_Add.Visibility = Visibility.Hidden;
                lbl_Company.Content = Contact.CompanyName;
                lbl_Contact.Content = Contact.FirstName + " " + Contact.LastName;
            }
            else
            {
                Contact = await NetworkManager.GetContactByNumber(Call.ToNumber);
                lbl_Phone.Content = Call.ToNumber;
                if (Contact == null)
                {
                    lbl_Company.Content = "Onbekend bedrijf";
                    lbl_Contact.Content = "Onbekende contactpersoon";
                    btn_Add.Visibility = Visibility.Visible;
                    return;
                }
                btn_Add.Visibility = Visibility.Hidden;
                lbl_Company.Content = Contact.CompanyName;
                lbl_Contact.Content = Contact.FirstName + " " + Contact.LastName;
            }
        }

        private async void btn_Cancel(object sender, RoutedEventArgs e)
        {
            await NetworkManager.CancelCall(Call.CallId);
        }

        private async void btn_OnHold(object sender, RoutedEventArgs e)
        {
            if (IsOnHold)
                await NetworkManager.ResumeCall(Call.CallId);
            else
                await NetworkManager.PauseCall(Call.CallId);

            IsOnHold = !IsOnHold;
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if(Call.Direction == "inbound")
                new ContactEditWindow(Call.CallerId).Show();
            else
                new ContactEditWindow(Call.ToNumber).Show();
        }

        private async void Btn_Terminate_Click(object sender, RoutedEventArgs e)
        {
            await NetworkManager.ForceTerminateCall(Call);
            this.Close();
            App.ActiveCall = null;
            App.CallControlWindow = null;
        }
    }
}
