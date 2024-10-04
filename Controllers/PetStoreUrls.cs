namespace PetStoreAutotests.Controllers
{
    internal static class PetStoreUrls
    {
        #region Base
        public const string BaseUrl = "https://petstore.swagger.io/v2/";
        #endregion

        #region Pets
        public const string AddPetToStore = @"pet";
        public const string FindPetById = @"pet/{petId}";
        public const string FindPetsByStatus = @"pet/findByStatus";
        public const string FindPetsByTag = @"pet/findByTags";
        #endregion

        #region Store
        public const string PlaceOrderForPet = @"store/order";
        public const string DeleteOrderById = @"store/order/{orderId}";
        public const string StoreInventoryStatus = @"store/inventory";
        #endregion
    }
}