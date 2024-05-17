using AnimalsAPI.Models;
using AnimalsAPI.Repositories;

namespace AnimalsAPI.Services
{
    public class AnimalsService : IAnimalsService
    {
        private readonly IAnimalsRepository _animalsRepository;

        public AnimalsService(IAnimalsRepository animalsRepository)
        {
            _animalsRepository = animalsRepository;
        }

        public async Task<IEnumerable<Animal>> GetAnimals(string orderBy)
        {
            return await _animalsRepository.GetAnimals(orderBy);
        }

        public async Task InsertAnimal(Animal animal)
        {
            await _animalsRepository.InsertAnimal(animal);
        }

        public async Task UpdateAnimal(Animal animal)
        {
            await _animalsRepository.UpdateAnimal(animal);
        }

        public async Task<bool> DeleteAnimal(int id)
        {
            return await _animalsRepository.DeleteAnimal(id);
        }

    }
}
