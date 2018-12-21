namespace NHLStats.Api.Controllers.Rest
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public int PlayerId { get; set; }
    }
}