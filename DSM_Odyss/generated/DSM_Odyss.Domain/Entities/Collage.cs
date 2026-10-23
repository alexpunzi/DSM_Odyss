using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DSM_Odyss.Domain.Entities;

public partial class Collage
{
    public virtual int Id { get; set; }
    public virtual string UrlImagen { get; set; } = string.Empty;
    public virtual DateTime FechaCreacion { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Collage other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
