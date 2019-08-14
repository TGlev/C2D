using Npoi.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models
{
    public class Contact
    {
        public string Id { get; set; }
        public RecordType RecordType { get; set; }
        [Column("company_name")]
        public string CompanyName { get; set; }
        public string Gender { get; set; }
        [Column("display_name")]
        public string FirstName { get; set; }
        public string Preposition { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberExtra { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Column("office_number")]
        public string PhoneNumberOne { get; set; }
        public string PhoneNumberOneDescription { get; set; }
        public string PhoneNumberTwo { get; set; }
        public string PhoneNumberTwoDescription { get; set; }
        public string PhoneNumberThree { get; set; }
        public string PhoneNumberThreeDescription { get; set; }
        public string FaxOne { get; set; }
        public string FaxOneDescription { get; set; }
        [Column("mobile_number")]
        public string MobilePhoneOne { get; set; }
        public string MobilePhoneOneDescription { get; set; }
        public string Website { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
