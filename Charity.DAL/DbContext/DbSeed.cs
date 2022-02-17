/*
 *  File:   DbSeed.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using Charity.DAL.Entities;
using Microsoft.EntityFrameworkCore;


namespace Charity.DAL
{
    public static class DbSeed
    {
        public static readonly AdminEntity Admin = new AdminEntity
        {
            Id = Guid.Parse("35bfc454-ff3c-4079-92ab-52c3060be8a1"),
            Email = "admin@shelterio.com",
            FirstName = "Guy",
            LastName = "Super",
            Password = "verysecurepass",
            Phone = "+420777111222",
            PhotoURL = "",
            Role = "admin"
        };

        public static readonly DonationEntity Donation = new DonationEntity
        {
            Id = Guid.Parse("5af6bbb5-a2ae-433a-b999-d7b3891eb51b"),
            Title = "Doggie donation",
            Description = "Donate cute dog lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean malesuada lacus tortor, at rutrum ante dictum nec. Proin sodales porttitor lorem ut dictum. Pellentesque fringilla vulputate luctus. Nam sollicitudin risus id libero commodo aliquam. Duis ut feugiat neque. In elementum turpis quis odio ultrices, eget fermentum elit malesuada. Ut sit amet imperdiet massa. Suspendisse urna nisl, cursus eu pharetra eget, tincidunt ut neque.",
            Goal = 1000,
            State = 200,
            DateTime = new DateTime(2021,12,01,0,0,0),
            PhotoURL = "https://www.expats.cz/images/publishing/articles/2019/11/1280_650/go-cuddle-in-kadlin-handipet-rescue-animal-shelter-is-seeking-volunteers-jpg-xpmac.jpg"
        };

        public static readonly EnrollmentEntity Enrollment = new EnrollmentEntity
        {
            Id = Guid.Parse("b9f0731b-3b02-48a2-88ce-7d7fd48a4d2a"),
            DateTime = new DateTime(2021, 11, 20, 17, 26, 50)
        };

        public static readonly ShelterAdminEntity ShelterAdmin = new ShelterAdminEntity
        {
            Id = Guid.Parse("2ea469b7-70e9-4810-b679-0a60ab205f16"),
            Email = "johndoe@superdoggies.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "doggiepass",
            Phone = "+420666555444",
            PhotoURL = "",
            Role = "shelter-admin"
        };

        public static readonly ShelterEntity Shelter = new ShelterEntity
        {
            Id = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5"),
            Title = "Super Doggies",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer sit amet lectus ligula. Aliquam tincidunt a ex et viverra. Nam dignissim egestas urna, et lobortis erat aliquet vitae. Aenean bibendum magna est, eu iaculis tortor blandit quis. Integer vestibulum sapien at velit pretium, sed sollicitudin mi scelerisque. Donec vitae auctor urna. In faucibus turpis turpis, et finibus purus imperdiet et. Nulla sed magna orci. Cras tortor urna, dictum nec ligula et, vehicula consectetur lorem. Aenean quis pharetra ipsum, eget malesuada ligula. Donec sed consectetur eros. Integer aliquam lacinia nisl, nec condimentum arcu pellentesque a. Aliquam vel odio lectus. Sed eget est nisl. Curabitur at sapien in tellus dapibus feugiat. Maecenas dolor sem, volutpat id tempus ut, iaculis a dolor.",
            Address = "Brno st main",
            PhotoURL = "https://www.logolynx.com/images/logolynx/3b/3b4e5f16f0ccd5f02f4c3f5fa68031e9.jpeg"
        };

        public static readonly TransactionEntity Transaction = new TransactionEntity
        {
            Id = Guid.Parse("7600763f-6a2e-482c-9ded-fa9a824376e5"),
            DateTime = new DateTime(2021, 10, 21, 11, 20, 15),
            Sum = 100
        };

        public static readonly VolunteerEntity Volunteer = new VolunteerEntity
        {
            Id = Guid.Parse("952f40f0-8181-4cc6-aff8-d932e002d98f"),
            Email = "janedoe@mail.com",
            FirstName = "Jane",
            LastName = "Sue",
            Password = "suepass",
            Phone = "+420555444222",
            PhotoURL = "https://scontent-prg1-1.xx.fbcdn.net/v/t39.30808-6/214664454_3054028748220135_5232530489626250445_n.jpg?_nc_cat=110&ccb=1-5&_nc_sid=09cbfe&_nc_ohc=jvZz0yhGArwAX-FAACl&tn=mRRjwhJuYUsf6LX7&_nc_ht=scontent-prg1-1.xx&oh=a1ff46a61f59ff521bcc67f9083cd5f2&oe=61A900D7",
            Role = "volunteer"
        };

        public static readonly VolunteeringEntity Volunteering = new VolunteeringEntity
        {
            Id = Guid.Parse("9a2625c7-9008-403f-8d0e-c5257a5e9af5"),
            Title = "Volunteer Super Doggies",
            DateTime = new DateTime(2020, 04, 12, 10, 34, 42),
            Description = "Help as volunteer",
            PhotoURL = "https://i.ebayimg.com/images/g/hXoAAOSwQnpgblRI/s-l300.jpg",
            RequiredCount = 4
        };

        static DbSeed()
        {
            Donation.Shelter = Shelter;
            Donation.ShelterId = Shelter.Id;
            Enrollment.Volunteer = Volunteer;
            Enrollment.VolunteerId = Volunteer.Id;
            Enrollment.Volunteering = Volunteering;
            Enrollment.VolunteeringId = Volunteering.Id;
            ShelterAdmin.Shelter = Shelter;
            ShelterAdmin.ShelterId = Shelter.Id;
            Shelter.Admin = ShelterAdmin;
            Shelter.AdminId = ShelterAdmin.Id;
            Transaction.Donation = Donation;
            Transaction.DonationId = Donation.Id;
            Transaction.Volunteer = Volunteer;
            Transaction.VolunteerId = Volunteer.Id;
            Volunteering.Shelter = Shelter;
            Volunteering.ShelterId = Shelter.Id;

            Donation.Transactions = new List<TransactionEntity> { Transaction };
            Shelter.Donations = new List<DonationEntity> { Donation };
            Shelter.Volunteerings = new List<VolunteeringEntity> { Volunteering };
            Volunteer.Enrollments = new List<EnrollmentEntity> { Enrollment };
            Volunteer.Transactions = new List<TransactionEntity> { Transaction };
            Volunteering.Enrollments = new List<EnrollmentEntity> { Enrollment };
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminEntity>().HasData(
                new AdminEntity
                {
                    Id = Admin.Id,
                    Email = Admin.Email,
                    FirstName = Admin.FirstName,
                    LastName = Admin.LastName,
                    Password = Admin.Password,
                    Phone = Admin.Phone,
                    PhotoURL = Admin.PhotoURL,
                    Role = Admin.Role
                });

            modelBuilder.Entity<DonationEntity>().HasData(
                new DonationEntity
                {
                    Id = Donation.Id,
                    Title = Donation.Title,
                    Description = Donation.Description,
                    Goal = Donation.Goal,
                    State = Donation.State,
                    DateTime = Donation.DateTime,
                    PhotoURL = Donation.PhotoURL,
                    ShelterId = Shelter.Id
                });

            modelBuilder.Entity<EnrollmentEntity>().HasData(
                new EnrollmentEntity
                {
                    Id = Enrollment.Id,
                    DateTime = Enrollment.DateTime,
                    VolunteerId = Volunteer.Id,
                    VolunteeringId = Volunteering.Id
                });

            modelBuilder.Entity<ShelterAdminEntity>().HasData(
                new ShelterAdminEntity
                {
                    Id = ShelterAdmin.Id,
                    Email = ShelterAdmin.Email,
                    FirstName = ShelterAdmin.FirstName,
                    LastName = ShelterAdmin.LastName,
                    Password = ShelterAdmin.Password,
                    Phone = ShelterAdmin.Phone,
                    PhotoURL = ShelterAdmin.PhotoURL,
                    Role = ShelterAdmin.Role,
                    ShelterId = Shelter.Id
                });

            modelBuilder.Entity<ShelterEntity>().HasData(
                new ShelterEntity
                {
                    Id = Shelter.Id,
                    Title = Shelter.Title,
                    Description = Shelter.Description,
                    PhotoURL = Shelter.PhotoURL,
                    AdminId = ShelterAdmin.Id
                });

            modelBuilder.Entity<TransactionEntity>().HasData(
                new TransactionEntity
                {
                    Id = Transaction.Id,
                    DateTime = Transaction.DateTime,
                    Sum = Transaction.Sum,
                    DonationId = Donation.Id,
                    VolunteerId = Volunteer.Id
                }
                );

            modelBuilder.Entity<VolunteerEntity>().HasData(
                new VolunteerEntity
                {
                    Id = Volunteer.Id,
                    Email = Volunteer.Email,
                    FirstName = Volunteer.FirstName,
                    LastName = Volunteer.LastName,
                    Password = Volunteer.Password,
                    Phone = Volunteer.Phone,
                    PhotoURL = Volunteer.PhotoURL,
                    Role = Volunteer.Role,
                });

            modelBuilder.Entity<VolunteeringEntity>().HasData(
                new VolunteeringEntity
                {
                    Id = Volunteering.Id,
                    Title = Volunteering.Title,
                    DateTime = Volunteering.DateTime,
                    Description = Volunteering.Description,
                    PhotoURL = Volunteering.PhotoURL,
                    RequiredCount = Volunteering.RequiredCount,
                    ShelterId = Shelter.Id
                });
        }
    }
}
