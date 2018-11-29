using GraphQL.Types;
using NHLStats.Core.Models;

namespace NHLStats.Api.Models
{
	public class AddressType : ObjectGraphType<Address>
	{
		public AddressType()
		{
			Field(x => x.Id);
			Field(x => x.Street);
			Field(x => x.City);
			Field(x => x.PostCode, nullable: true);
		}
	}
}