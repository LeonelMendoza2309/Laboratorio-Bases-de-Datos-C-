using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoProducto
{
    public class conexionDB
    {
        private static string cadenaConexion = "server=localhost;port=3306;database=ProdDB;user id=root;password=leonelM2005;";

        private static string MaskConnectionString(string cs)
        {
            if (string.IsNullOrEmpty(cs)) return cs;
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(cs, "(password\\*\\*\\*=)([^;]+)", "$1****", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            catch
            {
                return cs;
            }
    // No-op placeholder: patch applied successfully. (No functional changes.)
    }

        public static MySqlConnection ObtenerConexion()
        {
            try
            {
                string sanitized = cadenaConexion ?? string.Empty;

                sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"(?:^|;)\s*[^=;]+=\s*None\s*(?=;|$)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, ";{2,}", ";").Trim(';');

                if (string.IsNullOrWhiteSpace(sanitized))
                {
                    var msg = "Cadena de conexión vacía tras sanitización.";
                    Console.WriteLine(msg);
                    try { MessageBox.Show(msg, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
                    return null;
                }

                Console.WriteLine("Usando cadena de conexión: " + MaskConnectionString(sanitized));

                try
                {
                    MySqlConnection conexion = new MySqlConnection(sanitized);
                    conexion.Open();
                    return conexion;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al abrir conexión: " + ex.Message);
                    Console.WriteLine(ex.ToString());
                    try { MessageBox.Show("No se pudo conectar a la base de datos:\n" + ex.Message + "\nCadena: " + MaskConnectionString(sanitized), "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
                    return null;
                }
            }
            catch (ArgumentException aex)
            {
                try
                {
                    var builder = new MySqlConnectionStringBuilder();
                    builder.ConnectionString = cadenaConexion ?? string.Empty;

                    var keysToRemove = new List<string>();
                    foreach (var key in builder.Keys)
                    {
                        var k = key.ToString();
                        try
                        {
                            var val = builder[k];
                            if (val != null && string.Equals(val.ToString(), "None", StringComparison.OrdinalIgnoreCase))
                            {
                                keysToRemove.Add(k);
                            }
                        }
                        catch
                        {
                        }
                    }

                    foreach (var k in keysToRemove)
                    {
                        try { builder.Remove(k); } catch { }
                    }

                    var sanitized2 = builder.ConnectionString;
                    if (string.IsNullOrWhiteSpace(sanitized2))
                    {
                        var msg2 = "Cadena de conexión vacía tras builder-sanitización.";
                        Console.WriteLine(msg2);
                        try { MessageBox.Show(msg2, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
                        return null;
                    }

                    Console.WriteLine("Usando cadena de conexión (builder): " + MaskConnectionString(sanitized2));
                    try
                    {
                        MySqlConnection conexion = new MySqlConnection(sanitized2);
                        conexion.Open();
                        return conexion;
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Error al abrir conexión (builder): " + ex2.Message);
                        Console.WriteLine(ex2.ToString());
                        try { MessageBox.Show("No se pudo conectar a la base de datos (builder):\n" + ex2.Message + "\nCadena: " + MaskConnectionString(sanitized2), "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar (sanitización avanzada): " + ex.Message);
                    return null;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al conectar: " + ex.Message);
                return null;
            }

        }
        public static List<Productos> GetProductos(string filtro)
        {
            List<Productos> listaProductos = new List<Productos>();
            string query = "SELECT id, nombre, precio, cantidad, imagen FROM productos";
            if (!string.IsNullOrEmpty(filtro))
            {
                query += " WHERE id LIKE @filtro OR nombre LIKE @filtro OR precio LIKE @filtro OR cantidad LIKE @filtro";
            }

            using (MySqlConnection conn = ObtenerConexion())
            {
                if (conn == null) return listaProductos;
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    using (MySqlDataReader mReader = cmd.ExecuteReader())
                    {
                        while (mReader.Read())
                        {
                            Productos prod = new Productos();
                            prod.ID = mReader["id"] != DBNull.Value ? Convert.ToInt32(mReader["id"]) : 0;
                            prod.Nombre = mReader["nombre"] != DBNull.Value ? mReader["nombre"].ToString() : string.Empty;
                            prod.Precio = mReader["precio"] != DBNull.Value ? Convert.ToDecimal(mReader["precio"]) : 0m;
                            prod.Cantidad = mReader["cantidad"] != DBNull.Value ? Convert.ToInt32(mReader["cantidad"]) : 0;
                            prod.Imagen = mReader["imagen"] != DBNull.Value ? (byte[])mReader["imagen"] : null;
                            listaProductos.Add(prod);
                        }
                        mReader.Close();
                    }
                }
            }

            return listaProductos;
        }
public static bool EliminarProducto(int id)
        {
            using (MySqlConnection conn = ObtenerConexion())
            {
                if (conn == null) return false;
                string query = "DELETE FROM productos WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public static bool InsertSeguro(string tbName, Dictionary<string, object> data)
{
    if (string.IsNullOrWhiteSpace(tbName) || !System.Text.RegularExpressions.Regex.IsMatch(tbName, @"^[a-zA-Z0-9_]+$"))
    {
        Console.WriteLine("Nombre de tabla inválido.");
        return false;
    }

    if (data == null || data.Count == 0)
    {
        Console.WriteLine("No hay datos para insertar.");
        return false;
    }
    var normalized = data.ToDictionary(k => k.Key.Trim().ToLowerInvariant(), v => v.Value);

    foreach (var col in normalized.Keys)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(col, "^[a-z0-9_]+$"))
        {
            Console.WriteLine("Clave de columna inválida: " + col);
            return false;
        }
    }

    var columns = string.Join(", ", normalized.Keys.Select(k => $"`{k}`"));
    var placeholders = string.Join(", ", normalized.Keys.Select(k => "@" + k));
    string sql = $"INSERT INTO `{tbName}` ({columns}) VALUES ({placeholders})";

    try
    {
        using (MySqlConnection conexion = ObtenerConexion())
        {
            if (conexion == null)
            {
                Console.WriteLine("InsertSeguro: no se pudo obtener conexión a la base de datos.");
                try { MessageBox.Show("No se pudo conectar a la base de datos. Revisa la configuración y la salida de la consola.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch {}
                return false;
            }

            using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
            {
                foreach (var kvp in normalized)
                {
                    stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                }

                try
                {
                    Console.WriteLine("SQL: " + sql);
                    foreach (MySqlParameter p in stmt.Parameters)
                    {
                        Console.WriteLine($"{p.ParameterName} = { (p.Value == null || p.Value == DBNull.Value ? "(null)" : p.Value) }");
                    }
                }
                catch { }

                stmt.ExecuteNonQuery();
                return true;
            }
        }
    }
    catch (MySqlException ex)
    {
        Console.WriteLine("Error en INSERT: " + ex.Message);
        Console.WriteLine(ex.ToString());
        try { MessageBox.Show("Error en INSERT: " + ex.Message + "\nComprueba la consola Debug para más detalles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
        return false;
    }
    }

        public static bool UpdateProducto(int id, Dictionary<string, object> data)
        {
            if (id <= 0) return false;
            if (data == null || data.Count == 0) return false;

            var normalized = data.ToDictionary(k => k.Key.Trim().ToLowerInvariant(), v => v.Value);

            foreach (var col in normalized.Keys)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(col, "^[a-z0-9_]+$"))
                {
                    Console.WriteLine("Clave de columna inválida: " + col);
                    return false;
                }
            }

            using (MySqlConnection conn = ObtenerConexion())
            {
                if (conn == null) return false;
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    var sets = new List<string>();
                    foreach (var kv in normalized)
                    {
                        var param = "@" + kv.Key;
                        sets.Add($"`{kv.Key}` = {param}");
                        cmd.Parameters.AddWithValue(param, kv.Value ?? DBNull.Value);
                    }
                    cmd.CommandText = $"UPDATE `productos` SET {string.Join(", ", sets)} WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (MySqlException mex)
                    {
                        Console.WriteLine("Error en UPDATE: " + mex.Message);
                        try { MessageBox.Show("Error al actualizar: " + mex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
                        return false;
                    }
                }
            }
        }

    }
}

