using OtomotoSimpleBackend.DTOs;

namespace OtomotoSimpleBackend.Services
{
    public interface IOwnerService
    {
        void CreatePasswordHash(string Password,
                out byte[] passwordHash
                , out byte[] passwordSalt);
        bool VerifyPasswordHash(string Password,
                byte[] passwordHash
                , byte[] passwordSalt);

        string CreateRandomToken();

        string CreateToken(OwnerDtoLogin owner);
    }
}
