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
    using System.Collections.Generic;
    
    public partial class OTable
    {
        public int OrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> OrderShippingDate { get; set; }
        public string OrderAddress { get; set; }
        public string OrderCity { get; set; }
        public string OrderPostalCode { get; set; }
        public string OrderCountry { get; set; }
        public string ShippingServiceName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> ProductRating { get; set; }
    }
}
