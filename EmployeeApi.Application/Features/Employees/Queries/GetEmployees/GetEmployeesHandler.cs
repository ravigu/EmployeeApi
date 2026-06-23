using AutoMapper;
using EmployeeApi.Application.Common.Models;
using EmployeeApi.Application.DTOs;
using EmployeeApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeApi.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesHandler :
        IRequestHandler<GetEmployeesQuery, PagedResponse<EmployeeDto>>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetEmployeesHandler> _logger;

        public GetEmployeesHandler(
            IEmployeeRepository repository,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<GetEmployeesHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<PagedResponse<EmployeeDto>> Handle(
            GetEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"employees_{request.Parameters.PageNumber}_" +
                $"{request.Parameters.PageSize}_" +
                $"{request.Parameters.Search}_" +
                $"{request.Parameters.Descending}";

            // Redis
            var cachedEmployees =
                await _cacheService.GetAsync<PagedResponse<EmployeeDto>>(cacheKey);

            if (cachedEmployees != null)
            {
                _logger.LogInformation("Cache hit for employees");

                return cachedEmployees;
            }

            _logger.LogInformation("Cache miss for employees");

            // Database
            var employees =
                await _repository.GetAllAsync(request.Parameters);

            var response = new PagedResponse<EmployeeDto>
            {
                PageNumber = employees.PageNumber,
                PageSize = employees.PageSize,
                TotalRecords = employees.TotalRecords,
                TotalPages = employees.TotalPages,
                Items = _mapper.Map<List<EmployeeDto>>(employees.Items)
            };

            // Save to Redis
            await _cacheService.SetAsync(
                cacheKey,
                response,
                TimeSpan.FromMinutes(5));

            return response;
        }
    }
}