namespace ProjectGym
{

    public class ExerciseRootobjectDTO
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public ExerciseDTO[] results { get; set; }
    }

    public class ExerciseDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public int exercise_base { get; set; }
        public string description { get; set; }
        public DateTime created { get; set; }
        public int category { get; set; }
        public int?[] muscles { get; set; }
        public int?[] muscles_secondary { get; set; }
        public int?[] equipment { get; set; }
        public int language { get; set; }
        public int license { get; set; }
        public string license_author { get; set; }
        public int?[] variations { get; set; }
        public string[] author_history { get; set; }
    }


    public class ExerciseCategoryRootobjectDTO
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ExerciseCategoryDTO[] results { get; set; }
    }

    public class ExerciseCategoryDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class ExerciseCommentRootobjectDTO
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public ExerciseCommentDTO[] results { get; set; }
    }

    public class ExerciseCommentDTO
    {
        public int id { get; set; }
        public int exercise { get; set; }
        public string comment { get; set; }
    }


    public class ExerciseImageRootobjectDTO
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public ExerciseImageDTO[] results { get; set; }
    }

    public class ExerciseImageDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public int exercise_base { get; set; }
        public string exercise_base_uuid { get; set; }
        public string image { get; set; }
        public bool is_main { get; set; }
        public string style { get; set; }
        public int license { get; set; }
        public string license_title { get; set; }
        public string license_object_url { get; set; }
        public string license_author { get; set; }
        public string license_author_url { get; set; }
        public string license_derivative_source_url { get; set; }
        public object[] author_history { get; set; }
    }


    public class ExerciseBaseInfoRootobjectDTO
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public ExerciseBaseInfoDTO[] results { get; set; }
    }

    public class ExerciseBaseInfoDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public DateTime created { get; set; }
        public string creation_date { get; set; }
        public DateTime last_update { get; set; }
        public DateTime last_update_global { get; set; }
        public CategoryDTO category { get; set; }
        public MuscleDTO[] muscles { get; set; }
        public Muscles_SecondaryDTO[] muscles_secondary { get; set; }
        public EquipmentDTO[] equipment { get; set; }
        public LicenseDTO license { get; set; }
        public string license_author { get; set; }
        public ImageDTO[] images { get; set; }
        public ExercisDTO[] exercises { get; set; }
        public int? variations { get; set; }
        public VideoDTO[] videos { get; set; }
        public string[] author_history { get; set; }
        public string[] total_authors_history { get; set; }
    }

    public class CategoryDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class LicenseDTO
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string short_name { get; set; }
        public string url { get; set; }
    }

    public class MuscleDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public bool is_front { get; set; }
        public string image_url_main { get; set; }
        public string image_url_secondary { get; set; }
    }

    public class Muscles_SecondaryDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_en { get; set; }
        public bool is_front { get; set; }
        public string image_url_main { get; set; }
        public string image_url_secondary { get; set; }
    }

    public class EquipmentDTO
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ImageDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public int exercise_base { get; set; }
        public string exercise_base_uuid { get; set; }
        public string image { get; set; }
        public bool is_main { get; set; }
        public string style { get; set; }
        public int license { get; set; }
        public string license_title { get; set; }
        public string license_object_url { get; set; }
        public string license_author { get; set; }
        public string license_author_url { get; set; }
        public string license_derivative_source_url { get; set; }
        public string[] author_history { get; set; }
    }

    public class ExercisDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public int exercise_base { get; set; }
        public string description { get; set; }
        public DateTime created { get; set; }
        public string creation_date { get; set; }
        public int language { get; set; }
        public AliasDTO[] aliases { get; set; }
        public NoteDTO[] notes { get; set; }
        public int license { get; set; }
        public string license_title { get; set; }
        public string license_object_url { get; set; }
        public string license_author { get; set; }
        public string license_author_url { get; set; }
        public string license_derivative_source_url { get; set; }
        public string[] author_history { get; set; }
    }

    public class AliasDTO
    {
        public int id { get; set; }
        public string alias { get; set; }
    }

    public class NoteDTO
    {
        public int id { get; set; }
        public int exercise { get; set; }
        public string comment { get; set; }
    }

    public class VideoDTO
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public int exercise_base { get; set; }
        public string exercise_base_uuid { get; set; }
        public string video { get; set; }
        public bool is_main { get; set; }
        public int size { get; set; }
        public string duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string codec { get; set; }
        public string codec_long { get; set; }
        public int license { get; set; }
        public string license_title { get; set; }
        public string license_object_url { get; set; }
        public string license_author { get; set; }
        public string license_author_url { get; set; }
        public string license_derivative_source_url { get; set; }
        public string[] author_history { get; set; }
    }
}
