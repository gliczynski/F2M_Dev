using AutoMapper;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Find2MeWeb.App_Start
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
            CreateMap<Currency, CurrencyVM>();
        }
    }

    public class ToDatabase : Profile
    {
        public ToDatabase()
        {
            CreateMap<UserProfileVM, ApplicationUser>();
            CreateMap<CurrencyVM, Currency>();
        }
    }
}