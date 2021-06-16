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



        public IList<Contact> GetListOfUsers()
        {
            return this.contactRepo.GetList();
        }



        public IList<Contact> FindListOfUsers(string namePattern)
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
                return GetListOfUsers();
            }
            if ((contact.ID == 0) && (contact.Name != null))
            {
                // Only the Username property has been set.
                return FindListOfUsers(contact.Name);
            }
            return this.contactRepo.FindList(contact);
        }



        public bool ExistsUser(int id)
        {
            Contact userToCheck = new Contact { ID = id };
            bool exists = this.contactRepo.Exists(userToCheck);
            return exists;
        }



        public Contact LoadUser(int id)
        {
            Contact userToLoad = new Contact { ID = id };
            Contact userLoaded = this.contactRepo.Load(userToLoad);
            return userLoaded;
        }



        //public void SaveUser(User user)
        public Contact SaveUser(Contact user)
        {
            Contact userToReturn = user;
            if (user.ID == 0)
            {
                this.contactRepo.Add(user);
                // Find all users with the given username.
                List<Contact> listOfUsersToProcess = (List<Contact>) this.contactRepo.FindList(user);
                // Sort the list of users by their ID's in an ascending order.
                listOfUsersToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last user (with the greatest ID).
                //userToReturn = listOfUsersToProcess[0];
                userToReturn = listOfUsersToProcess[listOfUsersToProcess.Count - 1];
            }
            else
            {
                this.contactRepo.Store(user);
            }
            return userToReturn;
        }



        //public void DeleteUser(int id)
        public Contact DeleteUser(int id)
        {
            Contact userToDelete = new Contact { ID = id };
            Contact userToDeleteFound = this.contactRepo.Load(userToDelete);
            this.contactRepo.Remove(userToDelete);
            return userToDeleteFound;
        }



    }



}
