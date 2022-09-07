using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Principal
{
    class ArticuloDatos
    {
        //funcion listar que devuelve una lista de articulos
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Id,Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio from ARTICULOS";
                //ejecutar el comando en la conexion
                comando.Connection = conexion;
                //abrir conexion
                conexion.Open();
                //ejecutar lectura
                lector = comando.ExecuteReader();
                //si el lector lee algo se ubica en la 1ra posicion (registro) y con el aux le asignamos los datos y se agrega a la lista
                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.IdMarca = (int)lector["IdMarca"];
                    aux.IdCategoria = (int)lector["IdCategoria"];
                    aux.ImagenUrl = (string)lector["ImagenUrl"];
                    aux.Precio = (decimal)lector["Precio"];

                    lista.Add(aux);

                }
                //una vez que sale del while se cierra la conexion
                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
