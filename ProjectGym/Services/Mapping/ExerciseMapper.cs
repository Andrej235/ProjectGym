using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Mapping
{
    public class ExerciseMapper : IEntityMapperAsync<Exercise, ExerciseDTO>
    {
        private readonly IReadService<Image> imageReadService;
        private readonly IReadService<Equipment> equipmentReadService;
        private readonly IReadService<MuscleGroup> muscleReadService;
        private readonly IReadService<Alias> aliasReadService;
        private readonly IReadService<Note> noteReadService;

        public ExerciseMapper(IReadService<Image> imageReadService,
                              IReadService<Equipment> equipmentReadService,
                              IReadService<MuscleGroup> muscleReadService,
                              IReadService<Alias> aliasReadService,
                              IReadService<Note> noteReadService)
        {
            this.imageReadService = imageReadService;
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
            EquipmentIds = entity.Equipment.Select(a => a.Id),
            NoteIds = entity.Notes.Select(a => a.Id),
            PrimaryMuscleGroupIds = entity.PrimaryMuscleGroups.Select(a => a.Id),
            SecondaryMuscleGroupIds = entity.SecondaryMuscleGroups.Select(a => a.Id),
        };

        public async Task<Exercise> Map(ExerciseDTO dto) => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Equipment = await equipmentReadService.Get(x => dto.EquipmentIds.Contains(x.Id), 0, -1, "none"),
            Notes = await noteReadService.Get(x => dto.NoteIds.Contains(x.Id), 0, -1, "none"),
            Aliases = await aliasReadService.Get(x => dto.AliasIds.Contains(x.Id), 0, -1, "none"),
            Images = await imageReadService.Get(x => dto.ImageIds.Contains(x.Id), 0, -1, "none"),
            PrimaryMuscleGroups = await muscleReadService.Get(x => dto.PrimaryMuscleGroupIds.Contains(x.Id), 0, -1, "none"),
            SecondaryMuscleGroups = await muscleReadService.Get(x => dto.SecondaryMuscleGroupIds.Contains(x.Id), 0, -1, "none"),
        };
    }
}
