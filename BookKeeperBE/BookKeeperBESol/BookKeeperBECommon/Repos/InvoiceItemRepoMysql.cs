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
    public class InvoiceItemsRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all contacts.
        /// </summary>
        /// <returns>Returns the list of all contacts.</returns>
        public IList<InvoiceItem> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.InvoiceItems
                            select u;
                //var query = context.InvoiceItems;
                var contacts = query.ToList<InvoiceItem>();

                return contacts;

            }
        }



        /// <summary>
        /// Finds all contacts matching given criteria (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Criteria that the found contacts should match.</param>
        /// <returns>Returns a list of matching contacts.</returns>
        public IList<InvoiceItem> FindList(InvoiceItem invoiceItem)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.InvoiceItems
                //            where u.Contactname == contact.Contactname
                //            select u;

                IQueryable<InvoiceItem> query = BuildQuery(context.InvoiceItems, invoiceItem);

                var contacts = query.ToList<InvoiceItem>();
                return contacts;

            }
        }



        /// <summary>
        /// Checks the repo for a given contact (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Contact to check the repo for.</param>
        /// <returns>Returns true :-: the contact exists, false :-: the contact does not exist.</returns>
        public bool Exists(InvoiceItem invoiceItem)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.InvoiceItems
                //            where u.Contactname == contact.Contactname
                //            select u;

                IQueryable<InvoiceItem> query = BuildQuery(context.InvoiceItems, invoiceItem);

                var exists = query.Any<InvoiceItem>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given contact (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="contact">Information identifying the contact to be loaded (their ID).</param>
        /// <returns>Returns the requested contact. If no such contact exists, the method should throw an exception.</returns>
        public InvoiceItem Load(InvoiceItem invoiceItem)
        {
            if (!Exists(invoiceItem))
            {
                throw new Exception($"There's no such contact with ID: {invoiceItem.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.InvoiceItems.Find(invoiceItem.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given contact.
        /// </summary>
        /// <param name="contact">Contact to be persisted in the repo.</param>
        public void Store(InvoiceItem invoiceItem)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(invoiceItem).State = ((invoiceItem.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new contact to the repo.
        /// </summary>
        /// <param name="contact">Contact to add.</param>
        public void Add(InvoiceItem invoiceItem)
        {
            using (var context = new MysqlContext())
            {

                context.InvoiceItems.Add(invoiceItem);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given contact from the repo.
        /// </summary>
        /// <param name="contact">Contact to remove.</param>
        public void Remove(InvoiceItem invoiceItem)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(invoiceItem).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<InvoiceItem> BuildQuery(IQueryable<InvoiceItem> query, InvoiceItem invoiceItem)
        {

            if (invoiceItem.ID != 0)
            {
                query = query.Where(u => u.ID == invoiceItem.ID);
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
