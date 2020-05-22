﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace App.Extensions
{
    public class EmailTagHelper : TagHelper
    {
        public string EmailDomain { get; set; } = "livraria.com";

        public override async Task ProcessAsync
            (TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent() + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + target);
            output.Content.SetContent(target);
        }
    }
}
