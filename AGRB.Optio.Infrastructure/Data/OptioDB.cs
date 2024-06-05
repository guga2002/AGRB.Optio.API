﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Optio.Core.Entities;
using RGBA.Optio.Core.Entities;

namespace Optio.Core.Data
{
    public class OptioDB(DbContextOptions<OptioDB> bs) : IdentityDbContext<User>(bs)
    {
        public virtual DbSet<LocationToMerchant> LocationToMerchants { get; set; }
        public virtual DbSet<Category> CategoryOfTransactions { get; set; }
        public virtual DbSet<Channels>Channels { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TypeOfTransaction> Types { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
    }
}
