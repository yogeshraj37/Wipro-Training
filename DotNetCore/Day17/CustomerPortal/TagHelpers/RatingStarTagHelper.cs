using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace CustomerPortal.TagHelpers
{
    [HtmlTargetElement("rating-stars")]
    public class RatingStarTagHelper : TagHelper
    {
        public int Count { get; set; } = 5;

        public override void Process(TagHelperContext context,
            TagHelperOutput output)
        {
            output.TagName = "div";

            StringBuilder sb = new();

            for (int i = 1; i <= Count; i++)
            {
                sb.Append($"<span style='font-size:30px;color:gold'>★</span>");
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}