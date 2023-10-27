using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class ExerciseMapper : IEntityMapper<Exercise, ExerciseDTO>
    {
        public ExerciseDTO Map(Exercise entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Images = entity.Images.Select(i => new ImageDTO()
            {
                Id = i.Id,
                ImageURL = i.ImageURL,
                IsMain = i.IsMain,
                Style = i.Style
            }),
            AliasIds = entity.Aliases.Select(a => a.Id),
            CategoryId = entity.CategoryId,
            EquipmentIds = entity.Equipment.Select(a => a.Id),
            IsVariationOfIds = entity.IsVariationOf.Select(a => a.Id),
            VariationIds = entity.VariationExercises.Select(a => a.Id),
            NoteIds = entity.Notes.Select(a => a.Id),
            PrimaryMuscleIds = entity.PrimaryMuscles.Select(a => a.Id),
            SecondaryMuscleIds = entity.SecondaryMuscles.Select(a => a.Id),
            VideoIds = entity.Videos.Select(a => a.Id),
        };
    }
}
