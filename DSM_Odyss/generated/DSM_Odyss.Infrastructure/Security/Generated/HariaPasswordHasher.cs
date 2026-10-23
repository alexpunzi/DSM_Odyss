using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Infrastructure.Utils;

namespace DSM_Odyss.Infrastructure.Security;

public sealed class HariaPasswordHasher : IHariaPasswordHasher
{
    public string HashPasswordIfNeeded(string password) =>
        HariaPasswordHashingUtils.HashPasswordIfNeeded(password);

    public string HashPassword(string password) =>
        HariaPasswordHashingUtils.HashPassword(password);

    public bool VerifyPassword(string? storedHash, string? password) =>
        HariaPasswordHashingUtils.VerifyPassword(storedHash, password);
}