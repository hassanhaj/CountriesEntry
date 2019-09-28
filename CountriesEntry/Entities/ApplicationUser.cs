using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry.Entities
{
    public class ApplicationUser: IdentityUser<int>
    {
        public int SchoolId { get; set; }
    }
}
