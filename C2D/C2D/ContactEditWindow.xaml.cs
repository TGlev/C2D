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
    /// Interaction logic for ContactEditWindow.xaml
    /// </summary>
    public partial class ContactEditWindow : Window
    {
        Contact ToEdit;
        ContactListWindow Window;
        bool MadeChanges;

        public ContactEditWindow(Contact c, ContactListWindow window)
        {
            InitializeComponent();
            this.ToEdit = c;
            this.Window = window;
            btn_Delete.IsEnabled = false;
            if (ToEdit != null)
                FillFields();
            
            MadeChanges = false;
            btn_Save.IsEnabled = MadeChanges;
        }

        public ContactEditWindow(string phone)
        {
            InitializeComponent();
            txt_PhoneNumberOne.Text = phone;
            btn_Delete.IsEnabled = false;

            MadeChanges = false;
            btn_Save.IsEnabled = MadeChanges;
        }

        private void FillFields()
        {
            txt_CompanyName.Text = ToEdit.CompanyName;
            txt_Gender.Text = ToEdit.Gender;
            txt_FirstName.Text = ToEdit.FirstName;
            txt_Preposition.Text = ToEdit.Preposition;
            txt_LastName.Text = ToEdit.LastName;
            txt_Street.Text = ToEdit.StreetName;
            txt_Number.Text = ToEdit.HouseNumber;
            txt_NumberExtra.Text = ToEdit.HouseNumberExtra;
            txt_Zipcode.Text = ToEdit.Zipcode;
            txt_City.Text = ToEdit.City;
            txt_Country.Text = ToEdit.Country;
            txt_PhoneNumberOne.Text = ToEdit.PhoneNumberOne;
            txt_PhoneNumberOneDescription.Text = ToEdit.PhoneNumberOneDescription;
            txt_PhoneNumberTwo.Text = ToEdit.PhoneNumberTwo;
            txt_PhoneNumberTwoDescription.Text = ToEdit.PhoneNumberTwoDescription;
            txt_PhoneNumberThree.Text = ToEdit.PhoneNumberThree;
            txt_PhoneNumberThreeDescription.Text = ToEdit.PhoneNumberThree;
            txt_Fax.Text = ToEdit.FaxOne;
            txt_FaxDescription.Text = ToEdit.FaxOne;
            txt_MobilePhoneOne.Text = ToEdit.MobilePhoneOne;
            txt_MobilePhoneOneDescription.Text = ToEdit.MobilePhoneOneDescription;
            txt_Website.Text = ToEdit.Website;
            txt_Email.Text = ToEdit.Email;
            txt_Notes.Text = ToEdit.Notes;

            btn_Delete.IsEnabled = true;
        }

        private void Btn_Mail_Click(object sender, RoutedEventArgs e)
        {
            if (ToEdit.Email == null || ToEdit.Email == "")
            {
                MessageBox.Show("Er is geen mailadres bekend bij dit contactpersoon! Vul aub eerst een email adres in en druk op opslaan.");
                return;
            }
            Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook._MailItem oMailItem = (Microsoft.Office.Interop.Outlook._MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            oMailItem.To = ToEdit.Email;
            oMailItem.Display(true);
        }

        private async void Btn_Call_Click(object sender, RoutedEventArgs e)
        {
            await NetworkManager.StartCall(ToEdit);
        }

        private async void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if(txt_CompanyName.Text == "" || txt_PhoneNumberOne.Text == "")
            {
                MessageBox.Show("Vul alstublieft een bedrijfsnaam en telefoonnummer in!");
                return;
            }

            string PhoneNumber = txt_PhoneNumberOne.Text;
            while(PhoneNumber.StartsWith("0"))
                PhoneNumber = PhoneNumber.Remove(0, 1);

            if (PhoneNumber.Length < 10)
            {
                MessageBox.Show("Het telefoonnummer is te kort.");
                return;
            }

            var contact = new Contact
            {
                CompanyName = txt_CompanyName.Text,
                Gender = txt_Gender.Text,
                FirstName = txt_FirstName.Text,
                Preposition = txt_Preposition.Text,
                LastName = txt_LastName.Text,
                StreetName = txt_Street.Text,
                HouseNumber = txt_Number.Text,
                HouseNumberExtra = txt_NumberExtra.Text,
                Zipcode = txt_Zipcode.Text,
                City = txt_City.Text,
                Country = txt_Country.Text,
                PhoneNumberOne = txt_PhoneNumberOne.Text,
                PhoneNumberOneDescription = txt_PhoneNumberOneDescription.Text,
                PhoneNumberTwo = txt_PhoneNumberTwo.Text,
                PhoneNumberTwoDescription = txt_PhoneNumberTwoDescription.Text,
                PhoneNumberThree = txt_PhoneNumberThree.Text,
                PhoneNumberThreeDescription = txt_PhoneNumberThreeDescription.Text,
                FaxOne = txt_Fax.Text,
                FaxOneDescription = txt_FaxDescription.Text,
                MobilePhoneOne = txt_MobilePhoneOne.Text,
                MobilePhoneOneDescription = txt_MobilePhoneOneDescription.Text,
                Website = txt_Website.Text,
                Email = txt_Email.Text,
                Notes = txt_Notes.Text,
                RecordType = RecordType.GLOBAL
            };

            if(ToEdit != null)
            {
                contact.Id = ToEdit.Id;
                await NetworkManager.SaveContact(contact);
                if(Window != null)
                    await Window.FillGrid();
                MadeChanges = false;
                btn_Save.IsEnabled = MadeChanges; 
            }
            else
            {
                this.Close();
                await NetworkManager.CreateNewContact(contact);
                if (Window != null)
                    await Window.FillGrid();
                MadeChanges = false;
                btn_Save.IsEnabled = MadeChanges;
            }
        }

        private async void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!(MessageBox.Show("Weet je zeker dat je deze contactpersoon wilt verwijderen?", "Let op!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No))
            {
                await NetworkManager.DeleteContact(ToEdit);
                this.Close();
                await Window.FillGrid();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MadeChanges = true;
            btn_Save.IsEnabled = MadeChanges;
        }
    }
}
