using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBA.Optio.Domain.Models.ResponseModels
{
    public class LocationResponseModel
    {
        public string Location { get; set; }
        public string MerchantName {  get; set; }
        public decimal Volume { get; set; }
        public long Quantity { get; set; }
        public decimal Average { get; set; }
    }
}
