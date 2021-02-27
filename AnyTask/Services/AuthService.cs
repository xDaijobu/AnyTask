using System;
using System.Threading.Tasks;
using Plugin.FirebaseAuth;

namespace AnyTask.Services
{
    public class AuthService
    {
        public async Task<string> SignUp(string email, string password)
        {
            try
            {
                var result = await CrossFirebaseAuth.Current.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await result.User.GetIdTokenAsync(true);
                return token;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in method SignUp : " + ex.Message);
                return "";
            }
        }

        public async Task<string> SignInAnonymously()
        {
            try
            {
                var result = await CrossFirebaseAuth.Current.Instance.SignInAnonymouslyAsync();
                var token = await result.User.GetIdTokenAsync(true);
                return token;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in method SignInAnonymously : " + ex.Message);
                return "";
            }
        }

        public async Task<IAuthResult> SignInWithEmailPassword(string email, string password)
        {
            try
            {
                var result = await CrossFirebaseAuth.Current.Instance.SignInWithEmailAndPasswordAsync(email, password);
                System.Diagnostics.Debug.WriteLine("Uid : " + result.User.Uid);

                return result;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in SignInWithEmailPassword : " + ex.Message);
                return null;
            }
        }

        public async Task SignInWithFacebook()
        {

        }

        public async Task SignInWithGithub()
        {

        }

        public void CurrentUser()
        {
            var user = CrossFirebaseAuth.Current.Instance.CurrentUser;
            System.Diagnostics.Debug.WriteLine(user);
        }

    }
}
