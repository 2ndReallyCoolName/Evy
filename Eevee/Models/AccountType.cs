using System.ComponentModel.DataAnnotations;

namespace Eevee.Models
{
    public class AccountType
    {
        public int AccountTypeID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
