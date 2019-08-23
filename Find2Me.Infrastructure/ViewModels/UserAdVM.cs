using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
    public class UserAdVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class UserAdImageVM
    {
        public UserAdImageVM()
        {
            UserAd = new UserAdVM();
        }       

        public long Id { get; set; }

        public int AdID { get; set; }

        public int ImageNumber { get; set; }

        public UserAdVM UserAd { get; set; }

        public string AdImageOriginal { get; set; }

        public string AdImageSelected { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
