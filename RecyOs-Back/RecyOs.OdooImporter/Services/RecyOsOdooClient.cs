using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RecyOs.OdooImporter.Interfaces;

namespace RecyOs.OdooImporter.Services
{
    public class RecyOsOdooClient : IRecyOsOdooClient
    {
        private readonly HttpClient _httpClient;
        private readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _baseUrl;
        private readonly string _db;
        private readonly string _username;
        private readonly string _password;
        private int _uid;

        public RecyOsOdooClient(string baseUrl, string db, string username, string password)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl.TrimEnd('/');
            _db = db;
            _username = username;
            _password = password;
            InitializeClient().Wait();
        }

        private async Task InitializeClient()
        {
            try
            {
                _uid = await Authenticate();
                _logger.Info($"Authenticated with UID: {_uid}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Authentication failed");
                throw;
            }
        }

        private async Task<int> Authenticate()
        {
            var jsonRpc = new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    service = "common",
                    method = "authenticate",
                    args = new object[] { _db, _username, _password, new { } }
                },
                id = 1
            };

            var response = await CallOdooAsync("/jsonrpc", jsonRpc);
            return response.Value<int>();
        }

        public async Task<List<Dictionary<string, object>>> SearchRead(string model, object[] domain, string[] fields, int limit = 0)
        {
            var jsonRpc = new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    service = "object",
                    method = "execute_kw",
                    args = new object[]
                    {
                        _db,
                        _uid,
                        _password,
                        model,
                        "search_read",
                        new object[] { domain },
                        new { fields, limit }
                    }
                },
                id = new Random().Next()
            };

            var response = await CallOdooAsync("/jsonrpc", jsonRpc);
            return ParseResponse(response);
        }

        private async Task<JToken?> CallOdooAsync(string endpoint, object data)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP error {response.StatusCode}: {responseString}");
            }

            var jsonResponse = JObject.Parse(responseString);

            if (jsonResponse["error"] != null)
            {
                var error = jsonResponse["error"]["data"]["message"].Value<string>();
                throw new Exception($"Odoo error: {error}");
            }

            return jsonResponse["result"];
        }

        private static List<Dictionary<string, object>> ParseResponse(JToken response)
        {
            var result = new List<Dictionary<string, object>>();
            
            if (response is JArray array)
            {
                foreach (var item in array)
                {
                    if (item is JObject obj)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (var prop in obj.Properties())
                        {
                            dict[prop.Name] = prop.Value.ToObject<object>();
                        }
                        result.Add(dict);
                    }
                }
            }
            
            return result;
        }
    }
}
