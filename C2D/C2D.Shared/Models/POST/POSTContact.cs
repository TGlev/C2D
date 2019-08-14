using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public class POSTContact : Contact, POSTBase
    {

        public POSTContact(Contact contact, string client_id, string access_token, string action)
        {
            if (contact == null)
                return;
            record_id = contact.Id ?? "";
            record_type = contact.RecordType.ToString();
            company_name = contact.CompanyName;
            gender = contact.Gender;
            firstname = contact.FirstName;
            preposition = contact.Preposition;
            lastname = contact.LastName;
            streetname = contact.StreetName;
            housenumber = contact.HouseNumber;
            housenumber_ext = contact.HouseNumberExtra;
            zipcode = contact.Zipcode;
            city = contact.City;
            country = contact.Country;
            phone_1 = contact.PhoneNumberOne;
            phone_1_desc = contact.PhoneNumberOneDescription;
            phone_2 = contact.PhoneNumberTwo;
            phone_2_desc = contact.PhoneNumberTwoDescription;
            phone_3 = contact.PhoneNumberThree;
            phone_3_desc = contact.PhoneNumberThreeDescription;
            fax_1 = contact.FaxOne;
            fax_1_desc = contact.FaxOneDescription;
            mobile_1 = contact.MobilePhoneOne;
            mobile_1_desc = contact.MobilePhoneOneDescription;
            website = contact.Website;
            email = contact.Email;
            notes = contact.Email;

            this.client_id = client_id;
            this.access_token = access_token;
            this.action = action;
        }

        public string record_id
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
            }
        }
        public string record_type
        {
            get
            {
                return RecordType.ToString();
            }
            set
            {
                RecordType = (RecordType)Enum.Parse(RecordType.GetType(), value.ToUpper());
            }
        }
        public string company_name
        {
            get
            {
                return CompanyName;
            }
            set
            {
                CompanyName = value;
            }
        }
        public string gender
        {
            get
            {
                return Gender;
            }
            set
            {
                Gender = value;
            }
        }
        public string firstname
        {
            get
            {
                return FirstName;
            }
            set
            {
                FirstName = value;
            }
        }
        public string preposition
        {
            get
            {
                return Preposition;
            }
            set
            {
                Preposition = value;
            }
        }
        public string lastname
        {
            get
            {
                return LastName;
            }
            set
            {
                LastName = value;
            }
        }
        public string streetname
        {
            get
            {
                return StreetName;
            }
            set
            {
                StreetName = value;
            }
        }
        public string housenumber
        {
            get
            {
                return HouseNumber;
            }
            set
            {
                HouseNumber = value;
            }
        }
        public string housenumber_ext
        {
            get
            {
                return HouseNumberExtra;
            }
            set
            {
                HouseNumberExtra = value;
            }
        }
        public string zipcode
        {
            get
            {
                return Zipcode;
            }
            set
            {
                Zipcode = value;
            }
        }
        public string city
        {
            get
            {
                return City;
            }
            set
            {
                City = value;
            }
        }
        public string country
        {
            get
            {
                return Country;
            }
            set
            {
                Country = value;
            }
        }
        public string phone_1
        {
            get
            {
                return PhoneNumberOne;
            }
            set
            {
                PhoneNumberOne = value;
            }
        }
        public string phone_1_desc
        {
            get
            {
                return PhoneNumberOneDescription;
            }
            set
            {
                PhoneNumberOneDescription = value;
            }
        }
        public string phone_2
        {
            get
            {
                return PhoneNumberTwo;
            }
            set
            {
                PhoneNumberTwo = value;
            }
        }
        public string phone_2_desc
        {
            get
            {
                return PhoneNumberTwoDescription;
            }
            set
            {
                PhoneNumberTwoDescription = value;
            }
        }
        public string phone_3
        {
            get
            {
                return PhoneNumberThree;
            }
            set
            {
                PhoneNumberThree = value;
            }
        }
        public string phone_3_desc
        {
            get
            {
                return PhoneNumberThreeDescription;
            }
            set
            {
                PhoneNumberThreeDescription = value;
            }
        }
        public string fax_1
        {
            get
            {
                return FaxOne;
            }
            set
            {
                FaxOne = value;
            }
        }
        public string fax_1_desc
        {
            get
            {
                return FaxOneDescription;
            }
            set
            {
                FaxOneDescription = value;
            }
        }
        public string mobile_1
        {
            get
            {
                return MobilePhoneOne;
            }
            set
            {
                MobilePhoneOne = value;
            }
        }
        public string mobile_1_desc
        {
            get
            {
                return MobilePhoneOneDescription;
            }
            set
            {
                MobilePhoneOneDescription = value;
            }
        }
        public string website
        {
            get
            {
                return Website;
            }
            set
            {
                Website = value;
            }
        }
        public string email
        {
            get
            {
                return Email;
            }
            set
            {
                Email = value;
            }
        }
        public string notes
        {
            get
            {
                return Notes;
            }
            set
            {
                Notes = value;
            }
        }

        public string client_id { get; set; }
        public string access_token { get; set; }
        public string action { get; set; }
    }
}
