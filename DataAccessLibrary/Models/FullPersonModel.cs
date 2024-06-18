using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class FullPersonModel
    {
        public PersonModel BasicPerson { get; set; }
        public List<AddressModel> Address { get; set; } = new List<AddressModel>();
        public List<CompanyModel> Company { get; set; } = new List<CompanyModel>();
    }
}
