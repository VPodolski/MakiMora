using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;

namespace MakiMora.API.Services
{
    public class CourierEarningService : ICourierEarningService
    {
        private readonly ICourierEarningRepository _earningRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public CourierEarningService(
            ICourierEarningRepository earningRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _earningRepository = earningRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<CourierEarningDto?> GetEarningByIdAsync(Guid id)
        {
            var earning = await _earningRepository.GetByIdAsync(id);
            if (earning == null) return null;

            return _mapper.Map<CourierEarningDto>(earning);
        }

        public async Task<IEnumerable<CourierEarningDto>> GetEarningsAsync()
        {
            var earnings = await _earningRepository.GetAllAsync();
            return earnings.Select(e => _mapper.Map<CourierEarningDto>(e));
        }

        public async Task<IEnumerable<CourierEarningDto>> GetEarningsByCourierAsync(Guid courierId)
        {
            var earnings = await _earningRepository.GetByCourierAsync(courierId);
            return earnings.Select(e => _mapper.Map<CourierEarningDto>(e));
        }

        public async Task<IEnumerable<CourierEarningDto>> GetEarningsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var earnings = await _earningRepository.GetByDateRangeAsync(startDate, endDate);
            return earnings.Select(e => _mapper.Map<CourierEarningDto>(e));
        }

        public async Task<IEnumerable<CourierEarningDto>> GetEarningsByOrderAsync(Guid orderId)
        {
            var earnings = await _earningRepository.GetByOrderAsync(orderId);
            return earnings.Select(e => _mapper.Map<CourierEarningDto>(e));
        }

        public async Task<IEnumerable<CourierEarningDto>> GetEarningsByTypeAsync(string earningType)
        {
            var earnings = await _earningRepository.GetByTypeAsync(earningType);
            return earnings.Select(e => _mapper.Map<CourierEarningDto>(e));
        }

        public async Task<decimal> GetTotalEarningsByCourierAsync(Guid courierId, DateTime startDate, DateTime endDate)
        {
            return await _earningRepository.GetTotalEarningsByCourierAsync(courierId, startDate, endDate);
        }

        public async Task<CourierEarningDto> CreateEarningAsync(CreateCourierEarningRequestDto createEarningDto)
        {
            var courier = await _userRepository.GetByIdAsync(createEarningDto.CourierId);
            if (courier == null)
                throw new ArgumentException($"Courier with id '{createEarningDto.CourierId}' not found");

            var order = await _orderRepository.GetByIdAsync(createEarningDto.OrderId);
            if (order == null)
                throw new ArgumentException($"Order with id '{createEarningDto.OrderId}' not found");

            var earning = new CourierEarning
            {
                CourierId = createEarningDto.CourierId,
                OrderId = createEarningDto.OrderId,
                Amount = createEarningDto.Amount,
                EarningType = createEarningDto.EarningType,
                Date = createEarningDto.Date
            };

            var createdEarning = await _earningRepository.AddAsync(earning);
            return _mapper.Map<CourierEarningDto>(createdEarning);
        }

        public async Task<bool> DeleteEarningAsync(Guid id)
        {
            var earning = await _earningRepository.GetByIdAsync(id);
            if (earning == null) return false;

            await _earningRepository.DeleteAsync(earning);
            return true;
        }
    }
}