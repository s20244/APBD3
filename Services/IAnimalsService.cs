using AnimalsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalsAPI.Services
{
    public interface IAnimalsService
    {
        Task<IEnumerable<Animal>> GetAnimals(string orderBy);
        Task InsertAnimal(Animal animal);
        Task UpdateAnimal(Animal animal);
        Task<bool> DeleteAnimal(int id);
    }
}
