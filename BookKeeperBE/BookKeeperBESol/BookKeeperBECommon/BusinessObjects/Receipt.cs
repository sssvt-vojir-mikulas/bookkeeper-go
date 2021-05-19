using System;
using System.Collections.Generic;
using BookKeeperBECommon.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("Receipt")]
    public class Receipt
    {


        [Column("Year")]
        public int Year { get; set; }
        [Column("Direction")]
        public DocumentDirection Direction { get; set; }
        [Column("ReceiptNumber")]
        public string ReceiptNumber { get; set; }
        [Column("Contact")]
        public Contact Contact { get; set; }
        [Column("Note")]
        public string Note { get; set; }
        [Column("TotalAmount")]
        public decimal TotalAmount { get; set; }
        [Column("DatePaid")]
        public DateTime? DatePaid { get; set; }
        [Column("IsBooked")]
        public bool IsBooked { get; set; }



        public IList<ReceiptItem> ReceiptItems { get; set; }



    }



}
