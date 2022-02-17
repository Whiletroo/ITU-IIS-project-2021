/*
 *  File:   ShelterAdminRepository.cs
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
    public class ShelterAdminRepository : RepositoryBase, IRepository<ShelterAdminEntity>
    {
        public ShelterAdminRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public ShelterAdminEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<ShelterAdminEntity> query = dbContext.Set<ShelterAdminEntity>();
            query = query.Include(entity => entity.Shelter);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<ShelterAdminEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<ShelterAdminEntity> query = dbContext.Set<ShelterAdminEntity>();
            query = query.Include(entity => entity.Shelter);

            return query.ToArray();
        }

        public ShelterAdminEntity Insert(ShelterAdminEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.ShelterAdmins.Any(sa => sa.Email == entity.Email))
                return null;
            var shelter = dbContext.Shelters.FirstOrDefault(s => s.Id == entity.ShelterId);
            entity.Shelter = shelter;
            dbContext.ShelterAdmins.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public ShelterAdminEntity Update(ShelterAdminEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.ShelterAdmins.Any(p => p.Id == entity.Id))
            {
                entity.Shelter = dbContext.Shelters.FirstOrDefault(s => s.Id == entity.ShelterId);
                dbContext.ShelterAdmins.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.ShelterAdmins.Any(p => p.Id == id))
            {
                var entity = new ShelterAdminEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
