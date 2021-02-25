using System;
using System.Threading.Tasks;

namespace AnyTask.Interfaces
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);
    }
}
