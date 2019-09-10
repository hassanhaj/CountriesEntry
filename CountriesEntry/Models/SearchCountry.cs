using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountriesEntry.Models
{
    public class SearchCountry
    {
        public IEnumerable<CountriesEntry.Entities.Country> Countries { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
