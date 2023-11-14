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
        private readonly IReadService<ExerciseCategory> categoryReadService;
        private readonly IReadService<Muscle> muscleReadService;
        private readonly IReadService<Equipment> equipmentReadService;
        private readonly IReadService<ExerciseImage> imageReadService;
        private readonly IReadService<ExerciseNote> noteReadService;
        private readonly IReadService<ExerciseAlias> aliasReadService;

        private readonly IEntityMapperAsync<Exercise, ExerciseDTO> exerciseMapper;
        private readonly IEntityMapperAsync<ExerciseCategory, CategoryDTO> categoryMapper;
        private readonly IEntityMapperAsync<Muscle, MuscleDTO> muscleMapper;
        private readonly IEntityMapperAsync<Equipment, EquipmentDTO> equipmentMapper;
        private readonly IEntityMapperSync<ExerciseImage, ImageDTO> imageMapper;
        private readonly IEntityMapperSync<ExerciseNote, NoteDTO> noteMapper;
        private readonly IEntityMapperSync<ExerciseAlias, ExerciseAliasDTO> aliasMapper;

        private readonly ICreateService<Exercise, int> exerciseCreateService;
        private readonly ICreateService<ExerciseCategory, int> categoryCreateService;
        private readonly ICreateService<Muscle, int> muscleCreateService;
        private readonly ICreateService<Equipment, int> equipmentCreateService;
        private readonly ICreateService<ExerciseImage, int> imageCreateService;
        private readonly ICreateService<ExerciseNote, int> noteCreateService;
        private readonly ICreateService<ExerciseAlias, int> aliasCreateService;

        public DatabaseController(ExerciseContext context,
                                  IReadService<Exercise> exerciseReadService,
                                  IReadService<ExerciseCategory> categoryReadService,
                                  IReadService<Muscle> muscleReadService,
                                  IReadService<Equipment> equipmentReadService,
                                  IReadService<ExerciseImage> imageReadService,
                                  IReadService<ExerciseNote> noteReadService,
                                  IReadService<ExerciseAlias> aliasReadService,
                                  IEntityMapperAsync<Exercise, ExerciseDTO> exerciseMapper,
                                  IEntityMapperAsync<ExerciseCategory, CategoryDTO> categoryMapper,
                                  IEntityMapperAsync<Muscle, MuscleDTO> muscleMapper,
                                  IEntityMapperAsync<Equipment, EquipmentDTO> equipmentMapper,
                                  IEntityMapperSync<ExerciseImage, ImageDTO> imageMapper,
                                  IEntityMapperSync<ExerciseNote, NoteDTO> noteMapper,
                                  IEntityMapperSync<ExerciseAlias, ExerciseAliasDTO> aliasMapper,
                                  ICreateService<Exercise, int> exerciseCreateService,
                                  ICreateService<ExerciseCategory, int> categoryCreateService,
                                  ICreateService<Muscle, int> muscleCreateService,
                                  ICreateService<Equipment, int> equipmentCreateService,
                                  ICreateService<ExerciseImage, int> imageCreateService,
                                  ICreateService<ExerciseNote, int> noteCreateService,
                                  ICreateService<ExerciseAlias, int> aliasCreateService)
        {
            this.context = context;
            this.exerciseReadService = exerciseReadService;
            this.categoryReadService = categoryReadService;
            this.muscleReadService = muscleReadService;
            this.equipmentReadService = equipmentReadService;
            this.imageReadService = imageReadService;
            this.noteReadService = noteReadService;
            this.aliasReadService = aliasReadService;
            this.exerciseMapper = exerciseMapper;
            this.categoryMapper = categoryMapper;
            this.muscleMapper = muscleMapper;
            this.equipmentMapper = equipmentMapper;
            this.imageMapper = imageMapper;
            this.noteMapper = noteMapper;
            this.aliasMapper = aliasMapper;
            this.exerciseCreateService = exerciseCreateService;
            this.categoryCreateService = categoryCreateService;
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
                    Categories = (await categoryReadService.Get(x => true, 0, -1, "all")).Select(categoryMapper.Map),
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
                foreach (var entity in databaseDTO.Categories)
                    await categoryCreateService.Add(await categoryMapper.Map(entity));

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
            public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
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