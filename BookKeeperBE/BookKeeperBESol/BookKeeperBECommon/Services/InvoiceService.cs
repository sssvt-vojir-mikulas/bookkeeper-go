using System;
using System.Collections.Generic;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Repos;



namespace BookKeeperBECommon.Services
{



    public class InvoiceService
    {



        private InvoiceRepoMysql invoiceRepo;



        public InvoiceService()
        {
            // Temporary solution.
            this.invoiceRepo = new InvoiceRepoMysql();
        }



        public IList<Invoice> GetListOfInvoices()
        {
            return this.invoiceRepo.GetList();
        }



        public IList<Invoice> FindListOfInvoices(string invoiceNumberPattern)
        {
            Invoice searchCriteriaAsInvoice = new Invoice { InvoiceNumber = $"*{invoiceNumberPattern}*" };
            //Invoice searchCriteriaAsInvoice = new Invoice { InvoiceNumber = invoiceNumberPattern };
            return this.invoiceRepo.FindList(searchCriteriaAsInvoice);
        }



        public IList<Invoice> SearchInvoices(Invoice invoice)
        {
            if ((invoice.ID == 0) && (invoice.InvoiceNumber == null))
            {
                // Empty invoice-search criteria.
                return GetListOfInvoices();
            }
            if ((invoice.ID == 0) && (invoice.InvoiceNumber != null))
            {
                // Only the InvoiceNumber property has been set.
                return FindListOfInvoices(invoice.InvoiceNumber);
            }
            return this.invoiceRepo.FindList(invoice);
        }



        public bool ExistsInvoice(int id)
        {
            Invoice invoiceToCheck = new Invoice { ID = id };
            bool exists = this.invoiceRepo.Exists(invoiceToCheck);
            return exists;
        }



        public Invoice LoadInvoice(int id)
        {
            Invoice invoiceToLoad = new Invoice { ID = id };
            Invoice invoiceLoaded = this.invoiceRepo.Load(invoiceToLoad);
            return invoiceLoaded;
        }



        //public void SaveInvoice(Invoice invoice)
        public Invoice SaveInvoice(Invoice invoice)
        {
            Invoice invoiceToReturn = invoice;
            if (invoice.ID == 0)
            {
                this.invoiceRepo.Add(invoice);
                // Find all invoices with the given invoiceNumber.
                List<Invoice> listOfInvoicesToProcess = (List<Invoice>) this.invoiceRepo.FindList(invoice);
                // Sort the list of invoices by their ID's in an ascending order.
                listOfInvoicesToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last invoice (with the greatest ID).
                //invoiceToReturn = listOfInvoicesToProcess[0];
                invoiceToReturn = listOfInvoicesToProcess[listOfInvoicesToProcess.Count - 1];
            }
            else
            {
                this.invoiceRepo.Store(invoice);
            }
            return invoiceToReturn;
        }



        //public void DeleteInvoice(int id)
        public Invoice DeleteInvoice(int id)
        {
            Invoice invoiceToDelete = new Invoice { ID = id };
            Invoice invoiceToDeleteFound = this.invoiceRepo.Load(invoiceToDelete);
            this.invoiceRepo.Remove(invoiceToDelete);
            return invoiceToDeleteFound;
        }



    }



}
