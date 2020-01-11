using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Organization { get; set; }

        public string Role { get; set; }

        public List<ContactTag> Tags { get; set; } 

        public List<Note> Notes { get; set; }
    }
}
