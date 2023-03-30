using System.Web;

namespace SummryApi.Services.HttpClients
{
    public class ScrapeApprovalClient
    {
        public ScrapeApprovalClient()
        {
        }


        public async Task<bool> IsShopifyScrapeable(string storeUrl)
        {
            bool canBeScraped = false;
            var queryParams = new Dictionary<string, string> { 
                { "page", "1" },
                { "limit", "250" }
            };

            string qs = CreateQueryString(queryParams);
            string reqUrl = $"/products.json?{qs}";

            var client = new HttpClient()
            {
                BaseAddress = new Uri(storeUrl)
            };
            client = AddHeaders(client);


            HttpResponseMessage response = await client.GetAsync(reqUrl);
            if ((int)response.StatusCode == 200) // this logic works but needs to change because what if store url returns 200 but page has 0 products?
            {
                canBeScraped = true; 
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            // var resData = JsonConvert.DeserializeObject<JObject>(responseBody);

            return canBeScraped;
        }

        // private 
        private static string CreateQueryString(Dictionary<string, string> dict)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            var keys = dict.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                string val = dict[key];
                query[key] = val;
            }

            return query.ToString();
        }


        private static HttpClient AddHeaders(HttpClient client)
        {
            var header = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.110 Safari/537.36";
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", header);
            return client;
        }

        //public async Task<string> Login(LoginRequest loginRequest)
        //{
        //    string payload = JsonConvert.SerializeObject(loginRequest);
        //    var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await _httpClient.PostAsync("/some_endpoint", httpContent);

        //    if ((int)response.StatusCode == 401)
        //    {
        //        throw new AuthenticationException($"not authorized");
        //    }

        //    response.EnsureSuccessStatusCode();
        //    string responseBody = await response.Content.ReadAsStringAsync();

        //    var token_obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

        //    return token_obj["token"]; ;
        //}
    }
}
