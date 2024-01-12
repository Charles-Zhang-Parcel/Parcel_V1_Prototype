using System.Reflection;
using Parcel.Shared;

namespace Parcel.WebHost.Utils
{
    internal static class WebHostEndpoints
    {
        #region Accesor
        private static WebHostRuntime Runtime => WebHostRuntime.Singleton;
        #endregion

        #region Endpoints
        // public static async Task EndpointGetItems(HttpContext context)
        // {
        //     string tempalte = GetTemplate("Somewhere2.WebHost.RazorTemplates.GetItemsTemplate.cshtml");
        //     GetItemsTemplateModel model = new GetItemsTemplateModel()
        //     {
        //         Items = Runtime.AllItems.ToList()
        //     };
        //     string html = Engine.Razor.RunCompile(tempalte, "GetItems", typeof(GetItemsTemplateModel), model);
        //     
        //     await context.Response.WriteAsync(html);
        // }
        // public static async Task EndpointGetNotes(HttpContext context)
        // {
        //     string tempalte = GetTemplate("Somewhere2.WebHost.RazorTemplates.GetNotesTemplate.cshtml");
        //     GetNotesTemplateModel model = new GetNotesTemplateModel()
        //     {
        //         Items = Runtime.AllItems.ToList()
        //     };
        //     string html = Engine.Razor.RunCompile(tempalte, "GetNotes", typeof(GetNotesTemplateModel), model);
        //     
        //     await context.Response.WriteAsync(html);
        // }
        #endregion

        #region Routines

        private static string GetTemplate(string templateURI)
            => Helpers.ReadTextResource(templateURI.EndsWith(".ignore") ? templateURI : templateURI + ".ignore");
        #endregion
    }
}