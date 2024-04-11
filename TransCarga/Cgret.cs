﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransCarga
{
    public class Cgret
    {
        public guiaTransp despatchAdvice { get; set; }
    }
    public class Cdocrel
    {
        public string tip_doc_descrip {get; set;}
        public string emi_num_doc { get; set;}
        public string emi_tip_doc { get; set;}
        public string tip_doc { get; set;}
        public string serie_correl { get; set;}
    }
    public class Cremitente
    {
        public string tip_doc { get; set; }
        public string num_doc { get; set; }
        public string raz_soc { get; set; }
    }
    public class Csubcontratado
    {
        public string tip_doc { get; set; }
        public string num_doc { get; set; }
        public string raz_soc { get; set; }
    }
    public class Ctercero
    {
        public string tip_doc {get; set;}
        public string num_doc { get; set;}
        public string raz_soc { get; set;}
    }
    public class Cpartida
    {
        public string cod_ubi { get; set; }
        public string dir { get; set; }
    }
    public class Cllegada
    {
        public string cod_ubi { get; set; }
        public string dir { get; set; }
    }
    public class guiaTransp
    {
        public string tip_doc { get; set; }
        public string serie { get; set; }
        public string correl { get; set; }
        public string fec_emi { get; set; }
        public string hora_emi { get; set; }
        public string ubl_version { get; set; }
        public string customizacion { get; set; }
        public CemisorGR emisor { get; set; }
        public List<Cdocrel> docrel { get; set; }
        public Cremitente remitente { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Csubcontratado subcontratado {get; set;}
        public Cenvio envio { get; set; }
        public List<Cvehiculo> vehiculo { get; set; }
        public List<Cconductor> conductor { get; set; }
        public Cpartida partida { get; set; }
        public Cllegada llegada { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
    }
}
