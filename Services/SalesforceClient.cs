using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PCM.Services
{
    public class SalesforceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;
        private string _instanceUrl;

        public SalesforceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //public async Task AuthenticateAsync()
        //{
        //    var salesforceConfig = _configuration.GetSection("Salesforce");
        //    var requestBody = new Dictionary<string, string>
        //{
        //    { "grant_type", "password" },
        //    { "client_id", salesforceConfig["ConsumerKey"] },
        //    { "client_secret", salesforceConfig["ConsumerSecret"] },
        //    { "username", salesforceConfig["Username"] },
        //    { "password", salesforceConfig["Password"] + salesforceConfig["SecurityToken"] }
        //};

        //    var requestContent = new FormUrlEncodedContent(requestBody);
        //    var response = await _httpClient.PostAsync("https://login.salesforce.com/services/oauth2/token", requestContent);
        //   // response.EnsureSuccessStatusCode();

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var authResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

        //    Log.Information("Auth response: {AuthResponse}", authResponse);

        //    _accessToken = authResponse.GetProperty("access_token").GetString();

        //    Log.Information("Access token: {AccessToken}", _accessToken);

        //    _instanceUrl = authResponse.GetProperty("instance_url").GetString();

        //    Log.Information("Instance URL: {InstanceUrl}", _instanceUrl);
        //}

        public async Task AuthenticateAsync()
        {
            var salesforceConfig = _configuration.GetSection("Salesforce");
            var requestBody = new Dictionary<string, string>
    {
        { "grant_type", "password" },
        { "client_id", "3MVG9GCMQoQ6rpzRFkD66tOc6EJbS6ee38gViLM0W54KFbdCamNjGnVbgOLZizoL71eBubYfQTwvSWuErZm12" },
        { "client_secret", "2B7AF8C1667FA2D916BD58E5973783112FAC988FEE94CCCF167F18F0F648B6E4" },
        { "username", "coc13259@gmail.com" },
        { "password", "A@a11223344!" + "r8bUA8ee8XTxJCIKQ1jZvMnO" }
    };

            Log.Information("Request body: {RequestBody}", requestBody.ToString() );

            var requestContent = new FormUrlEncodedContent(requestBody);
            var response = await _httpClient.PostAsync("https://login.salesforce.com/services/oauth2/token", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response from Salesforce: " + responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Authentication failed: " + responseContent);
            }

            var authResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

            // Check if keys exist before accessing them
            if (authResponse.TryGetProperty("access_token", out var accessTokenElement))
            {
                _accessToken = accessTokenElement.GetString();
            }
            else
            {
                throw new Exception("Access token not found in response.");
            }

            if (authResponse.TryGetProperty("instance_url", out var instanceUrlElement))
            {
                _instanceUrl = instanceUrlElement.GetString();
            }
            else
            {
                throw new Exception("Instance URL not found in response.");
            }
        }

        public async Task CreateAccountAndContactAsync(string accountName, string contactFirstName, string contactLastName, string contactEmail)
        {
            await AuthenticateAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var account = new { Name = accountName };
            var accountResponse = await _httpClient.PostAsJsonAsync($"{_instanceUrl}/services/data/v56.0/sobjects/Account/", account);
           // accountResponse.EnsureSuccessStatusCode();

            Log.Information("Account response: {AccountResponse}", await accountResponse.Content.ReadAsStringAsync());

            var accountId = (await accountResponse.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetString();

            var contact = new
            {
                FirstName = contactFirstName,
                LastName = contactLastName,
                Email = contactEmail,
                AccountId = accountId
            };

            var contactResponse = await _httpClient.PostAsJsonAsync($"{_instanceUrl}/services/data/v56.0/sobjects/Contact/", contact);
           // contactResponse.EnsureSuccessStatusCode();
           Log.Information("Contact response: {ContactResponse}", await contactResponse.Content.ReadAsStringAsync());
        }
    }
}
