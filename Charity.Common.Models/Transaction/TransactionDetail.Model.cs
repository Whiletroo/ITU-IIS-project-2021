//< !--Author xpimen00-- >
using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class TransactionDetailModel : ModelBase
    {
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int Sum { get; set; }
        [Required]
        public Guid VolunteerId { get; set; }
        [Required]
        public string VolunteerEmail { get; set; }
        public string? VolunteerFirstName { get; set; }
        public string? VolunteerLastName { get; set; }
        [Required]
        public Guid DonationId { get; set; }
        public string? DonationTitle { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as TransactionDetailModel;
            return this.Sum == other.Sum
                && this.DonationTitle == other.DonationTitle
                && this.VolunteerLastName == other.VolunteerLastName
                && this.VolunteerFirstName == other.VolunteerFirstName
                && this.VolunteerEmail == other.VolunteerEmail
                && this.DonationId.Equals(other.DonationId)
                && this.VolunteerId.Equals(other.VolunteerId)
                && this.DateTime.Equals(other.DateTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sum, DonationId, VolunteerId, VolunteerLastName, VolunteerEmail);
        }
    }
}
