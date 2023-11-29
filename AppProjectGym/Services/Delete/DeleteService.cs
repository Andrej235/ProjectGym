using AppProjectGym.Information;
using AppProjectGym.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Delete
{
    public class DeleteService(HttpClient client) : IDeleteService
    {
        public async Task<bool> Delete(object id, string endPoint)
        {
            try
            {
                string url = $"{AppInfo.BaseApiURL}/{endPoint}/{Convert.ToString(id)}";
                HttpResponseMessage response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return false;
            }
        }

        public async Task<bool> Delete<T>(T entityToDelete, string endPoint = "") where T : class
        {
            try
            {
                Type type = typeof(T);
                object id = type.GetProperty("Id").GetValue(entityToDelete);
                if (endPoint == "")
                    endPoint = type.Name;

                string url = $"{AppInfo.BaseApiURL}/{endPoint}/{Convert.ToString(id)}";
                HttpResponseMessage response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return false;
            }
        }
    }
}
