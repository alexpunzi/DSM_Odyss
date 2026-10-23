using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Application.Exceptions;
using DSM_Odyss.Domain.Entities;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Application.Services;

public sealed partial class ClienteService : ServiceEntity<Cliente>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClienteRepository _repository;
    private readonly IHariaPasswordHasher _passwordHasher;
    private readonly IHariaTokenGenerator _tokenGenerator;

    public ClienteService(IUnitOfWork unitOfWork)
        : this(unitOfWork, ownsUnitOfWork: false)
    {
    }

    public ClienteService(IUnitOfWorkFactory unitOfWorkFactory)
        : this(unitOfWorkFactory.Create(), ownsUnitOfWork: true)
    {
    }

    private ClienteService(IUnitOfWork unitOfWork, bool ownsUnitOfWork) : base(unitOfWork, unitOfWork.ClienteRepository, ownsUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.ClienteRepository;
        _passwordHasher = unitOfWork.PasswordHasher;
        _tokenGenerator = unitOfWork.TokenGenerator;
    }

    public Task<Cliente> AddClienteDefaultAsync(Cliente entity)
    {
        HashPasswordProperties(entity);
        return AddDefaultAsync(entity);
    }

    public Task DeleteClienteDefaultAsync(int id) =>
        DeleteDefaultAsync(id);

    public Task<Cliente?> ReadOIDClienteDefaultAsync(int id) =>
        ReadOIDDefaultAsync(id);

    public Task<IReadOnlyList<Cliente>> ReadAllClienteDefaultAsync() =>
        ReadAllDefaultAsync();

    public Task<Cliente> ModifierClienteDefaultAsync(Cliente entity)
    {
        HashPasswordProperties(entity);
        return ModifierDefaultAsync(entity);
    }

    private void HashPasswordProperties(Cliente entity)
    {
        if (entity.Password is not null)
        {
            entity.Password = _passwordHasher.HashPasswordIfNeeded(entity.Password);
        }
    }

    public async Task<Cliente> CrearClienteAsync(string p_user, string p_password, string p_nombre, string p_apellidos, string p_email, DateTime p_fechaNacimiento, CanalNotificacion p_canalNotificacion, bool p_recibirRecordatorios, int p_checklist_oid, int p_recordatorio_oid)
    {
        Cliente entity = new Cliente
        {
            User = p_user,
            Password = _passwordHasher.HashPasswordIfNeeded(p_password),
            Nombre = p_nombre,
            Apellidos = p_apellidos,
            Email = p_email,
            FechaNacimiento = p_fechaNacimiento,
            CanalNotificacion = p_canalNotificacion,
            RecibirRecordatorios = p_recibirRecordatorios,
        };

        await _repository.SaveAsync(entity);
        return entity;
    }

    public async Task BorrarClienteAsync(int p_oid)
    {
        Cliente entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Cliente not found.");
        await _repository.DeleteAsync(entity);
    }

    public async Task ModificarClienteAsync(int p_oid, string p_user, string p_password, string p_nombre, string p_apellidos, string p_email, DateTime p_fechaNacimiento, CanalNotificacion p_canalNotificacion, bool p_recibirRecordatorios, int p_checklist_oid, int p_recordatorio_oid)
    {
        Cliente entity = await _repository.ReadAsync(p_oid)
            ?? throw new ModelException("Cliente not found.");
        entity.User = p_user;
        entity.Password = _passwordHasher.HashPasswordIfNeeded(p_password);
        entity.Nombre = p_nombre;
        entity.Apellidos = p_apellidos;
        entity.Email = p_email;
        entity.FechaNacimiento = p_fechaNacimiento;
        entity.CanalNotificacion = p_canalNotificacion;
        entity.RecibirRecordatorios = p_recibirRecordatorios;
        await _repository.SaveAsync(entity);
    }

    public async Task<Cliente?> LeerClienteOIDAsync(int p_oid)
    {
        return await _repository.ReadAsync(p_oid);
    }

    public async Task<IReadOnlyList<Cliente>> LeerTodosClientesAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<string?> LoginAsync(int p_oid, string p_password)
    {
        try
        {
            Cliente? entity = await _repository.ReadAsync(p_oid);
            if (entity is null)
            {
                return null;
            }

            if (!_passwordHasher.VerifyPassword(entity.Password, p_password))
            {
                return null;
            }

            var tokenSubject = Convert.ToString(entity.Id, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
            var tokenClaims = new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["id"] = Convert.ToString(entity.Id, System.Globalization.CultureInfo.InvariantCulture),
            };
            return _tokenGenerator.CreateToken(tokenSubject, tokenClaims);
        }
        catch (Exception)
        {
            return null;
        }
    }

}
