using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dogue.UI.MVC.Models
{
    public class PortfolioViewModel
    {
        public int PhotoID { get; set; }
        public int FilterID { get; set; }
        public string FilterName { get; set; }
        public string PhotoUrl { get; set; }
        public string Title { get; set; }
        public int OwnerAssetID { get; set; }
        public string AssetCallName { get; set; }
    }
}