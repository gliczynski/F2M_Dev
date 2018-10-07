using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    public class UserValidationSuggestions
    {
        public bool UserExists { get; set; }

        public List<string> UserSuggestion { get; set; }
    }

    public class ResponseResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
