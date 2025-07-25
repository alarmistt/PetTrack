﻿using Microsoft.EntityFrameworkCore;
using PetTrack.Entity;
using System;

namespace PetTrack.Repositories.Base
{
    public class PetTrackDbContext : DbContext
    {
        public PetTrackDbContext(DbContextOptions<PetTrackDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicSchedule> ClinicSchedules { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<TopUpTransaction> TopUpTransactions { get; set; }
        public DbSet<BookingNotification> BookingNotifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var utcConverter = new UtcDateTimeOffsetConverter();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTimeOffset))
                    {
                        property.SetValueConverter(utcConverter);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetTrackDbContext).Assembly);
        }

    }
}
