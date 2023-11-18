using ProjectGym.DTOs;
using ProjectGym.Models;

namespace ProjectGym.Services.Mapping
{
    public class UserMapper : IEntityMapper<User, UserDTO>
    {
        public UserDTO Map(User entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            WeightIds = entity.Weights.Select(x => x.Id),
            CreatedWorkoutIds = entity.CreatedWorkouts.Select(x => x.Id),
            CreatedExerciseSetIds = entity.CreatedExerciseSets.Select(x => x.Id),
            CommentIds = entity.Comments.Select(x => x.Id),
            CommentUpvoteIds = entity.CommentUpvotes.Select(x => x.Id),
            CommentDownvoteIds = entity.CommentDownvotes.Select(x => x.Id),
            BookmarkIds = entity.Bookmarks.Select(x => x.Id),
        };
    }
}
