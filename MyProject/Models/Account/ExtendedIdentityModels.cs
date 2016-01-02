using System.Web;
using MyProject.Models.ViewModels;

namespace MyProject.Models.Account
{
    public class ExtendedIdentityModels : RegisterViewModel
    {
        public HttpPostedFileBase UserProfilePicture { get; set; }

        //public string Image64()
        //{
        //    var image = System.Drawing.Image.FromStream(UserProfilePicture.InputStream, true, true);

        //    string image64;

        //    ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
        //        .Where(codecInfo => codecInfo.MimeType == "image/jpeg").First();
        //    using (EncoderParameters encParams = new EncoderParameters(1))
        //    {
        //        encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)50);

        //        //quality should be in the range [0..100]
        //        using (var ms = new MemoryStream())
        //        {
        //            // Convert Image to byte[]
        //            image.Save(ms, ImageFormat.Jpeg);
        //            byte[] imageBytes = ms.ToArray();

        //            // Convert byte[] to Base64 String
        //            string base64String = Convert.ToBase64String(imageBytes);
        //            image64 = base64String;
        //        }
        //    }
        //    return image64;
        //}

        public byte[] Image { get; set; }
    }

    public class ExtendedProfileModels : ProfileViewModel
    {
        public HttpPostedFileBase UserProfilePicture { get; set; }
        public byte[] Image { get; set; }     
    }
}