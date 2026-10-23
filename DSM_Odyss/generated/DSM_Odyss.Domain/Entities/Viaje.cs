using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DSM_Odyss.Domain.Enums;

namespace DSM_Odyss.Domain.Entities;

public partial class Viaje
{
    public virtual int Id { get; set; }
    public virtual string Titulo { get; set; } = string.Empty;
    public virtual DateTime FechaInicio { get; set; }
    public virtual DateTime FechaFin { get; set; }
    public virtual bool EsPublico { get; set; }
    public virtual float Presupuesto { get; set; }
    public virtual TipoViaje TipoViaje { get; set; }
    public virtual EstadoViaje Estado { get; set; }
    public virtual IList<ParadaActividad> Paradas { get; set; } = new List<ParadaActividad>();
    public virtual Destino Destino { get; set; } = null!;
    public virtual IList<Cliente> CompartidoCon { get; set; } = new List<Cliente>();
    public virtual IList<Resena> Resenas { get; set; } = new List<Resena>();
    public virtual Carpeta? Carpetas { get; set; }
    public virtual Collage? Collages { get; set; }

    private bool HasDefaultIdentity() => EqualityComparer<int>.Default.Equals(Id, default);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is Viaje other && !HasDefaultIdentity() && !other.HasDefaultIdentity() && EqualityComparer<int>.Default.Equals(Id, other.Id);
    public override int GetHashCode() => HasDefaultIdentity() ? RuntimeHelpers.GetHashCode(this) : EqualityComparer<int>.Default.GetHashCode(Id);
}
