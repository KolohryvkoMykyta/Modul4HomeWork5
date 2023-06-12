using System.Threading.Tasks;
using ALevelSample.Models;

namespace ALevelSample.Services.Abstractions;

public interface IUserService
{
    Task<string> AddUserAsync(string firstName, string lastName);
    Task<User> GetUserAsync(string id);
    Task<bool> UpdateUserAsync(string id, string newFirstName, string newLastName);
    Task<bool> DeleteUserAsync(string id);
}