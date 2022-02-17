using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class ShelterAdminListModel : ModelBase
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        [Required]
        public string Email { get; set; }
        public string? ShelterTitle { get; set; }
        public Guid? ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterAdminListModel;
            return this.FirstName == other.FirstName
                   && this.LastName == other.LastName
                   && this.PhotoURL == other.PhotoURL
                   && this.Email == other.Email
                   && this.ShelterId.Equals(other.ShelterId)
                   && this.ShelterTitle == other.ShelterTitle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, PhotoURL, Email, ShelterId);
        }
    }
}