using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DentaCare.Refactored
{
    public class SqlServerEjecutor : IRepositorio
    {
        private string _cadenaConexionSql = "Server=localhost,1435;Database=DentalDb;User Id=sa;Password=DentalPass123!;TrustServerCertificate=True;"; 
        public void GuardarCita(Cita cita) 
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexionSql))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Citas (Id, PacienteId, OdontologoId, Fecha, Copago) VALUES (@id, @p, @o, @f, @c)", conn);
                cmd.Parameters.AddWithValue("@id", cita.Id);
                cmd.Parameters.AddWithValue("@p", cita.Paciente.Id);
                cmd.Parameters.AddWithValue("@o", cita.Odontologo.Id);
                cmd.Parameters.AddWithValue("@f", cita.FechaHora);
                cmd.Parameters.AddWithValue("@c", cita.CopagoCalculado);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarCita(String citaId, decimal penalizacion) 
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexionSql))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Citas SET Estado = 'CANCELADA', Penalizacion = @pen WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@pen", penalizacion);
                cmd.Parameters.AddWithValue("@id", citaId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
