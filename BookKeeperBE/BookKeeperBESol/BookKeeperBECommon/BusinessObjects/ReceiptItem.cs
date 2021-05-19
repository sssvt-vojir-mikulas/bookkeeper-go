using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("ReceiptItem")]
    public class ReceiptItem
    {


        [Column("Receipt")]
        public Receipt Receipt { get; set; }
        [Column("Ordinal")]
        public int Ordinal { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Accounting")]
        public AccountingCode Accounting { get; set; }
        [Column("Amount")]
        public decimal Amount { get; set; }



    }



}
