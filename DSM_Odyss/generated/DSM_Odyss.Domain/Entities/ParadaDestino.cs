using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DSM_Odyss.Domain.Entities;

public partial class ParadaDestino
{
    public virtual int Id { get; set; }
    public virtual int Orden { get; set; }
    public virtual DateTime FechaLlegada { get; set; }
    public virtual DateTime FechaSalida { get; set; }
    public virtual Viaje Viaje { get; set; } = null!;
    public virtual Destino Destino { get; set; } = null!;
    public virtual IList<ParadaActividad> Actividades { get; set; } = new List<ParadaActividad>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ParadaDestino other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
