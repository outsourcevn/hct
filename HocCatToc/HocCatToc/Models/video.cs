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
    
    public partial class video
    {
        public long id { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string img { get; set; }
        public string des { get; set; }
        public string date { get; set; }
        public string tag { get; set; }
        public string viewcount { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<System.DateTime> update_date { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> cat_id { get; set; }
    }
}
