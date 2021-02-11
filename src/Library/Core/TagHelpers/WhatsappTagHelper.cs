using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Core.TagHelpers
{
    [HtmlTargetElement("whatsapp")]
    public class WhatsappTagHelper : BaseTagHelper
    {
        public string Number { get; set; }
        public string Message { get; set; }
        public bool NumberShowContent { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var value = "https://wa.me/9" + Number + (Message.IsNullOrEmptyWhiteSpace() ? "" : "?text=" + Message);
            output.Attributes.SetAttribute("href", value);
            output.Attributes.SetAttribute("target", "_blank");
            output.PreContent.AppendHtml("&nbsp;<i class='fab fa-whatsapp'></i>&nbsp;");

            if (NumberShowContent)
                output.Content.SetContent(Number);
        }
    }
}
