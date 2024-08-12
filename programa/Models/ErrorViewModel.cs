namespace tarea001.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
/*
RegistroPaciente
crea el objeto paciente ademas de almacenar metodos get/set*/
public class RegistroPaciente

{
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Edad { get; set; } = string.Empty;
   public DateTime? FechaNacimiento { get; set; } 

    public string Direccion { get; set; } = string.Empty;
}
