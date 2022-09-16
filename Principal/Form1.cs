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
    public partial class Form1 : Form
    {
        private List<Articulo> lista;
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridViewArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargar();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {

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
        }
    }
}
