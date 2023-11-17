using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly ExerciseContext context;

        private readonly IReadService<Exercise> exerciseReadService;
        private readonly IReadService<Muscle> muscleReadService;
        private readonly IReadService<Equipment> equipmentReadService;
        private readonly IReadService<Image> imageReadService;
        private readonly IReadService<Note> noteReadService;
        private readonly IReadService<Alias> aliasReadService;

        private readonly IEntityMapperAsync<Exercise, ExerciseDTO> exerciseMapper;
        private readonly IEntityMapperAsync<Muscle, MuscleDTO> muscleMapper;
        private readonly IEntityMapperAsync<Equipment, EquipmentDTO> equipmentMapper;
        private readonly IEntityMapperSync<Image, ImageDTO> imageMapper;
        private readonly IEntityMapperSync<Note, NoteDTO> noteMapper;
        private readonly IEntityMapperSync<Alias, ExerciseAliasDTO> aliasMapper;

        private readonly ICreateService<Exercise, int> exerciseCreateService;
        private readonly ICreateService<Muscle, int> muscleCreateService;
        private readonly ICreateService<Equipment, int> equipmentCreateService;
        private readonly ICreateService<Image, int> imageCreateService;
        private readonly ICreateService<Note, int> noteCreateService;
        private readonly ICreateService<Alias, int> aliasCreateService;

        public DatabaseController(ExerciseContext context,
                                  IReadService<Exercise> exerciseReadService,
                                  IReadService<Muscle> muscleReadService,
                                  IReadService<Equipment> equipmentReadService,
                                  IReadService<Image> imageReadService,
                                  IReadService<Note> noteReadService,
                                  IReadService<Alias> aliasReadService,
                                  IEntityMapperAsync<Exercise, ExerciseDTO> exerciseMapper,
                                  IEntityMapperAsync<Muscle, MuscleDTO> muscleMapper,
                                  IEntityMapperAsync<Equipment, EquipmentDTO> equipmentMapper,
                                  IEntityMapperSync<Image, ImageDTO> imageMapper,
                                  IEntityMapperSync<Note, NoteDTO> noteMapper,
                                  IEntityMapperSync<Alias, ExerciseAliasDTO> aliasMapper,
                                  ICreateService<Exercise, int> exerciseCreateService,
                                  ICreateService<Muscle, int> muscleCreateService,
                                  ICreateService<Equipment, int> equipmentCreateService,
                                  ICreateService<Image, int> imageCreateService,
                                  ICreateService<Note, int> noteCreateService,
                                  ICreateService<Alias, int> aliasCreateService)
        {
            this.context = context;
            this.exerciseReadService = exerciseReadService;
            this.muscleReadService = muscleReadService;
            this.equipmentReadService = equipmentReadService;
            this.imageReadService = imageReadService;
            this.noteReadService = noteReadService;
            this.aliasReadService = aliasReadService;
            this.exerciseMapper = exerciseMapper;
            this.muscleMapper = muscleMapper;
            this.equipmentMapper = equipmentMapper;
            this.imageMapper = imageMapper;
            this.noteMapper = noteMapper;
            this.aliasMapper = aliasMapper;
            this.exerciseCreateService = exerciseCreateService;
            this.muscleCreateService = muscleCreateService;
            this.equipmentCreateService = equipmentCreateService;
            this.imageCreateService = imageCreateService;
            this.noteCreateService = noteCreateService;
            this.aliasCreateService = aliasCreateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                DatabaseDTO databaseDTO = new()
                {
                    Exercises = (await exerciseReadService.Get(x => true, 0, -1, "all")).Select(exerciseMapper.Map),
                    Muscles = (await muscleReadService.Get(x => true, 0, -1, "all")).Select(muscleMapper.Map),
                    Equipment = (await equipmentReadService.Get(x => true, 0, -1, "all")).Select(equipmentMapper.Map),
                    Images = (await imageReadService.Get(x => true, 0, -1, "all")).Select(imageMapper.Map),
                    Notes = (await noteReadService.Get(x => true, 0, -1, "all")).Select(noteMapper.Map),
                    Aliases = (await aliasReadService.Get(x => true, 0, -1, "all")).Select(aliasMapper.Map),
                };
                return Ok(databaseDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> LoadData([FromBody] DatabaseDTO databaseDTO)
        {
            try
            {
                foreach (var entity in databaseDTO.Equipment)
                    await equipmentCreateService.Add(await equipmentMapper.Map(entity));

                foreach (var entity in databaseDTO.Muscles)
                    await muscleCreateService.Add(await muscleMapper.Map(entity));

                foreach (var entity in databaseDTO.Exercises)
                    await exerciseCreateService.Add(await exerciseMapper.Map(entity));

                foreach (var entity in databaseDTO.Images)
                    await imageCreateService.Add(imageMapper.Map(entity));

                foreach (var entity in databaseDTO.Notes)
                    await noteCreateService.Add(noteMapper.Map(entity));

                foreach (var entity in databaseDTO.Aliases)
                    await aliasCreateService.Add(aliasMapper.Map(entity));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDB()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDB()
        {
            try
            {
                await context.Database.EnsureDeletedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message);
            }
        }

        public class DatabaseDTO
        {
            public IEnumerable<ExerciseDTO> Exercises { get; set; } = new List<ExerciseDTO>();
            public IEnumerable<MuscleDTO> Muscles { get; set; } = new List<MuscleDTO>();
            public IEnumerable<EquipmentDTO> Equipment { get; set; } = new List<EquipmentDTO>();
            //public IEnumerable<ExerciseComment> ExerciseComments { get; set; }
            public IEnumerable<ImageDTO> Images { get; set; } = new List<ImageDTO>();
            //public IEnumerable<ExerciseVideo> ExerciseVideos { get; set; }
            public IEnumerable<NoteDTO> Notes { get; set; } = new List<NoteDTO>();
            public IEnumerable<ExerciseAliasDTO> Aliases { get; set; } = new List<ExerciseAliasDTO>();

            /*            public IEnumerable<User> Users { get; set; }
                        public IEnumerable<Client> Clients { get; set; }
                        public IEnumerable<CommentUserUpvote> CommentUserUpvotes { get; set; }
                        public IEnumerable<CommentUserDownvote> CommentUserDownvotes { get; set; }
                        public IEnumerable<UserExerciseBookmark> UserExerciseBookmarks { get; set; }
                        public IEnumerable<Set> Sets { get; set; }
                        public IEnumerable<Superset> Supersets { get; set; }
                        public IEnumerable<UserExerciseWeight> UserExerciseWeights { get; set; }
                        public IEnumerable<Workout> Workouts { get; set; }
                        public IEnumerable<WorkoutSet> WorkoutSets { get; set; }*/
        }
    }
}