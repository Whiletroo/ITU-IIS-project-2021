using System;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly IDbContextFactory<CharityDbContext> _dbContextFactory;
        public RepositoryBase(IDbContextFactory<CharityDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
    }
}
