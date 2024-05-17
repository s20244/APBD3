using AnimalsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalsAPI.Repositories
{
    public interface IAnimalsRepository
    {
        Task<IEnumerable<Animal>> GetAnimals(string orderBy);
        Task<Animal> GetAnimalById(int id);
        Task InsertAnimal(Animal animal);
        Task UpdateAnimal(Animal animal);
        Task<bool> DeleteAnimal(int id);
    }

}
