//<!-- Author xpimen00-->
namespace Charity.Common.Models
{
    public class TransactionListModel : ModelBase
    {
        public DateTime DateTime { get; set; }
        public int Sum { get; set; }
        public Guid DonationId { get; set; }
        public string DonationTitle { get; set; }
        public Guid VolunteerId { get; set; }
        public string VolunteerEmail { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TransactionListModel;
            return this.Sum == other.Sum
                && this.DonationId.Equals(other.DonationId)
                && this.VolunteerEmail == other.VolunteerEmail
                && this.VolunteerId.Equals(other.VolunteerId)
                && this.DateTime.Equals(other.DateTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sum, DonationId, VolunteerId, VolunteerEmail);
        }


    }
}
