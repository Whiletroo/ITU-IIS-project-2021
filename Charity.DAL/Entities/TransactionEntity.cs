//<!-- Author xpimen00-->
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.DAL.Entities
{
    public class TransactionEntity : EntityBase
    {
        public DateTime DateTime { get; set; }
        public int Sum { get; set; }
        public VolunteerEntity Volunteer { get; set; }
        public Guid DonationId { get; set; }
        public DonationEntity Donation { get; set; }
        public Guid VolunteerId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TransactionEntity;
            return this.Sum == other.Sum
                && this.DonationId.Equals(other.DonationId)
                && this.VolunteerId.Equals(other.VolunteerId)
                && this.DateTime.Equals(other.DateTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sum, Donation, DonationId, VolunteerId);
        }
    }
}
