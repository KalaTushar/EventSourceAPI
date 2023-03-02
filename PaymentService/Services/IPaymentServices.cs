using PaymentService.ViewModels;
using PaymentService.Models;
namespace PaymentService.Services
{
    public interface IPaymentServices
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> GetByIdAsync(int id);
        Task<Payment> CreateAsync(CreateViewModel book);
    }
}
