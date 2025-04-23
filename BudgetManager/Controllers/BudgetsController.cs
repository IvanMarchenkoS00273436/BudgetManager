using BudgetManager.Models;
using BudgetManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.Controllers
{
    public class BudgetsController
    {
        private BudgetRepository _budgetRepository = new BudgetRepository();

        // Get budgets by user ID
        public List<Budget> GetBudgetsByUserId(int userId)
        {
            try
            {
                return _budgetRepository.GetAllBudgetsByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving budgets: {ex.Message}");
            }
        }

        // Get a budget by ID
        public Budget GetBudgetById(int budgetId)
        {
            try
            {
                return _budgetRepository.GetBudgetById(budgetId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving budget: {ex.Message}");
            }
        }

        // Create a new budget
        public bool AddBudget(Budget budget)
        {
            try
            {
                // Validate budget
                if (budget.BudgetAmount <= 0)
                    throw new ArgumentException("Budget amount must be greater than 0");

                if (budget.StartDate > budget.EndDate)
                    throw new ArgumentException("Start date must be before end date");

                return _budgetRepository.AddBudget(budget);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding budget: {ex.Message}");
            }
        }

        // Update an existing budget
        public bool UpdateBudget(Budget budget)
        {
            try
            {
                // Validate budget
                if (budget.BudgetAmount <= 0)
                    throw new ArgumentException("Budget amount must be greater than 0");

                if (budget.StartDate > budget.EndDate)
                    throw new ArgumentException("Start date must be before end date");

                return _budgetRepository.UpdateBudget(budget);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating budget: {ex.Message}");
            }
        }

        // Delete a budget
        public bool DeleteBudget(int budgetId)
        {
            try
            {
                return _budgetRepository.DeleteBudget(budgetId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting budget: {ex.Message}");
            }
        }

        // Update the spent amount of a budget
        public bool UpdateBudgetSpentAmount(int userId, int transactionTypeId, decimal amount)
        {
            try
            {
                // Get budget for this transaction type
                var budget = _budgetRepository.GetAllBudgetsByUserId(userId)
                    .FirstOrDefault(b => b.TransactionTypeId == transactionTypeId);

                // No matching budget found
                if (budget == null)
                {
                    return true;
                }

                // Update the spent amount
                budget.SpentAmount += amount;

                // Save the changes
                return _budgetRepository.UpdateBudget(budget);
            }
            catch
            {
                return false;
            }
        }

    }
}
