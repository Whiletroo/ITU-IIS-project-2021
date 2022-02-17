using System;
using System.Linq;
using System.Collections.Generic;

namespace Charity.DAL.Entities
{
    public class VolunteeringEntity : EntityBase
    {
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<EnrollmentEntity>? Enrollments { get; set; } = new List<EnrollmentEntity>();
        public int? RequiredCount { get; set; }
        public ShelterEntity Shelter { get; set; }
        public Guid ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteeringEntity;
            return this.Title == other.Title
                   && this.PhotoURL == other.PhotoURL
                   && this.Description == other.Description
                   && Enumerable.SequenceEqual(this.Enrollments.OrderBy(e => e.Id),
                       other.Enrollments.OrderBy(e => e.Id))
                   && this.RequiredCount == other.RequiredCount
                   && this.ShelterId.Equals(other.ShelterId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, Enrollments, RequiredCount, ShelterId);
        }
    }
}
