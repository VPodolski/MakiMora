using MakiMora.Core.Entities;

namespace MakiMora.Core.Services
{
    public interface ICourierEarningService
    {
        Task<CourierEarningDto?> GetEarningByIdAsync(Guid id);
        Task<IEnumerable<CourierEarningDto>> GetEarningsAsync();
        Task<IEnumerable<CourierEarningDto>> GetEarningsByCourierAsync(Guid courierId);
        Task<IEnumerable<CourierEarningDto>> GetEarningsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CourierEarningDto>> GetEarningsByOrderAsync(Guid orderId);
        Task<IEnumerable<CourierEarningDto>> GetEarningsByTypeAsync(string earningType);
        Task<decimal> GetTotalEarningsByCourierAsync(Guid courierId, DateTime startDate, DateTime endDate);
        Task<CourierEarningDto> CreateEarningAsync(CreateCourierEarningRequestDto createEarningDto);
        Task<bool> DeleteEarningAsync(Guid id);
    }
}