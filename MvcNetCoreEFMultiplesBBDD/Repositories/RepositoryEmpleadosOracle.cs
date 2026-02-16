using Microsoft.Data.SqlClient;
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
    CREATE OR REPLACE PROCEDURE SP_INSERTAR_EMPLEADO (
    p_apellido IN VARCHAR2,
    p_oficio   IN VARCHAR2,
    p_dir      IN NUMBER,
    p_salario  IN NUMBER,
    p_comision IN NUMBER,
    p_nomdept  IN VARCHAR2
)
IS
    -- Las variables se declaran aquí, antes del BEGIN
    v_iddepartamento NUMBER;
    v_idempleado     NUMBER;
    v_fecha          DATE;
BEGIN
    -- 1. Obtener ID Departamento
    SELECT DEPT_NO INTO v_iddepartamento 
    FROM DEPT 
    WHERE DNOMBRE = p_nomdept 
    AND ROWNUM = 1;

    -- 2. Calcular siguiente ID (NVL es el ISNULL de Oracle)
    SELECT NVL(MAX(EMP_NO), 0) + 1 INTO v_idempleado FROM EMP;

    -- 3. Fecha actual
    v_fecha := SYSDATE;

    -- 4. Insertar
    INSERT INTO EMP (EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO)
    VALUES (v_idempleado, p_apellido, p_oficio, p_dir, v_fecha, p_salario, p_comision, v_iddepartamento);

    COMMIT; -- En Oracle suele ser necesario confirmar la transacción
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

        public async Task InsertarEmpleadoAsync(string apellido, string oficio, int dir, int salario, int comision, string nomDept)
        {
            string sql = "BEGIN SP_INSERTAR_EMPLEADO (:apellido, :oficio, :dir, :salario, :comision,:nomdept); END;";
            OracleParameter pamApe = new OracleParameter(":apellido", apellido);
            OracleParameter pamOfi = new OracleParameter(":oficio", oficio);
            OracleParameter pamDir = new OracleParameter(":dir", dir);
            OracleParameter pamSal = new OracleParameter(":salario", salario);
            OracleParameter pamCom = new OracleParameter(":comision", comision);
            OracleParameter pamDept = new OracleParameter(":nomdept", nomDept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
        }
    }
}
