using DogtorAPI.ViewModel.Admin;
using DogtorAPI.ViewModel.Pet;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace DogtorAPI.Model
{
    public class Admin
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        protected Admin()
        {

        }

        public Admin(Guid id, string name, string email, string password)
        {
            ID = id;
            Name = name;
            Email = email;
            Password = password;
        }

  
    }

 
}
