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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StableDiffusionDB;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*            modelBuilder.Entity<Exercise>()
                            .HasMany(e => e.VariationExercise)
                            .WithMany(e => e.VariationExercise)
                            .UsingEntity*/ //TODO: Add using entity, idk if this will even work

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Exercises)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Muscles)
                .WithMany(m => m.PrimaryInExercises); //TODO: Add using entity

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.MusclesSecondary)
                .WithMany(m => m.SecondaryInExercises); //TODO: Add using entity

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Equipment)
                .WithMany(e => e.UsedInExercises); //TODO: Add using entity

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Images)
                .WithOne(i => i.Exercise)
                .HasForeignKey(i => i.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Videos)
                .WithOne(v => v.Exercise)
                .HasForeignKey(v => v.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Comments)
                .WithOne(c => c.Exercise)
                .HasForeignKey(c => c.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Notes)
                .WithOne(n => n.Exercise)
                .HasForeignKey(n => n.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Aliases)
                .WithOne(a => a.Exercise)
                .HasForeignKey(a => a.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
