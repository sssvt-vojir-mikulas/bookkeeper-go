using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookKeeperBECommon.BusinessObjects
{


    [Table("Contact")]
    public class Contact
    {


        [Column("Name")]
        public string Name { get; set; }

        [Column("Ico")]
        public string Ico { get; set; }

        [Column("Dic")]
        public string Dic { get; set; }

        [Column("BankAccount")]
        public string BankAccount { get; set; }

        [Column("Mobile")]
        public string Mobile { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Www")]
        public string Www { get; set; }

        [Column("AddressStreet")]
        public string AddressStreet { get; set; }

        [Column("AddressCity")]
        public string AddressCity { get; set; }

        [Column("AddressZip")]
        public string AddressZip { get; set; }

        [Column("AddressCountry")]
        public string AddressCountry { get; set; }



    }



}
