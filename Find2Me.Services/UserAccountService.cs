﻿using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Services
{
    public class UserAccountService
    {
        ApplicationDbContext _dbContext;
        public UserAccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}