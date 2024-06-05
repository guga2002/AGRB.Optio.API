using RGBA.Optio.Core.Interfaces;
using Optio.Core.Entities;
using RGBA.Optio.Stream.Interfaces;
using Optio.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace RGBA.Optio.Stream.SeedServices
{
    public class MerchantRelatedSer:IMerchantRelatedSer
    {
        private readonly IUniteOfWork _uniteOfWork;
        private readonly OptioDB optioDB;
        private readonly Random rand;
        private readonly Random rand1;
        public MerchantRelatedSer(IUniteOfWork _uniteOfWork, OptioDB optioDB)
        {
            this._uniteOfWork = _uniteOfWork;
            this.optioDB = optioDB;
            rand= new Random();
            rand1= new Random();
        }

        #region FillDataToLocation

        public async Task<bool> FillDataToLocation()
        {
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "თბილისი",IsActive = true});
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "რუსთავი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ბათუმი", IsActive = true });
            await  _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ქობულეთი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "გარდაბანი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ბაკურიანი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ლანჩუთი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ურეკი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "გორი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "გუდაური", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ზუგდიდი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "სამტრედია", IsActive = true });
            await  _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ფოთი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "მესტია", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "თელავი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "სიღნაღი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ქუთაისი", IsActive = true });
            await _uniteOfWork.LocationRepository.AddAsync(new Location { LocationName = "ამბროლაური", IsActive = true });
            return true;
        }
        #endregion

        #region FillDataMerchant
        public async Task<bool> FillDataMerchant()
        {
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "H&M", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Lutecia", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "AgroHub", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Euro Product", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Nikora", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Carrefour", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Ori nabiji", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Spar", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Libre", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "New-Yourker", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Massimo dutti", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Lacoste", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "D&G", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Armani", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Nike", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Pumma", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Terranova", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Socar", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Wissol", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Gulf", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "McDonald's", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "KFC", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Wendy's", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Biblusi", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "IPlus", IsActive = true });
            await _uniteOfWork.MerchantRepository.AddAsync(new Merchant { Name = "Zoomer", IsActive = true });
            return true;
        }
        #endregion

        #region LocationToMerchant
        public async Task<bool> FillDataLocationToMerchant(int countNumber)
        {

            var minLocationId = await optioDB.Locations.OrderBy(l => l.Id).FirstOrDefaultAsync();
            var maxLocationId= await optioDB.Locations.OrderBy(l => l.Id).LastOrDefaultAsync();
            var minMerchantId = await optioDB.Merchants.OrderBy(l => l.Id).FirstOrDefaultAsync();
            var maxMerchantId = await optioDB.Merchants.OrderBy(l => l.Id).LastOrDefaultAsync();
            for (int i = 0; i < countNumber; i++)
            {
                try
                {
                    if (minLocationId is not null && maxLocationId is not null && minMerchantId is not null && maxMerchantId is not null)
                    {

                        var randMerch = rand.Next((int)minMerchantId.Id, (int)maxMerchantId.Id);
                        var randLocat = rand1.Next((int)minLocationId.Id, (int)maxLocationId.Id);
                        if (await optioDB.Merchants.AnyAsync(i=>i.Id==randMerch) || await optioDB.Locations.AnyAsync(i=>i.Id==randLocat))
                        {
                            await _uniteOfWork.MerchantRepository.AssignLocationtoMerchant(randMerch, randLocat);
                        }
                        else
                        {
                            i--;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    await Console.Out.WriteLineAsync($"shecdopmaaaaaaaaaaa {ex.Message}");
                    Console.ResetColor();
                }
            }
            return true;
        }
        #endregion
    }
}
