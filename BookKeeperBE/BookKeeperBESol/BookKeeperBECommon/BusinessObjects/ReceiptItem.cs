﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("BK_RECEIPT_ITEM")]
    public class ReceiptItem
    {

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [ForeignKey]
        [Column("RECEIPT_ID")]
        public Receipt Receipt { get; set; }

        [Column("ORDINAL")]
        public int Ordinal { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [ForeignKey]
        [Column("ACCOUNTING_CODE_ID")]
        public AccountingCode Accounting { get; set; }

        [Column("AMOUNT")]
        public decimal Amount { get; set; }



    }



}
