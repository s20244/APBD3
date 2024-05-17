using AnimalsAPI.Models;
using Microsoft.Data.SqlClient;

namespace AnimalsAPI.Repositories
{
    public class AnimalsRepository : IAnimalsRepository
    {
        private readonly string _connectionString;

        public AnimalsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<Animal>> GetAnimals(string orderBy)
        {
            var validColumns = new HashSet<string> { "name", "description", "category", "area" };
            if (string.IsNullOrWhiteSpace(orderBy) || !validColumns.Contains(orderBy.ToLower()))
            {
                orderBy = "name";
            }

            var animals = new List<Animal>();
            var query = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animals.Add(new Animal
                    {
                        IdAnimal = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    });
                }
            }

            return animals;
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            Animal animal = null;
            var query = "SELECT * FROM Animal WHERE IdAnimal = @IdAnimal";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", id);

                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    animal = new Animal
                    {
                        IdAnimal = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    };
                }
            }

            return animal;
        }

        public async Task InsertAnimal(Animal animal)
        {
            var insertQuery = "INSERT INTO Animal (IdAnimal, Name, Description, Category, Area) VALUES (@IdAnimal, @Name, @Description, @Category, @Area)";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
                    insertCommand.Parameters.AddWithValue("@Name", animal.Name);
                    insertCommand.Parameters.AddWithValue("@Description", (object)animal.Description ?? DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Category", animal.Category);
                    insertCommand.Parameters.AddWithValue("@Area", animal.Area);

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAnimal(Animal animal)
        {
            var query = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", (object)animal.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> DeleteAnimal(int id)
        {
            var query = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", id);

                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
