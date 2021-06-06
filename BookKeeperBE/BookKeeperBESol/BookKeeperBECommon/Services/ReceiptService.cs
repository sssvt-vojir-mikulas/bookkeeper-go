using System;
using System.Collections.Generic;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Repos;



namespace BookKeeperBECommon.Services
{



    public class ReceiptService
    {



        private ReceiptRepoMysql receiptRepo;



        public ReceiptService()
        {
            // Temporary solution.
            this.receiptRepo = new ReceiptRepoMysql();
        }



        public IList<Receipt> GetListOfReceipts()
        {
            return this.receiptRepo.GetList();
        }



        public IList<Receipt> FindListOfReceipts(string receiptNumberPattern)
        {
            Receipt searchCriteriaAsReceipt = new Receipt { ReceiptNumber = $"*{receiptNumberPattern}*" };
            //Receipt searchCriteriaAsReceipt = new Receipt { ReceiptNumber = receiptNumberPattern };
            return this.receiptRepo.FindList(searchCriteriaAsReceipt);
        }



        public IList<Receipt> SearchReceipts(Receipt receipt)
        {
            if ((receipt.ID == 0) && (receipt.ReceiptNumber == null))
            {
                // Empty receipt-search criteria.
                return GetListOfReceipts();
            }
            if ((receipt.ID == 0) && (receipt.ReceiptNumber != null))
            {
                // Only the ReceiptNumber property has been set.
                return FindListOfReceipts(receipt.ReceiptNumber);
            }
            return this.receiptRepo.FindList(receipt);
        }



        public bool ExistsReceipt(int id)
        {
            Receipt receiptToCheck = new Receipt { ID = id };
            bool exists = this.receiptRepo.Exists(receiptToCheck);
            return exists;
        }



        public Receipt LoadReceipt(int id)
        {
            Receipt receiptToLoad = new Receipt { ID = id };
            Receipt receiptLoaded = this.receiptRepo.Load(receiptToLoad);
            return receiptLoaded;
        }



        //public void SaveReceipt(Receipt receipt)
        public Receipt SaveReceipt(Receipt receipt)
        {
            Receipt receiptToReturn = receipt;
            if (receipt.ID == 0)
            {
                this.receiptRepo.Add(receipt);
                // Find all receipts with the given receiptNumber.
                List<Receipt> listOfReceiptsToProcess = (List<Receipt>) this.receiptRepo.FindList(receipt);
                // Sort the list of receipts by their ID's in an ascending order.
                listOfReceiptsToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last receipt (with the greatest ID).
                //receiptToReturn = listOfReceiptsToProcess[0];
                receiptToReturn = listOfReceiptsToProcess[listOfReceiptsToProcess.Count - 1];
            }
            else
            {
                this.receiptRepo.Store(receipt);
            }
            return receiptToReturn;
        }



        //public void DeleteReceipt(int id)
        public Receipt DeleteReceipt(int id)
        {
            Receipt receiptToDelete = new Receipt { ID = id };
            Receipt receiptToDeleteFound = this.receiptRepo.Load(receiptToDelete);
            this.receiptRepo.Remove(receiptToDelete);
            return receiptToDeleteFound;
        }



    }



}
