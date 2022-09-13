﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    
    public class Articulo
    {
        //Id,Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }
        
        public Marca marca { get; set; }
        public Categoria categoria { get; set; }
    }
}
