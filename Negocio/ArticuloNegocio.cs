using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        //funcion listar que devuelve una lista de articulos
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setConsulta("select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, M.Id IdMar, C.Descripcion Categoria, C.Id IdCat, A.ImagenUrl, A.Precio from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdCategoria = C.Id and A.IdMarca = M.Id");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.marca = new Marca();                                
                    aux.marca.Id = (int)datos.Lector["IdMar"];
                    aux.marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)datos.Lector["IdCat"];
                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];
                    
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
       
        }

        public void Agregar(Articulo Nuevo)
        {
            //conexion a base de datos
            AccesoDatos datos = new AccesoDatos();
            try
            {   
                datos.setConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio)values('"+Nuevo.Codigo+"','"+Nuevo.Nombre+"','"+Nuevo.Descripcion+"',@IdMarca,@IdCategoria,'"+Nuevo.ImagenUrl+"','"+Nuevo.Precio+"')");
                datos.SetParametros("@IdMarca", Nuevo.marca.Id);
                datos.SetParametros("@IdCategoria", Nuevo.categoria.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }    

        }
        public void Modificar(Articulo art)
        {
            AccesoDatos Datos = new AccesoDatos();
            try
            {
                Datos.setConsulta("update ARTICULOS set Codigo = @artCod, Nombre = @artNombre, Descripcion = @descrip, IdMarca = @idMarca, IdCategoria = @idCat, ImagenUrl = @urlimg, Precio = @precio WHERE Id = @id");
                Datos.SetParametros("@artCod", art.Codigo);
                Datos.SetParametros("@artNombre", art.Nombre);
                Datos.SetParametros("@descrip", art.Descripcion);
                Datos.SetParametros("@idMarca", art.marca.Id);
                Datos.SetParametros("@idCat", art.categoria.Id);
                Datos.SetParametros("@urlimg", art.ImagenUrl);
                Datos.SetParametros("@precio", art.Precio);
                Datos.SetParametros("@id", art.Id);

                Datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Datos.cerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setConsulta("delete from ARTICULOS where id=@id");
                datos.SetParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> FiltrarAvanzado(string campo, string criterio, string filtro)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Articulo> lista = new List<Articulo>();
            try
            {
                string consulta = "select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, M.Id IdMar, C.Descripcion Categoria, C.Id IdCat, A.ImagenUrl, A.Precio from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdCategoria = C.Id and A.IdMarca = M.Id and ";
                switch (campo)
                {
                    case "Código":
                        switch (criterio)
                        {
                            case "Es igual a":
                                consulta += "A.Codigo =  '"+filtro+"' ";
                                break;
                            case "Contiene":
                                consulta += "A.Codigo like '%"+filtro+"%' ";
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Nombre":
                        switch (criterio)
                        {
                            case "Empieza con":
                                consulta += "A.Nombre like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "A.Nombre like '%" + filtro + "' ";
                                break;
                            case "Contiene":
                                consulta += "A.Nombre like '%" + filtro + "%' ";
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Descripción":
                        switch (criterio)
                        {
                            case "Empieza con":
                                consulta += "A.Descripcion like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "A.Descripcion like '%" + filtro + "' ";
                                break;
                            case "Contiene":
                                consulta += "A.Descripcion like '%" + filtro + "%' ";
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Marca":
                        consulta += "M.Descripcion = '"+criterio+"' ";
                        break;
                    case "Categoría":
                        consulta += "C.Descripcion = '" + criterio + "' ";
                        break;
                    default:
                        break;
                }
                datos.setConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.marca = new Marca();
                    aux.marca.Id = (int)datos.Lector["IdMar"];
                    aux.marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)datos.Lector["IdCat"];
                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];

                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"],2);

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
