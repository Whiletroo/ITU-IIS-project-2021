using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class VolunteeringDetailModel : ModelBase
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid? ShelterId { get; set; }
        [Required]
        public string? ShelterTitle { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTime { get; set; }
        public ICollection<EnrollmentListModel>? Enrollments { get; set; }
        public int? RequiredCount { get; set; }
        public string? ShelterLogoUrl { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteeringDetailModel;
            return this.Id.Equals(other.Id)
                   && this.Title == other.Title
                   && this.PhotoURL == other.PhotoURL
                   && this.Description == other.Description
                   && Enumerable.SequenceEqual(this.Enrollments.OrderBy<EnrollmentListModel, Guid>(e => e.Id),
                       other.Enrollments.OrderBy<EnrollmentListModel, Guid>(e => e.Id))
                   && this.RequiredCount == other.RequiredCount
                   && this.ShelterId.Equals(other.ShelterId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, Enrollments, RequiredCount, ShelterId);
        }
    }
}
