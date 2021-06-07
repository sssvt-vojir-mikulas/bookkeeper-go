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
    /// Exposes CRUD methods for the business object Statement.
    /// </summary>
    public class StatementRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all statements.
        /// </summary>
        /// <returns>Returns the list of all statements.</returns>
        public IList<Statement> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.Statements
                            select u;
                //var query = context.Statements;
                var statements = query.ToList<Statement>();

                return statements;

            }
        }



        /// <summary>
        /// Finds all statements matching given criteria (their ID and/or statementnumber).
        /// </summary>
        /// <param name="statement">Criteria that the found statements should match.</param>
        /// <returns>Returns a list of matching statements.</returns>
        public IList<Statement> FindList(Statement statement)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Statements
                //            where u.StatementNumber == statement.StatementNumber
                //            select u;

                IQueryable<Statement> query = BuildQuery(context.Statements, statement);

                var statements = query.ToList<Statement>();
                return statements;

            }
        }



        /// <summary>
        /// Checks the repo for a given statement (their ID and/or statementnumber).
        /// </summary>
        /// <param name="statement">Statement to check the repo for.</param>
        /// <returns>Returns true :-: the statement exists, false :-: the statement does not exist.</returns>
        public bool Exists(Statement statement)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Statements
                //            where u.StatementNumber == statement.StatementNumber
                //            select u;

                IQueryable<Statement> query = BuildQuery(context.Statements, statement);

                var exists = query.Any<Statement>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given statement (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="statement">Information identifying the statement to be loaded (their ID).</param>
        /// <returns>Returns the requested statement. If no such statement exists, the method should throw an exception.</returns>
        public Statement Load(Statement statement)
        {
            if (!Exists(statement))
            {
                throw new Exception($"There's no such statement with ID: {statement.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.Statements.Find(statement.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given statement.
        /// </summary>
        /// <param name="statement">Statement to be persisted in the repo.</param>
        public void Store(Statement statement)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(statement).State = ((statement.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new statement to the repo.
        /// </summary>
        /// <param name="statement">Statement to add.</param>
        public void Add(Statement statement)
        {
            using (var context = new MysqlContext())
            {

                context.Statements.Add(statement);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given statement from the repo.
        /// </summary>
        /// <param name="statement">Statement to remove.</param>
        public void Remove(Statement statement)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(statement).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<Statement> BuildQuery(IQueryable<Statement> query, Statement statement)
        {

            if (statement.ID != 0)
            {
                query = query.Where(u => u.ID == statement.ID);
            }
            if (statement.StatementNumber != null)
            {
                //query = query.Where(u => u.StatementNumber == statement.StatementNumber);
                string statementnumber = statement.StatementNumber;
                //if ( ! statementnumber.Contains('*') )
                //{
                //    query = query.Where(u => u.StatementNumber == statementnumber);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //statementnumber = statementnumber.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.StatementNumber, statementnumber));
                //}
                int countStars = statementnumber.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.StatementNumber == statementnumber);
                        break;
                    case 1:
                        // One asterisk.
                        // One asterisk may be at the beginning, in the middle or at the end of the search term.
                        if (statementnumber.Length > 1)
                        {
                            // Expect one non-asterisk character at least.
                            if (statementnumber[0] == '*')
                            {
                                // Wildcard at the beginning of the search term.
                                // WHERE USERNAME LIKE '%ba'
                                string term = statementnumber.Substring(1);
                                query = query.Where(u => u.StatementNumber.EndsWith(term));
                                //query = query.Where(u => u.StatementNumber.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (statementnumber[statementnumber.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = statementnumber.Substring(0, statementnumber.Length - 1);
                                query = query.Where(u => u.StatementNumber.StartsWith(term));
                                //query = query.Where(u => u.StatementNumber.StartsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else
                            {
                                // Wildcard in the middle of the search term.
                                // WHERE USERNAME LIKE 'na%ta'
                                // There must be at least 3 characters in such a string.
                                if (statementnumber.Length < 3)
                                {
                                    // This should never happen.
                                    throw new Exception($"This situation is not expected. The search term: {statementnumber}");
                                }
                                string[] terms = statementnumber.Split('*');
                                query = query.Where(u => u.StatementNumber.StartsWith(terms[0]) && u.StatementNumber.EndsWith(terms[1]));
                                //query = query.Where(u => u.StatementNumber.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.StatementNumber.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
                            }
                        }
                        break;
                    case 2:
                        // In case of two asterisks, we expect only this: *ba*. No other variants are allowed.
                        if (!((statementnumber.IndexOf('*') == 0) && (statementnumber.LastIndexOf('*') == statementnumber.Length - 1)))
                        {
                            throw new NotSupportedException($"This search term is not supported: {statementnumber}");
                        }
                        if (statementnumber.Length > 2)
                        {
                            // Expect one non-asterisk character at least.
                            // WHERE USERNAME LIKE '%ba%'
                            string term = statementnumber.Substring(1, statementnumber.Length - 2);
                            query = query.Where(u => u.StatementNumber.Contains(term));
                            //query = query.Where(u => u.StatementNumber.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {statementnumber}");
                }
            }
            // ...

            return query;

        }



    }



}
