using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class VolunteeringListModel : ModelBase
    {
        [Required]
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteeringListModel;
            return this.Id.Equals(other.Id)
                   && this.Title == other.Title
                   && this.PhotoURL == other.PhotoURL
                   && this.Description == other.Description
                   && this.DateTime.Equals(other.DateTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, PhotoURL, DateTime, Description);
        }
    }
}