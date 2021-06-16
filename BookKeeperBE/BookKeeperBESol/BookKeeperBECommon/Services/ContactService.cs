using System;
using System.Collections.Generic;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Repos;



namespace BookKeeperBECommon.Services
{



    public class ContactService
    {



        private ContactRepoMysql contactRepo;



        public ContactService()
        {
            // Temporary solution.
            this.contactRepo = new ContactRepoMysql();
        }



        public IList<Contact> GetListOfContacts()
        {
            return this.contactRepo.GetList();
        }



        public IList<Contact> FindListOfContacts(string namePattern)
        {
            Contact searchCriteriaAsUser = new Contact { Name = $"*{namePattern}*" };
            //User searchCriteriaAsUser = new User { Username = usernamePattern };
            return this.contactRepo.FindList(searchCriteriaAsUser);
        }



        public IList<Contact> SearchContacts(Contact contact)
        {
            if ((contact.ID == 0) && (contact.Name == null))
            {
                // Empty user-search criteria.
                return GetListOfContacts();
            }
            if ((contact.ID == 0) && (contact.Name != null))
            {
                // Only the Username property has been set.
                return FindListOfContacts(contact.Name);
            }
            return this.contactRepo.FindList(contact);
        }



        public bool ExistsContact(int id)
        {
            Contact userToCheck = new Contact { ID = id };
            bool exists = this.contactRepo.Exists(userToCheck);
            return exists;
        }



        public Contact LoadContact(int id)
        {
            Contact userToLoad = new Contact { ID = id };
            Contact userLoaded = this.contactRepo.Load(userToLoad);
            return userLoaded;
        }



        //public void SaveUser(User user)
        public Contact SaveContact(Contact contact)
        {
            Contact contactToReturn = contact;
            if (contact.ID == 0)
            {
                this.contactRepo.Add(contact);
                // Find all users with the given username.
                List<Contact> listOfUsersToProcess = (List<Contact>) this.contactRepo.FindList(contact);
                // Sort the list of users by their ID's in an ascending order.
                listOfUsersToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last user (with the greatest ID).
                //userToReturn = listOfUsersToProcess[0];
                contactToReturn = listOfUsersToProcess[listOfUsersToProcess.Count - 1];
            }
            else
            {
                this.contactRepo.Store(contact);
            }
            return contactToReturn;
        }



        //public void DeleteUser(int id)
        public Contact DeleteContact(int id)
        {
            Contact userToDelete = new Contact { ID = id };
            Contact userToDeleteFound = this.contactRepo.Load(userToDelete);
            this.contactRepo.Remove(userToDelete);
            return userToDeleteFound;
        }



    }



}
