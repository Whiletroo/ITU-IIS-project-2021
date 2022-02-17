using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class ShelterAdminDetailModel : ModelBase
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? PhotoURL { get; set; }
        
        public string? Phone { get; set; }
        
        public string? Role { get; set; }
        public string? ShelterTitle { get; set; }
        public Guid? ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterAdminDetailModel;
            return this.FirstName == other.FirstName
                   && this.LastName == other.LastName 
                   && this.PhotoURL == other.PhotoURL 
                   && this.Email == other.Email 
                   && this.Phone == other.Phone 
                   && this.Password == other.Password 
                   && this.Role == other.Role 
                   && this.ShelterId.Equals(other.ShelterId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, PhotoURL, Email, Phone, Password, Role, ShelterId);
        }
    }
}
