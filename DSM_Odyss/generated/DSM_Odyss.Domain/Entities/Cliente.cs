using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Domain.Entities;

public partial class Cliente
{
    public virtual int Id { get; set; }
    public virtual string User { get; set; } = string.Empty;
    public virtual string Password { get; set; } = string.Empty;
    public virtual string Nombre { get; set; } = string.Empty;
    public virtual string Apellidos { get; set; } = string.Empty;
    public virtual string Email { get; set; } = string.Empty;
    public virtual DateTime FechaNacimiento { get; set; }
    public virtual CanalNotificacion CanalNotificacion { get; set; }
    public virtual bool RecibirRecordatorios { get; set; }
    public virtual IList<Cliente> Contactos { get; set; } = new List<Cliente>();
    public virtual IList<Cliente> ContactosInversos { get; set; } = new List<Cliente>();
    public virtual IList<Estadisticas> Estadisticas { get; set; } = new List<Estadisticas>();
    public virtual IList<Viaje> Viajes { get; set; } = new List<Viaje>();
    public virtual IList<Resena> Resenas { get; set; } = new List<Resena>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Cliente other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
