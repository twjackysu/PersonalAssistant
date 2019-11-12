using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalAssistant.Models
{
    public class PasswordInfo
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }
        public string Notes { get; set; }
    }
}
