/*
 *  File:   EnrollmentEntity.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.DAL.Entities
{
    public class EnrollmentEntity : EntityBase
    {
        public DateTime DateTime { get; set; }
        public VolunteerEntity Volunteer { get; set;}
        public Guid VolunteerId { get; set; }
        public VolunteeringEntity Volunteering { get; set; }
        public Guid VolunteeringId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as EnrollmentEntity;
            return this.DateTime.Equals(other.DateTime)
                   && this.VolunteerId.Equals(other.VolunteerId)
                   && this.VolunteeringId.Equals(other.VolunteeringId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DateTime, Volunteer, Volunteering);
        }
    }
}
