using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Zlib;
using System.Collections.Generic;
using static Google.Protobuf.Reflection.ExtensionRangeOptions.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    #region VISTAS
    // CREATE OR REPLACE VIEW V_EMPLEADOSV2 AS
    //SELECT
    //    EMP.EMP_NO        AS IDEMPLEADO,
    //    EMP.APELLIDO AS APELLIDO,
    //    EMP.OFICIO AS OFICIO,
    //    EMP.SALARIO AS SALARIO,
    //    EMP.DEPT_NO AS IDDEPARTAMENTO,
    //    DEPT.DNOMBRE AS NOMBRE,
    //    DEPT.LOC AS LOCALIDAD
    //FROM EMP
    //INNER JOIN DEPT

    //    ON EMP.DEPT_NO = DEPT.DEPT_NO;
    #endregion
    #region PROCEDIMIENTOS ALMACENADOS
    /*DELIMITER //

//CREATE PROCEDURE SP_ALL_EMPLEADOS()
//BEGIN
//    SELECT* from v_empleadosv2;

//END //

//DELIMITER;
DELIMITER //

CREATE PROCEDURE SP_INSERTAR_EMPLEADO(
    IN p_apellido VARCHAR(50),
    IN p_oficio VARCHAR(50),
    IN p_dir INT,
    IN p_salario INT,
    IN p_comision INT,
    IN p_nomdept VARCHAR(50)
)
BEGIN
    -- Declaración de variables al inicio del bloque
    DECLARE v_iddepartamento INT;
    DECLARE v_idempleado INT;
    DECLARE v_fecha DATE;

    -- Lógica de negocio
    SELECT DEPT_NO INTO v_iddepartamento FROM DEPT WHERE DNOMBRE = p_nomdept LIMIT 1;
    
    SELECT IFNULL(MAX(EMP_NO), 0) + 1 INTO v_idempleado FROM EMP;

    SET v_fecha = CURDATE(); -- Equivalente a GETDATE() de SQL Server

    -- Inserción
    INSERT INTO EMP(EMP_NO, APELLIDO, OFICIO, DIR, FECHA_ALT, SALARIO, COMISION, DEPT_NO)
    VALUES(v_idempleado, p_apellido, p_oficio, p_dir, v_fecha, p_salario, p_comision, v_iddepartamento);
    END //

    DELIMITER;
    */
    #endregion
    public class RepositoryEmpleadosMySQL : IRepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleadosMySQL(EmpleadosContext context)
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
            string sql = "CALL SP_ALL_EMPLEADOS()";
            var consulta = from datos in this.context.Empleados.FromSqlRaw(sql)
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task InsertarEmpleadoAsync(string apellido, string oficio, int dir, int salario, int comision, string nomDept)
        {
            string sql = "CALL SP_INSERTAR_EMPLEADO(@apellido,@oficio,@dir,@salario,@comision,@nomdept);";
            MySqlParameter pamApe = new MySqlParameter("@apellido", apellido);
            MySqlParameter pamOfi = new MySqlParameter("@oficio", oficio);
            MySqlParameter pamDir = new MySqlParameter("@dir", dir);
            MySqlParameter pamSal = new MySqlParameter("@salario", salario);
            MySqlParameter pamCom = new MySqlParameter("@comision", comision);
            MySqlParameter pamDept = new MySqlParameter("@nomdept", nomDept);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSal, pamCom, pamDept);
        }
    }
}
