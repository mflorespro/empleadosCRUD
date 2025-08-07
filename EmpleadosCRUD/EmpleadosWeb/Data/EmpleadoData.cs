using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmpleadosWeb.Models;
using Microsoft.Data.SqlClient;

namespace EmpleadosWeb.Data
{
    public class EmpleadoData
    {
        private readonly string _connectionString;

        public EmpleadoData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Empleado>> ListarAsync()
        {
            var lista = new List<Empleado>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Empleados", conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Edad = Convert.ToInt32(reader["Edad"]),
                            Correo = reader["Correo"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Puesto = reader["Puesto"].ToString(),
                            Salario = Convert.ToDecimal(reader["Salario"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Empleado> ObtenerAsync(int id)
        {
            Empleado empleado = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Empleados WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        empleado = new Empleado
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Edad = Convert.ToInt32(reader["Edad"]),
                            Correo = reader["Correo"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Puesto = reader["Puesto"].ToString(),
                            Salario = Convert.ToDecimal(reader["Salario"])
                        };
                    }
                }
            }
            return empleado;
        }

        public async Task<bool> GuardarAsync(Empleado empleado)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"INSERT INTO Empleados (Nombre, Apellido, Edad, Correo, Departamento, Puesto, Salario)
                                           VALUES (@Nombre, @Apellido, @Edad, @Correo, @Departamento, @Puesto, @Salario)", conn);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd.Parameters.AddWithValue("@Edad", empleado.Edad);
                cmd.Parameters.AddWithValue("@Correo", empleado.Correo);
                cmd.Parameters.AddWithValue("@Departamento", empleado.Departamento);
                cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> EditarAsync(Empleado empleado)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(@"UPDATE Empleados SET Nombre = @Nombre, Apellido = @Apellido, Edad = @Edad,
                                           Correo = @Correo, Departamento = @Departamento, Puesto = @Puesto, Salario = @Salario
                                           WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", empleado.Id);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                cmd.Parameters.AddWithValue("@Edad", empleado.Edad);
                cmd.Parameters.AddWithValue("@Correo", empleado.Correo);
                cmd.Parameters.AddWithValue("@Departamento", empleado.Departamento);
                cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Empleados WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
