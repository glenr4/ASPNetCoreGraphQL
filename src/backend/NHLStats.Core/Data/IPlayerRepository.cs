using NHLStats.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHLStats.Core.Data
{
	public interface IPlayerRepository
	{
		Task<Player> Get(int id);

		Task<Player> GetRandom();

		Task<List<Player>> All();

		Task<Player> Add(Player player);

		Task<List<Address>> GetAddresses(int playerId);
	}
}