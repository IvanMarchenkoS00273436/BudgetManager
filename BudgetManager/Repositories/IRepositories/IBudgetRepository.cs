using BudgetManager.DatabaseSets;
using BudgetManager.Models;

namespace BudgetManager.Repositories.IRepositories
{
    internal interface IBudgetRepository
    {
        DbContextData _dbContexData { get; set; }

        List<Budget> GetAllBudgetsByUserId(int userId);
        bool AddBudget(Budget budget);
        bool UpdateBudget(Budget budget);
        bool DeleteBudget(int budgetId);
        Budget GetBudgetById(int budgetId);
    }
}
