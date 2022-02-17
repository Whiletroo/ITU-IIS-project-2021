//<!-- Author xpimen00-->
namespace Charity.Common.Models
{
    public class DonationListModel : ModelBase
    {
        public string Title { get; set; }
        public int Goal { get; set; }
        public int? State { get; set; }
        public string? PhotoURL { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Description { get; set; }
        public string ShelterTitle { get; set; }
        public Guid ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as DonationListModel;
            return this.Title == other.Title
                && this.Goal == other.Goal
                && this.ShelterTitle == other.ShelterTitle
                && this.ShelterId.Equals(other.ShelterId)
                && this.State == other.State
                && this.PhotoURL == other.PhotoURL;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Goal, ShelterTitle, ShelterId, State, PhotoURL);
        }

    }
}
