namespace AppProjectGym.Services.Read
{
    public interface IReadService
    {
        Task<T> Get<T>(string include = "none", string endPoint = "", params string[] query) where T : class;
    }
}