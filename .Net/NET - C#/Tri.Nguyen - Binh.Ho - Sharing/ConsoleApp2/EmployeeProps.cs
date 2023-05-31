using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partial
{
    public partial class Student
    {
        private string _firstName;
        private string _lastName;
        partial void OnSettingFirstName(string value);
        partial void OnSettingLastName(string value);
        public int Id { get; set; }
        public string FirstName
        {
            get => _firstName;
            set
            {
                OnSettingFirstName(value);
                _firstName = value;
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                OnSettingLastName(value);
                _lastName = value;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public string Major { get; set; }
        public string Specialization { get; set; }
    }
}
