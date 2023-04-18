using CareerExplorer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        IEnumerable<Country> GetFirstCountries(string search, int maxAmount = 5);
        IEnumerable<City> GetFirstCitiesOfCountry(int? countryId, string search, int maxAmount = 5);
    }
}
