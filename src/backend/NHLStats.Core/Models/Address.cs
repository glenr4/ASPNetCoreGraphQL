namespace NHLStats.Core.Models
{
	public class Address
	{
		public int Id { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }
		public int PlayerId { get; set; }
        public Player Player { get; set; }
	}
}