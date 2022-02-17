/*
 *  File:   VolunteerRepository.cs
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
    public class VolunteerRepository : RepositoryBase, IRepository<VolunteerEntity>
    {
        public VolunteerRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public VolunteerEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<VolunteerEntity> query = dbContext.Set<VolunteerEntity>();
            query = query.Include(entity => entity.Transactions)
                .Include(entity => entity.Enrollments);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public IEnumerable<VolunteerEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Volunteers.ToArray();
        }

        public VolunteerEntity Insert(VolunteerEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            if (dbContext.Volunteers.Any(v => v.Email == entity.Email))
                return null;

            dbContext.Volunteers.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public VolunteerEntity Update(VolunteerEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Volunteers.Any(p => p.Id == entity.Id))
            {
                dbContext.Volunteers.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Volunteers.Any(p => p.Id == id))
            {
                var entity = new VolunteerEntity()
                {
                    Id = id
                };
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
