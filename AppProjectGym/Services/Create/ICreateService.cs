namespace AppProjectGym.Services.Create
{
    public interface ICreateService
    {
        Task<string> Add<T>(T entityToAdd, string endPoint = "");
    }
}
