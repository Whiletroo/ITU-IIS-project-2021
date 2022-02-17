/*
 *  File:   ShelterDetailModel.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class ShelterDetailModel : ModelBase
    {
        [Required]
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }
        public ICollection<DonationListModel>? Donations { get; set; }
        public ICollection<VolunteeringListModel>? Volunteerings { get; set; }
        public string? Address { get; set; }
        public Guid? ShelterAdminId { get; set; }
        public string? ShelterAdminEmail { get; set; }
        public string? ShelterAdminPhone { get; set; }
        public string? ShelterAdminFirstName { get; set; }
        public string? ShelterAdminLastName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterDetailModel;
            return this.Id.Equals(other.Id)
                   && this.Title == other.Title
                   && this.Description == other.Description
                   && this.PhotoURL == other.PhotoURL
                   && this.Address == other.Address
                   && Enumerable.SequenceEqual(this.Donations.OrderBy<DonationListModel, Guid>(e => e.Id), other.Donations.OrderBy<DonationListModel, Guid>(e => e.Id))
                   && Enumerable.SequenceEqual(this.Volunteerings.OrderBy<VolunteeringListModel, Guid>(e => e.Id), other.Volunteerings.OrderBy<VolunteeringListModel, Guid>(e => e.Id))
                   && this.ShelterAdminId.Equals(other.ShelterAdminId)
                   && this.ShelterAdminEmail == other.ShelterAdminEmail
                   && this.ShelterAdminPhone == other.ShelterAdminPhone
                   && this.ShelterAdminFirstName == other.ShelterAdminFirstName
                   && this.ShelterAdminLastName == other.ShelterAdminLastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, PhotoURL, Description, Donations, Volunteerings, ShelterAdminId, ShelterAdminEmail, Address);
        }
    }
}
