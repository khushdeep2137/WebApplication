using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Contracts.Responses;
using WebApplication.Data;
using WebApplication.EntityFilter;
using static WebApplication.Enums.Enum;

namespace WebApplication.Services
{
    public interface IUserService
    {
        Task<ResultData> SearchAsync<R>(UserFilter filter = null) where R : class;
        Task<R> GetAsync<R>(UserFilter filter = null) where R : class;
    }
    public class UserService : IUserService
    {

        private readonly WebapplicationContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(WebapplicationContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }


        public async Task<R> GetAsync<R>(UserFilter filter = null) where R : class
        {
            ResultData result = await this.SearchAsync<R>(filter);
            if (result.Total > 0)
                return (result.Data as List<R>).FirstOrDefault();

            return null;
        }

        public async Task<ResultData> SearchAsync<R>(UserFilter filter = null) where R : class
        {
            int totalRecords = default(int);

            IQueryable<User> queryable = _dbContext.Set<User>().OrderByDescending(x => x.Email)
                                                                           .AsNoTracking()
                                                                           .AsQueryable();


            if (filter != null)
            {

                if (!string.IsNullOrEmpty(filter.Keyword))
                {

                    filter.Keyword = filter.Keyword.ToLower().Trim();

                    queryable = queryable.Where(u => (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(filter.Keyword)) ||
                                                 (!string.IsNullOrEmpty(u.LastName) && u.LastName.ToLower().Contains(filter.Keyword)) ||
                                                 (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(filter.Keyword)));
                }


                switch (filter.SortingByDirection)
                {
                    case SortingByDirection.AscToDesc:
                        {
                            switch (filter.SortingByColumn)
                            {
                                case UserSortingByColumn.Name:
                                    queryable = queryable.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                                    break;
                                case UserSortingByColumn.Email:
                                    queryable = queryable.OrderBy(x => x.Email);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case SortingByDirection.DescToAsc:
                        {
                            switch (filter.SortingByColumn)
                            {
                                case UserSortingByColumn.Name:
                                    queryable = queryable.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName).ThenByDescending(x => x.FirstName);
                                    break;
                                case UserSortingByColumn.Email:
                                    queryable = queryable.OrderByDescending(x => x.Email);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                totalRecords = queryable.Count();
                if (filter.IsPaginationConfigure)
                {
                    var skip = (filter.PaginationFilter.PageNumber - 1) * filter.PaginationFilter.PageSize;
                    queryable = queryable.Skip(skip).Take(filter.PaginationFilter.PageSize);
                }
            }

            List<User> records = await queryable.ToListAsync();
            List<R> data = _mapper.Map<List<R>>(records);

            return new ResultData
            {
                Data = data,
                Total = totalRecords
            };
        }

    }
}
