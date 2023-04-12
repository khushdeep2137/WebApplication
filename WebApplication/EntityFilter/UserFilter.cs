using static WebApplication.Enums.Enum;

namespace WebApplication.EntityFilter
{
    public class UserFilter 
    {

        public string Keyword { get; set; }
        public UserSortingByColumn SortingByColumn { get; set; }

        public PaginationInfo PaginationFilter { get; set; }
        public bool IsPaginationConfigure
        {
            get
            {
                return PaginationFilter != null;
            }
        }

        public SortingByDirection SortingByDirection { get; set; }

    }

    public class PaginationInfo
    {
        public PaginationInfo()
        {
            //TODO: default values
            PageNumber = 1;
            PageSize = 100;
        }

        public PaginationInfo(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            //PageSize = pageSize > 100 ? 100 : pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
