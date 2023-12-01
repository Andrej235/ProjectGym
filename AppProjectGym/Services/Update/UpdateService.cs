using AppProjectGym.Information;
using AppProjectGym.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Update
{
    public class UpdateService(HttpClient client) : IUpdateService
    {
        public async Task<bool> Update<T>(T updatedEntity, string endPoint = "") where T : class
        {
			try
			{
                if (endPoint == "") 
                    endPoint = typeof(T).FullName;

                string url = AppInfo.BaseApiURL + "/" + endPoint;
                await client.PutAsync(url, new StringContent(JsonSerializer.Serialize(updatedEntity)));
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
