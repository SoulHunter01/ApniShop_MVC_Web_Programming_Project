//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApniShopWebAssignment.Models
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    
    public partial class UPTable
    {
        public int UPID { get; set; }
        public string UPNAME { get; set; }
        public string UPImage { get; set; }
        public string UPCategory { get; set; }
        public Nullable<int> UPRating { get; set; }
        public Nullable<double> UPPrice { get; set; }
        public Nullable<int> UPWish { get; set; }
        public Nullable<int> UPQuantity { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
