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
    public class ContactRepoMysql
    {



        /// <summary>
        /// Gets a complete list of all contacts.
        /// </summary>
        /// <returns>Returns the list of all contacts.</returns>
        public IList<Contact> GetList()
        {
            using (var context = new MysqlContext())
            {

                var query = from u in context.Contacts
                            select u;
                //var query = context.Contacts;
                var contacts = query.ToList<Contact>();

                return contacts;

            }
        }



        /// <summary>
        /// Finds all contacts matching given criteria (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Criteria that the found contacts should match.</param>
        /// <returns>Returns a list of matching contacts.</returns>
        public IList<Contact> FindList(Contact contact)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Contacts
                //            where u.Name == contact.Name
                //            select u;

                IQueryable<Contact> query = BuildQuery(context.Contacts, contact);

                var contacts = query.ToList<Contact>();
                return contacts;

            }
        }



        /// <summary>
        /// Checks the repo for a given contact (their ID and/or contactname).
        /// </summary>
        /// <param name="contact">Contact to check the repo for.</param>
        /// <returns>Returns true :-: the contact exists, false :-: the contact does not exist.</returns>
        public bool Exists(Contact contact)
        {
            using (var context = new MysqlContext())
            {

                //var query = from u in context.Contacts
                //            where u.Name == contact.Name
                //            select u;

                IQueryable<Contact> query = BuildQuery(context.Contacts, contact);

                var exists = query.Any<Contact>();
                return exists;

            }
        }



        /// <summary>
        /// Tries to load data about a given contact (according to their ID) and returns the information loaded.
        /// </summary>
        /// <param name="contact">Information identifying the contact to be loaded (their ID).</param>
        /// <returns>Returns the requested contact. If no such contact exists, the method should throw an exception.</returns>
        public Contact Load(Contact contact)
        {
            if (!Exists(contact))
            {
                throw new Exception($"There's no such contact with ID: {contact.ID}");
            }
            using (var context = new MysqlContext())
            {

                return context.Contacts.Find(contact.ID);

            }
        }



        /// <summary>
        /// Tries to store (persist) data about a given contact.
        /// </summary>
        /// <param name="contact">Contact to be persisted in the repo.</param>
        public void Store(Contact contact)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(contact).State = ((contact.ID == 0) ? (EntityState.Added) : (EntityState.Modified));

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Adds a new contact to the repo.
        /// </summary>
        /// <param name="contact">Contact to add.</param>
        public void Add(Contact contact)
        {
            using (var context = new MysqlContext())
            {

                context.Contacts.Add(contact);

                context.SaveChanges();

            }
        }



        /// <summary>
        /// Removes a given contact from the repo.
        /// </summary>
        /// <param name="contact">Contact to remove.</param>
        public void Remove(Contact contact)
        {
            using (var context = new MysqlContext())
            {

                context.Entry(contact).State = EntityState.Deleted;

                context.SaveChanges();

            }
        }



        private IQueryable<Contact> BuildQuery(IQueryable<Contact> query, Contact contact)
        {

            if (contact.ID != 0)
            {
                query = query.Where(u => u.ID == contact.ID);
            }
            if (contact.Name != null)
            {
                //query = query.Where(u => u.Name == contact.Name);
                string contactname = contact.Name;
                //if ( ! contactname.Contains('*') )
                //{
                //    query = query.Where(u => u.Name == contactname);
                //}
                //else
                //{
                //    // For search terms like 'ba*', replace '*' with '%' and use LIKE (e.g. WHERE USERNAME LIKE 'ba%').
                //    //contactname = contactname.Replace('*', '%');
                //    //query = query.Where(u => SqlMethods.Like(u.Name, contactname));
                //}
                int countStars = contactname.Count(c => c == '*');
                switch (countStars)
                {
                    case 0:
                        // No asterisks (wildcards) at all.
                        query = query.Where(u => u.Name == contactname);
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
                                query = query.Where(u => u.Name.EndsWith(term));
                                //query = query.Where(u => u.Name.EndsWith(term, StringComparison.OrdinalIgnoreCase));
                            }
                            else if (contactname[contactname.Length - 1] == '*')
                            {
                                // Wildcard at the end of the search term.
                                // WHERE USERNAME LIKE 'ba%'
                                string term = contactname.Substring(0, contactname.Length - 1);
                                query = query.Where(u => u.Name.StartsWith(term));
                                //query = query.Where(u => u.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase));
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
                                query = query.Where(u => u.Name.StartsWith(terms[0]) && u.Name.EndsWith(terms[1]));
                                //query = query.Where(u => u.Name.StartsWith(terms[0], StringComparison.OrdinalIgnoreCase) && u.Name.EndsWith(terms[1], StringComparison.OrdinalIgnoreCase));
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
                            query = query.Where(u => u.Name.Contains(term));
                            //query = query.Where(u => u.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"This search term is not supported: {contactname}");
                }
            }
            // ...

            return query;

        }



    }



}
