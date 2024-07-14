using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SqlServerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServerWebApi.Data
{
    public class ItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var Items = new List<Item>();

            using (var db = new ItemsContext(_connectionString))
            {
                Items = db.Inventory.ToList();
            }

            return Items;
        }
    }
}
