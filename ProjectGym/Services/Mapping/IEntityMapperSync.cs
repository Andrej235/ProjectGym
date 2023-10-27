namespace ProjectGym.Services.Mapping
{
    public interface IEntityMapperSync<TEntity, TDTO> : IEntityMapper<TEntity, TDTO>
    {
        TEntity Map(TDTO dto);
    }
}
