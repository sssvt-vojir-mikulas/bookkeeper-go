using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("AccountingCode")]
    public class AccountingCode
    {


        [Column("Code")]
        public string Code { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Note")]
        public string Note { get; set; }
        [Column("IsTaxable")]
        public bool IsTaxable { get; set; }



    }



}
