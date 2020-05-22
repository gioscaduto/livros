using Microsoft.AspNetCore.Mvc.Razor;

namespace App.Extensions
{
    public static class RazorExtensions
    {
        public static string FormataValorBooleano(this RazorPage page, bool valor)
        {
            return valor ? "Sim" : "Não";
        }
    }
}
