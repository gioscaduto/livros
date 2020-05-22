using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace App.Extensions
{
    [HtmlTargetElement("*", Attributes = "disable-by-role")]
    public class DesabilitaLinkTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DesabilitaLinkTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("disable-by-role")]
        public string IdentityRole { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarRoleUsuario(_contextAccessor.HttpContext, IdentityRole);

            if (temAcesso) return;

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão"));
        }
    }
}
