using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransCarga
{
    public class NotaCred
    {
        public Ccreditnote creditnote { get; set; }
    }
    public class Ccreditnote
    {
        public string tip_doc { get; set; }         // an(2)    Tipo de nota 
        public string serie { get; set; }           // an(4)    Serie de la nota
        public string correl { get; set; }          // n(8)     Número de nota
        public string fec_emi { get; set; }         // an(10)   Fecha emisión
        public string cod_mon { get; set; }         // an(3)    Código de moneda internacional
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cdocmodif docmodif { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
    }
    public class Cdocmodif
    {
        public string tip_doc { get; set; }
        public string serie_correl { get; set; }
        public string cod_ref { get; set; }
        public string descrip_motiv { get; set; }
        public string fec_emi { get; set; }
    }
}
