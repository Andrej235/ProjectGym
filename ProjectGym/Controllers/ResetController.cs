using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ProjectGym.Controllers
{
    [Route("api/databasereset")]
    [ApiController]
    public class ResetController : ControllerBase
    {
        struct IdPairs
        {
            public int oldId;
            public int newId;
        }
        readonly JsonSerializerOptions jsonSerializerOptions;
        public ResetController()
        {
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [HttpGet]
        public async Task<IActionResult> PopulateDB()
        {
            ExerciseContext context = new();

            await context.Database.EnsureCreatedAsync();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var muscleDTOs = JsonSerializer.Deserialize<DTORootObject<ResetMuscleDTO>>(await GetData("muscle?limit=100000000&ordering=id&language=2"), jsonSerializerOptions);
            var muscles = muscleDTOs?.Results.Select(m => new Muscle
            {
                Name = m.Name,
                Name_en = m.Name_en,
                IsFront = m.Is_front,
                ImageUrlMain = m.Image_url_main,
                ImageUrlSecondary = m.Image_url_secondary,
            }).ToList();

            muscles ??= new();
            foreach (var muscle in muscles)
                await context.Muscles.AddAsync(muscle);
            await context.SaveChangesAsync();



            var equipmentDTOs = JsonSerializer.Deserialize<DTORootObject<ResetEquipmentDTO>>(await GetData("equipment?limit=100000000&ordering=id&language=2"), jsonSerializerOptions);
            var equipment = equipmentDTOs?.Results.Select(m => new Equipment
            {
                Name = m.Name,
            }).ToList();

            equipment ??= new();
            foreach (var eq in equipment)
                await context.Equipment.AddAsync(eq);
            await context.SaveChangesAsync();



            var exerciseCategoryDTOs = JsonSerializer.Deserialize<DTORootObject<ResetExerciseCategoryDTO>>(await GetData("exercisecategory?limit=100000000&ordering=id&language=2"), jsonSerializerOptions);
            var exerciseCategories = exerciseCategoryDTOs?.Results.Select(m => new ExerciseCategory
            {
                Name = m.Name,
            }).ToList();

            exerciseCategories ??= new();
            foreach (var category in exerciseCategories)
                await context.ExerciseCategories.AddAsync(category);
            await context.SaveChangesAsync();

            List<IdPairs> categoryIdPairs = new();
            if (exerciseCategoryDTOs != null)
                for (int i = 0; i < exerciseCategories.Count; i++)
                    categoryIdPairs.Add(new() { oldId = exerciseCategoryDTOs.Results[i].Id, newId = exerciseCategories[i].Id });


            var exerciseDTOs = JsonSerializer.Deserialize<DTORootObject<ResetExerciseDTO>>(await GetData("exercise?limit=100000000&ordering=id&language=2"), jsonSerializerOptions);
            var exercises = exerciseDTOs?.Results.Select(e => new Exercise
            {
                UUID = e.Uuid,
                Name = e.Name,
                Description = e.Description,
                Category = exerciseCategories.First(ec => ec.Id == categoryIdPairs.First(cidp => cidp.oldId == e.Category).newId)
            }).ToList();

            exercises ??= new();
            foreach (var exercise in exercises)
                await context.Exercises.AddAsync(exercise);
            await context.SaveChangesAsync();

            List<IdPairs> exerciseIdPairs = new();
            if (exerciseDTOs != null)
                for (int i = 0; i < exercises.Count; i++)
                {
                    exerciseIdPairs.Add(new()
                    {
                        oldId = exerciseDTOs.Results[i].Id,
                        newId = exercises[i].Id
                    });
                }

            exerciseDTOs ??= new();
            for (int i = 0; i < exerciseDTOs.Results.Length; i++)
            {
                var exerciseDTO = exerciseDTOs.Results[i];
                var exercise = exercises.First(e => e.UUID == exerciseDTO.Uuid);
                var exerciseVariationDTOIds = exerciseDTO.Variations.ToList();
                exerciseVariationDTOIds.Remove(exerciseDTO.Id);

                if (exerciseVariationDTOIds.Any())
                {
                    var exerciseVariationIds = exerciseIdPairs.Where(x => exerciseVariationDTOIds.Contains(x.oldId)).Select(x => x.newId).Order().ToList();
                    var variations = exercises.Where(e => exerciseVariationIds.Contains(e.Id)).ToList();

                    exercise.VariationExercises = variations;
                }

                var primaryMuscleIds = exerciseDTO.Muscles.ToList();
                var secondaryMuscleIds = exerciseDTO.Muscles_secondary.ToList();

                var primaryMuscles = muscles.Where(m => primaryMuscleIds.Contains(m.Id)).ToList();
                var secondaryMuscles = muscles.Where(m => secondaryMuscleIds.Contains(m.Id)).ToList();

                exercise.PrimaryMuscles = primaryMuscles;
                exercise.SecondaryMuscles = secondaryMuscles;

                exercise.Equipment = equipment.Where(eq => exerciseDTO.Equipment.Contains(eq.Id)).ToList();
            }
            await context.SaveChangesAsync();

            var exerciseBaseInfoDTOs = JsonSerializer.Deserialize<DTORootObject<ResetExerciseBaseInfoDTO>>(await GetData("exercisebaseinfo?ordering=id&language=2&limit=1000000"), jsonSerializerOptions);
            if (exerciseBaseInfoDTOs != null)
            {
                IEnumerable<ResetExerciseFromInfoDTO> exercisesInBaseInfos = exerciseBaseInfoDTOs.Results.SelectMany(x => x.Exercises);
                exercisesInBaseInfos = exercisesInBaseInfos.Where(x => x.Language == 2);
                Debug.WriteLine($"---> Exercises: {exercisesInBaseInfos.Count()}");

                IEnumerable<ResetImageDTO> imageDTOs = exerciseBaseInfoDTOs.Results.SelectMany(x => x.Images);
                Debug.WriteLine($"---> Images: {imageDTOs.Count()}");

                IEnumerable<ResetVideoDTO> videoDTOs = exerciseBaseInfoDTOs.Results.SelectMany(x => x.Videos);
                Debug.WriteLine($"---> Videos: {videoDTOs.Count()}");

                IEnumerable<ResetNoteDTO> noteDTOs = exercisesInBaseInfos.SelectMany(e => e.Notes);
                Debug.WriteLine($"---> Notes: {noteDTOs.Count()}");

                imageDTOs ??= new List<ResetImageDTO>();
                foreach (var imgDTO in imageDTOs)
                {
                    int exerciseIdOld = exercisesInBaseInfos.First(e => e.Exercise_base == imgDTO.Exercise_base).Id;
                    int exerciseId = exerciseIdPairs.First(x => x.oldId == exerciseIdOld).newId;

                    ExerciseImage img = new()
                    {
                        UUID = imgDTO.Uuid,
                        ImageURL = imgDTO.Image,
                        IsMain = imgDTO.Is_main,
                        Style = imgDTO.Style,
                        ExerciseId = exerciseId,
                    };
                    await context.ExerciseImages.AddAsync(img);
                }
                await context.SaveChangesAsync();



                videoDTOs ??= new List<ResetVideoDTO>();
                foreach (var videoDTO in videoDTOs)
                {
                    int exerciseIdOld = exercisesInBaseInfos.First(e => e.Exercise_base == videoDTO.Exercise_base).Id;
                    int exerciseId = exerciseIdPairs.First(x => x.oldId == exerciseIdOld).newId;

                    ExerciseVideo video = new()
                    {
                        UUID = videoDTO.Uuid,
                        Height = videoDTO.Height,
                        Width = videoDTO.Width,
                        Size = videoDTO.Size,
                        IsMain = videoDTO.Is_main,
                        Codec = videoDTO.Codec,
                        CodecLong = videoDTO.Codec,
                        Duration = videoDTO.Duration,
                        VideoUrl = videoDTO.Video,
                        ExerciseId = exerciseId
                    };
                    await context.ExerciseVideos.AddAsync(video);
                }
                await context.SaveChangesAsync();



                noteDTOs ??= new List<ResetNoteDTO>();
                foreach (var noteDTO in noteDTOs)
                {
                    int exerciseId = exerciseIdPairs.First(x => x.oldId == noteDTO.Exercise).newId;

                    ExerciseNote note = new()
                    {
                        Comment = noteDTO.Comment,
                        ExerciseId = exerciseId
                    };
                    await context.ExerciseNotes.AddAsync(note);
                }
                await context.SaveChangesAsync();



                foreach (var exercise in exercisesInBaseInfos)
                {
                    if (exercise.Aliases.Any())
                    {
                        int newExerciseId = exerciseIdPairs.First(x => x.oldId == exercise.Id).newId;
                        foreach (var aliasDTO in exercise.Aliases)
                        {
                            ExerciseAlias alias = new()
                            {
                                Alias = aliasDTO.Alias,
                                ExerciseId = newExerciseId
                            };
                            await context.ExerciseAliases.AddAsync(alias);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
            return Ok("Database has been successfully reset.");
        }

        public async Task<string> GetData(string uriExtension = "exercise?limit=10&language=2")
        {
            var clientHandler = new HttpClientHandler
            {
                UseCookies = false,
            };
            var client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://wger.de/api/v2/{uriExtension}"),
                Headers =
                {
                    { "cookie", "sessionid=v561230ypq0cqs5abnkg3qh9ujb7cna0" },
                },
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        #region DTO Definitions
        public class DTORootObject<T>
        {
            public int Count { get; set; }
            public string Next { get; set; } = null!;
            public string Previous { get; set; } = null!;
            public T[] Results { get; set; } = null!;
        }

        public class ResetExerciseDTO
        {
            public int Id { get; set; }
            public string Uuid { get; set; } = null!;
            public string Name { get; set; } = null!;
            public int Exercise_base { get; set; }
            public string Description { get; set; } = null!;
            public DateTime Created { get; set; }
            public int Category { get; set; }
            public int?[] Muscles { get; set; } = null!;
            public int?[] Muscles_secondary { get; set; } = null!;
            public int?[] Equipment { get; set; } = null!;
            public int Language { get; set; }
            public int License { get; set; }
            public string License_author { get; set; } = null!;
            public int?[] Variations { get; set; } = null!;
            public string[] Author_history { get; set; } = null!;
        }
        public class ResetExerciseCategoryDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
        }
        public class ResetExerciseBaseInfoDTO
        {
            public int Id { get; set; }
            public string Uuid { get; set; } = null!;
            public DateTime Created { get; set; }
            public string Creation_date { get; set; } = null!;
            public DateTime Last_update { get; set; }
            public DateTime Last_update_global { get; set; }
            public ResetCategoryDTO Category { get; set; } = null!;
            public ResetMuscleDTO[] Muscles { get; set; } = null!;
            public ResetMuscles_SecondaryDTO[] Muscles_secondary { get; set; } = null!;
            public ResetEquipmentDTO[] Equipment { get; set; } = null!;
            public ResetLicenseDTO License { get; set; } = null!;
            public string License_author { get; set; } = null!;
            public ResetImageDTO[] Images { get; set; } = null!;
            public ResetExerciseFromInfoDTO[] Exercises { get; set; } = null!;
            public int? Variations { get; set; } = null!;
            public ResetVideoDTO[] Videos { get; set; } = null!;
            public string[] Author_history { get; set; } = null!;
            public string[] Total_authors_history { get; set; } = null!;
        }
        public class ResetExerciseFromInfoDTO
        {
            public int Id { get; set; }
            public string Uuid { get; set; } = null!;
            public string Name { get; set; } = null!;
            public int Exercise_base { get; set; }
            public string Description { get; set; } = null!;
            public DateTime Created { get; set; }
            public string Creation_date { get; set; } = null!;
            public int Language { get; set; }
            public ResetAliasDTO[] Aliases { get; set; } = null!;
            public ResetNoteDTO[] Notes { get; set; } = null!;
            public int License { get; set; }
            public string License_title { get; set; } = null!;
            public string License_object_url { get; set; } = null!;
            public string License_author { get; set; } = null!;
            public string License_author_url { get; set; } = null!;
            public string License_derivative_source_url { get; set; } = null!;
            public string[] Author_history { get; set; } = null!;
        }
        public class ResetCategoryDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
        }
        public class ResetMuscleDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Name_en { get; set; } = null!;
            public bool Is_front { get; set; }
            public string Image_url_main { get; set; } = null!;
            public string Image_url_secondary { get; set; } = null!;
        }
        public class ResetMuscles_SecondaryDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Name_en { get; set; } = null!;
            public bool Is_front { get; set; }
            public string Image_url_main { get; set; } = null!;
            public string Image_url_secondary { get; set; } = null!;
        }
        public class ResetEquipmentDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
        }
        public class ResetImageDTO
        {
            public int Id { get; set; }
            public string Uuid { get; set; } = null!;
            public int Exercise_base { get; set; }
            public string Exercise_base_uuid { get; set; } = null!;
            public string Image { get; set; } = null!;
            public bool Is_main { get; set; }
            public string Style { get; set; } = null!;
            public int License { get; set; }
            public string License_title { get; set; } = null!;
            public string License_object_url { get; set; } = null!;
            public string License_author { get; set; } = null!;
            public string License_author_url { get; set; } = null!;
            public string License_derivative_source_url { get; set; } = null!;
            public string[] Author_history { get; set; } = null!;
        }
        public class ResetVideoDTO
        {
            public int Id { get; set; }
            public string Uuid { get; set; } = null!;
            public int Exercise_base { get; set; }
            public string Exercise_base_uuid { get; set; } = null!;
            public string Video { get; set; } = null!;
            public bool Is_main { get; set; }
            public int Size { get; set; }
            public string Duration { get; set; } = null!;
            public int Width { get; set; }
            public int Height { get; set; }
            public string Codec { get; set; } = null!;
            public string Codec_long { get; set; } = null!;
            public int License { get; set; }
            public string License_title { get; set; } = null!;
            public string License_object_url { get; set; } = null!;
            public string License_author { get; set; } = null!;
            public string License_author_url { get; set; } = null!;
            public string License_derivative_source_url { get; set; } = null!;
            public string[] Author_history { get; set; } = null!;
        }
        public class ResetAliasDTO
        {
            public int Id { get; set; }
            public string Alias { get; set; } = null!;
        }
        public class ResetNoteDTO
        {
            public int Id { get; set; }
            public int Exercise { get; set; }
            public string Comment { get; set; } = null!;
        }
        public class ResetLicenseDTO
        {
            public int Id { get; set; }
            public string Full_name { get; set; } = null!;
            public string Short_name { get; set; } = null!;
            public string Url { get; set; } = null!;
        }
        #endregion
    }
}
