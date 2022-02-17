//!--Author xkrukh00-- >
using System;
using System.Linq;
using System.Collections.Generic;

namespace Charity.DAL.Entities
{
    public class ShelterEntity : EntityBase
    {
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public ICollection<DonationEntity>? Donations { get; set; } = new List<DonationEntity>();
        public ICollection<VolunteeringEntity>? Volunteerings { get; set; } = new List<VolunteeringEntity>();
        public string? Address { get; set; }
        public ShelterAdminEntity? Admin { get; set; }
        public Guid? AdminId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterEntity;
            return this.Title == other.Title
                && this.PhotoURL == other.PhotoURL
                && this.Description == other.Description
                && this.Address == other.Address
                && Enumerable.SequenceEqual(this.Donations.OrderBy(e => e.Id), other.Donations.OrderBy(e => e.Id))
                && Enumerable.SequenceEqual(this.Volunteerings.OrderBy(e => e.Id), other.Volunteerings.OrderBy(e => e.Id))
                && this.AdminId.Equals(other.AdminId);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, Donations, Volunteerings, Admin, AdminId);
        }
    }
}
