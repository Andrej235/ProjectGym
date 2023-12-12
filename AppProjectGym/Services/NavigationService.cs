using AppProjectGym.Pages;
using AppProjectGym.Utilities;

namespace AppProjectGym.Services
{
    public class NavigationService
    {
        public static async Task GoToAsync(ShellNavigationState state, params KeyValuePair<string, object>[] navigationParameters)
        {
            try
            {
                Dictionary<string, object> navigationParameter = [];
                foreach (var param in navigationParameters)
                    navigationParameter.Add(param.Key, param.Value);

                await Shell.Current.GoToAsync(state, navigationParameter);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
            }
        }

        public static async Task GoToAsync(Type page, params KeyValuePair<string, object>[] navigationParameters) => await GoToAsync(page.Name, navigationParameters);

        public static async Task GoToAsync(ShellNavigationState state, Dictionary<string, object> navigationParameters) => await Shell.Current.GoToAsync(state, navigationParameters);

        public static async Task GoToAsync(Type page, Dictionary<string, object> navigationParameters) => await Shell.Current.GoToAsync(page.Name, navigationParameters);

        public static async Task GoToAsync(ShellNavigationState state) => await Shell.Current.GoToAsync(state);

        public static async Task GoToAsync(Type page) => await Shell.Current.GoToAsync(page.Name);

        public static async Task SearchAsync(params string[] queryPairs) => await GoToAsync(nameof(SearchResultsPage), new KeyValuePair<string, object>("q", queryPairs.Length > 1 || !queryPairs.Any(x => x.Contains("strict=")) ? string.Join(';', queryPairs) : ""));

        public static async Task SearchAsync(bool isInSelectionMode, params string[] queryPairs) => await GoToAsync(nameof(SearchResultsPage), new KeyValuePair<string, object>("q", queryPairs.Length > 1 || !queryPairs.Any(x => x.Contains("strict=")) ? string.Join(';', queryPairs) : ""), new KeyValuePair<string, object>("selectionMode", isInSelectionMode));
    }
}
