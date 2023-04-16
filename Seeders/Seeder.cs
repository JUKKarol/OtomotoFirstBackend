using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Seeders
{
    public class Seeder
    {
        private readonly OtomotoContext _otomotoContext;

        public Seeder(OtomotoContext otomotoContext)
        {
            _otomotoContext = otomotoContext;
        }

        public async Task Seed()
        {
            if (await _otomotoContext.Database.CanConnectAsync())
            {
                if (!_otomotoContext.Owners.Any() && !_otomotoContext.Offers.Any())
                {
                    Owner owner1 = new Owner()
                    {
                        Id = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                        FirstName = "John",
                        LastName = "Doe",
                        PhoneNumber = 123456789,
                        City = "New York"
                    };

                    Owner owner2 = new Owner()
                    {
                        Id = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F"),
                        FirstName = "Jane",
                        LastName = "Smith",
                        PhoneNumber = 987654321,
                        City = "Los Angeles"
                    };

                    Owner owner3 = new Owner()
                    {
                        Id = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2"),
                        FirstName = "Michael",
                        LastName = "Johnson",
                        PhoneNumber = 555555555,
                        City = "Chicago"
                    };

                    _otomotoContext.Owners.AddRange(owner1, owner2, owner3);
                    _otomotoContext.SaveChanges();

                    Offer offer1 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E36",
                        EngineSizeInL = 2.5,
                        ProductionYear = 1996,
                        Milleage = 300000,
                        OwnerId = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
                    };

                    Offer offer2 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E46",
                        EngineSizeInL = 2.8,
                        ProductionYear = 2003,
                        Milleage = 250000,
                        OwnerId = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
                    };

                    Offer offer3 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E38",
                        EngineSizeInL = 5,
                        ProductionYear = 1998,
                        Milleage = 400000,
                        OwnerId = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F")
                    };

                    Offer offer4 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "Cls",
                        EngineSizeInL = 5,
                        ProductionYear = 2007,
                        Milleage = 140000,
                        OwnerId = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F")
                    };

                    Offer offer5 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "S Class",
                        EngineSizeInL = 6,
                        ProductionYear = 2010,
                        Milleage = 180000,
                        OwnerId = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2")
                    };

                    Offer offer6 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "E Class",
                        EngineSizeInL = 6.3,
                        ProductionYear = 2005,
                        Milleage = 350000,
                        OwnerId = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2")
                    };

                    _otomotoContext.Offers.AddRange(offer1, offer2, offer3, offer4, offer5, offer6);
                    await _otomotoContext.SaveChangesAsync();
                }
            }
        }
    }
}
