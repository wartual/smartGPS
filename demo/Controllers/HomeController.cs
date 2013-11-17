using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace demo.Controllers
{
    public class HomeController : AsyncController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Test()
        {
            RunClient();
            return View();
        }

       

        private async Task RunClient()
        {
            string _address = "http://api.worldbank.org/countries?format=json";

            // Create an HttpClient instance

            HttpClient client = new HttpClient();

            // Send a request asynchronously and continue when complete

            HttpResponseMessage response = await client.GetAsync(_address);

            // Check that response was successful or throw exception

            response.EnsureSuccessStatusCode();

            // Read response asynchronously as JToken and write out top facts for each country

            JArray content = await response.Content.ReadAsAsync<JArray>();

            Console.WriteLine("First 50 countries listed by The World Bank...");

            foreach (var country in content[1])
            {

                Console.WriteLine("   {0}, Country Code: {1}, Capital: {2}, Latitude: {3}, Longitude: {4}",

                    country.Value<string>("name"),

                    country.Value<string>("iso2Code"),

                    country.Value<string>("capitalCity"),

                    country.Value<string>("latitude"),

                    country.Value<string>("longitude"));

            }


        }
    }

}
