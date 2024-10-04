using System.Net;
using FluentAssertions;
using PetStoreAutotests.Utilities;
using PetStoreAutotests.Models;
using PetStoreAutotests.Controllers;
using RestSharp;

namespace PetStoreAutotests.Actions
{
    public static class PetActions
    {
        public static Pet CreatePet(string name, PetStatus status, long id = 0)
        {
            var newPet = new Pet()
            {
                Name = name,
                Status = status,
                Category = new PetCategory()
                {
                    Id = int.Parse(Randomizer.GenerateRandomId()),
                    Name = Randomizer.GenerateRandomString()
                }
            };
            if (id > 0) newPet.Id = id;
            return newPet;
        }

        public static RestResponse FindPetById(long petId, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Pet>();
            var client = petStore.CreateClient();

            var request = petStore.CreateGETRequest(PetStoreUrls.FindPetById);
            request.AddUrlSegment("petId", petId);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse AddPetToStore(Pet body, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Pet>();
            var client = petStore.CreateClient();

            var jsonBody = JsonParser.Serialize(body);
            var request = petStore.CreatePOSTRequest(PetStoreUrls.AddPetToStore, jsonBody);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse UpdateExistingPet(Pet body, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Pet>();
            var client = petStore.CreateClient();

            var jsonBody = JsonParser.Serialize(body);
            var request = petStore.CreatePUTRequest(PetStoreUrls.AddPetToStore, jsonBody);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse DeletePet(long petId, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Pet>();
            var client = petStore.CreateClient();

            var request = petStore.CreateDELETERequest(PetStoreUrls.FindPetById);
            request.AddUrlSegment("petId", petId);
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }

        public static RestResponse FindPetsByStatus(PetStatus petStatus, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var petStore = new PetStoreController<Pet>();
            var client = petStore.CreateClient();

            var request = petStore.CreateGETRequest(PetStoreUrls.FindPetsByStatus);
            request.AddQueryParameter("status", petStatus.ToString());
            var response = petStore.GetResponse(client, request);

            response.StatusCode.Should().Be(expectedStatusCode);
            return response;
        }
    }
}