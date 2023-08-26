using System.Linq.Expressions;
using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.DTOs.Views;
using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Address;
using BackendTemplateCore.Models.User;

namespace BackendTemplateCore.Services;

public interface IDataService {

   Task<T?> GetByIdAsync<T>(params object[] keys) where T : class;
   Task<T?> GetAsync<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes)
      where T : class;
   // Task<List<T>?> GetAll<T>(Expression<Func<T, bool>>? expression = null) where T : class;
   Task<List<T>> GetAll<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes)
      where T : class;
   Task<int> Count<T>(Expression<Func<T, bool>>? expression = null) where T : class;
   Task<T?> GetLast<T>(Expression<Func<T, bool>>? expression = null) where T : AuditableEntity;
   Task<T> Add<T>(T entity) where T : class;
   Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Update<T>(T entity, Guid? userId = null) where T : class;
   Task Delete<T>(T entity) where T : class;
   Task DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Atomic(Func<Task> operation);
   
   Task RegisterLogin(Guid userId);
   Task<Guid> LockoutUser(User user, int days = 0, int hours = 0);
   Task<Guid> UnlockoutUser(User user);
   
   Task<Country?> GetCurrentCountry();
   
   Task<bool> ExistsUserWithEmail(string email);
   Task<bool> ExistsUserWithPhone(string phone);
   Task<bool> ExistsRoleWithName(string dataName);
   Task<User> GetUserWithRoles(Guid userId);
   Task<User?> GetUserLogin(string login);
   Task<List<RoleView>> GetRoleViews(int start, int count, string? filter);
   Task<List<User>> GetUserViews(string? filter, int start, int count);
   Task<List<CityView>> GetCities(int start, int count, string? filter);
   Task<bool> ExistsCityWithName(string dataName, int dataStateId);
   Task<bool> ExistsStateWithName(string dataName, int dataCountryId);
   Task<List<StateView>> GetStates(string? filter);
   Task<List<StatusItemView>> GetCountries(string? filter);
   Task<bool> ExistsBranchWithCode(string dataCode);
   Task<bool> ExistsCompanyWithCode(string code);
}
