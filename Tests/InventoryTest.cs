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
            Log.Information("\nTEST: Verify Inventory Status after adding/deleting Pet and creating/deleting Order\n");

            // Get initial Inventory Status of the Store
            var response = StoreActions.InventoryStatus();
            var inventoryStatus1 = JsonParser.ParseJson<Inventory>(response);

            // Create new available Pet and add to the Store
            var newPet = PetActions.CreatePet("Belka", PetStatus.available);
            response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            // After adding available Pet to the Store, number of available Pets should increase for 1
            response = StoreActions.InventoryStatus();
            var inventoryStatus2 = JsonParser.ParseJson<Inventory>(response);

            inventoryStatus1.Available += 1;
            inventoryStatus1.Compare(inventoryStatus2);

            // Place new Order for the Pet
            var newOrder = StoreActions.CreateOrder(newPetPost.Id, DateTime.Now.ToString(timeDateFormat), OrderStatus.approved, 1, true);
            response = StoreActions.PlaceOrderForPet(newOrder);
            var newOrderPost = JsonParser.ParseJson<Order>(response);

            // After placing new Order for the Pet, number of available Pets should decrease for 1
            response = StoreActions.InventoryStatus();
            var inventoryStatus3 = JsonParser.ParseJson<Inventory>(response);

            inventoryStatus2.Available -= 1;
            inventoryStatus2.Compare(inventoryStatus3);

            // Delete placed Order for the Pet
            StoreActions.DeleteOrder(newOrderPost.Id);

            // After deleting Order for a Pet, number of available Pets should increase for 1
            response = StoreActions.InventoryStatus();
            var inventoryStatus4 = JsonParser.ParseJson<Inventory>(response);

            inventoryStatus3.Available += 1;
            inventoryStatus3.Compare(inventoryStatus4);

            // Delete created Pet from the Store
            PetActions.DeletePet(newPetPost.Id);

            // After deleting created Pet from the Store, number of available Pets should decrease for 1
            response = StoreActions.InventoryStatus();
            var inventoryStatus5 = JsonParser.ParseJson<Inventory>(response);

            inventoryStatus4.Available -= 1;
            inventoryStatus4.Compare(inventoryStatus5);
        }
    }
}