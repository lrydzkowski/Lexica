using Lexica.Core.Models;
using Lexica.EF.Models;
using Lexica.Words.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Services
{
    class NamespaceService : INamespaceService
    {
        public async Task<OperationResult> ChangePath(string oldNamespacePath, string newNamespacePath)
        {
            using (var context = new LexicaContext())
            using (var transaction = context.Database.BeginTransaction())
            {
                List<SetTable> sets = await context.Sets.Where(x => x.Namespace == oldNamespacePath).ToListAsync();
                foreach (SetTable set in sets)
                {
                    set.Namespace = newNamespacePath;
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            return new OperationResult(true);
        }
    }
}
