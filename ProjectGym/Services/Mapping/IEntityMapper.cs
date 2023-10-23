namespace ProjectGym.Services.Mapping
{
    public interface IEntityMapper<TEntity, TDTO>
    {
        TDTO MapEntity(TEntity entity);
    }
}
