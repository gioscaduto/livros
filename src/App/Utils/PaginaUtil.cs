using Microsoft.Extensions.Configuration;
using System;

namespace App.Utils
{
    public class PaginaUtil
    {
        public int QtdItensPagina { get; }

        public PaginaUtil(IConfiguration configuration)
        {
            var qtdItensPagina = Convert.ToInt32(configuration.GetSection("QtdItensPagina").Value);

            if (qtdItensPagina <= 0) qtdItensPagina = 25;

            QtdItensPagina = qtdItensPagina;
        }
    }
}
