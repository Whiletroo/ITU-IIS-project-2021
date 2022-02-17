//<!-- Author xpimen00-->
using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class VolunteerDetailModel :ModelBase
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public string? PhotoUrl { get; set; }
        public ICollection<EnrollmentListModel>? Enrollments { get; set; } = new List<EnrollmentListModel>();
        public ICollection<TransactionListModel>? Transactions { get; set; } = new List<TransactionListModel>();
       


        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteerDetailModel;
            return this.LastName == other.LastName
                && this.Email == other.Email
                && this.FirstName == other.FirstName
                && this.Phone == other.Phone
                && this.Password == other.Password
                && this.Role == other.Role
                && this.PhotoUrl == other.PhotoUrl;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine( FirstName, LastName, Email, Enrollments, Transactions, Phone, Password, Role);
        }
        


    }
}
