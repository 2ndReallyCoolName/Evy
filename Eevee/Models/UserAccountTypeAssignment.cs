using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eevee.Models
{
    public class UserAccountTypeAssignment
    {
        public int UserAccountTypeAssignmentID { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
    }
}
