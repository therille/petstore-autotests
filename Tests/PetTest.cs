using System.Collections.Generic;
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
    public class PetTest(ITestOutputHelper outputHelper) : BaseTest(outputHelper)
    {
        [Fact]
        public void PetsCRUDTest()
        {
            Log.Information("\nTEST: Verify pets can be created/updated/deleted\n");

            var newPet = PetActions.CreatePet("Snowflake", PetStatus.available);
            var response = PetActions.AddPetToStore(newPet);
            var newPetPost = JsonParser.ParseJson<Pet>(response);

            response = PetActions.FindPetById(newPetPost.Id);
            var newPetGet = JsonParser.ParseJson<Pet>(response);

            newPetPost.Compare(newPetGet);

            var updatePet = newPetPost;
            updatePet.Name = "Snowman";
            updatePet.Status = PetStatus.sold;

            response = PetActions.UpdateExistingPet(updatePet);
            var updatePetPut = JsonParser.ParseJson<Pet>(response);

            response = PetActions.FindPetById(updatePet.Id);
            var updatePetGet = JsonParser.ParseJson<Pet>(response);

            updatePetPut.Compare(updatePetGet);

            response = PetActions.DeletePet(updatePet.Id);
            response.Content.Should().Contain("\"message\":\"" + updatePet.Id + "\"");

            response = PetActions.FindPetById(updatePet.Id, HttpStatusCode.NotFound);
            response.Content.Should().Contain("\"message\":\"Pet not found\"");
        }

        [Fact]
        public void FindPetsByStatus()
        {
            Log.Information("\nTEST: Verify pets can be search by status (available, pending and sold)\n");

            var response = PetActions.FindPetsByStatus(PetStatus.available);
            var petList = JsonParser.ParseJson<List<Pet>>(response);
            petList.ForEach(x => x.Status.Should().Be(PetStatus.available));

            response = PetActions.FindPetsByStatus(PetStatus.pending);
            petList = JsonParser.ParseJson<List<Pet>>(response);
            petList.ForEach(x => x.Status.Should().Be(PetStatus.pending));

            response = PetActions.FindPetsByStatus(PetStatus.sold);
            petList = JsonParser.ParseJson<List<Pet>>(response);
            petList.ForEach(x => x.Status.Should().Be(PetStatus.sold));
        }

        [Fact]
        public void FindPetsByInvalidStatus()
        {
            Log.Information("\nTEST: Verify \"Bad Request\" when searching pets by invalid status\n");

            // Calling endpoint with invalid status should return error 400: Bad Request (according to Swagger)
            PetActions.FindPetsByStatus(PetStatus.invalid, HttpStatusCode.BadRequest);
        }
    }
}