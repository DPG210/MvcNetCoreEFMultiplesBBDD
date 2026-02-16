using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<VistaEmpleadoDepartamento>> GetEmpleadosDepartamentoAsync();
        Task<VistaEmpleadoDepartamento> GetDetallesEmpleadosDepartamentoAsync(int idEmp);
        Task InsertarEmpleadoAsync
            (string apellido, string oficio, int dir, int salario, int comision, string nomDept);
    }
}
