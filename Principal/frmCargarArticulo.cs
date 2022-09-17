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
        private void BorrarErrorProviderGuardarArticulo()//elimina el mensaje de error una vez solucionado
        {
            errorProviderCargarArticulo.SetError(txtCodigo, "");
            errorProviderCargarArticulo.SetError(cboxCategoria , "");
            errorProviderCargarArticulo.SetError(cboxMarca , "");
            errorProviderCargarArticulo.SetError(txtDescripcion, "");
            errorProviderCargarArticulo.SetError(txtNombre, "");
            errorProviderCargarArticulo.SetError(txtPrecio, "");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio Negocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                articulo = new Articulo();//se crea un nuevo articulo vacio
                BorrarErrorProviderGuardarArticulo();
                if (ValidarGuardarArticulo())
                {
                    return;
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cboxMarca.SelectedItem;
                articulo.categoria = (Categoria)cboxCategoria.SelectedItem;

                if(articulo.Id !=0)
                {
                    Negocio.Modificar(articulo);
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
        private bool SoloNumeros(string cadena)
        {
            bool validar = false;
            foreach (char item in cadena)
            {
                if (!(char.IsNumber(item)))
                {
                    validar = true;
                }
            }
            return validar;
        }
        private bool ValidarGuardarArticulo()
        {
            bool validar = false;
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                errorProviderCargarArticulo.SetError(txtCodigo, "Debe escribir un código");
                 validar = true;
            }
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                errorProviderCargarArticulo.SetError(txtNombre, "Debe escribir un nombre");
                validar = true;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                errorProviderCargarArticulo.SetError(txtDescripcion, "Debe escribir una descripcion");
                validar = true;
            }
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                errorProviderCargarArticulo.SetError(txtPrecio, "Debe escribir un precio");
                validar = true;
            }
            if (SoloNumeros(txtPrecio.Text)){
                errorProviderCargarArticulo.SetError(txtPrecio, "Debe escribir solo numeros");
                validar = true;
            }
            
            if (cboxMarca.SelectedIndex < 0)
            {
                errorProviderCargarArticulo.SetError(cboxMarca, "Debe seleccionar una marca");
                 validar = true;
            }
            if (cboxCategoria.SelectedIndex<0)
            {
                errorProviderCargarArticulo.SetError(cboxCategoria, "Debe seleccionar una categoría");
                 validar = true;
            }
            
            return validar;
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
                cboxCategoria.SelectedIndex = -1;

                cboxMarca.DataSource = marcaNegocio.Listar();
                cboxMarca.ValueMember = "Id";
                cboxMarca.DisplayMember = "Descripcion";
                cboxMarca.SelectedIndex = -1;

                if (articulo != null)
                {
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
