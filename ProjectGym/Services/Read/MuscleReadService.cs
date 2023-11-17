﻿using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class MuscleReadService : AbstractReadService<Muscle, int>
    {
        private readonly ExerciseContext context;
        public MuscleReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<Muscle, int> PrimaryKey => m => m.Id;

        protected override IQueryable<Muscle> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Muscle> musclesIncluding = context.Muscles.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return musclesIncluding;

            if (include.Contains("all") || include.Contains("musclegroup"))
            {
                return musclesIncluding
                    .Include(x => x.MuscleGroup);
            }

            return musclesIncluding;
        }

        protected override Expression<Func<Muscle, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return m => m.Name.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
