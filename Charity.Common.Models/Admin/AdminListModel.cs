namespace Charity.Common.Models
{
    public class AdminListModel : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as AdminListModel;
            return this.Id.Equals(other.Id)
                   && this.FirstName == other.FirstName
                   && this.LastName == other.LastName
                   && this.PhotoURL == other.PhotoURL;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine<Guid, string, string, string?>(Id, FirstName, LastName, PhotoURL);
        }
    }
}