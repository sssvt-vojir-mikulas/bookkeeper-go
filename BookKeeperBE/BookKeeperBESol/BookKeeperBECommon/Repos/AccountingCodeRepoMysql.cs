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
    /// Exposes CRUD methods for the business object AccountingCode.
    /// </summary>
    public class AccountingCodeRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all accountingCodes.
        /// </summary>
        /// <returns>Returns the list of all accountingCodes.</returns>
        public IList<AccountingCode> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.AccountingCodes
                            select u;
                //var query = context.AccountingCodes;
                var accountingCodes = query.ToList<AccountingCode>();

                return accountingCodes;

            }
        }



        /// <summary>
        /// Finds all accountingCodes matching given criteria (their ID and/or accountingCodename).
        /// </summary>
        /// <param name="accountingCode">Criteria that the found accountingCodes should match.</param>
        /// <returns>Returns a list of matching accountingCodes.</returns>
        public IList<AccountingCode> FindList(AccountingCode accountingCode)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.AccountingCodes
                //            where u.AccountingCodename == accountingCode.AccountingCodename
                //            select u;

                IQueryable<AccountingCode> query = BuildQuery(context.AccountingCodes, accountingCode);

                var accountingCodes = query.ToList<AccountingCode>();
                return accountingCodes;

            }
        }



        /// <summary>
        /// Checks the repo for a given accountingCode (their ID and/or accountingCodename).
        /// </summary>
        /// <param name="accountingCode">AccountingCode to check the repo for.</param>
        /// <returns>Returns true :-: the accountingCode exists, false :-: the accountingCode does not exist.</returns>
        public bool Exists(AccountingCode accountingCode)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.AccountingCodes
                //            where u.AccountingCodename == accountingCode.AccountingCodename
                //            select u;

                IQueryable<AccountingCode> query = BuildQuery(context.AccountingCodes, accountingCode);

                var exists = query.Any<AccountingCode>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given accountingCode (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="accountingCode">Information identifying the accountingCode to be loaded (their ID).</param>
        /// <returns>Returns the requested accountingCode. If no such accountingCode exists, the method should throw an exception.</returns>
        public AccountingCode Load(AccountingCode accountingCode)
        {
            if (!Exists(accountingCode))
            {
                throw new Exception($"There's no such accountingCode with ID: {accountingCode.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.AccountingCodes.Find(accountingCode.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given accountingCode.
        /// </summary>
        /// <param name="accountingCode">AccountingCode to be persisted in the repo.</param>
        public void Store(AccountingCode accountingCode)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(accountingCode).State = ((accountingCode.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new accountingCode to the repo.
        /// </summary>
        /// <param name="accountingCode">AccountingCode to add.</param>
        public void Add(AccountingCode accountingCode)
        {
            using (var context = new MysqlContext())
            {

                context.AccountingCodes.Add(accountingCode);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given accountingCode from the repo.
        /// </summary>
        /// <param name="accountingCode">AccountingCode to remove.</param>
        public void Remove(AccountingCode accountingCode)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(accountingCode).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<AccountingCode> BuildQuery(IQueryable<AccountingCode> query, AccountingCode accountingCode)
        {

            if (accountingCode.ID != 0)
            {
                query = query.Where(u => u.ID == accountingCode.ID);
            }/*
            if (accountingCode.AccountingCodename != null)
            {
                //query = query.Where(u => u.AccountingCodename == accountingCode.AccountingCodename);
                string accountingCodename = accountingCode.AccountingCodename;
                //if ( ! accountingCodename.Contains('*') )
                //{
                //    query = query.Where(u => u.AccountingCodename == accountingCodename);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //accountingCodename = accountingCodename.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.AccountingCodename, accountingCodename));
                //}
                int countStars = accountingCodename.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.AccountingCodename == accountingCodename);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (accountingCodename.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (accountingCodename[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = accountingCodename.Substring(1);
                                query = query.Where(u => u.AccountingCodename.EndsWith(term));
                                //query = query.Where(u => u.AccountingCodename.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (accountingCodename[accountingCodename.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = accountingCodename.Substring(0, accountingCodename.Length - 1);
                                query = query.Where(u => u.AccountingCodename.StartsWith(term));
                                //query = query.Where(u => u.AccountingCodename.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (accountingCodename.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {accountingCodename}");
                                }
                                string[] terms = accountingCodename.Split('*');
                                query = query.Where(u => u.AccountingCodename.StartsWith(terms[0]) && u.AccountingCodename.EndsWith(terms[1]));
                                //query = query.Where(u => u.AccountingCodename.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.AccountingCodename.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((accountingCodename.IndexOf('*') == 0) && (accountingCodename.LastIndexOf('*') == accountingCodename.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {accountingCodename}");
                        }
                        if (accountingCodename.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = accountingCodename.Substring(1, accountingCodename.Length - 2);
                            query = query.Where(u => u.AccountingCodename.Contains(term));
                            //query = query.Where(u => u.AccountingCodename.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {accountingCodename}");
                }
            }*/
            // ...

            return query;

        }



    }



}
