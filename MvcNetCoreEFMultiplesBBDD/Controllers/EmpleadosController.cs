using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleadoDepartamento> empleados = await this.repo.GetEmpleadosDepartamento();
            return View(empleados);
        }
        
        public async Task<IActionResult> Details(int idEmp)
        {
            VistaEmpleadoDepartamento empleado = await this.repo.GetDetallesEmpleadosDepartamento(idEmp);
            return View(empleado);
        }
    }
}
