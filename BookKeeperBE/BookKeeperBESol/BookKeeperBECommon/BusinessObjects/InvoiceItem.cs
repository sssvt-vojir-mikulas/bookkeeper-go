using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("InvoiceItem")]
    public class InvoiceItem
    {

        
        [Column("Invoice")]
        public Invoice Invoice { get; set; }

        [Column("Ordinal")]
        public int Ordinal { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Quantity")]
        public double Quantity { get; set; }

        [Column("AmountPerUnit")]
        public decimal AmountPerUnit { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }



    }



}
