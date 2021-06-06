using System;
using System.Collections.Generic;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Repos;



namespace BookKeeperBECommon.Services
{



    public class StatementService
    {



        private StatementRepoMysql statementRepo;



        public StatementService()
        {
            // Temporary solution.
            this.statementRepo = new StatementRepoMysql();
        }



        public IList<Statement> GetListOfStatements()
        {
            return this.statementRepo.GetList();
        }



        public IList<Statement> FindListOfStatements(string statementnamePattern)
        {
            Statement searchCriteriaAsStatement = new Statement { StatementNumber = $"*{statementnamePattern}*" };
            //Statement searchCriteriaAsStatement = new Statement { Statementname = statementnamePattern };
            return this.statementRepo.FindList(searchCriteriaAsStatement);
        }



        public IList<Statement> SearchStatements(Statement statement)
        {
            if ((statement.ID == 0) && (statement.StatementNumber == null))
            {
                // Empty statement-search criteria.
                return GetListOfStatements();
            }
            if ((statement.ID == 0) && (statement.StatementNumber != null))
            {
                // Only the Statementname property has been set.
                return FindListOfStatements(statement.StatementNumber);
            }
            return this.statementRepo.FindList(statement);
        }



        public bool ExistsStatement(int id)
        {
            Statement statementToCheck = new Statement { ID = id };
            bool exists = this.statementRepo.Exists(statementToCheck);
            return exists;
        }



        public Statement LoadStatement(int id)
        {
            Statement statementToLoad = new Statement { ID = id };
            Statement statementLoaded = this.statementRepo.Load(statementToLoad);
            return statementLoaded;
        }



        //public void SaveStatement(Statement statement)
        public Statement SaveStatement(Statement statement)
        {
            Statement statementToReturn = statement;
            if (statement.ID == 0)
            {
                this.statementRepo.Add(statement);
                // Find all statements with the given statementname.
                List<Statement> listOfStatementsToProcess = (List<Statement>) this.statementRepo.FindList(statement);
                // Sort the list of statements by their ID's in an ascending order.
                listOfStatementsToProcess.Sort((u1, u2) => u1.ID - u2.ID);
                // Get the last statement (with the greatest ID).
                //statementToReturn = listOfStatementsToProcess[0];
                statementToReturn = listOfStatementsToProcess[listOfStatementsToProcess.Count - 1];
            }
            else
            {
                this.statementRepo.Store(statement);
            }
            return statementToReturn;
        }



        //public void DeleteStatement(int id)
        public Statement DeleteStatement(int id)
        {
            Statement statementToDelete = new Statement { ID = id };
            Statement statementToDeleteFound = this.statementRepo.Load(statementToDelete);
            this.statementRepo.Remove(statementToDelete);
            return statementToDeleteFound;
        }



    }



}
