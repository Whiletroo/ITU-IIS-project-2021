//<!-- Author xpimen00-->
using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class DonationDetailModel : ModelBase
    {
        [Required]
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Goal { get; set; }
        public int? State { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<TransactionListModel>? Transactions { get; set; }
        [Required]
        public Guid ShelterId { get; set; }
        [Required]
        public string ShelterTitle { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as DonationDetailModel;
            return this.Title == other.Title
                && this.PhotoURL == other.PhotoURL
                && this.Description == other.Description
                && this.State == other.State
                && this.Goal == other.Goal
                && this.ShelterTitle == other.ShelterTitle
                && this.ShelterId.Equals(other.ShelterId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, State, Goal, Transactions, ShelterTitle, ShelterId);
        }


    }
}
