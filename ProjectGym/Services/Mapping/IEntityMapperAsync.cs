namespace ProjectGym.Services.Mapping
{
    public interface IEntityMapperAsync<TEntity, TDTO> : IEntityMapper<TEntity, TDTO>
    {
        Task<TEntity> Map(TDTO dto);
    }
}
