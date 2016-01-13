using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyProject.DAL;
using MyProject.Models;
using MyProject.Models.Account;
using Newtonsoft.Json;

namespace MyProject.Controllers
{
    
    [Authorize]
    public class ProfileController : Controller
    {
        //private IdentityDb _identityDb = new IdentityDb();

        [AllowAnonymous]
        public ActionResult ViewProfile()
        {
            ViewBag.Message = "Your Profile";
            
 
            using (IdentityContext _idDb = new IdentityContext())
            {
                var _currentUserId = User.Identity.GetUserId();
                var _currentUser = _idDb.Users.FirstOrDefault(x => x.Id == _currentUserId);

                var currentUserExtendedIdentity = new ExtendedProfileModels()
                {
                    FirstName = _currentUser.FirstName,
                    LastName = _currentUser.LastName,
                    Email = _currentUser.Email,
                    Image = _currentUser.ProfilePicture,
                    Password = _currentUser.PasswordHash,
                    ConfirmPassword = _currentUser.PasswordHash
                };
                return View(currentUserExtendedIdentity);
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ViewProfile(ExtendedProfileModels model)
        {
            if (ModelState.IsValid)
            {
                using (IdentityContext idDb = new IdentityContext())
                {

                    var currentUserId = User.Identity.GetUserId();
                    var currentUser = idDb.Users.FirstOrDefault(x => x.Id == currentUserId);


                    var copy = Clone(currentUser);
                    copy.FirstName = model.FirstName;
                    copy.LastName = model.LastName;
                    copy.Email = model.Email;
                    //if (model.Password != null)
                    //{

                    //    copy.PasswordHash = new UserManager<ApplicationUser>(idDb).PasswordHasher.HashPassword(model.Password);
                    //}
                    

                    if (model.UserProfilePicture != null)
                    {
                        if (model.UserProfilePicture.ContentLength > (4 * 1024 * 1024))
                        {
                            ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                            return View();
                        }
                        if (
                            !(model.UserProfilePicture.ContentType == "image/jpeg" ||
                              model.UserProfilePicture.ContentType == "image/gif"))
                        {
                            ModelState.AddModelError("CustomError", "Image must be in jpeg or gif format.");
                        }

                        byte[] data = new byte[model.UserProfilePicture.ContentLength];
                        model.UserProfilePicture.InputStream.Read(data, 0, model.UserProfilePicture.ContentLength);

                        copy.ProfilePicture = data;
                    }

                    idDb.Entry(currentUser).CurrentValues.SetValues(copy);
                    await idDb.SaveChangesAsync();
                }

                return RedirectToAction("ViewProfile", "Profile");
            }
            // If we got this far, something failed, redisplay form
            return View();
        }


        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        //public ApplicationUser Clone(ApplicationUser user)
        //{
        //    return new ApplicationUser()
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        LastName = user.LastName,
        //        FullName = user.FullName,
        //        AccessFailedCount = user.AccessFailedCount,
        //        EmailConfirmed = user.EmailConfirmed,
        //        LockoutEnabled = user.LockoutEnabled,
                

        //    };
        //}
    }
}