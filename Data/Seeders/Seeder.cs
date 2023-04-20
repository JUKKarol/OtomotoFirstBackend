using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Data.Seeders
{
    public class Seeder
    {
        private readonly OtomotoContext _otomotoContext;

        public Seeder(OtomotoContext otomotoContext)
        {
            _otomotoContext = otomotoContext;
        }

        public void Seed()
        {
            if (_otomotoContext.Database.CanConnect())
            {
                if (!_otomotoContext.Owners.Any() && !_otomotoContext.Offers.Any())
                {
                    Owner owner1 = new Owner()
                    {
                        Id = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                        FirstName = "John",
                        LastName = "Doe",
                        PhoneNumber = 123456789,
                        City = "New York",
                        Email = "johndoe12@gmail.com",
                        Password = "IlikeBMW123"
                    };

                    Owner owner2 = new Owner()
                    {
                        Id = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F"),
                        FirstName = "Jane",
                        LastName = "Smith",
                        PhoneNumber = 987654321,
                        City = "Los Angeles",
                        Email = "janesmith11@gmail.com",
                        Password = "carmellover633"
                    };

                    Owner owner3 = new Owner()
                    {
                        Id = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2"),
                        FirstName = "Michael",
                        LastName = "Johnson",
                        PhoneNumber = 555555555,
                        City = "Chicago",
                        Email = "michaeljonson9@gmail.com",
                        Password = "askd8qasS"
                    };

                    _otomotoContext.Owners.AddRange(owner1, owner2, owner3);
                    _otomotoContext.SaveChanges();

                    Offer offer1 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E36",
                        Body = "Sedan",
                        Description = "Drift mods, a lot of new parts, full working",
                        PriceInEur = 6000,
                        FuelType = "Petrol",
                        EngineSizeInL = 2.5,
                        HorsePower = 192,
                        AutomaticTransmission = false,
                        ProductionYear = 1996,
                        Milleage = 300000,
                        OwnerId = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
                    };

                    Offer offer2 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E46",
                        Body = "Coupe",
                        Description = "Nice daily car with no defects",
                        PriceInEur = 8000,
                        FuelType = "Petrol",
                        EngineSizeInL = 2.8,
                        HorsePower = 193,
                        AutomaticTransmission = false,
                        ProductionYear = 1999,
                        Milleage = 293000,
                        OwnerId = Guid.Parse("3F2504E0-4F89-11D3-9A0C-0305E82C3301")
                    };

                    Offer offer3 = new Offer()
                    {
                        Brand = "BMW",
                        Model = "E38",
                        Body = "Sedan",
                        Description = "Good condiction, there is somrthing to repair",
                        PriceInEur = 10000,
                        FuelType = "Petrol",
                        EngineSizeInL = 4.4,
                        HorsePower = 280,
                        AutomaticTransmission = true,
                        ProductionYear = 1998,
                        Milleage = 400000,
                        OwnerId = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F")
                    };

                    Offer offer4 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "Cls",
                        Body = "Sedan",
                        Description = "100% working car, nice power, no overheating and rust",
                        PriceInEur = 12000,
                        FuelType = "Petrol",
                        EngineSizeInL = 5,
                        HorsePower = 306,
                        AutomaticTransmission = false,
                        ProductionYear = 2007,
                        Milleage = 140000,
                        OwnerId = Guid.Parse("B779E86A-2F75-4E4F-82AB-4C1DEA25A17F")
                    };

                    Offer offer5 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "S Class",
                        Body = "Sedan",
                        Description = "Elegance, massage, full electric seats, parktronic, very gooc conidtion",
                        PriceInEur = 15000,
                        FuelType = "Diesel",
                        EngineSizeInL = 3,
                        HorsePower = 230,
                        AutomaticTransmission = true,
                        ProductionYear = 2010,
                        Milleage = 180000,
                        OwnerId = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2")
                    };

                    Offer offer6 = new Offer()
                    {
                        Brand = "Mercedes",
                        Model = "E Class",
                        Body = "Combi",
                        Description = "Good condition, I need to sell it quick",
                        PriceInEur = 4000,
                        FuelType = "Petrol + LPG",
                        EngineSizeInL = 5,
                        HorsePower = 306,
                        AutomaticTransmission = true,
                        ProductionYear = 2002,
                        Milleage = 350000,
                        OwnerId = Guid.Parse("D0DC591C-1F23-4AB9-BC0E-1D51DCB843C2")
                    };

                    _otomotoContext.Offers.AddRange(offer1, offer2, offer3, offer4, offer5, offer6);
                    _otomotoContext.SaveChanges();
                }
            }
        }
    }
}
