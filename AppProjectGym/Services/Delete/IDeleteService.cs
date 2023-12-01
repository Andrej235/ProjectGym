namespace AppProjectGym.Services.Delete
{
    public interface IDeleteService
    {
        Task<bool> Delete(object id, string endPoint);
        Task<bool> Delete<T>(T entityToDelete, string endPoint = "") where T : class;
    }
}
