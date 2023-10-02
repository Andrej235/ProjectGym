using Microsoft.EntityFrameworkCore;
using ProjectGym.Models;

namespace ProjectGym.Data
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<ExerciseComment> ExerciseComments { get; set; }
        public DbSet<ExerciseImage> ExerciseImages { get; set; }
        public DbSet<ExerciseVideo> ExerciseVideos { get; set; }
        public DbSet<ExerciseNote> ExerciseNotes { get; set; }
        public DbSet<ExerciseAlias> ExerciseAliases { get; set; }
    }
}
