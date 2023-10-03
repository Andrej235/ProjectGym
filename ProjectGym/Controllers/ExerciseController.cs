using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> A()
        {
            ExerciseContext context = new();

            var a = await context.Exercises
                .Include(e => e.VariationExercises)
                .Include(e => e.IsVariationOf)
                .Include(e => e.Category)
                .Include(e => e.PrimaryMuscles)
                .Include(e => e.SecondaryMuscles)
                .Include(e => e.Equipment)
                .Include(e => e.Images)
                .Include(e => e.Videos)
                .Include(e => e.Aliases)
                .Include(e => e.Notes)
                .ToListAsync();

            var res = a.Select(a => new SimpleExerciseDTO()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                ImageUrls = a.Images.Select(i => i.ImageURL),
                VideoUrls = a.Videos.Select(i => i.VideoUrl),
                IsVariationOf = a.IsVariationOf.Select(e => e.Id),
                Variations = a.VariationExercises.Select(e => e.Id),
                Aliases = a.Aliases.Select(a => a.Alias),
                Notes = a.Notes.Select(n => n.Comment),
                EquipmentName = a.Equipment.Select(e => e.Name),
                PrimaryMuscles = a.PrimaryMuscles.Select(m => m.Name),
                SecondaryMuscles = a.SecondaryMuscles.Select(m => m.Name),
                Category = a.Category.Name
            });

            return Ok(res);
        }

        public class SimpleExerciseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public string Category { get; set; } = null!;
            public IEnumerable<string> ImageUrls { get; set; } = null!;
            public IEnumerable<string> VideoUrls { get; set; } = null!;
            public IEnumerable<int> IsVariationOf { get; set; } = null!;
            public IEnumerable<int> Variations { get; set; } = null!;
            public IEnumerable<string> EquipmentName { get; set; } = null!;
            public IEnumerable<string> PrimaryMuscles { get; set; } = null!;
            public IEnumerable<string> SecondaryMuscles { get; set; } = null!;
            public IEnumerable<string> Aliases { get; set; } = null!;
            public IEnumerable<string> Notes { get; set; } = null!;
        }
    }
}
