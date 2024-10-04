using FluentAssertions;
using FluentAssertions.AssertMultiple;
using Newtonsoft.Json;

namespace PetStoreAutotests.Models
{
    public partial class Inventory
    {
        [JsonProperty("available", Required = Required.Default)]
        public int Available { get; set; }

        [JsonProperty("AVAILABLE", Required = Required.Default)]
        public int AvailableCap { get; set; }

        [JsonProperty("sold", Required = Required.Default)]
        public int Sold { get; set; }

        [JsonProperty("SOLD", Required = Required.Default)]
        public int SoldCap { get; set; }

        [JsonProperty("pending", Required = Required.Default)]
        public int Pending { get; set; }

        [JsonProperty("PENDING", Required = Required.Default)]
        public int PendingCap { get; set; }

        [JsonProperty("status", Required = Required.Default)]
        public int Status { get; set; }

        public void Compare(Inventory inventory)
        {
            AssertMultiple.Multiple(() =>
            {
                Available.Should().Be(inventory.Available);
                AvailableCap.Should().Be(inventory.AvailableCap);
                Sold.Should().Be(inventory.Sold);
                SoldCap.Should().Be(inventory.SoldCap);
                Pending.Should().Be(inventory.Pending);
                PendingCap.Should().Be(inventory.PendingCap);
                Status.Should().Be(inventory.Status);
            });
        }
    }
}