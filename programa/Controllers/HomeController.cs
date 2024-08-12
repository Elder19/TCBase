using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tarea001.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace tarea001.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /*Funcion INdex 
        Objetivo: recoge el objeto con la informacion de cada paciente y lo retorna a el html privacy.cshtml */
        public IActionResult Index()
        {
           // Ruta para el directorio donde se guardará el archivo JSON
                   string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "JSON");
                   string filePath = Path.Combine(directoryPath, "datos.json");

            List<RegistroPaciente> registros = new List<RegistroPaciente>();

            // ai existe el archivo lo lee
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                try
                {
                    //lee el archivo json
                    registros = JsonConvert.DeserializeObject<List<RegistroPaciente>>(jsonData) ?? new List<RegistroPaciente>();
                }
                catch (JsonSerializationException ex)
                {
                    // Manejar la excepción de deserialización
                    _logger.LogError(ex, "Error al deserializar el archivo JSON.");
                }
            }

            //retorna los datos a la visa
            return View(registros);
        }
        /*
        privacy
        se encarga de hacer llamada a index*/
        public IActionResult Privacy()
        {
            return Index();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        /*
        DatosRegistro
        recibe un los datos de la vista y los procesa para escribirlos en el documentos datos.json
        */
        public JsonResult DatosRegistro([FromBody] RegistroPaciente datos)
        {   //si los datos no son nulos 
            if (datos == null)
            {
                return Json(new { Success = false, Message = "Datos no recibidos. El objeto es nulo." });
            }

            try
            {
               // Ruta para el directorio donde se guardará el archivo JSON
                       string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "JSON");
                       string filePath = Path.Combine(directoryPath, "datos.json");

                // Verifica si el directorio existe; 
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);// si no existe se crea
                }

                // Leer el contenido existente del archivo JSON
                List<RegistroPaciente> registros;
                // el fragmento lee de vuelta el archivo json lo almacena en una lista para volver a rescribirlo actualizado
                if (System.IO.File.Exists(filePath))
                {
                    var jsonData = System.IO.File.ReadAllText(filePath);
                    registros = JsonConvert.DeserializeObject<List<RegistroPaciente>>(jsonData) ?? new List<RegistroPaciente>();
                }
                else
                {
                    registros = new List<RegistroPaciente>();
                }

                // Agregar el nuevo registro a la lista
                registros.Add(datos);

                // Convertir la lista a una cadena JSON
                string jsonString = JsonConvert.SerializeObject(registros, Formatting.Indented);

                /// Escribir la cadena JSON en el archivo (sin usar FileShare.None)
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        using (var streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(jsonString);
        }
            
            return Json(new { message = "Paciente guardado con exito" });
            }
            catch (Exception ex)
            {
                // Loguear el error y devolver un mensaje de error
                _logger.LogError(ex, "Error al escribir en el archivo JSON.");
                return Json(new { Success = false, Message = "Error al escribir en el archivo JSON." });
            }
        }
    }
}
