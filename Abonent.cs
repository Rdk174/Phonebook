using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook
{
    class Abonent
    {
        private string name;
        private string phoneNumber;

        public string Name
        {
            set
            {
                if(!string.IsNullOrEmpty(value))
                    name = value.Trim();
            }
            get
            {
                return name;
            }
        }
        public string PhoneNumber
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    phoneNumber = value.Trim();
            }
            get
            {
                return phoneNumber;
            }
        }
        public Abonent(string name, string phoneNumber)
        {
            this.name = name;
            this.phoneNumber = phoneNumber;
        }

    }
}
