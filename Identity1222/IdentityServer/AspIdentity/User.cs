using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.AspIdentity
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Siren { get; set; }
    }
}
