using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DSM_Odyss.Domain.Entities;

public partial class ParadaActividad
{
    public virtual int Id { get; set; }
    public virtual string Nombre { get; set; } = string.Empty;
    public virtual int Dia { get; set; }
    public virtual DateTime Horario { get; set; }
    public virtual string? Detalles { get; set; }
    public virtual bool Completada { get; set; }
    public virtual Ubicacion? Ubicacion { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ParadaActividad other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
