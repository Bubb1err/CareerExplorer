using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CareerExplorer.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task<CountriesResponse> GetCountriesFromExternalAPIAsync(CancellationToken cancelationToken)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(1);
            try
            {
                var response = await client.GetAsync("https://countriesnow.space/api/v0.1/countries", cancelationToken);
                var content = await response.Content.ReadAsStringAsync(cancelationToken);
                var countriesResponse = JsonConvert.DeserializeObject<CountriesResponse>(content);
                return countriesResponse;
            }
            catch
            {
                return null;
            }
        }
        public static async Task SeedDataToDb(ModelBuilder builder)
        {
            CancellationTokenSource cts = new();
            var response = await GetCountriesFromExternalAPIAsync(cts.Token);
            if (response != null && response.Error == false)
            {
                var countries = new List<Country>();
                var cities = new List<City>();
                int countryId = 1;
                int cityId = 1;
                foreach (var countryData in response.Data)
                {
                    var country = new Country { Id = countryId, Name = countryData.Country ?? "" };
                    countries.Add(country);

                    if (countryData.Cities != null)
                    {
                        foreach (var cityName in countryData.Cities)
                        {
                            var city = new City { Id = cityId, Name = cityName ?? "", CountryId = countryId };
                            cities.Add(city);
                            cityId++;
                        }
                    }
                    countryId++;
                }
                builder.Entity<Country>().HasData(countries);
                builder.Entity<City>().HasData(cities);
            }
        }
    }
    public class CountriesResponse
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<CountryData> Data { get; set; }
    }

    public class CountryData
    {
        public string Country { get; set; }
        public List<string> Cities { get; set; }
    }
}
