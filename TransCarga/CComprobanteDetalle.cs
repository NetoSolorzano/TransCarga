using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransCarga
{
    public class CComprobanteDetalle
    {
        public int nro_item { get; set; }       // n 5 
        public string cod_prod { get; set; }    // an 30
        public string cod_und_med { get; set; } // an 3
        public string descrip { get; set; }     // an 500
        public double cant { get; set; }        // n 12,10
        public double val_unit_item { get; set; }   // n 12,10
        public double sub_tot { get; set; }     // n 12,2
        public double dsc_item { get; set; }    // n 12,2
        public double val_vta_item { get; set; }    // n 12,2
        public double igv_item { get; set; }    // n 12,2
        //public double isc_item { get; set; }    // n 12,2
        public double prec_unit_item { get; set; }  // n 12,10
        public string tip_afec_igv { get; set; }    // an 2
        //public string tip_afec_isc { get; set; }    // an 2
        //public double val_ref_unit_item { get; set; }   // an 12,10   solo se usa cuando va en comprobante gratuito
        public string cod_prod_sunat { get; set; }  // an 8
        public string cod_prod_gs1 { get; set; }    // an 14
        public string tip_prod_gtin { get; set; }   // an 14
        //public string vehi_placa { get; set; }      // an 15
        public double impsto_tot { get; set; }      // n 12,2
        public double base_igv { get; set; }        // n 12,2
        public int tasa_igv { get; set; }        // n 9
        public string ind_grat { get; set; }    // an 1
        //public double base_isc { get; set; }    // n 12,2
        //public string tasa_isc { get; set; }    // an 2
        //public double base_otr_trib { get; set; }   // n 12,2
        //public double otr_trib_item { get; set; }   // n 12,2
        //public string tasa_otr_trib { get; set; }   // an 2
        //public string cod_cargo_item { get; set; }  // an 2
        //public double factor_cargo_item { get; set; }   // n 3,5
        //public double cargo_item { get; set; }      // n 12,2
        //public double base_cargo_item { get; set; } // n 12,2
        //public string cod_dsc_item { get; set; }    // an 2
        //public double factor_dsc_item { get; set; } // n 3,5
        //public double base_dsc_item { get; set; }   // n 12,2
        //public string nomconcept { get; set; }      // an 100
        //public string codconcept { get; set; }      // an 4
        //public string valorconcept { get; set; }    // an 200
        //public double icbper_item { get; set; }     // 12,2
        //public int cant_icbper { get; set; }        // n 5
        //public double monto_unit_icbper { get; set; }   // n 3,5
        //public double base_ir { get; set; }         // n 12,2
        //public double ir_item { get; set; }         // n 12,2
        //public double tasa_ir { get; set; }         // n 3,5
    }
}
