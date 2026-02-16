using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region STORED PROCEDURE
    /*
 --ORACLE
CREATE or replace VIEW V_EMPLEADOSV2
AS
	SELECT EMP_NO AS IDEMPLEADO,EMP.APELLIDO,EMP.OFICIO,EMP.SALARIO, DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS NOMBRE, DEPT.LOC AS LOCALIDAD FROM EMP
	INNER JOIN DEPT
	ON EMP.DEPT_NO = DEPT.DEPT_NO
create or replace procedure SP_ALL_EMPLEADOS
(p_cursor_empleados out SYS_REFCURSOR)
AS
	BEGIN
		OPEN p_cursor_empleados for
		select * from V_EMPLEADOSV2;
	END;
 */
    #endregion
    public class RepositoryEmpleadosOracle : IRepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleadosOracle(EmpleadosContext context)
        {
            this.context = context;
        }
        public async Task<VistaEmpleadoDepartamento> GetDetallesEmpleadosDepartamentoAsync(int idEmp)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdEmpleado == idEmp
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<VistaEmpleadoDepartamento>> GetEmpleadosDepartamentoAsync()
        {
            //begin 
            //    sp_procedure(:param1,:param2),
            //end;
            string sql = "begin ";
            sql += "SP_ALL_EMPLEADOS (:p_cursor_empleados); ";
            sql += " end;";
            OracleParameter pamCursor = new OracleParameter();
            pamCursor.ParameterName = "p_cursor_empleados";
            pamCursor.Value = null;
            pamCursor.Direction = ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamCursor.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.Empleados
                .FromSqlRaw(sql, pamCursor);
            return await consulta.ToListAsync();
        }
    }
}
