﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using com.tuscomprobantespe.webservice;

namespace TransCarga
{
    public partial class facelect : Form
    {
        static string nomform = "facelect";             // nombre del formulario
        string colback = TransCarga.Program.colbac;   // color de fondo
        string colpage = TransCarga.Program.colpag;   // color de los pageframes
        string colgrid = TransCarga.Program.colgri;   // color de las grillas
        string colfogr = TransCarga.Program.colfog;   // color fondo con grillas
        string colsfon = TransCarga.Program.colsbg;   // color fondo seleccion
        string colsfgr = TransCarga.Program.colsfc;   // color seleccion grilla
        string colstrp = TransCarga.Program.colstr;   // color del strip
        bool conectS = TransCarga.Program.vg_conSol;    // usa conector solorsoft? true=si; false=no
        static string nomtab = "cabfactu";              // cabecera de guias INDIVIDUALES

        #region variables
        string img_btN = "";
        string img_btE = "";
        string img_btA = "";            // anula = bloquea
        string img_btP = "";            // imprime
        string img_btV = "";            // visualiza
        string img_bti = "";            // imagen boton inicio
        string img_bts = "";            // imagen boton siguiente
        string img_btr = "";            // imagen boton regresa
        string img_btf = "";            // imagen boton final
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string img_ver = "";
        string vtc_dni = "";            // variable tipo cliente natural
        string vtc_ruc = "";            // variable tipo cliente empresa
        string vtc_ext = "";            // variable tipo cliente extranjero
        string codAnul = "";            // codigo de documento anulado
        string codGene = "";            // codigo documento nuevo generado
        string codCanc = "";            // codigo documento cancelado (pagado 100%)
        string MonDeft = "";            // moneda por defecto
        string v_clu = "";              // codigo del local del usuario
        string v_slu = "";              // serie del local del usuario
        string v_nbu = "";              // nombre del usuario
        string vi_formato = "";         // formato de impresion del documento
        string vi_copias = "";          // cant copias impresion
        //string v_impA5 = "";            // nombre de la impresora matricial
        string v_impTK = "";            // nombre de la ticketera
        //string v_cid = "";              // codigo interno de tipo de documento
        string v_fra2 = "";             // frase que va en obs de cobranza cuando se cancela desde el doc.vta.
        string v_sanu = "";             // serie anulacion interna ANU
        string v_mpag = "";             // medio de pago automatico x defecto para las cobranzas
        string v_codcob = "";           // codigo del documento cobranza
        string v_CR_gr_ind = "";        // nombre del formato FT/BV en CR
        string v_mfildet = "";          // maximo numero de filas en el detalle, coord. con el formato
        string vint_A0 = "";            // variable codigo anulacion interna por BD
        string v_codidv = "";           // variable codifo interno de documento de venta en vista TDV
        string codfact = "";            // idcodice de factura
        string v_igv = "";              // valor igv %
        string v_estcaj = "";           // estado de la caja
        string v_idcaj = "";            // id de la caja actual
        string codAbie = "";            // codigo estado de caja abierta
        string logoclt = "";            // ruta y nombre archivo logo
        string fshoy = "";              // fecha hoy del servidor en formato ansi
        string codppc = "";             // codigo del plazo de pago por defecto para fact a crédito
        string codcont = "";            // codigo plazo contraentrega o efectivo no credito
        string codsuser_cu = "";        // usuarios autorizados a crear Ft de cargas unicas
        int v_cdpa = 0;                 // cantidad de días despues de emitida la fact. en que un usuario normal puede anular
        //
        string usuaInteg = "";          // usuario de la integración con SeenCorp
        string clavInteg = "";          // clave de la integración con SeenCorp
        string rutatxt = "";            // ruta de los txt para la fact. electronica
        string tipdo = "";              // CODIGO SUNAT tipo de documento de venta
        string tipoDocEmi = "";         // CODIGO SUNAT tipo de documento RUC/DNI
        string tipoMoneda = "";         // CODIGO SUNAT tipo de moneda
        string glosdet = "";            // glosa para las operaciones con detraccion
        string glosser = "";            // glosa que va en el detalle del doc. de venta
        string glosser2 = "";           // glosa 2 que va despues de la glosa principal
        string restexto = "xxx";        // texto resolucion sunat autorizando prov. fact electronica
        string autoriz_OSE_PSE = "yyy"; // numero resolucion sunat autorizando prov. fact electronica
        string despedida = "";          // texto para mensajes al cliente al final de la impresión del doc.vta. 
        string webose = "";             // direccion web del ose o pse para la descarga del 
        string correo_gen = "";         // correo generico del emisor cuando el cliente no tiene correo propio
        string codusanu = "";           // usuarios que pueden anular fuera de plazo
        string cusdscto = "";           // usuarios que pueden hacer descuentos
        string usercfece = "";          // usuarios que pueden cambiar fecha de emision
        string otro = "";               // ruta y nombre del png código QR
        string caractNo = "";           // caracter prohibido en campos texto, caracter delimitador para los TXT
        string nipfe = "";              // nombre identificador del proveedor de fact electronica
        string glosaAnul = "";          // texto motivo de baja/anulacion en los TXT para el pse/ose
        string tipdocAnu = "";          // Tipos de documentos que se pueden dar de baja
        string tdocsBol = "";           // tipos de documentos de clientes que permiten boletas
        string tdocsFac = "";           // tipos de documentos de clientes que permiten facturas
        string NoRetGl = "";            // 
        string webdni = "";             // 
        //
        string gloDeta = "";            // glosa detalle de las guias de remision
        string rutaQR = "";             // ruta donde se trabajan los QR -> "C:\temp\"
        string nomImgQR = "";           // nombre general de los QR -> "imgQR.png"
        string v_iniGRET = "";          // sigla, inicicla, marca de las GRE-T
        // variables sunat
        string s_tipOpeN = "";          // tipo de operacion venta interna sin detracción
        string s_tipOpeDG = "";         // tipo de operacion venta interna sujeta a detracción general
        string s_tipOpeDTC = "";        // tipo de operacion venta interna sujeta a detracción en TRANSPORTE DE CARGA
        //
        static libreria lib = new libreria();   // libreria de procedimientos
        publico lp = new publico();             // libreria de clases
        string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
        string nomclie = Program.cliente;           // cliente usuario del sistema
        string rucclie = Program.ruc;               // ruc del cliente usuario del sistema
        string ubiclie = Program.ubidirfis;         // ubigeo direc fiscal
        string asd = TransCarga.Program.vg_user;    // usuario conectado al sistema
        string dirloc = TransCarga.Program.vg_duse; // direccion completa del local usuario conectado
        string ubiloc = TransCarga.Program.vg_uuse; // ubigeo local del usuario conectado
        #endregion
        
        AutoCompleteStringCollection departamentos = new AutoCompleteStringCollection();// autocompletado departamentos
        AutoCompleteStringCollection provincias = new AutoCompleteStringCollection();   // autocompletado provincias
        AutoCompleteStringCollection distritos = new AutoCompleteStringCollection();    // autocompletado distritos
        DataTable dataUbig = (DataTable)CacheManager.GetItem("ubigeos");

        // string de conexion
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + login.data + ";";
        
        DataTable dttd0 = new DataTable();
        DataTable dttd1 = new DataTable();
        DataTable dtm = new DataTable();        // moneda
        DataTable dtp = new DataTable();        // plazo de credito 
        DataTable tcfe = new DataTable();       // facturacion electronica - cabecera
        DataTable tdfe = new DataTable();       // facturacion electronica -detalle

        string[,] datcltsR = new string[9, 9];
        string[,] datcltsD = new string[9, 9];
        string[,] datguias = new string[9, 22];

        string[] vs = {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",      // 20
                           "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};     // 20
        string[] va = { "", "", "", "", "", "", "", "", "", "" };      // 10
        string[,] dt = new string[10, 9] {
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" },
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }
                }; // 6 columnas, 10 filas
        string[] cu = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };    // 17

