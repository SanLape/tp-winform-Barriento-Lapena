﻿using System;
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
    }
}