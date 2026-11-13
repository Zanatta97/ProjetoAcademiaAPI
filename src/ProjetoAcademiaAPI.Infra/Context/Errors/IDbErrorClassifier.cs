using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Infra.Context.Errors
{
    public interface IDbErrorClassifier
    {
        bool IsUniqueVaiolation(DbUpdateException ex);
    }
}
