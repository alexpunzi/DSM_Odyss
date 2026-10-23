using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Domain.Entities;

public partial class Recordatorio
{
    public virtual int Id { get; set; }
    public virtual string Titulo { get; set; } = string.Empty;
    public virtual string Detalles { get; set; } = string.Empty;
    public virtual DateTime Fecha { get; set; }
    public virtual DateTime Hora { get; set; }
    public virtual TipoAviso TipoAviso { get; set; }
    public virtual bool Enviado { get; set; }
    public virtual IList<Cliente> Recordatorios { get; set; } = new List<Cliente>();
    public virtual Tarea? Tarea { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Recordatorio other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
