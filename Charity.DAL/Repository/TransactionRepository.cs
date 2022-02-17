//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL.Repository
{
    public class TransactionRepository : RepositoryBase, IRepository<TransactionEntity>
    {
        public TransactionRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public TransactionEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<TransactionEntity> query = dbContext.Set<TransactionEntity>();
            query = query.Include(trans => trans.Donation).ThenInclude(don => don.Shelter);
            query = query.Include(trans => trans.Volunteer).ThenInclude(vol => vol.Transactions);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public TransactionEntity Insert(TransactionEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            entity.Volunteer = dbContext.Volunteers.FirstOrDefault(v => v.Id.Equals(entity.VolunteerId));
            entity.Donation = dbContext.Donations.FirstOrDefault(d => d.Id.Equals(entity.DonationId));
            dbContext.Transactions.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public IEnumerable<TransactionEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Transactions.Include(t => t.Volunteer).Include(t => t.Donation).ToArray();
        }

        public TransactionEntity Update(TransactionEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Transactions.Any(p => p.Id == entity.Id))
            {
                dbContext.Transactions.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var entity = dbContext.Transactions.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
