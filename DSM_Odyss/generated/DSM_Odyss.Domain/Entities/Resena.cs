using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DSM_Odyss.Domain.Entities;

public partial class Resena
{
    public virtual int Id { get; set; }
    public virtual string Comentario { get; set; } = string.Empty;
    public virtual DateTime Fecha { get; set; }
    public virtual int Puntuacion { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Resena other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
