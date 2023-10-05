using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        [HttpGet("basic")]
        public async Task<IActionResult> A()
        {
            ExerciseContext context = new();

            var a = await context.Exercises
                .Include(e => e.Images)
                .Take(10)
                .ToListAsync();

            var res = a.Select(a => new ExerciseDTO()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageDTO()
                {
                    Id = i.Id,
                    ImageURL = i.ImageURL,
                    IsMain = i.IsMain,
                    Style = i.Style
                }),
            });

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetFull()
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
                .Take(10)
                .ToListAsync();

            var res = a.Select(e => new ExerciseDTO()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Images = e.Images.Select(i => new ImageDTO()
                {
                    Id = i.Id,
                    ImageURL = i.ImageURL,
                    IsMain = i.IsMain,
                    Style = i.Style
                }),
                AliasIds = e.Aliases.Select(a => a.Id),
                CategoryId = e.CategoryId,
                EquipmentIds = e.Equipment.Select(a => a.Id),
                IsVariationOfIds = e.IsVariationOf.Select(a => a.Id),
                VariationIds = e.VariationExercises.Select(a => a.Id),
                NoteIds = e.Notes.Select(a => a.Id),
                PrimaryMuscleIds = e.PrimaryMuscles.Select(a => a.Id),
                SecondaryMuscleIds = e.SecondaryMuscles.Select(a => a.Id),
                VideoIds = e.Videos.Select(a => a.Id)
            });

            return Ok(res);
        }

        public class ExerciseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;

            public int CategoryId { get; set; }
            public IEnumerable<ImageDTO> Images { get; set; } = null!;
            public IEnumerable<int> VideoIds { get; set; } = null!;
            public IEnumerable<int> IsVariationOfIds { get; set; } = null!;
            public IEnumerable<int> VariationIds { get; set; } = null!;
            public IEnumerable<int> EquipmentIds { get; set; } = null!;
            public IEnumerable<int> PrimaryMuscleIds { get; set; } = null!;
            public IEnumerable<int> SecondaryMuscleIds { get; set; } = null!;
            public IEnumerable<int> AliasIds { get; set; } = null!;
            public IEnumerable<int> NoteIds { get; set; } = null!;
        }

        public class ImageDTO
        {
            public int Id { get; set; }
            public string ImageURL { get; set; } = null!;
            public bool IsMain { get; set; }
            public string Style { get; set; } = null!;
        }
    }
}
