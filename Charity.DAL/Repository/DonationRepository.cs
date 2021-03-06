/*
 *  File:   DonationRepository.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Charity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Charity.DAL.Repository
{
    public class DonationRepository : RepositoryBase, IRepository<DonationEntity>
    {
        public DonationRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public DonationEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<DonationEntity> query = dbContext.Set<DonationEntity>();
            query = query.Include(entity => entity.Shelter)
                .Include(entity => entity.Transactions).ThenInclude(t => t.Volunteer);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<DonationEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            IQueryable<DonationEntity> query = dbContext.Set<DonationEntity>();
            query = query.Include(entity => entity.Shelter);

            return query.ToArray();
        }

        public DonationEntity Insert(DonationEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Donations.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public DonationEntity Update(DonationEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Donations.Any(p => p.Id == entity.Id))
            {
                entity.Shelter = dbContext.Shelters.FirstOrDefault(s => s.Id == entity.ShelterId);
                dbContext.Donations.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Donations.Any(p => p.Id == id))
            {
                var entity = new DonationEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
