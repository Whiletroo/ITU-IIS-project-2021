/*
 *  File:   ShelterListModel.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System.ComponentModel.DataAnnotations;

namespace Charity.Common.Models
{
    public class ShelterListModel : ModelBase
    {
        [Required]
        public string Title { get; set; }
        public string? PhotoURL { get; set; }
        public string? Description { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as ShelterListModel;
            return this.Id.Equals(other.Id)
                   && this.Title == other.Title
                   && this.Description == other.Description
                   && this.PhotoURL == other.PhotoURL;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Description, PhotoURL);
        }
    }
}
