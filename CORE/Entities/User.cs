using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        // Password for login (it is recommended to never store passwords in plain text)
        public string Password { get; set; }
    }
}
