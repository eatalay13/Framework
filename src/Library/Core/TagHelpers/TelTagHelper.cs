using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Core.TagHelpers
{
    [HtmlTargetElement("tel")]
    public class TelTagHelper : TagHelper
    {
        public string Number { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "tel:" + Number);
            output.Attributes.SetAttribute("target", "_blank");
            output.PreContent.AppendHtml("&nbsp;<i class='fas fa-phone-alt'></i>&nbsp;");
            output.Content.SetContent(Number);
        }
    }
}
