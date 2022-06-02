using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Person
    {
        public Person()
        {
            Clients = new HashSet<Client>();
            Moderators = new HashSet<Moderator>();
            Operators = new HashSet<Operator>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<Operator> Operators { get; set; }
    }
}
