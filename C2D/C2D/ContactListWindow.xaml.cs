using C2D.Shared;
using C2D.Shared.Models;
using Npoi.Mapper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Office.Interop;

namespace C2D
{
    /// <summary>
    /// Interaction logic for ContactListWindow.xaml
    /// </summary>
    public partial class ContactListWindow : Window
    {

        public ContactListWindow()
        {
            InitializeComponent();

            FillGrid();
        }

        public async Task FillGrid()
        {
            string query = txt_Search.Text;
            while (query.StartsWith("0"))
                query = query.Remove(0, 1);
            while (query.StartsWith("31"))
                query = query.Remove(0, 2);
            if (query == null || query == "")
                list_Contacts.ItemsSource = (await NetworkManager.GetContacts()).OrderBy(contact => contact.FirstName).ToList();
            else
                list_Contacts.ItemsSource = (await NetworkManager.GetContacts()).FindAll(
                    x => x.City.ContainsNoCase(query)
                    || x.CompanyName.ContainsNoCase(query)
                    || x.FirstName.ContainsNoCase(query)
                    || x.LastName.ContainsNoCase(query)
                    || x.StreetName.ContainsNoCase(query)
                    || x.Zipcode.ContainsNoCase(query)
                    || x.City.ContainsNoCase(query)
                    || x.PhoneNumberOne.ContainsNoCase(query)
                    || x.PhoneNumberTwo.ContainsNoCase(query)
                    || x.PhoneNumberThree.ContainsNoCase(query)
                    || x.FaxOne.ContainsNoCase(query)
                    || x.MobilePhoneOne.ContainsNoCase(query)
                    || x.Website.ContainsNoCase(query)
                    || x.Email.ContainsNoCase(query)).OrderBy(contact => contact.FirstName).ToList();
        }

       private async void ImportFromExcel()
        {
            IWorkbook workbook;
            using (FileStream file = new FileStream("C:\\Users\\Luca\\Desktop\\Contacten.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = WorkbookFactory.Create(file);
            }
            var importer = new Mapper(workbook);
            var items = importer.Take<Contact>();
            foreach(var item in items)
            {
                await NetworkManager.CreateNewContact(item.Value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedContact = list_Contacts.SelectedItem as Contact;
            if(selectedContact == null)
            {
                MessageBox.Show("Selecteer eerst een contact aub.");
                return;
            }
            if(selectedContact.Email == null || selectedContact.Email == "")
            {
                MessageBox.Show("Er is geen mailadres bekend bij dit contactpersoon!");
                return;
            }
            Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook._MailItem oMailItem = (Microsoft.Office.Interop.Outlook._MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            oMailItem.To = selectedContact.Email;
            oMailItem.Display(true);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedContact = list_Contacts.SelectedItem as Contact;
            if (selectedContact == null)
            {
                MessageBox.Show("Selecteer eerst een contact aub.");
                return;
            }
            await NetworkManager.StartCall(selectedContact);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Contact selectedContact = list_Contacts.SelectedItem as Contact;
            if (selectedContact == null)
            {
                MessageBox.Show("Selecteer eerst een contact aub.");
                return;
            }
            new ContactEditWindow(selectedContact, this).ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new ContactEditWindow(null, this).ShowDialog();
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!(MessageBox.Show("Weet je zeker dat je deze contactpersoon wilt verwijderen?", "Let op!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No))
            {
                Contact selectedContact = list_Contacts.SelectedItem as Contact;
                if (selectedContact == null)
                {
                    MessageBox.Show("Selecteer eerst een contact aub.");
                    return;
                }
                await NetworkManager.DeleteContact(selectedContact);
                await FillGrid();
            }
        }

        private void Email_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string email = (sender as TextBlock).Text;
            Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook._MailItem oMailItem = (Microsoft.Office.Interop.Outlook._MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            oMailItem.To = email;
            oMailItem.Display(true);
        }

        private async void Phone_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedContact = list_Contacts.SelectedItem as Contact;
            if (selectedContact == null)
                return;

            await NetworkManager.StartCall(selectedContact);
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await FillGrid();
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await FillGrid();
        }
    }
}
