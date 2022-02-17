//< !--Author xpimen00-- >
using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class VolunteerListModel : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhotoUrl { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteerListModel;
            return this.LastName == other.LastName
                && this.Email == other.Email
                && this.FirstName == other.FirstName
                && this.PhotoUrl == other.PhotoUrl;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LastName, Email, FirstName);
        }
    }
}
