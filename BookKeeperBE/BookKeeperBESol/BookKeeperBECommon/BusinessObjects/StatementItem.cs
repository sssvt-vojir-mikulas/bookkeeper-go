using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("StatementItem")]
    public class StatementItem
    {


        [Column("Statement")]
        public Statement Statement { get; set; }
        [Column("Ordinal")]
        public int Ordinal { get; set; }
        [Column("DateTransaction")]
        public DateTime? DateTransaction { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Accounting")]
        public AccountingCode Accounting { get; set; }
        [Column("Contact")]
        public Contact Contact { get; set; }
        [Column("Amount")]
        public decimal Amount { get; set; }
        [Column("ContraAccount")]
        public string ContraAccount { get; set; }
        [Column("Vs")]
        public string Vs { get; set; }
        [Column("Ks")]
        public string Ks { get; set; }
        [Column("Ss")]
        public string Ss { get; set; }



    }



}
