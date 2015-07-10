using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Import
{
    public class ApplicationArguments
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public int DbID { get; set; }

        public string Instance { get; set; }
    }
}
