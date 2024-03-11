using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace BoostifySolution.Global
{
    public static class HTML
    {
        public static string NavigationActive(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass="active")
        {
            string currentAction = (htmlHelper.ViewContext.RouteData.Values["action"] as string).ToLower();
            string currentController = (htmlHelper.ViewContext.RouteData.Values["controller"] as string).ToLower();

            IEnumerable<string> acceptedActions = (actions.ToLower() ?? currentAction.ToLower()).Split(',');
            IEnumerable<string> acceptedControllers = (controllers.ToLower() ?? currentController.ToLower()).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : String.Empty;
        }
    }

    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public static class FileExtensions
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}