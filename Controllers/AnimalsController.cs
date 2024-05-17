using AnimalsAPI.Models;
using AnimalsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AnimalsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalsRepository _animalsRepository;

        public AnimalsController(IAnimalsRepository animalsRepository)
        {
            _animalsRepository = animalsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] string? orderBy)
        {
            var animals = await _animalsRepository.GetAnimals(orderBy ?? "name");
            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> InsertAnimal([FromBody] Animal newAnimal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAnimal = await _animalsRepository.GetAnimalById(newAnimal.IdAnimal);
            if (existingAnimal != null)
            {
                return Conflict(new { message = "Zwierzę z podanym ID już istnieje." });
            }

            await _animalsRepository.InsertAnimal(newAnimal);
            return CreatedAtAction(nameof(GetAnimals), new { id = newAnimal.IdAnimal }, new { message = "Zwierzę zostało dodane pomyślnie.", animal = newAnimal });
        }

        [HttpPut("{idAnimal}")]
        public async Task<IActionResult> UpdateAnimal(int idAnimal, [FromBody] Animal updatedAnimal)
        {
            if (idAnimal != updatedAnimal.IdAnimal)
            {
                return BadRequest("ID mismatch");
            }
            var existingAnimal = await _animalsRepository.GetAnimalById(idAnimal);
            if (existingAnimal == null)
            {
                return NotFound(new { message = "Zwierzę nie zostało znalezione po podanym ID." });
            }

            await _animalsRepository.UpdateAnimal(updatedAnimal);
            return Ok(new { message = "Zwierzę zaktualizowano pomyślnie." });
        }

        [HttpDelete("{idAnimal}")]
        public async Task<IActionResult> DeleteAnimal(int idAnimal)
        {
            bool deleted = await _animalsRepository.DeleteAnimal(idAnimal);

            if (deleted)
            {
                return Ok(new { message = "Zwierzę usunięto pomyślnie." });
            }
            else
            {
                return NotFound(new { message = "Zwierzę nie zostało znalezione po podanym ID." });
            }
        }
    }
}
