using System;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}