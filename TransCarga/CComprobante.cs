using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransCarga
{
    public class CComprobante
    {
        public string cod_ubi { get; set; }     // 
        public string urb { get; set; }     // 
        public string prov { get; set; }     // 
        public string dep { get; set; }     // 
        public string dist { get; set; }     // 
        public string cod_pais { get; set; }     // 
        public string cod_sucur { get; set; }     // 
        public string telef { get; set; }     // 
        public string website { get; set; }     // 
        public string nom_comer { get; set; }     // 
        public string num_autorizacion { get; set; }     // 
        public string cod_autorizacion { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string raz_soc { get; set; }    // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        public string apellidos { get; set; }     // 
        public string nombres { get; set; }     // 
        public string tip_conductor { get; set; }     // 
        public string num_licencia { get; set; }     // 
        public string num_content { get; set; }     // 
        public string num_precint { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        public string serie { get; set; }     // 
        public string correl { get; set; }     // 
        public string fec_emi { get; set; }     // 
        public string cod_mon { get; set; }     // 
        public string tip_oper { get; set; }     // 
        public string vehi_placa { get; set; }     // 
        public string fec_venc { get; set; }     // 
        public string ord_compr { get; set; }     // 
        public string cond_pago { get; set; }     // 
        public string hora_emi { get; set; }     // 
        public string cod_mon_ref { get; set; }     // 
        public string cod_mon_obj { get; set; }     // 
        public string factor { get; set; }     // 
        public string fec_tipo_cambio { get; set; }     // 
        public string ubl_version { get; set; }     // 
        public string customizacion { get; set; }     // 
        public string porcent { get; set; }     // 
        public string cod { get; set; }     // 
        public string monto { get; set; }     // 
        public string cod_bn { get; set; }     // 
        public string med_pago { get; set; }     // 
        //public string cod_mon { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        public string serie_correl { get; set; }     // 
        public string cod_ref { get; set; }     // 
        public string descrip_motiv { get; set; }     // 
        //public string fec_emi { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string serie_correl { get; set; }     // 
        //public string fec_emi { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string serie_correl { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        //public string nom_comer { get; set; }     // 
        //public string dir { get; set; }     // 
        //public string cod_ubi { get; set; }     // 
        //public string dep { get; set; }     // 
        //public string prov { get; set; }     // 
        //public string dist { get; set; }     // 
        //public string cod_pais { get; set; }     // 
        public string cond { get; set; }     // 
        //public string email { get; set; }     // 
        //public string telef { get; set; }     // 
        //public string website { get; set; }     // 
        //public string cod_sucur { get; set; }     // 
        //public string urb { get; set; }     // 
        //public string num_autorizacion { get; set; }     // 
        //public string cod_autorizacion { get; set; }     // 
        public string cod_motiv_traslado { get; set; }     // 
        //public string descrip_motiv { get; set; }     // 
        public string ind_transb { get; set; }     // 
        public string peso_bruto_total { get; set; }     // 
        public string cod_und_med { get; set; }     // 
        public string num_bultos { get; set; }     // 
        public string cod_modal_traslado { get; set; }     // 
        public string fec_ini_traslado { get; set; }     // 
        public string fec_entrega { get; set; }     // 
        public string sus_diff { get; set; }     // 
        public string ind_trasla { get; set; }     // 
        public string ind_transb_prog { get; set; }     // 
        public string tip_event { get; set; }     // 
        public string peso_bruto_total_item { get; set; }     // 
        public string cod_und_med_item { get; set; }     // 
        public string ind_traslado_totdam { get; set; }     // 
        public string nro_contenedor_01 { get; set; }     // 
        public string nro_precinto_01 { get; set; }     // 
        public string nro_contenedor_02 { get; set; }     // 
        public string nro_precinto_02 { get; set; }     // 
        public string ind_traslado_tot { get; set; }     // 
        public string ind_retorno_enva_vacio { get; set; }     // 
        public string ind_retorno_vehi_vacio { get; set; }     // 
        public string ind_transp_subcontra { get; set; }     // 
        public string ind_paga_flete { get; set; }     // 
        public string anotacion { get; set; }     // 
        public string descrip { get; set; }     // 
        public string monto_neto { get; set; }     // 
        //public string cod_mon { get; set; }     // 
        public string orig_dir { get; set; }     // 
        public string orig_cod_ubi { get; set; }     // 
        public string orig_dep { get; set; }     // 
        public string orig_prov { get; set; }     // 
        public string orig_dist { get; set; }     // 
        public string orig_urb { get; set; }     // 
        public string orig_cod_pais { get; set; }     // 
        public string dest_dir { get; set; }     // 
        public string dest_cod_ubi { get; set; }     // 
        public string dest_dep { get; set; }     // 
        public string dest_prov { get; set; }     // 
        public string dest_dist { get; set; }     // 
        public string dest_urb { get; set; }     // 
        public string dest_cod_pais { get; set; }     // 
        //public string vehi_placa { get; set; }     // 
        public string vehi_cons_insc { get; set; }     // 
        public string vehi_marca { get; set; }     // 
        public string licen_cond { get; set; }     // 
        public string transpor_tip_doc { get; set; }     // 
        public string transpor_num_doc { get; set; }     // 
        public string transpor_raz_soc { get; set; }     // 
        public string transpor_modal { get; set; }     // 
        public string peso_tot { get; set; }     // 
        public string peso_und_med { get; set; }     // 
        public string cod_puerto { get; set; }     // 
        public string cod_aeropuerto { get; set; }     // 
        public string nom_puerto { get; set; }     // 
        public string nom_aeropuerto { get; set; }     // 
        public string incoterms { get; set; }     // 
        public string incoterms_descrip { get; set; }     // 
        //public string cod_ubi { get; set; }     // 
        //public string dir { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string cod_sucur { get; set; }     // 
        public string longitid_geo { get; set; }     // 
        public string latitud_geo { get; set; }     // 
        //public string cod_ubi { get; set; }     // 
        //public string dir { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string cod_sucur { get; set; }     // 
        //public string longitid_geo { get; set; }     // 
        //public string latitud_geo { get; set; }     // 
        public string imp_percep { get; set; }     // 
        public string imp_cob { get; set; }     // 
        public string base_impo { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        public string cod_reten { get; set; }     // 
        public string factor_reten { get; set; }     // 
        public string imp_tot_reten { get; set; }     // 
        public string imp_tot_pagado { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        public string grav { get; set; }     // 
        public string inaf { get; set; }     // 
        public string exo { get; set; }     // 
        public string grat { get; set; }     // 
        public string sub_tot { get; set; }     // 
        public string dsc_item { get; set; }     // 
        public string val_vent { get; set; }     // 
        public string igv { get; set; }     // 
        public string isc { get; set; }     // 
        public string imp_tot { get; set; }     // 
        public string otr_carg { get; set; }     // 
        public string otr_trib { get; set; }     // 
        public string dsct_tot { get; set; }     // 
        public string dsct_glob { get; set; }     // 
        public string antic { get; set; }     // 
        public string impsto_tot { get; set; }     // 
        public string exp { get; set; }     // 
        public string trib_exp { get; set; }     // 
        public string trib_inaf { get; set; }     // 
        public string trib_exo { get; set; }     // 
        public string trib_grat { get; set; }     // 
        public string grav_ivap { get; set; }     // 
        public string ivap { get; set; }     // 
        public string base_isc { get; set; }     // 
        public string base_otr_trib { get; set; }     // 
        public string prec_tot { get; set; }     // 
        public string redondeo { get; set; }     // 
        public string icbper { get; set; }     // 
        public string base_ir { get; set; }     // 
        public string trib_ir { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        public string num_mtc { get; set; }     // 
        //public string num_autorizacion { get; set; }     // 
        //public string cod_autorizacion { get; set; }     // 
        //public string vehi_placa { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        public string cert_vehi { get; set; }     // 
        //public string num_autorizacion { get; set; }     // 
        //public string cod_autorizacion { get; set; }     // 
        //public string tip_conductor { get; set; }     // 
        //public string num_licencia { get; set; }     // 
        //public string apellidos { get; set; }     // 
        //public string nombres { get; set; }     // 
        //public string num_doc { get; set; }     // 
        //public string tip_doc { get; set; }     // 
        //public string raz_soc { get; set; }     // 
        //public string num_mtc { get; set; }     // 
        //public string num_autorizacion { get; set; }     // 
        //public string cod_autorizacion { get; set; }     // 
        //public string vehi_placa { get; set; }     // 
        //public string cert_vehi { get; set; }     // 
        //public string num_autorizacion { get; set; }     // 
        //public string cod_autorizacion { get; set; }     // 
        //public string tip_vehiculo { get; set; }     // 
        //public string dest_cod_ubi { get; set; }     // 
        //public string dest_dir { get; set; }     // 
        //public string dest_urb { get; set; }     // 
        //public string dest_prov { get; set; }     // 
        //public string dest_dep { get; set; }     // 
        //public string dest_dist { get; set; }     // 
        //public string dest_cod_pais { get; set; }     // 
        //public string cond { get; set; }     // 
        //public string num_mtc { get; set; }     // 

    }

    public class adquiriente
    {
        public int tip_doc { get; set; }    // n 1
        public string num_doc { get; set; } // an 15
        public string raz_soc { get; set; } // an 100
        public string dir { get; set; }     // an 100
        public string email { get; set; }     // 
        public string cod_ubi { get; set; }     // 
        public string urb { get; set; }     // 
        public string prov { get; set; }     // 
        public string dep { get; set; }     // 
        public string dist { get; set; }     // 
        public string cod_pais { get; set; }     // 
        public string cod_sucur { get; set; }     // 
        public string telef { get; set; }     // 
        public string website { get; set; }     // 
    }

    public class emisor
    {

    }
}
