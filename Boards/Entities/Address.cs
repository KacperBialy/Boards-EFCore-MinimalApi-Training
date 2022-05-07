using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boards.Entities
{
    public class Address
    {
        public Guid Id { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string PostalCode { get; private set; }

        public User User { get; private set; }
        public Guid UserId { get; private set; }
    }
}
