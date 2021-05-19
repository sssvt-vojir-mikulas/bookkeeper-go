using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("Journal")]
    public class Journal
    {


        [Column("Year")]
        public int Year { get; set; }
        [Column("Ordinal")]
        public int Ordinal { get; set; }
        [Column("StatementItem")]
        public StatementItem StatementItem { get; set; }
        [Column("ReceiptItem")]
        public ReceiptItem ReceiptItem { get; set; }
        [Column("Invoice")]
        public Invoice Invoice { get; set; }
        [Column("DateBooked")]
        public DateTime DateBooked { get; set; }


        [Column("DateTransaction")]
        public DateTime? DateTransaction { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("AccountingCode")]
        public AccountingCode AccountingCode { get; set; }


        [Column("CashCredit")]
        public decimal? CashCredit { get; set; }
        [Column("CashDebit")]
        public decimal? CashDebit { get; set; }
        [Column("CashBalance")]
        public decimal? CashBalance { get; set; }
        [Column("BankCredit")]
        public decimal? BankCredit { get; set; }
        [Column("BankDebit")]
        public decimal? BankDebit { get; set; }
        [Column("BankBalance")]
        public decimal? BankBalance { get; set; }



    }



}
