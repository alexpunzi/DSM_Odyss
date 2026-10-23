using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Domain.Entities;

public partial class Multimedia
{
    public virtual int Id { get; set; }
    public virtual TipoMultimedia Tipo { get; set; }
    public virtual string Nombre { get; set; } = string.Empty;
    public virtual DateTime FechaSubida { get; set; }
    public virtual string Descripcion { get; set; } = string.Empty;

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Multimedia other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
