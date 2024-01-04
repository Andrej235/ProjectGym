﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectGym.Data;

#nullable disable

namespace ProjectGym.Migrations
{
    [DbContext(typeof(ExerciseContext))]
    partial class ExerciseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectGym.Models.Alias", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AliasName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Aliases");
                });

            modelBuilder.Entity("ProjectGym.Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserGUID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserGUID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ProjectGym.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("ProjectGym.Models.EquipmentUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("EquipmentUsages");
                });

            modelBuilder.Entity("ProjectGym.Models.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("ProjectGym.Models.ExerciseBookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("ExerciseBookmarks");
                });

            modelBuilder.Entity("ProjectGym.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ProjectGym.Models.Muscle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MuscleGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MuscleGroupId");

                    b.ToTable("Muscles");
                });

            modelBuilder.Entity("ProjectGym.Models.MuscleGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MuscleGroups");
                });

            modelBuilder.Entity("ProjectGym.Models.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<string>("NoteText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("ProjectGym.Models.PersonalExerciseWeight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfAchieving")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("Weights");
                });

            modelBuilder.Entity("ProjectGym.Models.PrimaryMuscleGroupInExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("MuscleGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("MuscleGroupId");

                    b.ToTable("PrimaryMuscleGroups");
                });

            modelBuilder.Entity("ProjectGym.Models.PrimaryMuscleInExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("MuscleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("MuscleId");

                    b.ToTable("PrimaryMuscles");
                });

            modelBuilder.Entity("ProjectGym.Models.SecondaryMuscleGroupInExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("MuscleGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("MuscleGroupId");

                    b.ToTable("SecondaryMuscleGroups");
                });

            modelBuilder.Entity("ProjectGym.Models.SecondaryMuscleInExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("MuscleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("MuscleId");

                    b.ToTable("SecondaryMuscles");
                });

            modelBuilder.Entity("ProjectGym.Models.Set", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("DropSet")
                        .HasColumnType("bit");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("RepRange_Bottom")
                        .HasColumnType("int");

                    b.Property<int>("RepRange_Top")
                        .HasColumnType("int");

                    b.Property<bool>("ToFaliure")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("ProjectGym.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectGym.Models.Workout", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("ProjectGym.Models.WorkoutSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TargetSets")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.HasIndex("WorkoutId");

                    b.ToTable("WorkoutSets");
                });

            modelBuilder.Entity("ProjectGym.Models.Alias", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", "Exercise")
                        .WithMany("Aliases")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("ProjectGym.Models.Client", b =>
                {
                    b.HasOne("ProjectGym.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGUID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectGym.Models.EquipmentUsage", b =>
                {
                    b.HasOne("ProjectGym.Models.Equipment", null)
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.ExerciseBookmark", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.Image", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", "Exercise")
                        .WithMany("Images")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("ProjectGym.Models.Muscle", b =>
                {
                    b.HasOne("ProjectGym.Models.MuscleGroup", "MuscleGroup")
                        .WithMany("Muscles")
                        .HasForeignKey("MuscleGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MuscleGroup");
                });

            modelBuilder.Entity("ProjectGym.Models.Note", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", "Exercise")
                        .WithMany("Notes")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("ProjectGym.Models.PersonalExerciseWeight", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.User", "User")
                        .WithMany("Weights")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectGym.Models.PrimaryMuscleGroupInExercise", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.MuscleGroup", null)
                        .WithMany()
                        .HasForeignKey("MuscleGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.PrimaryMuscleInExercise", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.Muscle", null)
                        .WithMany()
                        .HasForeignKey("MuscleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.SecondaryMuscleGroupInExercise", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.MuscleGroup", null)
                        .WithMany()
                        .HasForeignKey("MuscleGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.SecondaryMuscleInExercise", b =>
                {
                    b.HasOne("ProjectGym.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.Muscle", null)
                        .WithMany()
                        .HasForeignKey("MuscleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectGym.Models.Set", b =>
                {
                    b.HasOne("ProjectGym.Models.User", "Creator")
                        .WithMany("CreatedExerciseSets")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("ProjectGym.Models.Workout", b =>
                {
                    b.HasOne("ProjectGym.Models.User", "Creator")
                        .WithMany("CreatedWorkouts")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ProjectGym.Models.WorkoutSet", b =>
                {
                    b.HasOne("ProjectGym.Models.Set", "Set")
                        .WithMany()
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectGym.Models.Workout", "Workout")
                        .WithMany("WorkoutSets")
                        .HasForeignKey("WorkoutId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Set");

                    b.Navigation("Workout");
                });

            modelBuilder.Entity("ProjectGym.Models.Exercise", b =>
                {
                    b.Navigation("Aliases");

                    b.Navigation("Images");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("ProjectGym.Models.MuscleGroup", b =>
                {
                    b.Navigation("Muscles");
                });

            modelBuilder.Entity("ProjectGym.Models.User", b =>
                {
                    b.Navigation("CreatedExerciseSets");

                    b.Navigation("CreatedWorkouts");

                    b.Navigation("Weights");
                });

            modelBuilder.Entity("ProjectGym.Models.Workout", b =>
                {
                    b.Navigation("WorkoutSets");
                });
#pragma warning restore 612, 618
        }
    }
}
