using ListenerTCP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ListenerTCP.Helpers
{
    public class BulkCopyHelper
    {
        public void CargaRawDataBulkCopy(List<Data> datos)
        {
            if (datos.Count() > 0)
            {
                var entidad = datos.FirstOrDefault();
                var propertiesEntidad = entidad.GetType().GetProperties();
                var tableName = "RawData";
                var dataTable = new DataTable(tableName);

                foreach (PropertyInfo PI in propertiesEntidad)
                {
                    var nombre = PI.Name;
                    var propertyType = Nullable.GetUnderlyingType(PI.PropertyType) ?? PI.PropertyType;

                    dataTable.Columns.Add(nombre, propertyType);
                }

                foreach (var dato in datos)
                {
                    var propertiesDato = dato.GetType().GetProperties();
                    DataRow newRow = dataTable.NewRow();

                    foreach (PropertyInfo datoPI in propertiesDato)
                    {
                        var nombre = datoPI.Name;
                        var datoPropertyValue = datoPI.GetValue(dato, null);

                        newRow[nombre] = datoPropertyValue ?? DBNull.Value;
                    }

                    dataTable.Rows.Add(newRow);
                }

                CargaRawDataBulkDataTable(dataTable);
            }
        }

        private void CargaRawDataBulkDataTable(DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString))
            {
                connection.Open();
                using (var bcp = new SqlBulkCopy(connection))
                {
                    bcp.BatchSize = dataTable.Rows.Count;
                    bcp.DestinationTableName = dataTable.TableName;
                    bcp.BulkCopyTimeout = 0;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        SqlBulkCopyColumnMapping Mapping = new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName);
                        bcp.ColumnMappings.Add(Mapping);
                    }

                    bcp.WriteToServer(dataTable);
                    EliminarDuplicados();

                    connection.Close();
                }
            }
        }

        private void EliminarDuplicados()
        {
            var fecha = DateTime.Today;
            var fechaSiguiente = fecha.AddDays(1);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("RawData_EliminarDuplicados", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter paramFechaDesde = cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.DateTime);
                    SqlParameter paramFechaHasta = cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.DateTime);
                    paramFechaDesde.Value = fecha;
                    paramFechaHasta.Value = fechaSiguiente;
                    cmd.CommandTimeout = 0;
                    con.Open();
                    //Ejecuta el SP.
                    var reader = cmd.ExecuteNonQuery();

                    con.Close();
                }
            }
        }
    }
}
