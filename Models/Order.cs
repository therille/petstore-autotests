using FluentAssertions;
using FluentAssertions.AssertMultiple;
using Newtonsoft.Json;

namespace PetStoreAutotests.Models
{
    public partial class Order
    {
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty("petId", Required = Required.Always)]
        public long PetId { get; set; }

        [JsonProperty("quantity", Required = Required.Always)]
        public int Quantity { get; set; }

        [JsonProperty("shipDate", Required = Required.Always)]
        public string ShipDate { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        public OrderStatus Status { get; set; }

        [JsonProperty("complete", Required = Required.Always)]
        public bool Complete { get; set; }

        public void Compare(Order order)
        {
            AssertMultiple.Multiple(() =>
            {
                Id.Should().Be(order.Id);
                PetId.Should().Be(order.PetId);
                Quantity.Should().Be(order.Quantity);
                ShipDate.Should().Be(order.ShipDate);
                Status.Should().Be(order.Status);
                Complete.Should().Be(order.Complete);
            });
        }
    }

    public enum OrderStatus
    {
        placed,
        approved,
        delivered
    }
}
