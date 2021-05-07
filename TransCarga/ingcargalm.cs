﻿using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using MySql.Data.MySqlClient;

namespace TransCarga
{
    public partial class ingcargalm : Form
    {
        static string nomform = "ingcargalm";           // nombre del formulario
        string colback = TransCarga.Program.colbac;     // color de fondo
        string colpage = TransCarga.Program.colpag;     // color de los pageframes
        string colgrid = TransCarga.Program.colgri;     // color de las grillas
        string colfogr = TransCarga.Program.colfog;     // color fondo con grillas
        string colsfon = TransCarga.Program.colsbg;     // color fondo seleccion
        string colsfgr = TransCarga.Program.colsfc;     // color seleccion grilla
        string colstrp = TransCarga.Program.colstr;     // color del strip
        bool conectS = TransCarga.Program.vg_conSol;    // usa conector solorsoft? true=si; false=no
        static string nomtab = "cabplacar";

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
        string codAnul = "";            // codigo de documento anulado
        string codGene = "";            // codigo documento generado
        string codIngA = "";            // codigo documento recepcionado
        string codCier = "";            // codigo planilla cerrada
        string v_clu = "";              // codigo del local del usuario
        string v_slu = "";              // serie del local del usuario
        string v_nbu = "";              // nombre del usuario
        string vi_formato = "";         // formato de impresion del documento
        string vi_copias = "";          // cant copias impresion
        string v_impA4 = "";            // nombre de la impresora matricial
        string v_impTK = "";            // nombre de la ticketera
        string vtc_flete = "";          // el detalle va con el flete impreso ?? SI || NO
        string v_cid = "";              // codigo interno de tipo de documento
        string v_fra1 = "";             // frase de si va o no con clave
        string v_fra2 = "";             // frase 
        string v_sanu = "";             // serie anulacion interna ANU
        string v_CR_gr_ind = "";        // nombre del formato en CR
        //string v_mfildet = "";          // maximo numero de filas en el detalle, coord. con el formato
        string v_trompa = "";           // codigo interno placa de tracto
        string v_carret = "";           // código interno placa de carreta/furgon
        string v_camion = "";           // código interno placa de camion
        string v_mondef = "";           // moneda por defecto del form
        string vint_A0 = "";            // variable INTERNA para amarrar el codigo anulacion cliente con A0
        //
        static libreria lib = new libreria();   // libreria de procedimientos
        publico lp = new publico();             // libreria de clases
        string verapp = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
        string nomclie = Program.cliente;           // cliente usuario del sistema
        string rucclie = Program.ruc;               // ruc del cliente usuario del sistema
        string asd = TransCarga.Program.vg_user;    // usuario conectado al sistema
        #endregion

        // string de conexion
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + login.data + ";";
        DataTable dtu = new DataTable();
        DataTable dtd = new DataTable();
        DataTable dtm = new DataTable();
        DataTable dtf = new DataTable();    // formatos de impresion CR
        string[] retorD = { "", "", "", "", "", "", "", "" };      // datos devueltos de busqueda de planlla y GR

