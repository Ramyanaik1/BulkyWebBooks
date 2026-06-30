using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Models.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public string? Name { get; set; }
        [Display(Name = "Address")]
        public string? StreetAddress { get; set; }
        [Display(Name = "City")]
        public string? City { get; set; }
        [Display(Name = "State")]
        public string? State { get; set; }
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
