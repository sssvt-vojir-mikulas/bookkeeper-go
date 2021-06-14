﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("BK_JOURNAL")]
    public class Journal
    {

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("YEAR")]
        public int Year { get; set; }

        [Column("ORDINAL")]
        public int Ordinal { get; set; }


        //[ForeignKey]
        [Column("STATEMENT_ITEM_ID")]
        public StatementItem StatementItem { get; set; }

        //[ForeignKey]
        [Column("RECEIPT_ITEM_ID")]
        public ReceiptItem ReceiptItem { get; set; }

        //[ForeignKey]
        [Column("INVOICE_ID")]
        public Invoice Invoice { get; set; }

        [Column("DATE_BOOKED")]
        public DateTime DateBooked { get; set; }


        [Column("DATE_TRANSACTION")]
        public DateTime? DateTransaction { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        //[ForeignKey]
        [Column("ACCOUNTING_CODE_ID")]
        public AccountingCode AccountingCode { get; set; }


        [Column("CASH_CREDIT")]
        public decimal? CashCredit { get; set; }

        [Column("CASH_DEBIT")]
        public decimal? CashDebit { get; set; }

        [Column("CASH_BALANCE")]
        public decimal? CashBalance { get; set; }

        [Column("BANK_CREDIT")]
        public decimal? BankCredit { get; set; }

        [Column("BANK_DEBIT")]
        public decimal? BankDebit { get; set; }

        [Column("BANK_BALANCE")]
        public decimal? BankBalance { get; set; }
    }
}
