/*
 *  File:   MappingProfile.cs
 *  Author: Everyone
 *
 */

using System;
using AutoMapper;
using Charity.Common.Models;
using Charity.DAL.Entities;

namespace Charity.API.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdminEntity, AdminListModel>();
            CreateMap<AdminEntity, AdminDetailModel>();

            CreateMap<AdminDetailModel, AdminEntity>();
            CreateMap<AdminListModel, AdminEntity>();

            
            CreateMap<VolunteeringEntity, VolunteeringListModel>();
            CreateMap<VolunteeringEntity, VolunteeringDetailModel>()
                .ForMember(dest => dest.ShelterLogoUrl, opt => opt.MapFrom(src=>src.Shelter.PhotoURL))
                .ForMember(dest => dest.ShelterTitle, opt => opt.MapFrom(src => src.Shelter.Title));

            CreateMap<VolunteeringDetailModel, VolunteeringEntity>();
            CreateMap<VolunteeringListModel, VolunteeringEntity>();


            CreateMap<ShelterEntity, ShelterListModel>();
            CreateMap<ShelterEntity, ShelterDetailModel>()
                .ForMember(dest => dest.ShelterAdminEmail, opt => opt.MapFrom(src => src.Admin.Email))
                .ForMember(dest => dest.ShelterAdminPhone, opt => opt.MapFrom(src => src.Admin.Phone))
                .ForMember(dest => dest.ShelterAdminFirstName, opt => opt.MapFrom(src => src.Admin.FirstName))
                .ForMember(dest => dest.ShelterAdminLastName, opt => opt.MapFrom(src => src.Admin.LastName))
                .ForMember(dest => dest .ShelterAdminId, opt => opt.MapFrom(src => src.AdminId));
            CreateMap<ShelterDetailModel, ShelterEntity>();
            CreateMap<ShelterListModel, ShelterEntity>();

            
            CreateMap<EnrollmentEntity, EnrollmentListModel>()
                .ForMember(dest => dest.VolunteerEmail, opt => opt.MapFrom(src => src.Volunteer.Email))
                .ForMember(dest => dest.VolunteeringTitle, opt => opt.MapFrom(src => src.Volunteering.Title));
            CreateMap<EnrollmentEntity, EnrollmentDetailModel>()
                .ForMember(dest => dest.VolunteerEmail, opt => opt.MapFrom(src => src.Volunteer.Email))
                .ForMember(dest => dest.VolunteeringTitle, opt => opt.MapFrom(src => src.Volunteering.Title));

            CreateMap<EnrollmentListModel, EnrollmentEntity>();
            CreateMap<EnrollmentDetailModel, EnrollmentEntity>();


            CreateMap<VolunteerEntity, VolunteerListModel>();
            CreateMap<VolunteerEntity, VolunteerDetailModel>();

            CreateMap<VolunteerDetailModel, VolunteerEntity>();
            CreateMap<VolunteerListModel, VolunteerEntity>();


            CreateMap<TransactionEntity, TransactionListModel>()
                .ForMember(dest => dest.VolunteerEmail, opt => opt.MapFrom(src => src.Volunteer.Email))
                .ForMember(dest => dest.DonationTitle, opt => opt.MapFrom(src => src.Donation.Title));
            CreateMap<TransactionEntity, TransactionDetailModel>()
                .ForMember(dest => dest.VolunteerFirstName, opt => opt.MapFrom(src => src.Volunteer.FirstName))
                .ForMember(dest => dest.VolunteerLastName, opt => opt.MapFrom(src => src.Volunteer.LastName))
                .ForMember(dest => dest.VolunteerEmail, opt => opt.MapFrom(src => src.Volunteer.Email))
                .ForMember(dest => dest.DonationTitle, opt => opt.MapFrom(src => src.Donation.Title));

            CreateMap<TransactionDetailModel, TransactionEntity>();
            CreateMap<TransactionListModel, TransactionEntity>();


            CreateMap<DonationEntity, DonationListModel>()
                .ForMember(dest => dest.ShelterTitle, opt => opt.MapFrom(src => src.Shelter.Title));
            CreateMap<DonationEntity, DonationDetailModel>()
                .ForMember(dest => dest.ShelterTitle, opt => opt.MapFrom(src => src.Shelter.Title)); ;

            CreateMap<DonationListModel, DonationEntity>();
            CreateMap<DonationDetailModel, DonationEntity>();


            CreateMap<ShelterAdminEntity, ShelterAdminListModel>()
                .ForMember(dest => dest.ShelterTitle, opt => opt.MapFrom(src => src.Shelter.Title));
            CreateMap<ShelterAdminEntity, ShelterAdminDetailModel>()
                .ForMember(dest => dest.ShelterTitle, opt => opt.MapFrom(src => src.Shelter.Title));

            CreateMap<ShelterAdminListModel, ShelterAdminEntity>();
            CreateMap<ShelterAdminDetailModel, ShelterAdminEntity>();

            CreateMap<RegistrationModel, VolunteerEntity>();
            CreateMap<RegistrationModel, ShelterAdminEntity>();
            CreateMap<RegistrationModel, AdminEntity>();
        }
    }
}
