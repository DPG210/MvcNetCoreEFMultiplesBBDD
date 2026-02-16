using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;


#region STORED PROCEDURE

/*
 --SQL
CREATE VIEW V_EMPLEADOSV2
AS
	SELECT EMP_NO AS IDEMPLEADO,EMP.APELLIDO,EMP.OFICIO,EMP.SALARIO, DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS NOMBRE, DEPT.LOC AS LOCALIDAD FROM EMP
	INNER JOIN DEPT
	ON EMP.DEPT_NO = DEPT.DEPT_NO
GO
alter procedure SP_ALL_EMPLEADOS
as
	select * from V_EMPLEADOSV2
go
create or alter procedure SP_INSERTAR_EMPLEADO
(@apellido nvarchar(50),@oficio nvarchar(50), @dir int, @salario int, @comision int ,@nomdept nvarchar(50))
as
	declare @iddepartamento int
	select @iddepartamento = DEPT_NO from dept where DNOMBRE = @nomdept
	declare @idempleado int
	select @idempleado= max(emp_no) + 1 from emp ;
	declare @fecha date;
	set @fecha = GETDATE(); 

	insert into emp values(@idempleado,@apellido,@oficio,@dir,@fecha,@salario,@comision,@iddepartamento)
go
 */
#endregion
namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosSQLServer:IRepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleadosSQLServer(EmpleadosContext context)
        {
            this.context = context;
        }
        public async Task<List<VistaEmpleadoDepartamento>> GetEmpleadosDepartamentoAsync()
        {
            //var consulta = from datos in this.context.Empleados
            //               select datos;
            //return await consulta.ToListAsync();
            string sql = "SP_ALL_EMPLEADOS";
            var consulta = this.context.Empleados
                .FromSqlRaw(sql);
            List<VistaEmpleadoDepartamento> data = await
                consulta.ToListAsync();
            return data;
        }
        public async Task<VistaEmpleadoDepartamento> GetDetallesEmpleadosDepartamentoAsync(int idEmp)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdEmpleado== idEmp
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task InsertarEmpleadoAsync
            (string apellido,string oficio, int dir, int salario, int comision,string nomDept)
        {
            string sql = "SP_INSERTAR_EMPLEADO @apellido, @oficio, @dir, @salario, @comision,@nomdept";
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", dir);
            SqlParameter pamSal = new SqlParameter("@salario", salario);
            SqlParameter pamCom = new SqlParameter("@comision", comision);
            SqlParameter pamDept = new SqlParameter("@nomdept", nomDept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
        }
    }
}
