using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CampusLifePlanner.WebUI.Helpers
{
    [HtmlTargetElement("layout-container")]
    public class IndexHelper : TagHelper
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string[] Columns { get; set; }
        public string Button { get; set; }
        public string HeaderClass { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "layout-container");

            var headerHtml = $"<h1 class=\"{HeaderClass}\">{Title}</h1><a class=\"btn btn-outline-primary fp-add\">{Button}</a>";
            output.Content.AppendHtml(headerHtml);

            output.Content.AppendHtml("<div class=\"content\">");
            output.Content.AppendHtml(output.GetChildContentAsync().Result.GetContent());
            output.Content.AppendHtml("</div>");
        }

    }
}
