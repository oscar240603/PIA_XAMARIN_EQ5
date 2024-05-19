using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EV3_EQ5.Models
{
    public class UserAuthentication
    {
        public string nombres { get; set; }

        public string edad {  get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }
    }
}