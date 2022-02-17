/*
 *  File:   ShelterRepository.cs
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
    public class ShelterRepository : RepositoryBase, IRepository<ShelterEntity>
    {
        public ShelterRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public ShelterEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<ShelterEntity> query = dbContext.Set<ShelterEntity>();
            query = query.Include(entity => entity.Admin)
                .Include(entity => entity.Donations)
                .Include(entity => entity.Volunteerings);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<ShelterEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Shelters.ToArray();
        }

        public ShelterEntity Insert(ShelterEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            entity.Admin = dbContext.ShelterAdmins.FirstOrDefault(sa => sa.Id == entity.AdminId);
            dbContext.Shelters.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public ShelterEntity Update(ShelterEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Shelters.Any(p => p.Id == entity.Id))
            {
                entity.Admin = dbContext.ShelterAdmins.FirstOrDefault(sa => sa.Id == entity.AdminId);
                dbContext.Shelters.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Shelters.Any(p => p.Id == id))
            {
                var entity = new ShelterEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
