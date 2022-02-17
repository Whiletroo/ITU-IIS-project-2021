using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL.Repository
{
    public class AdminRepository : RepositoryBase, IRepository<AdminEntity>
    {
        public AdminRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public AdminEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<AdminEntity> query = dbContext.Set<AdminEntity>();
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<AdminEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Admins.ToArray();
        }

        public AdminEntity Insert(AdminEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Admins.Any(a => a.Email == entity.Email))
                return null;
            dbContext.Admins.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public AdminEntity Update(AdminEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Admins.Any(p => p.Id == entity.Id))
            {
                dbContext.Admins.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Admins.Any(p => p.Id == id))
            {
                var entity = new AdminEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
