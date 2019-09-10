using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry.Entities
{
    public class Country
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(3)]
        public string CountryCode { get; set; }

        [InverseProperty("Country")]
        public virtual ICollection<City> Cities { get; set; }
    }
}
