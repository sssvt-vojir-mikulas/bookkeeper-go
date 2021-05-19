using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("Statement")]
    public class Statement
    {


        [Column("Year")]
        public int Year { get; set; }
        [Column("StatementNumber")]
        public string StatementNumber { get; set; }
        [Column("DateIssued")]
        public DateTime? DateIssued { get; set; }
        [Column("IsBooked")]
        public bool IsBooked { get; set; }



        public IList<StatementItem> StatementItems { get; set; }



    }



}
