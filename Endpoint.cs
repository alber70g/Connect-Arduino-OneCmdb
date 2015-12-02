using System;
using System.Net;
using RestSharp;
using System.Linq;

namespace ConsoleApplication
{

    public class Endpoint
    {
        private RestClient _restClient;
        private string _authToken;
        public Endpoint()
        {
            _restClient = new RestClient("http://onecmdb.cloudapp.net:8080");

            var getRequest = new RestRequest();
            getRequest.Resource = "remote/http/auth";
            getRequest.AddQueryParameter("user", "admin");
            getRequest.AddQueryParameter("pwd", "123");

            var authResponse = _restClient.Get(getRequest);
            
            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                _authToken = authResponse.Content;
                System.Console.WriteLine("Login successful...");
            }
            else
            {
                System.Console.WriteLine("Login failed: ");
                System.Console.WriteLine(authResponse.StatusCode);
                System.Console.WriteLine(authResponse.Content);
            }
        }

        public void UpdateDevice(string deviceId, string location)
        {
            System.Console.WriteLine($"Update {deviceId} to location {location}");
            var getRequest = createNewRequest();

            // path instance/*/*[SerialNumber/asString='6D 49 D3 A5']
            getRequest.AddQueryParameter("path", "instance/*/" + deviceId);

            // inputAttributes Location=[instance/Location/Location1]
            getRequest.AddQueryParameter("inputAttributes", "Location=[instance/Location/" + location + "]");

            System.Console.WriteLine("request made");

            var response = _restClient.Get(getRequest);
            
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);
        }

        private RestRequest createNewRequest()
        {
            var getRequest = new RestRequest();
            getRequest.Resource = "remote/http/update";
            getRequest.AddQueryParameter("auth", _authToken);
            return getRequest;
        }

        public string GetAliasLocation(string alias)
        {
            System.Console.WriteLine("GetAliasLocation " + alias);
            
            var getRequest = createNewRequest();
            getRequest.Resource = "remote/http/query";
            getRequest.AddQueryParameter("path", "instance/*/" + alias);
            getRequest.AddQueryParameter("outputAttributes", "Location/alias");
            getRequest.AddQueryParameter("outputFormat", "property");

            var response = _restClient.Get(getRequest);
            System.Console.WriteLine("Response " + response.Content);
            var parts = response.Content.Split('=');
            return parts.Last();
        }

        public void UpdateReaderLocation(string alias, string id)
        {
            System.Console.WriteLine("UpdateReaderLocation " + alias + " " + id);
            UpdateDevice(alias, id);
        }

        public string GetDerivedFrom(string id)
        {
            System.Console.WriteLine("GetDerivedFrom " + id);
            
            var getRequest = createNewRequest();
            getRequest.Resource = "remote/http/query";
            
            getRequest.AddQueryParameter("path", "instance/*/" + id);
            getRequest.AddQueryParameter("outputAttributes", "derivedFrom");
            getRequest.AddQueryParameter("outputFormat", "property");

            var response = _restClient.Get(getRequest);
            System.Console.WriteLine(response.Content);
            var derivedFrom = response.Content.Split('=').Last().Trim();
            System.Console.WriteLine("DerivedFrom " + derivedFrom);
            return derivedFrom;
        }
    }
}
