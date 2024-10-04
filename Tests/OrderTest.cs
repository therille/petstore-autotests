using System;
using System.Net;
using FluentAssertions;
using PetStoreAutotests.Actions;
using PetStoreAutotests.Models;
using PetStoreAutotests.Utilities;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace PetStoreAutotests.Tests
{
    public class OrderTest(ITestOutputHelper outputHelper) : BaseTest(outputHelper)
    {
        [Fact]
        public void OrdersCRUDTest()
        {
            Log.Information("\nTEST: Verify Order can be created/deleted\n");
            // No PUT method for Order to be updated

            var newPet = PetActions.CreatePet("Milica", PetStatus.available);
            var response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            var newOrder = StoreActions.CreateOrder(newPetPost.Id, DateTime.Now.ToString(timeDateFormat), OrderStatus.placed, 1, true);

            response = StoreActions.PlaceOrderForPet(newOrder);
            var newOrderPost = JsonParser.ParseJson<Order>(response);

            newOrderPost.Complete.Should().BeTrue();
            newOrderPost.Status.Should().Be(OrderStatus.placed);

            response = StoreActions.FindOrderById(newOrderPost.Id);
            var newOrderGet = JsonParser.ParseJson<Order>(response);
            newOrderPost.Compare(newOrderGet);

            response = StoreActions.DeleteOrder(newOrderPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newOrderPost.Id + "\"");

            response = PetActions.DeletePet(newPetPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newPetPost.Id + "\"");
        }

        [Fact]
        public void InvalidOrderGet()
        {
            Log.Information("\nTEST: Verify \"Not Found\" for getting deleted order\n");

            var newPet = PetActions.CreatePet("Mrvica", PetStatus.available);
            var response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            var newOrder = StoreActions.CreateOrder(newPetPost.Id, DateTime.Now.ToString(timeDateFormat), OrderStatus.placed, 1, true);
            response = StoreActions.PlaceOrderForPet(newOrder);
            var newOrderPost = JsonParser.ParseJson<Order>(response);

            response = StoreActions.DeleteOrder(newOrderPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newOrderPost.Id + "\"");

            response = StoreActions.FindOrderById(newOrderPost.Id, HttpStatusCode.NotFound);
            response.Content.Should().Contain("\"message\":\"Order not found\"");

            response = PetActions.DeletePet(newPetPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newPetPost.Id + "\"");
        }

        [Fact]
        public void InvalidOrderDelete()
        {
            Log.Information("\nTEST: Verify \"Not Found\" for deleting deleted order\n");

            var newPet = PetActions.CreatePet("Gricko", PetStatus.available);
            var response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            var newOrder = StoreActions.CreateOrder(newPetPost.Id, DateTime.Now.ToString(timeDateFormat), OrderStatus.placed, 1, true);
            response = StoreActions.PlaceOrderForPet(newOrder);
            var newOrderPost = JsonParser.ParseJson<Order>(response);

            response = StoreActions.DeleteOrder(newOrderPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newOrderPost.Id + "\"");

            response = StoreActions.DeleteOrder(newOrderPost.Id, HttpStatusCode.NotFound);
            response.Content.Should().Contain("\"message\":\"Order Not Found\"");

            response = PetActions.DeletePet(newPetPost.Id);
            response.Content.Should().Contain("\"message\":\"" + newPetPost.Id + "\"");
        }
    }
}