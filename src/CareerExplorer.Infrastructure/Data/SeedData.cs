using Newtonsoft.Json;

namespace CareerExplorer.Infrastructure.Data
{
    public static class SeedData
    {
        public static CountriesResponse GetCountriesFromExternalAPI()
        {
            using var client = new HttpClient();
            var response = client.GetAsync("https://countriesnow.space/api/v0.1/countries").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var countriesResponse = JsonConvert.DeserializeObject<CountriesResponse>(content);
            return countriesResponse;
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
}