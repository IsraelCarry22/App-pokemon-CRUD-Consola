using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    static string connectionString = "Server=ISRAELMONCADA\\SQLEXPRESS;Database=Pokemon;User Id=root;Password=0n1n6hhge7;TrustServerCertificate=true;";
    static object result;

    static void Main(string[] args)
    {
        bool circle = true;
        while (circle)
        {
            Console.Clear();
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Registrar");
            Console.WriteLine("3. Salir");

            Console.Write("Opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Ingrese su nombre de usuario:");
                    string usuario = Console.ReadLine();

                    Console.WriteLine("Ingrese su contraseña:");
                    string contrasena = Console.ReadLine();

                    if (Iniciar_seccion(usuario, contrasena))
                    {
                        Console.Clear();
                        SubMenu(result);
                    }
                    else
                    {
                        Console.WriteLine("*** Credenciales incorrectas/usuario inactivo. Acceso denegado. ***");
                    }
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Ingrese su nombre de usuario:");
                    usuario = Console.ReadLine();

                    Console.WriteLine("Ingrese su contraseña:");
                    contrasena = Console.ReadLine();

                    if (Regsitrar(usuario, contrasena))
                    {
                        Console.WriteLine("Registro exitoso a la base de datos.");
                    }
                    else
                    {
                        Console.WriteLine("*** Error al regsitrar el usuario. ***");
                    }
                    break;
                case "3":
                    circle = false;
                    break;
                default:
                    Console.WriteLine("*** Opción no válida. ***");
                    break;
            }
        }
    }

    static bool Iniciar_seccion(string nombreUsuario, string contrasena, int idUsuario = 0)
    {
        idUsuario = -1;  // Inicializamos el idUsuario con un valor negativo (indica que no se encontró)

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "SELECT Id FROM [User] WHERE Username = @usuario AND Password = @contrasena";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetros a la consulta
                    command.Parameters.AddWithValue("@usuario", nombreUsuario);
                    command.Parameters.AddWithValue("@contrasena", EncriptarContrasena(contrasena));

                    // Ejecutar la consulta y capturar el ID del usuario
                    result = command.ExecuteScalar();

                    // Si result no es null, se ha encontrado un usuario
                    if (result != null)
                    {
                        // Asignamos el ID del usuario a la variable de salida
                        idUsuario = Convert.ToInt32(result);

                        // Cerramos la conexión y retornamos true
                        connection.Close();
                        return true;
                    }
                }

                connection.Close();
                return false;  // Si no se encontró el usuario
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                connection.Close();
                return false;
            }
        }
    }

    static bool Regsitrar(string nombreUsuario, string contrasena)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO [User] (Username, Password, IdCreationUser, CreateDate)VALUES(@usuario, @contrasena, 1, GETDATE())";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", nombreUsuario);
                    command.Parameters.AddWithValue("@contrasena", EncriptarContrasena(contrasena)); // Asegúrate de encriptar la contraseña

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected != null)
                    {
                        connection.Close();
                        return true;
                    }

                    connection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

    static string EncriptarContrasena(string contrasena)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contrasena));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    static void SubMenu(object id)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Seleccione una tabla:");
            Console.WriteLine("1. Entrenador");
            Console.WriteLine("2. Pokemón");
            Console.WriteLine("3. Salir");

            Console.Write("Opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Clear();
                    CRUD_Menu(int.Parse(id.ToString()), "Entrenador");
                    break;
                case "2":
                    Console.Clear();
                    CRUD_Menu(int.Parse(id.ToString()), "Pokémon");
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("*** Opción no válida. ***");
                    break;
            }
        }
    }

    static void CRUD_Menu(int id, string Tabla)
    {
        while (true)
        {
            Console.WriteLine($"Tabla {Tabla}");
            Console.WriteLine("1. (C) Crear:");
            Console.WriteLine("2. (R) Leer");
            Console.WriteLine("3. (U) Actualizar");
            Console.WriteLine("4. (D) Eliminar");
            Console.WriteLine("5. Salir");

            Console.Write("Opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    if (Tabla == "Entrenador") { Entrenador_Crear(id); }
                    else { Pokemón_Crear(id); }
                    break;
                case "2":
                    if (Tabla == "Entrenador") { Leer(Tabla); }
                    else { Leer(Tabla); }
                    break;
                case "3":
                    if (Tabla == "Entrenador") { Sub_A_E(id, "actualizar", Tabla); }
                    else { Sub_A_E(id, "actualizar", Tabla); }
                    return;
                case "4":
                    if (Tabla == "Entrenador") { Sub_A_E(id, "eliminar", Tabla); }
                    else { Sub_A_E(id, "eliminar", Tabla); }
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    static void Entrenador_Crear(int id)
    {
        Console.Clear(); 
        Console.WriteLine("Registro de entrenador\n\n\n");
        Console.Write("Nombre");
        string txtNombre = Console.ReadLine();
        Console.Write("Apellido paterno");
        string txtApellidoPaterno = Console.ReadLine();
        Console.Write("Apellido materno");
        string txtApellidoMaterno = Console.ReadLine();
        Console.Write("Edad");
        int intEdad = int.Parse(Console.ReadLine());
        Console.Write("Ciudad");
        int? intCiudad = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Medalla (op)");
        int? intMedalla = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Mochila (op)");
        int? intMochila = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = $"INSERT INTO [Entrenador] (Nombre, ApellidoPaterno, ApellidoMaterno, Edad, Ciudad_ID, Medalla_ID, Mochila_ID, IdCreationUser) VALUES (@nombre, @apellidopaterno, @apellidomaterno, @edad, @ciudad, @medalla, @mochila, @IdCreationUser)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", txtNombre);
                    command.Parameters.AddWithValue("@apellidopaterno", txtApellidoPaterno);
                    command.Parameters.AddWithValue("@apellidomaterno", txtApellidoMaterno);
                    command.Parameters.AddWithValue("@edad", intEdad);
                    command.Parameters.AddWithValue("@ciudad", (object)intCiudad ?? DBNull.Value);
                    command.Parameters.AddWithValue("@medalla", (object)intMedalla ?? DBNull.Value);
                    command.Parameters.AddWithValue("@mochila", (object)intMochila ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdCreationUser", id);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Empleado creado con éxito.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    static void Pokemón_Crear(int id)
    {
        Console.Clear();
        Console.WriteLine("Registro de Pokémon\n\n\n");

        Console.Write("Nombre: ");
        string txtNombre = Console.ReadLine();
        Console.Write("Especie: ");
        int intEspecie = int.Parse(Console.ReadLine());
        Console.Write("Nivel: ");
        int intNivel = int.Parse(Console.ReadLine());
        Console.Write("Estado: ");
        int? intEstado = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Entrenador: ");
        int? intEntrenador = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Habilidad: ");
        int? habilidadId = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Equipo Villano: ");
        int? intEquipoVillano = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Huevo: ");
        int? intHuevo = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());

        // Insertar el nuevo Pokémon en la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = @"INSERT INTO [Pokémon] (Nombre, Especie_ID, Nivel, Estado_ID, Entrenador_ID, Habilidad_ID, EquipoVillano_ID, Huevo_ID, IdCreationUser)VALUES (@nombre, @especieId, @nivel, @estadoId, @entrenadorId, @habilidadId, @equipoVillanoId, @huevoId, @id)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", txtNombre);
                    command.Parameters.AddWithValue("@especieId", intEspecie);
                    command.Parameters.AddWithValue("@nivel", intNivel);
                    command.Parameters.AddWithValue("@estadoId", (object)intEstado ?? DBNull.Value);
                    command.Parameters.AddWithValue("@entrenadorId", (object)intEntrenador ?? DBNull.Value); // ID del entrenador pasado al método
                    command.Parameters.AddWithValue("@habilidadId", (object)habilidadId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@equipoVillanoId", (object)intEquipoVillano ?? DBNull.Value);
                    command.Parameters.AddWithValue("@huevoId", (object)intHuevo ?? DBNull.Value); // Si es nulo, se pasa DBNull.Value
                    command.Parameters.AddWithValue("@id", id );

                    command.ExecuteNonQuery();
                }

                Console.WriteLine("¡Pokémon registrado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el Pokémon: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
    }

    static void Leer(string Tabla)
    {
        Console.Clear();
        Console.WriteLine($"Tabla {Tabla}");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = $"SELECT * FROM [{Tabla}] WHERE Status=1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (Tabla == "Entrenador")
                        {
                            Console.WriteLine("ID: {0}, Nombre: {1}, Apellido Paterno: {2}, Apellido Materno: {3}", "Edad: {4}", "Ciudad: {5}", "Medalla: {6}", "Mochila: {7}",
                            reader["ID_Entrenador"], reader["Nombre"], reader["ApellidoPaterno"], reader["ApellidoMaterno"], reader["Edad"], reader["Ciudad_ID"], reader["Medalla_ID"], reader["Mochila_ID"]);
                        }
                        else if (Tabla == "Pokemón")
                        {
                            Console.WriteLine("ID: {0}, Nombre: {1}, Especie: {2}, Nivel: {3}", "Estado: {4}", "Entrenador: {5}", "Habilidad: {6}", "Equipo Villano: {7}", "Huevo: {8}",
                            reader["ID_Pokemón"], reader["Nombre"], reader["Especie_ID"], reader["Nivel"], reader["Estado_ID"], reader["Entrenador_ID"], reader["Habilidad_ID"], reader["EquipoVillano_ID"], reader["Huevo_ID"]);
                        }
                    }
                    Console.WriteLine("\n\n\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    //funcioona para tomar un id despues de mostrar y el usuario elige que va a eliminar o actualizar
    static void Sub_A_E(int id_EditUser, string tipo_accion, string Tabla)
    {
        Leer(Tabla);

        try
        {
            Console.Write("Ingrese el ID que dese {0}?: ", tipo_accion);
            int id_accion = int.Parse(Console.ReadLine());

            switch (tipo_accion)
            {
                case "actualizar":
                    if (Tabla == "Entrenador") { Entrenador_Actualizar(id_accion, id_EditUser); }
                    else { Pokémon_Actualizar(id_accion, id_EditUser); }
                    break;
                case "eliminar":
                    if (Tabla == "Entrenador") { Eliminar(id_accion, id_EditUser, Tabla); }
                    else { Eliminar(id_accion, id_EditUser, Tabla); }
                    return;
            }
        }
        catch { }
    }

    static void Entrenador_Actualizar(int id_actualizar, int id_EditUser)
    {
        Console.Clear();
        Console.WriteLine("Actualizar entrenador\n\n\n");

        // Datos del entrenador
        Console.Write("Nombre: ");
        string txtNombre = Console.ReadLine();
        Console.Write("Apellido paterno: ");
        string txtApellidoPaterno = Console.ReadLine();
        Console.Write("Apellido materno: ");
        string txtApellidoMaterno = Console.ReadLine();
        Console.Write("Edad: ");
        int intEdad = int.Parse(Console.ReadLine());
        Console.Write("Ciudad: ");
        int? intCiudad = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Medalla (opcional): ");
        int? intMedalla = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Mochila (opcional): ");
        int? intMochila = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Consulta UPDATE para actualizar los datos del entrenador por su ID
                string query = $@"UPDATE [Entrenador] SET Nombre = @nombre, ApellidoPaterno = @apellidopaterno, ApellidoMaterno = @apellidomaterno, Edad = @edad, Ciudad_ID = @ciudad, Medalla_ID = @medalla, Mochila_ID = @mochila, IdEditUser = @idedituser WHERE ID_Entrenador = {id_actualizar}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Asignamos los parámetros a la consulta SQL
                    command.Parameters.AddWithValue("@nombre", txtNombre);
                    command.Parameters.AddWithValue("@apellidopaterno", txtApellidoPaterno);
                    command.Parameters.AddWithValue("@apellidomaterno", txtApellidoMaterno);
                    command.Parameters.AddWithValue("@edad", intEdad);
                    command.Parameters.AddWithValue("@ciudad", (object)intCiudad ?? DBNull.Value); // Usar DBNull si es null
                    command.Parameters.AddWithValue("@medalla", (object)intMedalla ?? DBNull.Value); // Usar DBNull si es null
                    command.Parameters.AddWithValue("@mochila", (object)intMochila ?? DBNull.Value); // Usar DBNull si es null
                    command.Parameters.AddWithValue("@idedituser", id_EditUser);  // El ID del entrenador que se actualizará

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Entrenador actualizado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró un entrenador con ese ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    static void Pokémon_Actualizar(int id_actualizar, int id_EditUser)
    {
        Console.Clear();
        Console.WriteLine("Actualizar Pokémon\n\n\n");

        // Datos del Pokémon
        Console.Write("Nombre: ");
        string txtNombre = Console.ReadLine();
        Console.Write("Especie: ");
        int intEspecie = int.Parse(Console.ReadLine());
        Console.Write("Nivel: ");
        int intNivel = int.Parse(Console.ReadLine());
        Console.Write("Estado: ");
        int? intEstado = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Entrenador: ");
        int? intEntrenador = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Habilidad: ");
        int? habilidadId = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Equipo Villano (op): ");
        int? equipoVillanoId = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());
        Console.Write("Huevo (op): ");
        int? huevoId = string.IsNullOrEmpty(Console.ReadLine()) ? (int?)null : int.Parse(Console.ReadLine());

        // Actualizar Pokémon en la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = $"UPDATE [Pokemón] SET Nombre = @nombre, Especie_ID = @especie, Nivel = @nivel, Estado_ID = @estado, Entrenador_ID = @entrenador, Habilidad_ID = @habilidad, EquipoVillano_ID = @equipoVillano, Huevo_ID = @huevo, IdEditUser = @idedituser WHERE ID_Pokémon = {id_actualizar}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", txtNombre);
                    command.Parameters.AddWithValue("@especie", intEspecie);
                    command.Parameters.AddWithValue("@nivel", intNivel);
                    command.Parameters.AddWithValue("@estado", (object)intEstado ?? DBNull.Value);
                    command.Parameters.AddWithValue("@entrenador", (object)intEntrenador ?? DBNull.Value);
                    command.Parameters.AddWithValue("@habilidad", (object)habilidadId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@equipoVillano", (object)equipoVillanoId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@huevo", (object)huevoId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@idedituser", id_EditUser);  // El ID que se quiere actualizar

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Pokémon actualizado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró el Pokémon con el ID especificado.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el Pokémon: " + ex.Message);
            }
        }
    }

    static void Eliminar(int id_eliminar, int id_EditUser, string tabla)
    {
        // Insertar el nuevo Pokémon en la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = $"INSERT INTO [{tabla}] WHERE ID_{tabla}={id_eliminar} (Status,IdEditUser)VALUES (@status, @id)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", 0);
                    command.Parameters.AddWithValue("@id", id_EditUser);

                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"¡{tabla} eliminado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el {tabla}: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
