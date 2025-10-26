using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    
    Task<User?> GetAllAsync();
    
    Task<User?> GetByEmailAsync(string email);
    
    Task AddAsync(User user);
    
    Task UpdateAsync(User user);
    
    Task<bool> ExistsByEmailAsync(string email);    
}
