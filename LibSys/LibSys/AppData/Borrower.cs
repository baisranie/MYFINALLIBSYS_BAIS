//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibSys.AppData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Borrower
    {
        public int BorrowerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Age { get; set; }
        public int DaysBorrow { get; set; }
        public Nullable<int> book_accession_number { get; set; }
    
        public virtual book book { get; set; }
    }
}
