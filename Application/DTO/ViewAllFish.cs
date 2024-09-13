using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ViewAllFish
    {
        public string FishName { get; set; }
        public double FishQuantity { get; set; }
        public decimal FishPrice { get; set; }
        public string FishImage { get; set; }
    }
}
