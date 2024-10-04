using RestSharp;
using Serilog;

namespace PetStoreAutotests.Controllers
{
    public class PetStoreController<T>
    {
        public RestClient CreateClient()
        {
            var restClient = new RestClient(PetStoreUrls.BaseUrl);
            return restClient;
        }

        public RestRequest CreateGETRequest(string location)
        {
            var restRequest = new RestRequest(location, Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }

        public RestRequest CreatePOSTRequest(string location, string requestBody)
        {
            var restRequest = new RestRequest(location, Method.Post);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreatePUTRequest(string location, string requestBody)
        {
            var restRequest = new RestRequest(location, Method.Put);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreateDELETERequest(string location)
        {
            var restRequest = new RestRequest(location, Method.Delete);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }

        public RestResponse GetResponse(RestClient client, RestRequest request)
        {
            Log.Information("------------------------- Request -------------------------");
            Log.Information(request.Method.ToString() + " " + typeof(T).Name + ": " + PetStoreUrls.BaseUrl + request.Resource.ToString());

            var response = client.Execute(request);

            Log.Information("------------------------- Response -------------------------");
            Log.Information("Status: " + (int)response.StatusCode + " " + response.StatusCode.ToString());
            if (!string.IsNullOrEmpty(response.Content))
            {
                Log.Information("Message: " + response.Content.ToString() + "\n");
                //Log.Information(JToken.Parse(response.Content).ToString(Formatting.Indented))
            }
            return response;
        }
    }
}