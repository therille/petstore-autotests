using FluentAssertions;
using PetStoreAutotests.Utilities;
using PetStoreAutotests.Controllers;
using PetStoreAutotests.Models;
using RestSharp;
using System.Net;

namespace PetStoreAutotests.Actions
{
    public static class StoreActions
    {
        public static Order CreateOrder(long petId, string shipDate, OrderStatus orderStatus, int quantity, bool complete, long id = 0)
        {
            var newOrder = new Order()
            {
                PetId = petId,
                Quantity = quantity,
                ShipDate = shipDate,
                Status = orderStatus,
                Complete = complete
            };
            if (id > 0) newOrder.Id = id;
            return newOrder;
        }

        public static RestResponse PlaceOrderForPet(Order body, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Order>();
            var client = petStore.CreateClient();

            var jsonBody = JsonParser.Serialize(body);
            var request = petStore.CreatePOSTRequest(PetStoreUrls.PlaceOrderForPet, jsonBody);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse InventoryStatus(HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Order>();
            var client = petStore.CreateClient();

            var request = petStore.CreateGETRequest(PetStoreUrls.StoreInventoryStatus);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse DeleteOrder(long orderId, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Order>();
            var client = petStore.CreateClient();

            var request = petStore.CreateDELETERequest(PetStoreUrls.DeleteOrderById);
            request.AddUrlSegment("orderId", orderId);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse FindOrderById(long orderId, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Order>();
            var client = petStore.CreateClient();

            var request = petStore.CreateGETRequest(PetStoreUrls.DeleteOrderById);
            request.AddUrlSegment("orderId", orderId);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }
    }
}