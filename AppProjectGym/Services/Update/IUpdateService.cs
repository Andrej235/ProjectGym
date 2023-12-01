namespace AppProjectGym.Services.Update
{
    public interface IUpdateService 
    {
        Task<bool> Update<T>(T updatedEntity, string endPoint = "") where T : class;
    }
}
