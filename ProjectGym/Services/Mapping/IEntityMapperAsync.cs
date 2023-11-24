namespace ProjectGym.Services.Mapping
{
    public interface IEntityMapperAsync<TEntity, TDTO> : IEntityMapper<TEntity, TDTO>
    {
        Task<TEntity> MapAsync(TDTO dto);
    }
}
