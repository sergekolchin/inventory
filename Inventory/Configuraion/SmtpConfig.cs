using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuraion
{
    public class SmtpConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public bool UseSsl { get; set; }
    }
}
