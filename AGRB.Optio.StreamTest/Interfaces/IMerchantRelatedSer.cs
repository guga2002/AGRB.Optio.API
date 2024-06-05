namespace RGBA.Optio.Stream.Interfaces
{
    public interface IMerchantRelatedSer
    {
        Task<bool> FillDataToLocation();
        Task<bool> FillDataMerchant();
        Task<bool> FillDataLocationToMerchant(int countNumber);
    }
}
