using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Principal
{
    public partial class frmCatalogo : Form
    {
        private List<Articulo> lista;
        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void dataGridViewArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {  
            Cargar();
            //reinician los combobox
            cboxCriterio.DataSource = null;
            cboxCriterio.Items.Clear();
            cboxCampo.Items.Clear();
            //
            cboxCampo.Items.Add("Código");
            cboxCampo.Items.Add("Nombre");
            cboxCampo.Items.Add("Descripción");
            cboxCampo.Items.Add("Marca");
            cboxCampo.Items.Add("Categoría");
            

            lblFiltroAvanzado.Visible = false;
            txtFiltroAvanzado.Clear();
            txtFiltroAvanzado.Visible = false;
        }

        private void Cargar()
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            try
            {
                lista = datos.Listar();
                dataGridViewArticulos.DataSource = lista;
                ocultarColumnas();
                CargarImagen(lista[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ocultarColumnas()
        {
            dataGridViewArticulos.Columns["Id"].Visible = false;
            dataGridViewArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void dataGridViewArticulos_SelectionChanged(object sender, EventArgs e)
        {
            //DataBoundItem = objeto enlazado a la fila actual (hay que castear al tipo de obj ) y se lo asiganmos a una variable
            if(dataGridViewArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.ImagenUrl);
            }
            
        }

        private void CargarImagen(string img)
        {
            try
            {
                pictureBoxImgArticulo.Load(img);
            }
            catch (Exception)
            {
                pictureBoxImgArticulo.Load("https://stockperfume.com/wp-content/uploads/2022/02/2248045_2adf358a479610b04c0848672d49776e.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmCargarArticulo alta = new frmCargarArticulo();
            alta.ShowDialog();
            Cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;

            frmCargarArticulo modificar = new frmCargarArticulo(seleccionado);
            modificar.ShowDialog();
            Cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
            try
            {
                DialogResult respuestaYesNo = MessageBox.Show("Confirmar baja del articulo?","Eliminar",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuestaYesNo == DialogResult.Yes)
                {
                    negocio.Eliminar(seleccionado.Id);
                    Cargar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ValidarFiltroAvanzado()
        {
            bool validar = false;
            if (cboxCampo.SelectedIndex<0)
            {
                //MessageBox.Show("Por favor seleccionar un campo");
                errorProviderFiltroAvanzado.SetError(cboxCampo, "Debe seleccionar un campo");
                validar = true;
            }

            if (cboxCriterio.SelectedIndex<0)
            {
                //MessageBox.Show("Por favor seleccionar un criterio");
                errorProviderFiltroAvanzado.SetError(cboxCriterio, "Debe seleccionar un criterio");

                validar = true;
            }
            return validar;
        }
        private void BorrarErrorProviderFiltroAvanzado()//elimina el mensaje de error una vez solucionado
        {
            errorProviderFiltroAvanzado.SetError(cboxCampo, "");
            errorProviderFiltroAvanzado.SetError(cboxCriterio, "");
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                BorrarErrorProviderFiltroAvanzado();
                if (ValidarFiltroAvanzado())
                {
                    return;
                }
                //necesitamos el campo, criterio y filtro seleccionado y pasarlos a string
                string campo = cboxCampo.SelectedItem.ToString();
                string criterio = cboxCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dataGridViewArticulos.DataSource = negocio.FiltrarAvanzado(campo, criterio, filtro);
                if(dataGridViewArticulos.Rows.Count == 0)
                {
                    btnModificar.Enabled = false;
                    btnEliminar.Enabled = false;
                }
            }
            catch (Exception ex)    
            {
                throw ex;
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFilrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                filtro = filtro.ToUpper();

                listaFilrada = lista.FindAll(x => x.Codigo.ToUpper().Contains(filtro) || x.Nombre.ToUpper().Contains(filtro) || x.marca.Descripcion.ToUpper().Contains(filtro) || x.categoria.Descripcion.ToUpper().Contains(filtro));
                // .ToUpper() convierte los strings en MAYUSCULAS para haecr las comparaciones
                // .Contains compara las palabras BUSCADA.CONTAINS( DONDE )
            }
            else
            {
                listaFilrada = lista;
            }

            dataGridViewArticulos.DataSource = null;
            dataGridViewArticulos.DataSource = listaFilrada;
            ocultarColumnas();
            if (dataGridViewArticulos.Rows.Count == 0)
            {
                btnModificar.Enabled = false;
                btnEliminar.Enabled = false;
            }
        }

 
        private void cboxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string seleccionado = cboxCampo.SelectedItem.ToString();

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio(); 
            switch (seleccionado)
            {
                case "Código":
                    cboxCriterio.DataSource = null;
                    cboxCriterio.Items.Clear();
                    cboxCriterio.Items.Add("Es igual a");
                    cboxCriterio.Items.Add("Contiene");
                    lblFiltroAvanzado.Visible = true;
                    txtFiltroAvanzado.Visible = true;
                    break;
                case "Nombre":
                    cboxCriterio.DataSource = null;
                    cboxCriterio.Items.Clear();
                    cboxCriterio.Items.Add("Empieza con");
                    cboxCriterio.Items.Add("Termina con");
                    cboxCriterio.Items.Add("Contiene");
                    lblFiltroAvanzado.Visible = true;
                    txtFiltroAvanzado.Visible = true;
                    break;
                case "Descripción":
                    cboxCriterio.DataSource = null;
                    cboxCriterio.Items.Clear();
                    cboxCriterio.Items.Add("Empieza con");
                    cboxCriterio.Items.Add("Termina con");
                    cboxCriterio.Items.Add("Contiene");
                    lblFiltroAvanzado.Visible = true;
                    txtFiltroAvanzado.Visible = true;
                    break;
                case "Marca":
                    cboxCriterio.DataSource = null;
                    cboxCriterio.Items.Clear();
                    cboxCriterio.DataSource = marcaNegocio.Listar();
                    cboxCriterio.SelectedIndex = -1;
                    lblFiltroAvanzado.Visible = false;
                    txtFiltroAvanzado.Visible = false;
                    break;
                case "Categoría":
                    cboxCriterio.DataSource = null;
                    cboxCriterio.Items.Clear();
                    cboxCriterio.DataSource = categoriaNegocio.Listar();
                    cboxCriterio.SelectedIndex = - 1;
                    lblFiltroAvanzado.Visible = false;
                    txtFiltroAvanzado.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            
            Form1_Load(sender, e);
        }
    }
}
