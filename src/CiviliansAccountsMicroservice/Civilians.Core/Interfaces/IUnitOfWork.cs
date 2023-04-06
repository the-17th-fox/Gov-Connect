using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civilians.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IPassportsRepository PassportsRepository { get; }
        public ITokensRepository TokensRepository { get; }
        public Task SaveChangesAsync();
    }
}
