namespace AppProjectGym.Services.Read
{
    public interface IEntityReadService<T> where T : class
    {
        Task<T> Get(string endPoint, string include = "all", params string[] query);
    }
}