        public facelect()
        {
            InitializeComponent();
        }
        private void facelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        private void facelect_Load(object sender, EventArgs e)
        {
            /*
            ToolTip toolTipNombre = new ToolTip();           // Create the ToolTip and associate with the Form container.
            // Set up the delays for the ToolTip.
            toolTipNombre.AutoPopDelay = 5000;
            toolTipNombre.InitialDelay = 1000;
            toolTipNombre.ReshowDelay = 500;
            toolTipNombre.ShowAlways = true;                 // Force the ToolTip text to be displayed whether or not the form is active.
            toolTipNombre.SetToolTip(toolStrip1, nomform);   // Set up the ToolTip text for the object
            */
            this.Focus();
            jalainfo();
            init();
            dataload();
            toolboton();
            this.KeyPreview = true;
            autodepa();                                     // autocompleta departamentos
            armacfe();
            armadfe();
            if (valiVars() == false)
            {
                Application.Exit();
                return;
            }
        }
        private void init()
        {
            this.BackColor = Color.FromName(colback);
            toolStrip1.BackColor = Color.FromName(colstrp);
            dataGridView1.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            //dataGridView1.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            //dataGridView1.DefaultCellStyle.ForeColor = Color.FromName(colfogr);
            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromName(colsfon);
            //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromName(colsfgr);
            //
            tx_user.Text += asd;
            tx_nomuser.Text = TransCarga.Program.vg_nuse;   // lib.nomuser(asd);
            //tx_locuser.Text = TransCarga.Program.vg_luse;  // lib.locuser(asd);
            tx_locuser.Text = tx_locuser.Text + " " + TransCarga.Program.vg_nlus;
            tx_fechact.Text = DateTime.Today.ToString();
            //
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_btA);
            Bt_ver.Image = Image.FromFile(img_btV);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_close.Image = Image.FromFile(img_btq);
            Bt_ini.Image = Image.FromFile(img_bti);
            Bt_sig.Image = Image.FromFile(img_bts);
            Bt_ret.Image = Image.FromFile(img_btr);
            Bt_fin.Image = Image.FromFile(img_btf);
            // autocompletados
            tx_dptoRtt.AutoCompleteMode = AutoCompleteMode.Suggest;           // departamentos
            tx_dptoRtt.AutoCompleteSource = AutoCompleteSource.CustomSource;  // departamentos
            tx_dptoRtt.AutoCompleteCustomSource = departamentos;              // departamentos
            tx_provRtt.AutoCompleteMode = AutoCompleteMode.Suggest;           // provincias
            tx_provRtt.AutoCompleteSource = AutoCompleteSource.CustomSource;  // provincias
            tx_provRtt.AutoCompleteCustomSource = provincias;                 // provincias
            tx_distRtt.AutoCompleteMode = AutoCompleteMode.Suggest;           // distritos
            tx_distRtt.AutoCompleteSource = AutoCompleteSource.CustomSource;  // distritos
            tx_distRtt.AutoCompleteCustomSource = distritos;                  // distritos
            // longitudes maximas de campos
            tx_serie.MaxLength = 4;         // serie doc vta
            tx_numero.MaxLength = 8;        // numero doc vta
            tx_serGR.MaxLength = 4;         // serie guia
            tx_serGR.CharacterCasing = CharacterCasing.Upper;
            tx_numGR.MaxLength = 8;         // numero guia
            tx_numDocRem.MaxLength = 11;    // ruc o dni cliente
            tx_dirRem.MaxLength = 100;
            tx_nomRem.MaxLength = 100;           // nombre remitente
            tx_distRtt.MaxLength = 25;
            tx_provRtt.MaxLength = 25;
            tx_dptoRtt.MaxLength = 25;
            tx_obser1.MaxLength = 150;
            tx_telc1.MaxLength = 12;
            tx_telc2.MaxLength = 12;
            tx_fletLetras.MaxLength = 249;
            tx_dat_dpo.MaxLength = 100;
            tx_dat_dpd.MaxLength = 100;
            tx_pla_placa.MaxLength = 7;
            tx_pla_confv.MaxLength = 15;
            tx_pla_autor.MaxLength = 15;
            // grilla
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // todo desabilidado
            sololee();
            iniMatris();
        }
        private void initIngreso()
        {
            string[] vs = {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",      // 20
                           "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};     // 20
            string[] va = { "", "", "", "", "", "", "", "", "", "" };      // 10
            string[,] dt = new string[10, 9] {
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" },
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }
                }; // 6 columnas, 10 filas
            string[] cu = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };    // 17

            iniMatris();
            limpiar();
            limpia_chk();
            limpia_otros();
            limpia_combos();
            cmb_tdv.SelectedIndex = -1;
            dataGridView1.Rows.Clear();
            dataGridView1.ReadOnly = true;
            tx_igv.Text = "";
            tx_subt.Text = "";
            tx_flete.Text = "";
            tx_pagado.Text = "";
            tx_salxcob.Text = "";
            tx_numero.Text = "";
            tx_serie.Text = v_slu;
            tx_numero.ReadOnly = true;
            tx_dat_mone.Text = MonDeft;
            cmb_mon.SelectedValue = tx_dat_mone.Text;
            tx_fechope.Text = DateTime.Today.ToString("dd/MM/yyyy");
            tx_digit.Text = v_nbu;
            tx_dat_estad.Text = codGene;
            tx_estado.Text = lib.nomstat(tx_dat_estad.Text);
            tx_idcaja.ReadOnly = true;
            tx_idcaja.Text = "";
            tx_fletLetras.ReadOnly = true;
            if (Tx_modo.Text == "NUEVO")
            {
                rb_contado.Enabled = true;
                rb_contado.Checked = false;
                rb_credito.Enabled = true;
                rb_credito.Checked = false;
                rb_si.Enabled = true;
                rb_no.Enabled = true;
                if (codsuser_cu.Contains(asd)) chk_cunica.Enabled = true;
                else chk_cunica.Enabled = false;
                if (cusdscto.Contains(asd)) tx_flete.ReadOnly = false;
                else tx_flete.ReadOnly = true;
                chk_iGRE.Checked = false;
                tx_dat_sun_autor.Text = autoriz_OSE_PSE;        // autorizacion sunat del pse
                tx_dat_sun_web.Text = webose;                   // web de consulta de comprobante del pse
                if (v_estcaj == codAbie)      // caja esta abierta?
                {
                    if (fshoy != TransCarga.Program.vg_fcaj)  // fecha de la caja vs fecha de hoy ..... me quede aca, este dato debe limpiarse al cerrar la caja
                    {
                        MessageBox.Show("Las fechas no coinciden" + Environment.NewLine +
                            "Fecha de caja vs Fecha actual", "Caja fuera de fecha", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        //return;
                    }
                    else
                    {
                        tx_idcaja.Text = v_idcaj;
                    }
                }
            }
            if (Tx_modo.Text == "EDITAR")
            {
                fshoy = tx_fechope.Text;
            }
            if ("NUEVO,EDITAR".Contains(Tx_modo.Text) && usercfece.ToUpper().Contains(asd.ToUpper())) { tx_fechope.Enabled = true; tx_fechope.ReadOnly = false; }
            cargaunica(0);
        }
        private void jalainfo()                 // obtiene datos de imagenes y variables
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in (@nofo,@nfin,@nofa,@nofi,@noca,@noco,@ngre)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@nfin", "interno");
                micon.Parameters.AddWithValue("@nofi", "clients");
                micon.Parameters.AddWithValue("@noco", "cobranzas");
                micon.Parameters.AddWithValue("@noca", "ayccaja");
                micon.Parameters.AddWithValue("@nofa", nomform);
                micon.Parameters.AddWithValue("@ngre", "guiati_e");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["formulario"].ToString() == "main")
                    {
                        if (row["campo"].ToString() == "imagenes")
                        {
                            if (row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                            if (row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                            if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                            if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                            if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                            if (row["param"].ToString() == "img_btV") img_btV = row["valor"].ToString().Trim();         // imagen del boton de accion visualizar
                            if (row["param"].ToString() == "img_bti") img_bti = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL INICIO
                            if (row["param"].ToString() == "img_bts") img_bts = row["valor"].ToString().Trim();         // imagen del boton de accion SIGUIENTE
                            if (row["param"].ToString() == "img_btr") img_btr = row["valor"].ToString().Trim();         // imagen del boton de accion RETROCEDE
                            if (row["param"].ToString() == "img_btf") img_btf = row["valor"].ToString().Trim();         // imagen del boton de accion IR AL FINAL
                            if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                            if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                            if (row["param"].ToString() == "img_preview") img_ver = row["valor"].ToString().Trim();      // imagen del boton grabar visualizar
                            if (row["param"].ToString() == "logoPrin") logoclt = row["valor"].ToString().Trim();         // logo emisor
                        }
                        if (row["campo"].ToString() == "estado")
                        {
                            if (row["param"].ToString() == "anulado") codAnul = row["valor"].ToString().Trim();         // codigo doc anulado
                            if (row["param"].ToString() == "generado") codGene = row["valor"].ToString().Trim();        // codigo doc generado
                            if (row["param"].ToString() == "cancelado") codCanc = row["valor"].ToString().Trim();        // codigo doc cancelado
                        }
                        if (row["campo"].ToString() == "rutas")
                        {
                            if (row["param"].ToString() == "fe_txt") rutatxt = row["valor"].ToString().Trim();          // ruta de los txt para la fact. electronica
                            if (row["param"].ToString() == "web_dni") webdni = row["valor"].ToString().Trim();          // pag web para busqueda de dni
                        }
                        if (row["campo"].ToString() == "conector")
                        {
                            if (row["param"].ToString() == "noRetGlosa") NoRetGl = row["valor"].ToString().Trim();          // glosa que retorna umasapa cuando no encuentra dato
                        }
                    }
                    if (row["formulario"].ToString() == "clients" && row["campo"].ToString() == "documento")
                    {
                        if (row["param"].ToString() == "dni") vtc_dni = row["valor"].ToString().Trim();
                        if (row["param"].ToString() == "ruc") vtc_ruc = row["valor"].ToString().Trim();
                        if (row["param"].ToString() == "ext") vtc_ext = row["valor"].ToString().Trim();
                    }
                    if (row["formulario"].ToString() == "cobranzas" && row["campo"].ToString() == "documento")
                    {
                        if (row["param"].ToString() == "codigo") v_codcob = row["valor"].ToString().Trim();
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "documento")
                        {
                            if (row["param"].ToString() == "frase2") v_fra2 = row["valor"].ToString().Trim();               // frase cuando se cancela el doc.vta.
                            if (row["param"].ToString() == "serieAnu") v_sanu = row["valor"].ToString().Trim();               // serie anulacion interna
                            if (row["param"].ToString() == "mpagdef") v_mpag = row["valor"].ToString().Trim();               // medio de pago x defecto para cobranzas
                            if (row["param"].ToString() == "factura") codfact = row["valor"].ToString().Trim();               // codigo doc.venta factura
                            if (row["param"].ToString() == "plazocred") codppc = row["valor"].ToString().Trim();               // codigo plazo de pago x defecto para fact. a CREDITO
                            if (row["param"].ToString() == "plzoCont") codcont = row["valor"].ToString().Trim();               // codigo de plazo contado o efectivo o contraentrega
                            if (row["param"].ToString() == "usercar_unic") codsuser_cu = row["valor"].ToString().Trim();       // usuarios autorizados a crear Ft de cargas unicas
                            if (row["param"].ToString() == "diasanul") v_cdpa = int.Parse(row["valor"].ToString());            // cant dias en que usuario normal puede anular 
                            if (row["param"].ToString() == "useranul") codusanu = row["valor"].ToString();                      // usuarios autorizados a anular fuera de plazo 
                            if (row["param"].ToString() == "userdscto") cusdscto = row["valor"].ToString();                 // usuarios que pueden hacer descuentos
                            if (row["param"].ToString() == "usercfece") usercfece = row["valor"].ToString();                 // usuarios que pueden cambiar fecha de emision
                            if (row["param"].ToString() == "cltesBol") tdocsBol = row["valor"].ToString();                  // tipos de documento de clientes para boletas
                            if (row["param"].ToString() == "cltesFac") tdocsFac = row["valor"].ToString();                  // tipos de documento de clientes para facturas

                        }
                        if (row["campo"].ToString() == "impresion")
                        {
                            if (row["param"].ToString() == "formato") vi_formato = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "filasDet") v_mfildet = row["valor"].ToString().Trim();       // maxima cant de filas de detalle
                            if (row["param"].ToString() == "copias") vi_copias = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "impTK") v_impTK = row["valor"].ToString().Trim();
                            //if (row["param"].ToString() == "nomfor_cr") v_CR_gr_ind = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "forA4CRn") v_CR_gr_ind = row["valor"].ToString().Trim();           // ruta y nombre del formato CR de factura/boletas "normales"
                        }
                        if (row["campo"].ToString() == "moneda" && row["param"].ToString() == "default") MonDeft = row["valor"].ToString().Trim();      // moneda por defecto
                        if (row["campo"].ToString() == "detraccion" && row["param"].ToString() == "glosa") glosdet = row["valor"].ToString().Trim();    // glosa detraccion
                        if (row["campo"].ToString() == "factelect")
                        {
                            if (row["param"].ToString() == "textaut") restexto = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "autoriz") autoriz_OSE_PSE = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "despedi") despedida = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "webose") webose = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "correo_c1") correo_gen = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "caracterNo") caractNo = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "ose-pse") nipfe = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "motivoBaja") glosaAnul = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "tipsDocbaja") tipdocAnu = row["valor"].ToString().Trim();
                            if (row["param"].ToString() == "usuarioInteg") usuaInteg = row["valor"].ToString().Trim();     // usuario de la integración con Seencorp
                            if (row["param"].ToString() == "claveInteg") clavInteg = row["valor"].ToString().Trim();        // clave del usuario de la integración con Seencorp
                        }
                        if (row["campo"].ToString() == "sunat")
                        {
                            if (row["param"].ToString() == "vtaInterna") s_tipOpeN = row["valor"].ToString().Trim();           // tipo de operacion venta interna sin detracción
                            if (row["param"].ToString() == "operDetGene") s_tipOpeDG = row["valor"].ToString().Trim();         // tipo de operacion venta interna sujeta a detracción general
                            if (row["param"].ToString() == "operDetCarga") s_tipOpeDTC = row["valor"].ToString().Trim();       // tipo de operacion venta interna sujeta a detracción en TRANSPORTE DE CARGA
                        }
                    }
                    if (row["formulario"].ToString() == "ayccaja" && row["campo"].ToString() == "estado")
                    {
                        if (row["param"].ToString() == "abierto") codAbie = row["valor"].ToString().Trim();             // codigo caja abierta
                        //if (row["param"].ToString() == "cerrado") codCier = row["valor"].ToString().Trim();             // codigo caja cerrada
                    }
                    if (row["formulario"].ToString() == "interno")              // codigo enlace interno de anulacion del cliente con en BD A0
                    {
                        if (row["campo"].ToString() == "anulado" && row["param"].ToString() == "A0") vint_A0 = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "codinDV" && row["param"].ToString() == "DV") v_codidv = row["valor"].ToString().Trim();           // codigo de dov.vta en tabla TDV
                        if (row["campo"].ToString() == "igv" && row["param"].ToString() == "%") v_igv = row["valor"].ToString().Trim();
                    }
                    if (row["formulario"].ToString() == "guiati_e")
                    {
                        if (row["campo"].ToString() == "detalle" && row["param"].ToString() == "glosa") gloDeta = row["valor"].ToString().Trim();             // glosa del detalle
                        if (row["campo"].ToString() == "impresion")
                        {
                            if (row["param"].ToString() == "rutaQR") rutaQR = row["valor"].ToString().Trim();           // "C:\temp\"
                            if (row["param"].ToString() == "nomImgQR") nomImgQR = row["valor"].ToString().Trim();       // "imgQR.png"
                        }
                        if (row["campo"].ToString() == "documento")
                        {
                            if (row["param"].ToString() == "ini_GRET") v_iniGRET = row["valor"].ToString().Trim();          // inicial (sigla) de las GRE-T
                        }
                    }
                }
                da.Dispose();
                dt.Dispose();
                // jalamos datos del usuario y local
                v_clu = TransCarga.Program.vg_luse;                // codigo local usuario
                v_slu = lib.serlocs(v_clu);                        // serie local usuario
                v_nbu = TransCarga.Program.vg_nuse;                // nombre del usuario
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        private void jalaoc(string campo)        // jala doc venta
        {
            //try
            {
                string parte = "";
                if (campo == "tx_idr")
                {
                    parte = "where a.id=@ida";
                }
                if (campo == "sernum")
                {
                    parte = "where a.tipdvta=@tdv and a.serdvta=@ser and a.numdvta=@num";
                }
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open) // tippago,plazocred  
                {
                    string consulta = "select a.id,a.fechope,a.martdve,a.tipdvta,a.serdvta,a.numdvta,a.ticltgr,a.tidoclt,a.nudoclt,a.nombclt,a.direclt,a.dptoclt,a.provclt,a.distclt,a.ubigclt,a.corrclt,a.teleclt," +
                        "a.locorig,a.dirorig,a.ubiorig,a.obsdvta,a.canfidt,a.canbudt,a.mondvta,a.tcadvta,a.subtota,a.igvtota,a.porcigv,a.totdvta,a.totpags,a.saldvta,a.estdvta,a.frase01,a.impreso," +
                        "a.tipoclt,a.m1clien,a.tippago,a.ferecep,a.userc,a.fechc,a.userm,a.fechm,b.descrizionerid as nomest,ifnull(c.id,'') as cobra,a.idcaja,a.plazocred," +
                        "a.cargaunica,a.placa,a.confveh,a.autoriz,a.detPeso,a.detputil,a.detMon1,a.detMon2,a.detMon3,a.dirporig,a.ubiporig,a.dirpdest,a.ubipdest,a.porcendscto,a.valordscto,a.totdvMN," +
                        "ifnull(ad.ose_pse,'') as ose_pse,ifnull(ad.autoriz,'') as autorizPSE,ifnull(ad.webose,'') as webosePSE " +
                        "from cabfactu a left join desc_est b on b.idcodice=a.estdvta " +
                        "left join adifactu ad on ad.idc=a.id " +
                        "left join cabcobran c on c.tipdoco=a.tipdvta and c.serdoco=a.serdvta and c.numdoco=a.numdvta and c.estdcob<>@coda "
                        + parte;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    micon.Parameters.AddWithValue("@tdep", vtc_ruc);
                    micon.Parameters.AddWithValue("@coda", codAnul);
                    if (campo == "tx_idr")
                    {
                        micon.Parameters.AddWithValue("@ida", tx_idr.Text);
                    }
                    if (campo == "sernum")
                    {
                        micon.Parameters.AddWithValue("@tdv", tx_dat_tdv.Text);
                        micon.Parameters.AddWithValue("@ser", tx_serie.Text);
                        micon.Parameters.AddWithValue("@num", tx_numero.Text);
                    }
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr != null)
                    {
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString("id");
                            tx_idcaja.Text = dr.GetString("idcaja");
                            tx_fechope.Text = dr.GetString("fechope").Substring(0, 10);
                            //.Text = dr.GetString("martdve");
                            tx_dat_tdv.Text = dr.GetString("tipdvta");
                            tx_serie.Text = dr.GetString("serdvta");
                            tx_numero.Text = dr.GetString("numdvta");
                            rb_remGR.Checked = (dr.GetString("ticltgr") == "1")? true : false;
                            rb_desGR.Checked = (dr.GetString("ticltgr") == "2") ? true : false;
                            rb_otro.Checked = (dr.GetString("ticltgr") == "3") ? true : false;
                            tx_dat_tdRem.Text = dr.GetString("tidoclt");
                            tx_numDocRem.Text = dr.GetString("nudoclt");
                            tx_nomRem.Text = dr.GetString("nombclt");
                            tx_dirRem.Text = dr.GetString("direclt");
                            tx_dptoRtt.Text = dr.GetString("dptoclt");
                            tx_provRtt.Text = dr.GetString("provclt");
                            tx_distRtt.Text = dr.GetString("distclt");
                            tx_ubigRtt.Text = dr.GetString("ubigclt");
                            tx_email.Text = dr.GetString("corrclt");
                            tx_telc1.Text = dr.GetString("teleclt");
                            //locorig,dirorig,ubiorig
                            tx_obser1.Text = dr.GetString("obsdvta");
                            tx_tfil.Text = dr.GetString("canfidt");
                            tx_totcant.Text = dr.GetString("canbudt");  // total bultos
                            tx_dat_mone.Text = dr.GetString("mondvta");
                            tx_tipcam.Text = dr.GetString("tcadvta");
                            tx_subt.Text = Math.Round(dr.GetDecimal("subtota"),2).ToString();
                            tx_igv.Text = Math.Round(dr.GetDecimal("igvtota"), 2).ToString();
                            //,,,porcigv
                            tx_flete.Text = Math.Round(dr.GetDecimal("totdvta"),2).ToString();           // total inc. igv
                            tx_pagado.Text = dr.GetString("totpags");
                            tx_salxcob.Text = Math.Round(dr.GetDecimal("saldvta"), 2).ToString();    // dr.GetString("saldvta"
                            tx_fletMN.Text = dr.GetString("totdvMN");
                            tx_dat_estad.Text = dr.GetString("estdvta");        // estado
                            tx_dat_tcr.Text = dr.GetString("tipoclt");          // tipo de cliente credito o contado
                            tx_dat_m1clte.Text = dr.GetString("m1clien");
                            tx_impreso.Text = dr.GetString("impreso");
                            tx_idcob.Text = dr.GetString("cobra");              // id de cobranza
                            //
                            cmb_tdv.SelectedValue = tx_dat_tdv.Text;
                            cmb_tdv_SelectedIndexChanged(null, null);
                            tx_numero.Text = dr.GetString("numdvta");       // al cambiar el indice en el combox se borra numero, por eso lo volvemos a jalar
                            cmb_docRem.SelectedValue = tx_dat_tdRem.Text;
                            cmb_mon.SelectedValue = tx_dat_mone.Text;
                            tx_estado.Text = dr.GetString("nomest");   // lib.nomstat(tx_dat_estad.Text);
                            if (dr.GetString("userm") == "") tx_digit.Text = lib.nomuser(dr.GetString("userc"));
                            else tx_digit.Text = lib.nomuser(dr.GetString("userm"));
                            if (decimal.Parse(tx_salxcob.Text) == decimal.Parse(tx_flete.Text)) rb_no.Checked = true;
                            else rb_si.Checked = true;
                            // campos de carga unica
                            tx_dat_upd.Text = dr.GetString("ubipdest");
                            tx_dat_upo.Text = dr.GetString("ubiporig");
                            tx_dat_dpd.Text = dr.GetString("dirpdest");
                            tx_dat_dpo.Text = dr.GetString("dirporig");
                            tx_valref3.Text = dr.GetString("detMon3");
                            tx_valref2.Text = dr.GetString("detMon2");
                            tx_valref1.Text = dr.GetString("detMon1");
                            tx_cutm.Text = dr.GetString("detputil");
                            tx_cetm.Text = dr.GetString("detPeso");
                            tx_pla_autor.Text = dr.GetString("autoriz");
                            tx_pla_confv.Text = dr.GetString("confveh");
                            tx_pla_placa.Text = dr.GetString("placa");
                            if (dr.GetInt16("cargaunica") == 1) chk_cunica.Checked = true;
                            tx_valdscto.Text = dr.GetString("valordscto");
                            tx_dat_porcDscto.Text = dr.GetString("porcendscto");
                            tx_dat_plazo.Text = dr.GetString("plazocred");
                            tx_dat_sun_autor.Text = dr.GetString("autorizPSE");        // autorizacion sunat del pse
                            tx_dat_sun_web.Text = dr.GetString("webosePSE");           // web de consulta de comprobante del pse
                        }
                        else
                        {
                            MessageBox.Show("No existe el número del documento de venta!", "Atención - dato incorrecto",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            tx_numero.Text = "";
                            tx_numero.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No existe el número buscado!", "Atención - dato incorrecto",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    dr.Dispose();
                    micon.Dispose();
                    //
                    if (decimal.Parse(tx_valdscto.Text) > 0)
                    {
                        lin_dscto.Visible = true;
                        lb_dscto.Visible = true;
                        tx_valdscto.Visible = true;
                    }
                    else
                    {
                        lin_dscto.Visible = false;
                        lb_dscto.Visible = false;
                        tx_valdscto.Visible = false;
                    }
                    DataRow[] row = dtm.Select("idcodice='" + tx_dat_mone.Text + "'");
                    NumLetra nel = new NumLetra();
                    tx_fletLetras.Text = nel.Convertir(tx_flete.Text,true) + row[0][3].ToString().Trim();
                    //
                    if (tx_dat_plazo.Text.Trim() != "" && tx_dat_plazo.Text != codcont)    // osea que no seas contado -> osea es credito 
                    {
                        cmb_plazoc.SelectedValue = tx_dat_plazo.Text;
                    }
                }
                conn.Close();
            }
        }
        private void jaladet(string idr)         // jala el detalle
        {
            //string jalad = "SELECT a.filadet,a.codgror,a.cantbul,a.unimedp,a.descpro,a.pesogro,a.codmogr,a.totalgr," +
            //    "'' as unimedpro, '' as docsremit, '' as fechopegr,'' as orides " +
            //    "FROM detfactu a where a.idc = @idr";
            string jalad = "SELECT a.filadet,a.codgror,a.cantbul,a.unimedp,a.descpro,a.pesogro,a.codmogr,a.totalgr," +
                "'' as unimedpro, '' as docsremit, '' as fechopegr,'' as orides,m.idcodice " +
                "FROM detfactu a left join desc_mon m on trim(m.descrizionerid)=trim(a.codmogr) where a.idc = @idr";
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                using (MySqlCommand micon = new MySqlCommand(jalad, conn))
                {
                    micon.Parameters.AddWithValue("@idr", idr);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        // aca buscamos los datos detalle de la guía
                        for (int i = 0; i<dt.Rows.Count; i++)
                        {
                            string busdet = "SELECT z.id, z.docsremit, max(x.unimedpro) AS unimedpro, z.locorigen, z.locdestin, z.fechopegr, CONCAT(lo.descrizionerid,'-',ld.DescrizioneRid) AS orides " +
                                "FROM cabguiai z LEFT JOIN detguiai x ON z.id = x.idc " +
                                "left join desc_loc lo on lo.idcodice = z.locorigen " +
                                "left join desc_loc ld on ld.idcodice = z.locdestin " +
                                "WHERE concat(z.sergui,'-',z.numgui)=@grd";
                            using (MySqlCommand midet = new MySqlCommand(busdet, conn))
                            {
                                midet.Parameters.AddWithValue("@grd", dt.Rows[i].ItemArray[1].ToString());
                                using (MySqlDataReader dr = midet.ExecuteReader())
                                {
                                    if (dr.Read())
                                    {
                                        // actualizamos el dt con los datos encontrados
                                        dt.Rows[i][8] = dr.GetString("unimedpro");
                                        dt.Rows[i][9] = dr.GetString("docsremit");
                                        dt.Rows[i][10] = dr.GetString("fechopegr");
                                        dt.Rows[i][11] = dr.GetString("orides");
                                    }
                                }
                            }
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            dataGridView1.Rows.Add(
                                row[1].ToString(),
                                row[4].ToString(),
                                row[2].ToString(),
                                row[6].ToString(),
                                row[7].ToString(),
                                "",
                                "",
                                row[10].ToString().Substring(6, 4) + "-" + row[10].ToString().Substring(3, 2) + "-" + row[10].ToString().Substring(0, 2),
                                row[9].ToString(),
                                row[12].ToString(),
                                row[11].ToString(),
                                row[8].ToString()
                            );
                            jalaguia(conn, row[1].ToString().Substring(0, 4), row[1].ToString().Substring(5, 8), int.Parse(row[0].ToString())-1);
                        }
                        dt.Dispose();
                    }
                }
            }
        }
        public void dataload()                  // jala datos para los combos 
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                while (true)
                {
                    try
                    {
                        conn.Open();
                        break;
                    }
                    catch (MySqlException ex)
                    {
                        var aa = MessageBox.Show(ex.Message + Environment.NewLine + "No se pudo conectar con el servidor" + Environment.NewLine +
                            "Desea volver a intentarlo?", "Error de conexión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.No)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }
                // datos para el combobox documento de venta
                cmb_tdv.Items.Clear();
                string consu = "select distinct a.idcodice,a.descrizionerid,a.enlace1,a.codsunat,b.glosaser,b.serie " +
                    "from desc_tdv a LEFT JOIN series b ON b.tipdoc = a.IDCodice where a.numero=@bloq and a.codigo=@codv and b.sede=@loca";
                using (MySqlCommand cdv = new MySqlCommand(consu, conn))
                {
                    cdv.Parameters.AddWithValue("@bloq", 1);
                    cdv.Parameters.AddWithValue("@codv", v_codidv);
                    cdv.Parameters.AddWithValue("@loca", v_clu);
                    using (MySqlDataAdapter datv = new MySqlDataAdapter(cdv))
                    {
                        dttd1.Clear();
                        datv.Fill(dttd1);
                        cmb_tdv.DataSource = dttd1;
                        cmb_tdv.DisplayMember = "descrizionerid";
                        cmb_tdv.ValueMember = "idcodice";
                    }
                }
                //  datos para los combobox de tipo de documento
                cmb_docRem.Items.Clear();
                using (MySqlCommand cdu = new MySqlCommand("select idcodice,descrizionerid,codigo,codsunat from desc_doc where numero=@bloq", conn))
                {
                    cdu.Parameters.AddWithValue("@bloq", 1);
                    using (MySqlDataAdapter datd = new MySqlDataAdapter(cdu))
                    {
                        dttd0.Clear();
                        datd.Fill(dttd0);
                        cmb_docRem.DataSource = dttd0;
                        cmb_docRem.DisplayMember = "descrizionerid";
                        cmb_docRem.ValueMember = "idcodice";
                    }
                }
                // datos para el combo de moneda
                cmb_mon.Items.Clear();
                using (MySqlCommand cmo = new MySqlCommand("select idcodice,descrizionerid,codsunat,deta1 from desc_mon where numero=@bloq", conn))
                {
                    cmo.Parameters.AddWithValue("@bloq", 1);
                    using (MySqlDataAdapter dacu = new MySqlDataAdapter(cmo))
                    {
                        dtm.Clear();
                        dacu.Fill(dtm);
                        cmb_mon.DataSource = dtm;
                        cmb_mon.DisplayMember = "descrizionerid";
                        cmb_mon.ValueMember = "idcodice";
                    }
                }
                // datos del combo plazo de pago creditos
                using (MySqlCommand compla = new MySqlCommand("select idcodice,descrizionerid,codsunat,marca1 from desc_tpa where numero=@bloq", conn))
                {
                    compla.Parameters.AddWithValue("@bloq", 1);
                    using (MySqlDataAdapter dapla = new MySqlDataAdapter(compla))
                    {
                        dtp.Clear();
                        dapla.Fill(dtp);
                        cmb_plazoc.DataSource = dtp;
                        cmb_plazoc.DisplayMember = "descrizionerid";
                        cmb_plazoc.ValueMember = "idcodice";
                    }
                }
                // jalamos la caja
                using (MySqlCommand micon = new MySqlCommand("select id,fechope,statusc from cabccaja where loccaja=@luc order by id desc limit 1", conn))
                {
                    micon.Parameters.AddWithValue("@luc", v_clu);
                    using (MySqlDataReader dr = micon.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            v_estcaj = dr.GetString("statusc");
                            v_idcaj = dr.GetString("id");
                        }
                    }
                }
            }
        }
        private bool valiVars()                 // valida existencia de datos en variables del form
        {
            bool retorna = true;
            if (vtc_dni == "")           // variable tipo cliente natural
            {
                lib.messagebox("Tipo de cliente Natural");
                retorna = false;
            }
            if (vtc_ruc == "")          // variable tipo cliente empresa
            {
                lib.messagebox("Tipo de cliente Empresa");
                retorna = false;
            }
            if (vtc_ext == "")          // variable tipo cliente extranjero
            {
                lib.messagebox("Tipo de cliente Extranjero");
                retorna = false;
            }
            if (codAnul == "")          // codigo de documento anulado
            {
                lib.messagebox("Código de Doc.Venta ANULADA");
                retorna = false;
            }
            if (codGene == "")          // codigo documento nuevo generado
            {
                lib.messagebox("Código de Doc.Venta GENERADA/NUEVA");
                retorna = false;
            }
            if (MonDeft == "")          // moneda por defecto
            {
                lib.messagebox("Moneda por defecto");
                retorna = false;
            }
            if (v_slu == "")            // serie del local del usuario
            {
                lib.messagebox("Serie general local del usuario");
                retorna = false;
            }
            if (vi_formato == "")       // formato de impresion del documento
            {
                lib.messagebox("formato de impresion del Doc.Venta");
                retorna = false;
            }
            if (vi_copias == "")        // cant copias impresion
            {
                lib.messagebox("# copias impresas del Doc.Venta");
                retorna = false;
            }
            if (v_impTK == "")           // nombre de la ticketera
            {
                lib.messagebox("Nombre de impresora de Tickets");
                retorna = false;
            }
            if (v_sanu == "")           // serie de anulacion del documento
            {
                lib.messagebox("Serie de Anulación interna");
                retorna = false;
            }
            if (v_CR_gr_ind == "")
            {
                lib.messagebox("Nombre formato Doc.Venta en CR");
                retorna = false;
            }
            if (v_mfildet == "")
            {
                lib.messagebox("Max. filas de detalle");
                retorna = false;
            }
            if (vint_A0 == "")
            {
                lib.messagebox("Código interno enlace anulación BD - A0");
                retorna = false;
            }
            // aca falta agregar resto  ...........
            return retorna;
        }
        private bool validGR(string serie, string corre, int fila)    // validamos y devolvemos datos
        {
            bool retorna = false;
            if (serie != "" && corre != "")
            {
                // validamos que la GR: 1.exista, 2.No este facturada, 3.No este anulada
                // y devolvemos una fila con los datos del remitente y otra fila los datos del destinatario
                string hay = "no";
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    lib.procConn(conn);
                    string cons = "select fecguitra,totguitra,estadoser,fecdocvta,tipdocvta,serdocvta,numdocvta,codmonvta,totdocvta,saldofina " +
                        "from controlg where serguitra=@ser and numguitra=@num";
                    using (MySqlCommand mic1 = new MySqlCommand(cons, conn))
                    {
                        mic1.Parameters.AddWithValue("@ser", serie);
                        mic1.Parameters.AddWithValue("@num", corre);
                        using (MySqlDataReader dr = mic1.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    if (dr.GetString("numdocvta").Trim() != "") hay = "sif"; // si hay guía pero ya esta facturado
                                    else hay = "sin";    // si hay guía y no tiene factura
                                    if (dr.GetString("saldofina") != dr.GetString("totguitra") && dr.GetDecimal("saldofina") > 0)
                                    {
                                        MessageBox.Show("No esta permitido generar un documento" + Environment.NewLine + 
                                            "de venta de una guía que tiene pago parcial","Atención - no puede continuar");
                                        hay = "no";
                                    }
                                }
                            }
                            else
                            {
                                hay = "no"; // no existe la guía
                            }
                        }
                    }
                    if (hay == "sin")
                    {
                        if (jalaguia(conn, serie, corre, fila) == true) tx_dat_saldoGR.Text = datguias[fila,17]; // dr.GetString("salgri");
                        retorna = true;
                    }
                }
            }
            return retorna;
        }
        private void iniMatris()
        {
            for (int fila = 0; fila < 9; fila ++)
            {
                for (int col = 0; col < 9; col ++)
                {
                    datcltsR[fila, col] = "";
                    datcltsD[fila, col] = "";
                }
                for (int col = 0; col < 22; col++)
                {
                    datguias[fila, col] = "";
                }
            }
        }
        private bool jalaguia(MySqlConnection conn, string serGR, string numGR, int fila)
        {
            bool retorna = false;
            string parte = "";
            if (Tx_modo.Text == "NUEVO") parte = " AND c.fecdocvta IS NULL";
            string consulta = "SELECT a.tidoregri,a.nudoregri,b1.razonsocial as nombregri,b1.direcc1 as direregri,a.ubigregri as ubigregri,ifnull(b1.email,'') as emailR,ifnull(b1.numerotel1,'') as numtel1R," +
                            "ifnull(b1.numerotel2,'') as numtel2R,a.tidodegri,a.nudodegri,b2.razonsocial as nombdegri,b2.direcc1 as diredegri,b2.ubigeo as ubigdegri,ifnull(b2.email,'') as emailD," +
                            "ifnull(b2.numerotel1,'') as numtel1D,ifnull(b2.numerotel2,'') as numtel2D,a.tipmongri,a.totgri,a.salgri,SUM(d.cantprodi) AS bultos,date(a.fechopegr) as fechopegr,a.tipcamgri," +
                            "max(d.descprodi) AS descrip,ifnull(m.descrizionerid,'') as mon,a.totgrMN,a.codMN,c.fecdocvta,b1.tiposocio as tipsrem,b2.tiposocio as tipsdes,a.docsremit," +
                            "a.plaplagri,a.carplagri,a.autplagri,a.confvegri,concat(lo.descrizionerid,' - ',ld.descrizionerid) as orides,max(d.unimedpro) as umed," +
                            "a.ubigregri,a.direregri as direorigen,a.ubigdegri,a.diredegri as diredesti " +
                            "from cabguiai a left join detguiai d on d.idc=a.id " +
                            "LEFT JOIN controlg c ON c.serguitra = a.sergui AND c.numguitra = a.numgui " +
                            "left join anag_cli b1 on b1.tipdoc=a.tidoregri and b1.ruc=a.nudoregri " +
                            "left join anag_cli b2 on b2.tipdoc=a.tidodegri and b2.ruc=a.nudodegri " +
                            "left join desc_mon m on m.idcodice=a.tipmongri " +
                            "left join desc_loc lo on lo.idcodice=a.locorigen " +
                            "left join desc_loc ld on ld.idcodice=a.locdestin " +
                            "WHERE a.sergui = @ser AND a.numgui = @num AND a.estadoser not IN(@est)" + parte;   // descprodi
            using (MySqlCommand micon = new MySqlCommand(consulta, conn))
            {
                micon.Parameters.AddWithValue("@ser", serGR);
                micon.Parameters.AddWithValue("@num", numGR);
                micon.Parameters.AddWithValue("@est", codAnul);
                using (MySqlDataReader dr = micon.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (!dr.IsDBNull(0))    //  && dr[24] == DBNull.Value
                        {
                            datcltsR[fila,0] = dr.GetString("tidoregri");        // datos del remitente de la GR
                            datcltsR[fila, 1] = dr.GetString("nudoregri");
                            datcltsR[fila, 2] = dr.GetString("nombregri");
                            datcltsR[fila, 3] = dr.GetString("direregri");
                            datcltsR[fila, 4] = dr.GetString("ubigregri");        // ubigeo pto de partida de la GR
                            datcltsR[fila, 5] = dr.GetString("emailR");
                            datcltsR[fila, 6] = dr.GetString("numtel1R");
                            datcltsR[fila, 7] = dr.GetString("numtel2R");
                            datcltsR[fila, 8] = dr.GetString("tipsrem");
                            //
                            datcltsD[fila, 0] = dr.GetString("tidodegri");        // datos del destinatario de la GR
                            datcltsD[fila, 1] = dr.GetString("nudodegri");
                            datcltsD[fila, 2] = dr.GetString("nombdegri");
                            datcltsD[fila, 3] = dr.GetString("diredegri");
                            datcltsD[fila, 4] = dr.GetString("ubigdegri");        // ubigeo del pto de llegada de la GR
                            datcltsD[fila, 5] = dr.GetString("emailD");
                            datcltsD[fila, 6] = dr.GetString("numtel1D");
                            datcltsD[fila, 7] = dr.GetString("numtel2D");
                            datcltsD[fila, 8] = dr.GetString("tipsdes");
                            //  
                            datguias[fila, 0] = serGR + "-" + numGR;                 // GR
                            datguias[fila, 1] = (dr.IsDBNull(20)) ? "" : dr.GetString("descrip");         // descrip = descprodi
                            datguias[fila, 2] = (dr.IsDBNull(19)) ? "0" : dr.GetString("bultos");          // cant bultos
                            datguias[fila, 3] = dr.GetString("mon");             // nombre moneda de la GR
                            datguias[fila, 4] = dr.GetString("totgri");          // valor GR en su moneda
                            datguias[fila, 5] = dr.GetString("totgrMN");         // valor GR en moneda local
                            datguias[fila, 6] = dr.GetString("codMN");            // codigo moneda local
                            datguias[fila, 7] = dr.GetString("tipmongri");        // codigo moneda de la guía
                            datguias[fila, 8] = dr.GetString("tipcamgri");     // tipo de cambio de la GR
                            var a = dr.GetString("fechopegr").Substring(0, 10);
                            datguias[fila, 9] = a.Substring(6, 4) + "-" + a.Substring(3, 2) + "-" + a.Substring(0, 2);     // fecha de la GR
                            datguias[fila, 10] = dr.GetString("docsremit");
                            datguias[fila, 11] = dr.GetString("plaplagri");
                            datguias[fila, 12] = dr.GetString("carplagri");
                            datguias[fila, 13] = dr.GetString("autplagri");
                            datguias[fila, 14] = dr.GetString("confvegri");
                            datguias[fila, 15] = dr.GetString("orides");
                            datguias[fila, 16] = dr.GetString("umed");
                            datguias[fila, 17] = dr.GetString("salgri");
                            datguias[fila, 18] = dr.GetString("ubigregri");        // ubigeo origen GR  
                            datguias[fila, 19] = dr.GetString("direorigen");        // direc origen GR
                            datguias[fila, 20] = dr.GetString("ubigdegri");        // ubigeo destino GR
                            datguias[fila, 21] = dr.GetString("diredesti");        // direc destino GR
                            retorna = true;
                        }
                    }
                }
            }
            return retorna;
        }
        private void tipcambio(string codmod)                // funcion para calculos con el tipo de cambio
        {
            decimal totflet = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    totflet = totflet + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()); // VALOR DE LA GR EN MONEDA LOCAL
                }
            }
            // si codmod es moneda local, suma campos totales de moneda local y retorna valor
            if (codmod == MonDeft)
            {
                tx_flete.Text = totflet.ToString("#0.00");
            }
            else
            {
                if (codmod != "")
                {
                    vtipcam vtipcam = new vtipcam(tx_tfmn.Text, codmod, DateTime.Now.Date.ToString());
                    var result = vtipcam.ShowDialog();
                    tx_flete.Text = vtipcam.ReturnValue1;
                    tx_fletMN.Text = vtipcam.ReturnValue2;
                    tx_tipcam.Text = vtipcam.ReturnValue3;
                    tx_flete_Leave(null, null);
                }
            }
        }
        private void calculos(decimal totDoc)
        {
            decimal tigv = 0;
            decimal tsub = 0;
            if (totDoc > 0)
            {
                tsub = Math.Round(totDoc / (1 + decimal.Parse(v_igv) / 100), 2);
                tigv = Math.Round(totDoc - tsub, 2);
                
            }
            tx_igv.Text = tigv.ToString("#0.00");
            tx_subt.Text = tsub.ToString("#0.00");
        }
        int CentimeterToPixel(double Centimeter)
        {
            double pixel = -1;
            using (Graphics g = this.CreateGraphics())
            {
                pixel = Centimeter * g.DpiY / 2.54d;
            }
            return (int)pixel;
        }
        private void cargaunica(int fila)               // campos de carga unica
        {
            if (true)        // 08/02/2024    Tx_modo.Text == "NUEVO"
            {
                if (chk_cunica.Checked == true) // .CheckState.ToString() == "True"
                {
                    panel2.Enabled = true;
                    tx_dat_dpo.Enabled = true;
                    tx_dat_dpd.Enabled = true;
                    if (dataGridView1.Rows[0].Cells[0].Value != null)
                    {
                        //MessageBox.Show("hay guia");
                        tx_pla_placa.Text = datguias[fila,11].ToString();
                        tx_pla_confv.Text = datguias[fila,14].ToString();
                        tx_pla_autor.Text = datguias[fila,13].ToString();
                        tx_cetm.Text = "";
                        tx_cutm.Text = "";
                        tx_valref1.Text = "";
                        tx_valref2.Text = "";
                        tx_valref3.Text = "";
                        tx_dat_dpo.Text = datguias[fila, 19].ToString();    // datcltsR[fila,3].ToString();
                        tx_dat_dpd.Text = datguias[fila, 21].ToString();    // datcltsD[fila,3].ToString();
                        tx_dat_upo.Text = datguias[fila, 18].ToString();    // datcltsR[fila,4].ToString();
                        tx_dat_upd.Text = datguias[fila, 20].ToString();    // datcltsD[fila,4].ToString();
                    }
                }
                else
                {
                    panel2.Enabled = false;
                    tx_dat_dpo.Enabled = false;
                    tx_dat_dpd.Enabled = false;
                    //
                    tx_pla_placa.Text = "";
                    tx_pla_confv.Text = "";
                    tx_pla_autor.Text = "";
                    tx_cetm.Text = "";
                    tx_cutm.Text = "";
                    tx_valref1.Text = "";
                    tx_valref2.Text = "";
                    tx_valref3.Text = "";
                    tx_dat_dpo.Text = "";
                    tx_dat_dpd.Text = "";
                    tx_dat_upo.Text = "";
                    tx_dat_upd.Text = "";
                }
            }
        }
        private void armacfe()                  // arma cabecera de fact elect.
        {
            tcfe.Clear();
            tcfe.Columns.Add("_fecemi");    // fecha de emision   yyyy-mm-dd
            tcfe.Columns.Add("Prazsoc");    // razon social del emisor
            tcfe.Columns.Add("Pnomcom");    // nombre comercial del emisor
            tcfe.Columns.Add("ubigEmi");    // UBIGEO DOMICILIO FISCAL
            tcfe.Columns.Add("Pdf_dir");    // DOMICILIO FISCAL - direccion
            tcfe.Columns.Add("Pdf_urb");    // DOMICILIO FISCAL - Urbanizacion
            tcfe.Columns.Add("Pdf_pro");    // DOMICILIO FISCAL - provincia
            tcfe.Columns.Add("Pdf_dep");    // DOMICILIO FISCAL - departamento
            tcfe.Columns.Add("Pdf_dis");    // DOMICILIO FISCAL - distrito
            tcfe.Columns.Add("paisEmi");    // DOMICILIO FISCAL - código de país
            tcfe.Columns.Add("Ptelef1");    // teléfono del emisor
            tcfe.Columns.Add("Pweb1");      // página web del emisor
            tcfe.Columns.Add("Prucpro");    // Ruc del emisor
            tcfe.Columns.Add("Pcrupro");    // codigo Ruc emisor
            tcfe.Columns.Add("_tipdoc");    // Tipo de documento de venta - 1 car
            tcfe.Columns.Add("_moneda");    // Moneda del doc. de venta - 3 car
            tcfe.Columns.Add("_sercor");    // Serie y correlat concatenado F001-00000001 - 13 car
            tcfe.Columns.Add("Cnumdoc");    // numero de doc. del cliente - 15 car
            tcfe.Columns.Add("Ctipdoc");    // tipo de doc. del cliente - 1 car
            tcfe.Columns.Add("Cnomcli");    // nombre del cliente - 100 car
            tcfe.Columns.Add("ubigAdq");    // ubigeo del adquiriente - 6 car
            tcfe.Columns.Add("dir1Adq");    // direccion del adquiriente 1
            tcfe.Columns.Add("dir2Adq");    // direccion del adquiriente 2
            tcfe.Columns.Add("provAdq");    // provincia del adquiriente
            tcfe.Columns.Add("depaAdq");    // departamento del adquiriente
            tcfe.Columns.Add("distAdq");    // distrito del adquiriente
            tcfe.Columns.Add("paisAdq");    // pais del adquiriente
            tcfe.Columns.Add("_totoin");    // total operaciones inafectas
            tcfe.Columns.Add("_totoex");    // total operaciones exoneradas
            tcfe.Columns.Add("_toisc");     // total impuesto selectivo consumo
            tcfe.Columns.Add("_totogr");    // Total valor venta operaciones grabadas n(12,2)  15
            tcfe.Columns.Add("_totven");    // Importe total de la venta n(12,2)             15
            tcfe.Columns.Add("tipOper");    // tipo de operacion - 4 car
            tcfe.Columns.Add("codLocE");    // codigo local emisor
            tcfe.Columns.Add("conPago");    // condicion de pago
            tcfe.Columns.Add("plaPago");    // plazo de pago en días
            tcfe.Columns.Add("fvencto");    // fecha de vencimiento de la fact credito yyyy-mm-dd
            tcfe.Columns.Add("_codgui");    // Código de la guia de remision TRANSPORTISTA
            tcfe.Columns.Add("_scotro");    // serie y numero concatenado de la guia
            tcfe.Columns.Add("obser1");     // observacion del documento
            tcfe.Columns.Add("obser2");     // mas observaciones
            tcfe.Columns.Add("maiAdq");     // correo del adquiriente
            tcfe.Columns.Add("teladq");     // telefono del adquiriente
            tcfe.Columns.Add("totImp");     // total impuestos del documento
            tcfe.Columns.Add("codImp");     // codigo impuesto
            tcfe.Columns.Add("nomImp");     // nombre del tipo de impuesto
            tcfe.Columns.Add("tipTri");     // tipo de tributo
            tcfe.Columns.Add("monLet");     // monto en letras
            tcfe.Columns.Add("_horemi");    // hora de emision del doc.venta
            tcfe.Columns.Add("_fvcmto");    // fecha de vencimiento del doc.venta
            tcfe.Columns.Add("corclie");    // correo del emisor
            tcfe.Columns.Add("_morefD");    // moneda de refencia para el tipo de cambio
            tcfe.Columns.Add("_monobj");    // moneda objetivo del tipo de cambio
            tcfe.Columns.Add("_tipcam");    // tipo de cambio con 3 decimales
            tcfe.Columns.Add("_fechca");    // fecha del tipo de cambio
            tcfe.Columns.Add("d_conpa");    // condicion de pago
            tcfe.Columns.Add("d_valre");    // valor referencial
            tcfe.Columns.Add("d_numre");    // numero registro mtc del camion
            tcfe.Columns.Add("d_confv");    // config. vehicular del camion
            tcfe.Columns.Add("d_ptori");    // Pto de origen
            tcfe.Columns.Add("d_ptode");    // Pto de destino
            tcfe.Columns.Add("d_vrepr");    // valor referencial preliminar
            tcfe.Columns.Add("codleyt");    // codigoLeyenda 1 - valor en letras
            tcfe.Columns.Add("codobs");     // codigo del ose para las observaciones, caso carrion documentos origen del remitente
            tcfe.Columns.Add("_forpa");     // glosa de forma de pago SUNAT
            tcfe.Columns.Add("_valcr");     // valor credito
            tcfe.Columns.Add("_fechc");     // fecha programada del pago credito
            // detraccion
            tcfe.Columns.Add("d_porde");                    // 2 Porcentaje de detracción
            tcfe.Columns.Add("d_valde");                    // 3 Monto de la detracción
            tcfe.Columns.Add("d_codse");                    // 4 Código del Bien o Servicio Sujeto a Detracción
            tcfe.Columns.Add("d_ctade");                    // 5 Número del cta en el bco de la nación
            tcfe.Columns.Add("d_medpa");                    // 6 medio de pago de la detraccion (001 = deposito en cuenta)
            tcfe.Columns.Add("glosdet");                    // 7 Leyenda: Detracción        300
            tcfe.Columns.Add("totdet", typeof(double));     // total detraccion
            tcfe.Columns.Add("codleyd");                    // codigo leyenda detraccion
            tcfe.Columns.Add("d_monde");                    // moneda de la detraccion
        }
        private void armadfe()                  // arma detalle de fact elect.
        {
            tdfe.Clear();
            tdfe.Columns.Add("Inumord");                    // 2 numero de orden del item           
            tdfe.Columns.Add("Idatper");                    // 3 Datos personilazados del item      
            tdfe.Columns.Add("Iumeded");                    // 4 Unidad de medida                    3
            tdfe.Columns.Add("Icantid");                    // 5 Cantidad de items             n(12,2)
            tdfe.Columns.Add("Idescri");                    // 6 Descripcion                       500
            tdfe.Columns.Add("Idesglo");                    // 7 descricion de la glosa del item   250
            tdfe.Columns.Add("Icodprd");                    // 8 codigo del producto del cliente    30
            tdfe.Columns.Add("Icodpro");                    // 9 codigo del producto SUNAT           8
            tdfe.Columns.Add("Icodgs1");                    // 10 codigo del producto GS1           14
            tdfe.Columns.Add("Icogtin");                    // 11 tipo de producto GTIN             14
            tdfe.Columns.Add("Inplaca");                    // 12 numero placa de vehiculo
            tdfe.Columns.Add("Ivaluni");                    // 13 Valor unitario del item SIN IMPUESTO 
            tdfe.Columns.Add("Ipreuni");                    // 14 Precio de venta unitario CON IGV
            tdfe.Columns.Add("Ivalref");                    // 15 valor referencial del item cuando la venta es gratuita
            tdfe.Columns.Add("_msigv", typeof(double));     // 16 monto igv
            tdfe.Columns.Add("Icatigv");                    // 17 tipo/codigo de afectacion igv
            tdfe.Columns.Add("Itasigv");                    // 18 tasa del igv
            tdfe.Columns.Add("Iigvite");                    // 19 monto IGV del item
            tdfe.Columns.Add("Icodtri");                    // 20 codigo del tributo por item
            tdfe.Columns.Add("Iiscmba");                    // 21 ISC monto base
            tdfe.Columns.Add("Iisctas");                    // 22 ISC tasa del tributo
            tdfe.Columns.Add("Iisctip");                    // 23 ISC tipo de afectacion
            tdfe.Columns.Add("Iiscmon");                    // 24 ISC monto del tributo
            tdfe.Columns.Add("Icbper1");                    // 25 indicador de afecto a ICBPER
            tdfe.Columns.Add("Icbper2");                    // 26 monto unitario de ICBPER
            tdfe.Columns.Add("Icbper3");                    // 27 monto total ICBPER del item
            tdfe.Columns.Add("Iotrtri");                    // 28 otros tributos monto base
            tdfe.Columns.Add("Iotrtas");                    // 29 otros tributos tasa del tributo
            tdfe.Columns.Add("Iotrlin");                    // 30 otros tributos monto unitario
            tdfe.Columns.Add("Itdscto");                    // 31 Descuentos por ítem
            tdfe.Columns.Add("Iincard");                    // 32 indicador de cargo/descuento
            tdfe.Columns.Add("Icodcde");                    // 33 codigo de cargo/descuento
            tdfe.Columns.Add("Ifcades");                    // 34 Factor de cargo/descuento
            tdfe.Columns.Add("Imoncde");                    // 35 Monto de cargo/descuento
            tdfe.Columns.Add("Imobacd");                    // 36 Monto base del cargo/descuento
            tdfe.Columns.Add("Ivalvta");                    // 37 Valor de venta del ítem

            //tdfe.Columns.Add("Iotrsis");                    // otros tributos tipo de sistema
            //tdfe.Columns.Add("Imonbas");                    // monto base (valor sin igv * cantidad)
            //tdfe.Columns.Add("Isumigv");                    // Sumatoria de igv
            //tdfe.Columns.Add("Iindgra");                    // indicador de gratuito
        }
        public void bt_jala_Click(object sender, EventArgs e)   // reconecta y jala datos de conectores
        {
            if (tx_dat_tdRem.Text != "" && tx_numDocRem.Text != "" && Tx_modo.Text == "NUEVO")
            {
                if (tx_dat_tdRem.Text == vtc_ruc)
                {
                    if (lib.valiruc(tx_numDocRem.Text, vtc_ruc) == false)
                    {
                        MessageBox.Show("Número de RUC inválido", "Atención - revise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tx_numDocRem.Focus();
                        return;
                    }
                    if (TransCarga.Program.vg_conSol == true)
                    {
                        string[] rl = lib.conectorSolorsoft("RUC", tx_numDocRem.Text);
                        string myStr = rl[0].Replace("\r\n", "");
                        if (rl[0] == "" || myStr == NoRetGl)
                        {
                            var aa = MessageBox.Show(" No encontramos el documento en ningún registro. " + Environment.NewLine +
                                                    " Deberá ingresarlo manualmente si esta seguro(a) " + Environment.NewLine +
                                                    " de la validez del número y documento. " + Environment.NewLine +
                                                    "" + Environment.NewLine +
                                                    "Confirma que desea ingresarlo manualmente?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (aa == DialogResult.No)
                            {
                                tx_numDocRem.Text = "";
                                tx_nomRem.Text = "";      // razon social
                                tx_ubigRtt.Text = "";     // ubigeo
                                tx_dirRem.Text = "";      // direccion
                                tx_dptoRtt.Text = "";      // departamento
                                tx_provRtt.Text = "";      // provincia
                                tx_distRtt.Text = "";      // distrito
                                tx_telc1.Text = "";
                                tx_email.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            if (rl[6] != "ACTIVO" || rl[7] != "HABIDO")
                            {
                                var aa = MessageBox.Show("No debería crear al cliente" + Environment.NewLine +
                                    "el ruc tiene estado o condición no correcto" + Environment.NewLine + Environment.NewLine +
                                    "Condición: " + rl[7] + Environment.NewLine +
                                    "Estado: " + rl[6] + Environment.NewLine + Environment.NewLine +
                                    "CONFIRMA QUE DESEA CONTINUAR?", "Alerta - no debería continuar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (aa == DialogResult.No)
                                {
                                    tx_numDocRem.Text = "";
                                    tx_nomRem.Text = "";      // razon social
                                    tx_ubigRtt.Text = "";     // ubigeo
                                    tx_dirRem.Text = "";      // direccion
                                    tx_dptoRtt.Text = "";      // departamento
                                    tx_provRtt.Text = "";      // provincia
                                    tx_distRtt.Text = "";      // distrito
                                    tx_telc1.Text = "";
                                    tx_email.Text = "";
                                    return;
                                }
                            }
                            else
                            {
                                tx_nomRem.Text = rl[0].Trim().Replace("\r\n", "");      // razon social
                                tx_ubigRtt.Text = rl[1].Trim().Replace("\r\n", "");     // ubigeo
                                tx_dirRem.Text = rl[2].Trim().Replace("\r\n", "");      // direccion
                                tx_dptoRtt.Text = (rl[3].Trim().Replace("\r\n", "") == "PROV. CONST. DEL CALLAO") ? "CALLAO" : rl[3];      // departamento      
                                tx_provRtt.Text = (rl[4].Trim().Replace("\r\n", "") == "PROV.CONST.DEL CALLAO") ? "CALLAO" : rl[4];      // provincia    
                                tx_distRtt.Text = rl[5].Trim().Replace("\r\n", "");      // distrito
                            }
                        }
                    }
                }
                if (tx_dat_tdRem.Text == vtc_dni)
                {
                    if (TransCarga.Program.vg_conSol == true) // conector solorsoft para dni
                    {
                        string[] rl = lib.conectorSolorsoft("DNI", tx_numDocRem.Text);
                        string myStr = rl[0].Replace("\r\n", "");
                        if (rl[0] == "" || myStr == NoRetGl)
                        {
                            MessageBox.Show("No encontramos el DNI en la busqueda inicial, estamos abriendo" + Environment.NewLine +
                            "una página web para que efectúe la busqueda manualmente", "Redirección a web de DNI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            System.Diagnostics.Process.Start(webdni);    // "https://eldni.com/pe/buscar-por-dni"
                        }
                        else
                        {
                            tx_nomRem.Text = rl[0];      // nombre
                            tx_numDocRem.Text = rl[1];     // num dni
                        }
                    }
                }
            }
        }
        private void llena_matris_FE()          // funcion que llena las matrices con datos para el comprobante electrónico
        {
            /*
            string[] vs = {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",      // 20
                           "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};     // 20
            string[] va = { "", "", "", "", "", "", "", "", "", "" };      // 10
            string[,] dt = new string[10, 9] {
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" },
                    { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }, { "", "", "", "", "", "", "", "", "" }
                }; // 6 columnas, 10 filas
            string[] cu = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };    // 17
            */
            DataRow[] row = dttd1.Select("idcodice='" + tx_dat_tdv.Text + "'");             // tipo de documento venta
            tipdo = row[0][3].ToString();
            DataRow[] rowd = dttd0.Select("idcodice='" + tx_dat_tdRem.Text + "'");          // tipo de documento del cliente
            tipoDocEmi = rowd[0][3].ToString().Trim();
            DataRow[] rowm = dtm.Select("idcodice='" + tx_dat_mone.Text + "'");         // tipo de moneda
            tipoMoneda = rowm[0][2].ToString().Trim();
            // 
            vs[0] = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3);      // dr.GetString("martdve") + lib.Right(serie, 3);
            vs[1] = tx_numero.Text;                                                 // numero;
            vs[2] = tx_dat_tdv.Text;                                                // tipo;
            vs[3] = Program.dirfisc;                                                // direccion emisor
            if (tx_dat_tdv.Text != codfact) vs[4] = "Boleta de Venta Electrónica";
            if (tx_dat_tdv.Text == codfact) vs[4] = "Factura Electrónica";
            vs[5] = tx_fechope.Text;                                                // dr.GetString("fechope");
            vs[6] = tx_nomRem.Text;                                                 // dr.GetString("nombclt");
            vs[7] = tx_numDocRem.Text;                                              // dr.GetString("nudoclt");
            vs[8] = tx_dirRem.Text;                                                 // dr.GetString("direclt");
            vs[9] = tx_distRtt.Text;                                                // dr.GetString("distclt");
            vs[10] = tx_provRtt.Text;                                               // dr.GetString("provclt");
            vs[11] = tx_dptoRtt.Text;                                               // dr.GetString("dptoclt");
            vs[12] = tx_tfil.Text;      // tx_totcant.Text;                                               // dr.GetString("canfidt");
            vs[13] = tx_subt.Text;                                                  // dr.GetString("subtota");
            vs[14] = tx_igv.Text;                                                   // dr.GetString("igvtota");
            vs[15] = tx_flete.Text;                                                 // dr.GetString("totdvta");
            vs[16] = cmb_mon.Text;                                                  // dr.GetString("inimon");
            vs[17] = tx_fletLetras.Text.Trim();                                     // + ((dr.GetString("mondvta") == codmon) ? " SOLES" : " DOLARES AMERICANOS");
            vs[18] = (tx_dat_plazo.Text == "") ? "CONTADO" : "CREDITO";             // (dr.GetString("tippago").Trim() != "" && dr.GetString("plazocred").Trim() == "") ? "CONTADO" : "CREDITO";
            vs[19] = (tx_dat_plazo.Text == "") ? "" : tx_dat_dpla.Text;             // (dr.GetString("plazocred") != "") ? dr.GetString("dpc") : "";
            vs[20] = (double.Parse(tx_fletMN.Text) >= double.Parse(Program.valdetra)) ? glosdet : "";      // (dr.GetDouble("totdvMN") >= double.Parse(Program.valdetra)) ? glosdetra : "";   // Glosa para la detracción SI TIENE
            vs[21] = tipdo;                                                         // dr.GetString("cdtdv");
            vs[22] = tipoDocEmi;                                                    // dr.GetString("ctdcl");
            vs[23] = nipfe;                                                         // identificador de ose/pse metodo de envío
            vs[24] = restexto;                                                      // texto del resolucion sunat del ose/pse
            vs[25] = tx_dat_sun_autor.Text;                                         // dr.GetString("autorizPSE");
            vs[26] = tx_dat_sun_web.Text;                                           // dr.GetString("webosePSE");
            vs[27] = tx_digit.Text;                                                 // dr.GetString("userc").Trim();
            vs[28] = Program.vg_nlus;                                               // dr.GetString("nomLocO").Trim();
            vs[29] = despedida;                                                     // glosa despedida
            vs[30] = Program.cliente;                                               // nombre del emisor del comprobante
            vs[31] = Program.ruc;                                                   // ruc del emisor
            vs[32] = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd");   // dr.GetString("fvence");
            vs[33] = (tx_dat_plazo.Text == "") ? "CONTADO" : "CREDITO 1 CUOTA";  // dr.GetString("condicion");
            vs[34] = "Transporte Privado";          // modalidad de transporte
            vs[35] = "Venta";                       // motivo de traslado
            vs[36] = rowm[0][3].ToString().Trim();      // tipoMoneda;                    // dr.GetString("nonmone");
            vs[37] = "0";                           // tot operaciones inafectas
            vs[38] = "0";                           // tot operaciones exoneradas
                                                    // carga unica
            cu[0] = "";     // dr.GetString("placa");
            cu[1] = "";     // dr.GetString("confv");
            cu[2] = "";     // dr.GetString("autoriz");
            cu[3] = "";     // dr.GetString("cargaEf");
            cu[4] = "";     // dr.GetString("cargaUt");
            cu[5] = "";     // dr.GetString("rucTrans");
            cu[6] = "";     // dr.GetString("nomTrans");
            cu[7] = "";     // dr.GetString("fecIniTras");
            cu[8] = "";     // dr.GetString("dirPartida");
            cu[9] = "";     // dr.GetString("ubiPartida");
            cu[10] = "";    // dr.GetString("dirDestin");
            cu[11] = "";    // dr.GetString("ubiDestin");
            cu[12] = "";    // dr.GetString("dniChof");
            cu[13] = "";    // dr.GetString("brevete");
            cu[14] = "";    // dr.GetString("valRefViaje");
            cu[15] = "";    // dr.GetString("valRefVehic");
            cu[16] = "";    // dr.GetString("valRefTon");
                            // varios
            va[0] = logoclt;                    // Ruta y nombre del logo del emisor electrónico
            va[1] = glosser;                    // glosa del servicio en facturacion
            va[2] = codfact;                    // Tipo de documento FACTURA
            va[3] = Program.pordetra;           // porcentaje detracción
            double impDetr = Double.Parse(tx_fletMN.Text) * double.Parse(Program.pordetra) / 100;               // importe calculado de la detracción
            va[4] = impDetr.ToString("#0.00"); // (double.Parse(tx_fletMN.Text) * double.Parse(Program.pordetra) / 100).ToString("#0.00");         // monto detracción
            va[5] = Program.ctadetra;           // cta. detracción
            va[6] = "";                         // concatenado de Guias Transportista para Formato de cargas unicas
            va[7] = rutaQR + "pngqr";           // ruta y nombre del png codigo QR
            va[8] = rutaQR + Program.ruc + "-" + tipdo + "-" + vs[0] + "-" + vs[1] + ".pdf";                // ruta y nombre del pdf a subir a seencorp
            va[9] = (tx_tipcam.Text == "") ? "0" : tx_tipcam.Text;             // tipo de cambio

            double pigv = double.Parse(v_igv);
            double valCuot = 0;                     // valor de la cuota SI ES CREDITO
            if (vs[20] == "" && vs[18] == "CREDITO") valCuot = double.Parse(tx_flete.Text); // dr.GetDouble("totdvta");
            else
            {
                if (tx_dat_mone.Text == MonDeft)      // comprobante en soles?
                {
                    valCuot = Double.Parse(tx_flete.Text) - impDetr;
                }
                else
                {
                    valCuot = Math.Round(Double.Parse(tx_flete.Text) - (impDetr / double.Parse(tx_tipcam.Text)), 2);
                }
            }
            vs[39] = valCuot.ToString("#0.00");

            // detalle
            int tfg = (dataGridView1.Rows.Count == int.Parse(v_mfildet) && int.Parse(tx_tfil.Text) == int.Parse(v_mfildet)) ? int.Parse(v_mfildet) : dataGridView1.Rows.Count - 1;
            for (int l = 0; l < tfg; l++)
            {
                if (!string.IsNullOrEmpty(dataGridView1.Rows[l].Cells[0].Value.ToString()))   //  dataGridView1.Rows[l].Cells[0].Value != null
                {
                    dt[l, 0] = dataGridView1.Rows[l].Cells[10].Value.ToString();
                    dt[l, 1] = dataGridView1.Rows[l].Cells[2].Value.ToString();     // drg.GetString("cantbul");
                    dt[l, 2] = dataGridView1.Rows[l].Cells[11].Value.ToString();     // drg.GetString("unimedp");
                    dt[l, 3] = dataGridView1.Rows[l].Cells[0].Value.ToString();     // drg.GetString("codgror");
                    dt[l, 4] = dataGridView1.Rows[l].Cells[1].Value.ToString();     // drg.GetString("descpro");
                    dt[l, 5] = dataGridView1.Rows[l].Cells[8].Value.ToString();     // drg.GetString("docsremit");
                    dt[l, 6] = Math.Round(double.Parse(dataGridView1.Rows[l].Cells[4].Value.ToString()) / (1 + (double.Parse(v_igv) / 100)), 2).ToString("#0.00");     // drg.GetString("valUni");
                    dt[l, 7] = Math.Round(double.Parse(dataGridView1.Rows[l].Cells[4].Value.ToString()), 2).ToString("#0.00");     // drg.GetString("preUni");
                    dt[l, 8] = Math.Round(double.Parse(dataGridView1.Rows[l].Cells[4].Value.ToString()), 2).ToString("#0.00");     // drg.GetString("totalgr");
                    va[6] = va[6] + " " + dataGridView1.Rows[l].Cells[0].Value.ToString();      // drg.GetString("codgror");
                }
            }
        }

        #region facturacion electronica
        private bool factElec(string provee, bool envia, string accion, int ctab, bool EnvPdf)                 // conexion a facturacion electrónica provee=proveedor | tipo=txt ó json
        {
            // provee -> identificador del proveedor pse/ose
            // envia --> true = genera y sube el json al proveedor, false = no genera tampoco sube
            // accion -> identificador de alta (nuevo) o baja (anulacion)
            // ctab ---> contador para las bajas
            // EnvPdf -> true = genera y sube pdf al proveedor, false = no genera tampoco sube
            bool retorna = false;
            
            DataRow[] row = dttd1.Select("idcodice='"+tx_dat_tdv.Text+"'");             // tipo de documento venta
            tipdo = row[0][3].ToString();
            string serie = row[0][1].ToString().Substring(0,1) + lib.Right(tx_serie.Text,3);
            string corre = tx_numero.Text;
            DataRow[] rowd = dttd0.Select("idcodice='"+tx_dat_tdRem.Text+"'");          // tipo de documento del cliente
            tipoDocEmi = rowd[0][3].ToString().Trim();
            DataRow[] rowm = dtm.Select("idcodice='" + tx_dat_mone.Text + "'");         // tipo de moneda
            tipoMoneda = rowm[0][2].ToString().Trim();
            //
            string archi = "";
            if (provee == "Horizont")
            {
                string ruta = rutatxt + "TXT/";
                if (accion == "alta")
                {
                    archi = rucclie + "-" + tipdo + "-" + serie + "-" + corre;
                    if (crearTXT(tipdo, serie, corre, ruta + archi) == true)
                    {
                        retorna = true;
                    }
                }
                if (accion == "baja")
                {
                    //string _fecemi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   
                    string _fecemi = tx_fechact.Text.Substring(6, 4) + "-" + tx_fechact.Text.Substring(3, 2) + "-" + tx_fechact.Text.Substring(0, 2);   // fecha de emision   yyyy-mm-dd
                    string _secuen = lib.Right("00" + ctab.ToString(), 3);
                    string _codbaj = "RA" + "-" + tx_fechact.Text.Substring(6, 4) + tx_fechact.Text.Substring(3, 2) + tx_fechact.Text.Substring(0, 2);  // codigo comunicacion de baja
                    archi = rucclie + "-" + _codbaj + "-" + _secuen;
                    if (bajaTXT(tipdo, _fecemi, _codbaj, _secuen, ruta + archi, ctab, serie, corre) == true) retorna = true;
                }
            }
            if (provee == "seencorp")
            {
                string ruta = rutatxt + "TXT/";
                string rutaRpta = rutatxt + "RPTA/";
                archi = rucclie + "-" + tipdo + "-" + serie + "-" + corre;
                string archiR = "R-" + rucclie + "-" + tipdo + "-" + serie + "-" + corre + ".txt";
                IConectarWS cws = new ConectarWS();
                if (accion == "alta")
                {
                    if (envia == true)
                    {
                        //archi = rucclie + "-" + tipdo + "-" + serie + "-" + corre + ".json";
                        string ajson = json_venta(tipdo, tipoDocEmi);
                        System.IO.File.WriteAllText(ruta + archi + ".json", ajson);
                        String respuesta = cws.leerArchivo(archi + ".json", ruta, rutaRpta, usuaInteg, clavInteg);
                        if (respuesta.Substring(0, 7) == "Client.")
                        {
                            MessageBox.Show("No se pudo enviar el comprobante al servicio del proveedor: " + provee + Environment.NewLine +
                                "El motivo fue el siguiente: " + Environment.NewLine +
                                respuesta, " ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            System.IO.File.WriteAllText(rutaRpta + archiR, respuesta);
                            //retorna = false;
                        }
                        else
                        {
                            retorna = true;
                            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                            {
                                conn.Open();
                                //string actua = "update adifactu set nticket=@ntk,fticket=now() where tipdoc=@tdv and serie=@sdv and numero=@ndv";
                                string actua = "update adifactu set nticket=@ntk,fticket=now(),ose_pse=@pse,autoriz=@aut,webose=@web " +
                                    "where tipdoc=@tdv and serie=@sdv and numero=@ndv";
                                using (MySqlCommand micon = new MySqlCommand(actua, conn))
                                {
                                    micon.Parameters.AddWithValue("@ntk", respuesta);
                                    micon.Parameters.AddWithValue("@tdv", tx_dat_tdv.Text);
                                    micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                                    micon.Parameters.AddWithValue("@ndv", tx_numero.Text);
                                    micon.Parameters.AddWithValue("@pse", nipfe);
                                    micon.Parameters.AddWithValue("@aut", autoriz_OSE_PSE);
                                    micon.Parameters.AddWithValue("@web", webose);
                                    micon.ExecuteNonQuery();
                                    System.IO.File.WriteAllText(rutaRpta + archiR, respuesta);
                                }
                            }
                        }
                    }
                    if (EnvPdf == true)                        // generar el pdf para subirlo al servidor de seencorp 04/03/2024
                    {
                        llena_matris_FE();
                        try
                        {
                            impDVs imp = new impDVs();
                            imp.impDV(1, v_impTK, vs, dt, va, cu, vi_formato, v_CR_gr_ind, true);   // generamos el pdf en el directorio temporal
                            cws.leerArchivoPdf(archi + ".PDF", rutaQR, "", usuaInteg, clavInteg);
                            // Una vez resuelto el problema se debe proceder a regenerar el json ... 05/02/2024
                            if (File.Exists(@va[8])) File.Delete(@va[8]);
                            retorna = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No se pudo grabar el documento destino" + Environment.NewLine +
                                ex.Message, "Error en generar el PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //retorna = false;
                        }

                    }

                }
                if (accion == "baja")
                {
                    if (false)   // tx_dat_tdv.Text != codfact
                    {
                        MessageBox.Show("Recuerde que las anulaciones de BOLETAS deben" + Environment.NewLine + 
                            "hacerse manualmente su baja en el portal de " + provee,"Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        retorna = true;
                    }   // este pse seencorp no permite hacer bajas de Boletas .... que monses !! 05/02/2024 ... seguro ?
                    else
                    {
                        string _fecdoc = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   // fecha del comprobante
                        string _fecemi = tx_fechact.Text.Substring(6, 4) + "-" + tx_fechact.Text.Substring(3, 2) + "-" + tx_fechact.Text.Substring(0, 2);   // fecha de emision de la baja  yyyy-mm-dd
                        string _secuen = lib.Right("00" + ctab.ToString(), 3);
                        string _codbaj = "RA" + "-" + tx_fechact.Text.Substring(6, 4) + tx_fechact.Text.Substring(3, 2) + tx_fechact.Text.Substring(0, 2);  // codigo comunicacion de baja
                        archi = rucclie + "-" + _codbaj + "-" + _secuen + ".json";

                        string ajson = json_baja(_fecdoc, _codbaj + "-" + _secuen, _fecemi, tipdo);
                        System.IO.File.WriteAllText(ruta + archi, ajson);
                        if (true == true)
                        {
                            //IConectarWS cws = new ConectarWS();
                            String respuesta = cws.leerArchivo(archi, ruta, rutaRpta, usuaInteg, clavInteg);
                            if (respuesta.Substring(0, 7) == "Client.")
                            {
                                MessageBox.Show("No se pudo enviar el comprobante al servicio del proveedor: " + provee + Environment.NewLine +
                                    "El motivo fue el siguiente: " + Environment.NewLine +
                                    respuesta, " ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            System.IO.File.WriteAllText(rutaRpta + archiR, respuesta);
                            // Una vez resuelto el problema se debe proceder a regenerar el json ... 05/02/2024
                            retorna = true;
                        }
                    }
                }
            }
            return retorna;
        }

        #region horizonte
        private bool crearTXT(string tipdo, string serie, string corre, string file_path)
        {
            bool retorna;
            retorna = false;

            string _fecemi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   // fecha de emision   yyyy-mm-dd
            string Prazsoc = nomclie.Trim();                                            // razon social del emisor
            string Pnomcom = "";                                                        // nombre comercial del emisor
            string ubigEmi = ubiclie;                                                   // UBIGEO DOMICILIO FISCAL
            string Pdf_dir = Program.dirfisc.Trim();                                    // DOMICILIO FISCAL - direccion
            string Pdf_urb = "-";                                                       // DOMICILIO FISCAL - Urbanizacion
            string Pdf_pro = Program.provfis.Trim();                                    // DOMICILIO FISCAL - provincia
            string Pdf_dep = Program.depfisc.Trim();                                    // DOMICILIO FISCAL - departamento
            string Pdf_dis = Program.distfis.Trim();                                    // DOMICILIO FISCAL - distrito
            string paisEmi = "PE";                                                      // DOMICILIO FISCAL - código de país
            string Ptelef1 = Program.telclte1.Trim();                                   // teléfono del emisor
            string Pweb1 = "";                                                          // página web del emisor
            string Prucpro = Program.ruc;                                               // Ruc del emisor
            string Pcrupro = "6";                                                       // codigo Ruc emisor
            string _tipdoc = int.Parse(tipdo).ToString();                               // Tipo de documento de venta - 1 car
            string _moneda = tipoMoneda;                                                // Moneda del doc. de venta - 3 car
            string _sercor = serie + "-" + corre;                                       // Serie y correlat concatenado F001-00000001 - 13 car
            string Cnumdoc = tx_numDocRem.Text;                                         // numero de doc. del cliente - 15 car
            string Ctipdoc = tipoDocEmi;                                                // tipo de doc. del cliente - 1 car
            string Cnomcli = tx_nomRem.Text.Trim();                                     // nombre del cliente - 100 car
            string ubigAdq = tx_ubigRtt.Text;                                           // ubigeo del adquiriente - 6 car
            string dir1Adq = tx_dirRem.Text.Trim();                                     // direccion del adquiriente 1
            //string dir2Adq = "";                                                        // direccion del adquiriente 2
            string provAdq = tx_provRtt.Text.Trim();                                    // provincia del adquiriente
            string depaAdq = tx_dptoRtt.Text.Trim();                                    // departamento del adquiriente
            string distAdq = tx_distRtt.Text.Trim();                                    // distrito del adquiriente
            string paisAdq = "PE";                                                      // pais del adquiriente
            //string _totoin = "0.00";                                                       // total operaciones inafectas
            //string _totoex = "0.00";                                                       // total operaciones exoneradas
            //string _toisc = "0.00";                                                        // total impuesto selectivo consumo
            string _totogr = tx_flete.Text;                                             // Total valor venta operaciones grabadas n(12,2)  15
            string _totven = tx_subt.Text;                                              // Importe total de la venta n(12,2)             15
            string tipOper = s_tipOpeN;         // "0101";                                                    // tipo de operacion - 4 car
            string codLocE = Program.codlocsunat;                                       // codigo local emisor
            //string conPago = "01";                                                      // condicion de pago
            //string _codgui = "31";                                                      // Código de la guia de remision TRANSPORTISTA
            string _scotro = dataGridView1.Rows[0].Cells[0].Value.ToString();           // serie y numero concatenado de la guia
            string obser1 = tx_obser1.Text.Trim();                                      // observacion del documento
            //string obser2 = "";                                                         // mas observaciones
            string maiAdq = tx_email.Text.Trim();                                       // correo del adquiriente
            string teladq = tx_telc1.Text;                                              // telefono del adquiriente
            string totImp = tx_igv.Text;                                                // total impuestos del documento
            //string codImp = "1000";                                                     // codigo impuesto
            //string nomImp = "IGV";                                                      // nombre del tipo de impuesto
            //string tipTri = "VAT";                                                      // tipo de tributo
            string monLet = tx_fletLetras.Text.Trim();                                  // monto en letras
            string _horemi = "";                                                        // hora de emision del doc.venta
            string _fvcmto = "";                                                        // fecha de vencimiento del doc.venta
            string corclie = Program.mailclte;                                          // correo del emisor
            string _morefD = "";                                                        // moneda de refencia para el tipo de cambio
            string _monobj = "";                                                        // moneda objetivo del tipo de cambio
            string _tipcam = "";                                                        // tipo de cambio con 3 decimales
            string _fechca = "";                                                        // fecha del tipo de cambio

            string d_medpa = "";                                                        // medio de pago de la detraccion (001 = deposito en cuenta)
            string d_monde = "";                                                        // moneda de la detraccion
            string d_conpa = "";                                                        // condicion de pago
            double totdet = 0;
            string d_porde = "";                                                        // porcentaje de detraccion
            string d_valde = "";                                                        // valor de la detraccion
            string d_codse = "";                                                        // codigo de servicio
            string d_ctade = "";                                                        // cuenta detraccion BN
            //string d_valre = "";                                                        // valor referencial
            //string d_numre = "";                                                        // numero registro mtc del camion
            //string d_confv = "";                                                        // config. vehicular del camion
            //string d_ptori = "";                                                        // Pto de origen
            //string d_ptode = "";                                                        // Pto de destino
            //string d_vrepr = "";                                                        // valor referencial preliminar
            string codleyt = "1000";                                                    // codigoLeyenda 1 - valor en letras
            string codleyd = "";                                                        // codigo leyenda detraccion
            string codobs = "107";                                                      // codigo del ose para las observaciones, caso carrion documentos origen del remitente
            string _forpa = "";                                                         // glosa de forma de pago SUNAT
            string _valcr = "";                                                         // valor credito
            string _fechc = "";                                                         // fecha programada del pago credito
            if (tx_dat_tdv.Text == codfact)                          // campos solo para facturas "formas de pago"
            {
                if (rb_si.Checked == true)
                {
                    if (Convert.ToDateTime(fshoy) >= Convert.ToDateTime("2021-04-01"))  // forma de pago, campos para usarse a partir del 01/04/2021 según resolucion sunat
                    {
                        _forpa = "Contado"; 
                        _valcr = "";
                    }
                }
                else
                {
                    if (rb_no.Checked == true && rb_credito.Checked == true)
                    {
                        if (tx_dat_dpla.Text.Trim() == "") tx_dat_dpla.Text = "7";
                        if (Convert.ToDateTime(fshoy) >= Convert.ToDateTime("2021-04-01"))  // forma de pago, campos para usarse a partir del 01/04/2021 según resolucion sunat
                        {
                            _forpa = "Credito";
                            _valcr = tx_flete.Text;
                        }
                        string fansi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);
                        _fechc = DateTime.Parse(fansi).AddDays(double.Parse(tx_dat_dpla.Text)).Date.ToString("yyyy-MM-dd");        // fecha de emision + dias plazo credito
                    }
                    else
                    {
                        if (Convert.ToDateTime(fshoy) >= Convert.ToDateTime("2021-04-01"))  // forma de pago, campos para usarse a partir del 01/04/2021 según resolucion sunat
                        {
                            _forpa = "Contado";
                            _valcr = "";
                        }
                    }
                }
            }
            /* *********************   calculo y campos de detracciones   ****************************** */
            if (double.Parse(tx_flete.Text) > double.Parse(Program.valdetra) && tx_dat_tdv.Text == codfact && tx_dat_mone.Text == MonDeft)    // soles
            {

                // Están sujetos a las detracciones los servicios de transporte de bienes por vía terrestre gravado con el IGV, 
                // siempre que el importe de la operación o el valor referencial, según corresponda, sea mayor a 
                // S/ 400.00 o su equivalente en dólares ........ DICE SUNAT
                // ctadetra;                                                            // numeroCtaBancoNacion
                // valdetra;                                                            // monto a partir del cual tiene detraccion la operacion
                // coddetra;                                                            // codigoDetraccion
                // pordetra;                                                            // porcentajeDetraccion
                d_medpa = "001";                                    // medio de pago de la detraccion (001 = deposito en cuenta)
                d_monde = "PEN"; // MonDeft;                                  // moneda de la detraccion
                d_conpa = "CONTADO";                                // condicion de pago
                d_porde = Program.pordetra;                         // porcentaje de detraccion
                d_valde = Program.valdetra;                         // valor de la detraccion
                d_codse = Program.coddetra;                         // codigo de servicio
                d_ctade = Program.ctadetra;                         // cuenta detraccion BN
                //d_valre = "0";                                      // valor referencial
                //d_numre = "";                // numero registro mtc del camion
                //d_confv = "";                // config. vehicular del camion
                //d_ptori = "";                // Pto de origen
                //d_ptode = "";                // Pto de destino
                //d_vrepr = "0";               // valor referencial preliminar
                codleyt = "1000";            // codigoLeyenda 1 - valor en letras
                totdet = Math.Round(double.Parse(tx_flete.Text) * double.Parse(Program.pordetra) / 100, 2);    // totalDetraccion
                _valcr = Math.Round((double.Parse(tx_flete.Text) - totdet), 2).ToString("#0.00");               // cuota credito = valor - detraccion
                codleyd = "2006";
                tipOper = "1001";
                glosdet = glosdet + " " + d_ctade;                // leyenda de la detración
            }
            if (tx_dat_mone.Text != MonDeft) 
            {
                _morefD = tx_dat_monsunat.Text;                                      // moneda de refencia para el tipo de cambio
                _monobj = "PEN";        //tipoMoneda;                                // moneda objetivo del tipo de cambio
                _tipcam = tx_tipcam.Text;                                            // tipo de cambio con 3 decimales
                //_fechca = string.Format("{0:yyyy-MM-dd}", tx_fechope.Text);          // fecha del tipo de cambio
                _fechca = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);
                if (double.Parse(tx_flete.Text) > (double.Parse(Program.valdetra) / double.Parse(tx_tipcam.Text)) && tx_dat_tdv.Text == codfact)
                {
                    d_medpa = "001";                                    // medio de pago de la detraccion (001 = deposito en cuenta)
                    d_monde = "PEN";                                    // moneda de la detraccion SIEMPRE ES PEN moneda nacional
                    d_conpa = "CONTADO";                                // condicion de pago
                    d_porde = Program.pordetra;                         // porcentaje de detraccion
                    d_valde = Program.valdetra;                         // valor de la detraccion
                    d_codse = Program.coddetra;                         // codigo de servicio
                    d_ctade = Program.ctadetra;                         // cuenta detraccion BN
                    //d_valre = "0";                                      // valor referencial
                    //d_numre = "";                // numero registro mtc del camion
                    //d_confv = "";                // config. vehicular del camion
                    //d_ptori = "";                // Pto de origen
                    //d_ptode = "";                // Pto de destino
                    //d_vrepr = "0";               // valor referencial preliminar
                    codleyt = "1000";            // codigoLeyenda 1 - valor en letras
                    codleyd = "2006";
                    tipOper = "1001";
                    totdet = Math.Round(double.Parse(tx_fletMN.Text) * double.Parse(Program.pordetra) / 100, 2);    // totalDetraccion
                    _valcr = Math.Round((double.Parse(tx_fletMN.Text) - totdet), 2).ToString("#0.00");               // cuota credito = valor - detraccion
                }
            }
            /* ********************************************** GENERAMOS EL TXT    ************************************* */
            string sep = "|";    // char sep = (char)31;
            StreamWriter writer;
            file_path = file_path + ".txt";
            writer = new StreamWriter(file_path);
            writer.WriteLine("V|2.1|2.0||");
            writer.WriteLine("G" + sep +
                tipdo + sep +                   // Tipo de Comprobante Electrónico
                serie + sep +                   // Serie del Comprobante Electrónico
                corre + sep +                   // Numeración de Comprobante Electrónico
                _fecemi + sep +                 // Fecha de emisión
                _horemi + sep +                 // hora de emisión
                _moneda + sep +                 // Tipo de moneda
                _fvcmto + sep +                 // fecha de vencimiento del doc.venta
                Pcrupro + sep +                 // tipo de documento del emisor
                Prucpro + sep +                 // ruc emisor
                Prazsoc + sep +                 // razon social emisor
                Pnomcom + sep +                 // nombre comercial emisor
                Pdf_dir + sep +                 // Dirección detallada completa
                ubigEmi + sep +                 // ubigeo del emisor
                Pdf_dep + sep +                 // Departamento
                Pdf_pro + sep +                 // Provincia
                Pdf_urb + sep +                 // Urbanización
                Pdf_dis + sep +                 // Distrito
                paisEmi + sep +                 // pais del emisor
                codLocE + sep +                 // codigo sunat del local emisor
                corclie + sep +                 // Correo-Emisor
                Ptelef1 + sep +                 // telefono emisor
                Pweb1 + sep +                   // sitio web
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +    // lugar de entrega/venta itinerante
                Ctipdoc + sep +                 // Tipo de documento del cliente
                Cnumdoc + sep +                 // Nro. Documento del cliente
                Cnomcli + sep +                 // Razón social del cliente
                dir1Adq + sep +                 // Dirección
                ubigAdq + sep +                 // Ubigeo
                depaAdq + sep +                 // Departamento
                provAdq + sep +                 // Provincia
                "" + sep +                      // Urbanización   dir2Adq
                distAdq + sep +                 // Distrito
                paisAdq + sep +                 // Código país
                "" + sep +                      // codigo establecimiento adquiriente
                maiAdq + sep +                  // Correo-Receptor
                teladq + sep +                  // telefono del receptor
                "" + sep +                      // sitio web del arquiriente/receptor
                "" + sep + "" + sep +           // datos del comprador
                totImp + sep +                  // Total IGV
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +   // exportaciones, inafectas, exoneradas, gratuitas, etc
                _totven + sep +                 // Total operaciones gravadas
                totImp + sep +                  // total tributos grabados
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +       // ivap, isc, otros tributos
                "" + sep + "" + sep +           // total descuentos, total otros cargos
                _totogr + sep +                 // Importe total de la venta
                _totven + sep +                 // total valor venta
                _totogr + sep +                 // total precio venta
                "" + sep +                      // redondeo del importe total
                "" + sep +                      // total anticipos
                tipOper + sep +                 // Tipo de Operación
                "" + sep +                      // orden de compra
                _morefD + sep +                 // TIPO DE CAMBIO - moneda a cambiar
                _monobj + sep +                 // TIPO DE CAMBIO - moneda destino cambiada, osea MN
                _tipcam + sep +                 // TIPO DE CAMBIO - tipo de cambio
                _fechca + sep +                 // TIPO DE CAMBIO - fecha del tipo de cambio
                d_codse + sep +                 // DETRACCION - codigo de servicio
                d_ctade + sep +                 // DETRACCION - cuenta detraccion BN
                d_medpa + sep +                 // DETRACCION - medio de pago
                totdet + sep +                  // DETRACCION - valor
                d_porde + sep +                 // DETRACCION - porcentaje
                d_monde + sep +                 // DETRACCION - moneda
                d_conpa + sep +                 // DETRACCION - condicion de pago
                "" + sep +                      // FERROVIARIO
                "" + sep +                      // FERROVIARIO
                "" + sep +                      // FERROVIARIO
                "" + sep +                      // FERROVIARIO
                "" + sep +                      // DOCUMENTOS MODIFICA
                "" + sep +                      // DOCUMENTOS MODIFICA
                "" + sep +                      // DOCUMENTOS MODIFICA
                "" + sep +                      // DOCUMENTOS MODIFICA
                "" + sep +                      // DOCUMENTOS MODIFICA
                "" + sep +                      // INCOTERMS
                "" + sep +                      // INCOTERMS
                "" + sep +                      // IMPUESTO ICBPER
                _forpa + sep +                  // INF.ADICIONAL FORMA DE PAGO
                _valcr + sep                    // INF.ADICIONAL FORMA DE PAGO
            );
            int tfg = (dataGridView1.Rows.Count == int.Parse(v_mfildet) && int.Parse(tx_tfil.Text) == int.Parse(v_mfildet)) ? int.Parse(v_mfildet) : dataGridView1.Rows.Count - 1;
            for (int s = 0; s < tfg; s++)  // DETALLE
            {
                double _msigv = double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) / (1 + (double.Parse(v_igv) / 100));
                string Ipreuni = double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()).ToString("#0.00");     // Precio de venta unitario CON IGV
                if (tx_dat_mone.Text != MonDeft && dataGridView1.Rows[s].Cells["codmondoc"].Value.ToString() == MonDeft)   // 
                {
                    _msigv = Math.Round(_msigv / double.Parse(tx_tipcam.Text),2);
                    Ipreuni = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString())/ double.Parse(tx_tipcam.Text), 2).ToString("#0.00");
                }
                if (tx_dat_mone.Text == MonDeft && (dataGridView1.Rows[s].Cells["codmondoc"].Value.ToString().Trim() != "" && dataGridView1.Rows[s].Cells["codmondoc"].Value.ToString() != MonDeft))
                {
                    _msigv = Math.Round(_msigv * double.Parse(tx_tipcam.Text), 2);
                    Ipreuni = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) * double.Parse(tx_tipcam.Text), 2).ToString("#0.00");
                }
                string Inumord = (s + 1).ToString();                                        // numero de orden del item             5
                string Iumeded = "ZZ";                                                      // Unidad de medida                     3
                string Icantid = "1.00";                                                    // Cantidad de items   n(12,3)         16
                string Icodprd = "-";                                                       // codigo del producto del cliente
                string Icodpro = "";                                                        // codigo del producto SUNAT                          30
                string Icodgs1 = "";                                                        // codigo del producto GS1
                string Icogtin = "";                                                        // tipo de producto GTIN
                string Inplaca = "";                                                        // numero placa de vehiculo
                string Idescri = glosser + " " + dataGridView1.Rows[s].Cells["Descrip"].Value.ToString();   // Descripcion
                string Idescr2 = dataGridView1.Rows[s].Cells["Cant"].Value.ToString() + " " + dataGridView1.Rows[s].Cells["umed"].Value.ToString();
                string Ivaluni = _msigv.ToString("#0.00");                                  // Valor unitario del item SIN IMPUESTO 
                string Ivalref = "";                                                        // valor referencial del item cuando la venta es gratuita
                string Iigvite = Math.Round(double.Parse(Ipreuni) - double.Parse(Ivaluni),2).ToString("#0.00");     // monto IGV del item
                string Imonbas = Ivaluni;                                                   // monto base (valor sin igv * cantidad)
                string Isumigv = Iigvite;                                                   // Sumatoria de igv
                string Itasigv = Math.Round(double.Parse(v_igv), 2).ToString("#0.00");      // tasa del igv
                string Icatigv = "10";                                                      // Codigo afectacion al igv                    2
                string Iindgra = "";                                                        // indicador de gratuito
                string Iiscmba = "";                                                        // ISC monto base
                string Iiscmon = "";                                                        // ISC monto del tributo
                string Iisctas = "";                                                        // ISC tasa del tributo
                string Iisctip = "";                                                        // ISC tipo de sistema
                string Iotrtri = "";                                                        // otros tributos monto base
                string Iotrlin = "";                                                        // otros tributos monto unitario
                string Iotrtas = "";                                                        // otros tributos tasa del tributo
                string Iotrsis = "";                                                        // otros tributos tipo de sistema
                string Ivalvta = Ivaluni;                                                   // Valor de venta del ítem
                //
                writer.WriteLine("I" + sep +
                    Inumord + sep +     // orden
                    Iumeded + sep +     // unidad de medida ...... servicio ZZ
                    Icantid + sep +     // cantidad 1 servicio de transporte
                    Icodprd + sep +     // codigo del producto o servicio
                    Icodpro + sep +     // codigo del producto sunat
                    Icodgs1 + sep +     // codigo de producto GS1
                    Icogtin + sep +     // tipo de producto GTIN
                    Inplaca + sep +     // numero placa de vehiculo
                    Idescri + " " + Idescr2 + sep +     // descripcion del servicio
                    Ivaluni + sep +     // Valor unitario por ítem - SIN IGV
                    Ipreuni + sep +     // Precio de venta unitario por ítem - CON IGV
                    Ivalref + sep +     // valor referencial del item cuando la venta es gratuita
                    Iigvite + sep +     // Monto IGV
                    Imonbas + sep +     // monto base (valor sin igv * cantidad)
                    Isumigv + sep +     // monto igv (valor igv * cantidad)
                    Itasigv + sep +     // tasa del igv
                    Icatigv + sep +     // Codigo afectacion al igv
                    Iindgra + sep +     // indicador de gratuidad
                    Iiscmba + sep +     // ISC monto base
                    Iiscmon + sep +     // ISC monto del tributo
                    Iisctas + sep +     // ISC tasa del tributo
                    Iisctip + sep +     // ISC tipo de sistema
                    Iotrtri + sep +     // otros tributos monto base
                    Iotrlin + sep +     // otros tributos monto unitario
                    Iotrtas + sep +     // otros tributos tasa del tributo
                    Iotrsis + sep +     // otros tributos tipo de sistema
                    Ivalvta + sep +     // Valor de venta del ítem
                    "" + sep + "" + sep + "" + sep + "" + sep +         // CARGO, codigo, factor, etc.
                    "" + sep + "" + sep + "" + sep + "" + sep +         // DESCUENTO, codigo, factor, etc
                    "" + sep + "" + sep + "" + sep                      // BOLSAS DE PLASTICO
                );
            }
            for (int s = 0; s < tfg; s++)
            {
                writer.WriteLine("T" + sep +
                    "31" + sep +
                    dataGridView1.Rows[s].Cells["guias"].Value.ToString() + sep +
                    dataGridView1.Rows[s].Cells["fechaGR"].Value.ToString() + sep
                );
            }
            writer.WriteLine("L" + sep +
                codleyt + sep +         // codigo leyenda monto en letras
                monLet + sep            // Leyenda: Monto expresado en Letras
            );
            if (chk_cunica.Checked == true && double.Parse(tx_flete.Text) > double.Parse(Program.valdetra))     // carga unica con detracción
            {
                writer.WriteLine("Q" + sep +
                "1" + sep +                              // item de detalle, como es carga unica siempre es 1
                tx_dat_upo.Text + sep +                  // ubigeo punto de origen
                tx_dat_dpo.Text + sep +                  // direccion detallada del pto de origen
                tx_dat_upd.Text + sep +                  // ubigeo punto destino
                tx_dat_dpd.Text + sep +                  // direccion detallada del pto destino
                "zzzzzz" + sep +                         // detalle del viaje
                "01" + sep +                             // tipo de valor referencial 1
                tx_valref1.Text + sep +                  // valor referencial del serv de transporte
                _moneda + sep +                          // tipo moneda 
                "02" + sep +                             // tipo de valor referencial 2
                tx_valref2.Text + sep +                  // valor referencial sobre la carga efectiva
                _moneda + sep +                          // tipo moneda 
                "03" + sep +                             // tipo de valor referencial 2
                tx_valref3.Text + sep +                  // valor referencial sobre la carga util nominal
                _moneda + sep +                          // tipo moneda 
                "" + sep +                              // inicio datos de tramo
                "" + sep +                              // aca no aplica porque todas son de un tramo
                "" + sep +                              // ..
                "" + sep +                              // ..
                "" + sep +                              // ..
                "" + sep +                              // ..
                "" + sep +                              // fin datos de tramo
                "" + sep +                              // inicio detalle del vehiculo
                "" + sep + "" + sep + "" + sep +        // ..
                "" + sep + "" + sep + "" + sep +        // ..
                "" + sep + "" + sep + "" + sep +        // ..
                "" + sep + "" + sep + "" + sep          // fin detalle del vehiculo
                );
            }
            if (_forpa == "Credito")
            {
                writer.WriteLine("F" + sep +
                "Cuota001" + sep +
                _valcr + sep +
                _fechc + sep);
            }
            if (codleyd != "")
            {
                writer.WriteLine("L" + sep +
                codleyd + sep +         // codigo leyenda monto en letras
                glosdet + sep);            // Leyenda: Monto expresado en Letras
            }
            for (int s = 0; s < tfg; s++)
            {
                writer.WriteLine("E" + sep +
                codobs + sep +
                dataGridView1.Rows[s].Cells["guiasclte"].Value.ToString() + sep);
            }
            writer.Flush();
            writer.Close();
            retorna = true;
            return retorna;
            /*
            d_valre + sep +                // valor referencial
                    d_numre + sep +                // numero registro mtc del camion
                    d_confv + sep +                // config. vehicular del camion
                    d_ptori + sep +                // Pto de origen
                    d_ptode + sep +                // Pto de destino
                    d_vrepr + sep +                      // valor referencial preliminar
                    "" + sep + "" + sep + "" + sep + "" + sep +     // monto anticipos, numero, ruc emisor, total anticipos
                        "" + sep + "" + sep + "" + sep + "" + sep +     // Tipo de nota(Crédito/Débito),Tipo del documento afectado,Numeración de documento afectado,Motivo del documento afectado
                        conPago + sep +     // Condición de Pago
                        "" + sep +          // Plazo de Pago
                        "" + sep +          // Fecha de vencimiento
                        "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +           // Forma de Pago del 1 al 6
                        "" + sep + "" + sep +                           // Número del pedido, Número de la orden de compra
                        "" + sep + "" + sep + "" + sep + "" + sep +     // sector publico: Numero de Expediente,Código de unidad ejecutora, Nº de contrato,Nº de proceso de selección
                        "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +  // varios campos opcionales
                        obser1 + sep + obser2 + sep + "" + sep +        // observaciones del documento 1 y 2
                        _totoin + sep +                  // Total operaciones inafectas
                        _totoex + sep +                  // total operaciones exoneradas
                        "" + sep +                       // total operaciones gratuitas gratuitas
                        "" + sep +                       // Monto Fondo Inclusión Social Energético FISE
                        _toisc + sep +                          // Total ISC
                        "" + sep + "" + sep + "" + sep + "" + sep +  // Total otros tributos,Total otros,Descuento Global,Total descuento
                        "" + sep +      // Leyenda: Transferencia gratuita o servicio prestado gratuitamente
                        "" + sep +      // Leyenda: Bienes transferidos en la Amazonía
                        "" + sep +      // Leyenda: Servicios prestados en la Amazonía
                        "" + sep +      // Leyenda: Contratos de construcción ejecutados en la Amazonía
                        "" + sep + "" + sep + "");  // Leyenda: Exoneradas,Leyenda: Inafectas,Leyenda: Emisor itinerante
            */        
        }
        private bool bajaTXT(string tipdo, string _fecemi, string _codbaj, string _secuen, string file_path, int cuenta, string serie, string corre)
        {
            bool retorna = false;

            string Prazsoc = nomclie.Trim();                                            // razon social del emisor
            string Prucpro = Program.ruc;                                               // Ruc del emisor
            string Pcrupro = "6";                                                       // codigo Ruc emisor
            string motivo = glosaAnul;          // "ANULACION";
            string fecdoc = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   // fecha de emision   yyyy-mm-dd
            /* ********************************************** GENERAMOS EL TXT    ************************************* */
            string sep = "|";    // char sep = (char)31;
            StreamWriter writer;
            file_path = file_path + ".txt";
            writer = new StreamWriter(file_path);
            writer.WriteLine("G" + sep +
                Pcrupro + sep +                 // tipo de documento del emisor
                Prucpro + sep +                 // ruc emisor
                Prazsoc + sep +                 // razon social emisor
                fecdoc + sep +                 // fecha del documento dado de baja
                _codbaj + "-" + _secuen + sep +       // codigo identificador de la baja, secuencial dentro de cada día
                _fecemi + sep                   // fecha de la baja
            );
            writer.WriteLine("I" + sep +
                "1" + sep +
                tipdo + sep +
                serie + sep +
                corre + sep +
                motivo + sep
            );
            writer.Flush();
            writer.Close();
            retorna = true;

            return retorna;
        }
        private bool datosTXT(string tipdo, string serie, string corre, string file_path)
        {
            bool retorna = false;
            tcfe.Rows.Clear();
            DataRow row = tcfe.NewRow();
            row["_fecemi"] = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   // fecha de emision   yyyy-mm-dd
            row["Prazsoc"] = nomclie.Trim();                                            // razon social del emisor
            row["Pnomcom"] = "";                                                        // nombre comercial del emisor
            row["ubigEmi"] = ubiclie;                                                   // UBIGEO DOMICILIO FISCAL
            row["Pdf_dir"] = Program.dirfisc.Trim();                                    // DOMICILIO FISCAL - direccion
            row["Pdf_urb"] = "-";                                                       // DOMICILIO FISCAL - Urbanizacion
            row["Pdf_pro"] = Program.provfis.Trim();                                    // DOMICILIO FISCAL - provincia
            row["Pdf_dep"] = Program.depfisc.Trim();                                    // DOMICILIO FISCAL - departamento
            row["Pdf_dis"] = Program.distfis.Trim();                                    // DOMICILIO FISCAL - distrito
            row["paisEmi"] = "PE";                                                      // DOMICILIO FISCAL - código de país
            row["Ptelef1"] = Program.telclte1.Trim();                                   // teléfono del emisor
            row["Pweb1"] = "";                                                          // página web del emisor
            row["Prucpro"] = Program.ruc;                                               // Ruc del emisor
            row["Pcrupro"] = "6";                                                       // codigo Ruc emisor
            row["_tipdoc"] = tipdo;                                                     // Tipo de documento de venta - 1 car
            row["_moneda"] = tipoMoneda;                                                // Moneda del doc. de venta - 3 car
            row["_sercor"] = serie + "-" + corre;                                       // Serie y correlat concatenado F001-00000001 - 13 car
            row["Cnumdoc"] = tx_numDocRem.Text;                                         // numero de doc. del cliente - 15 car
            row["Ctipdoc"] = tipoDocEmi;                                                // tipo de doc. del cliente - 1 car
            row["Cnomcli"] = tx_nomRem.Text.Trim();                                     // nombre del cliente - 100 car
            row["ubigAdq"] = tx_ubigRtt.Text;                                           // ubigeo del adquiriente - 6 car
            row["dir1Adq"] = tx_dirRem.Text.Trim();                                     // direccion del adquiriente 1
            row["dir2Adq"] = "";                                                        // direccion del adquiriente 2
            row["provAdq"] = tx_provRtt.Text.Trim();                                    // provincia del adquiriente
            row["depaAdq"] = tx_dptoRtt.Text.Trim();                                    // departamento del adquiriente
            row["distAdq"] = tx_distRtt.Text.Trim();                                    // distrito del adquiriente
            row["paisAdq"] = "PE";  // y si es boliviano o veneco???                    // pais del adquiriente
            row["_totoin"] = "0.00";                                                       // total operaciones inafectas
            row["_totoex"] = "0.00";                                                       // total operaciones exoneradas
            row["_toisc"] = "";                                                         // total impuesto selectivo consumo
            row["_totogr"] = tx_subt.Text;                                              // Total valor venta operaciones grabadas n(12,2)  15
            row["_totven"] = tx_flete.Text;                                             // Importe total de la venta n(12,2)             15
            row["tipOper"] = s_tipOpeN;     // "0101";                                                    // tipo de operacion - 4 car
            row["codLocE"] = Program.codlocsunat;                                       // codigo local emisor
            //row["conPago"] = "01";                                                      // condicion de pago
            row["_codgui"] = "31";                                                      // Código de la guia de remision TRANSPORTISTA
            row["_scotro"] = dataGridView1.Rows[0].Cells[0].Value.ToString();           // serie y numero concatenado de la guia
            row["obser1"] = tx_obser1.Text.Trim();                                      // observacion del documento
            //row["obser2"] = "";                                                         // mas observaciones
            row["maiAdq"] = tx_email.Text.Trim();                                       // correo del adquiriente
            row["teladq"] = tx_telc1.Text;                                              // telefono del adquiriente
            row["totImp"] = tx_igv.Text;                                                // total impuestos del documento
            //row["codImp"] = "1000";                                                     // codigo impuesto
            //row["nomImp"] = "IGV";                                                      // nombre del tipo de impuesto
            //row["tipTri"] = "VAT";                                                      // tipo de tributo
            row["monLet"] = tx_fletLetras.Text.Trim();                                  // monto en letras
            row["_horemi"] = "";                                                        // hora de emision del doc.venta
            row["_fvcmto"] = "";                                                        // fecha de vencimiento del doc.venta
            row["plaPago"] = "";                                                        // plazo de pago cuando es credito
            row["corclie"] = Program.mailclte;                                          // correo del emisor
            row["_morefD"] = "";                                                        // moneda de refencia para el tipo de cambio
            row["_monobj"] = "";                                                        // moneda objetivo del tipo de cambio
            row["_tipcam"] = "";                                                        // tipo de cambio con 3 decimales
            row["_fechca"] = "";                                                        // fecha del tipo de cambio
            row["d_medpa"] = "";                                                        // medio de pago de la detraccion (001 = deposito en cuenta)
            row["d_monde"] = "";                                                        // moneda de la detraccion
            row["d_conpa"] = "";                                                        // condicion de pago
            row["totdet"] = 0;                                                          // total detraccion
            row["d_porde"] = "";                                                        // porcentaje de detraccion
            row["d_valde"] = "";                                                        // valor de la detraccion
            row["d_codse"] = "";                                                        // codigo de servicio
            row["d_ctade"] = "";                                                        // cuenta detraccion BN
            //row["d_valre"] = "";                                                        // valor referencial
            //row["d_numre"] = "";                                                        // numero registro mtc del camion
            //row["d_confv"] = "";                                                        // config. vehicular del camion
            //row["d_ptori"] = "";                                                        // Pto de origen
            //row["d_ptode"] = "";                                                        // Pto de destino
            //row["d_vrepr"] = "";                                                        // valor referencial preliminar
            row["codleyt"] = "1000";                                                    // codigoLeyenda 1 - valor en letras
            row["codleyd"] = "";                                                        // codigo leyenda detraccion
            row["codobs"] = "107";                                                      // codigo del ose para las observaciones, caso carrion documentos origen del remitente
            row["_forpa"] = "";                                                         // glosa de forma de pago SUNAT
            row["_valcr"] = "";                                                         // valor credito
            row["_fechc"] = "";                                                         // fecha programada del pago credito
            if (tx_dat_tdv.Text == codfact)                          // campos solo para facturas "formas de pago"
            {
                if (rb_si.Checked == true)
                {
                    if (true)  // forma de pago, campos para usarse a partir del 01/04/2021 según resolucion sunat
                    {   // Convert.ToDateTime(fshoy) >= Convert.ToDateTime("2021-04-01")
                        row["conPago"] = "01";
                        row["_forpa"] = "Contado";
                        row["_valcr"] = "";
                        row["_fechc"] = row["_fecemi"];
                    }
                }
                else
                {
                    if (rb_no.Checked == true && rb_credito.Checked == true)
                    {
                        if (tx_dat_dpla.Text.Trim() == "") tx_dat_dpla.Text = "7";
                        string fansi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);
                        row["_fechc"] = DateTime.Parse(fansi).AddDays(double.Parse(tx_dat_dpla.Text)).Date.ToString("yyyy-MM-dd");        // fecha de emision + dias plazo credito
                        if (true)  // forma de pago, campos para usarse a partir del 01/04/2021 según resolucion sunat
                        {   // Convert.ToDateTime(fshoy) >= Convert.ToDateTime("2021-04-01")
                            row["conPago"] = "02";
                            row["_forpa"] = "Credito";
                            row["_valcr"] = tx_flete.Text;
                            row["plaPago"] = int.Parse(tx_dat_dpla.Text).ToString();
                            row["_fvcmto"] = row["_fechc"];
                            row["fvencto"] = row["_fechc"];
                        }
                    }
                    else
                    {   // SI NO ESTA CHECK EN SI TAMPOCO ESTA EN NO, ENTONCES SE ASUME SI, EFECTIVO
                        row["conPago"] = "01";
                        row["_forpa"] = "Contado";
                        row["_valcr"] = "";
                        row["_fechc"] = row["_fecemi"];
                    }
                }
            }
            /* *********************   calculo y campos de detracciones   ****************************** */
            if (double.Parse(tx_flete.Text) > double.Parse(Program.valdetra) && tx_dat_tdv.Text == codfact && tx_dat_mone.Text == MonDeft)    // soles
            {

                // Están sujetos a las detracciones los servicios de transporte de bienes por vía terrestre gravado con el IGV, 
                // siempre que el importe de la operación o el valor referencial, según corresponda, sea mayor a 
                // S/ 400.00 o su equivalente en dólares ........ DICE SUNAT
                // ctadetra;                                                            // numeroCtaBancoNacion
                // valdetra;                                                            // monto a partir del cual tiene detraccion la operacion
                // coddetra;                                                            // codigoDetraccion
                // pordetra;                                                            // porcentajeDetraccion
                row["d_medpa"] = "001";                                                 // medio de pago de la detraccion (001 = deposito en cuenta)
                row["d_monde"] = "PEN"; // MonDeft;                                  // moneda de la detraccion
                row["d_conpa"] = "CONTADO";                                         // condicion de pago
                row["d_porde"] = Program.pordetra;                         // porcentaje de detraccion
                row["d_valde"] = Program.valdetra;                         // valor de la detraccion
                row["d_codse"] = Program.coddetra;                         // codigo de servicio
                row["d_ctade"] = Program.ctadetra;                         // cuenta detraccion BN
                //d_valre = "0";                                      // valor referencial
                //d_numre = "";                // numero registro mtc del camion
                //d_confv = "";                // config. vehicular del camion
                //d_ptori = "";                // Pto de origen
                //d_ptode = "";                // Pto de destino
                //d_vrepr = "0";               // valor referencial preliminar
                row["codleyt"] = "1000";            // codigoLeyenda 1 - valor en letras
                row["totdet"] = Math.Round(double.Parse(tx_flete.Text) * double.Parse(Program.pordetra) / 100, 2);    // totalDetraccion
                row["codleyd"] = "2006";
                row["tipOper"] = "1001";
                glosdet = glosdet + " " + row["d_ctade"];                // leyenda de la detración
                row["glosdet"] = glosdet;
            }
            if (tx_dat_mone.Text != MonDeft)
            {
                row["_morefD"] = tx_dat_monsunat.Text;                                      // moneda de refencia para el tipo de cambio
                row["_monobj"] = "PEN";        //tipoMoneda;                                // moneda objetivo del tipo de cambio
                row["_tipcam"] = tx_tipcam.Text;                                            // tipo de cambio con 3 decimales
                //_fechca = string.Format("{0:yyyy-MM-dd}", tx_fechope.Text);          // fecha del tipo de cambio
                row["_fechca"] = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);
                if (double.Parse(tx_flete.Text) > (double.Parse(Program.valdetra) / double.Parse(tx_tipcam.Text)) && tx_dat_tdv.Text == codfact)
                {
                    row["d_medpa"] = "001";                                    // medio de pago de la detraccion (001 = deposito en cuenta)
                    row["d_monde"] = "PEN";                                    // moneda de la detraccion SIEMPRE ES PEN moneda nacional
                    row["d_conpa"] = "CONTADO";                                // condicion de pago
                    row["d_porde"] = Program.pordetra;                         // porcentaje de detraccion
                    row["d_valde"] = Program.valdetra;                         // valor de la detraccion
                    row["d_codse"] = Program.coddetra;                         // codigo de servicio
                    row["d_ctade"] = Program.ctadetra;                         // cuenta detraccion BN
                    //d_valre = "0";                                      // valor referencial
                    //d_numre = "";                // numero registro mtc del camion
                    //d_confv = "";                // config. vehicular del camion
                    //d_ptori = "";                // Pto de origen
                    //d_ptode = "";                // Pto de destino
                    //d_vrepr = "0";               // valor referencial preliminar
                    row["codleyt"] = "1000";            // codigoLeyenda 1 - valor en letras
                    row["codleyd"] = "2006";
                    row["tipOper"] = "1001";
                    row["totdet"] = Math.Round(double.Parse(tx_fletMN.Text) * double.Parse(Program.pordetra) / 100, 2);    // totalDetraccion
                    glosdet = glosdet + " " + row["d_ctade"];                // leyenda de la detración
                    row["glosdet"] = glosdet;
                }
            }
            retorna = true;
            tcfe.Rows.Add(row);

            return retorna;
        }
        private bool datDetxt(string tipdo, string serie, string corre)
        {
            bool retorna = false;
            tdfe.Rows.Clear();
            for (int s = 0; s < dataGridView1.Rows.Count - 1; s++)
            {
                glosser2 = dataGridView1.Rows[s].Cells["OriDest"].Value.ToString() + " - " + tx_totcant.Text.Trim() + " Bultos";
                DataRow row = tdfe.NewRow();
                row["Idatper"] = "";                                                        // datos personalizados del item
                row["_msigv"] = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) - (double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) / (1 + (double.Parse(v_igv) / 100))),2);
                row["Ipreuni"] = double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()).ToString("#0.0000000000");     // Precio de venta unitario CON IGV
                row["Ivaluni"] = (double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) - (double)row["_msigv"]).ToString("#0.0000000000");
                if (tx_dat_mone.Text != MonDeft && dataGridView1.Rows[s].Cells["codmondoc"].Value.ToString() == MonDeft)   // 
                {
                    //row["_msigv"] = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) / (1 + (double.Parse(v_igv) / 100)) / double.Parse(tx_tipcam.Text), 2);
                    row["_msigv"] = Math.Round(((double)row["_msigv"] / double.Parse(tx_tipcam.Text)), 2);
                    row["Ipreuni"] = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) / double.Parse(tx_tipcam.Text), 2).ToString("#0.0000000000");
                    row["Ivaluni"] = ((double)row["Ivaluni"] / double.Parse(tx_tipcam.Text)).ToString("#0.0000000000");
                }
                if (tx_dat_mone.Text == MonDeft && dataGridView1.Rows[s].Cells["codmondoc"].Value.ToString() != MonDeft)
                {
                    row["_msigv"] = Math.Round((double)row["_msigv"] * double.Parse(tx_tipcam.Text), 2);
                    row["Ipreuni"] = Math.Round(double.Parse(dataGridView1.Rows[s].Cells["valor"].Value.ToString()) * double.Parse(tx_tipcam.Text), 2).ToString("#0.0000000000");
                    row["Ivaluni"] = ((double)row["Ivaluni"] * double.Parse(tx_tipcam.Text)).ToString("#0.0000000000");
                }
                row["Inumord"] = (s + 1).ToString();                                        // numero de orden del item             5
                row["Iumeded"] = "ZZ";                                                      // Unidad de medida                     3
                row["Icantid"] = "1.00";                                                    // Cantidad de items   n(12,3)         16
                row["Icodprd"] = " - ";                                                     // codigo del producto del cliente
                row["Icodpro"] = "";                                                        // codigo del producto SUNAT                          30
                row["Icodgs1"] = "";                                                        // codigo del producto GS1
                row["Icogtin"] = "";                                                        // tipo de producto GTIN
                row["Inplaca"] = "";                                                        // numero placa de vehiculo
                row["Idescri"] = glosser + " " + dataGridView1.Rows[s].Cells["Descrip"].Value.ToString() + " " + glosser2;   // Descripcion
                row["Idesglo"] = "";                                                        // descricion de la glosa del item 
                //row["Ivaluni"] = Math.Round((double.Parse(row["Ipreuni"].ToString()) - double.Parse(row["_msigv"].ToString())), 10).ToString();     // Valor unitario del item SIN IMPUESTO 
                row["Ivalref"] = "";                                                        // valor referencial del item cuando la venta es gratuita
                //row["Iigvite"] = Math.Round(double.Parse(row["Ipreuni"].ToString()) - double.Parse(row["Ivaluni"].ToString()), 2).ToString("#0.00");     // monto IGV del item
                row["Iigvite"] = row["_msigv"];
                //row["Imonbas"] = row["Ivaluni"];                                            // monto base (valor sin igv * cantidad)
                //row["Isumigv"] = row["Iigvite"];                                            // Sumatoria de igv
                row["Itasigv"] = Math.Round(double.Parse(v_igv), 2).ToString("#0.00");      // tasa del igv
                row["Icatigv"] = "10";                                                      // Codigo afectacion al igv                    2
                row["Icodtri"] = "1000";                                                    // codigo del tributo del item => igv = 1000
                //row["Iindgra"] = "";                                                      // indicador de gratuito
                row["Iiscmba"] = "";                                                        // ISC monto base
                row["Iiscmon"] = "";                                                        // ISC monto del tributo
                row["Icbper1"] = "";
                row["Icbper2"] = "";
                row["Icbper3"] = "";
                row["Iisctas"] = "";                                                        // ISC tasa del tributo
                row["Iisctip"] = "";                                                        // ISC tipo de sistema
                row["Iotrtri"] = "";                                                        // otros tributos monto base
                row["Iotrlin"] = "";                                                        // otros tributos monto unitario
                row["Itdscto"] = "0.00";                                                    // descuento por item
                row["Iincard"] = "2";                                                       // indicador de cargo/descuento => 2=No aplica cargo/descuento
                row["Icodcde"] = "";
                row["Ifcades"] = "";
                row["Imoncde"] = "";
                row["Imobacd"] = "";
                row["Iotrtas"] = "";                                                        // otros tributos tasa del tributo
                //row["Iotrsis"] = "";                                                        // otros tributos tipo de sistema
                //row["Ivalvta"] = Math.Round(double.Parse(row["Ipreuni"].ToString()),10).ToString("#0.00");       // Valor de venta del ítem
                row["Ivalvta"] = Math.Round(double.Parse(row["Ivaluni"].ToString()), 10).ToString("#0.00");       // Valor de venta del ítem
                retorna = true;
                tdfe.Rows.Add(row);
            }
            return retorna;
        }
        private bool generaTxt(string tipdo, string serie, string corre, string file_path)
        {
            bool retorna = false;
            DataRow row = tcfe.Rows[0];

            char sep = (char)31;
            StreamWriter writer;
            file_path = file_path + ".txt";
            writer = new StreamWriter(file_path);
            writer.WriteLine("CONTROL" + sep + "31007" + sep);
            writer.WriteLine("ENCABEZADO" + sep +
                "" + sep +                                      // 2 id del erp emisor
                row["_tipdoc"] + sep +                          // 3 Tipo de Comprobante Electrónico
                row["_sercor"] + sep +                          // 4 Numeración de Comprobante Electrónico
                row["_fecemi"] + sep +                          // 5 Fecha de emisión
                "" + sep +                                      // 6 Hora de emision V.31006
                row["_moneda"] + sep +                          // 7 Tipo de moneda
                "" + sep + "" + sep + "" + sep +                // 8,9,10, tcambio, vendedor, unidad de negocio
                row["tipOper"] + sep +                          // 11 Tipo de Operación
                "" + sep + "" + sep + "" + sep +                // 12,13,14 monto anticipos, numero, ruc emisor,
                "" + sep + "" + sep + "" + sep +                // 15,16,17 total anticipos
                "" + sep + "" + sep + "" + sep + "" + sep +     // 18,19,20,21 Tipo de nota(Crédito/Débito),Tipo del documento afectado,Numeración de documento afectado,Motivo del documento afectado
                row["conPago"] + sep +                          // 22 Condición de Pago
                row["plaPago"] + sep +                          // 23 Plazo de Pago
                row["fvencto"] + sep +                          // 24 Fecha de vencimiento
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +   // Forma de Pago del 1 al 6
                "" + sep + "" + sep +                           // 31,32 Número del pedido, Número de la orden de compra
                "" + sep + "" + sep + "" + sep + "" + sep +     // 33,34,35,36 sector publico: Numero de Expediente,Código de unidad ejecutora, Nº de contrato,Nº de proceso de selección
                row["_codgui"] + sep + row["_scotro"] + sep +   // 37,38 tipo de guia y serie+numero
                "" + sep + "" + sep +                           // 39,40 Tipo otro doc relacionado, numero doc relacionado
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +  // varios campos opcionales
                "" + sep +                                      // 48 pais de uso si es 0201 o 0208 V.3006
                row["obser1"] + sep + row["obser2"] + sep + "" + sep +    // 49,50,51 observaciones del documento 1 y 2
                row["_totogr"] + sep +                          // 52 Total operaciones gravadas
                row["_totoin"] + sep +                          // 53 Total operaciones inafectas
                row["_totoex"] + sep +                          // 54 total operaciones exoneradas
                "0.00" + sep +                                  // 55 Total operaciones exportacion
                "0.00" + sep +                                  // 56 total operaciones gratuitas gratuitas
                "0.00" + sep +                                  // 57 monto impuestos operaciones gratuitas V.3006
                "" + sep +                                      // 58 Monto Fondo Inclusión Social Energético FISE
                row["totImp"] + sep +                           // 59 Total IGV
                row["_toisc"] + sep +                           // 60 Total ISC
                "" + sep + "" + sep + "" + sep + "" + sep + "" + sep +  // 61,62,63,64,65  indicador imp,cod.motivo,factor dscto,monto dscto,monto base
                "" + sep + "0.00" + sep + "0.00" + sep +        // 66,67,68 Total otros tributos,Total otros cargos
                "0.00" + sep +                                  // 69 Descuento Global
                "0.00" + sep +                                  // 70 Total descuento
                row["_totven"] + sep +                          // 71 Importe total de la venta
                "" + sep +                                      // 72 monto para redondeo del importe total V.3006
                row["monLet"] + sep +                           // 73 Leyenda: Monto expresado en Letras
                "" + sep +                                      // 74 Leyenda: Transferencia gratuita o servicio prestado gratuitamente
                "" + sep +                                      // 75 Leyenda: Bienes transferidos en la Amazonía
                "" + sep +                                      // 76 Leyenda: Servicios prestados en la Amazonía
                "" + sep +                                      // 77 Leyenda: Contratos de construcción ejecutados en la Amazonía
                "" + sep + "" + sep + "" + sep);                // 78,79,80 Leyenda: Exoneradas,Leyenda: Inafectas,Leyenda: Emisor itinerante
            if (row["_forpa"].ToString() == "Credito")
            {
                string fansi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);
                string _fechc = DateTime.Parse(fansi).AddDays(double.Parse(tx_dat_dpla.Text)).Date.ToString("yyyy-MM-dd");        // fecha de emision + dias plazo credito
                writer.WriteLine("ENCABEZADO-CREDITO" + sep +
                    row["_totven"] + sep                        // OJO, las ventas a credito, son totales, no tenemos pagos parciales facturados, factura = pago total
                    );
                writer.WriteLine("DETALLE-CREDITO" + sep +
                    "Cuota001" + sep +                          // 2 numero de cuota
                    row["_totven"] + sep +                      // 3 monto de la cuota
                    _fechc + sep                                // 4 fecha del pago
                    );
            }
            //
            // datos del traslados de bienes
            // 
            writer.WriteLine("ENCABEZADO-EMISOR" + sep +
                row["Prucpro"] + sep +                          // 2 ruc emisor
                row["Prazsoc"] + sep +                          // 3 razon social emisor
                row["Pnomcom"] + sep +                          // 4 nombre comercial emisor
                row["paisEmi"] + sep +                          // 5 pais del emisor
                row["ubigEmi"] + sep +                          // 6 ubigeo del emisor
                row["Pdf_dep"] + sep +                          // 7 Departamento
                row["Pdf_pro"] + sep +                          // 8 Provincia
                row["Pdf_dis"] + sep +                          // 9 Distrito
                row["Pdf_urb"] + sep +                          // 10 Urbanización
                row["Pdf_dir"] + sep +                          // 11 Dirección detallada
                "" + sep +                                      // 12 Punto de emisión ... aca deberia ser la serie asignada por sunat al local emisor
                "" + sep +                                      // 13 Dirección de emisión ... aca deberia ir la direc del local emisor
                row["codLocE"] + sep +                          // 14 codigo local anexo sunat
                row["Ptelef1"] + sep +                          // 15 Teléfono
                "" + sep +                                      // 16 Fax
                row["corclie"] + sep);                          // 17 Correo-Emisor
            if (row["Ctipdoc"].ToString() == "0") row["Cnumdoc"] = "";
            writer.WriteLine("ENCABEZADO-RECEPTOR" + sep +
                row["Ctipdoc"] + sep +                          // 2 Tipo de documento del cliente
                row["Cnumdoc"] + sep +                          // 3 Nro. Documento del cliente
                row["Cnomcli"] + sep +                          // 4 Razón social del cliente
                "" + sep +                                      // 5 Identificador del cliente
                "" + sep +                                      // 6 Tipo de documento del receptor  V.3006 
                "" + sep +                                      // 7 Numero de documento del receptor  V.3006 
                row["paisAdq"] + sep +                          // 8 Código país
                row["ubigAdq"] + sep +                          // 9 Ubigeo
                row["depaAdq"] + sep +                          // 10 Departamento
                row["provAdq"] + sep +                          // 11 Provincia
                row["distAdq"] + sep +                          // 12 Distrito
                "" + sep +                                      // 13 Urbanización   dir2Adq
                row["dir1Adq"] + sep +                          // 14 Dirección
                row["maiAdq"] + sep);                           // 15 Correo-Receptor
            //
            // datos de percepcion
            // datos de retencion
            // datos de anticipos
            // 
            if (row["totdet"].ToString() != "0")
            {
                writer.WriteLine("ENCABEZADO-DETRACCION" + sep +
                    row["d_porde"] + sep +                      // 2 porcentaje de detraccion
                    row["totdet"] + sep +                       // 3 valor de la detraccion
                    row["d_codse"] + sep +                      // 4 codigo de servicio
                    row["d_ctade"] + sep +                      // 5 cuenta detraccion BN
                    row["d_medpa"] + sep +                      // 6 medio de pago
                    row["glosdet"] + sep);                      // 7 leyenda de la detración
            }
            // ***** DETALLE ***** //
            foreach (DataRow rdrow in tdfe.Rows)
            {
                writer.WriteLine(
                    "ITEM" + sep +
                    rdrow["Inumord"] + sep +                    // 2 orden
                    rdrow["Idatper"] + sep +                    // 3 Datos personilazados del item      
                    rdrow["Iumeded"] + sep +                    // 4 Unidad de medida                    3
                    rdrow["Icantid"] + sep +                    // 5 Cantidad de items             n(12,2)
                    rdrow["Idescri"] + sep +                    // 6 Descripcion                       500
                    rdrow["Idesglo"] + sep +                    // 7 descricion de la glosa del item   250
                    rdrow["Icodprd"] + sep +                    // 8 codigo del producto del cliente    30
                    rdrow["Icodpro"] + sep +                    // 9 codigo del producto SUNAT           8
                    rdrow["Icodgs1"] + sep +                    // 10 codigo del producto GS1           14
                    rdrow["Icogtin"] + sep +                    // 11 tipo de producto GTIN             14
                    rdrow["Inplaca"] + sep +                    // 12 numero placa de vehiculo
                    rdrow["Ivaluni"] + sep +                    // 13 Valor unitario del item SIN IMPUESTO 
                    rdrow["Ipreuni"] + sep +                    // 14 Precio de venta unitario CON IGV
                    rdrow["Ivalref"] + sep +                    // 15 valor referencial del item cuando la venta es gratuita
                    rdrow["Iigvite"] + sep +                     // 16 monto igv   .. ."_msigv"
                    rdrow["Icatigv"] + sep +                    // 17 tipo/codigo de afectacion igv
                    rdrow["Itasigv"] + sep +                    // 18 tasa del igv
                    rdrow["Iigvite"] + sep +                    // 19 monto IGV del item
                    rdrow["Icodtri"] + sep +                    // 20 codigo del tributo por item
                    rdrow["Iiscmba"] + sep +                    // 21 ISC monto base
                    rdrow["Iisctas"] + sep +                    // 22 ISC tasa del tributo
                    rdrow["Iisctip"] + sep +                    // 23 ISC tipo de afectacion
                    rdrow["Iiscmon"] + sep +                    // 24 ISC monto del tributo
                    rdrow["Icbper1"] + sep +                    // 25 indicador de afecto a ICBPER
                    rdrow["Icbper2"] + sep +                    // 26 monto unitario de ICBPER
                    rdrow["Icbper3"] + sep +                    // 27 monto total ICBPER del item
                    rdrow["Iotrtri"] + sep +                    // 28 otros tributos monto base
                    rdrow["Iotrtas"] + sep +                    // 29 otros tributos tasa del tributo
                    rdrow["Iotrlin"] + sep +                    // 30 otros tributos monto unitario
                    rdrow["Itdscto"] + sep +                    // 31 Descuentos por ítem
                    rdrow["Iincard"] + sep +                    // 32 indicador de cargo/descuento
                    rdrow["Icodcde"] + sep +                    // 33 codigo de cargo/descuento
                    rdrow["Ifcades"] + sep +                    // 34 Factor de cargo/descuento
                    rdrow["Imoncde"] + sep +                    // 35 Monto de cargo/descuento
                    rdrow["Imobacd"] + sep +                    // 36 Monto base del cargo/descuento
                    rdrow["Ivalvta"] + sep);                    // 37 Valor de venta del ítem
            }
            writer.Flush();
            writer.Close();
            retorna = true;
            return retorna;
        }
        private bool baja2TXT(string tipdo, string _fecemi, string _codbaj, string _secuen, string file_path, int cuenta, string serie, string corre)
        {
            bool retorna = false;

            string Prazsoc = nomclie.Trim();                                            // razon social del emisor
            string Prucpro = Program.ruc;                                               // Ruc del emisor
            string Pcrupro = "6";                                                       // codigo Ruc emisor
            string motivo = glosaAnul;      // "ANULACION";
            string fecdoc = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2);   // fecha de emision   yyyy-mm-dd
            /* ********************************************** GENERAMOS EL TXT de baja   ************************************* */
            //string sep = "|";
            char sep = (char)31;
            StreamWriter writer;
            file_path = file_path + ".txt";
            writer = new StreamWriter(file_path);
            writer.WriteLine("CONTROL" + sep + "31001");
            writer.WriteLine("ENCABEZADO" + sep +
                "" + sep +                      // 2 Id del comprobante erp emisor
                "RA" + sep +                    // 3 tipo de comprobante
                Prucpro + sep +                 // 4 ruc emisor
                Prazsoc + sep +                 // 5 razon social emisor
                _codbaj + "-" + _secuen + sep +       // 6 codigo identificador de la baja, secuencial dentro de cada día
                _fecemi + sep +                 // 7 fecha de la baja  
                fecdoc + sep +                  // 8 fecha del documento dado de baja
                Program.mailclte +              // 9 correo del emisor
                "" + sep                        // 10 correo del receptor
            );
            writer.WriteLine("ITEM" + sep +
                "1" + sep +
                tipdo + sep +
                serie + sep +
                corre + sep +
                motivo + sep
            );
            writer.Flush();
            writer.Close();
            retorna = true;

            return retorna;
        }
        #endregion
        
        #region json_facturacion
        private string json_venta(string tipdo, string tipoDocClte)
        {
            string retorna = "";
            int cta_ron = 1;            // contador filas de detalle
            string d_medpa, d_conpa, d_valde, d_ctade;
            decimal totdet = 0, valcre = decimal.Parse(tx_flete.Text);
            string tipOper = s_tipOpeN;     // "0101";    // operacion venta interna
            string v_hor_em = lib.Right(DateTime.UtcNow.ToLocalTime().ToString(), 8);
            string notaGuias = "";      // observación de las guias de remision transportista
            //
            Cdetracc cdetracc = null;
            List<Cleyen> lll = new List<Cleyen>();    // 01/02/2024  List<Cleyen> lll = null;
            Cleyen cleyen = new Cleyen()
            {
                leyen_cod = "1000",
                leyen_descrip = tx_fletLetras.Text.Trim()
            };
            lll.Add(cleyen);     // lll.Insert(1, cleyen);        // 01/02/2024
            // Detracción - leyenda de detracción - transp. de carga y tramo en detalle
            Ctramo ctramo = null;
            Ctransp_carga ctransp_Carga = null; // 08/02/2024 acá debe ser el calculo en base al valor en soles del flete
            if (double.Parse(tx_fletMN.Text) > double.Parse(Program.valdetra) && tx_dat_tdv.Text == codfact) // double.Parse(tx_flete.Text) > double.Parse(Program.valdetra) && tx_dat_tdv.Text == codfact
            {
                d_medpa = "001";                                    // medio de pago de la detraccion (001 = deposito en cuenta)
                d_conpa = "CONTADO";                                // condicion de pago
                d_valde = Program.valdetra;                         // valor de la detraccion
                d_ctade = Program.ctadetra;                         // cuenta detraccion BN
                totdet = Math.Round(decimal.Parse(tx_flete.Text) * decimal.Parse(Program.pordetra) / 100, 2);    // totalDetraccion
                valcre = Math.Round((decimal.Parse(tx_flete.Text) - totdet), 2);               // cuota credito = valor - detraccion
                decimal totdetSol = 0;
                if (tx_dat_mone.Text == MonDeft) totdetSol = totdet;
                else totdetSol = Math.Round(totdet * decimal.Parse(tx_tipcam.Text),0);

                tipOper = s_tipOpeDTC;              // "1001";      // operación venta interna sujeta a detracción de transporte de carga
                glosdet = glosdet + " " + d_ctade;                  // leyenda de la detración
                //
                cdetracc = new Cdetracc()
                {
                    porcent = decimal.Parse(Program.pordetra),
                    cod = Program.coddetra,
                    monto = totdetSol,
                    cod_bn = Program.ctadetra,
                    med_pago = d_medpa,
                    cod_mon = "PEN"                                    // moneda de la detraccion
                };
                lll = new List<Cleyen>();
                lll.Add(cleyen);
                cleyen = new Cleyen
                {
                    leyen_cod = "2006",
                    leyen_descrip = glosdet
                };
                lll.Add(cleyen);
            }
            List<CComprobanteDetalle> aaa = new List<CComprobanteDetalle>();
            List<CComprobDetDetrac> ddd = new List<CComprobDetDetrac>();
            Ctramo tramito = new Ctramo()
            {
                cod_ubi_ori = tx_dat_upo.Text,
                dir_ori = tx_dat_dpo.Text,
                cod_ubi_des = tx_dat_upd.Text,
                dir_des = tx_dat_dpd.Text,
                descrip = dataGridView1.Rows[0].Cells[10].Value.ToString(),     // DESCRIPCION DE TRAMO
                val_pre_ref_carga_efectiva = 1,
                conf_vehi = "-",
                carga_util = 1,
                carga_efectiva = 1,
                val_ref_tne_metri = 1,
                val_pre_ref_carga_util = 1,
                retorno_vacio = true
            };
            List<Ctramo> ctramos = new List<Ctramo>();
            ctramos.Add(tramito);
            Ctransp_carga ctransp = new Ctransp_carga()
            {
                cod_ubi_ori = datguias[cta_ron - 1, 18],     // tx_dat_upo.Text,
                dir_ori = datguias[cta_ron - 1, 19],         // tx_dat_dpo.Text,
                cod_ubi_des = datguias[cta_ron - 1, 20],     // tx_dat_upd.Text,
                dir_des = datguias[cta_ron - 1, 21],         // tx_dat_dpd.Text,
                nota = "Transporte consolidado",
                val_ref_transporte = 1,
                val_ref_carga_efectiva = 1,
                val_ref_carga_util = 1,
                tramo = ctramos
            };
            List<CDocref> cdocref = new List<CDocref>();
            foreach (DataGridViewRow ron in dataGridView1.Rows)
            {
                if (ron.Cells[1].Value != null)
                {
                    /* CDocref docref = new CDocref()           comentado el 18/03/2023 porque la validacion de sunat arroja una observacion con las guias electrónicas
                    {
                        tip_doc = "31",
                        serie_correl = ron.Cells["guias"].Value.ToString()
                    };
                    cdocref.Add(docref);
                    */
                    notaGuias = ron.Cells["guias"].Value.ToString() + " ";      // 18/03/2024 las guias pasan a notas en lugar de documento referente
                    double vval_f = 0;      // Math.Round(double.Parse(ron.Cells["valor"].Value.ToString()));
                    if (ron.Cells["codmondoc"].Value.ToString() == MonDeft && tx_dat_mone.Text == MonDeft)
                    {
                        vval_f = double.Parse(ron.Cells["valor"].Value.ToString());
                    }
                    if (ron.Cells["codmondoc"].Value.ToString() == MonDeft && tx_dat_mone.Text != MonDeft)
                    {
                        vval_f = double.Parse(ron.Cells["valor"].Value.ToString()) / double.Parse(tx_tipcam.Text);
                    }
                    if (ron.Cells["codmondoc"].Value.ToString() != MonDeft && tx_dat_mone.Text == MonDeft)
                    {
                        vval_f = double.Parse(ron.Cells["valor"].Value.ToString()) * double.Parse(tx_tipcam.Text);
                    }
                    if (ron.Cells["codmondoc"].Value.ToString() != MonDeft && tx_dat_mone.Text != MonDeft)
                    {
                        vval_f = double.Parse(ron.Cells["valor"].Value.ToString());
                    }
                    if (cdetracc == null)           // comprobante sin detracción
                    {
                        CComprobanteDetalle det = new CComprobanteDetalle
                        {
                            nro_item = cta_ron,         // solo val_unit_item, prec_unit_item y val_ref_unit_item puede tener hasta 10 decimales,
                                                        //cod_prod = "",            //  val_ref_unit_item solo debe ir cuando es venta gratuita.
                            cod_und_med = "ZZ",         // el resto solo hasta 2 decimales.
                            descrip = glosser + " " + ron.Cells["Descrip"].Value.ToString() + " " + glosser2,
                            cant = 1,
                            val_unit_item = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 10),
                            sub_tot = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),
                            dsc_item = 0,
                            val_vta_item = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),       // valor venta x item
                            igv_item = Math.Round(vval_f - (vval_f / (1 + (double.Parse(v_igv) / 100))), 2),
                            //isc_item = 0,           // Sistema de ISC por ítem
                            prec_unit_item = Math.Round(vval_f,2),
                            tip_afec_igv = "10",    // Afectación al IGV por ítem
                            impsto_tot = Math.Round(vval_f - (vval_f / (1 + (double.Parse(v_igv) / 100))), 2),          // Monto total de impuestos del ítem
                            base_igv = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),           // Monto Base IGV/IVAP
                            tasa_igv = int.Parse(v_igv),          // Tasa del IGV/IVAP
                            ind_grat = "N",
                            cod_prod_sunat = null,      // no usamos codificación estandarizada
                            cod_prod_gs1 = null,        // no usamos este codigo
                            tip_prod_gtin = null,       // no usamos este codigo
                        };      // detalle del comprob sin detracción
                        aaa.Add(det);
                    }
                    else
                    {
                        CComprobDetDetrac det = new CComprobDetDetrac()
                        {
                            nro_item = cta_ron,
                            cod_und_med = "ZZ",
                            descrip = glosser + " " + ron.Cells["Descrip"].Value.ToString() + " " + glosser2,
                            cant = 1,
                            val_unit_item = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 10),
                            sub_tot = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),
                            dsc_item = 0,
                            val_vta_item = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),
                            igv_item = Math.Round(vval_f - (vval_f / (1 + (double.Parse(v_igv) / 100))), 2),
                            prec_unit_item = Math.Round(vval_f,2),
                            tip_afec_igv = "10",
                            impsto_tot = Math.Round(vval_f - (vval_f / (1 + (double.Parse(v_igv) / 100))), 2),          // Monto total de impuestos del ítem
                            base_igv = Math.Round(vval_f / (1 + (double.Parse(v_igv) / 100)), 2),           // Monto Base IGV/IVAP
                            tasa_igv = int.Parse(v_igv),
                            ind_grat = "N",
                            transp_carga = ctransp
                        };                            // comprobante con detracción y por lo tanto tiene que tener detalle de transp. de carga
                        ddd.Add(det);
                    }
                    cta_ron += 1;
                }
            }
            cleyen = new Cleyen
            {
                leyen_descrip = "Guía de Remisión Transportista: " + notaGuias
            };
            lll.Add(cleyen);
            Cemisor cemisor = new Cemisor()
            {
                tip_doc = "6",
                num_doc = Program.ruc,
                raz_soc = Program.cliente,
                //nom_comer = "",
                dir = Program.dirfisc,
                cod_ubi = Program.ubidirfis,
                dep = Program.depfisc,
                prov = Program.provfis,
                dist = Program.distfis,
                cod_pais = "PE",
                email = Program.mailclte,
                telef = Program.telclte1,
                website = Program.webclte1,
                cod_sucur = Program.codlocsunat,                // "0000" en pruebas, en producción regresarlo a su estado normal 07/02/2024
            };
            Cadquiriente cadquiriente = new Cadquiriente()
            {
                tip_doc = tipoDocClte,
                num_doc = tx_numDocRem.Text,
                raz_soc = tx_nomRem.Text.Replace(Environment.NewLine,string.Empty),
                dir = tx_dirRem.Text.Replace(Environment.NewLine, string.Empty),
                cod_pais = "PE",
                cod_sucur = "0000",
                email = tx_email.Text,
                nom_comer = null,
                cod_ubi = tx_ubigRtt.Text,
                dist = tx_distRtt.Text,
                prov = tx_provRtt.Text,
                dep = tx_dptoRtt.Text,
                telef = (tx_telc1.Text.Trim() == "")? null : tx_telc1.Text,
                website = null
            };
            Ctot ctot = new Ctot()
            {
                grav = decimal.Parse(tx_subt.Text),
                val_vent = decimal.Parse(tx_subt.Text),
                //inaf = 0,     // no hacemos
                //exo = 0,      // operaciones exoneradas
                //grat = 0,     // o gratuitas en TRANSPORTE DE CARGA
                igv = decimal.Parse(tx_igv.Text),
                imp_tot = decimal.Parse(tx_flete.Text),
                impsto_tot = decimal.Parse(tx_igv.Text),
                prec_tot = decimal.Parse(tx_flete.Text),
                redondeo = 0
            };
            Cforma_pago formap = new Cforma_pago()
            {
                cod_mon = tipoMoneda,     // tx_dat_monsunat.Text
                monto_neto = valcre,   // decimal.Parse(tx_flete.Text),
                //descrip = (rb_contado.Checked == true) ? "Contado" : (rb_credito.Checked == true)? "Credito" : "Contado"
                descrip = (tx_dat_plazo.Text.Trim() == "") ? "Contado" : "Credito"
            };
            List<CCuota> ccc = null;
            if (tx_dat_plazo.Text.Trim() != "")     // rb_credito.Checked == true
            {
                ccc = new List<CCuota>();
                // en Transcarga los créditos son solo de una cuota 29/01/2024
                CCuota cuot = new CCuota()
                {
                    descrip = "Cuota001",
                    monto_neto = valcre,
                    cod_mon = tipoMoneda,   // tx_dat_monsunat.Text,
                    fec_venc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd")
                };
                ccc.Add(cuot);
            }

            if (tx_dat_plazo.Text.Trim() == "" && cdetracc == null)        // rb_credito.Checked == false && cdetracc == null
            {
                CComprobante1 comprobante = new CComprobante1
                {
                    tip_doc = tipdo,
                    serie = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3),
                    correl = tx_numero.Text,
                    fec_emi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    cod_mon = tipoMoneda,
                    tip_oper = tipOper,
                    fec_venc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd"),
                    hora_emi = v_hor_em,             
                    cod_mon_ref = "PEN",            // tx_dat_monsunat.Text
                    cod_mon_obj = tipoMoneda,       // tx_dat_monsunat.Text
                    //factor = ((tx_tipcam.Text.Trim() == "") ? null : tx_tipcam.Text),
                    fec_tipo_cambio = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    ubl_version = "2.1",
                    customizacion = "2.0",
                    emisor = cemisor,
                    adquiriente = cadquiriente,
                    tot = ctot,
                    forma_pago = formap,
                    det = aaa,
                    leyen = lll,
                    docref = cdocref
                };
                Cinvoice1 cinvoice = new Cinvoice1
                {
                    invoice = comprobante
                };
                retorna = JsonConvert.SerializeObject(cinvoice, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }        // comprobante clase 1
            if (tx_dat_plazo.Text.Trim() == "" && cdetracc != null)        // rb_credito.Checked == false && cdetracc != null
            {
                CComprobante3 comprobante3 = new CComprobante3
                {
                    tip_doc = tipdo,
                    serie = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3),
                    correl = tx_numero.Text,
                    fec_emi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    cod_mon = tipoMoneda,
                    tip_oper = tipOper,
                    fec_venc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd"),
                    hora_emi = v_hor_em,
                    cod_mon_ref = "PEN",            // tx_dat_monsunat.Text
                    cod_mon_obj = tipoMoneda,       // tx_dat_monsunat.Text
                    //factor = ((tx_tipcam.Text.Trim() == "") ? null : tx_tipcam.Text),
                    fec_tipo_cambio = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    ubl_version = "2.1",
                    customizacion = "2.0",
                    emisor = cemisor,
                    adquiriente = cadquiriente,
                    tot = ctot,
                    forma_pago = formap,
                    detracc = cdetracc,
                    det = ddd,
                    leyen = lll,
                    docref = cdocref
                };
                Cinvoice3 cinvoice = new Cinvoice3
                {
                    invoice = comprobante3
                };
                retorna = JsonConvert.SerializeObject(cinvoice, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }        // comprobante clase 3
            if (tx_dat_plazo.Text.Trim() != "" && cdetracc == null)      // rb_credito.Checked == true && cdetracc == null
            {
                CComprobante4 comprobante = new CComprobante4
                {
                    tip_doc = tipdo,
                    serie = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3),
                    correl = tx_numero.Text,
                    fec_emi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    cod_mon = tipoMoneda,
                    tip_oper = tipOper,
                    fec_venc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd"),
                    hora_emi = v_hor_em,
                    cod_mon_ref = "PEN",            // tx_dat_monsunat.Text
                    cod_mon_obj = tipoMoneda,       // tx_dat_monsunat.Text
                    //factor = ((tx_tipcam.Text.Trim() == "") ? null : tx_tipcam.Text),
                    fec_tipo_cambio = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    ubl_version = "2.1",
                    customizacion = "2.0",
                    emisor = cemisor,
                    adquiriente = cadquiriente,
                    tot = ctot,
                    forma_pago = formap,
                    cuota = ccc,
                    det = aaa,
                    leyen = lll,
                    docref = cdocref
                };
                Cinvoice4 cinvoice = new Cinvoice4
                {
                    invoice = comprobante
                };
                retorna = JsonConvert.SerializeObject(cinvoice, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }        // comprobante clase 4
            if (tx_dat_plazo.Text.Trim() != "" && cdetracc != null)         // rb_credito.Checked == true && cdetracc != null
            {
                CComprobante6 comprobante6 = new CComprobante6
                {
                    tip_doc = tipdo,
                    serie = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3),
                    correl = tx_numero.Text,
                    fec_emi = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    cod_mon = tipoMoneda,
                    tip_oper = tipOper,
                    fec_venc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse((tx_dat_dpla.Text == "") ? "0" : tx_dat_dpla.Text)).ToString("yyyy-MM-dd"),
                    hora_emi = v_hor_em,
                    cod_mon_ref = "PEN",            // tx_dat_monsunat.Text
                    cod_mon_obj = tipoMoneda,       // tx_dat_monsunat.Text
                    //factor = ((tx_tipcam.Text.Trim() == "") ? null : tx_tipcam.Text),
                    fec_tipo_cambio = tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2),
                    ubl_version = "2.1",
                    customizacion = "2.0",
                    emisor = cemisor,
                    adquiriente = cadquiriente,
                    tot = ctot,
                    forma_pago = formap,
                    cuota = ccc,
                    detracc = cdetracc,
                    det = ddd,
                    leyen = lll,
                    docref = cdocref
                };
                Cinvoice6 cinvoice = new Cinvoice6
                {
                    invoice = comprobante6
                };
                retorna = JsonConvert.SerializeObject(cinvoice, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }        // comprobante clase 6
            return retorna;
        }
        private string json_baja(string _fecdoc, string indentif, string _fecemi, string tipDComp)
        {
            string retorna = "";
            Cemisor cemisor = new Cemisor()
            {
                tip_doc = "6",
                num_doc = Program.ruc,
                raz_soc = Program.cliente,
            };
            List<CdetBaja> cdets = new List<CdetBaja>();
            CdetBaja deta = new CdetBaja()
            {
                nro_item = 1,
                tip_doc = tipDComp,
                serie = cmb_tdv.Text.Substring(0, 1) + lib.Right(tx_serie.Text, 3),
                correl = tx_numero.Text,
                motivo = glosaAnul
            };
            cdets.Add(deta);
            Ccpe cbaja = new Ccpe()
            {
                fec_ref = _fecdoc,
                identificador = indentif,
                fec_gen = _fecemi,
                emisor = cemisor,
                det = cdets
            };
            CinvoiceA cinvoice = new CinvoiceA
            {
                baja = cbaja
            };
            return retorna = JsonConvert.SerializeObject(cinvoice, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        #endregion

        #endregion

        #region autocompletados
        private void autodepa()                 // se jala en el load
        {
            if (dataUbig == null)
            {
                //DataTable dataUbig = (DataTable)CacheManager.GetItem("ubigeos");
                using (MySqlConnection conn =  new MySqlConnection(DB_CONN_STR))
                {
                    if (lib.procConn(conn) == true)
                    {
                        // tabla de ubigeos - departamentos, provincias, distritos
                        string consulta = "select * from ubigeos";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                            {
                                DataTable dtu = new DataTable();
                                da.Fill(dtu);
                                CacheManager.AddItem("ubigeos", dtu, 3600);
                            }
                        }
                    }
                }
            }
            DataRow[] depar = dataUbig.Select("depart<>'00' and provin='00' and distri='00'");
            departamentos.Clear();
            foreach (DataRow row in depar)
            {
                departamentos.Add(row["nombre"].ToString());
            }
        }
        private void autoprov()                 // se jala despues de ingresado el departamento
        {
            if (tx_dptoRtt.Text.Trim() != "")
            {
                DataRow[] provi = dataUbig.Select("depart='" + tx_ubigRtt.Text.Substring(0, 2) + "' and provin<>'00' and distri='00'");
                provincias.Clear();
                foreach (DataRow row in provi)
                {
                    provincias.Add(row["nombre"].ToString());
                }
            }
        }
        private void autodist()                 // se jala despues de ingresado la provincia
        {
            if (tx_ubigRtt.Text.Trim() != "" && tx_provRtt.Text.Trim() != "")
            {
                DataRow[] distr = dataUbig.Select("depart='" + tx_ubigRtt.Text.Substring(0, 2) + "' and provin='" + tx_ubigRtt.Text.Substring(2, 2) + "' and distri<>'00'");
                distritos.Clear();
                foreach (DataRow row in distr)
                {
                    distritos.Add(row["nombre"].ToString());
                }
            }
        }
        #endregion autocompletados

        #region limpiadores_modos
        private void sololee()
        {
            lp.sololee(this);
        }
        private void escribe()
        {
            lp.escribe(this);
            tx_nomRem.ReadOnly = true;
            tx_serie.ReadOnly = true;
            tx_ubigRtt.ReadOnly = true;
            //tx_dirRem.ReadOnly = true;
            //tx_dptoRtt.ReadOnly = true;
            //tx_provRtt.ReadOnly = true;
            //tx_distRtt.ReadOnly = true;
        }
        private void limpiar()
        {
            lp.limpiar(this);
        }
        private void limpia_chk()    
        {
            lp.limpia_chk(this);
        }
        private void limpia_otros()
        {
            //
        }
        private void limpia_combos()
        {
            lp.limpia_cmb(this);
            cmb_plazoc.SelectedIndex = -1;
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA
        private void bt_agr_Click(object sender, EventArgs e)
        {
            if (tx_serGR.Text.Trim() != "" && tx_numGR.Text.Trim() != "" && Tx_modo.Text == "NUEVO")
            {
                int fila = (dataGridView1.Rows.Count - 1);          // numero de filas de la grilla detalle
                // validamos que la GR: 1.exista, 2.No este facturada, 3.No este anulada
                if (validGR(tx_serGR.Text, tx_numGR.Text, fila) == false)
                {
                    MessageBox.Show("La GR no existe, esta anulada o ya esta facturada", "Error en Guía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tx_numGR.Text = "";
                    tx_numGR.Focus();
                    return;
                }
                else
                {
                    if (datguias[fila,1].Trim() == "")   // datguias[1] = (dr.IsDBNull(20)) ? "" : dr.GetString("descrip");         // descrip = descprodi
                    {
                        MessageBox.Show("La GR tiene el detalle incompleto", "Error en Guía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tx_numGR.Text = "";
                        tx_numGR.Focus();
                        return;
                    }
                    rb_desGR.PerformClick();
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() == (tx_serGR.Text.Trim() + "-" + tx_numGR.Text.Trim()))
                        {
                            MessageBox.Show("Esta repitiendo la Guía!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tx_numGR.Text = "";
                            tx_numGR.Focus();
                            return;
                        }
                        // validamos si la guia que ya esta tiene saldo o no y es = a la guia que se va a insertar
                        if (decimal.Parse(dataGridView1.Rows[i].Cells[12].Value.ToString()) > 0 && decimal.Parse(datguias[fila, 17]) == 0 ||
                            decimal.Parse(dataGridView1.Rows[i].Cells[12].Value.ToString()) == 0 && decimal.Parse(datguias[fila, 17]) > 0)
                        {
                            MessageBox.Show("Todas las guías deben estar cobradas o no cobradas", "Error, las GR deben o no tener saldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tx_numGR.Text = "";
                            tx_numGR.Focus();
                            return;
                        }
                    }
                }
                //dataGridView1.Rows.Clear(); nooooo, se puede hacer una fact de varias guias, n guias
                dataGridView1.Rows.Add(datguias[fila, 0], datguias[fila, 1], datguias[fila, 2], datguias[fila, 3], datguias[fila, 4], datguias[fila, 5], datguias[fila, 6], datguias[fila, 9], datguias[fila, 10], datguias[fila, 7], datguias[fila, 15], datguias[fila, 16], datguias[fila, 17]);     // insertamos en la grilla los datos de la GR
                int totfil = 0;
                int totcant = 0;
                decimal totflet = 0;    // acumulador en moneda de la GR
                tx_dat_mone.Text = datguias[fila, 7].ToString();
                cmb_mon.SelectedValue = datguias[fila, 7].ToString();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        totcant = totcant + int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        totfil += 1;
                        if (tx_dat_mone.Text != MonDeft)
                        {
                            totflet = totflet + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()); // VALOR DE LA GR EN MONEDA LOCAL
                        }
                        else
                        {
                            totflet = totflet + decimal.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()); // VALOR DE LA GR EN SU MONEDA
                        }
                    }
                }
                tx_tfmn.Text = totflet.ToString("#0.00");
                tx_totcant.Text = totcant.ToString();
                tx_tfil.Text = totfil.ToString();
                tx_flete.Text = totflet.ToString("#0.00");
                tx_fletMN.Text = totflet.ToString("#0.00"); // Math.Round(decimal.Parse(tx_flete.Text) * decimal.Parse(tx_tipcam.Text), 2).ToString();
                if (tx_dat_mone.Text != MonDeft && datguias[fila, 9].ToString().Substring(0,10) != tx_fechope.Text)
                {
                    // llamanos a tipo de cambio
                    vtipcam vtipcam = new vtipcam("", tx_dat_mone.Text, DateTime.Now.Date.ToString());
                    var result = vtipcam.ShowDialog();
                    if (vtipcam.ReturnValue1 != null && vtipcam.ReturnValue1 != "")
                    {
                        tx_flete.Text = vtipcam.ReturnValue1;
                        tx_fletMN.Text = vtipcam.ReturnValue2;
                        tx_tipcam.Text = vtipcam.ReturnValue3;
                        tx_fletMN.Text = Math.Round(decimal.Parse(tx_flete.Text) * decimal.Parse(tx_tipcam.Text), 2).ToString();
                    }
                }
                else
                {
                    tx_tipcam.Text = datguias[fila, 8].ToString();
                }
                if (int.Parse(tx_tfil.Text) == int.Parse(v_mfildet))
                {
                    MessageBox.Show("Número máximo de filas de detalle", "El formato no permite mas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView1.AllowUserToAddRows = false;
                }
                else
                {
                    dataGridView1.AllowUserToAddRows = true;
                }
                rb_no.Enabled = true;
                if (decimal.Parse(tx_dat_saldoGR.Text) <= 0)
                {
                    MessageBox.Show("La GR esta cancelada, el documento de venta"+ Environment.NewLine +
                         "se creará con el estado cancelado","Atención verifique",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    //rb_si.PerformClick();
                    rb_contado.Checked = true;
                    rb_credito.Enabled = false;
                    rb_contado.Enabled = false;
                    rb_si.Checked = false;
                    rb_no.Checked = false;
                    rb_si.Enabled = false;
                    rb_no.Enabled = false;
                    cmb_plazoc.SelectedIndex = -1;
                    cmb_plazoc.Enabled = false;
                    tx_salxcob.Text = "0";
                    tx_pagado.Text = "0";   // tx_flete.Text;
                }
                else
                {
                    tx_flete.ReadOnly = true;
                }
                tx_flete_Leave(null, null);
                chk_cunica.Checked = true; // cargaunica();                   // jalamos los datos del camion
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region validaciones
            if (tx_dat_tdv.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de comprobante", " Atención ");
                cmb_tdv.Focus();
                return;
            }
            if (tx_serie.Text.Trim() == "")
            {
                tx_serie.Focus();
                return;
            }
            if (tx_dat_mone.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el tipo de moneda", " Atención ");
                cmb_mon.Focus();
                return;
            }
            if (tx_flete.Text.Trim() == "" || tx_flete.Text.Trim() == "0")
            {
                MessageBox.Show("No existe valor del documento", " Atención ");
                tx_flete.Focus();
                return;
            }
            if (tx_tfil.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el detalle del documento de venta", "Faltan ingresar guías");
                tx_serGR.Focus();
                return;
            }
            if (tx_dat_tdRem.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione el documento de cliente", " Error en Cliente ");
                tx_dat_tdRem.Focus();
                return;
            }
            if (tx_numDocRem.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el número de documento", " Error en Cliente ");
                tx_numDocRem.Focus();
                return;
            }
            if (tx_nomRem.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre o razón social", " Error en Cliente ");
                tx_nomRem.Focus();
                return;
            }
            if (tx_dirRem.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la dirección", " Error en Remitente ");
                tx_dirRem.Focus();
                return;
            }
            if (tx_ubigRtt.Text.Trim().Length != 6)
            {
                MessageBox.Show("Registre correctamente el departamento, provincia y distrito", "Ubigeo incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_dptoRtt.Focus();
                return;
            }
            if (tx_dptoRtt.Text.Trim() == "" || tx_provRtt.Text.Trim() == "" || tx_distRtt.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese departamento, provincia y distrito", "Dirección incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_dptoRtt.Focus();
                return;
            }
            if (tx_email.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar un correo electrónico", " Error en Cliente ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tx_email.Focus();
                return;
            }
            if (tx_dat_tdec.Text != tx_dat_tdRem.Text)
            {
                // aca validamos que el tipo de doc de venta se corresponda con el documento del cliente
                if (tx_dat_tdv.Text != codfact)
                {
                    if (!tdocsBol.Contains(tx_dat_tdRem.Text))
                    {
                        MessageBox.Show("Asegurese que el tipo de documento de venta" + Environment.NewLine +
                            "sean coincidente con el tipo de cliente", "Error de tipos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmb_docRem.Focus();
                        return;
                    }
                }
                else
                {
                    if (!tdocsFac.Contains(tx_dat_tdRem.Text))
                    {
                        MessageBox.Show("Asegurese que el tipo de documento de venta" + Environment.NewLine +
                            "sean coincidente con el tipo de cliente", "Error de tipos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmb_docRem.Focus();
                        return;
                    }
                }
            }
            #endregion
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            if (modo == "NUEVO")
            {
                // valida contado o credito
                if (rb_contado.Checked == false && rb_credito.Checked == false)
                {
                    MessageBox.Show("Seleccione si el comprobante se mitirá" + Environment.NewLine +
                         "al Contado o al Crédito","Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                // valida pago y calcula
                if (rb_si.Checked == false && rb_no.Checked == false && rb_si.Enabled == true)
                {
                    MessageBox.Show("Seleccione si se cancela la factura o no","Atención - Confirme",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    rb_si.Focus();
                    return;
                }
                if (tx_pagado.Text.Trim() == "" && tx_salxcob.Text.Trim() == "")
                {
                    MessageBox.Show("Seleccione si se cancela la factura o no", "Atención - Confirme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rb_si.Focus();
                    return;
                }
                if (tx_dat_mone.Text != MonDeft && tx_tipcam.Text == "" || tx_tipcam.Text == "0")
                {
                    MessageBox.Show("Problemas con el tipo de cambio","Atención",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    cmb_mon.Focus();
                    return;
                }
                if (tx_dat_mone.Text != MonDeft && decimal.Parse(tx_tipcam.Text) > 1)
                {
                    /*
                    if (Math.Round(decimal.Parse(tx_tfmn.Text), 1) != Math.Round(decimal.Parse(tx_fletMN.Text), 1))
                    {
                        MessageBox.Show("El valor a facturar no puede ser diferente al valor de la(s) GR");
                        tx_flete.Focus();
                        return;
                    } */
                }
                if (fshoy != lib.fechCajaLoc(TransCarga.Program.almuser, codGene) && rb_si.Checked == true) // si la caja esta abierta permite cobrar sino NO!
                {
                    MessageBox.Show("No puede cobrar en automático","No existe caja abierta");
                    rb_no.PerformClick();
                }
                if (rb_si.Checked == true)
                {
                    tx_pagado.Text = tx_flete.Text;
                    tx_salxcob.Text = "0.00";
                    tx_salxcob.BackColor = Color.Green;
                }
                if (rb_no.Checked == true)
                {
                    tx_pagado.Text = "0.00";
                    tx_salxcob.Text = tx_flete.Text;
                    tx_salxcob.BackColor = Color.Red;
                    //cmb_plazoc.Enabled = true;
                    //cmb_plazoc.SelectedValue = codppc;
                    //tx_dat_plazo.Text = codppc;
                }
                if (tx_idr.Text.Trim() == "")
                {
                    var aa = MessageBox.Show("Confirma que desea crear el documento?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        // deshabilitamos el boton hasta volver a dar click en el boton nuevo
                        button1.Enabled = false;

                        if (lib.DirectoryVisible(rutatxt) == true)
                        {
                            if (graba() == true)
                            {
                                if (factElec(nipfe, true, "alta", 0, true) == true)
                                {
                                    // actualizamos la tabla seguimiento de usuarios
                                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                                    if (resulta != "OK")
                                    {
                                        MessageBox.Show(resulta, "Error en actualización de seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    //  TODO DOC.VTA. SE ENVIA A LA ETIQUETERA DE FRENTE ... 28/10/2020
                                    //  AL GRABAR SE ASUME IMPRESA 28/10/2020 ... ya no 13/12/2020
                                    var bb = MessageBox.Show("Desea imprimir el documento?" + Environment.NewLine +
                                        "El formato actual es " + vi_formato, "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (bb == DialogResult.Yes)
                                    {
                                        Bt_print.PerformClick();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No se puede generar el documento de venta electrónico" + Environment.NewLine +
                                        "Se generó una anulación interna para el presente documento", "Error en proveedor de Fact.Electrónica");
                                    iserror = "si";
                                    anula("INT");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se puede grabar el documento de venta electrónico", "Error en conexión");
                                iserror = "si";
                            }
                        }
                        else
                        {
                            MessageBox.Show("No existe ruta o no es valida para" + Environment.NewLine +
                                        "generar el comprobante electrónico", "Ruta para Fact.Electrónica", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            iserror = "si";
                        }
                    }
                    else
                    {
                        tx_numDocRem.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Los datos no son nuevos en doc.venta", "Verifique duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                if (tx_numero.Text.Trim() == "")
                {
                    tx_numero.Focus();
                    MessageBox.Show("Ingrese el número del documento", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (tx_dat_estad.Text == codAnul)
                {
                    MessageBox.Show("El documento esta ANULADO", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tx_numero.Focus();
                    return;
                }
                if (true)
                {
                    if (tx_idr.Text.Trim() != "")
                    {
                        var aa = MessageBox.Show("Confirma que desea modificar el documento?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            edita();    // modificacion total
                            // actualizamos la tabla seguimiento de usuarios
                            string resulta = lib.ult_mov(nomform, nomtab, asd);
                            if (resulta != "OK")
                            {
                                MessageBox.Show(resulta, "Error en actualización de seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            if (Program.vg_tius == "TPU001" && Program.vg_nius == "NIV000") // solo para todo poderoso
                            {
                                var xxx = MessageBox.Show("Regenera json y pdf del comprobante?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (xxx == DialogResult.Yes)
                                {
                                    if (factElec(nipfe, true, "alta", 0, true) == true)       // facturacion electrónica ...  cambiar a true 
                                    {
                                        // tutto finito
                                    }
                                }
                            }
                        }
                        else
                        {
                            tx_serie.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El documento ya debe existir para editar", "Debe ser edición", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
            }
            if (modo == "ANULAR")
            {
                if (tx_numero.Text.Trim() == "")
                {
                    tx_numero.Focus();
                    MessageBox.Show("Ingrese el número del documento", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (tx_dat_estad.Text == codAnul)
                {
                    MessageBox.Show("El documento esta ANULADO", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tx_numero.Focus();
                    return;
                }
                if (tx_idcob.Text != "")
                {
                    MessageBox.Show("El documento de venta tiene Cobranza activa" + Environment.NewLine +
                        "La cobranza permanece sin cambios", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //tx_numero.Focus();
                    //return;
                }
                // validaciones de fecha para poder anular
                DateTime fedv = DateTime.Parse(tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                TimeSpan span = DateTime.Parse(lib.fechaServ("ansi")) - fedv;
                if (span.Days > v_cdpa)
                {
                    // no se puede anular ... a menos que sea un usuario autorizado
                    if (codusanu.Contains(asd))
                    {
                        // SOLO USUARIOS AUTORIZADOS DEBEN ACCEDER A ESTA OPCIÓN
                        // SE ANULA EL DOCUMENTO Y SE HACEN LOS MOVIMIENTOS INTERNOS
                        // LA ANULACION EN EL PROVEEDOR DE FACT. ELECTRONICA SE HACE A MANO POR EL ENCARGADO ... 28/10/2020 ya no al 09/01/2021
                        // la anulacion debe generar un TXT de comunicacion de baja y guardarse en el directorio del prov. de fact. electronica 09/01/2021
                        if (tx_idr.Text.Trim() != "")
                        {
                            var aa = MessageBox.Show("Confirma que desea ANULAR el documento?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (aa == DialogResult.Yes)
                            {
                                if (lib.DirectoryVisible(rutatxt) == true)
                                {
                                    int cta = anula("FIS");      // cantidad de doc.vtas anuladas en la fecha
                                    if (factElec(nipfe, true, "baja", cta, false) == true)
                                    {
                                        string resulta = lib.ult_mov(nomform, nomtab, asd);
                                        if (resulta != "OK")
                                        {
                                            MessageBox.Show(resulta, "Error en actualización de seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No existe ruta o no es valida para" + Environment.NewLine +
                                        "generar la anulación electrónica","Ruta para Fact.Electrónica",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                                    iserror = "si";
                                }
                            }
                            else
                            {
                                tx_serie.Focus();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El documento ya debe existir para anular", "No esta el Id del registro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede anular por estar fuera de plazo","Usuario no permito",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    if (tx_idr.Text.Trim() != "")
                    {
                        var aa = MessageBox.Show("Confirma que desea ANULAR el documento?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            if (lib.DirectoryVisible(rutatxt) == true)
                            {
                                int cta = anula("FIS");      // cantidad de doc.vtas anuladas en la fecha
                                if (factElec(nipfe, true, "baja", cta, false) == true)
                                {
                                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                                    if (resulta != "OK")
                                    {
                                        MessageBox.Show(resulta, "Error en actualización de seguimiento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No existe ruta o no es valida para" + Environment.NewLine +
                                        "generar la anulación electrónica", "Ruta para Fact.Electrónica", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                iserror = "si";
                            }
                        }
                        else
                        {
                            tx_serie.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El documento ya debe existir para anular", "No esta el Id del registro", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
            }
            if (iserror == "no")
            {
                string resulta = lib.ult_mov(nomform, nomtab, asd);
                if (resulta != "OK")                                        // actualizamos la tabla usuarios
                {
                    MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // debe limpiar los campos y actualizar la grilla
            }
            initIngreso();          // limpiamos todo para volver a empesar
        }
        private bool graba()
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                // ANTES QUE NADA REVISAMOS QUE LA GUÍA NO HAYA SIDO FACTURADA EN EL ULTIMO MINUTO SEGUNDO ... 04/04/2024
                bool guiafac = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        string consu = "select tipdocvta,serdocvta,numdocvta from controlg where serguitra=@serg and numguitra=@numg";
                        using (MySqlCommand mi = new MySqlCommand(consu, conn))
                        {
                            mi.Parameters.AddWithValue("@serg", row.Cells[0].Value.ToString().Substring(0, 4));
                            mi.Parameters.AddWithValue("@numg", row.Cells[0].Value.ToString().Substring(5, 8));
                            using (MySqlDataReader dr = mi.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    if (dr.GetString(0).Trim() != "") guiafac = true;
                                }
                            }
                        }
                    }
                }
                if (guiafac == true)
                {
                    MessageBox.Show("Problema con guía","Error");
                    return retorna;
                }
                //
                string todo = "corre_serie";
                using (MySqlCommand micon = new MySqlCommand(todo, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("td", tx_dat_tdv.Text);
                    micon.Parameters.AddWithValue("ser", tx_serie.Text);
                    using (MySqlDataReader dr0 = micon.ExecuteReader())
                    {
                        if (dr0.Read())
                        {
                            if (dr0[0] != null && dr0.GetString(0) != "")
                            {
                                tx_numero.Text = lib.Right("00000000" + dr0.GetString(0), 8);
                            }
                        }
                    }
                }
                if (tx_tipcam.Text == "") tx_tipcam.Text = "0";
                decimal fletMN = 0;
                decimal subtMN = 0;
                decimal igvtMN = 0;
                if (tx_dat_mone.Text != MonDeft)
                {
                    if (tx_tipcam.Text == "0" || tx_fletMN.Text == "")
                    {
                        MessageBox.Show("Error con el tipo de cambio", "Error interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return retorna;
                    }
                    else
                    {
                        fletMN = Math.Round(decimal.Parse(tx_fletMN.Text), 3);
                        subtMN = Math.Round(fletMN / (1 + decimal.Parse(v_igv)/100), 3);
                        igvtMN = Math.Round(fletMN - subtMN, 3);
                    }
                }
                else
                {
                    fletMN = Math.Round(decimal.Parse(tx_flete.Text), 3);
                    subtMN = Math.Round(decimal.Parse(tx_subt.Text), 3);
                    igvtMN = Math.Round(decimal.Parse(tx_igv.Text), 3);
                }
                // comprobamos si los datos del cliente tienen cambios
                if (rb_remGR.Checked == true)
                {
                    if (datcltsR[0, 3].ToString().Trim() != tx_dirRem.Text.Trim() ||
                        datcltsR[0, 6].ToString().Trim() != tx_telc1.Text.Trim() ||
                        datcltsR[0, 5].ToString().Trim() != tx_email.Text.Trim() ||
                        datcltsR[0, 4].ToString().Trim() != tx_ubigRtt.Text.Trim())
                    {
                        tx_dat_m1clte.Text = "E";
                    }
                }
                if (rb_desGR.Checked == true)
                {
                    if (datcltsD[0, 3].ToString().Trim() != tx_dirRem.Text.Trim() ||
                        datcltsD[0, 6].ToString().Trim() != tx_telc1.Text.Trim() ||
                        datcltsD[0, 5].ToString().Trim() != tx_email.Text.Trim() ||
                        datcltsD[0, 4].ToString().Trim() != tx_ubigRtt.Text.Trim())
                    {
                        tx_dat_m1clte.Text = "E";
                    }
                }
                string inserta = "insert into cabfactu (" +
                    "fechope,martdve,tipdvta,serdvta,numdvta,ticltgr,tidoclt,nudoclt,nombclt,direclt,dptoclt,provclt,distclt,ubigclt,corrclt,teleclt," +
                    "locorig,dirorig,ubiorig,obsdvta,canfidt,canbudt,mondvta,tcadvta,subtota,igvtota,porcigv,totdvta,totpags,saldvta,estdvta,frase01," +
                    "tipoclt,m1clien,tippago,ferecep,impreso,codMN,subtMN,igvtMN,totdvMN,pagauto,tipdcob,idcaja,plazocred,porcendscto,valordscto," +
                    "cargaunica,placa,confveh,autoriz,detPeso,detputil,detMon1,detMon2,detMon3,dirporig,ubiporig,dirpdest,ubipdest," +
                    "verApp,userc,fechc,diriplan4,diripwan4,netbname) values (" +
                    "@fechop,@mtdvta,@ctdvta,@serdv,@numdv,@tcdvta,@tdcrem,@ndcrem,@nomrem,@dircre,@dptocl,@provcl,@distcl,@ubicre,@mailcl,@telecl," +
                    "@ldcpgr,@didegr,@ubdegr,@obsprg,@canfil,@totcpr,@monppr,@tcoper,@subpgr,@igvpgr,@porcigv,@totpgr,@pagpgr,@salxpa,@estpgr,@frase1," +
                    "@ticlre,@m1clte,@tipacc,@feredv,@impSN,@codMN,@subMN,@igvMN,@totMN,@pagaut,@tipdco,@idcaj,@plazc,@pordesc,@valdesc," +
                    "@caruni,@placa,@confv,@autor,@dPeso,@dputil,@dMon1,@dMon2,@dMon3,@dporig,@uporig,@dpdest,@updest," +
                    "@verApp,@asd,now(),@iplan,@ipwan,@nbnam)";
                using (MySqlCommand micon = new MySqlCommand(inserta, conn))
                {
                    micon.Parameters.AddWithValue("@fechop", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@mtdvta", cmb_tdv.Text.Substring(0,1));
                    micon.Parameters.AddWithValue("@ctdvta", tx_dat_tdv.Text);
                    micon.Parameters.AddWithValue("@serdv", tx_serie.Text);
                    micon.Parameters.AddWithValue("@numdv", tx_numero.Text);
                    micon.Parameters.AddWithValue("@tcdvta", (rb_remGR.Checked == true)? "1" : (rb_desGR.Checked == true)? "2" : "3");
                    micon.Parameters.AddWithValue("@tdcrem", tx_dat_tdRem.Text);
                    micon.Parameters.AddWithValue("@ndcrem", tx_numDocRem.Text);
                    micon.Parameters.AddWithValue("@nomrem", tx_nomRem.Text);
                    micon.Parameters.AddWithValue("@dircre", tx_dirRem.Text);
                    micon.Parameters.AddWithValue("@dptocl", tx_dptoRtt.Text);
                    micon.Parameters.AddWithValue("@provcl", tx_provRtt.Text);
                    micon.Parameters.AddWithValue("@distcl", tx_distRtt.Text);
                    micon.Parameters.AddWithValue("@ubicre", tx_ubigRtt.Text);
                    micon.Parameters.AddWithValue("@mailcl", tx_email.Text);
                    micon.Parameters.AddWithValue("@telecl", tx_telc1.Text);
                    micon.Parameters.AddWithValue("@ldcpgr", TransCarga.Program.almuser);         // local origen
                    micon.Parameters.AddWithValue("@didegr", dirloc);                             // direccion origen
                    micon.Parameters.AddWithValue("@ubdegr", ubiloc);                             // ubigeo origen
                    micon.Parameters.AddWithValue("@obsprg", tx_obser1.Text);
                    micon.Parameters.AddWithValue("@canfil", tx_tfil.Text);     // cantidad de filas de detalle
                    micon.Parameters.AddWithValue("@totcpr", tx_totcant.Text);  // total bultos
                    micon.Parameters.AddWithValue("@monppr", tx_dat_mone.Text);
                    micon.Parameters.AddWithValue("@tcoper", tx_tipcam.Text);                   // TIPO DE CAMBIO
                    micon.Parameters.AddWithValue("@subpgr", tx_subt.Text);                     // sub total
                    micon.Parameters.AddWithValue("@igvpgr", tx_igv.Text);                      // igv
                    micon.Parameters.AddWithValue("@porcigv", v_igv);                           // porcentaje en numeros de IGV
                    micon.Parameters.AddWithValue("@totpgr", tx_flete.Text);                    // total inc. igv
                    micon.Parameters.AddWithValue("@pagpgr", (tx_pagado.Text == "") ? "0" : tx_pagado.Text);
                    micon.Parameters.AddWithValue("@salxpa", (tx_salxcob.Text == "") ? "0" : tx_salxcob.Text);
                    micon.Parameters.AddWithValue("@estpgr", (tx_pagado.Text == "" || tx_pagado.Text == "0.00" || tx_pagado.Text == "0") ? tx_dat_estad.Text : codCanc); // estado
                    micon.Parameters.AddWithValue("@frase1", "");                   // no hay nada que poner 19/11/2020
                    micon.Parameters.AddWithValue("@ticlre", tx_dat_tcr.Text);      // tipo de cliente credito o contado
                    micon.Parameters.AddWithValue("@m1clte", tx_dat_m1clte.Text);
                    micon.Parameters.AddWithValue("@tipacc", v_mpag);                   // pago del documento x defecto si nace la fact pagada
                    micon.Parameters.AddWithValue("@feredv", DBNull.Value);         // si es pago contado la fecha de recep del doc. es la misma fecha
                    micon.Parameters.AddWithValue("@impSN", "N");
                    micon.Parameters.AddWithValue("@codMN", MonDeft);               // codigo moneda local
                    micon.Parameters.AddWithValue("@subMN", subtMN);
                    micon.Parameters.AddWithValue("@igvMN", igvtMN);
                    micon.Parameters.AddWithValue("@totMN", fletMN);
                    micon.Parameters.AddWithValue("@pagaut", (rb_si.Checked == true)? "S" : "N");
                    micon.Parameters.AddWithValue("@tipdco", (rb_si.Checked == true)? v_codcob : "");
                    micon.Parameters.AddWithValue("@idcaj", (rb_si.Checked == true)? tx_idcaja.Text : "0");
                    micon.Parameters.AddWithValue("@plazc", (rb_no.Checked == true)? (rb_credito.Checked == true)? codppc: "": "");
                    micon.Parameters.AddWithValue("@pordesc", (tx_dat_porcDscto.Text.Trim() == "") ? "0" : tx_dat_porcDscto.Text);
                    micon.Parameters.AddWithValue("@valdesc", (tx_valdscto.Text.Trim() == "") ? "0" : tx_valdscto.Text);
                    micon.Parameters.AddWithValue("@caruni", (chk_cunica.Checked == true)? 1 : 0);
                    micon.Parameters.AddWithValue("@placa", tx_pla_placa.Text);
                    micon.Parameters.AddWithValue("@confv", tx_pla_confv.Text);
                    micon.Parameters.AddWithValue("@autor", tx_pla_autor.Text);
                    micon.Parameters.AddWithValue("@dPeso", (tx_cetm.Text.Trim() == "")? "0" : tx_cetm.Text);
                    micon.Parameters.AddWithValue("@dputil", (tx_cutm.Text.Trim() == "")? "0" : tx_cutm.Text);
                    micon.Parameters.AddWithValue("@dMon1", (tx_valref1.Text.Trim() == "")? "0" : tx_valref1.Text);
                    micon.Parameters.AddWithValue("@dMon2", (tx_valref2.Text.Trim() == "")? "0" : tx_valref2.Text);
                    micon.Parameters.AddWithValue("@dMon3", (tx_valref3.Text.Trim() == "")? "0" : tx_valref3.Text);
                    micon.Parameters.AddWithValue("@dporig", tx_dat_dpo.Text);
                    micon.Parameters.AddWithValue("@uporig", tx_dat_upo.Text);
                    micon.Parameters.AddWithValue("@dpdest", tx_dat_dpd.Text);
                    micon.Parameters.AddWithValue("@updest", tx_dat_upd.Text);
                    micon.Parameters.AddWithValue("@verApp", verapp);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@iplan", lib.iplan());
                    micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                    micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                    micon.ExecuteNonQuery();
                }
                /*  ESO LO COMENTE EL 21/05/2022 ... VEREMOS SI CON ESTO MAS EL UPDATE CON TIPO,SERIE Y NUMERO MEJORAMOS EN ALGO
                using (MySqlCommand micon = new MySqlCommand("select last_insert_id()", conn))
                {
                    using (MySqlDataReader dr = micon.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString(0);
                        }
                    }
                }
                */
                // detalle
                if (dataGridView1.Rows.Count > 0)
                {
                    int fila = 1;
                    int tfg = (dataGridView1.Rows.Count == int.Parse(v_mfildet) && int.Parse(tx_tfil.Text) == int.Parse(v_mfildet)) ? int.Parse(v_mfildet) : dataGridView1.Rows.Count - 1;
                    for (int i = 0; i < tfg; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() != "")
                        {
                            string inserd2 = "update detfactu set " +
                                "codgror=@guia,cantbul=@bult,unimedp=@unim,descpro=@desc,pesogro=@peso,codmogr=@codm,totalgr=@pret,codMN=@cmnn," +
                                "totalgrMN=@tgrmn,pagauto=@pagaut " +
                                "where tipdocvta=@tdv and serdvta=@sdv and numdvta=@cdv and filadet=@fila"; // "where idc=@idr and filadet=@fila"
                            using (MySqlCommand micon = new MySqlCommand(inserd2, conn))
                            {
                                micon.CommandTimeout = 60;
                                micon.Parameters.AddWithValue("@tdv", tx_dat_tdv.Text);
                                micon.Parameters.AddWithValue("@sdv", tx_serie.Text);
                                micon.Parameters.AddWithValue("@cdv", tx_numero.Text);
                                micon.Parameters.AddWithValue("@fila", fila);
                                micon.Parameters.AddWithValue("@guia", dataGridView1.Rows[i].Cells[0].Value.ToString());
                                micon.Parameters.AddWithValue("@bult", dataGridView1.Rows[i].Cells[2].Value.ToString());
                                micon.Parameters.AddWithValue("@unim", "");
                                micon.Parameters.AddWithValue("@desc", dataGridView1.Rows[i].Cells[1].Value.ToString());
                                micon.Parameters.AddWithValue("@peso", "0");
                                micon.Parameters.AddWithValue("@codm", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                micon.Parameters.AddWithValue("@pret", dataGridView1.Rows[i].Cells[4].Value.ToString());
                                micon.Parameters.AddWithValue("@cmnn", dataGridView1.Rows[i].Cells[6].Value.ToString());
                                micon.Parameters.AddWithValue("@tgrmn", dataGridView1.Rows[i].Cells[5].Value.ToString());
                                micon.Parameters.AddWithValue("@pagaut", (rb_si.Checked == true) ? "S" : "N");
                                micon.ExecuteNonQuery();
                                fila += 1;
                                //
                                retorna = true;         // no hubo errores!
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return retorna;
            }
            conn.Close();
            return retorna;
        }
        private void edita()
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    if (true)     // EDICION DE CABECERA
                    {
                        string actua = "update cabfactu a set obsdvta=@obsprg,fechope=@fechop," +
                            "a.verApp=@verApp,a.userm=@asd,a.fechm=now(),a.diriplan4=@iplan,a.diripwan4=@ipwan,a.netbname=@nbnam " +
                            "where a.id=@idr";
                        MySqlCommand micon = new MySqlCommand(actua, conn);
                        micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                        micon.Parameters.AddWithValue("@fechop", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                        micon.Parameters.AddWithValue("@obsprg", tx_obser1.Text);
                        micon.Parameters.AddWithValue("@verApp", verapp);
                        micon.Parameters.AddWithValue("@asd", asd);
                        micon.Parameters.AddWithValue("@iplan", lib.iplan());
                        micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                        micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                        micon.ExecuteNonQuery();
                        //
                        // EDICION DEL DETALLE .... no hay 28/10/2020
                        micon.Dispose();
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en modificar el documento");
                    Application.Exit();
                    return;
                }
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return;
            }
        }
        private int anula(string tipo)
        {
            int ctanul = 0;
            // en el caso de documentos de venta HAY 1: ANULACION FISICA ... 28/10/2020
            // tambien podría haber ANULACION interna con la serie ANU1 .... 19/11/2020
            // Anulacion fisica se "anula" el numero del documento en sistema y en fisico se tacha y en prov. fact.electronica se da baja de numeracion
            // se borran todos los enlaces mediante triggers en la B.D.
            if (tipo == "FIS")
            {
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string canul = "update cabfactu set estdvta=@estser,obsdvta=@obse,usera=@asd,fecha=now()," +
                            "verApp=@veap,diriplan4=@dil4,diripwan4=@diw4,netbname=@nbnp,estintreg=@eiar " +
                            "where id=@idr";
                        using (MySqlCommand micon = new MySqlCommand(canul, conn))
                        {
                            micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                            micon.Parameters.AddWithValue("@estser", codAnul);
                            micon.Parameters.AddWithValue("@obse", tx_obser1.Text);
                            micon.Parameters.AddWithValue("@asd", asd);
                            micon.Parameters.AddWithValue("@dil4", lib.iplan());
                            micon.Parameters.AddWithValue("@diw4", TransCarga.Program.vg_ipwan);
                            micon.Parameters.AddWithValue("@nbnp", Environment.MachineName);
                            micon.Parameters.AddWithValue("@veap", verapp);
                            micon.Parameters.AddWithValue("@eiar", (vint_A0 == codAnul) ? "A0" : "");  // codigo anulacion interna en DB A0
                            micon.ExecuteNonQuery();
                        }
                        string consul = "select count(id) from cabfactu where date(fecha)=@fech and estdvta=@estser";
                        using (MySqlCommand micon = new MySqlCommand(consul, conn))
                        {
                            micon.Parameters.AddWithValue("@fech", tx_fechact.Text.Substring(6, 4) + "-" + tx_fechact.Text.Substring(3, 2) + "-" + tx_fechact.Text.Substring(0, 2));
                            micon.Parameters.AddWithValue("@estser", codAnul);
                            using (MySqlDataReader dr = micon.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    ctanul = dr.GetInt32(0);
                                }
                            }
                        }
                    }
                }
            }
            if (tipo == "INT")
            {
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string canul = "update cabfactu set serdvta=@sain,estdvta=@estser,obsdvta=@obse,usera=@asd,fecha=now()," +
                            "verApp=@veap,diriplan4=@dil4,diripwan4=@diw4,netbname=@nbnp,estintreg=@eiar " +
                            "where id=@idr";
                        using (MySqlCommand micon = new MySqlCommand(canul, conn))
                        {
                            micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                            micon.Parameters.AddWithValue("@sain", v_sanu);
                            micon.Parameters.AddWithValue("@estser", codAnul);
                            micon.Parameters.AddWithValue("@obse", tx_obser1.Text);
                            micon.Parameters.AddWithValue("@asd", asd);
                            micon.Parameters.AddWithValue("@dil4", lib.iplan());
                            micon.Parameters.AddWithValue("@diw4", TransCarga.Program.vg_ipwan);
                            micon.Parameters.AddWithValue("@nbnp", Environment.MachineName);
                            micon.Parameters.AddWithValue("@veap", verapp);
                            micon.Parameters.AddWithValue("@eiar", (vint_A0 == codAnul) ? "A0" : "");  // codigo anulacion interna en DB A0
                            micon.ExecuteNonQuery();
                        }
                        string updser = "update series set actual=actual-1 where tipdoc=@tipd AND serie=@serd";
                        using (MySqlCommand micon = new MySqlCommand(updser, conn))
                        {
                            micon.Parameters.AddWithValue("@tipd", tx_dat_tdv.Text);
                            micon.Parameters.AddWithValue("@serd", tx_serie.Text);
                            micon.ExecuteNonQuery();
                        }
                    }
                }
            }
            return ctanul;
        }
        #endregion boton_form;

        #region leaves y checks
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text != "")
            {
                dataGridView1.Rows.Clear();
                jalaoc("tx_idr");
                jaladet(tx_idr.Text);
                cargaunica(0);
            }
        }
        private void tx_nomRem_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_nomRem);
        }
        private void tx_dirRem_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_dirRem);
        }
        private void textBox7_Leave(object sender, EventArgs e)         // departamento del remitente, jala provincia
        {
            if(tx_dptoRtt.Text.Trim() != "")    //  && TransCarga.Program.vg_conSol == false
            {
                DataRow[] row = dataUbig.Select("nombre='" + tx_dptoRtt.Text.Trim() + "' and provin='00' and distri='00'");
                if (row.Length > 0)
                {
                    tx_ubigRtt.Text = row[0].ItemArray[1].ToString(); // lib.retCodubigeo(tx_dptoRtt.Text.Trim(),"","");
                    autoprov();
                }
                else tx_dptoRtt.Text = "";
            }
        }
        private void textBox8_Leave(object sender, EventArgs e)         // provincia del remitente
        {
            if(tx_provRtt.Text != "" && tx_dptoRtt.Text.Trim() != "")   // && TransCarga.Program.vg_conSol == false
            {
                DataRow[] row = dataUbig.Select("depart='" + tx_ubigRtt.Text.Substring(0, 2) + "' and nombre='" + tx_provRtt.Text.Trim() + "' and provin<>'00' and distri='00'");
                if (row.Length > 0)
                {
                    tx_ubigRtt.Text = tx_ubigRtt.Text.Trim().Substring(0, 2) + row[row.Length - 1].ItemArray[2].ToString();
                    autodist();
                }
                else tx_provRtt.Text = "";
            }
        }
        private void textBox9_Leave(object sender, EventArgs e)         // distrito del remitente
        {
            if(tx_distRtt.Text.Trim() != "" && tx_provRtt.Text.Trim() != "" && tx_dptoRtt.Text.Trim() != "")
            {
                DataRow[] row = dataUbig.Select("depart='" + tx_ubigRtt.Text.Substring(0, 2) + "' and provin='" + tx_ubigRtt.Text.Substring(2, 2) + "' and nombre='" + tx_distRtt.Text.Trim() + "'");
                if (row.Length > 0)
                {
                    tx_ubigRtt.Text = tx_ubigRtt.Text.Trim().Substring(0, 4) + row[row.Length -1].ItemArray[3].ToString();
                    //else tx_ubigRtt.Text = tx_ubigRtt.Text.Trim().Substring(0, 4) + row[1].ItemArray[3].ToString();
                }
                else tx_distRtt.Text = "";
            }
        }
        private void textBox13_Leave(object sender, EventArgs e)        // ubigeo del remitente
        {
            if(tx_ubigRtt.Text.Trim() != "" && tx_ubigRtt.Text.Length == 6)
            {
                string[] du_remit = lib.retDPDubigeo(tx_ubigRtt.Text);
                tx_dptoRtt.Text = du_remit[0];
                tx_provRtt.Text = du_remit[1];
                tx_distRtt.Text = du_remit[2];
            }
        }
        private void textBox3_Leave(object sender, EventArgs e)         // número de documento remitente
        {
            if (tx_numDocRem.Text.Trim() != "" && tx_mld.Text.Trim() != "")
            {
                if (tx_numDocRem.Text.Trim().Length != Int16.Parse(tx_mld.Text))
                {
                    MessageBox.Show("El número de caracteres para" + Environment.NewLine +
                        "su tipo de documento debe ser: " + tx_mld.Text, "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    tx_numDocRem.Focus();
                    return;
                }
                if (tx_dat_tdRem.Text == vtc_ruc && lib.valiruc(tx_numDocRem.Text, vtc_ruc) == false)
                {
                    MessageBox.Show("Número de RUC inválido", "Atención - revise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_numDocRem.Focus();
                    return;
                }
                string encuentra = "no";
                if (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
                {
                    tx_nomRem.Text = "";
                    tx_dirRem.Text = "";
                    tx_dptoRtt.Text = "";
                    tx_provRtt.Text = "";
                    tx_distRtt.Text = "";
                    tx_ubigRtt.Text = "";
                    tx_email.Text = "";
                    tx_telc1.Text = "";

                    string[] datos = lib.datossn("CLI", tx_dat_tdRem.Text.Trim(), tx_numDocRem.Text.Trim());
                    if (datos[0] != "")  // datos.Length > 0
                    {
                        tx_nomRem.Text = datos[0];
                        tx_nomRem.Select(0, 0);
                        tx_dirRem.Text = datos[1];
                        tx_dirRem.Select(0, 0);
                        tx_dptoRtt.Text = datos[2];
                        tx_dptoRtt.Select(0, 0);
                        tx_provRtt.Text = datos[3];
                        tx_provRtt.Select(0, 0);
                        tx_distRtt.Text = datos[4];
                        tx_distRtt.Select(0, 0);
                        tx_ubigRtt.Text = datos[5];
                        tx_ubigRtt.Select(0, 0);
                        tx_email.Text = datos[7];
                        tx_email.Select(0, 0);
                        tx_telc1.Text = datos[6];
                        tx_telc1.Select(0, 0);
                        encuentra = "si";
                        tx_dat_m1clte.Text = "E";
                    }
                    if (tx_dat_tdRem.Text == vtc_ruc)
                    {
                        if (true)       // encuentra == "no"    11/04/2023
                        {
                            if (TransCarga.Program.vg_conSol == true) // conector solorsoft para ruc
                            {
                                string[] rl = lib.conectorSolorsoft("RUC", tx_numDocRem.Text);
                                string myStr = rl[0].Replace("\r\n", "");
                                if (rl[0] == "" || myStr == NoRetGl)
                                {
                                    var aa = MessageBox.Show(" No encontramos el documento en ningún registro. " + Environment.NewLine +
                                                            " Deberá ingresarlo manualmente si esta seguro(a) " + Environment.NewLine +
                                                            " de la validez del número y documento. " + Environment.NewLine +
                                                            "" + Environment.NewLine +
                                                            "Confirma que desea ingresarlo manualmente?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (aa == DialogResult.No)
                                    {
                                        tx_numDocRem.Text = "";
                                        tx_nomRem.Text = "";
                                        tx_dirRem.Text = "";
                                        tx_dptoRtt.Text = "";
                                        tx_provRtt.Text = "";
                                        tx_distRtt.Text = "";
                                        tx_ubigRtt.Text = "";
                                        tx_email.Text = "";
                                        tx_telc1.Text = "";
                                        tx_numDocRem.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    if (rl[6] != "ACTIVO" || rl[7] != "HABIDO")
                                    {
                                        var aa = MessageBox.Show("No debería generar el comprobante" + Environment.NewLine +
                                            "el ruc tiene el estado o condición no correcto" + Environment.NewLine + Environment.NewLine +
                                            "Condición: " + rl[7] + Environment.NewLine +
                                            "Estado: " + rl[6] + Environment.NewLine + Environment.NewLine +
                                            "CONFIRMA QUE DESEA CONTINUAR?", "Alerta - no debería continuar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (aa == DialogResult.No)
                                        {
                                            tx_numDocRem.Text = "";
                                            tx_nomRem.Text = "";
                                            tx_dirRem.Text = "";
                                            tx_dptoRtt.Text = "";
                                            tx_provRtt.Text = "";
                                            tx_distRtt.Text = "";
                                            tx_ubigRtt.Text = "";
                                            tx_email.Text = "";
                                            tx_telc1.Text = "";
                                            tx_numDocRem.Focus();
                                            return;
                                        }
                                    }
                                    if (tx_numDocRem.Text.Substring(0,2) == "20")
                                    {
                                        tx_nomRem.Text = rl[0].Trim().Replace("\r\n", "");      // razon social
                                        tx_ubigRtt.Text = rl[1].Trim().Replace("\r\n", "");     // ubigeo
                                        tx_dirRem.Text = rl[2].Trim().Replace("\r\n", "");      // direccion
                                        tx_dptoRtt.Text = (rl[3].Trim().Replace("\r\n", "") == "PROV. CONST. DEL CALLAO") ? "CALLAO" : rl[3];      // departamento
                                        tx_provRtt.Text = (rl[4].Trim().Replace("\r\n", "") == "PROV.CONST.DEL CALLAO") ? "CALLAO" : rl[4];      // provincia
                                        tx_distRtt.Text = rl[5].Trim().Replace("\r\n", "");      // distrito
                                    }
                                }
                                tx_dat_m1clte.Text = "N";
                            }
                        }
                    }
                    if (tx_dat_tdRem.Text == vtc_dni)
                    {
                        if (encuentra == "no")
                        {
                            if (TransCarga.Program.vg_conSol == true) // conector solorsoft para dni
                            {
                                string[] rl = lib.conectorSolorsoft("DNI", tx_numDocRem.Text);
                                string myStr = rl[0].Replace("\r\n", "");
                                if (rl[0] == "" || myStr == NoRetGl)
                                {
                                    MessageBox.Show("No encontramos el DNI en la busqueda inicial, estamos abriendo" + Environment.NewLine +
                                    "una página web para que efectúe la busqueda manualmente", "Redirección a web de DNI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    System.Diagnostics.Process.Start(webdni);    // "https://eldni.com/pe/buscar-por-dni"
                                }
                                else
                                {
                                    tx_nomRem.Text = rl[0];      // nombre
                                }
                                tx_dat_m1clte.Text = "N";
                            }
                        }
                    }
                }
            }
            if (tx_numDocRem.Text.Trim() != "" && tx_mld.Text.Trim() == "")
            {
                cmb_docRem.Focus();
            }
        }
        private void tx_numero_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_numero.Text.Trim() != "")
            {
                // en el caso de las pre guias el numero es el mismo que el ID del registro
                tx_numero.Text = lib.Right("00000000" + tx_numero.Text, 8);
                //tx_idr.Text = tx_numero.Text;
                jalaoc("sernum");
                dataGridView1.Rows.Clear();
                jaladet(tx_idr.Text);
                cargaunica(0);
            }
        }
        private void tx_serie_Leave(object sender, EventArgs e)
        {
            tx_serie.Text = lib.Right("0000" + tx_serie.Text, 4);
            if (Tx_modo.Text == "NUEVO") tx_serGR.Focus();
        }
        private void tx_flete_Leave(object sender, EventArgs e)
        {
            if (tx_flete.Text.Trim() != "" && Tx_modo.Text == "NUEVO")
            {
                tx_flete.Text = Math.Round(decimal.Parse(tx_flete.Text), 2).ToString("#0.00");
                calculos(decimal.Parse((tx_flete.Text.Trim() != "") ? tx_flete.Text : "0"));
                //
                if (tx_dat_mone.Text != MonDeft)
                {
                    if (tx_tipcam.Text == "" || tx_tipcam.Text.Trim() == "0")
                    {
                        MessageBox.Show("Se requiere tipo de cambio");
                        tx_flete.Text = "";
                        tx_flete.Focus();
                        return;
                    }
                    else
                    {
                        tx_fletMN.Text = Math.Round(decimal.Parse(tx_flete.Text) * decimal.Parse(tx_tipcam.Text), 2).ToString();
                        if (Math.Round(decimal.Parse(tx_tfmn.Text),1) != Math.Round(decimal.Parse(tx_fletMN.Text),1))   // OJO, no hacemos dscto en moneda diferente al nacional 
                        {
                            var aa = MessageBox.Show("No coinciden los valores por tipo de cambio" + Environment.NewLine +
                                "Desea continuar?","Error en valores",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                            if (aa == DialogResult.No)
                            {
                                tx_flete.Text = "";
                                tx_flete.Focus();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    // si el valor del flete es menor al valor de tx_tfmn ===> tiene descuento
                    // si tiene descuento, visibiliza campo descuento y calcula monto y %
                    if (Math.Round(decimal.Parse(tx_flete.Text), 1) < Math.Round(decimal.Parse(tx_tfmn.Text), 1))
                    {
                        lin_dscto.Visible = true;
                        lb_dscto.Visible = true;
                        tx_valdscto.Visible = true;
                        // calculos
                        tx_valdscto.Text = (Math.Round(decimal.Parse(tx_tfmn.Text), 1) - Math.Round(decimal.Parse(tx_flete.Text), 1)).ToString("#0.0");
                        tx_dat_porcDscto.Text = ((Math.Round(decimal.Parse(tx_flete.Text), 1) * 100) / Math.Round(decimal.Parse(tx_tfmn.Text), 1)).ToString("#0.00");
                    }
                    else
                    {
                        if (Math.Round(decimal.Parse(tx_flete.Text), 1) > Math.Round(decimal.Parse(tx_tfmn.Text), 1))
                        {
                            MessageBox.Show("No se permite facturar montos de las guías","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            tx_flete.Text = tx_tfmn.Text;
                        }
                        lin_dscto.Visible = false;
                        lb_dscto.Visible = false;
                        tx_valdscto.Visible = false;
                    }
                }
                DataRow[] row = dtm.Select("idcodice='" + tx_dat_mone.Text + "'");
                NumLetra numLetra = new NumLetra();
                tx_fletLetras.Text = numLetra.Convertir(tx_flete.Text,true) + row[0][3].ToString().Trim();
                button1.Focus();
            }
        }
        private void tx_serGR_Leave(object sender, EventArgs e)
        {
            if (tx_serGR.Text.Trim() != "")
            {
                if (tx_serGR.Text.Substring(0, 1) != "V") tx_serGR.Text = lib.Right("0000" + tx_serGR.Text, 4);
            }
        }
        private void tx_numGR_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" && tx_serGR.Text.Trim() != "" && tx_numGR.Text.Trim() != "")
            {
                tx_numGR.Text = lib.Right("00000000" + tx_numGR.Text, 8);
            }
        }
        private void rb_remGR_Click(object sender, EventArgs e)         // datos del remitente de la GR
        {
            tx_dat_tdRem.Text = datcltsR[0, 0];
            cmb_docRem.SelectedValue = tx_dat_tdRem.Text;
            tx_numDocRem.Text = datcltsR[0, 1];
            tx_nomRem.Text = datcltsR[0, 2];
            tx_dirRem.Text = datcltsR[0, 3];
            tx_dptoRtt.Text = "";
            tx_provRtt.Text = "";
            tx_distRtt.Text = "";
            
            DataRow[] fila = dttd0.Select("idcodice='" + tx_dat_tdRem.Text + "'");
            foreach (DataRow row in fila)
            {
                tx_mld.Text = row[2].ToString();
            }
            
            if (datcltsR[0, 4].ToString().Trim() != "")
            {
                DataRow[] row = dataUbig.Select("depart='" + datcltsR[0, 4].Substring(0, 2) + "' and provin='00' and distri='00'");
                tx_dptoRtt.Text = row[0].ItemArray[4].ToString();
                row = dataUbig.Select("depart='" + datcltsR[0, 4].Substring(0, 2) + "' and provin ='" + datcltsR[0, 4].Substring(2, 2) + "' and distri='00'");
                tx_provRtt.Text = row[0].ItemArray[4].ToString();
                row = dataUbig.Select("depart='" + datcltsR[0, 4].Substring(0, 2) + "' and provin ='" + datcltsR[0, 4].Substring(2, 2) + "' and distri='" + datcltsR[0, 4].Substring(4, 2) + "'");
                tx_distRtt.Text = row[0].ItemArray[4].ToString();
                //
                tx_email.Text = datcltsR[0, 5];
                tx_telc1.Text = datcltsR[0, 6];
                tx_telc2.Text = datcltsR[0, 7];
                tx_ubigRtt.Text = datcltsR[0, 4];
            }
            cmb_docRem.Enabled = false;
            tx_numDocRem.ReadOnly = true;
            tx_nomRem.ReadOnly = true;
        }
        private void rb_desGR_Click(object sender, EventArgs e)         // datos del destinatario de la GR
        {
            tx_dat_tdRem.Text = datcltsD[0, 0];
            cmb_docRem.SelectedValue = tx_dat_tdRem.Text;
            tx_numDocRem.Text = datcltsD[0, 1];
            tx_nomRem.Text = datcltsD[0, 2];
            tx_dirRem.Text = datcltsD[0, 3];
            tx_dptoRtt.Text = "";
            tx_provRtt.Text = "";
            tx_distRtt.Text = "";

            DataRow[] fila = dttd0.Select("idcodice='" + tx_dat_tdRem.Text + "'");
            foreach (DataRow row in fila)
            {
                tx_mld.Text = row[2].ToString();
            }

            try
            {
                if (datcltsD[0, 4].ToString().Trim() != "")
                {
                    DataRow[] row = dataUbig.Select("depart='" + datcltsD[0, 4].Substring(0, 2) + "' and provin='00' and distri='00'");
                    tx_dptoRtt.Text = row[0].ItemArray[4].ToString();
                    row = dataUbig.Select("depart='" + datcltsD[0, 4].Substring(0, 2) + "' and provin ='" + datcltsD[0, 4].Substring(2, 2) + "' and distri='00'");
                    tx_provRtt.Text = row[0].ItemArray[4].ToString();
                    row = dataUbig.Select("depart='" + datcltsD[0, 4].Substring(0, 2) + "' and provin ='" + datcltsD[0, 4].Substring(2, 2) + "' and distri='" + datcltsD[0, 4].Substring(4, 2) + "'");
                    tx_distRtt.Text = row[0].ItemArray[4].ToString();
                    //
                    tx_email.Text = datcltsD[0, 5];
                    tx_telc1.Text = datcltsD[0, 6];
                    tx_telc2.Text = datcltsD[0, 7];
                    tx_ubigRtt.Text = datcltsD[0, 4];
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error en datos del Destinatario " + Environment.NewLine + ex.Message, "Error interno", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //
            cmb_docRem.Enabled = false;
            tx_numDocRem.ReadOnly = true;
            tx_nomRem.ReadOnly = true;
        }
        private void rb_otro_Click(object sender, EventArgs e)
        {
            cmb_docRem.Enabled = true;
            tx_numDocRem.ReadOnly = false;
            tx_nomRem.ReadOnly = false;
            //
            tx_numDocRem.Text = "";
            tx_nomRem.Text = "";
            tx_dirRem.Text = "";
            tx_dptoRtt.Text = "";
            tx_provRtt.Text = "";
            tx_distRtt.Text = "";
            tx_email.Text = "";
            tx_telc1.Text = "";
            tx_telc2.Text = "";
            cmb_docRem.SelectedIndex = 0;
            tx_dat_tdRem.Text = cmb_docRem.SelectedValue.ToString();
            DataRow[] fila = dttd0.Select("idcodice='" + tx_dat_tdRem.Text + "'");
            foreach (DataRow row in fila)
            {
                tx_mld.Text = row[2].ToString();
            }
            cmb_docRem.Focus();
        }
        private void tx_email_Leave(object sender, EventArgs e)
        {
            if (tx_email.Text.Trim() != "")
            {
                if (lib.email_bien_escrito(tx_email.Text.Trim()) == false)
                {
                    MessageBox.Show("El correo electrónico esta mal", "Por favor corrija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tx_email.Focus();
                    return;
                }
                if (tx_dat_m1clte.Text != "N") tx_dat_m1clte.Text = "E";
            }
        }
        private void tx_telc1_Leave(object sender, EventArgs e)
        {
            if (tx_telc1.Text.Trim() != "") //  && (Tx_modo.Text == "NUEVO" || Tx_modo.Text == "EDITAR")
            {
                val_NoCaracteres(tx_telc1);
                if (tx_dat_m1clte.Text != "N") tx_dat_m1clte.Text = "E";
            }
        }
        private void tx_pla_placa_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_pla_placa);
        }
        private void tx_pla_confv_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_pla_confv);
        }
        private void tx_pla_autor_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_pla_autor);
        }
        private void tx_obser1_Leave(object sender, EventArgs e)
        {
            val_NoCaracteres(tx_obser1);
        }
        private void rb_si_Click(object sender, EventArgs e)
        {
            if (tx_idcaja.Text != "")
            {
                // validamos la fecha de la caja
                string fhoy = lib.fechaServ("ansi");
                if (fhoy != TransCarga.Program.vg_fcaj)  // ambas fecahs formato yyyy-mm-dd
                {
                    MessageBox.Show("Debe cerrar la caja anterior!", "Caja fuera de fecha", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    rb_si.Checked = false;
                    rb_no.PerformClick();
                    return;
                }
                else
                {
                    if (tx_dat_saldoGR.Text.Trim() != "")
                    {
                        if (decimal.Parse(tx_dat_saldoGR.Text) > 0)
                        {
                            tx_pagado.Text = tx_flete.Text;
                            tx_salxcob.Text = "0.00";
                            tx_salxcob.BackColor = Color.Green;
                        }
                        else
                        {
                            tx_salxcob.Text = "0.00";
                            tx_dat_plazo.Text = "";
                            cmb_plazoc.SelectedIndex = -1;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No existe caja abierta!" + Environment.NewLine +
                    "No puede cobrar hasta aperturar caja", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                rb_si.Checked = false;
                //rb_no_Click(null, null);
                //rb_no.Checked = true;
                rb_no.PerformClick();
            }
        }
        private void rb_no_Click(object sender, EventArgs e)
        {
            tx_pagado.Text = "0.00";
            tx_salxcob.Text = tx_flete.Text;
            tx_salxcob.BackColor = Color.Red;
            if (rb_credito.Checked == true)
            {
                cmb_plazoc.Enabled = true;
                cmb_plazoc.SelectedValue = codppc;
                tx_dat_plazo.Text = codppc;
            }
        }
        private void rb_contado_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (rb_contado.Checked == true)
                {
                    rb_si.Checked = false;
                    rb_si.Enabled = true;
                    rb_no.Checked = false;
                    rb_no.Enabled = true;
                    cmb_plazoc.SelectedIndex = -1;
                    tx_dat_dpla.Text = "";
                    cmb_plazoc.Enabled = false;
                }
            }
        }
        private void rb_credito_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO")
            {
                if (rb_credito.Checked == true)
                {
                    rb_si.Checked = false;
                    rb_si.Enabled = false;
                    rb_no.Checked = true;
                    rb_no.Enabled = true;
                    cmb_plazoc.Enabled = true;
                }
            }
        }
        private void chk_sinco_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_sinco.Checked == true)
            {
                if (tx_email.Text.Trim() != "") chk_sinco.Checked = false;
                else tx_email.Text = correo_gen;
            }
            else
            {
                if (tx_email.Text.Trim() != "") tx_email.Text = "";
                //else 
            }
        }
        private void chk_cunica_CheckedChanged(object sender, EventArgs e)
        {
            cargaunica(0);
        }
        private void val_NoCaracteres(TextBox textBox)
        {
            if (caractNo != "")
            {
                int index = textBox.Text.IndexOf(caractNo);
                if (index > -1)
                {
                    char cno = caractNo.ToCharArray()[0];
                    textBox.Text = textBox.Text.Replace(cno, ' ');
                }
            }
        }
        #endregion

        #region botones_de_comando
        public void toolboton()
        {
            DataTable mdtb = new DataTable();
            const string consbot = "select * from permisos where formulario=@nomform and usuario=@use";
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    MySqlCommand consulb = new MySqlCommand(consbot, conn);
                    consulb.Parameters.AddWithValue("@nomform", nomform);
                    consulb.Parameters.AddWithValue("@use", asd);
                    MySqlDataAdapter mab = new MySqlDataAdapter(consulb);
                    mab.Fill(mdtb);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return;
                }
                finally { conn.Close(); }
            }
            else
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (mdtb.Rows.Count > 0)
            {
                DataRow row = mdtb.Rows[0];
                if (Convert.ToString(row["btn1"]) == "S")
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                //if (Convert.ToString(row["btn5"]) == "S")
                //{
                //    this.Bt_print.Visible = true;
                //}
                //else { this.Bt_print.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                //if (Convert.ToString(row["btn4"]) == "S")
                //{
                //    this.Bt_ver.Visible = true;
                //}
                //else { this.Bt_ver.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
        }
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            escribe();
            // 
            button1.Enabled = true;
            Bt_ini.Enabled = false;
            Bt_sig.Enabled = false;
            Bt_ret.Enabled = false;
            Bt_fin.Enabled = false;
            tx_salxcob.BackColor = Color.White;
            // validamos la fecha de la caja
            fshoy = lib.fechaServ("ansi");
            chk_iGRE.Visible = true;
            chk_iGRE.Checked = false;
            tx_flete.ReadOnly = true;
            initIngreso();
            tx_numero.ReadOnly = true;
            cmb_tdv_SelectedIndexChanged(null, null);
            cmb_tdv.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            sololee();          
            Tx_modo.Text = "EDITAR";                    // solo puede editarse la observacion 28/10/2020
            button1.Image = Image.FromFile(img_grab);
            tx_flete.ReadOnly = true;
            initIngreso();
            tx_obser1.Enabled = true;
            tx_obser1.ReadOnly = false;
            gbox_serie.Enabled = true;
            tx_numero.Text = "";
            tx_serie.ReadOnly = false;
            tx_numero.ReadOnly = false;
            tx_serie.Focus();
            //
            button1.Enabled = true;
            chk_iGRE.Visible = false;
            chk_iGRE.Checked = false;
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
            tx_salxcob.BackColor = Color.White;
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            if (Tx_modo.Text.Trim() != "" && tx_numero.Text.Trim() != "")
            {
                if (tx_impreso.Text == "S")
                {
                    var aa = MessageBox.Show("Desea re imprimir el documento?", "Confirme por favor",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        if (vi_formato == "A4")            // Seleccion de formato ... A4
                        {
                            if (imprimeA4() == true) updateprint("S");
                        }
                        if (vi_formato == "A5")            // Seleccion de formato ... A5
                        {
                            if (imprimeA5() == true) updateprint("S");
                        }
                        if (vi_formato == "TK")            // Seleccion de formato ... Ticket
                        {
                            if (imprimeTK() == true) updateprint("S");
                        }
                    }
                }
                else
                {
                    if (vi_formato == "A4")            // Seleccion de formato ... A4
                    {
                        if (imprimeA4() == true) updateprint("S");
                    }
                    if (vi_formato == "A5")
                    {
                        if (imprimeA5() == true) updateprint("S");
                    }
                    if (vi_formato == "TK")
                    {
                        if (imprimeTK() == true) updateprint("S");
                    }
                    // Impresión de GRE en formato TK si esta marcado el check chk_iGRE
                    if (chk_iGRE.Checked == true && chk_iGRE.Visible == true)
                    {
                        imprimeGRE();
                    }
                }
            }
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            sololee();
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            initIngreso();
            gbox_serie.Enabled = true;
            tx_serie.ReadOnly = false;
            tx_numero.ReadOnly = false;
            tx_obser1.Enabled = true;
            tx_obser1.ReadOnly = false;
            tx_serie.Focus();
            //
            button1.Enabled = true;
            chk_iGRE.Visible = false;
            chk_iGRE.Checked = false;
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            sololee();
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = Image.FromFile(img_ver);
            initIngreso();
            gbox_serie.Enabled = true;
            tx_serie.ReadOnly = false;
            tx_numero.ReadOnly = false;
            tx_serie.Focus();
            //
            chk_iGRE.Visible = false;
            chk_iGRE.Checked = false;
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
        }
        private void Bt_first_Click(object sender, EventArgs e)
        {
            limpiar();
            limpia_chk();
            limpia_combos();
            limpia_otros();
            limpia_chk();
            tx_idr.Text = lib.gofirts(nomtab);
            tx_idr_Leave(null, null);
        }
        private void Bt_back_Click(object sender, EventArgs e)
        {
            if(tx_idr.Text.Trim() != "")
            {
                int aca = int.Parse(tx_idr.Text) - 1;
                limpiar();
                limpia_chk();
                limpia_combos();
                limpia_otros();
                tx_idr.Text = aca.ToString();
                tx_idr_Leave(null, null);
            }
        }
        private void Bt_next_Click(object sender, EventArgs e)
        {
            int aca = int.Parse(tx_idr.Text) + 1;
            limpiar();
            limpia_chk();
            limpia_combos();
            limpia_otros();
            tx_idr.Text = aca.ToString();
            tx_idr_Leave(null, null);
        }
        private void Bt_last_Click(object sender, EventArgs e)
        {
            limpiar();
            limpia_chk();
            limpia_combos();
            limpia_otros();
            tx_idr.Text = lib.golast(nomtab);
            tx_idr_Leave(null, null);
        }
        #endregion botones;
        // proveed para habilitar los botones de comando
        #endregion botones_de_comando  ;

        #region comboboxes
        private void cmb_docRem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_docRem.SelectedIndex > -1)
            {
                tx_dat_tdRem.Text = cmb_docRem.SelectedValue.ToString();
                DataRow[] fila = dttd0.Select("idcodice='" + tx_dat_tdRem.Text + "'");
                foreach (DataRow row in fila)
                {
                    tx_mld.Text = row[2].ToString();
                }
            }
        }
        private void cmb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tx_modo.Text == "NUEVO" && tx_totcant.Text != "")    //  || Tx_modo.Text == "EDITAR"
            {   // lo de totcant es para accionar solo cuando el detalle de la GR se haya cargado
                if (cmb_mon.SelectedIndex > -1)
                {
                    tx_dat_mone.Text = cmb_mon.SelectedValue.ToString();
                    DataRow[] row = dtm.Select("idcodice='"+ tx_dat_mone.Text+"'");
                    tx_dat_monsunat.Text = row[0][2].ToString();
                    tipcambio(tx_dat_mone.Text);
                    if (tx_flete.Text != "" && tx_flete.Text != "0.00") calculos(decimal.Parse(tx_flete.Text));
                    if (rb_no.Checked == true) rb_no_Click(null,null);
                    if (rb_si.Checked == true) rb_si_Click(null, null);
                    if (tx_dat_mone.Text != MonDeft)
                    {
                        tx_flete.ReadOnly = false;
                        tx_flete.Focus();
                    }
                    else
                    {
                        if (decimal.Parse(tx_dat_saldoGR.Text) <= 0)
                        {
                            if (cusdscto.Contains(asd)) tx_flete.ReadOnly = false;
                            else tx_flete.ReadOnly = true;
                        }
                        else
                        {
                            tx_flete.ReadOnly = true;
                        }
                    }
                }
            }
        }
        private void cmb_tdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_tdv.SelectedIndex > -1)
            {
                DataRow[] row = dttd1.Select("idcodice='" + cmb_tdv.SelectedValue.ToString() + "'");
                if (row.Length > 0)
                {
                    tx_dat_tdv.Text = row[0].ItemArray[0].ToString();
                    tx_dat_tdec.Text = row[0].ItemArray[2].ToString();
                    glosser = row[0].ItemArray[4].ToString();
                    if (Tx_modo.Text == "NUEVO") tx_serie.Text = row[0].ItemArray[5].ToString();
                    tx_numero.Text = "";
                }
            }
        }
        private void cmb_plazoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            /*if (cmb_plazoc.SelectedIndex > -1)
            {
                tx_dat_plazo.Text = cmb_plazoc.SelectedValue.ToString();
                DataRow[] dias = dtp.Select("idcodice='" + tx_dat_plazo.Text + "'");
                foreach (DataRow row in dias)
                {
                    tx_dat_dpla.Text = row[3].ToString();
                }
            }
            else
            {
                tx_dat_plazo.Text = "";
                tx_dat_dpla.Text = "";
            }*/
        }
        private void cmb_plazoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_plazoc.SelectedIndex > -1)
            {
                tx_dat_plazo.Text = cmb_plazoc.SelectedValue.ToString();
                DataRow[] dias = dtp.Select("idcodice='" + tx_dat_plazo.Text + "'");
                foreach (DataRow row in dias)
                {
                    tx_dat_dpla.Text = row[3].ToString();
                }
            }
            else
            {
                tx_dat_plazo.Text = "";
                tx_dat_dpla.Text = "";
            }
        }
        #endregion comboboxes

        #region impresion
        private void imprimeGRE()       // imprime GRET del comprobante
        {
            for (int i = 0; i <= dataGridView1.Rows.Count -1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() != "")
                {
                    string[] grt = dataGridView1.Rows[i].Cells[0].Value.ToString().Split('-');
                    if (grt[0].Substring(0,1) == v_iniGRET) lp.muestra_gr(grt[0], grt[1], "", (rutaQR + nomImgQR), gloDeta, v_impTK, "TK", "");
                }
            }
        }
        private bool imprimeA4()
        {
            bool retorna = false;

            return retorna;
        }
        private bool imprimeA5()
        {
            bool retorna = false;
            //llenaDataSet();                         // metemos los datos al dataset de la impresion
            return retorna;
        }
        private bool imprimeTK()
        {
            bool retorna = false;
            // imprime la clase
            //if (vs[12] == "") llena_matris_FE();      18/05/2024
            llena_matris_FE();
            impDVs imp = new impDVs();
            imp.impDV(1, v_impTK, vs, dt, va, cu, vi_formato, v_CR_gr_ind, false);

            if (File.Exists(@otro))
            {
                //File.Delete(@"C:\test.txt");
                File.Delete(@otro);
            }
            /*
            try
            {
                printDocument1.PrinterSettings.PrinterName = v_impTK;
                printDocument1.Print();
                retorna = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error en imprimir TK");
                retorna = false;
            }
            */
            return retorna;
        }
        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (vi_formato == "A4")
            {
                imprime_A4(sender, e);
            }
            if (vi_formato == "A5")
            {
                imprime_A5(sender, e);
            }
            if (vi_formato == "TK")
            {
                // imprime la clase
                if (vs[12] == "") llena_matris_FE();
                impDVs imp = new impDVs();
                imp.impDV(1, v_impTK, vs, dt, va, cu, vi_formato, v_CR_gr_ind, false);

                if (File.Exists(@otro))
                {
                    //File.Delete(@"C:\test.txt");
                    File.Delete(@otro);
                }
            }
        }
        private void imprime_A4(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }
        private void imprime_A5(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float alfi = 20.0F;     // alto de cada fila
            float alin = 50.0F;     // alto inicial
            float posi = 80.0F;     // posición de impresión
            float coli = 20.0F;     // columna mas a la izquierda
            float cold = 80.0F;
            Font lt_tit = new Font("Arial", 11);
            Font lt_titB = new Font("Arial", 11, FontStyle.Bold);
            PointF puntoF = new PointF(coli, alin);
            e.Graphics.DrawString(nomclie, lt_titB, Brushes.Black, puntoF, StringFormat.GenericTypographic);                      // titulo del reporte
            posi = posi + alfi;
            string numguia = "GR " + tx_serie.Text + "-" + tx_numero.Text;
            float lt = (lp.CentimeterToPixel(this,21F) - e.Graphics.MeasureString(numguia, lt_titB).Width) / 2;
            puntoF = new PointF(lt, posi);
            e.Graphics.DrawString(numguia, lt_titB, Brushes.Black, puntoF, StringFormat.GenericTypographic);                      // titulo del reporte
            posi = posi + alfi*2;
            PointF ptoimp = new PointF(coli, posi);                     // fecha de emision
            e.Graphics.DrawString("EMITIDO: " + tx_fechope.Text.Substring(0,10), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi + 30.0F;                                         // avance de fila
            ptoimp = new PointF(coli, posi);                               // direccion partida
            e.Graphics.DrawString("REMITENTE: " + tx_nomRem.Text.Trim(), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi;
            ptoimp = new PointF(coli, posi);                       // destinatario
            posi = posi + alfi * 2;
            /*
            // seleccion de impresion en ruc u otro tipo
            ptoimp = new PointF(coli + 50.0F, posi);
            e.Graphics.DrawString(tx_numDocRem.Text, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            ptoimp = new PointF(colm + 185.0F, posi);
            e.Graphics.DrawString(tx_numDocDes.Text, lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = 330.0F;             // avance de fila
            */
            // detalle de la pre guia
            for (int fila = 0; fila < dataGridView1.Rows.Count - 1; fila++)
            {
                ptoimp = new PointF(coli + 20.0F, posi);
                e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[0].Value.ToString(), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(cold, posi);
                e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[1].Value.ToString(), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(cold + 80.0F, posi);
                e.Graphics.DrawString(dataGridView1.Rows[fila].Cells[2].Value.ToString(), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                ptoimp = new PointF(cold + 400.0F, posi);
                e.Graphics.DrawString("KGs." + dataGridView1.Rows[fila].Cells[3].Value.ToString(), lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
                posi = posi + alfi;             // avance de fila
            }
            // guias del cliente
            posi = posi + alfi;
            ptoimp = new PointF(coli, posi);
            e.Graphics.DrawString("Docs. de remisión: ", lt_tit, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            // imprime el flete
            posi = posi + alfi * 2;
            string gtotal = "FLETE " + cmb_mon.Text + " " + tx_flete.Text;
            lt = (lp.CentimeterToPixel(this,21F) - e.Graphics.MeasureString(gtotal, lt_titB).Width) / 2;
            ptoimp = new PointF(lt, posi);
            e.Graphics.DrawString(gtotal, lt_titB, Brushes.Black, ptoimp, StringFormat.GenericTypographic);
            posi = posi + alfi;

        }
        private void imprime_TK(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            {
                // DATOS PARA EL TICKET
                string nomclie = Program.cliente;
                string rasclie = Program.cliente;
                string rucclie = Program.ruc;
                string dirclie = Program.dirfisc;
                // TIPOS DE LETRA PARA EL DOCUMENTO FORMATO TICKET
                Font lt_gra = new Font("Arial", 11);                // grande
                Font lt_tit = new Font("Lucida Console", 10);       // mediano
                Font lt_med = new Font("Arial", 9);                // normal textos
                Font lt_peq = new Font("Arial", 8);                 // pequeño
                                                                    //
                float anchTik = 7.8F;                               // ancho del TK en centimetros
                int coli = 5;                                      // columna inicial
                float posi = 20;                                    // posicion x,y inicial
                int alfi = 15;                                      // alto de cada fila
                float ancho = 360.0F;                                // ancho de la impresion
                int copias = 1;                                     // cantidad de copias del ticket
                Image photo = Image.FromFile(logoclt);
                for (int i = 1; i <= copias; i++)
                {
                    PointF puntoF = new PointF(coli, posi);
                    // imprimimos el logo o el nombre comercial del emisor
                    if (logoclt != "")
                    {
                        SizeF cuadLogo = new SizeF(CentimeterToPixel(anchTik) - 20.0F, alfi * 6);
                        RectangleF reclogo = new RectangleF(puntoF, cuadLogo);
                        e.Graphics.DrawImage(photo, reclogo);
                    }
                    else
                    {
                        e.Graphics.DrawString(nomclie, lt_gra, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // nombre comercial
                    }
                    float lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(nomclie, lt_gra).Width) / 2;
                    posi = posi + alfi * 7;
                    lt = (ancho - e.Graphics.MeasureString(rasclie, lt_gra).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(rasclie, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // razon social
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Dom.Fiscal", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // direccion emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    SizeF cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    RectangleF recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(dirclie, lt_med, Brushes.Black, recdom, StringFormat.GenericTypographic);     // direccion emisor
                    posi = posi + alfi * 2;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Sucursal", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // direccion emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(dirloc, lt_med, Brushes.Black, recdom, StringFormat.GenericTypographic);     // direccion emisor
                    posi = posi + alfi * 2;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("RUC ", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // ruc de emisor
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(rucclie, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // ruc de emisor
                    //string tipdo = cmb_tdv.Text;                                  // tipo de documento
                    string serie = cmb_tdv.Text.Substring(0, 1).ToUpper() + lib.Right(tx_serie.Text,3);                    // serie electrónica
                    string corre = tx_numero.Text;                                // numero del documento electrónico
                    //string nota = tipdo + "-" + serie + "-" + corre;
                    string titdoc = "";
                    if (tx_dat_tdv.Text != codfact) titdoc = "Boleta de Venta Electrónica";
                    if (tx_dat_tdv.Text == codfact) titdoc = "Factura Electrónica";
                    posi = posi + alfi + 8;
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(titdoc, lt_gra).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(titdoc, lt_gra, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // tipo de documento
                    posi = posi + alfi + 8;
                    string titnum = serie + " - " + corre;
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(titnum, lt_gra).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(titnum, lt_gra, Brushes.Black, puntoF, StringFormat.GenericTypographic);   // serie y numero
                    posi = posi + alfi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("F. Emisión", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic); // fecha y hora emision
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(tx_fechope.Text, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic); // fecha y hora emision
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Cliente", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // DNI/RUC cliente
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    if (tx_nomRem.Text.Trim().Length > 39) cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    else cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 1);
                    recdom = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(tx_nomRem.Text.Trim(), lt_peq, Brushes.Black, recdom, StringFormat.GenericTypographic);                  // DNI/RUC cliente
                    if (tx_nomRem.Text.Trim().Length > 39) posi = posi + alfi + alfi;
                    else posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("RUC", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // nombre del cliente
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    e.Graphics.DrawString(tx_numDocRem.Text, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);    // ruc/dni del cliente
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Dirección", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);  // direccion
                    puntoF = new PointF(coli + 65, posi);
                    e.Graphics.DrawString(":", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    puntoF = new PointF(coli + 70, posi);
                    string dipa = tx_dirRem.Text.Trim() + Environment.NewLine + tx_distRtt.Text.Trim() + " - " + tx_provRtt.Text.Trim() + " - " + tx_dptoRtt.Text.Trim();
                    if (dipa.Length < 60) cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 2);
                    else cuad = new SizeF(CentimeterToPixel(anchTik) - (coli + 70), alfi * 3);
                    RectangleF recdir = new RectangleF(puntoF, cuad);
                    e.Graphics.DrawString(tx_dirRem.Text.Trim() + Environment.NewLine +
                        tx_distRtt.Text.Trim() + " - " + tx_provRtt.Text.Trim() + " - " + tx_dptoRtt.Text.Trim(),
                        lt_peq, Brushes.Black, recdir, StringFormat.GenericTypographic);  // direccion
                    if (dipa.Length < 60) posi = posi + alfi + alfi;
                    else posi = posi + alfi + alfi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString(" ", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    // **************** detalle del documento ****************//
                    StringFormat alder = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                    SizeF siz = new SizeF(70, 15);
                    RectangleF recto = new RectangleF(puntoF, siz);
                    //int tfg = (dataGridView1.Rows.Count == int.Parse(v_mfildet)) ? int.Parse(v_mfildet) : dataGridView1.Rows.Count - 1;
                    int tfg = (dataGridView1.Rows.Count == int.Parse(v_mfildet) && int.Parse(tx_tfil.Text) == int.Parse(v_mfildet)) ? int.Parse(v_mfildet) : dataGridView1.Rows.Count - 1;
                    for (int l = 0; l < tfg; l++)
                    {
                        if (!string.IsNullOrEmpty(dataGridView1.Rows[l].Cells[0].Value.ToString()))
                        {
                            puntoF = new PointF(coli, posi);
                            e.Graphics.DrawString(glosser, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            posi = posi + alfi;
                            puntoF = new PointF(coli, posi);
                            if (glosser2.Trim() != "")
                            {
                                e.Graphics.DrawString(glosser2, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                                posi = posi + alfi;
                                puntoF = new PointF(coli, posi);
                            }
                            //recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString("GRT " + dataGridView1.Rows[l].Cells[0].Value.ToString() + " " + dataGridView1.Rows[l].Cells[1].Value.ToString(), lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            posi = posi + alfi;
                            puntoF = new PointF(coli, posi);
                            e.Graphics.DrawString(dataGridView1.Rows[l].Cells[2].Value.ToString() + " " + 
                                dataGridView1.Rows[l].Cells[11].Value.ToString() +
                                " Guía cliente: " + dataGridView1.Rows[l].Cells[8].Value.ToString(), lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            //posi = posi + alfi;
                            //puntoF = new PointF(coli, posi);
                            //e.Graphics.DrawString("Según doc.cliente: " + dataGridView1.Rows[l].Cells[8].Value.ToString(), lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                            posi = posi + alfi;
                        }
                    }
                    // pie del documento ;
                    if (tx_dat_tdv.Text != codfact)         // BOLETA
                    {
                        //SizeF siz = new SizeF(70, 15);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. GRAVADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_subt.Text, lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. INAFECTA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recig = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString("0.00", lt_peq, Brushes.Black, recig, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. EXONERADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recex = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString("0.00", lt_peq, Brushes.Black, recex, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IGV", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recgv = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_igv.Text, lt_peq, Brushes.Black, recgv, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IMPORTE TOTAL " + cmb_mon.Text, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        recto = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_flete.Text, lt_peq, Brushes.Black, recto, alder);
                    }
                    if (tx_dat_tdv.Text == codfact)     // FACTURA
                    {
                        //SizeF siz = new SizeF(70, 15);
                        //StringFormat alder = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. GRAVADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recst = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_subt.Text, lt_peq, Brushes.Black, recst, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. INAFECTA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recig = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString("0.00", lt_peq, Brushes.Black, recig, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("OP. EXONERADA", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recex = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString("0.00", lt_peq, Brushes.Black, recex, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IGV", lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        RectangleF recgv = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_igv.Text, lt_peq, Brushes.Black, recgv, alder);
                        posi = posi + alfi;
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString("IMPORTE TOTAL " + cmb_mon.Text, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        puntoF = new PointF(coli + 190, posi);
                        recto = new RectangleF(puntoF, siz);
                        e.Graphics.DrawString(tx_flete.Text, lt_peq, Brushes.Black, recto, alder);
                    }
                    posi = posi + alfi * 2;
                    puntoF = new PointF(coli, posi);
                    NumLetra nl = new NumLetra();
                    string monlet = "SON: " + tx_fletLetras.Text;
                    if (monlet.Length <= 30) siz = new SizeF(CentimeterToPixel(anchTik), alfi);
                    else siz = new SizeF(CentimeterToPixel(anchTik), alfi * 2);
                    recto = new RectangleF(puntoF, siz);
                    e.Graphics.DrawString(monlet, lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                    if (monlet.Length <= 30) posi = posi + alfi;
                    else posi = posi + alfi + alfi;
                    if (tx_dat_tdv.Text == codfact)
                    {
                        // forma de pago
                        posi = posi + (alfi/1.5F);
                        string ahiva = "";
                        if (rb_no.Checked == true && rb_credito.Checked == true)
                        {
                            string _fechc = DateTime.Parse(tx_fechope.Text).AddDays(double.Parse(tx_dat_dpla.Text)).Date.ToString("dd-MM-yyyy");    // "yyyy-MM-dd"
                            ahiva = "- AL CREDITO -" + " 1 CUOTA - VCMTO: " + _fechc;
                        }
                        else
                        {
                            ahiva = "PAGO AL CONTADO " + tx_flete.Text;
                        }
                        puntoF = new PointF(coli, posi);
                        e.Graphics.DrawString(ahiva, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                        posi = posi + alfi * 1.5F;
                        // leyenda de detracción
                        if (double.Parse(tx_flete.Text) > double.Parse(Program.valdetra))
                        {
                            siz = new SizeF(CentimeterToPixel(anchTik), 15 * 3);
                            puntoF = new PointF(coli, posi);
                            recto = new RectangleF(puntoF, siz);
                            e.Graphics.DrawString(glosdet.Trim() + " " + Program.ctadetra.Trim(), lt_peq, Brushes.Black, recto, StringFormat.GenericTypographic);
                            posi = posi + alfi * 3;
                        }
                    }
                    puntoF = new PointF(coli, posi);
                    string repre = "Representación impresa de la";
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(repre, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(repre, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    string previo = "";
                    if (tx_dat_tdv.Text != codfact) previo = "boleta de venta electrónica";
                    if (tx_dat_tdv.Text == codfact) previo = "factura electrónica";
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(previo, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(previo, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    //posi = posi + alfi;
                    string separ = "|";
                    string codigo = rucclie + separ + tipdo + separ +
                        serie + separ + tx_numero.Text + separ +
                        tx_igv.Text + separ + tx_flete.Text + separ +
                        tx_fechope.Text.Substring(6,4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2) + separ + tipoDocEmi + separ +
                        tx_numDocRem.Text + separ;  // string.Format("{0:yyyy-MM-dd}", tx_fechope.Text)
                    //
                    var rnd = Path.GetRandomFileName();
                    otro = Path.GetFileNameWithoutExtension(rnd);
                    otro = otro + ".png";
                    //
                    var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                    var qrCode = qrEncoder.Encode(codigo);
                    var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                    using (var stream = new FileStream(otro, FileMode.Create))
                        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    Bitmap png = new Bitmap(otro);
                    posi = posi + alfi + 7;
                    lt = (CentimeterToPixel(anchTik) - lib.CentimeterToPixel(3)) / 2;
                    puntoF = new PointF(lt, posi);
                    SizeF cuadro = new SizeF(lib.CentimeterToPixel(3), lib.CentimeterToPixel(3));    // 5x5 cm
                    RectangleF rec = new RectangleF(puntoF, cuadro);
                    e.Graphics.DrawImage(png, rec);
                    png.Dispose();
                    // leyenda 2
                    posi = posi + lib.CentimeterToPixel(3);
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(restexto, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(restexto, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi;
                    lt = (CentimeterToPixel(anchTik) - e.Graphics.MeasureString(autoriz_OSE_PSE, lt_med).Width) / 2;
                    puntoF = new PointF(lt, posi);
                    e.Graphics.DrawString(tx_dat_sun_autor.Text, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);     // autoriz_OSE_PSE
                    // centrado en rectangulo   *********************
                    StringFormat sf = new StringFormat();       //  *
                    sf.Alignment = StringAlignment.Center;      //  *
                    posi = posi + alfi + 5;
                    SizeF leyen = new SizeF(CentimeterToPixel(anchTik) - 20, alfi * 3);
                    puntoF = new PointF(coli, posi);
                    leyen = new SizeF(CentimeterToPixel(anchTik) - 20, alfi * 2);
                    RectangleF recley5 = new RectangleF(puntoF, leyen);
                    e.Graphics.DrawString(tx_dat_sun_web.Text, lt_med, Brushes.Black, recley5, sf);                                   // webose
                    posi = posi + alfi * 3;
                    string locyus = tx_locuser.Text + " - " + tx_user.Text;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString(locyus, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);                  // tienda y vendedor
                    posi = posi + alfi;
                    puntoF = new PointF(coli, posi);
                    e.Graphics.DrawString("Imp. " + DateTime.Now, lt_peq, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi + alfi;
                    puntoF = new PointF((CentimeterToPixel(anchTik) - e.Graphics.MeasureString(despedida, lt_med).Width) / 2, posi);
                    e.Graphics.DrawString(despedida, lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                    posi = posi + alfi + alfi;
                    //puntoF = new PointF(coli, posi);
                    //e.Graphics.DrawString(".", lt_med, Brushes.Black, puntoF, StringFormat.GenericTypographic);
                }
            }
        }       // desde el 12/02/2024 ya no usamos, ahora se usa la clase impDV
        private void updateprint(string sn)  // actualiza el campo impreso de la GR = S
        {   // S=si impreso || N=no impreso
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "update cabfactu set impreso=@sn where id=@idr";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.Parameters.AddWithValue("@sn", sn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
                }
            }
        }
        #endregion

    }
}
