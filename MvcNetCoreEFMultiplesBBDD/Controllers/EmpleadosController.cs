using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private IRepositoryEmpleados repo;
        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleadoDepartamento> empleados = await this.repo.GetEmpleadosDepartamentoAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int idEmp)
        {
            VistaEmpleadoDepartamento empleado = await this.repo.GetDetallesEmpleadosDepartamentoAsync(idEmp);
            return View(empleado);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Empleado emp)
        {
            await this.repo.InsertarEmpleadoAsync(emp.Apellido, emp.Oficio, emp.Dir, emp.Salario, emp.Comision, emp.NombreDepartamento);
            return View();
        }
    }
}
