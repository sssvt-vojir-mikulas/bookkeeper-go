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
    /// Exposes CRUD methods for the business object Receipt.
    /// </summary>
    public class ReceiptRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all receipts.
        /// </summary>
        /// <returns>Returns the list of all receipts.</returns>
        public IList<Receipt> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.Receipts
                            select u;
                //var query = context.Receipts;
                var receipts = query.ToList<Receipt>();

                return receipts;

            }
        }



        /// <summary>
        /// Finds all receipts matching given criteria (their ID and/or receiptnumber).
        /// </summary>
        /// <param name="receipt">Criteria that the found receipts should match.</param>
        /// <returns>Returns a list of matching receipts.</returns>
        public IList<Receipt> FindList(Receipt receipt)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Receipts
                //            where u.ReceiptNumber == receipt.ReceiptNumber
                //            select u;

                IQueryable<Receipt> query = BuildQuery(context.Receipts, receipt);

                var receipts = query.ToList<Receipt>();
                return receipts;

            }
        }



        /// <summary>
        /// Checks the repo for a given receipt (their ID and/or receiptnumber).
        /// </summary>
        /// <param name="receipt">Receipt to check the repo for.</param>
        /// <returns>Returns true :-: the receipt exists, false :-: the receipt does not exist.</returns>
        public bool Exists(Receipt receipt)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Receipts
                //            where u.ReceiptNumber == receipt.ReceiptNumber
                //            select u;

                IQueryable<Receipt> query = BuildQuery(context.Receipts, receipt);

                var exists = query.Any<Receipt>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given receipt (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="receipt">Information identifying the receipt to be loaded (their ID).</param>
        /// <returns>Returns the requested receipt. If no such receipt exists, the method should throw an exception.</returns>
        public Receipt Load(Receipt receipt)
        {
            if (!Exists(receipt))
            {
                throw new Exception($"There's no such receipt with ID: {receipt.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.Receipts.Find(receipt.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given receipt.
        /// </summary>
        /// <param name="receipt">Receipt to be persisted in the repo.</param>
        public void Store(Receipt receipt)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(receipt).State = ((receipt.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new receipt to the repo.
        /// </summary>
        /// <param name="receipt">Receipt to add.</param>
        public void Add(Receipt receipt)
        {
            using (var context = new MysqlContext())
            {

                context.Receipts.Add(receipt);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given receipt from the repo.
        /// </summary>
        /// <param name="receipt">Receipt to remove.</param>
        public void Remove(Receipt receipt)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(receipt).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<Receipt> BuildQuery(IQueryable<Receipt> query, Receipt receipt)
        {

            if (receipt.ID != 0)
            {
                query = query.Where(u => u.ID == receipt.ID);
            }
            if (receipt.ReceiptNumber != null)
            {
                //query = query.Where(u => u.ReceiptNumber == receipt.ReceiptNumber);
                string receiptnumber = receipt.ReceiptNumber;
                //if ( ! receiptnumber.Contains('*') )
                //{
                //    query = query.Where(u => u.ReceiptNumber == receiptnumber);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //receiptnumber = receiptnumber.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.ReceiptNumber, receiptnumber));
                //}
                int countStars = receiptnumber.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.ReceiptNumber == receiptnumber);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (receiptnumber.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (receiptnumber[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = receiptnumber.Substring(1);
                                query = query.Where(u => u.ReceiptNumber.EndsWith(term));
                                //query = query.Where(u => u.ReceiptNumber.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (receiptnumber[receiptnumber.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = receiptnumber.Substring(0, receiptnumber.Length - 1);
                                query = query.Where(u => u.ReceiptNumber.StartsWith(term));
                                //query = query.Where(u => u.ReceiptNumber.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (receiptnumber.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {receiptnumber}");
                                }
                                string[] terms = receiptnumber.Split('*');
                                query = query.Where(u => u.ReceiptNumber.StartsWith(terms[0]) && u.ReceiptNumber.EndsWith(terms[1]));
                                //query = query.Where(u => u.ReceiptNumber.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.ReceiptNumber.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((receiptnumber.IndexOf('*') == 0) && (receiptnumber.LastIndexOf('*') == receiptnumber.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {receiptnumber}");
                        }
                        if (receiptnumber.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = receiptnumber.Substring(1, receiptnumber.Length - 2);
                            query = query.Where(u => u.ReceiptNumber.Contains(term));
                            //query = query.Where(u => u.ReceiptNumber.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {receiptnumber}");
                }
            }
            // ...

            return query;

        }



    }



}
