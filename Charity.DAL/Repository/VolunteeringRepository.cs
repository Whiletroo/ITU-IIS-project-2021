/*
 *  File:   VolunteeringRepository.cs
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
    public class VolunteeringRepository : RepositoryBase, IRepository<VolunteeringEntity>
    {
        public VolunteeringRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public VolunteeringEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<VolunteeringEntity> query = dbContext.Set<VolunteeringEntity>();
            query = query.Include(entity => entity.Shelter)
                .Include(entity => entity.Enrollments).ThenInclude(t => t.Volunteer);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<VolunteeringEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Volunteerings.ToArray();
        }

        public VolunteeringEntity Insert(VolunteeringEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var shelter = dbContext.Shelters.FirstOrDefault(s => s.Id == entity.Id);
            if (shelter is not null)
            {
                entity.Shelter = shelter;
            }

            dbContext.Volunteerings.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public VolunteeringEntity Update(VolunteeringEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Volunteerings.Any(p => p.Id == entity.Id))
            {
                var shelter = dbContext.Shelters.FirstOrDefault(s => s.Id == entity.Id);
                if (shelter is not null)
                {
                    entity.Shelter = shelter;
                }
                dbContext.Volunteerings.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Volunteerings.Any(p => p.Id == id))
            {
                var entity = new VolunteeringEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
