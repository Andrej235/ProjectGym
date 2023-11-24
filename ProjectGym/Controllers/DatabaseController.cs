//using Microsoft.AspNetCore.Mvc;
//using ProjectGym.Data;
//using ProjectGym.DTOs;
//using ProjectGym.Models;
//using ProjectGym.Services.Create;
//using ProjectGym.Services.Mapping;
//using ProjectGym.Services.Read;

//namespace ProjectGym.Controllers
//{
//    [Route("api/database")]
//    [ApiController]
//    public class DatabaseController(ExerciseContext context,
//                              IReadService<Exercise> exerciseReadService,
//                              IReadService<Muscle> muscleReadService,
//                              IReadService<Equipment> equipmentReadService,
//                              IReadService<Image> imageReadService,
//                              IReadService<Note> noteReadService,
//                              IReadService<Alias> aliasReadService,
//                              IEntityMapperAsync<Exercise, ExerciseDTO> exerciseMapper,
//                              IEntityMapperAsync<Muscle, MuscleDTO> muscleMapper,
//                              IEntityMapperAsync<Equipment, EquipmentDTO> equipmentMapper,
//                              IEntityMapper<Image, ImageDTO> imageMapper,
//                              IEntityMapper<Note, NoteDTO> noteMapper,
//                              IEntityMapper<Alias, ExerciseAliasDTO> aliasMapper,
//                              ICreateService<Exercise> exerciseCreateService,
//                              ICreateService<Muscle> muscleCreateService,
//                              ICreateService<Equipment> equipmentCreateService,
//                              ICreateService<Image> imageCreateService,
//                              ICreateService<Note> noteCreateService,
//                              ICreateService<Alias> aliasCreateService) : ControllerBase
//    {
//        [HttpGet]
//        public async Task<IActionResult> GetAllData()
//        {
//            try
//            {
//                DatabaseDTO databaseDTO = new()
//                {
//                    Exercises = (await exerciseReadService.Get(x => true, 0, -1, "all")).Select(exerciseMapper.Map),
//                    Muscles = (await muscleReadService.Get(x => true, 0, -1, "all")).Select(muscleMapper.Map),
//                    Equipment = (await equipmentReadService.Get(x => true, 0, -1, "all")).Select(equipmentMapper.Map),
//                    Images = (await imageReadService.Get(x => true, 0, -1, "all")).Select(imageMapper.Map),
//                    Notes = (await noteReadService.Get(x => true, 0, -1, "all")).Select(noteMapper.Map),
//                    Aliases = (await aliasReadService.Get(x => true, 0, -1, "all")).Select(aliasMapper.Map),
//                };
//                return Ok(databaseDTO);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message + ex.InnerException?.Message);
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> LoadData([FromBody] DatabaseDTO databaseDTO)
//        {
//            try
//            {
//                foreach (var entity in databaseDTO.Equipment)
//                    await equipmentCreateService.Add(await equipmentMapper.Map(entity));

//                foreach (var entity in databaseDTO.Muscles)
//                    await muscleCreateService.Add(await muscleMapper.Map(entity));

//                foreach (var entity in databaseDTO.Exercises)
//                    await exerciseCreateService.Add(await exerciseMapper.Map(entity));

//                foreach (var entity in databaseDTO.Images)
//                    await imageCreateService.Add(imageMapper.Map(entity));

//                foreach (var entity in databaseDTO.Notes)
//                    await noteCreateService.Add(noteMapper.Map(entity));

//                foreach (var entity in databaseDTO.Aliases)
//                    await aliasCreateService.Add(aliasMapper.Map(entity));

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message + ex.InnerException?.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateDB()
//        {
//            try
//            {
//                await context.Database.EnsureCreatedAsync();
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message + ex.InnerException?.Message);
//            }
//        }

//        [HttpDelete]
//        public async Task<IActionResult> DeleteDB()
//        {
//            try
//            {
//                await context.Database.EnsureDeletedAsync();
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message + ex.InnerException?.Message);
//            }
//        }

//        public class DatabaseDTO
//        {
//            public IEnumerable<ExerciseDTO> Exercises { get; set; } = new List<ExerciseDTO>();
//            public IEnumerable<MuscleDTO> Muscles { get; set; } = new List<MuscleDTO>();
//            public IEnumerable<EquipmentDTO> Equipment { get; set; } = new List<EquipmentDTO>();
//            //public IEnumerable<ExerciseComment> ExerciseComments { get; set; }
//            public IEnumerable<ImageDTO> Images { get; set; } = new List<ImageDTO>();
//            //public IEnumerable<ExerciseVideo> ExerciseVideos { get; set; }
//            public IEnumerable<NoteDTO> Notes { get; set; } = new List<NoteDTO>();
//            public IEnumerable<ExerciseAliasDTO> Aliases { get; set; } = new List<ExerciseAliasDTO>();

//            /*            public IEnumerable<User> Users { get; set; }
//                        public IEnumerable<Client> Clients { get; set; }
//                        public IEnumerable<CommentUserUpvote> CommentUserUpvotes { get; set; }
//                        public IEnumerable<CommentUserDownvote> CommentUserDownvotes { get; set; }
//                        public IEnumerable<UserExerciseBookmark> UserExerciseBookmarks { get; set; }
//                        public IEnumerable<Set> Sets { get; set; }
//                        public IEnumerable<Superset> Supersets { get; set; }
//                        public IEnumerable<UserExerciseWeight> UserExerciseWeights { get; set; }
//                        public IEnumerable<Workout> Workouts { get; set; }
//                        public IEnumerable<WorkoutSet> WorkoutSets { get; set; }*/
//        }
//    }
//}