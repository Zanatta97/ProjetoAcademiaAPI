using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Common
{
    public sealed class PaginationParameters
    {
        private const int MaxPageSize = 100;

        /// <summary>
        /// Número máximo de páginas permitido, definido como int.MaxValue / MaxPageSize
        /// para evitar estouro de memória ao calcular o número total de páginas.
        /// </summary>
        private const int MaxPage = int.MaxValue / MaxPageSize;

        private int _page = 1;
        private int _pageSize = 20;

        public int Page
        {
            get => _page;
            set => _page = Math.Clamp(value, 1, MaxPage); // Garante que o valor da página esteja entre 1 e MaxPage
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, MaxPageSize); // Garante que o valor do tamanho da página esteja entre 1 e MaxPageSize
        }
    }
}
