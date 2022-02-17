/*
 *  File:   EnrollmentRepository.cs
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
    public class EnrollmentRepository : RepositoryBase, IRepository<EnrollmentEntity>
    {
        public EnrollmentRepository(IDbContextFactory<CharityDbContext> dbContextFactory) : base(dbContextFactory) { }

        public EnrollmentEntity Get(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            IQueryable<EnrollmentEntity> query = dbContext.Set<EnrollmentEntity>();
            query = query.Include(enrol => enrol.Volunteering).ThenInclude(vol => vol.Shelter);
            query = query.Include(enrol => enrol.Volunteer).ThenInclude(vol => vol.Transactions);
            var entity = query.FirstOrDefault(entity => entity.Id == id);
            return entity;
        }

        public EnrollmentEntity Insert(EnrollmentEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            entity.Volunteer = dbContext.Volunteers.FirstOrDefault(v => v.Id.Equals(entity.VolunteerId));
            entity.Volunteering = dbContext.Volunteerings.FirstOrDefault(vs => vs.Id.Equals(entity.VolunteeringId));
            dbContext.Enrollments.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public IEnumerable<EnrollmentEntity> GetAll()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            return dbContext.Enrollments.Include(e => e.Volunteering).Include(e => e.Volunteer).ToArray();
        }

        public EnrollmentEntity Update(EnrollmentEntity entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            if (dbContext.Enrollments.Any(p => p.Id == entity.Id))
            {
                dbContext.Enrollments.Update(entity);
                dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public void Delete(Guid id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var entity = dbContext.Enrollments.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
