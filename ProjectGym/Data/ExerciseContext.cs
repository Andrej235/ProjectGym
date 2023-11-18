using Microsoft.EntityFrameworkCore;
using ProjectGym.Models;

namespace ProjectGym.Data
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Alias> Aliases { get; set; }
        public DbSet<PrimaryMuscleGroupInExercise> PrimaryMuscleGroups { get; set; }
        public DbSet<SecondaryMuscleGroupInExercise> SecondaryMuscleGroups { get; set; }
        public DbSet<PrimaryMuscleGroupInExercise> PrimaryMuscles { get; set; }
        public DbSet<SecondaryMuscleGroupInExercise> SecondaryMuscles { get; set; }
        public DbSet<EquipmentUsage> EquipmentUsages { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CommentUpvote> CommentUpvotes { get; set; }
        public DbSet<CommentDownvote> CommentDownvotes { get; set; }
        public DbSet<ExerciseBookmark> ExerciseBookmarks { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Superset> Supersets { get; set; }
        public DbSet<PersonalExerciseWeight> Weights { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutSet> WorkoutSets { get; set; }



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
                .HasMany(e => e.PrimaryMuscleGroups)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleGroupInExercise>(
                    j => j.HasOne<MuscleGroup>().WithMany().HasForeignKey(m => m.MuscleGroupId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscleGroups)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleGroupInExercise>(
                    j => j.HasOne<MuscleGroup>().WithMany().HasForeignKey(m => m.MuscleGroupId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscles)
                .WithMany()
                .UsingEntity<PrimaryMuscleInExercise>(
                    j => j.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscles)
                .WithMany()
                .UsingEntity<SecondaryMuscleInExercise>(
                    j => j.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Equipment)
                .WithMany(e => e.UsedInExercises)
                .UsingEntity<EquipmentUsage>(
                    j => j.HasOne<Equipment>().WithMany().HasForeignKey(e => e.EquipmentId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(ee => ee.Id).ValueGeneratedOnAdd();
                        j.HasKey(ee => ee.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Images)
                .WithOne(i => i.Exercise)
                .HasForeignKey(i => i.ExerciseId)
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

            #region User - Client
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserGUID)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Weight
            modelBuilder.Entity<User>()
                .HasMany(u => u.Weights)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PersonalExerciseWeight>()
                .HasOne(w => w.Exercise)
                .WithMany()
                .HasForeignKey(w => w.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Workouts
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedWorkouts)
                .WithOne(w => w.Creator)
                .HasForeignKey(w => w.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.WorkoutSets)
                .WithOne(ws => ws.Workout)
                .HasForeignKey(ws => ws.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkoutSet>()
                .HasOne(ws => ws.Set)
                .WithMany()
                .HasForeignKey(ws => ws.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutSet>()
                .HasOne(ws => ws.Superset)
                .WithMany()
                .HasForeignKey(ws => ws.SuperSetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Superset>()
                .HasOne(ss => ss.Set)
                .WithMany()
                .HasForeignKey(ss => ss.SetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Set>()
                .HasOne(s => s.Creator)
                .WithMany(u => u.CreatedExerciseSets)
                .HasForeignKey(s => s.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Set>()
                .HasOne(s => s.Exercise)
                .WithMany()
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Comments and bookmarks
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Creator)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CommentUpvotes)
                .WithMany(c => c.Upvotes)
                .UsingEntity<CommentUpvote>(
                    j => j.HasOne<Comment>().WithMany().HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<User>().WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction),
                    j =>
                    {
                        j.Property(j => j.Id).ValueGeneratedOnAdd();
                        j.HasKey(j => j.Id);
                    }
                );

            modelBuilder.Entity<User>()
                .HasMany(u => u.CommentDownvotes)
                .WithMany(c => c.Downvotes)
                .UsingEntity<CommentDownvote>(
                    j => j.HasOne<Comment>().WithMany().HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<User>().WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction),
                    j =>
                    {
                        j.Property(x => x.Id).ValueGeneratedOnAdd();
                        j.HasKey(x => x.Id);
                    }
                );

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookmarks)
                .WithMany()
                .UsingEntity<ExerciseBookmark>(
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<User>().WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(x => x.Id).ValueGeneratedOnAdd();
                        j.HasKey(x => x.Id);
                    }
                );
            #endregion
        }
    }
}
