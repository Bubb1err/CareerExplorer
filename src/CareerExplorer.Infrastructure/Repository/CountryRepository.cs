using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;

namespace CareerExplorer.Infrastructure.Repository
{
    public class CountryRepository :Repository<Country>, ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(AppDbContext db) : base(db)
        {
            _context = db;
        }
        public IEnumerable<Country> GetFirstCountries(string search, int maxAmount = 5)
        {
            if(string.IsNullOrEmpty(search))
            {
                return _context.Countries.Take(maxAmount);
            }
            var countries = _context.Countries
                .Where(x => x.Name.ToLower().Contains(search.ToLower()))
                .Take(maxAmount);
            return countries;
        }
       public IEnumerable<City> GetFirstCitiesOfCountry(int? countryId, string search, int maxAmount = 5)
       {
            if(countryId == null || countryId == 0)
            {
                return _context.Cities.Take(maxAmount);
            }
            if (string.IsNullOrEmpty(search))
            {              
                return _context.Cities.Where(x => x.CountryId == countryId).Take(maxAmount);
            }
            var cities = _context.Cities
                .Where(x => x.CountryId == countryId && x.Name.ToLower().Contains(search.ToLower()))
                .Take(maxAmount);
            return cities;
        }
    }
}
