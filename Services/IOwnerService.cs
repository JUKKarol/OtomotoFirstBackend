using OtomotoSimpleBackend.DTOs;

namespace OtomotoSimpleBackend.Services
{
    public interface IOwnerService
    {
        void CreatePasswordHash(string Password,
                out byte[] passwordHash
                , out byte[] passwordSalt);

        string CreateRandomToken();
    }
}
