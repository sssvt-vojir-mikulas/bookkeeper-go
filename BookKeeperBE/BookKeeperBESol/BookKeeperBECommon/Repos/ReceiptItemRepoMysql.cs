using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.EF;



namespace BookKeeperBECommon.Repos
{



    /// <summary>
    /// Exposes CRUD methods for the business object Contact.
    /// </summary>
    public class ReceiptItemRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all contacts.
        /// </summary>
        /// <returns>Returns the list of all contacts.</returns>
        public IList<ReceiptItem> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.ReceiptItems
                            select u;
                //var query = context.ReceiptItems;
                var contacts = query.ToList<ReceiptItem>();

                return contacts;

            }
        }



        /// <summary>
        /// Finds all contacts matching given criteria (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Criteria that the found contacts should match.</param>
        /// <returns>Returns a list of matching contacts.</returns>
        public IList<ReceiptItem> FindList(ReceiptItem receiptItem)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.ReceiptItems
                //            where u.Contactname == contact.Contactname
                //            select u;

                IQueryable<ReceiptItem> query = BuildQuery(context.ReceiptItems, receiptItem);

                var contacts = query.ToList<ReceiptItem>();
                return contacts;

            }
        }



        /// <summary>
        /// Checks the repo for a given contact (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Contact to check the repo for.</param>
        /// <returns>Returns true :-: the contact exists, false :-: the contact does not exist.</returns>
        public bool Exists(ReceiptItem receiptItem)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.ReceiptItems
                //            where u.Contactname == contact.Contactname
                //            select u;

                IQueryable<ReceiptItem> query = BuildQuery(context.ReceiptItems, receiptItem);

                var exists = query.Any<ReceiptItem>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given contact (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="contact">Information identifying the contact to be loaded (their ID).</param>
        /// <returns>Returns the requested contact. If no such contact exists, the method should throw an exception.</returns>
        public ReceiptItem Load(ReceiptItem receiptItem)
        {
            if (!Exists(receiptItem))
            {
                throw new Exception($"There's no such contact with ID: {receiptItem.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.ReceiptItems.Find(receiptItem.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given contact.
        /// </summary>
        /// <param name="contact">Contact to be persisted in the repo.</param>
        public void Store(ReceiptItem receiptItem)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(receiptItem).State = ((receiptItem.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new contact to the repo.
        /// </summary>
        /// <param name="contact">Contact to add.</param>
        public void Add(ReceiptItem receiptItem)
        {
            using (var context = new MysqlContext())
            {

                context.ReceiptItems.Add(receiptItem);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given contact from the repo.
        /// </summary>
        /// <param name="contact">Contact to remove.</param>
        public void Remove(ReceiptItem receiptItem)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(receiptItem).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<ReceiptItem> BuildQuery(IQueryable<ReceiptItem> query, ReceiptItem receiptItem)
        {

            if (receiptItem.ID != 0)
            {
                query = query.Where(u => u.ID == receiptItem.ID);
            }/*
            if (contact.Contactname != null)
            {
                //query = query.Where(u => u.Contactname == contact.Contactname);
                string contactname = contact.Contactname;
                //if ( ! contactname.Contains('*') )
                //{
                //    query = query.Where(u => u.Contactname == contactname);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //contactname = contactname.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.Contactname, contactname));
                //}
                int countStars = contactname.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.Contactname == contactname);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (contactname.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (contactname[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = contactname.Substring(1);
                                query = query.Where(u => u.Contactname.EndsWith(term));
                                //query = query.Where(u => u.Contactname.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (contactname[contactname.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = contactname.Substring(0, contactname.Length - 1);
                                query = query.Where(u => u.Contactname.StartsWith(term));
                                //query = query.Where(u => u.Contactname.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (contactname.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {contactname}");
                                }
                                string[] terms = contactname.Split('*');
                                query = query.Where(u => u.Contactname.StartsWith(terms[0]) && u.Contactname.EndsWith(terms[1]));
                                //query = query.Where(u => u.Contactname.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.Contactname.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((contactname.IndexOf('*') == 0) && (contactname.LastIndexOf('*') == contactname.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {contactname}");
                        }
                        if (contactname.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = contactname.Substring(1, contactname.Length - 2);
                            query = query.Where(u => u.Contactname.Contains(term));
                            //query = query.Where(u => u.Contactname.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {contactname}");
                }
            }*/
            // ...

            return query;

        }



    }



}
