using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBus.Queue.Sender
{
    public class Person
    {
        public int Id { get; set; }
        public string ?FirstName { get; set; }

        public string ?LastName { get; set; }

        public string ?Email { get; set; }

        public string ?Country { get; set; }

        public string ?Modified { get; set; }

        public bool Vip { get; set; }
    }
}

