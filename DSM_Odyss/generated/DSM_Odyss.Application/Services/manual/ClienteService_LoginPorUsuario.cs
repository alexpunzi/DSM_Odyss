using DSM_Odyss.Domain.Entities;

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService
{
    public async Task<string?> LoginAsync(string p_user, string p_password)
    {
        if (string.IsNullOrWhiteSpace(p_user) || string.IsNullOrEmpty(p_password))
        {
            return null;
        }

        IReadOnlyList<Cliente> clientes = await _repository.ReadAllAsync();
        Cliente? cliente = clientes.FirstOrDefault(item =>
            string.Equals(item.User, p_user.Trim(), StringComparison.OrdinalIgnoreCase));
        if (cliente is null || !_passwordHasher.VerifyPassword(cliente.Password, p_password))
        {
            return null;
        }

        string tokenSubject = Convert.ToString(cliente.Id, System.Globalization.CultureInfo.InvariantCulture);
        var claims = new Dictionary<string, string?>(StringComparer.Ordinal)
        {
            ["id"] = Convert.ToString(cliente.Id, System.Globalization.CultureInfo.InvariantCulture),
            ["user"] = cliente.User,
        };
        return _tokenGenerator.CreateToken(tokenSubject, claims);
    }
}
