// HARIA-WEB GENERATED sha256:1c1e48a50cc7b15951a346437c4c5a14e95c04986cc4bfaeb97d92d5a3fc00ec
using System.ComponentModel.DataAnnotations;

namespace DSM_Odyss.Web.ViewModels;

public sealed class LoginViewModel
{
    public string UserClass { get; set; } = "Cliente";
    public string? Identifier { get; set; }
    [Display(Name = "id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debes indicar {0}.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
