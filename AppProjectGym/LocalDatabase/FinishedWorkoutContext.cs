using AppProjectGym.LocalDatabase.Models;
using LocalJSONDatabase.Core;
using LocalJSONDatabase.Services.ModelBuilder;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace AppProjectGym.LocalDatabase
{
    public class FinishedWorkoutContext(ModelBuilder modelBuilder) : DBContext(modelBuilder)
    {
        public static FinishedWorkoutContext Context { get; private set; }
        public override Task Initialize()
        {
            Context = this;
            dbDirPath = FileSystem.AppDataDirectory;
            return base.Initialize();
        }

        public DBTable<FinishedSet> FinishedSets { get; set; } = null!;
        public DBTable<FinishedWorkoutSet> FinishedWorkoutSets { get; set; } = null!;
        public DBTable<FinishedWorkout> FinishedWorkouts { get; set; } = null!;

        private string dbDirPath;
        protected override string DBDirectoryPath => dbDirPath;

        protected override void OnConfiguring(ModelBuilder modelBuilder)
        {
            modelBuilder.Model<FinishedSet>()
                .HasOne(x => x.WorkoutSet)
                .WithOne(x => x.Set);

            modelBuilder.Model<FinishedWorkoutSet>()
                .HasOne(x => x.Set)
                .WithOne(x => x.WorkoutSet);

            modelBuilder.Model<FinishedWorkoutSet>()
                .HasOne(x => x.Workout)
                .WithMany(x => x.WorkoutSets);
        }
    }
}
