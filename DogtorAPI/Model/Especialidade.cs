using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace DogtorAPI.Model
{
    public class Especialidade
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid VeterinarioID { get; set; }
        protected Especialidade()
        {

        }

        public Especialidade(string name, Guid veterinarioID)
        {
            Id = Guid.NewGuid();
            Name = name;
            VeterinarioID = veterinarioID;
        }
    }


}
