//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Linq;

namespace Charity.DAL.Entities
{
    public class DonationEntity : EntityBase
    {
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public int? State { get; set; }
        public int Goal { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<TransactionEntity>? Transactions { get; set; } = new List<TransactionEntity>();
        public ShelterEntity Shelter { get; set; }
        public Guid ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as DonationEntity;
            return this.Title == other.Title 
                && this.PhotoURL == other.PhotoURL
                && this.Description == other.Description
                && this.State == other.State
                && this.Goal== other.Goal
                && Enumerable.SequenceEqual(this.Transactions.OrderBy(e => e.Id), other.Transactions.OrderBy(e => e.Id))
                && this.ShelterId.Equals(other.ShelterId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, State, Goal, Transactions, Shelter, ShelterId);
        }
    }
}
