using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Infrastructure.Persistence;
namespace BloodBanking.Teste.Util
{
    public static class DatabaseSeeder
    {
        public static void Seed(BloodDonationDbContext context)
        {
            var donors = new List<Donor>
        {
            new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "johndoe@example.com",
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZipCode = "12345"
                }
            },
            new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                BloodType = BloodType.B,
                RhFactor = RhFactor.Negative,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZipCode = "67890"
                }
            }
        };

            context.Donors.AddRange(donors);
            context.SaveChanges();

            var donations = new List<Donation>
        {
            new Donation
            {
                Id = Guid.NewGuid(),
                DonorId = donors[0].Id, // ID do primeiro doador
                DonationDate = DateTime.Now.AddDays(-10),
                QuantityML = 500,
                Donor = donors[0]
            },
            new Donation
            {
                Id = Guid.NewGuid(),
                DonorId = donors[1].Id, // ID do segundo doador
                DonationDate = DateTime.Now.AddDays(-5),
                QuantityML = 450,
                Donor = donors[1]
            }
        };

            context.Donations.AddRange(donations);
            context.SaveChanges();

        }
    }


}