        public ingcargalm()
        {
            InitializeComponent();
        }
        private void ingcargalm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.N) Bt_add.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.E) Bt_edit.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.A) Bt_anul.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.O) Bt_ver.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.P) Bt_print.PerformClick();
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.S) Bt_close.PerformClick();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ingcargalm_Load(object sender, EventArgs e)
        {
            this.Focus();
            jalainfo();
            init();
            dataload();
            toolboton();
            this.KeyPreview = true;
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
            //dataGridView1.DefaultCellStyle.ForeColor = Color.FromName(colfogr);
            //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromName(colsfon);
            //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromName(colsfgr);
            //
            tx_user.Text += asd;
            tx_nomuser.Text = lib.nomuser(asd);
            tx_locuser.Text += lib.locuser(asd);
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
            // longitudes maximas de campos
            tx_serP.MaxLength = 4;
            tx_numP.MaxLength = 8;
            tx_pla_placa.MaxLength = 7;
            tx_pla_carret.MaxLength = 7;
            tx_pla_brevet.MaxLength = 10;
            tx_obser1.MaxLength = 150;
            // campos en mayusculas
            tx_pla_placa.CharacterCasing = CharacterCasing.Upper;
            tx_pla_carret.CharacterCasing = CharacterCasing.Upper;
            // grilla
            armagrilla();
            // todo desabilidado
            sololee();
        }
        private void armagrilla()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "fila";
            dataGridView1.Columns[0].HeaderText = "Fila";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].Name = "serguia";
            dataGridView1.Columns[1].HeaderText = "Ser.GR";
            dataGridView1.Columns[1].ReadOnly = false;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].Name = "numguia";
            dataGridView1.Columns[2].HeaderText = "Num.GR";
            dataGridView1.Columns[2].ReadOnly = false;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Name = "totcant";
            dataGridView1.Columns[3].HeaderText = "Bultos";
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[3].Width = 40;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[4].Name = "nombul";
            dataGridView1.Columns[4].HeaderText = "Nombul";
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[4].Width = 70;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[5].Name = "totpeso";
            dataGridView1.Columns[5].HeaderText = "Peso";
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[5].Width = 70;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                DataGridViewCheckBoxColumn marca = new DataGridViewCheckBoxColumn();
                marca.Name = "Borra";
                marca.HeaderText = "Borra";
                marca.Width = 50;
                marca.ReadOnly = false;
                marca.FillWeight = 20;
                dataGridView1.Columns.Add(marca);
        }
        private void initIngreso()
        {
            limpiar();
            limpia_chk();
            limpia_otros();
            limpia_combos();
            armagrilla();
            tx_fechope.Text = DateTime.Today.ToString("dd/MM/yyyy");
            tx_digit.Text = v_nbu;
            tx_dat_estad.Text = codGene;
            tx_estado.Text = lib.nomstat(tx_dat_estad.Text);
        }
        private void jalainfo()                 // obtiene datos de imagenes y variables
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in (@nofo,@nfin,@nofa)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@nfin", "interno");
                micon.Parameters.AddWithValue("@nofa", nomform);
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
                            if (row["param"].ToString() == "img_preview") img_ver = row["valor"].ToString().Trim();         // imagen del boton grabar visualizar
                        }
                        if (row["campo"].ToString() == "estado")
                        {
                            if (row["param"].ToString() == "anulado") codAnul = row["valor"].ToString().Trim();         // codigo doc anulado
                            if (row["param"].ToString() == "generado") codGene = row["valor"].ToString().Trim();        // codigo doc generado
                            if (row["param"].ToString() == "cerrado") codCier = row["valor"].ToString().Trim();        // codigo planilla cerrada
                        }
                    }
                    if (row["formulario"].ToString() == nomform)
                    {
                        if (row["campo"].ToString() == "documento")
                        {
                            if (row["param"].ToString() == "estplarecep") codIngA = row["valor"].ToString().Trim();           // estado planilla ingresada a alm.
                        }
                    }
                    if (row["formulario"].ToString() == "interno")  // variables configuracion interna, campos especiales de base de datos
                    {
                        if (row["campo"].ToString() == "anulado" && row["param"].ToString() == "A0") vint_A0 = row["valor"].ToString().Trim();
                    }
                }
                da.Dispose();
                dt.Dispose();
                // jalamos datos del usuario y local
                v_clu = lib.codloc(asd);                // codigo local usuario
                v_slu = lib.serlocs(v_clu);             // serie local usuario
                v_nbu = lib.nomuser(asd);               // nombre del usuario
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        private void jalaoc(string campo)        // jala planilla de carga
        {
            {
                string parte = "";
                if (campo == "tx_idr")
                {
                    parte = "where a.id=@ida";
                }
                if (campo == "sernum")
                {
                    parte = "where a.serplacar=@ser and a.numplacar=@num";
                }
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string consulta = "select a.id,a.fechope,a.serplacar,a.numplacar,a.locorigen,a.locdestin,a.obsplacar,a.cantfilas,a.cantotpla,a.pestotpla,a.tipmonpla," +
                        "a.tipcampla,a.subtotpla,a.igvplacar,a.totplacar,a.totpagado,a.salxpagar,a.estadoser,a.impreso,a.fleteimp,a.platracto,a.placarret,a.autorizac," +
                        "a.confvehic,a.brevchofe,a.nomchofe,a.brevayuda,a.nomayuda,a.rucpropie,a.tipoplani,a.userc,a.userm,a.usera,ifnull(b.razonsocial,'') as razonsocial," +
                        "a.marcaTrac,a.modeloTrac " +
                        "FROM cabplacar a left join anag_for b on a.rucpropie=b.ruc and b.estado=0 " + parte;
                    MySqlCommand micon = new MySqlCommand(consulta, conn);
                    if (campo == "tx_idr") micon.Parameters.AddWithValue("@ida", tx_idr.Text);
                    if (campo == "sernum")
                    {
                        micon.Parameters.AddWithValue("@ser", tx_serP.Text);
                        micon.Parameters.AddWithValue("@num", tx_numP.Text);
                    }
                    MySqlDataReader dr = micon.ExecuteReader();
                    if (dr != null)
                    {
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString("id");
                            tx_fechope.Text = dr.GetString("fechope").Substring(0,10);
                            tx_digit.Text = dr.GetString("userc") + " " + dr.GetString("userm") + " " + dr.GetString("usera");
                            //
                            tx_dat_estad.Text = dr.GetString("estadoser");
                            tx_serP.Text = dr.GetString("serplacar");
                            tx_numP.Text = dr.GetString("numplacar");
                            tx_obser1.Text = dr.GetString("obsplacar");
                            tx_tfil.Text = dr.GetString("cantfilas");
                            tx_totcant.Text = dr.GetString("cantotpla");
                            tx_totpes.Text = dr.GetString("pestotpla");
                            tx_dat_detflete.Text = dr.GetString("fleteimp");    // determina si en el detalle se muestra e imprime el valor del flete de la guia
                            //
                            tx_pla_placa.Text = dr.GetString("platracto");
                            tx_pla_carret.Text = dr.GetString("placarret");
                            tx_pla_brevet.Text = dr.GetString("brevchofe");
                            tx_pla_nomcho.Text = dr.GetString("nomchofe");
                        }
                        tx_estado.Text = lib.nomstat(tx_dat_estad.Text);
                        // si el documento esta ANULADO o un estado que no permite EDICION, se pone todo en sololee (ANULADO O RECIBIDO)
                        if (tx_dat_estad.Text != codGene)
                        {
                            sololee();
                            dataGridView1.ReadOnly = true;
                            MessageBox.Show("Este documento no puede ser editado/anulado", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {

                        }
                        button1.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No existe el número buscado!", "Atención - data incorrecto",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    //
                    dr.Dispose();
                    micon.Dispose();
                }
                conn.Close();
            }
        }
        private void jaladet(string idr)         // jala el detalle
        {
            string jalad = "select a.idc,a.serplacar,a.numplacar,a.fila,a.numpreg,a.serguia,a.numguia,a.totcant,floor(a.totpeso) as totpeso,b.descrizionerid as MON,a.totflet," +
                "a.estadoser,a.codmone,'X' as marca,a.id,a.pagado,a.salxcob,g.nombdegri,g.diredegri,g.teledegri,a.nombult,u1.nombre AS distrit,u2.nombre as provin," +
                "concat(d.descrizionerid,'-',if(SUBSTRING(g.serdocvta,1,2)='00',SUBSTRING(g.serdocvta,3,2),g.serdocvta),'-',if(SUBSTRING(g.numdocvta,1,3)='000',SUBSTRING(g.numdocvta,4,5),g.numdocvta))," +
                "g.nombregri " +
                "from detplacar a " +
                "left join desc_mon b on b.idcodice = a.codmone " +
                "left join cabguiai g on g.sergui = a.serguia and g.numgui = a.numguia " +
                "left join desc_tdv d on d.idcodice=g.tipdocvta " + 
                "LEFT JOIN ubigeos u1 ON CONCAT(u1.depart, u1.provin, u1.distri)= g.ubigdegri " +
                "LEFT JOIN(SELECT* FROM ubigeos WHERE depart<>'00' AND provin<>'00' AND distri = '00') u2 ON u2.depart = left(g.ubigdegri, 2) AND u2.provin = concat(substr(g.ubigdegri, 3, 2)) " +
                "where a.idc=@idr";
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
                        dataGridView1.Rows.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Tx_modo.Text != "EDITAR")
                            {
                                dataGridView1.Rows.Add(
                                    row[3].ToString(),
                                    row[4].ToString(),
                                    row[5].ToString(),
                                    row[6].ToString(),
                                    row[7].ToString(),
                                    row[8].ToString(),
                                    row[9].ToString(),
                                    row[10].ToString(),
                                    row[15].ToString(),
                                    row[16].ToString(),
                                    row[12].ToString(),
                                    row[13].ToString(),
                                    row[14].ToString(),
                                    row[17].ToString(),
                                    row[18].ToString() + " - " + row[21].ToString() + " - " + row[22].ToString(),
                                    row[19].ToString(),
                                    row[20].ToString(),
                                    row[23].ToString(),
                                    row[24].ToString()
                                    );
                            }
                            else
                            {
                                dataGridView1.Rows.Add(
                                    row[3].ToString(),
                                    row[4].ToString(),
                                    row[5].ToString(),
                                    row[6].ToString(),
                                    row[7].ToString(),
                                    row[8].ToString(),
                                    row[9].ToString(),
                                    row[10].ToString(),
                                    row[15].ToString(),
                                    row[16].ToString(),
                                    row[12].ToString(),
                                    row[13].ToString(),
                                    row[14].ToString(),
                                    row[17].ToString(),
                                    row[18].ToString() + " - " + row[21].ToString() + " - " + row[22].ToString(),
                                    row[19].ToString(),
                                    row[20].ToString(),
                                    row[23].ToString(),
                                    row[24].ToString(),
                                    false
                                    );
                            }
                        }
                        dt.Dispose();
                    }
                }
            }
            operaciones();
        }
        private void dataload()                  // jala datos para los combos 
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }

            conn.Close();
        }
        private bool valiGri()                  // valida filas completas en la grilla - 8 columnas
        {
            bool retorna = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null &&
                    dataGridView1.Rows[i].Cells[1].Value == null &&
                    dataGridView1.Rows[i].Cells[2].Value == null &&
                    dataGridView1.Rows[i].Cells[3].Value == null &&
                    dataGridView1.Rows[i].Cells[4].Value == null &&
                    dataGridView1.Rows[i].Cells[5].Value == null &&
                    dataGridView1.Rows[i].Cells[6].Value == null &&
                    dataGridView1.Rows[i].Cells[7].Value == null &&
                    dataGridView1.Rows[i].Cells[8].Value == null)
                {
                    // no hay problema
                    retorna = true;
                }
                else
                {
                    if (dataGridView1.Rows[i].Cells[0].Value == null ||
                        dataGridView1.Rows[i].Cells[1].Value == null ||
                        dataGridView1.Rows[i].Cells[2].Value == null ||
                        dataGridView1.Rows[i].Cells[3].Value == null ||
                        dataGridView1.Rows[i].Cells[4].Value == null ||
                        dataGridView1.Rows[i].Cells[5].Value == null ||
                        dataGridView1.Rows[i].Cells[6].Value == null ||
                        dataGridView1.Rows[i].Cells[7].Value == null ||
                        dataGridView1.Rows[i].Cells[8].Value == null)
                    {
                        retorna = false;
                        break;
                    }
                    else
                    {
                        retorna = true;
                    }
                }
            }
            return retorna;
        }
        private bool valiVars()                 // valida existencia de datos en variables del form
        {
            bool retorna = true;
            if (codIngA == "")          // codigo documento INGRESADO AL ALMACEN
            {
                lib.messagebox("Código de planilla INGRESADA");
                retorna = false;
            }
            if (v_clu == "")            // codigo del local del usuario
            {
                lib.messagebox("Código local del usuario");
                retorna = false;
            }
            if (vint_A0 == "")
            {
                lib.messagebox("Cód. Interno enlace Anulado: A0");
                retorna = false;
            }
            return retorna;
        }
        private string[] ValPlaCarr(string pc,string codigo)    // pc=G ó P, codigo=serie+numero
        {
            retorD[0] = ""; retorD[1] = ""; retorD[2] = ""; retorD[3] = ""; retorD[4] = ""; retorD[5] = ""; retorD[6] = ""; retorD[7] = "";
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "";
                if (pc == "P")
                { 
                    consulta = "select b1.descrizione,b2.descrizione,a.platracto,a.placarret,a.brevchofe,a.nomchofe,a.estadoser,space(1) as estalma " +
                        "from cabplacar a left join desc_loc b1 on b1.idcodice=a.locorigen left join desc_loc b2 on b2.idcodice=a.locdestin " +
                        "where concat(a.serplacar,a.numplacar)=@codigo";
                }
                if (pc == "G")
                {
                    consulta = "select b1.descrizione,b2.descrizione,a.plaplagri,a.plaplar2,a.breplagri,space(1 ) as nomchofe,a.estadoser,d.estalma " +
                        "from cabguiai a left join desc_loc b1 on b1.idcodice=a.locorigen left join desc_loc b2 on b2.idcodice=a.locdestin " +
                        "left join controlg d on d.serguitra=a.sergui and d.numguitra=a.numgui " +
                        "where concat(a.sergui,a.numgui)=@codigo";
                }
                using (MySqlCommand micon = new MySqlCommand(consulta,conn))
                {
                    micon.Parameters.AddWithValue("@codigo", codigo);
                    MySqlDataReader dr = micon.ExecuteReader();
                    while (dr.Read())
                    {
                        retorD[0] = dr.GetString(0);   // origen
                        retorD[1] = dr.GetString(1);   // destino
                        retorD[2] = dr.GetString(2);   // placa
                        retorD[3] = dr.GetString(3);   // carreta
                        retorD[4] = dr.GetString(4);   // brevete
                        retorD[5] = dr.GetString(5);   // nombre chofer
                        retorD[6] = dr.GetString(6);   // estado documento
                        retorD[7] = dr.GetString(7);   // estado almacen
                    }
                    dr.Dispose();
                }
            }
            return retorD;
        }
        private void operaciones()              // recalcula los totales de la grilla
        {
            int totfil = 0;
            int totcant = 0;
            decimal totpes = 0;
            decimal totfle = 0, totpag = 0, totsal = 0;
            //a.fila,a.numpreg,a.serguia,a.numguia,a.totcant,a.totpeso,b.descrizionerid as MON,a.totflet,a.totpag,a.salgri
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Tx_modo.Text == "EDITAR")
                {
                    if (dataGridView1.Rows.Count == 13 && dataGridView1.Rows[i].Cells[13].Value != null)
                    {
                        if (dataGridView1.Rows[i].Cells[13].Value.ToString() == "False")
                        {
                            if (dataGridView1.Rows[i].Cells[4].Value != null)
                            {
                                totcant = totcant + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                                totfil += 1;
                            }
                            if (dataGridView1.Rows[i].Cells[5].Value != null)
                            {
                                totpes = totpes + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                            }
                            if (dataGridView1.Rows[i].Cells[7].Value != null)
                            {
                                totfle = totfle + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                                totpag = totpag + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                                totsal = totsal + decimal.Parse(dataGridView1.Rows[i].Cells[9].Value.ToString());
                            }
                        }
                    }
                    else
                    {
                        //MessageBox.Show(dataGridView1.Rows[i].Cells[13].Value.ToString(),"fila: " + i.ToString());
                        if (dataGridView1.Rows[i].Cells[4].Value != null)
                        {
                            totcant = totcant + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                            totfil += 1;
                        }
                        if (dataGridView1.Rows[i].Cells[5].Value != null)
                        {
                            totpes = totpes + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                        }
                        if (dataGridView1.Rows[i].Cells[7].Value != null)
                        {
                            totfle = totfle + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                            totpag = totpag + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                            totsal = totsal + decimal.Parse(dataGridView1.Rows[i].Cells[9].Value.ToString());
                        }
                    }
                }
                else
                {
                    if (dataGridView1.Rows[i].Cells[4].Value != null)
                    {
                        totcant = totcant + int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                        totfil += 1;
                    }
                    if (dataGridView1.Rows[i].Cells[5].Value != null)
                    {
                        totpes = totpes + decimal.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    }
                    if (dataGridView1.Rows[i].Cells[7].Value != null)
                    {
                        totfle = totfle + decimal.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                        totpag = totpag + decimal.Parse(dataGridView1.Rows[i].Cells[8].Value.ToString());
                        totsal = totsal + decimal.Parse(dataGridView1.Rows[i].Cells[9].Value.ToString());
                    }
                }
            }
            tx_totcant.Text = totcant.ToString();
            tx_totpes.Text = totpes.ToString("0.00");
            tx_tfil.Text = totfil.ToString();
            dataGridView1.AllowUserToAddRows = true;
        }

        #region limpiadores_modos
        private void sololee()
        {
            lp.sololee(this);
        }
        private void escribe()
        {
            lp.escribe(this);
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

        }
        private void limpia_combos()
        {
            lp.limpia_cmb(this);
        }
        #endregion limpiadores_modos;

        #region boton_form GRABA EDITA ANULA
        private void bt_Agr_Click(object sender, EventArgs e)
        {
            if (rb_plani.Checked == true && (tx_serP.Text.Trim() == "" || tx_numP.Text.Trim() == ""))
            {
                MessageBox.Show("Ingrese correctamente la planilla de carga", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_serP.Focus();
                return;
            }
            if (rb_manual.Checked == true && (tx_serGR.Text.Trim() == "" || tx_numGR.Text.Trim() == ""))
            {
                MessageBox.Show("Ingrese correctamente la guía de remisión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tx_serGR.Focus();
                return;
            }
            if (tx_serP.Text.Trim() != "")
            {
                // jalamos igual que edicion de planillas de carga
                string jalad = "select a.idc,a.serplacar,a.numplacar,a.fila,a.serguia,a.numguia,a.totcant,floor(a.totpeso) as totpeso," +
                "a.estadoser,'X' as marca,a.id,g.nombdegri,g.diredegri,a.nombult,g.nombregri " +
                "from detplacar a " +
                "left join cabguiai g on g.sergui = a.serguia and g.numgui = a.numguia " +
                "where a.serplacar=@serp and a.numplacar=@nump";
                using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                {
                    conn.Open();
                    using (MySqlCommand micon = new MySqlCommand(jalad, conn))
                    {
                        micon.Parameters.AddWithValue("@serp", tx_serP.Text);
                        micon.Parameters.AddWithValue("@nump", tx_numP.Text);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dataGridView1.Rows.Clear();
                            foreach (DataRow row in dt.Rows)
                            {
                                dataGridView1.Rows.Add(
                                    row[3].ToString(),
                                    row[4].ToString(),
                                    row[5].ToString(),
                                    row[6].ToString(),
                                    row[14].ToString(),
                                    row[7].ToString(),
                                    false
                                    );
                                    /*row[10].ToString(),
                                    row[15].ToString(),
                                    row[16].ToString(),
                                    row[12].ToString(),
                                    row[13].ToString(),
                                    
                                    row[17].ToString(),
                                    row[18].ToString() + " - " + row[21].ToString() + " - " + row[22].ToString(),
                                    row[19].ToString(),
                                    row[20].ToString(),
                                    row[23].ToString(),
                                    row[24].ToString(),
                                    */
                            }
                            dt.Dispose();
                        }
                    }
                }
                operaciones();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region validaciones
            if (tx_serP.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la serie de la planilla", "Complete la información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tx_serP.Focus();
                return;
            }
            #endregion
            // recalculamos totales 
            operaciones();
            // grabamos, actualizamos, etc
            string modo = Tx_modo.Text;
            string iserror = "no";
            //MessageBox.Show(tx_pla_confv.Text + "-" + tx_carret_conf.Text);
            if (modo == "NUEVO")
            {
                // valida que las filas de la grilla esten completas
                if (valiGri() != true)
                {
                    MessageBox.Show("Complete las filas del detalle", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //dataGridView1.Focus();
                    return;
                }
                if (tx_idr.Text.Trim() == "")
                {
                    var aa = MessageBox.Show("Confirma que desea crear la planilla?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (aa == DialogResult.Yes)
                    {
                        if (graba() == true)
                        {
                            var bb = MessageBox.Show("Desea imprimir la planilla?" + Environment.NewLine +
                                "El formato actual es " + vi_formato, "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (bb == DialogResult.Yes)
                            {
                                Bt_print.PerformClick();
                            }
                        }
                        else
                        {
                            iserror = "si";
                        }
                    }
                    else
                    {
                        //tx_numDocRem.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Los datos no son nuevos", "Verifique duplicidad", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            if (modo == "EDITAR")
            {
                if (tx_numP.Text.Trim() == "")
                {
                    MessageBox.Show("Ingrese el número de la planilla", "Complete la información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tx_numP.Focus();
                    return;
                }
                if (tx_dat_estad.Text == codAnul)
                {
                    MessageBox.Show("La planilla esta anulada", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (tx_dat_estad.Text != codGene)
                {
                    MessageBox.Show("La planilla tiene estado que impide su edición", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (true)   // de momento no validamos mas
                {
                    if (tx_idr.Text.Trim() != "")
                    {
                        var aa = MessageBox.Show("Confirma que desea modificar la planilla?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            if (edita() == true)
                            {
                                // 
                            }
                            else
                            {
                                iserror = "si";
                            }
                        }
                        else
                        {
                            //tx_dat_tdRem.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("La Planilla ya debe existir para editar", "Debe ser edición", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
            }
            if (modo == "ANULAR")
            {
                // EN ESTE FORM, LA ANULACION ES FISICA PORQUE SU NUMERACION ES AUTOMATICA
                // si se anula, se tiene que desenlazar en todas sus guías y en control

                if (tx_dat_estad.Text != codAnul)   // (tx_pla_plani.Text.Trim() == "") && tx_impreso.Text == "N"
                {
                    if (tx_idr.Text.Trim() != "")
                    {
                        var aa = MessageBox.Show("Confirma que desea ANULAR la planilla?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (aa == DialogResult.Yes)
                        {
                            if (anula() == true)
                            {
                                // todo bien
                            }
                            else
                            {
                                iserror = "si";
                            }
                        }
                        else
                        {
                            //tx_dat_tdRem.Focus();
                            return;
                        }
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
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                initIngreso();          // limpiamos todo para volver a empesar
                armagrilla();
                return;
            }
        }
        private bool graba()
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if(conn.State == ConnectionState.Open)
            {
                int vtip = 0;
                string inserta = "insert into cabplacar (" +
                    "fechope,serplacar,locorigen,locdestin,obsplacar,cantfilas,cantotpla,pestotpla,tipmonpla,tipcampla,subtotpla," +
                    "igvplacar,totplacar,totpagado,salxpagar,estadoser,fleteimp,platracto,placarret,autorizac,confvehic,brevchofe," +
                    "brevayuda,rucpropie,tipoplani,nomchofe,nomayuda,marcaTrac,modeloTrac," +
                    "verApp,userc,fechc,diriplan4,diripwan4,netbname) " +
                    "values (@fecho,@serpl,@locor,@locde,@obspl,@cantf,@canto,@pesto,@tipmo,@tipca,@subto," +
                    "@igvpl,@totpl,@totpa,@salxp,@estad,@fleim,@platr,@placa,@autor,@confv,@brevc," +
                    "@breva,@rucpr,@tipop,@nocho,@noayu,@marca,@model," +
                    "@verApp,@asd,now(),@iplan,@ipwan,@nbnam)";
                using (MySqlCommand micon = new MySqlCommand(inserta, conn))
                {
                    micon.Parameters.AddWithValue("@fecho", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                    micon.Parameters.AddWithValue("@serpl", tx_serP.Text);
                    micon.Parameters.AddWithValue("@obspl", tx_obser1.Text);
                    micon.Parameters.AddWithValue("@cantf", tx_tfil.Text);      // cantidad filas detalle
                    micon.Parameters.AddWithValue("@canto", tx_totcant.Text);   // cant total de bultos
                    micon.Parameters.AddWithValue("@pesto", tx_totpes.Text);    // peso total
                    micon.Parameters.AddWithValue("@tipca", "0.00");
                    micon.Parameters.AddWithValue("@subto", "0.00");
                    micon.Parameters.AddWithValue("@igvpl", "0.00");
                    micon.Parameters.AddWithValue("@estad", tx_dat_estad.Text);
                    micon.Parameters.AddWithValue("@fleim", tx_dat_detflete.Text);      // variable si detalle lleva valores flete guias
                    micon.Parameters.AddWithValue("@platr", tx_pla_placa.Text);
                    micon.Parameters.AddWithValue("@placa", tx_pla_carret.Text);
                    micon.Parameters.AddWithValue("@brevc", tx_pla_brevet.Text);
                    micon.Parameters.AddWithValue("@nocho", tx_pla_nomcho.Text);           // nombre del chofer
                    micon.Parameters.AddWithValue("@tipop", vtip);              // tipo planilla, tipo transporte/transportista
                    micon.Parameters.AddWithValue("@verApp", verapp);
                    micon.Parameters.AddWithValue("@asd", asd);
                    micon.Parameters.AddWithValue("@iplan", lib.iplan());
                    micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                    micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                    try
                    {
                        micon.ExecuteNonQuery();
                    }
                    catch(MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Validación Interna", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return retorna;
                    }
                }
                using (MySqlCommand micon = new MySqlCommand("select last_insert_id()", conn))
                {
                    using (MySqlDataReader dr = micon.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // numplacar numeracion automatica estilo pre guias
                            tx_numP.Text = lib.Right("0000000" + dr.GetString(0), 8);
                            tx_idr.Text = dr.GetString(0);
                            retorna = true;
                        }
                    }
                }
                // detalle
                if (dataGridView1.Rows.Count > 0)
                {
                    int fila = 1;
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null)
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() != "")
                            {
                                string inserd2 = "insert into detplacar (idc,serplacar,numplacar,fila,numpreg,serguia,numguia,totcant,totpeso,totflet,codmone,estadoser,origreg," +
                                    "verApp,userc,fechc,diriplan4,diripwan4,netbname,platracto,placarret,autorizac,confvehic,brevchofe,brevayuda,rucpropiet,fechope,pagado,salxcob) " +
                                    "values (@idr,@serpl,@numpl,@fila,@numpr,@sergu,@numgu,@totca,@totpe,@totfl,@codmo,@estad,@orireg," +
                                    "@verApp,@asd,now(),@iplan,@ipwan,@nbnam,@platr,@placa,@autor,@confv,@brevc,@breva,@rucpr,@fecho,@paga,@xcob)";
                                using (MySqlCommand micon = new MySqlCommand(inserd2, conn))
                                {
                                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                                    micon.Parameters.AddWithValue("@fecho", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                                    micon.Parameters.AddWithValue("@serpl", tx_serP.Text);
                                    micon.Parameters.AddWithValue("@numpl", tx_numP.Text);
                                    micon.Parameters.AddWithValue("@fila", fila);
                                    micon.Parameters.AddWithValue("@numpr", dataGridView1.Rows[i].Cells[1].Value.ToString());
                                    micon.Parameters.AddWithValue("@sergu", dataGridView1.Rows[i].Cells[2].Value.ToString());
                                    micon.Parameters.AddWithValue("@numgu", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                    micon.Parameters.AddWithValue("@totca", dataGridView1.Rows[i].Cells[4].Value.ToString());
                                    micon.Parameters.AddWithValue("@totpe", dataGridView1.Rows[i].Cells[5].Value.ToString());
                                    micon.Parameters.AddWithValue("@totfl", dataGridView1.Rows[i].Cells[7].Value.ToString());
                                    micon.Parameters.AddWithValue("@codmo", dataGridView1.Rows[i].Cells[10].Value.ToString());
                                    micon.Parameters.AddWithValue("@estad", tx_dat_estad.Text);
                                    micon.Parameters.AddWithValue("@orireg", "M");              // origen del registro manual, cuando viene desde el form de guias es A
                                    micon.Parameters.AddWithValue("@verApp", verapp);
                                    micon.Parameters.AddWithValue("@asd", asd);
                                    micon.Parameters.AddWithValue("@iplan", lib.iplan());
                                    micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                                    micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                                    micon.Parameters.AddWithValue("@platr", tx_pla_placa.Text);
                                    micon.Parameters.AddWithValue("@placa", tx_pla_carret.Text);
                                    micon.Parameters.AddWithValue("@brevc", tx_pla_brevet.Text);
                                    micon.Parameters.AddWithValue("", tx_pla_nomcho.Text);           // nombre del chofer
                                    micon.Parameters.AddWithValue("@paga", dataGridView1.Rows[i].Cells[8].Value.ToString());    // 
                                    micon.Parameters.AddWithValue("@xcob", dataGridView1.Rows[i].Cells[9].Value.ToString());    // 
                                    //a.fila,a.numpreg,a.serguia,a.numguia,a.totcant,a.totpeso,b.descrizionerid as MON,a.totflet,a.totpag,a.salgri,a.codmon
                                    micon.ExecuteNonQuery();
                                    fila += 1;
                                    retorna = true;         // no hubo errores!
                                }
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
        private bool edita()
        {
            bool retorna = false;
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State == ConnectionState.Open)
            {
                //try
                {
                    if (tx_dat_estad.Text == codGene)               // solo edita estado GENERADO, otro estado no se edita
                    {                                               // El estado cambia solo cuando: SE CIERRA MANUALMENTE ó CUANDO SE RECEPCIONA LA PLANILLA
                        int vtip = 0;                               // los datos que NO SE EDITAN son: serie,numero,origen y destino
                        string actua = "update cabplacar set " +
                            "fechope=@fecho,obsplacar=@obspl,cantfilas=@cantf,cantotpla=@canto,pestotpla=@pesto,tipmonpla=@tipmo," +
                            "tipcampla=@tipca,subtotpla=@subto,igvplacar=@igvpl,totplacar=@totpl,totpagado=@totpa,salxpagar=@salxp,fleteimp=@fleim," +
                            "platracto=@platr,placarret=@placa,autorizac=@autor,confvehic=@confv,brevchofe=@brevc,brevayuda=@breva,rucpropie=@rucpr,tipoplani=@tipop," +
                            "verApp=@verApp,userm=@asd,fechm=now(),diriplan4=@iplan,diripwan4=@ipwan,netbname=@nbnam,nomchofe=@nocho,nomayuda=@noayu,estadoser=@estad," +
                            "marcaTrac=@marca,modeloTrac=@model " +
                            "where serplacar=@serpl and numplacar=@numpl";
                        MySqlCommand micon = new MySqlCommand(actua, conn);
                        micon.Parameters.AddWithValue("@fecho", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                        micon.Parameters.AddWithValue("@serpl", tx_serP.Text);
                        micon.Parameters.AddWithValue("@numpl", tx_numP.Text);
                        //micon.Parameters.AddWithValue("@locor", tx_dat_locori.Text);
                        //micon.Parameters.AddWithValue("@locde", tx_dat_locdes.Text);
                        micon.Parameters.AddWithValue("@obspl", tx_obser1.Text);
                        micon.Parameters.AddWithValue("@cantf", tx_tfil.Text);      // cantidad filas detalle
                        micon.Parameters.AddWithValue("@canto", tx_totcant.Text);   // cant total de bultos
                        micon.Parameters.AddWithValue("@pesto", tx_totpes.Text);    // peso total
                        micon.Parameters.AddWithValue("@tipca", "0.00");
                        micon.Parameters.AddWithValue("@subto", "0.00");
                        micon.Parameters.AddWithValue("@igvpl", "0.00");
                        micon.Parameters.AddWithValue("@fleim", tx_dat_detflete.Text);      // variable si detalle lleva valores flete guias
                        micon.Parameters.AddWithValue("@platr", tx_pla_placa.Text);
                        micon.Parameters.AddWithValue("@placa", tx_pla_carret.Text);
                        micon.Parameters.AddWithValue("@brevc", tx_pla_brevet.Text);
                        micon.Parameters.AddWithValue("@nocho", tx_pla_nomcho.Text);           // nombre del chofer
                        micon.Parameters.AddWithValue("@tipop", vtip);              // tipo planilla, tipo transporte/transportista
                        micon.Parameters.AddWithValue("@estad", codGene);
                        micon.Parameters.AddWithValue("@verApp", verapp);
                        micon.Parameters.AddWithValue("@asd", asd);
                        micon.Parameters.AddWithValue("@iplan", lib.iplan());
                        micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                        micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                        micon.ExecuteNonQuery();
                        //
                        // EDICION DEL DETALLE 
                        /*
                            Las filas marcadas SE BORRAN
                            Las filas NUEVAS SE INSERTAN
                            Las filas cambiasas NO HACE CASO O NO PERMITE EL CAMBIO, solo se permite borrar o agregar filas
                        */
                        int fila = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[17].Value != null)   // fila marcada para borrar
                            {
                                // saca la guia de detplacar
                                if (dataGridView1.Rows[i].Cells[17].Value.ToString() == "True")
                                {
                                    string consulta = "borraseguro";
                                    using (MySqlCommand comed = new MySqlCommand(consulta, conn))
                                    {
                                        comed.CommandType = CommandType.StoredProcedure;
                                        comed.Parameters.AddWithValue("@tabla", "detplacar");
                                        comed.Parameters.AddWithValue("@vidr", int.Parse(dataGridView1.Rows[i].Cells[12].Value.ToString()));
                                        comed.Parameters.AddWithValue("@vidc", 0);
                                        try
                                        {
                                            comed.ExecuteNonQuery();
                                            // trigger borra los campos en cabguiai
                                            // trigger borra los campos en controlg
                                        }
                                        catch (MySqlException ex)
                                        {
                                            MessageBox.Show("Ocurrió un error en el proceso de borrar la guía de la planilla" + Environment.NewLine +
                                                "y / o en la actualización posterior en Guías y Control " + Environment.NewLine +
                                                ex.Message, "Alerta proceso no concluido!");
                                        }
                                    }
                                }
                            }
                            if (dataGridView1.Rows[i].Cells[11].Value == null)   // fila nueva, se inserta  || .ToString() != "X"
                            {
                                string inserd2 = "insert into detplacar (idc,serplacar,numplacar,fila,numpreg,serguia,numguia,totcant,totpeso,totflet,codmone,estadoser,origreg," +
                                "verApp,userc,fechc,diriplan4,diripwan4,netbname,nombult," +
                                "platracto,placarret,autorizac,confvehic,brevchofe,brevayuda,rucpropiet,fechope,pagado,salxcob) " +
                                "values (@idr,@serpl,@numpl,@fila,@numpr,@sergu,@numgu,@totca,@totpe,@totfl,@codmo,@estad,@orireg," +
                                "@verApp,@asd,now(),@iplan,@ipwan,@nbnam,@nombu," +
                                "@platr,@placa,@autor,@confv,@brevc,@breva,@rucpr,@fecho,@paga,@xcob)";
                                micon = new MySqlCommand(inserd2, conn);
                                micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                                micon.Parameters.AddWithValue("@serpl", tx_serP.Text);
                                micon.Parameters.AddWithValue("@numpl", tx_numP.Text);
                                micon.Parameters.AddWithValue("@fila", fila);
                                micon.Parameters.AddWithValue("@numpr", dataGridView1.Rows[i].Cells[1].Value.ToString());
                                micon.Parameters.AddWithValue("@sergu", dataGridView1.Rows[i].Cells[2].Value.ToString());
                                micon.Parameters.AddWithValue("@numgu", dataGridView1.Rows[i].Cells[3].Value.ToString());
                                micon.Parameters.AddWithValue("@totca", dataGridView1.Rows[i].Cells[4].Value.ToString());
                                micon.Parameters.AddWithValue("@nombu", dataGridView1.Rows[i].Cells[16].Value.ToString());
                                micon.Parameters.AddWithValue("@totpe", dataGridView1.Rows[i].Cells[5].Value.ToString());
                                micon.Parameters.AddWithValue("@totfl", dataGridView1.Rows[i].Cells[7].Value.ToString());
                                micon.Parameters.AddWithValue("@estad", tx_dat_estad.Text);
                                micon.Parameters.AddWithValue("@orireg", "M");              // origen del registro manual, cuando viene desde el form de guias es A
                                micon.Parameters.AddWithValue("@verApp", verapp);
                                micon.Parameters.AddWithValue("@asd", asd);
                                micon.Parameters.AddWithValue("@iplan", lib.iplan());
                                micon.Parameters.AddWithValue("@ipwan", TransCarga.Program.vg_ipwan);
                                micon.Parameters.AddWithValue("@nbnam", Environment.MachineName);
                                micon.Parameters.AddWithValue("@platr", tx_pla_placa.Text);
                                micon.Parameters.AddWithValue("@placa", tx_pla_carret.Text);
                                micon.Parameters.AddWithValue("@brevc", tx_pla_brevet.Text);
                                micon.Parameters.AddWithValue("", tx_pla_nomcho.Text);           // nombre del chofer
                                micon.Parameters.AddWithValue("@fecho", tx_fechope.Text.Substring(6, 4) + "-" + tx_fechope.Text.Substring(3, 2) + "-" + tx_fechope.Text.Substring(0, 2));
                                micon.Parameters.AddWithValue("@paga", dataGridView1.Rows[i].Cells[8].Value.ToString());    // 
                                micon.Parameters.AddWithValue("@xcob", dataGridView1.Rows[i].Cells[9].Value.ToString());    // 
                                micon.ExecuteNonQuery();
                            }
                        }
                        micon.Dispose();
                        string conupd = "numdetpla";                                    // numeramos las filas de la planilla
                        using (MySqlCommand comup = new MySqlCommand(conupd, conn))     // secuencialmente del 1 al infinito
                        {
                            comup.CommandType = CommandType.StoredProcedure;
                            comup.Parameters.AddWithValue("@vseri", tx_serP.Text);
                            comup.Parameters.AddWithValue("@vnume", tx_numP.Text);
                            comup.ExecuteNonQuery();
                        }
                        retorna = true;
                    }
                    conn.Close();
                }
                /*catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en modificar la planilla");
                    Application.Exit();
                    return retorna;
                }*/
            }
            else
            {
                MessageBox.Show("No fue posible conectarse al servidor de datos");
                Application.Exit();
                return retorna;
            }
            return retorna;
        }
        private bool anula()
        {
            bool retorna = false;
            // cambia estado a ANULADO en cabecera
            // el trigger after_update debe cambiar estado ANULADO en detalle
            // el trigger after_update debe borrar los campos de enlace en cabguiai y controlg
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string canul = "update cabplacar set estadoser=@estser,usera=@asd,fecha=now()," +
                        "verApp=@veap,diriplan4=@dil4,diripwan4=@diw4,netbname=@nbnp,estintreg=@eirA0 " +
                        "where id=@idr";
                    using (MySqlCommand micon = new MySqlCommand(canul, conn))
                    {
                        micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                        micon.Parameters.AddWithValue("@estser", codAnul);
                        micon.Parameters.AddWithValue("@asd", asd);
                        micon.Parameters.AddWithValue("@dil4", lib.iplan());
                        micon.Parameters.AddWithValue("@diw4", TransCarga.Program.vg_ipwan);
                        micon.Parameters.AddWithValue("@nbnp", Environment.MachineName);
                        micon.Parameters.AddWithValue("@veap", verapp);
                        micon.Parameters.AddWithValue("@eirA0", (vint_A0 == codAnul) ? "A0" : "");  // codigo anulacion interna en DB A0
                        micon.ExecuteNonQuery();
                        retorna = true;
                    }
                }
            }
            return retorna;
        }
        #endregion boton_form;

        #region leaves y checks
        private void tx_idr_Leave(object sender, EventArgs e)
        {
            if (Tx_modo.Text != "NUEVO" && tx_idr.Text != "")
            {
                jalaoc("tx_idr");
                jaladet(tx_idr.Text);
            }
        }
        private void rb_plani_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_plani.Checked == true)
            {
                tx_serP.ReadOnly = false;
                tx_numP.ReadOnly = false;
                tx_serGR.ReadOnly = true;
                tx_serGR.Text = "";
                tx_numGR.ReadOnly = true;
                tx_numGR.Text = "";
                tx_serP.Focus();
            }
        }
        private void rb_manual_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_manual.Checked == true)
            {
                tx_serP.ReadOnly = true;
                tx_serP.Text = "";
                tx_numP.ReadOnly = true;
                tx_numP.Text = "";
                tx_serGR.ReadOnly = false;
                tx_numGR.ReadOnly = false;
                tx_serGR.Focus();
            }
        }
        private void tx_serP_Leave(object sender, EventArgs e)
        {
            if (tx_serP.Text.Trim() != "") tx_serP.Text = lib.Right("0000" + tx_serP.Text.Trim(),4);
        }
        private void tx_numP_Leave(object sender, EventArgs e)
        {
            if (tx_numP.Text.Trim() != "")
            {
                tx_numP.Text = lib.Right("00000000" + tx_numP.Text.Trim(), 8);
                ValPlaCarr("P",tx_serP.Text + tx_numP.Text);
                if (retorD[6].ToString() == codAnul || retorD[6].ToString() == codIngA || retorD[6].ToString() == codGene)
                {
                    MessageBox.Show("Planilla de carga esta Abierta, Anulada o Recibida","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    tx_origen.Text = retorD[0];
                    tx_destino.Text = retorD[1];
                    tx_pla_placa.Text = retorD[2];
                    tx_pla_carret.Text = retorD[3];
                    tx_pla_brevet.Text = retorD[4];
                    tx_pla_nomcho.Text = retorD[5];
                    bt_agr.Focus();
                }
            }
        }
        private void tx_serGR_Leave(object sender, EventArgs e)
        {
            if (tx_serGR.Text.Trim() != "") tx_serGR.Text = lib.Right("0000" + tx_serGR.Text.Trim(), 4);
        }
        private void tx_numGR_Leave(object sender, EventArgs e)
        {
            if (tx_numGR.Text.Trim() != "")
            {
                tx_numGR.Text = lib.Right("00000000" + tx_numGR.Text.Trim(), 8);
                ValPlaCarr("G", tx_serGR.Text + tx_numGR.Text);
                if (retorD[6].ToString() == codAnul || retorD[7].ToString() == codIngA)
                {
                    MessageBox.Show("La Guía se encuentra Anulada o ya fue ingresada","Atención",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    tx_origen.Text = retorD[0];
                    tx_destino.Text = retorD[1];
                    tx_pla_placa.Text = retorD[2];
                    tx_pla_carret.Text = retorD[3];
                    tx_pla_brevet.Text = retorD[4];
                    tx_pla_nomcho.Text = retorD[5];
                    bt_agr.Focus();
                }
            }
        }
        #endregion

        #region botones_de_comando
        public void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            Bt_ver.Visible = false;
            Bt_print.Visible = false;
            //
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
                if (Convert.ToString(row["btn1"]) == "S") Bt_add.Visible = true;
                else Bt_add.Visible = false;
                if (Convert.ToString(row["btn2"]) == "S") Bt_edit.Visible = true;
                else Bt_edit.Visible = false;
                if (Convert.ToString(row["btn3"]) == "S") Bt_anul.Visible = true;
                else Bt_anul.Visible = false;
                if (Convert.ToString(row["btn4"]) == "S") Bt_ver.Visible = true;
                else Bt_ver.Visible = false;
                if (Convert.ToString(row["btn5"]) == "S") Bt_print.Visible = true;
                else Bt_print.Visible = false;
                if (Convert.ToString(row["btn6"]) == "S") Bt_close.Visible = true;
                else Bt_close.Visible = false;
            }
        }
        #region botones
        private void Bt_add_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "NUEVO";
            button1.Image = Image.FromFile(img_grab);
            Bt_ini.Enabled = false;
            Bt_sig.Enabled = false;
            Bt_ret.Enabled = false;
            Bt_fin.Enabled = false;
            //
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            button1.Enabled = true;
            initIngreso();
            sololee(); //escribe();
            tx_serP.Text = "";
            tx_numP.Text = "";
            tx_serGR.Text = "";
            tx_numGR.Text = "";
            tx_serP.ReadOnly = true;
            tx_numP.ReadOnly = true;
            tx_serGR.ReadOnly = true;
            tx_numGR.ReadOnly = true;
            tx_tfil.Text = "0";
            tx_totcant.Text = "0";
            tx_totpes.Text = "0";
            rb_plani.Focus();
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "EDITAR";
            button1.Image = Image.FromFile(img_grab);
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
            //
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            initIngreso();
            sololee();
            //tx_serP.ReadOnly = false;      // cambia a true una ves jalado los datos
            //tx_numP.ReadOnly = false;     // cambia a true una ves jalado los datos
            rb_plani.Focus();
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            // Impresion ó Re-impresion ??
            //if (tx_impreso.Text == "S")
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            /*
            sololee();
            Tx_modo.Text = "ANULAR";
            button1.Image = Image.FromFile(img_anul);
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
            //
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            initIngreso();
            tx_serP.ReadOnly = false;
            tx_numP.ReadOnly = false;
            tx_serP.Focus();
            */
        }
        private void Bt_ver_Click(object sender, EventArgs e)
        {
            sololee();
            Tx_modo.Text = "VISUALIZAR";
            button1.Image = Image.FromFile(img_ver);
            initIngreso();
            //
            Bt_ini.Enabled = true;
            Bt_sig.Enabled = true;
            Bt_ret.Enabled = true;
            Bt_fin.Enabled = true;
            //
            tx_serP.Text = "";
            tx_numP.Text = "";
            tx_serGR.Text = "";
            tx_numGR.Text = "";
            tx_serP.ReadOnly = true;
            tx_numP.ReadOnly = true;
            tx_serGR.ReadOnly = true;
            tx_numGR.ReadOnly = true;
            tx_tfil.Text = "0";
            tx_totcant.Text = "0";
            tx_totpes.Text = "0";
            rb_plani.Focus();
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
            if (tx_idr.Text.Trim() != "")
            {
                int aca = int.Parse(tx_idr.Text) + 1;
                limpiar();
                limpia_chk();
                limpia_combos();
                limpia_otros();
                tx_idr.Text = aca.ToString();
                tx_idr_Leave(null, null);
            }
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

        #endregion comboboxes

        #region impresion
        private bool imprimeA4()
        {
            bool retorna = false;
            llenaDataSet();                         // metemos los datos al dataset de la impresion
            return retorna;
        }
        private bool imprimeA5()
        {
            bool retorna = false;
            //
            return retorna;
        }
        private bool imprimeTK()
        {
            bool retorna = false;
            // 
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
                imprime_TK(sender, e);
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
            //float cold = 80.0F;
            Font lt_tit = new Font("Arial", 11);
            Font lt_titB = new Font("Arial", 11, FontStyle.Bold);
            PointF puntoF = new PointF(coli, alin);
            e.Graphics.DrawString(nomclie, lt_titB, Brushes.Black, puntoF, StringFormat.GenericTypographic);                      // titulo del reporte
            posi = posi + alfi;
            posi = posi + alfi;

        }
        private void imprime_TK(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // no hay guias en TK
        }
        private void updateprint(string sn)  // actualiza el campo impreso de la GR = S
        {   // S=si impreso || N=no impreso
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "update cabguiai set impreso=@sn where id=@idr";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.Parameters.AddWithValue("@sn", sn);
                    micon.Parameters.AddWithValue("@idr", tx_idr.Text);
                    micon.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region crystal
        private void llenaDataSet()
        {
            try
            {
                if (v_CR_gr_ind.Trim() == "")
                {
                    MessageBox.Show("Seleccione formato de impresión", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                conClie data = generaReporte();
                ReportDocument repo = new ReportDocument();
                repo.Load(v_CR_gr_ind);
                repo.SetDataSource(data);
                repo.PrintOptions.PrinterName = v_impA4;
                repo.PrintToPrinter(int.Parse(vi_copias), false, 0, 0);    // ,1,1
            }
            catch (Exception ex)
            {
                MessageBox.Show("Confirme su configuración de impresión" + Environment.NewLine + 
                    ex.Message,"Error en Impresión");
                return;
            }
        }
        private conClie generaReporte()
        {
            conClie PlaniC = new conClie();
            // CABECERA
            conClie.placar_cabRow rowcabeza = PlaniC.placar_cab.Newplacar_cabRow();
            rowcabeza.rucEmisor = rucclie;
            rowcabeza.nomEmisor = nomclie;
            rowcabeza.dirEmisor = Program.dirfisc;  // + " " + Program.distfis + " " + Program.provfis + " " + Program.depfisc;
            rowcabeza.id = tx_idr.Text;
            rowcabeza.brevChofer = tx_pla_brevet.Text;
            rowcabeza.camion = tx_pla_carret.Text;
            rowcabeza.direDest = "";
            rowcabeza.direOrigen = "";
            rowcabeza.marcaModelo = "";
            rowcabeza.numpla = tx_numP.Text;
            rowcabeza.placa = tx_pla_placa.Text;
            rowcabeza.serpla = tx_serP.Text;
            rowcabeza.fechSalida = "";
            rowcabeza.fechLlegada = "";
            rowcabeza.estado = tx_estado.Text;
            rowcabeza.tituloF = Program.tituloF;
            PlaniC.placar_cab.Addplacar_cabRow(rowcabeza);
            //
            // DETALLE  
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    i = i + 1;
                    conClie.placar_detRow rowdetalle = PlaniC.placar_det.Newplacar_detRow();
                    rowdetalle.fila = i.ToString();  // row.Cells["fila"].Value.ToString();
                    rowdetalle.id = tx_idr.Text;
                    rowdetalle.idc = "";
                    rowdetalle.moneda = row.Cells["MON"].Value.ToString();
                    rowdetalle.numguia = row.Cells["numguia"].Value.ToString();
                    rowdetalle.pagado = double.Parse(row.Cells[8].Value.ToString());
                    rowdetalle.salxcob = double.Parse(row.Cells[9].Value.ToString());
                    rowdetalle.serguia = row.Cells["serguia"].Value.ToString();
                    rowdetalle.totcant = Int16.Parse(row.Cells["totcant"].Value.ToString());
                    rowdetalle.totflete = Double.Parse(row.Cells["totflet"].Value.ToString());
                    rowdetalle.totpeso = int.Parse(row.Cells["totpeso"].Value.ToString());
                    rowdetalle.nomdest = row.Cells[13].Value.ToString();
                    rowdetalle.dirdest = row.Cells[14].Value.ToString();
                    rowdetalle.teldest = row.Cells[15].Value.ToString();
                    rowdetalle.nombulto = row.Cells[16].Value.ToString();
                    rowdetalle.nomremi = "";    // row.Cells[].Value.ToString();
                    rowdetalle.docvta = row.Cells[17].Value.ToString();
                    rowdetalle.nomremi = row.Cells[18].Value.ToString();
                    PlaniC.placar_det.Addplacar_detRow(rowdetalle);
                }
            }
            //
            return PlaniC;
        }
        #endregion

        #region datagridview
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)     // jala datos de cabecera guias
        {
            if (e.ColumnIndex == 1 && e.FormattedValue.ToString().Trim() != "") // pre guia
            {
                // las planillas de carga solo se llenan con guias individuales
            }
            if (e.ColumnIndex == 2 && e.FormattedValue.ToString().Trim() != "") // serie guia
            {
                // validamos que la serie de la guia corresponda al local de la planilla, serie de la planilla
                if (e.FormattedValue.ToString() != tx_serP.Text)
                {
                    if (dataGridView1.EditingControl != null) dataGridView1.EditingControl.Text = tx_serP.Text;
                }
            }
            if (e.ColumnIndex == 3 && e.FormattedValue.ToString().Trim() != "") // numero gúia
            {
                string completo = "";
                if (e.FormattedValue.ToString().Trim().Length > 0)
                {
                    completo = lib.Right("0000000" + e.FormattedValue, 8);
                    if (dataGridView1.EditingControl != null) dataGridView1.EditingControl.Text = completo;
                }
                if (completo.Length == 8 && dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().Length == 4 && 
                    dataGridView1.Rows[e.RowIndex].Cells[11].Value == null)
                {
                    using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
                    {
                        conn.Open();
                        string consulta = "select a.numpregui,a.cantotgri,a.pestotgri,b.descrizionerid as MON,a.totgri,a.totpag,a.salgri,a.tipmongri,a.numplagri," +
                            "c.unimedpro,ifnull(" +
                            "concat(d.descrizionerid,'-',if(SUBSTRING(a.serdocvta,1,2)='00',SUBSTRING(a.serdocvta,3,2),a.serdocvta),'-',if(SUBSTRING(a.numdocvta,1,3)='000',SUBSTRING(a.numdocvta,4,5),a.numdocvta)),'')" + 
                            "from cabguiai a left join desc_mon b on b.idcodice=a.tipmongri " +
                            "left join detguiai c on c.idc=a.id " +
                            "left join desc_tdv d on d.idcodice=a.tipdocvta " +
                            "where a.sergui=@ser and a.numgui=@num limit 1 ";
                        using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                        {
                            micon.Parameters.AddWithValue("@ser", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim());
                            micon.Parameters.AddWithValue("@num", completo);
                            MySqlDataReader dr = micon.ExecuteReader();
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    if (dr.GetString(8).Trim() == "")
                                    {
                                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = e.RowIndex + 1;
                                        dataGridView1.Rows[e.RowIndex].Cells[1].Value = dr.GetString(0);
                                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = dr.GetString(1);
                                        dataGridView1.Rows[e.RowIndex].Cells[5].Value = dr.GetString(2);
                                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = dr.GetString(3);
                                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = dr.GetString(4);
                                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = dr.GetString(5);
                                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = dr.GetString(6);
                                        dataGridView1.Rows[e.RowIndex].Cells[10].Value = dr.GetString(7);
                                        dataGridView1.Rows[e.RowIndex].Cells[16].Value = dr.GetString(9);
                                        dataGridView1.Rows[e.RowIndex].Cells[17].Value = dr.GetString(10);
                                    }
                                    else
                                    {
                                        MessageBox.Show("La Guía ingresada ya está registrada" + Environment.NewLine +
                                            "Planilla: " + dr.GetString(8).Trim(), "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        e.Cancel = true;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("La Guía ingresada no existe", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cancel = true;
                            }
                            dr.Dispose();
                        }
                    }
                }
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)       // cursor a la derecha o siguiente fila ... NO FUNCA
        {
           if (e.KeyCode == Keys.Enter)
            {
                //if (dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    e.SuppressKeyPress = true;
                    SendKeys.Send("{TAB}");
                }
                //if (dataGridView1.CurrentCell.ColumnIndex == 3)
                //{
                //    dataGridView1.Rows[dataGridView1.CurrentRow.Index + 1].Cells[2].Selected = true;
                //}
            }
        }
        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            operaciones();
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tx_serP.Text;
            }
        }

        // evento click en el checkbox de la coumna 14
        #endregion

    }
}
