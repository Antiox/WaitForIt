using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFIWLib.Models
{
    class UserCreationObject : UserLoginObject
    {
        [JsonProperty("email")]
        public string MailAddress { get; set; }

        public UserCreationObject(string username, string password, string mail)
          : base(username, password)
        {
            MailAddress = mail;
        }
    }
}
