//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HocCatToc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string pass { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public Nullable<int> is_admin { get; set; }
        public Nullable<int> points { get; set; }
        public string identify { get; set; }
        public string address { get; set; }
        public Nullable<int> is_paid { get; set; }
        public Nullable<int> group_id { get; set; }
        public string group_name { get; set; }
        public string group_id_list { get; set; }
        public string token { get; set; }
    }
}
