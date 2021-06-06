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
    /// Exposes CRUD methods for the business object Journal.
    /// </summary>
    public class JournalRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all journals.
        /// </summary>
        /// <returns>Returns the list of all journals.</returns>
        public IList<Journal> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.Journals
                            select u;
                //var query = context.Journals;
                var journals = query.ToList<Journal>();

                return journals;

            }
        }



        /// <summary>
        /// Finds all journals matching given criteria (their ID and/or journalname).
        /// </summary>
        /// <param name="journal">Criteria that the found journals should match.</param>
        /// <returns>Returns a list of matching journals.</returns>
        public IList<Journal> FindList(Journal journal)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Journals
                //            where u.Journalname == journal.Journalname
                //            select u;

                IQueryable<Journal> query = BuildQuery(context.Journals, journal);

                var journals = query.ToList<Journal>();
                return journals;

            }
        }



        /// <summary>
        /// Checks the repo for a given journal (their ID and/or journalname).
        /// </summary>
        /// <param name="journal">Journal to check the repo for.</param>
        /// <returns>Returns true :-: the journal exists, false :-: the journal does not exist.</returns>
        public bool Exists(Journal journal)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Journals
                //            where u.Journalname == journal.Journalname
                //            select u;

                IQueryable<Journal> query = BuildQuery(context.Journals, journal);

                var exists = query.Any<Journal>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given journal (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="journal">Information identifying the journal to be loaded (their ID).</param>
        /// <returns>Returns the requested journal. If no such journal exists, the method should throw an exception.</returns>
        public Journal Load(Journal journal)
        {
            if (!Exists(journal))
            {
                throw new Exception($"There's no such journal with ID: {journal.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.Journals.Find(journal.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given journal.
        /// </summary>
        /// <param name="journal">Journal to be persisted in the repo.</param>
        public void Store(Journal journal)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(journal).State = ((journal.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new journal to the repo.
        /// </summary>
        /// <param name="journal">Journal to add.</param>
        public void Add(Journal journal)
        {
            using (var context = new MysqlContext())
            {

                context.Journals.Add(journal);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given journal from the repo.
        /// </summary>
        /// <param name="journal">Journal to remove.</param>
        public void Remove(Journal journal)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(journal).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<Journal> BuildQuery(IQueryable<Journal> query, Journal journal)
        {

            if (journal.ID != 0)
            {
                query = query.Where(u => u.ID == journal.ID);
            }/*
            if (journal.Journalname != null)
            {
                //query = query.Where(u => u.Journalname == journal.Journalname);
                string journalname = journal.Journalname;
                //if ( ! journalname.Contains('*') )
                //{
                //    query = query.Where(u => u.Journalname == journalname);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //journalname = journalname.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.Journalname, journalname));
                //}
                int countStars = journalname.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.Journalname == journalname);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (journalname.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (journalname[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = journalname.Substring(1);
                                query = query.Where(u => u.Journalname.EndsWith(term));
                                //query = query.Where(u => u.Journalname.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (journalname[journalname.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = journalname.Substring(0, journalname.Length - 1);
                                query = query.Where(u => u.Journalname.StartsWith(term));
                                //query = query.Where(u => u.Journalname.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (journalname.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {journalname}");
                                }
                                string[] terms = journalname.Split('*');
                                query = query.Where(u => u.Journalname.StartsWith(terms[0]) && u.Journalname.EndsWith(terms[1]));
                                //query = query.Where(u => u.Journalname.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.Journalname.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((journalname.IndexOf('*') == 0) && (journalname.LastIndexOf('*') == journalname.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {journalname}");
                        }
                        if (journalname.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = journalname.Substring(1, journalname.Length - 2);
                            query = query.Where(u => u.Journalname.Contains(term));
                            //query = query.Where(u => u.Journalname.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {journalname}");
                }
            }*/
            // ...

            return query;

        }



    }



}
