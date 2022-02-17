﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charity.DAL.Entities
{
    public class AdminEntity : UserEntityBase
    {
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as AdminEntity;
            return this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.PhotoURL == other.PhotoURL
                && this.Email == other.Email
                && this.Phone == other.Phone
                && this.Password == other.Password
                && this.Role == other.Role;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, PhotoURL, Email, Phone, Password, Role);
        }
    }
}
