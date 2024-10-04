using System;
using PetStoreAutotests.Actions;
using PetStoreAutotests.Models;
using PetStoreAutotests.Utilities;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace PetStoreAutotests.Tests
{
    public class InventoryTest(ITestOutputHelper outputHelper) : BaseTest(outputHelper)
    {
        [Fact]
        public void InventoryStatus()
        {
            Log.Information("\nTEST: Verify Inventory Status after andin Pet and placing Order\n");

            var response = StoreActions.InventoryStatus();
            var inventoryStatus1 = JsonParser.ParseJson<Inventory>(response);

            var newPet = PetActions.CreatePet("Belka", PetStatus.available);
            response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            response = StoreActions.InventoryStatus();
            var inventoryStatus2 = JsonParser.ParseJson<Inventory>(response);

            // After adding available Pet to a Store, number of available Pets should increase for 1
            inventoryStatus1.Available += 1;
            inventoryStatus1.Compare(inventoryStatus2);

            var newOrder = StoreActions.CreateOrder(newPetPost.Id, DateTime.Now.ToString(timeDateFormat), OrderStatus.approved, 1, true);
            response = StoreActions.PlaceOrderForPet(newOrder);
            var newOrderPost = JsonParser.ParseJson<Order>(response);

            response = StoreActions.InventoryStatus();
            var inventoryStatus3 = JsonParser.ParseJson<Inventory>(response);

            // After placing Order for a Pet, number of available Pets should decrease for 1
            inventoryStatus2.Available -= 1;
            inventoryStatus2.Compare(inventoryStatus3);

            StoreActions.DeleteOrder(newOrderPost.Id);
            response = StoreActions.InventoryStatus();
            var inventoryStatus4 = JsonParser.ParseJson<Inventory>(response);

            // After deleting Order for a Pet, number of available Pets should increase for 1
            inventoryStatus3.Available += 1;
            inventoryStatus3.Compare(inventoryStatus4);

            PetActions.DeletePet(newPetPost.Id);
            response = StoreActions.InventoryStatus();
            var inventoryStatus5 = JsonParser.ParseJson<Inventory>(response);

            // After deleting Pet from a Store, number of available Pets should decrease for 1
            inventoryStatus4.Available -= 1;
            inventoryStatus4.Compare(inventoryStatus5);
        }
    }
}