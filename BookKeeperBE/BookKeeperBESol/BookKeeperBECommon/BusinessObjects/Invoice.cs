using System;
using System.Collections.Generic;
using BookKeeperBECommon.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("Invoice")]
    public class Invoice
    {


        [Column("Year")]
        public int Year { get; set; }

        [Column("Direction")]
        public DocumentDirection Direction { get; set; }

        [Column("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [Column("Contact")]
        public Contact Contact { get; set; }

        [Column("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [Column("DateIssued")]
        public DateTime? DateIssued { get; set; }

        [Column("DateDue")]
        public DateTime? DateDue { get; set; }



        public IList<InvoiceItem> InvoiceItems { get; set; }



    }



}
