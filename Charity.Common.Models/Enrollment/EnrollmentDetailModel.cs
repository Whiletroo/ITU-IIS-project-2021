using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class EnrollmentDetailModel : ModelBase
    {
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string VolunteerEmail { get; set; }
        [Required]
        public Guid VolunteerId { get; set; }
        [Required]
        public string VolunteeringTitle { get; set; }
        [Required]
        public Guid VolunteeringId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as EnrollmentDetailModel;
            return this.DateTime.Equals(other.DateTime)
                   && this.VolunteerId.Equals(other.VolunteerId)
                   && this.VolunteerEmail == other.VolunteerEmail
                   && this.VolunteeringId.Equals(other.VolunteeringId)
                   && this.VolunteeringTitle == other.VolunteeringTitle;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine<Guid, DateTime, string, string>(Id, DateTime, VolunteerEmail, VolunteeringTitle);
        }
    }
}