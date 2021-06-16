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


        [ForeignKey("StatementItem")]
        [Column("STATEMENT_ITEM_ID")]
        public int StatementItemId { get; set; }
        public StatementItem StatementItem { get; set; }

        [ForeignKey("ReceiptItem")]
        [Column("RECEIPT_ITEM_ID")]
        public int ReceiptItemId { get; set; }
        public ReceiptItem ReceiptItem { get; set; }

        [ForeignKey("Invoice")]
        [Column("INVOICE_ID")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [Column("DATE_BOOKED")]
        public DateTime DateBooked { get; set; }


        [Column("DATE_TRANSACTION")]
        public DateTime? DateTransaction { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [ForeignKey("AccountingCode")]
        [Column("ACCOUNTING_CODE_ID")]
        public int AccountingCodeId { get; set; }
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
