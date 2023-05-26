using System.Threading.Tasks;

namespace MythicEmpire
{
    public interface ILoginService
    {
        void Login(string email, string password);
    }
}