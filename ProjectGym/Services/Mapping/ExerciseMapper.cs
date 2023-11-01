using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Mapping
{
    public class ExerciseMapper : IEntityMapperAsync<Exercise, ExerciseDTO>
    {
        private readonly IReadService<ExerciseCategory> categoryReadService;
        private readonly IReadService<ExerciseImage> imageReadService;
        private readonly IReadService<ExerciseVideo> videoReadService;
        private readonly IReadService<Exercise> exerciseReadService;
        private readonly IReadService<Equipment> equipmentReadService;
        private readonly IReadService<Muscle> muscleReadService;
        private readonly IReadService<ExerciseAlias> aliasReadService;
        private readonly IReadService<ExerciseNote> noteReadService;

        public ExerciseMapper(IReadService<ExerciseCategory> categoryReadService,
                              IReadService<ExerciseImage> imageReadService,
                              IReadService<ExerciseVideo> videoReadService,
                              IReadService<Exercise> exerciseReadService,
                              IReadService<Equipment> equipmentReadService,
                              IReadService<Muscle> muscleReadService,
                              IReadService<ExerciseAlias> aliasReadService,
                              IReadService<ExerciseNote> noteReadService)
        {
            this.categoryReadService = categoryReadService;
            this.imageReadService = imageReadService;
            this.videoReadService = videoReadService;
            this.exerciseReadService = exerciseReadService;
            this.equipmentReadService = equipmentReadService;
            this.muscleReadService = muscleReadService;
            this.aliasReadService = aliasReadService;
            this.noteReadService = noteReadService;
        }

        public ExerciseDTO Map(Exercise entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ImageIds = entity.Images.Select(i => i.Id),
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

        public async Task<Exercise> Map(ExerciseDTO dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Category = await categoryReadService.Get(x => x.Id == dto.CategoryId, "none"),
            Equipment = await equipmentReadService.Get(x => dto.EquipmentIds.Contains(x.Id), 0, -1, "none"),
            Notes = await noteReadService.Get(x => dto.NoteIds.Contains(x.Id), 0, -1, "none"),
            Aliases = await aliasReadService.Get(x => dto.AliasIds.Contains(x.Id), 0, -1, "none"),
            Images = await imageReadService.Get(x => dto.ImageIds.Contains(x.Id), 0, -1, "none"),
            Videos = await videoReadService.Get(x => dto.VideoIds.Contains(x.Id), 0, -1, "none"),
            PrimaryMuscles = await muscleReadService.Get(x => dto.PrimaryMuscleIds.Contains(x.Id), 0, -1, "none"),
            SecondaryMuscles = await muscleReadService.Get(x => dto.SecondaryMuscleIds.Contains(x.Id), 0, -1, "none"),
            VariationExercises = await exerciseReadService.Get(x => dto.VariationIds.Contains(x.Id), 0, -1, "none"),
            IsVariationOf = await exerciseReadService.Get(x => dto.IsVariationOfIds.Contains(x.Id), 0, -1, "none"),
            UUID = Guid.NewGuid().ToString()
        };
    }
}
