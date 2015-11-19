using System.Net;
using RestSharp;

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
            var getRequest = new RestRequest();

            getRequest.Resource = "remote/http/update";

            getRequest.AddQueryParameter("auth", _authToken);

            // path instance/*/*[SerialNumber/asString='6D 49 D3 A5']
            getRequest.AddQueryParameter("path", "instance/*/*[SerialNumber/asString='" + deviceId + "']");

            // inputAttributes Location=[instance/Location/Location1]
            getRequest.AddQueryParameter("inputAttributes", "Location=[instance/Location/" + location + "]");

            System.Console.WriteLine("request made");

            var response = _restClient.Get(getRequest);

            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);
        }
    }
}
