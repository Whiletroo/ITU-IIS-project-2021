//<!-- Author xkrukh00-->
using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Charity.DAL.Entities
{
    public class VolunteerEntity : UserEntityBase
    {
        public ICollection<TransactionEntity>? Transactions { get; set; } = new List<TransactionEntity>();
        public ICollection<EnrollmentEntity>? Enrollments { get; set; } = new List<EnrollmentEntity>();

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as VolunteerEntity;
            return this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.PhotoURL == other.PhotoURL
                && this.Email == other.Email
                && this.Phone == other.Phone
                && this.Password == other.Password
                && this.Role == other.Role
                && Enumerable.SequenceEqual(this.Transactions.OrderBy(e => e.Id), other.Transactions.OrderBy(e => e.Id))
                && Enumerable.SequenceEqual(this.Enrollments.OrderBy(e => e.Id), other.Enrollments.OrderBy(e => e.Id));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, PhotoURL, Email, Phone, Password, Role);
        }
    }
}