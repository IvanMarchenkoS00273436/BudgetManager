using BudgetManager.DatabaseSets;
using BudgetManager.Models;
using BudgetManager.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        // Data context for database operations
        public DbContextData _dbContexData { get; set; } = new DbContextData();

        // CRUD operations for Budgets
        /* =========================== */
        // Create a new budget
        public bool AddBudget(Budget budget)
        {
            try
            {
                if(budget != null)
                {
                    _dbContexData.Budgets.Add(budget);
                    _dbContexData.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Budget is null");
                }
            }
            catch { return false; } 
        }

        // Delete a budget
        public bool DeleteBudget(int budgetId)
        {
            try
            {
                Budget budget = _dbContexData.Budgets.FirstOrDefault(b => b.BudgetId == budgetId);
                if (budget != null)
                {
                    _dbContexData.Budgets.Remove(budget);
                    _dbContexData.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Budget not found");
                }
            }
            catch { return false; }
        }

        // Get all budgets by user ID
        public List<Budget> GetAllBudgetsByUserId(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new Exception("User ID is invalid");
                }
                else
                {
                    // fix for not working updating budgets
                    _dbContexData = new DbContextData();
                    // =================================== 

                    var budgets = _dbContexData.Budgets.Where(b => b.UserId == userId).ToList();
                    var transactionTypes = _dbContexData.TransactionTypes.ToList();

                    foreach (var budget in budgets)
                    {
                        foreach (var transactionType in transactionTypes)
                        {
                            if (budget.TransactionTypeId == transactionType.TransactionTypeId)
                            {
                                budget.TransactionType = transactionType;
                            }
                        }
                    }

                    //return _dbContexData.Budgets
                    //    .Include(b => b.TransactionType)
                    //    .Where(b => b.UserId == userId)
                    //    .ToList();

                    return budgets;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem with getting budgets : {ex.Message}");
            }
        }


        // Get a budget by ID
        public Budget GetBudgetById(int budgetId)
        {
            try
            {
                if(budgetId <= 0)
                {
                    throw new Exception("Budget ID is invalid");
                }
                else
                {
                    //return _dbContexData.Budgets.FirstOrDefault(b => b.BudgetId == budgetId);
                    return _dbContexData.Budgets
                       .Include(b => b.TransactionType)
                       .FirstOrDefault(b => b.BudgetId == budgetId);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Problem with getting budget : {ex.Message}");
            }
        }

        // Update a budget
        public bool UpdateBudget(Budget budget)
        {
            try
            {
                if(budget == null)
                {
                    throw new Exception("Budget is null");
                }
                else
                {
                    Budget budgetToUpdate = _dbContexData.Budgets.FirstOrDefault(b => b.BudgetId == budget.BudgetId);
                    if (budgetToUpdate != null)
                    {
                        // Keep the existing SpentAmount when updating other properties
                        decimal currentSpentAmount = budgetToUpdate.SpentAmount;
                        
                        budgetToUpdate.TransactionTypeId = budget.TransactionTypeId;
                        budgetToUpdate.StartDate = budget.StartDate;
                        budgetToUpdate.EndDate = budget.EndDate;
                        budgetToUpdate.BudgetAmount = budget.BudgetAmount;
                        
                        if (budget.SpentAmount != 0)
                        {
                            budgetToUpdate.SpentAmount = budget.SpentAmount;
                        }
                        else
                        {
                            budgetToUpdate.SpentAmount = currentSpentAmount;
                        }
                        
                        _dbContexData.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Budget not found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem with updating budget : {ex.Message}");
            }
        }
    }
}