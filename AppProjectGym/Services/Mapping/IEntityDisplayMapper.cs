namespace AppProjectGym.Services.Mapping
{
    public interface IEntityDisplayMapper<in TEntity, TDisplay> where TEntity : class where TDisplay : class
    {
        public Task<TDisplay> Map(TEntity entity);
    }
}
