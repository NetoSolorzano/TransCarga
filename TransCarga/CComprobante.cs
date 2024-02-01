using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransCarga
{
    public class Cinvoice1
    {
        public CComprobante1 invoice { get; set; }
    }
    public class Cinvoice2
    {
        public CComprobante2 invoice { get; set; }
    }
    public class Cinvoice3
    {
        public CComprobante3 invoice { get; set; }
    }
    public class Cinvoice4
    {
        public CComprobante4 invoice { get; set; }
    }
    public class Cinvoice5
    {
        public CComprobante5 invoice { get; set; }
    }
    public class Cinvoice6
    {
        public CComprobante6 invoice { get; set; }
    }
    public class CComprobante1                  // contado SOLES sin detrac varias leyendas
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        //public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
    }
    public class CComprobante2                  // sin definir 
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
    }
    public class CComprobante3                  // contado c/detrac varias leyendas
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cdetracc detracc { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
    }
    public class CComprobante4                  // credito sin detrac varias leyendas
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
        public List<CCuota> cuota { get; set; }
    }
    public class CComprobante5                  // sin definir 
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
        public List<CCuota> cuota { get; set; }
    }
    public class CComprobante6                  // credito c/detrac varias leyendas
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de comprobante
        public string serie { get; set; }       // an(4)    Serie del comprobante
        public string correl { get; set; }      // n(8)     Número de comprobante
        public string fec_emi { get; set; }     // an(10)   Fecha emisión
        public string cod_mon { get; set; }     // an(3)    Código de moneda internacional
        public string tip_oper { get; set; }        // n(4)    Código tipo de operación sunat
        public string fec_venc { get; set; }        // an(10)  Fecha vencimiento
        public string hora_emi { get; set; }        // an(10)  Hora de emisión
        public string cod_mon_ref { get; set; }     // an(3)    Moneda de referencia para el tipo de cambio
        public string cod_mon_obj { get; set; }     // an(3)    Moneda objetivo para el tipo de cambio
        public string factor { get; set; }          // n(12,3)  Tipo de cambio Sunat
        public string fec_tipo_cambio { get; set; }     // an(10)  Fecha del cambio
        public string ubl_version { get; set; }         // an(3)   Versión UBL
        public string customizacion { get; set; }       // an(3)   Versión Sunat 
        public Cemisor emisor { get; set; }
        public Cadquiriente adquiriente { get; set; }
        public Ctot tot { get; set; }
        public Cdetracc detracc { get; set; }
        public Cforma_pago forma_Pago { get; set; }
        public List<CComprobanteDetalle> det { get; set; }
        public List<Cleyen> leyen { get; set; }
        public List<CCuota> cuota { get; set; }
    }
    public class Cadquiriente
    {
        public string tip_doc { get; set; }    // n 1
        public string num_doc { get; set; } // an 15
        public string raz_soc { get; set; } // an 100
        public string dir { get; set; }     // an 100
        public string email { get; set; }     // 
        public string nom_comer { get; set; }   // 
        public string cod_ubi { get; set; }     // codigo de ubigeo
        public string prov { get; set; }        // Provincia
        public string dep { get; set; }         // Departamento
        public string dist { get; set; }        // Distrito
        public string cod_pais { get; set; }    // Codigo del país
        public string cod_sucur { get; set; }   // Codigo sucursal
        public string telef { get; set; }       // Telefono de contacto
        public string website { get; set; }     // Web Site de contacto 
    }
    public class CadquirienteGR     // DATOS DEL ADQUIRIENTE – GUIA DE REMISION
    {
        public Cadquiriente Cadquiriente { get; set; }
        public string num_autorizacion { get; set; }      // an(50)   Número de autorización especial emitido por la entidad – remitente
        public string cod_autorizacion { get; set; }      // an(2)    Código de entidad autorizadora

    }
    public class Cemisor
    {
        public string tip_doc { get; set; }        // n(1)     Tipo de documento del Emisor
        public string num_doc { get; set; }     // an(11)   Número de RUC del Emisor
        public string raz_soc { get; set; }     // an(150)  Apellidos y nombres, denominación o razón social del Emisor
        public string nom_comer { get; set; }     // an(150)  Nombre Comercial o Nombre Corto del Emisor
        public string dir { get; set; }         // an(200)  Dirección completa y detallada del Emisor
        public string cod_ubi { get; set; }     // an(6)    Código de ubigeo
        public string prov { get; set; }        // an(30)   Provincia
        public string dep { get; set; }         // an(30)   Departamento
        public string dist { get; set; }        // an(30)   Distrito
        public string cod_pais { get; set; }    // an2)     Codigo del país
        public string cod_sucur { get; set; }   // an(4)    Codigo sucursal
        public string telef { get; set; }       // an(100)  Telefono de contacto del emisor
        public string website { get; set; }     // an(100)  Web Site de contacto del emisor
        public string email { get; set; }       // an(100)  E-mail del emisor
    }
    public class CemisorGR                      // DATOS DEL EMISOR – GUIA DE REMISION
    {
        public Cemisor cemisor { get; set; }
        public string num_autorizacion { get; set; }    // an(50)   Número de autorización especial emitido por la entidad – remitente
        public string cod_autorizacion { get; set; }    // an(2)    Código de entidad autorizadora
        public string num_mtc { get; set; }             // an(20)   Número de Registro MTC

    }
    public class Cconductor
    {
        public string num_doc { get; set; }     // n(11)    Numero de documento de identidad del conductor
        public string tip_doc { get; set; }     // an(2)    Tipo de documento de identidad del conductor
        public string apellidos { get; set; }   // an(250)   Apellidos  del conductor
        public string nombres { get; set; }     // an(250)  Nombres  del conductor
        public string tip_conductor { get; set; }  // an(9)
        public string num_licencia { get; set; }  // an(10)
    }   // guía de remision
    public class Cenvio
    {
        public string cod_motiv_traslado { get; set; }  // an(2)    Motivo del traslado
        public string descrip_motiv { get; set; }       // an(100)  Descripción de motivo de traslado
        public decimal peso_bruto_total { get; set; }   // n(12,3)  Peso bruto total de los guía
        public string cod_und_med { get; set; }         // an(4)    Unidad de medida del peso bruto
        public int num_bultos { get; set; }             // n(11)    Numero de Bulltos o Pallets
        public string cod_modal_traslado { get; set; }  // an(2)    Modalidad de Traslado
        public string fec_ini_traslado { get; set; }    // an(10)   Fecha Inicio de traslado
        public string fec_entrega { get; set; }         // an(10)   Fecha de entrega de bienes al transportista
        public string sus_diff { get; set; }            // an(250)  Sustento de la diferencia del Peso bruto total de la carga respecto al peso de los ítems seleccionados
        public string ind_transb_prog { get; set; }     // an(50)   Indicador de Envio  
        public string tip_event { get; set; }           // an(1)    Tipo de evento  
        public decimal peso_bruto_total_item { get; set; }  // n(12,2)  Cuanto motivo de traslado es por Importacion o Exportacion
        public string cod_und_med_item { get; set; }    // an(4)    Cuanto motivo de traslado es por Importacion o Exportacion
        public string ind_traslado_totdam { get; set; }  // an(50)  Indicador de traslado Total DAM o DS
        public string nro_contenedor_01 { get; set; }    // an(11)  Número de contenedor 1
        public string nro_precinto_01 { get; set; }     // an(20)   Número de precinto 1
        public string nro_contenedor_02 { get; set; }   // an(11)   Número de contenedor 2
        public string nro_precinto_02 { get; set; }     // an(20)   Número de precinto 2
        // DATOS ENVIO - GUIA REMISION TRANSPORTISTA
        public string ind_traslado_tot { get; set; }    // an(50)   Indicador de traslado total de bienes
        public string ind_retorno_enva_vacio { get; set; }  // an(50)   Indicador de retorno de vehículo con envases o embalajes vacíos
        public string ind_retorno_vehi_vacio { get; set; }  // an(50)   Indicador de retorno de vehículo vacío
        public string ind_transp_subcontra { get; set; }    // an(50)   Indicador de transporte subcontratado
        public string ind_paga_flete { get; set; }          // an(50)   Indicador de pagador de flete
        public string anotacion { get; set; }           // an(500)  Anotación opcional sobre los bienes a transportar
    }       // guía de remision
    public class Cproveedor
    {
        public string num_doc { get; set; }     // an(11)   Numero de documento de identidad del proveedor
        public string tip_doc { get; set; }     // an(2)    Tipo de documento de identidad del proveedor
        public string raz_soc { get; set; }     // an(200)  Apellidos y nombres, denominacion o razon social del proveedor
    }   // DATOS PROVEEDOR - GUIA REMISION
    public class Cdetracc
    {
        public decimal porcent { get; set; }    // n(15,2)  % de la Detracción
        public string cod { get; set; }         // an(3)    Código del Bien o Servicio sujeto a Detracion
        public decimal monto { get; set; }      // n(15,2)  Monto de la Detraccion
        public string cod_bn { get; set; }      // an(20)   Nro. Cuenta del banco de la Nacion de la Detracción
        public string med_pago { get; set; }    // an(3)    Medio de pago
        public string cod_mon { get; set; }     // an(3)    Moneda de la detraccion
    }
    public class Ctransportista
    {
        public string num_doc { get; set; }     // an(11)   Numero de RUC transportista
        public string tip_doc { get; set; }     // an(2)    Tipo de documento del transportista
        public string raz_soc { get; set; }     // an(250)  Apellidos y Nombres o denominacion o razon social del transportista
        public string num_mtc { get; set; }     // an(20)   Número de Registro MTC
        public string num_autorizacion { get; set; }     // an(50)   Número de autorización especial emitido por la entidad – transportista
        public string cod_autorizacion { get; set; }     // an(2)   Código de entidad autorizadora
    }
    public class Cvehiculo
    {
        public string vehi_placa { get; set; }      // an(8)    Numero de placa del vehiculo
        public string cert_vehi { get; set; }       // an(15)   Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
        public string num_autorizacion { get; set; }    // an(50)   Número de autorización especial del vehículo emitido por la entidad - vehículo principal
        public string cod_autorizacion { get; set; }    // an(2)    Código de entidad emisora
        public string tip_vehiculo { get; set; }        // an(9)    Tipo de Vehiculo
    }
    public class Ctot
    {
        public decimal grav { get; set; }           // n(12,2)  (18).Total   valor   de   venta   - operaciones gravadas
        //public decimal inaf { get; set; }           // n(12,2) "(19).Total   valor   de   venta   - operaciones inafectas"
        //public decimal exo { get; set; }            // n(12,2)  "(20).Total   valor   de   venta  - operaciones exoneradas"
        //public decimal grat { get; set; }           // n(15,2)  (49).Total Valor de Venta - Operaciones gratuitas
        //public decimal sub_tot { get; set; }        // n(12,2)  Son solo de referencia, no se toma en cuenta para el XML
        public decimal dsc_item { get; set; }       // n(12,2) Son solo de referencia, no se toma en cuenta para el XML
        public decimal val_vent { get; set; }       // n(12,2) Son solo de referencia, no se toma en cuenta para el XML
        public decimal igv { get; set; }            // n(12,2)  (22).Sumatoria IGV
        public decimal isc { get; set; }            // n(12,2)  (23).Sumatoria ISC
        public decimal imp_tot { get; set; }        // n(12,2)  (27).Importe total de la venta, cesión  en  uso  o  del servicio prestado
        public decimal otr_carg { get; set; }       // n(12,2) (25).Sumatoria otros Cargos
        public decimal otr_trib { get; set; }       // n(12,2) (24).Sumatoria otros tributos
        public decimal dsct_tot { get; set; }       // n(12,2) (26).Total descuentos
        public decimal impsto_tot { get; set; }     // n(12,2)   sumatoria de impuestos (Códigos 1000+1016+2000+9999)
        //public decimal exp { get; set; }            // n(12,2)  Importe Operaciones Exportacion
        //public decimal trib_exp { get; set; }       // n(12,2) Tributos Operaciones de Exportacion
        //public decimal trib_inaf { get; set; }      // n(12,2)    Tributos Operaciones Inafectas
        //public decimal trib_exo { get; set; }       // n(12,2) Tributos Operaciones Exoneradas
        //public decimal trib_grat { get; set; }      // n(12,2)    Tributos Operaciones gratuitas
        //public decimal grav_ivap { get; set; }      // n(12,2)    Importe Operaciones Gravadas ivap
        //public decimal ivap { get; set; }           // n(12,2)  Tributos gravadas IVAP (impuesto 1ra venta arroz pilado)
        //public decimal base_isc { get; set; }       // n(12,2) Importe Operaciones ISC
        //public decimal base_otr_trib { get; set; }  // n(12,2)    Importe Operaciones Otros Tributos
        public decimal prec_tot { get; set; }       // n(12,2) Total Precio de Venta
        public decimal redondeo { get; set; }       // n(12,2) Monto para Redondeo del Importe Total
        //public decimal icbper { get; set; }         // n(12,2)   (48).Sumatoria ICBPER
    }        // Totales
    public class Cforma_pago
    {
        public string descrip { get; set; }     // an(7) ("Contado"/"Credito") Forma de pago Contado/Credito, Solo en facturas/NC
        public decimal monto_neto { get; set; } // n(12,2)  Monto neto pendiente de pago
        public string cod_mon { get; set; }     // an(3)    Tipo de moneda del monto pendiente de pago
    }
    public class CCuota
    {
        public string descrip { get; set; }         // an(8)
        public decimal monto_neto { get; set; }     // n(12,2)
        public string cod_mon { get; set; }         // an(3)
        public string fec_venc { get; set; }        // an(10)
    }
    public class Cleyen
    {
        public string leyen_cod { get; set; }  // n(4) Codigo Leyenda
        public string leyen_descrip { get; set; }   // an(250)  Descripcion de Leyenda
    }

}
