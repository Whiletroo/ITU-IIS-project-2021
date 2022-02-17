using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class AdminDetailModel : ModelBase
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as AdminDetailModel;
            return this.Id.Equals(other.Id)
                   && this.FirstName == other.FirstName
                   && this.LastName == other.LastName
                   && this.PhotoURL == other.PhotoURL
                   && this.Email == other.Email
                   && this.Phone == other.Phone
                   && this.Password == other.Password
                   && this.Role == other.Role;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, PhotoURL, Email, Phone, Password, Role);
        }
    }
}
