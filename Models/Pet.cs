using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.AssertMultiple;
using Newtonsoft.Json;

namespace PetStoreAutotests.Models
{
    public partial class Pet
    {
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty("category", Required = Required.Default)]
        public PetCategory Category { get; set; }

        [JsonProperty("name", Required = Required.Default)]
        public string Name { get; set; }

        [JsonProperty("photoUrls", Required = Required.Default)]
        public List<string> PhotoUrls { get; set; }

        [JsonProperty("tags", Required = Required.Default)]
        public List<PetCategory> Tags { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        public PetStatus Status { get; set; }

        public void Compare(Pet pet)
        {
            AssertMultiple.Multiple(() =>
            {
                Id.Should().Be(pet.Id);
                Category.Id.Should().Be(pet.Category.Id);
                Name.Should().Be(pet.Name);
                Category.Name.Should().Be(pet.Category.Name);
                PhotoUrls.Should().BeEquivalentTo(pet.PhotoUrls);
                Tags.Should().BeEquivalentTo(pet.Tags);
                Status.Should().Be(pet.Status);
            });
        }
    }

    public partial class PetCategory
    {
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty("name", Required = Required.Default)]
        public string Name { get; set; }
    }

    public enum PetStatus
    {
        available,
        pending,
        sold,
        invalid
    }
}