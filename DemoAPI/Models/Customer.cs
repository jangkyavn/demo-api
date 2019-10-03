using System;

namespace DemoAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string BirthDay { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
