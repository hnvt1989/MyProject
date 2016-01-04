using System.Web;
using MyProject.Models.ViewModels;

namespace MyProject.Models.Account
{
    public class ExtendedIdentityModels : RegisterViewModel
    {
        public HttpPostedFileBase UserProfilePicture { get; set; }

        public byte[] Image { get; set; }
    }

    public class ExtendedProfileModels : ProfileViewModel
    {
        public HttpPostedFileBase UserProfilePicture { get; set; }
        public byte[] Image { get; set; }     
    }
}