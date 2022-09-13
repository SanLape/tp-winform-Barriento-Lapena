using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Principal
{
    public partial class frmCargarArticulo : Form
    {
        public frmCargarArticulo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Articulo Nuevo = new Articulo();
            ArticuloNegocio Negocio = new ArticuloNegocio();
            try
            {
                Nuevo.Id = int.Parse(txtId.Text);
                Nuevo.Codigo = txtCodigo.Text;
                Nuevo.Nombre = txtNombre.Text;
                Nuevo.Descripcion = txtDescripcion.Text;
                Nuevo.ImagenUrl = txtUrlImagen.Text;
                Nuevo.Precio = decimal.Parse(txtPrecio.Text);
                Nuevo.marca = (Marca)cboxMarca.SelectedItem;
                Nuevo.categoria = (Categoria)cboxCategoria.SelectedItem;

                Negocio.Agregar(Nuevo);
                MessageBox.Show("Guardado!");
                Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmCargarArticulo_Load(object sender, EventArgs e)
        {
            //precargar en los desplegables de marca y categoria
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboxCategoria.DataSource = categoriaNegocio.Listar();
                cboxMarca.DataSource = marcaNegocio.Listar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string img)
        {
            try
            {
                pictureBoxImagen.Load(img);
            }
            catch (Exception)
            {

                pictureBoxImagen.Load("https://stockperfume.com/wp-content/uploads/2022/02/2248045_2adf358a479610b04c0848672d49776e.jpg");
            }
        }
    }
}
