using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DSM_Odyss.Domain.Entities;

public partial class Checklist
{
    public virtual int Id { get; set; }
    public virtual string Nombre { get; set; } = string.Empty;
    public virtual IList<Cliente> Checklist { get; set; } = new List<Cliente>();
    public virtual IList<Tarea> Tareas { get; set; } = new List<Tarea>();

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Checklist other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
