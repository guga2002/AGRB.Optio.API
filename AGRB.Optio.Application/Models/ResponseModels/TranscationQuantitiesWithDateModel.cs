using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBA.Optio.Domain.Models.ResponseModels
{
    public class TranscationQuantitiesWithDateModel
    {
        public DateTime Date {  get; set; }

        public decimal SubTotal { get; set; }
    }
}
