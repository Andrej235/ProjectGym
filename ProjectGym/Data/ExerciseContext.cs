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

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectGymDB;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            #region Exercise
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.VariationExercises)
                .WithMany(e => e.IsVariationOf)
                .UsingEntity<Variation>(
                    v => v.HasOne<Exercise>().WithMany().HasForeignKey(v => v.Exercise1Id).OnDelete(DeleteBehavior.NoAction),
                    v => v.HasOne<Exercise>().WithMany().HasForeignKey(v => v.Exercise2Id).OnDelete(DeleteBehavior.NoAction),
                    v =>
                    {
                        v.Property(v => v.Id).ValueGeneratedOnAdd();
                        v.HasKey(v => v.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Exercises)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscles)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleExerciseConnection>(
                    me => me.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.NoAction),
                    me => me.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.NoAction),
                    me =>
                    {
                        me.Property(me => me.Id).ValueGeneratedOnAdd();
                        me.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscles)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleExerciseConnection>(
                    me => me.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.NoAction),
                    me => me.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.NoAction),
                    me =>
                    {
                        me.Property(me => me.Id).ValueGeneratedOnAdd();
                        me.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Equipment)
                .WithMany(e => e.UsedInExercises)
                .UsingEntity<EquipmentExerciseUsage>(
                    ee => ee.HasOne<Equipment>().WithMany().HasForeignKey(e => e.EquipmentId).OnDelete(DeleteBehavior.NoAction),
                    ee => ee.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.NoAction),
                    ee =>
                    {
                        ee.Property(ee => ee.Id).ValueGeneratedOnAdd();
                        ee.HasKey(ee => ee.Id);
                    }
                );

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
            #endregion

            #region User
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserGUID)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }

        public class PrimaryMuscleExerciseConnection
        {
            public int Id { get; set; }
            public int ExerciseId { get; set; }
            public int MuscleId { get; set; }
        }

        public class SecondaryMuscleExerciseConnection
        {
            public int Id { get; set; }
            public int ExerciseId { get; set; }
            public int MuscleId { get; set; }
        }

        public class EquipmentExerciseUsage
        {
            public int Id { get; set; }
            public int ExerciseId { get; set; }
            public int EquipmentId { get; set; }
        }

        public class Variation
        {
            public int Id { get; set; }
            public int Exercise1Id { get; set; }
            public int Exercise2Id { get; set; }
        }
    }
}
