using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Data
{
    public class EmpleadosContext: DbContext
    {
        public EmpleadosContext(DbContextOptions<EmpleadosContext>options)
            :base(options) { }
        public DbSet<VistaEmpleadoDepartamento> Empleados { get; set; }
        public DbSet<Empleado> Emp { get; set; }
    }
}
