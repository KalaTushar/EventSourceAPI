using RetailService.ViewModels;
using RetailService.Models;
namespace RetailService.Services
{
    public interface IRetailServices
    {
        Task<IEnumerable<Retail>> GetAll();
        Task<Retail> GetById(int id);
        Task<RetailViewModel> CreateAsync(CreateViewModel book);
    }
}
