using System.ComponentModel.DataAnnotations;

namespace EmpleadosWeb.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        public string Apellido { get; set; }
        [Range(18, 100, ErrorMessage = "La edad debe estar entre 18 y 100")]
        public int Edad { get; set; }
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Correo { get; set; }
        [StringLength(50, ErrorMessage = "El departamento no puede exceder 50 caracteres")]
        public string Departamento { get; set; }
        [StringLength(50, ErrorMessage = "El puesto no puede exceder 50 caracteres")]
        public string Puesto { get; set; }
        [Range(0, 1000000, ErrorMessage = "El salario debe estar entre 0 y 1,000,000")]
        public decimal Salario { get; set; }
    }
}
