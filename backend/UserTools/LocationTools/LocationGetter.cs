using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LocationTools
{
    public class LocationGetter
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<string> GetIPAddressAsync()
        {
            var response = await HttpClient.GetStringAsync("https://api.ipify.org?format=json");
            var json = JObject.Parse(response);
            return json["ip"]?.ToString();
        }

        public async Task<Location> GetLocationAsync(string ipAddress)
        {
            var response = await HttpClient.GetStringAsync($"http://ipinfo.io/{ipAddress}?token=3f47dd5390f1f1");
            var json = JObject.Parse(response);

            return new Location
            {
                IP = json["ip"]?.ToString(),
                City = json["city"]?.ToString(),
                Region = json["region"]?.ToString(),
                Country = json["country"]?.ToString(),
                Loc = json["loc"]?.ToString()
            };
        }
    }

    public class Location
    {
        public string IP { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Loc { get; set; }
    }
}
