﻿using AutoMapper;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Find2Me.Services
{
    public class AutoMapperConfig
    {
        public static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<FromDatabase>();
                cfg.AddProfile<ToDatabase>();
            });
        }
    }

    public class FromDatabase : Profile
    {
        public FromDatabase()
        {
            CreateMap<ApplicationUser, UserProfileVM>();
            CreateMap<ApplicationUser, UserProfilePictureVM>();
            CreateMap<Currency, CurrencyVM>();
        }
    }

    public class ToDatabase : Profile
    {
        public ToDatabase()
        {
            CreateMap<UserProfileVM, ApplicationUser>();
            CreateMap<UserProfilePictureVM, ApplicationUser>();
            CreateMap<CurrencyVM, Currency>();
        }
    }
}