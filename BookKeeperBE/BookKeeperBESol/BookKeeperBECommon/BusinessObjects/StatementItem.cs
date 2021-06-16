using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("BK_STATEMENT_ITEM")]
    public class StatementItem
    {

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [ForeignKey("Statement")]
        [Column("STATEMENT_ID")]
        public int StatementId { get; set; }
        public Statement Statement { get; set; }

        [Column("ORDINAL")]
        public int Ordinal { get; set; }

        [Column("DATE_TRANSACTION")]
        public DateTime? DateTransaction { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [ForeignKey("Accounting")]
        [Column("ACCOUNTING_CODE_ID")]
        public int AccountingId { get; set; }
        public AccountingCode Accounting { get; set; }

        [ForeignKey("Contact")]
        [Column("CONTACT_ID")]
        public int ContactId { get; set; }
        public Contact Contact { get; set; }

        [Column("AMOUNT")]
        public decimal Amount { get; set; }

        [Column("CONTRA_ACCOUNT")]
        public string ContraAccount { get; set; }

        [Column("VS")]
        public string Vs { get; set; }

        [Column("KS")]
        public string Ks { get; set; }

        [Column("SS")]
        public string Ss { get; set; }



    }



}
