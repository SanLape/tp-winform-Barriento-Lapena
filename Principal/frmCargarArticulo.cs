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
        private Articulo articulo = null;
        public frmCargarArticulo()
        {
            InitializeComponent();
        }
        public frmCargarArticulo(Articulo art)
        {
            InitializeComponent();
            this.articulo = art;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio Negocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();
                
                
                articulo.Id = int.Parse(txtId.Text);
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cboxMarca.SelectedItem;
                articulo.categoria = (Categoria)cboxCategoria.SelectedItem;

                if(articulo.Id != 0)
                {
                    Negocio.modificar(articulo);
                    MessageBox.Show("Modificado");
                }
                else
                {
                    Negocio.Agregar(articulo);
                    MessageBox.Show("Guardado!");
                }

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
                
                cboxCategoria.ValueMember = "Id";               // datos "escondido" => atrivuto de la calse 
                cboxCategoria.DisplayMember = "Descripcion";    // datos q vamos a mostra
                
                cboxMarca.DataSource = marcaNegocio.Listar();
                
                cboxMarca.ValueMember = "Id";
                cboxMarca.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtId.Text = articulo.Nombre;
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    txtPrecio.Text = articulo.Precio.ToString();

                    cargarImagen(articulo.ImagenUrl);

                    cboxMarca.SelectedValue = articulo.marca.Id;
                    cboxCategoria.SelectedValue = articulo.categoria.Id;
                }
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
