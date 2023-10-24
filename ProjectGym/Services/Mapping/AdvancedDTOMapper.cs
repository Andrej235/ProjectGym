using ProjectGym.Controllers;

namespace ProjectGym.Services.Mapping
{
    public class AdvancedDTOMapper
    {
        public static AdvancedDTO<T> TranslateToAdvancedDTO<T>(List<T> values, string baseAPIUrl, int offset, int limit)
        {
            AdvancedDTO<T> res = new()
            {
                BatchSize = values.Count,
                PreviousBatchURLExtension = null,
                NextBatchURLExtension = null,
                Values = values,
            };

            if (limit >= 0 && values.Count >= limit)
            {
                var nextOffset = offset + limit;
                res.NextBatchURLExtension = $"{baseAPIUrl}&offset={nextOffset}&limit={limit}";
            }

            if (offset > 0)
            {
                var previousOffset = offset - limit;
                if (previousOffset < 0)
                    previousOffset = 0;

                res.PreviousBatchURLExtension = $"{baseAPIUrl}&offset={previousOffset}&limit={limit}";
            }

            return res;
        }
    }
}
