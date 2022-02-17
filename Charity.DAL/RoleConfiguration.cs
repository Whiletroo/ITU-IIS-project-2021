using System;
using System.Collections.Generic;
using System.Linq;
//!--Author xkrukh00-- >
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charity.DAL
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "Volunteer",
                    NormalizedName = "VOLUNTEER"
                },
                new IdentityRole
                {
                    Name = "ShelterAdministrator",
                    NormalizedName = "SHELTERADMINISTRATOR"
                });
        }
    }
}
