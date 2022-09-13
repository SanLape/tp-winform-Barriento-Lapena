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
                dataGridViewArticulos.Columns["ImagenUrl"].Visible = false;
                CargarImagen(lista[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dataGridViewArticulos_SelectionChanged(object sender, EventArgs e)
        {
            //DataBoundItem = objeto enlazado a la fila actual (hay que castear al tipo de obj ) y se lo asiganmos a una variable
            Articulo seleccionado = (Articulo)dataGridViewArticulos.CurrentRow.DataBoundItem;
            CargarImagen(seleccionado.ImagenUrl);
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
    }
}
