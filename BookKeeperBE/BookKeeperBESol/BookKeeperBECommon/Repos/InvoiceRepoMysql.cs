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
    /// Exposes CRUD methods for the business object Invoice.
    /// </summary>
    public class InvoiceRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all invoices.
        /// </summary>
        /// <returns>Returns the list of all invoices.</returns>
        public IList<Invoice> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.Invoices
                            select u;
                //var query = context.Invoices;
                var invoices = query.ToList<Invoice>();

                return invoices;

            }
        }



        /// <summary>
        /// Finds all invoices matching given criteria (their ID and/or invoiceNumber).
        /// </summary>
        /// <param name="invoice">Criteria that the found invoices should match.</param>
        /// <returns>Returns a list of matching invoices.</returns>
        public IList<Invoice> FindList(Invoice invoice)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Invoices
                //            where u.InvoiceNumber == invoice.InvoiceNumber
                //            select u;

                IQueryable<Invoice> query = BuildQuery(context.Invoices, invoice);

                var invoices = query.ToList<Invoice>();
                return invoices;

            }
        }



        /// <summary>
        /// Checks the repo for a given invoice (their ID and/or invoiceNumber).
        /// </summary>
        /// <param name="invoice">Invoice to check the repo for.</param>
        /// <returns>Returns true :-: the invoice exists, false :-: the invoice does not exist.</returns>
        public bool Exists(Invoice invoice)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Invoices
                //            where u.InvoiceNumber == invoice.InvoiceNumber
                //            select u;

                IQueryable<Invoice> query = BuildQuery(context.Invoices, invoice);

                var exists = query.Any<Invoice>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given invoice (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="invoice">Information identifying the invoice to be loaded (their ID).</param>
        /// <returns>Returns the requested invoice. If no such invoice exists, the method should throw an exception.</returns>
        public Invoice Load(Invoice invoice)
        {
            if (!Exists(invoice))
            {
                throw new Exception($"There's no such invoice with ID: {invoice.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.Invoices.Find(invoice.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given invoice.
        /// </summary>
        /// <param name="invoice">Invoice to be persisted in the repo.</param>
        public void Store(Invoice invoice)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(invoice).State = ((invoice.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new invoice to the repo.
        /// </summary>
        /// <param name="invoice">Invoice to add.</param>
        public void Add(Invoice invoice)
        {
            using (var context = new MysqlContext())
            {

                context.Invoices.Add(invoice);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given invoice from the repo.
        /// </summary>
        /// <param name="invoice">Invoice to remove.</param>
        public void Remove(Invoice invoice)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(invoice).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<Invoice> BuildQuery(IQueryable<Invoice> query, Invoice invoice)
        {

            if (invoice.ID != 0)
            {
                query = query.Where(u => u.ID == invoice.ID);
            }
            if (invoice.InvoiceNumber != null)
            {
                //query = query.Where(u => u.InvoiceNumber == invoice.InvoiceNumber);
                string invoiceNumber = invoice.InvoiceNumber;
                //if ( ! invoiceNumber.Contains('*') )
                //{
                //    query = query.Where(u => u.InvoiceNumber == invoiceNumber);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //invoiceNumber = invoiceNumber.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.InvoiceNumber, invoiceNumber));
                //}
                int countStars = invoiceNumber.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.InvoiceNumber == invoiceNumber);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (invoiceNumber.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (invoiceNumber[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = invoiceNumber.Substring(1);
                                query = query.Where(u => u.InvoiceNumber.EndsWith(term));
                                //query = query.Where(u => u.InvoiceNumber.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (invoiceNumber[invoiceNumber.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = invoiceNumber.Substring(0, invoiceNumber.Length - 1);
                                query = query.Where(u => u.InvoiceNumber.StartsWith(term));
                                //query = query.Where(u => u.InvoiceNumber.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (invoiceNumber.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {invoiceNumber}");
                                }
                                string[] terms = invoiceNumber.Split('*');
                                query = query.Where(u => u.InvoiceNumber.StartsWith(terms[0]) && u.InvoiceNumber.EndsWith(terms[1]));
                                //query = query.Where(u => u.InvoiceNumber.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.InvoiceNumber.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((invoiceNumber.IndexOf('*') == 0) && (invoiceNumber.LastIndexOf('*') == invoiceNumber.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {invoiceNumber}");
                        }
                        if (invoiceNumber.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = invoiceNumber.Substring(1, invoiceNumber.Length - 2);
                            query = query.Where(u => u.InvoiceNumber.Contains(term));
                            //query = query.Where(u => u.InvoiceNumber.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {invoiceNumber}");
                }
            }
            // ...

            return query;

        }



    }



}
