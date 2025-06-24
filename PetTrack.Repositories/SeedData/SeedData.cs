using Microsoft.EntityFrameworkCore;
using PetTrack.Entity;
using PetTrack.Repositories.Base;

namespace PetTrack.Repositories.SeedData
{
    public class SeedData
    {
        private readonly PetTrackDbContext _context;

        public SeedData(PetTrackDbContext context)
        {
            _context = context;
        }

        public async Task Initialise()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    bool dbExists = _context.Database.CanConnect();
                    if (!dbExists)
                    {
                        _context.Database.Migrate();
                    }
                    await SeedUsers();
                    await SeedWallets();
                    await SeedBankAccounts();
                    await SeedClinics();
                    await SeedClinicSchedules();
                    await SeedSlots();
                    await SeedServicePackages();
                    await SeedBookings();
                    await SeedBookingNotifications();
                    await SeedTopUpTransactions();
                    await SeedWalletTransactions();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _context.Dispose();
            }
        }
        private async Task SeedWalletTransactions()
        {
            if (await _context.WalletTransactions.CountAsync() >= 10) return;

            var transactions = new List<WalletTransaction>();

            var walletIds = new[] { "wallet1", "wallet2", "wallet3", "wallet4", "wallet5" };
            var bookingIds = new[] { "booking1", "booking2", "booking3", "booking4", "booking5" };
            var amounts = new[] { 150000m, 250000m, 500000m, 180000m, 220000m };

            for (int i = 0; i < walletIds.Length; i++)
            {
                var walletId = walletIds[i];
                var bookingId = bookingIds[i];
                var price = amounts[i];
                var platformFee = price * 0.1m;
                var receiveAmount = price - platformFee;

                transactions.AddRange(new[]
                {
            new WalletTransaction
            {
                Id = $"txn_{walletId}_1",
                WalletId = walletId,
                Type = "TopUp",
                Status = "Approved",
                Amount = price,
                Description = "Nạp tiền vào ví"
            },
            new WalletTransaction
            {
                Id = $"txn_{walletId}_2",
                WalletId = walletId,
                Type = "BookingPayment",
                BookingId = bookingId,
                Status = "Approved",
                Amount = -price,
                Description = $"Thanh toán booking {bookingId}"
            },
            new WalletTransaction
            {
                Id = $"txn_{walletId}_3",
                WalletId = walletId,
                Type = "ComissionFee",
                BookingId = bookingId,
                Status = "Approved",
                Amount = -platformFee,
                Description = "Phí nền tảng"
            },
            new WalletTransaction
            {
                Id = $"txn_{walletId}_4",
                WalletId = walletId,
                Type = "ReceiveAmount",
                BookingId = bookingId,
                Status = "Approved",
                Amount = receiveAmount,
                Description = "Phòng khám nhận tiền"
            },
            new WalletTransaction
            {
                Id = $"txn_{walletId}_5",
                WalletId = walletId,
                Type = "Refund",
                BookingId = bookingId,
                Status = "Approved",
                Amount = price,
                Description = "Hoàn tiền do huỷ lịch hẹn"
            }
        });
            }

            _context.WalletTransactions.AddRange(transactions);
            await _context.SaveChangesAsync();
        }

        private async Task SeedBookings()
        {
            if (await _context.Bookings.CountAsync() >= 10) return;

            var bookings = new List<Booking>
    {
        new Booking
        {
            Id = "booking1",
            UserId = "user1",
            ClinicId = "clinic1",
            SlotId = "slot_clinic1_1",
            ServicePackageId = "svc1",
            AppointmentDate = DateTimeOffset.Now.AddDays(1),
            Status = "Pending",
            Price = 150000,
            PlatformFee = 15000,
            ClinicReceiveAmount = 135000
        },
        new Booking
        {
            Id = "booking2",
            UserId = "user2",
            ClinicId = "clinic2",
            SlotId = "slot_clinic2_1",
            ServicePackageId = "svc3",
            AppointmentDate = DateTimeOffset.Now.AddDays(2),
            Status = "Confirmed",
            Price = 250000,
            PlatformFee = 25000,
            ClinicReceiveAmount = 225000
        },
        new Booking
        {
            Id = "booking3",
            UserId = "user3",
            ClinicId = "clinic3",
            SlotId = "slot_clinic3_1",
            ServicePackageId = "svc4",
            AppointmentDate = DateTimeOffset.Now.AddDays(-1),
            Status = "Completed",
            Price = 500000,
            PlatformFee = 50000,
            ClinicReceiveAmount = 450000
        },
        new Booking
        {
            Id = "booking4",
            UserId = "user4",
            ClinicId = "clinic4",
            SlotId = "slot_clinic4_1",
            ServicePackageId = "svc5",
            AppointmentDate = DateTimeOffset.Now.AddDays(3),
            Status = "Cancelled",
            Price = 180000,
            PlatformFee = 18000,
            ClinicReceiveAmount = 162000
        },
        new Booking
        {
            Id = "booking5",
            UserId = "user5",
            ClinicId = "clinic5",
            SlotId = "slot_clinic5_1",
            ServicePackageId = "svc6",
            AppointmentDate = DateTimeOffset.Now.AddDays(4),
            Status = "Paid",
            Price = 220000,
            PlatformFee = 22000,
            ClinicReceiveAmount = 198000
        }
    };

            _context.Bookings.AddRange(bookings);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            if (await _context.Users.CountAsync() >= 10) return;

            var users = new List<User>
            {
                new User { Id = "user1", FullName = "Nguyen Van A", Email = "user1@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user2", FullName = "Tran Thi B", Email = "user2@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user3", FullName = "Le Van C", Email = "user3@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user4", FullName = "Pham Thi D", Email = "user4@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user5", FullName = "Hoang Van E", Email = "user5@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user6", FullName = "Nguyen Thi F", Email = "user6@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user7", FullName = "Dang Van G", Email = "user7@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user8", FullName = "Vo Thi H", Email = "user8@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user9", FullName = "Pham Van I", Email = "user9@example.com", Role = "User", IsPasswordSet = false },
                new User { Id = "user10", FullName = "Tran Thi K", Email = "user10@example.com", Role = "User", IsPasswordSet = false }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
        }

        private async Task SeedBankAccounts()
        {
            if (await _context.BankAccounts.CountAsync() >= 10) return;

            var bankAccounts = new List<BankAccount>
            {
                new BankAccount { Id = "ba1", BankName = "Vietcombank", BankNumber = "0123456789", UserId = "user1" },
                new BankAccount { Id = "ba2", BankName = "Techcombank", BankNumber = "1234567890", UserId = "user2" },
                new BankAccount { Id = "ba3", BankName = "ACB", BankNumber = "2345678901", UserId = "user3" },
                new BankAccount { Id = "ba4", BankName = "BIDV", BankNumber = "3456789012", UserId = "user4" },
                new BankAccount { Id = "ba5", BankName = "Sacombank", BankNumber = "4567890123", UserId = "user5" },
                new BankAccount { Id = "ba6", BankName = "VPBank", BankNumber = "5678901234", UserId = "user6" },
                new BankAccount { Id = "ba7", BankName = "TPBank", BankNumber = "6789012345", UserId = "user7" },
                new BankAccount { Id = "ba8", BankName = "MB Bank", BankNumber = "7890123456", UserId = "user8" },
                new BankAccount { Id = "ba9", BankName = "VIB", BankNumber = "8901234567", UserId = "user9" },
                new BankAccount { Id = "ba10", BankName = "SHB", BankNumber = "9012345678", UserId = "user10" }
            };

            _context.BankAccounts.AddRange(bankAccounts);
            await _context.SaveChangesAsync();
        }
        private async Task SeedWallets()
        {
            if (await _context.Wallets.CountAsync() >= 10) return;

            var wallets = new List<Wallet>
    {
        new Wallet { Id = "wallet1", UserId = "user1", Balance = 100000 },
        new Wallet { Id = "wallet2", UserId = "user2", Balance = 200000 },
        new Wallet { Id = "wallet3", UserId = "user3", Balance = 150000 },
        new Wallet { Id = "wallet4", UserId = "user4", Balance = 300000 },
        new Wallet { Id = "wallet5", UserId = "user5", Balance = 50000 },
        new Wallet { Id = "wallet6", UserId = "user6", Balance = 75000 },
        new Wallet { Id = "wallet7", UserId = "user7", Balance = 120000 },
        new Wallet { Id = "wallet8", UserId = "user8", Balance = 90000 },
        new Wallet { Id = "wallet9", UserId = "user9", Balance = 250000 },
        new Wallet { Id = "wallet10", UserId = "user10", Balance = 180000 }
    };

            _context.Wallets.AddRange(wallets);
            await _context.SaveChangesAsync();
        }
        private async Task SeedClinics()
        {
            if (await _context.Clinics.CountAsync() >= 5) return;

            var clinics = new List<Clinic>
    {
        new Clinic
        {
            Id = "clinic1",
            Name = "PetCare Hanoi",
            Address = "123 Le Loi, Hanoi",
            PhoneNumber = "0901000001",
            Slogan = "Chăm sóc thú cưng toàn diện",
            Description = "Phòng khám thú y uy tín tại Hà Nội",
            BannerUrl = null,
            Status = "Approved",
            OwnerUserId = "user1"
        },
        new Clinic
        {
            Id = "clinic2",
            Name = "Happy Paw HCM",
            Address = "45 Nguyen Trai, HCM",
            PhoneNumber = "0901000002",
            Slogan = "Vì thú cưng hạnh phúc",
            Description = "Dịch vụ khám chữa và spa cho thú cưng",
            BannerUrl = null,
            Status = "Pending",
            OwnerUserId = "user2"
        },
        new Clinic
        {
            Id = "clinic3",
            Name = "Vet House Da Nang",
            Address = "88 Tran Phu, Da Nang",
            PhoneNumber = "0901000003",
            Slogan = null,
            Description = "Trung tâm thú y hiện đại ở miền Trung",
            BannerUrl = null,
            Status = "Rejected",
            OwnerUserId = "user3"
        },
        new Clinic
        {
            Id = "clinic4",
            Name = "GreenVet Hue",
            Address = "56 Hung Vuong, Hue",
            PhoneNumber = "0901000004",
            Slogan = "Sức khỏe thú cưng là niềm vui của chúng tôi",
            Description = "Phòng khám thân thiện và chuyên nghiệp",
            BannerUrl = null,
            Status = "Approved",
            OwnerUserId = "user4"
        },
        new Clinic
        {
            Id = "clinic5",
            Name = "Pet Clinic Vung Tau",
            Address = "17 Cach Mang Thang 8, Vung Tau",
            PhoneNumber = "0901000005",
            Slogan = null,
            Description = "Khám chữa bệnh cho chó mèo và động vật nhỏ",
            BannerUrl = null,
            Status = "Pending",
            OwnerUserId = "user5"
        }
        // Bạn có thể thêm clinic6 → clinic10 nếu muốn đủ 10 dòng.
    };

            _context.Clinics.AddRange(clinics);
            await _context.SaveChangesAsync();
        }
        private async Task SeedClinicSchedules()
        {
            if (await _context.ClinicSchedules.CountAsync() >= 10) return;

            var schedules = new List<ClinicSchedule>();
            var clinicIds = new[] { "clinic1", "clinic2", "clinic3", "clinic4", "clinic5" };

            foreach (var clinicId in clinicIds)
            {
                for (int day = 1; day <= 5; day++) // Monday to Friday
                {
                    schedules.Add(new ClinicSchedule
                    {
                        Id = $"sched_{clinicId}_{day}",
                        ClinicId = clinicId,
                        DayOfWeek = day,
                        OpenTime = new TimeSpan(8, 0, 0),  // 08:00
                        CloseTime = new TimeSpan(17, 0, 0) // 17:00
                    });
                }
            }

            _context.ClinicSchedules.AddRange(schedules);
            await _context.SaveChangesAsync();
        }
        private async Task SeedBookingNotifications()
        {
            if (await _context.BookingNotifications.CountAsync() >= 10) return;

            var notifications = new List<BookingNotification>
    {
        new BookingNotification
        {
            Id = "noti1",
            BookingId = "booking1",
            UserId = "user1",
            Subject = "Lịch hẹn đã được xác nhận",
            Type = "BookingConfirmed",
            Content = "Lịch hẹn khám thú cưng của bạn đã được xác nhận. Hẹn gặp bạn tại phòng khám!"
        },
        new BookingNotification
        {
            Id = "noti2",
            BookingId = "booking2",
            UserId = "user2",
            Subject = "Hủy lịch hẹn",
            Type = "BookingCancelled",
            Content = "Lịch hẹn của bạn đã bị hủy theo yêu cầu."
        },
        new BookingNotification
        {
            Id = "noti3",
            BookingId = "booking3",
            UserId = "user3",
            Subject = "Nhắc lịch hẹn khám",
            Type = "Reminder",
            Content = "Bạn có lịch khám thú cưng vào ngày mai lúc 9h00. Đừng quên đến đúng giờ nhé!"
        },
        new BookingNotification
        {
            Id = "noti4",
            BookingId = "booking4",
            UserId = "user4",
            Subject = "Cập nhật lịch khám",
            Type = "BookingUpdated",
            Content = "Lịch hẹn của bạn đã được cập nhật thành công."
        },
        new BookingNotification
        {
            Id = "noti5",
            BookingId = "booking5",
            UserId = "user5",
            Subject = "Xác nhận hẹn lại lịch",
            Type = "BookingRescheduled",
            Content = "Lịch hẹn của bạn đã được dời sang 15:00 chiều cùng ngày."
        }
    };

            _context.BookingNotifications.AddRange(notifications);
            await _context.SaveChangesAsync();
        }
        private async Task SeedServicePackages()
        {
            if (await _context.ServicePackages.CountAsync() >= 10) return;

            var packages = new List<ServicePackage>
    {
        new ServicePackage
        {
            Id = "svc1",
            ClinicId = "clinic1",
            Name = "Khám tổng quát",
            Description = "Dịch vụ khám tổng quát cho thú cưng",
            Price = 150000
        },
        new ServicePackage
        {
            Id = "svc2",
            ClinicId = "clinic1",
            Name = "Tiêm phòng",
            Description = "Tiêm vaccine định kỳ cho chó/mèo",
            Price = 200000
        },
        new ServicePackage
        {
            Id = "svc3",
            ClinicId = "clinic2",
            Name = "Spa thú cưng",
            Description = "Tắm, cắt tỉa lông, vệ sinh tai",
            Price = 250000
        },
        new ServicePackage
        {
            Id = "svc4",
            ClinicId = "clinic3",
            Name = "Phẫu thuật tiểu phẫu",
            Description = "Các tiểu phẫu đơn giản cho thú cưng",
            Price = 500000
        },
        new ServicePackage
        {
            Id = "svc5",
            ClinicId = "clinic4",
            Name = "Khám da liễu",
            Description = "Khám và điều trị các bệnh về da",
            Price = 180000
        },
        new ServicePackage
        {
            Id = "svc6",
            ClinicId = "clinic5",
            Name = "Cạo vôi răng",
            Description = "Làm sạch răng miệng và cạo vôi",
            Price = 220000
        }
    };

            _context.ServicePackages.AddRange(packages);
            await _context.SaveChangesAsync();
        }
        private async Task SeedTopUpTransactions()
        {
            if (await _context.TopUpTransactions.CountAsync() >= 10) return;

            var topUps = new List<TopUpTransaction>
    {
        new TopUpTransaction
        {
            Id = "topup1",
            UserId = "user1",
            Amount = 50000,
            PaymentMethod = "PayOS",
            Status = "Success",
            TransactionCode = "TXN001"
        },
        new TopUpTransaction
        {
            Id = "topup2",
            UserId = "user2",
            Amount = 100000,
            PaymentMethod = "MoMo",
            Status = "Pending",
            TransactionCode = "TXN002"
        },
        new TopUpTransaction
        {
            Id = "topup3",
            UserId = "user3",
            Amount = 75000,
            PaymentMethod = "PayOS",
            Status = "Failed",
            TransactionCode = "TXN003"
        },
        new TopUpTransaction
        {
            Id = "topup4",
            UserId = "user4",
            Amount = 200000,
            PaymentMethod = "MoMo",
            Status = "Success",
            TransactionCode = "TXN004"
        },
        new TopUpTransaction
        {
            Id = "topup5",
            UserId = "user5",
            Amount = 300000,
            PaymentMethod = "PayOS",
            Status = "Success",
            TransactionCode = "TXN005"
        }
    };

            _context.TopUpTransactions.AddRange(topUps);
            await _context.SaveChangesAsync();
        }
        private async Task SeedSlots()
        {
            if (await _context.Slots.CountAsync() >= 10) return;

            var clinicIds = new[] { "clinic1", "clinic2", "clinic3", "clinic4", "clinic5" };
            var slots = new List<Slot>();

            foreach (var clinicId in clinicIds)
            {
                // Tạo 3 slot: 08:00–10:00, 10:00–12:00, 13:00–15:00 cho thứ 2
                slots.Add(new Slot
                {
                    Id = $"slot_{clinicId}_1",
                    ClinicId = clinicId,
                    DayOfWeek = 1,
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(10, 0, 0)
                });

                slots.Add(new Slot
                {
                    Id = $"slot_{clinicId}_2",
                    ClinicId = clinicId,
                    DayOfWeek = 1,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0)
                });

                slots.Add(new Slot
                {
                    Id = $"slot_{clinicId}_3",
                    ClinicId = clinicId,
                    DayOfWeek = 1,
                    StartTime = new TimeSpan(13, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0)
                });
            }

            _context.Slots.AddRange(slots);
            await _context.SaveChangesAsync();
        }



    }
}
