using System.ComponentModel.DataAnnotations;
using WebApplication.EntityFilter;
using static WebApplication.Enums.Enum;

namespace WebApplication.Contracts.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class UserFilterRequest
    {
        public UserFilterRequest()
        {
            Sort = new UserSortRequest();
        }

        public string Keyword { get; set; }
        public UserSortRequest Sort { get; set; }
        public PaginationInfo pagination { get; set; }

        public class UserSortRequest
        {
            public UserSortingByColumn Property { get; set; }
            public SortingByDirection Direction { get; set; }
        }
    }

}
