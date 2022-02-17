//<!-- Author xkrukh00-->
using System;

namespace Charity.DAL.Entities
{
    public class ShelterAdminEntity : UserEntityBase
    {
        public ShelterEntity? Shelter { get; set; }
        public Guid? ShelterId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterAdminEntity;
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
