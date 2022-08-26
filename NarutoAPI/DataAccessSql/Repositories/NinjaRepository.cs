using DataAccessSql.Connection;
using Domain.Models;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessSql.Repositories
{
    public class NinjaRepository : BaseRepository, INinjaRepository
    {
        public NinjaRepository(string connectionString) : base(connectionString) { }

        public async Task AddNinja(NinjaModel ninja)
        {
            var parameters = new
            {
                Name = ninja.Name,
                Rank = ninja.Rank,
                Village = ninja.Village,
                Renegade = ninja.Renegade,
                ImageName = ninja.ImageName
            };

            var query = @"INSERT INTO Ninjas (Name, Rank, Village, Renegade, ImageName)
                        VALUES 
                        (@Name, @Rank, @Village, @Renegade, @ImageName)";

            await ExecuteAsync(query, parameters, commandType: System.Data.CommandType.Text);
        }

        public async Task UpdateNinja(NinjaModel ninja)
        {
            var parameters = new
            {
                ID = ninja.Id,
                Name = ninja.Name,
                Rank = ninja.Rank,
                Village = ninja.Village,
                Renegade = ninja.Renegade,
                ImageName = ninja.ImageName
            };

            string query = @"UPDATE Ninjas SET
                            Name = @Name,
                            Village = @Village,
                            Rank = @Rank,
                            ImageName = @ImageName
                            WHERE ID = @ID";

            await ExecuteAsync(query, parameters, commandType: System.Data.CommandType.Text);
        }

        public async Task<List<NinjaModel>> FindWithPagedSearch(string query)
        {
            var result = await QueryAsync<NinjaModel>(query, commandType: System.Data.CommandType.Text);
            return result?.ToList();
        }

        public async Task<NinjaModel> FindByID(int id)
        {
            string query = $"SELECT * FROM Ninjas WHERE ID = {id}";
            return await QueryFirstOrDefaultAsync<NinjaModel>(query, commandType: System.Data.CommandType.Text);
        }

        public async Task<int> GetCountNinjas(string query)
        {
            return await QueryFirstOrDefaultAsync<int>(query, commandType: System.Data.CommandType.Text);
        }

        public async Task Delete(int id)
        {
            var query = @"DELETE FROM Ninjas
                        WHERE ID = " + id;
            await ExecuteAsync(query, commandType: System.Data.CommandType.Text);
        }
    }
}
