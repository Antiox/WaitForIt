using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFIWLib.Models
{
    class UserLoginObject
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public UserLoginObject(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
