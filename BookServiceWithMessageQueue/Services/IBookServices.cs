using Microservice_Q.ViewModels;
namespace Microservice_Q.Services
{
    public interface IBookServices
    {
        Task<IEnumerable<BookViewModel>> GetAllAsync();
        Task<BookViewModel> GetByIdAsync(int id);
    }
}
