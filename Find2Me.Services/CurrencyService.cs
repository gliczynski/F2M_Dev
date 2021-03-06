﻿using AutoMapper;
using Find2Me.DAL;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Services
{
  public interface ICurrencyService
  {
    List<CurrencyVM> GetAllCurrencies();
    CurrencyVM GetCurrencyByCurrencyCode(string code);
  }

  public class CurrencyService : ICurrencyService
  {
    private CurrencyRepository currencyRepository;
    public CurrencyService(ApplicationDbContext dbContext)
    {
      currencyRepository = new CurrencyRepository(dbContext);
    }

    public List<CurrencyVM> GetAllCurrencies()
    {
      List<Currency> currencies = currencyRepository.GetAll().ToList();
      List<CurrencyVM> currencyVMs = new List<CurrencyVM>();
      Mapper.Map(currencies, currencyVMs);
      return currencyVMs;
    }

    public CurrencyVM GetCurrencyByCurrencyCode(string code)
    {
      Currency currency = currencyRepository.GetSingle(code);
      CurrencyVM currencyVM = new CurrencyVM();
      Mapper.Map(currency, currencyVM);
      return currencyVM;
    }
  }
}
