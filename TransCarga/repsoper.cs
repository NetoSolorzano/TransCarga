﻿using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;

namespace TransCarga
{
    public partial class repsoper : Form
    {
        static string nomform = "repsoper";           // nombre del formulario
        string colback = TransCarga.Program.colbac;   // color de fondo
        string colpage = TransCarga.Program.colpag;   // color de los pageframes
        string colgrid = TransCarga.Program.colgri;   // color de las grillas
        string colfogr = TransCarga.Program.colfog;   // color fondo con grillas
        string colsfon = TransCarga.Program.colsbg;   // color fondo seleccion
        string colsfgr = TransCarga.Program.colsfc;   // color seleccion grilla
        string colstrp = TransCarga.Program.colstr;   // color del strip
        static string nomtab = "cabpregr";            // 
        DataTable dt = new DataTable();
        #region variables
        string asd = TransCarga.Program.vg_user;      // usuario conectado al sistema
        public int totfilgrid, cta;             // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        string tipede = "";
        string tiesta = "";
        string img_btN = "";
        string img_btE = "";
        string img_btP = "";
        string img_btA = "";            // anula = bloquea
        string img_btexc = "";          // exporta a excel
        string img_btq = "";
        string img_grab = "";
        string img_anul = "";
        string img_imprime = "";
        string img_preview = "";        // imagen del boton preview e imprimir reporte
        string letpied = "";            // letra indentificadora de piedra en detalle 2
        string cliente = Program.cliente;    // razon social para los reportes
        //int pageCount = 1, cuenta = 0;
        #endregion
        libreria lib = new libreria();
        // string de conexion
        //static string serv = ConfigurationManager.AppSettings["serv"].ToString();
        static string port = ConfigurationManager.AppSettings["port"].ToString();
        //static string usua = ConfigurationManager.AppSettings["user"].ToString();
        //static string cont = ConfigurationManager.AppSettings["pass"].ToString();
        static string data = ConfigurationManager.AppSettings["data"].ToString();
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + data + ";";

        public repsoper()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            // en este form no usamos
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void repsoper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void repsoper_Load(object sender, EventArgs e)
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
            jalainfo();
            init();
            toolboton();
            dataload("todos");
            KeyPreview = true;
            tabControl1.Enabled = false;
            //
            tx_codped.CharacterCasing = CharacterCasing.Upper;
            tx_codped.TextAlign = HorizontalAlignment.Center;
        }
        private void init()
        {
            tabControl1.BackColor = Color.FromName(TransCarga.Program.colgri);

            this.BackColor = Color.FromName(colback);
            toolStrip1.BackColor = Color.FromName(colstrp);
            dgv_resumen.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_resumen.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_resumen.DefaultCellStyle.ForeColor = Color.FromName(colfogr);
            dgv_resumen.DefaultCellStyle.SelectionBackColor = Color.FromName(colsfon);
            dgv_resumen.DefaultCellStyle.SelectionForeColor = Color.FromName(colsfgr);
            //
            dgv_vtas.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_vtas.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_vtas.DefaultCellStyle.ForeColor = Color.FromName(colfogr);
            dgv_vtas.DefaultCellStyle.SelectionBackColor = Color.FromName(colsfon);
            dgv_vtas.DefaultCellStyle.SelectionForeColor = Color.FromName(colsfgr);
            //
            Bt_add.Image = Image.FromFile(img_btN);
            Bt_edit.Image = Image.FromFile(img_btE);
            Bt_anul.Image = Image.FromFile(img_btA);
            //Bt_ver.Image = Image.FromFile(img_btV);
            Bt_print.Image = Image.FromFile(img_btP);
            Bt_close.Image = Image.FromFile(img_btq);
            bt_exc.Image = Image.FromFile(img_btexc);
            Bt_close.Image = Image.FromFile(img_btq);
        }
        private void jalainfo()                                     // obtiene datos de imagenes
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                conn.Open();
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@ped)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@ped", "xxx");
                MySqlDataAdapter da = new MySqlDataAdapter(micon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    DataRow row = dt.Rows[t];
                    if (row["campo"].ToString() == "imagenes" && row["formulario"].ToString() == "main")
                    {
                        if (row["param"].ToString() == "img_btN") img_btN = row["valor"].ToString().Trim();         // imagen del boton de accion NUEVO
                        if (row["param"].ToString() == "img_btE") img_btE = row["valor"].ToString().Trim();         // imagen del boton de accion EDITAR
                        if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                        if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                        if (row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                        if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                        //if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();        // imagen del boton de accion IMPRIMIR
                        if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                        if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                        if (row["param"].ToString() == "img_imprime") img_imprime = row["valor"].ToString().Trim();  // imagen del boton IMPRIMIR REPORTE
                        if (row["param"].ToString() == "img_pre") img_preview = row["valor"].ToString().Trim();  // imagen del boton VISTA PRELIMINAR
                    }
                    if (row["formulario"].ToString() == "xxx")
                    {
                        if (row["campo"].ToString() == "tipoped" && row["param"].ToString() == "almacen") tipede = row["valor"].ToString().Trim();         // tipo de pedido por defecto en almacen
                        if (row["campo"].ToString() == "estado" && row["param"].ToString() == "default") tiesta = row["valor"].ToString().Trim();         // estado del pedido inicial
                        if (row["campo"].ToString() == "detalle2" && row["param"].ToString() == "piedra") letpied = row["valor"].ToString().Trim();         // letra identificadora de Piedra en Detalle2
                    }
                }
                da.Dispose();
                dt.Dispose();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error de conexión");
                Application.Exit();
                return;
            }
        }
        public void dataload(string quien)                          // jala datos para los combos y la grilla
        {
            MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
            conn.Open();
            if (conn.State != ConnectionState.Open)
            {
                MessageBox.Show("No se pudo conectar con el servidor", "Error de conexión");
                Application.Exit();
                return;
            }
            if (quien == "todos")
            {
                // seleccion de taller de produccion ... ok
                const string contaller = "select descrizionerid,idcodice,codigo from desc_loc " +
                                       "where numero=1 order by idcodice";
                MySqlCommand cmd = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmd);
                DataTable dttaller = new DataTable();
                dataller.Fill(dttaller);
                cmb_vtasloc.DataSource = dttaller;
                cmb_vtasloc.DisplayMember = "descrizionerid";
                cmb_vtasloc.ValueMember = "idcodice";
                // seleccion de estado de servicios
                string conestad = "select descrizionerid,idcodice,codigo from desc_est " +
                                       "where numero=1 order by idcodice";
                cmd = new MySqlCommand(conestad, conn);
                MySqlDataAdapter daestad = new MySqlDataAdapter(cmd);
                DataTable dtestad = new DataTable();
                daestad.Fill(dtestad);
                cmb_estad.DataSource = dtestad;
                cmb_estad.DisplayMember = "descrizionerid";
                cmb_estad.ValueMember = "idcodice";
                // seleccion del tipo de documento cliente
                const string contidoc = "select descrizionerid,idcodice,codigo from desc_doc " +
                                       "where numero=1 order by idcodice";
                cmd = new MySqlCommand(contidoc, conn);
                MySqlDataAdapter datad = new MySqlDataAdapter(cmd);
                DataTable dttd = new DataTable();
                datad.Fill(dttd);
                cmb_tidoc.DataSource = dttd;
                cmb_tidoc.DisplayMember = "descrizionerid";
                cmb_tidoc.ValueMember = "idcodice";
                //
                datad.Dispose();
            }
            conn.Close();
        }
        private void grilla(string dgv)                             // 
        {
            switch (dgv)
            {
                case "dgv_vtas":
                    Font tiplg = new Font("Arial", 7, FontStyle.Bold);
                    dgv_vtas.Font = tiplg;
                    dgv_vtas.DefaultCellStyle.Font = tiplg;
                    dgv_vtas.RowTemplate.Height = 15;
                    dgv_vtas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
                    dgv_vtas.AllowUserToAddRows = false;
                    dgv_vtas.Width = 1015;
                    if (dgv_vtas.DataSource == null) dgv_vtas.ColumnCount = 11;
                    //
                    for (int i = 0; i < dgv_vtas.Columns.Count; i++)
                    {
                        dgv_vtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        _ = decimal.TryParse(dgv_vtas.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                        if (vd != 0) dgv_vtas.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    int b = 0;
                    for (int i = 0; i < dgv_vtas.Columns.Count; i++)
                    {
                        int a = dgv_vtas.Columns[i].Width;
                        b += a;
                        dgv_vtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgv_vtas.Columns[i].Width = a;
                    }
                    if (b < dgv_vtas.Width) dgv_vtas.Width = b + 60;
                    break;
            }
        }
        private void grillares()                                    // FALTA arma la grilla del resumen
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_resumen.Font = tiplg;
            dgv_resumen.DefaultCellStyle.Font = tiplg;
            dgv_resumen.RowTemplate.Height = 15;
            dgv_vtas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_resumen.AllowUserToAddRows = false;
            //dgv_resumen.EnableHeadersVisualStyles = false;
            dgv_resumen.Width = 1015;
            if (dgv_resumen.DataSource == null) dgv_resumen.ColumnCount = 11;
            for (int i = 0; i < dgv_resumen.Columns.Count; i++)
            {
                dgv_resumen.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                _ = decimal.TryParse(dgv_resumen.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                if (vd != 0) dgv_resumen.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            int b = 0;
            for (int i = 0; i < dgv_resumen.Columns.Count; i++)
            {
                int a = dgv_resumen.Columns[i].Width;
                b += a;
                dgv_resumen.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_resumen.Columns[i].Width = a;
            }
            if (b < dgv_resumen.Width) dgv_resumen.Width = b + 60;
        }
        private void sumaGrilla(string grilla)
        {
            switch (grilla)
            {
                case "grillares":
                    object sumfletes, sumsaldos;
                    sumfletes = dt.Compute("Sum(TOT_PRE)", string.Empty);
                    sumsaldos = dt.Compute("Sum(SALDO)", string.Empty);
                    tx_valor.Text = sumfletes.ToString();
                    tx_pendien.Text = sumsaldos.ToString();
                    tx_nser.Text = dt.Rows.Count.ToString();
                    break;
            }
        }
        private void bt_vtasfiltra_Click(object sender, EventArgs e)    // filtra y muestra reporte pre guias
        {
            string consulta;
            string parte = "";
            if (tx_dat_vtasloc.Text != "") parte += " and a.locorigen=@loca";
            if (tx_dat_estad.Text != "")
            {
                if (chk_excluye.Checked == true) parte += " and a.estadoser<>@esta";
                else parte += " and a.estadoser=@esta";

            }
            if (rb_listado.Checked == true)
            {
                consulta = "select a.id as ID,a.fechpregr as FECHA,a.serpregui as SERIE,a.numpregui as NUMERO,b.descrizionerid as DOC," +
                    "a.nudodepre as NDOC,a.nombdepre as DESTINATARIO,a.diredepre as DIR_DESTINATARIO,a.ubigdepre as UBIG_D," +
                    "c.descrizionerid as DOC,a.nudorepre as NDOC,a.nombrepre as REMITENTE,a.direrepre as DIR_REMITENTE,a.ubigrepre as UBIG_R," +
                    "d.descrizionerid as LOCAL,a.dirorigen as DIR_PARTIDA,a.ubiorigen as UBIG_O,e.descrizionerid as DESTINO," +
                    "a.dirdestin as DIR_DESTINO,a.ubidestin as UBIG_D,a.docsremit as DOCS_REMITENTE,a.obspregui as OBSERV," +
                    "a.cantotpre as CANT,a.pestotpre as PESO,f.descrizionerid as MON,a.totpregui as FLETE,a.totpagpre as PAGADO," +
                    "a.salpregui as SALDO,g.descrizionerid as ESTADO,a.impreso as IMPSO,a.serguitra as S_GUIA,a.numguitra as NUM_GUIA " +
                    "from cabpregr a " +
                    "left join desc_doc b on b.idcodice=a.tidodepre " +
                    "left join desc_doc c on c.idcodice=a.tidorepre " +
                    "left join desc_loc d on d.idcodice=a.locorigen " +
                    "left join desc_loc e on e.idcodice=a.locdestin " +
                    "left join desc_mon f on f.idcodice=a.tipmonpre " +
                    "left join desc_est g on g.idcodice=a.estadoser " +
                    "where a.fechpregr between @fecini and @fecfin" + parte;
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_vtas.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.Parameters.AddWithValue("@fecini", dtp_vtasfini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fecfin", dtp_vtasfina.Value.ToString("yyyy-MM-dd"));
                        if (tx_dat_vtasloc.Text != "") micon.Parameters.AddWithValue("@loca", tx_dat_vtasloc.Text);
                        if (tx_dat_estad.Text != "") micon.Parameters.AddWithValue("@esta", tx_dat_estad.Text);
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            dgv_vtas.DataSource = dt;
                            grilla("dgv_vtas");
                        }
                        else dgv_vtas.DataSource = null;
                        da.Dispose();
                        micon.Dispose();
                        //
                        string resulta = lib.ult_mov(nomform, nomtab, asd);
                        if (resulta != "OK")                                        // actualizamos la tabla usuarios
                        {
                            MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
            else
            {
                consulta = "select * from controlg where ... ;" +
                    "union; " +
                    "select * from controlg where ... ;";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_vtas.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@fecini", dtp_vtasfini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fecfin", dtp_vtasfina.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@tienda", tx_dat_vtasloc.Text.Trim());
                        if (rb_listado.Checked == true) micon.Parameters.AddWithValue("@modo", "listado");
                        if (rb_resumen.Checked == true) micon.Parameters.AddWithValue("@modo", "resumen");
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_vtas.DataSource = dt;
                        if (dt.Rows.Count > 0) 
                        { 
                            dgv_vtas.DataSource = dt;
                            grilla("dgv_vtas");
                        }
                        else dgv_vtas.DataSource = null;
                        da.Dispose();
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
        }
        private void tx_codped_Leave(object sender, EventArgs e)    // valida existencia de pre guia
        {
            if(tx_codped.Text != "" && tx_dat_tido.Text != "")
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string consu = "select b.id,b.ruc,b.razonsocial,b.estado,b.tiposocio " +
                            "from anag_cli b " +
                            "where b.tipdoc=@td and ruc=@nd";
                        MySqlCommand micon = new MySqlCommand(consu, conn);
                        micon.Parameters.AddWithValue("@td", tx_dat_tido.Text);
                        micon.Parameters.AddWithValue("@nd", tx_codped.Text.Trim());
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            if(dr[0] == null)
                            {
                                MessageBox.Show("No existe el cliente", "Atención verifique", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                tx_codped.Text = "";
                                tx_docu.Text = "";
                                tx_cliente.Text = "";
                                tx_valor.Text = "";
                                tx_pendien.Text = "";
                                tx_nser.Text = "";
                                tx_codped.Focus();
                                dr.Close();
                                conn.Close();
                                return;
                            }
                            else
                            {
                                tx_cliente.Text = dr.GetString(2);
                                tx_docu.Text = dr.GetString(1);
                                dr.Close();
                            }
                        }
                        micon.Dispose();
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error de conectividad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
        }
        private void bt_resumen_Click(object sender, EventArgs e)   // genera resumen de pre guia
        {
            if(tx_codped.Text != "" && tx_dat_tido.Text != "")
            {
                tx_codped_Leave(null, null);
                string consulta = "res_serv_clte";
                try
                {
                    MySqlConnection conn = new MySqlConnection(DB_CONN_STR);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        dgv_resumen.DataSource = null;
                        MySqlCommand micon = new MySqlCommand(consulta, conn);
                        micon.CommandType = CommandType.StoredProcedure;
                        micon.Parameters.AddWithValue("@tido", tx_dat_tido.Text);
                        micon.Parameters.AddWithValue("@nudo", tx_codped.Text.Trim());
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        da.Fill(dt);
                        dgv_resumen.DataSource = dt;
                        dt.Dispose();
                        da.Dispose();
                        grillares();
                    }
                    else
                    {
                        conn.Close();
                        MessageBox.Show("No se puede conectar al servidor", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error en obtener datos");
                    Application.Exit();
                    return;
                }
            }
            sumaGrilla("grillares");
        }

        #region combos
        private void cmb_estad_ing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estad.SelectedValue != null) tx_dat_estad.Text = cmb_estad.SelectedValue.ToString();
            else
            {
                tx_dat_estad.Text = "";    // cmb_estad.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
                chk_excluye.Checked = false;
            }
        }
        private void cmb_vtasloc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_vtasloc.SelectedValue != null) tx_dat_vtasloc.Text = cmb_vtasloc.SelectedValue.ToString();
            else tx_dat_vtasloc.Text = ""; // cmb_vtasloc.SelectedItem.ToString().PadRight(6).Substring(0, 6).Trim();
        }
        private void cmb_estad_ing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad.SelectedIndex = -1;
                tx_dat_estad.Text = "";
            }
        }
        private void cmb_vtasloc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_vtasloc.SelectedIndex = -1;
                tx_dat_vtasloc.Text = "";
            }
        }
        private void cmb_tidoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_tidoc.SelectedValue != null) tx_dat_tido.Text = cmb_tidoc.SelectedValue.ToString();
            else tx_dat_tido.Text = "";
        }
        #endregion

        #region botones de comando
        public void toolboton()
        {
            Bt_add.Visible = false;
            Bt_edit.Visible = false;
            Bt_anul.Visible = false;
            Bt_print.Visible = false;
            bt_exc.Visible = false;
            Bt_ini.Visible = false;
            Bt_sig.Visible = false;
            Bt_ret.Visible = false;
            Bt_fin.Visible = false;
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
                if (Convert.ToString(row["btn1"]) == "S")               // nuevo ... ok
                {
                    this.Bt_add.Visible = true;
                }
                else { this.Bt_add.Visible = false; }
                if (Convert.ToString(row["btn2"]) == "S")               // editar ... ok
                {
                    this.Bt_edit.Visible = true;
                }
                else { this.Bt_edit.Visible = false; }
                if (Convert.ToString(row["btn3"]) == "S")               // anular ... ok
                {
                    this.Bt_anul.Visible = true;
                }
                else { this.Bt_anul.Visible = false; }
                /*if (Convert.ToString(row["btn4"]) == "S")               // visualizar ... ok
                {
                    this.bt_view.Visible = true;
                }
                else { this.bt_view.Visible = false; }*/
                if (Convert.ToString(row["btn5"]) == "S")               // imprimir ... ok
                {
                    this.Bt_print.Visible = true;
                }
                else { this.Bt_print.Visible = false; }
                /*if (Convert.ToString(row["btn7"]) == "S")               // vista preliminar ... ok
                {
                    this.bt_prev.Visible = true;
                }
                else { this.bt_prev.Visible = false; }*/
                if (Convert.ToString(row["btn8"]) == "S")               // exporta xlsx  .. ok
                {
                    this.bt_exc.Visible = true;
                }
                else { this.bt_exc.Visible = false; }
                if (Convert.ToString(row["btn6"]) == "S")               // salir del form ... ok
                {
                    this.Bt_close.Visible = true;
                }
                else { this.Bt_close.Visible = false; }
            }
        }
        private void Bt_add_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void Bt_edit_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void Bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Bt_print_Click(object sender, EventArgs e)
        {
            Tx_modo.Text = "IMPRIMIR";
            tabControl1.Enabled = true;
            cmb_estad.SelectedIndex = -1;
            cmb_vtasloc.SelectedIndex = -1;
            cmb_tidoc.SelectedIndex = -1;
            chk_excluye.Checked = false;
        }
        private void Bt_anul_Click(object sender, EventArgs e)
        {
            // nothing to do
        }
        private void bt_exc_Click(object sender, EventArgs e)
        {
            // segun la pestanha activa debe exportar
            string nombre = "";
            if (tabControl1.Enabled == false) return;
            if (tabControl1.SelectedTab == tabres && dgv_resumen.Rows.Count > 0)
            {
                nombre = "resumen_cliente_" + tx_codped.Text.Trim() +"_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_resumen.DataSource;
                    wb.Worksheets.Add(dt, "Resumen");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabvtas && dgv_vtas.Rows.Count > 0)
            {
                nombre = "Reportes_PreGuias_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_vtas.DataSource;
                    wb.Worksheets.Add(dt, "PreGuias");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
        }
        #endregion

        #region crystal
        private void button2_Click(object sender, EventArgs e)      // resumen de contrato
        {
            setParaCrystal("resumen");
        }
        private void button4_Click(object sender, EventArgs e)      // reporte de ventas
        {
            if (rb_listado.Checked == true) setParaCrystal("vtasxclte");
            else setParaCrystal("ventas");
        }

        private void setParaCrystal(string repo)                    // genera el set para el reporte de crystal
        {
            if (repo== "resumen")
            {
                //conClie datos = generareporte();                        // conClie = dataset de impresion de contrato   
                //frmvizcont visualizador = new frmvizcont(datos);        // POR ESO SE CREO ESTE FORM frmvizcont PARA MOSTRAR AHI. ES MEJOR ASI.  
                //visualizador.Show();
            }
            if (repo == "ventas")
            {
                //conClie datos = generarepvtas();
                //frmvizcont visualizador = new frmvizcont(datos);
                //visualizador.Show();
            }
            if (repo == "vtasxclte")
            {
                conClie datos = generarepvtasxclte();
                frmvizoper visualizador = new frmvizoper(datos);
                visualizador.Show();
            }
        }
        private conClie generarepvtasxclte()
        {
            conClie repvtas = new conClie();                        // xsd
            conClie.repvtas_cabRow cabrow = repvtas.repvtas_cab.Newrepvtas_cabRow();
            cabrow.id = "0";
            cabrow.fecini = dtp_vtasfini.Value.ToString("dd/MM/yyyy");
            cabrow.fecfin = dtp_vtasfina.Value.ToString("dd/MM/yyyy");
            if (rb_listado.Checked == true) cabrow.modo = "listado";
            //if (rb_resumen.Checked == true) cabrow.modo = "resumen";
            repvtas.repvtas_cab.Addrepvtas_cabRow(cabrow);
            // detalle
            foreach (DataGridViewRow row in dgv_vtas.Rows)
            {
                if (rb_listado.Checked == true) 
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        detrow.fecha = row.Cells["fecha"].Value.ToString().Substring(0,2) + "/" + row.Cells["fecha"].Value.ToString().Substring(3, 2) + "/" + row.Cells["fecha"].Value.ToString().Substring(6, 4); 
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
            }
            return repvtas;
        }
        private conClie generarepvtas()
        {
            conClie repvtas = new conClie();                        // xsd
            conClie.repvtas_cabRow cabrow = repvtas.repvtas_cab.Newrepvtas_cabRow();
            cabrow.id = "0";
            cabrow.fecini = dtp_vtasfini.Value.ToString("dd/MM/yyyy");
            cabrow.fecfin = dtp_vtasfina.Value.ToString("dd/MM/yyyy");
            cabrow.tienda = tx_dat_vtasloc.Text.Trim();
            if (rb_listado.Checked == true) cabrow.modo = "listado";
            if (rb_resumen.Checked == true) cabrow.modo = "resumen";
            repvtas.repvtas_cab.Addrepvtas_cabRow(cabrow);
            // detalle
            foreach(DataGridViewRow row in dgv_vtas.Rows)
            {
                if (rb_resumen.Checked == true)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
                if (rb_listado.Checked == true)
                {
                    if (row.Cells["item"].Value != null && row.Cells["item"].Value.ToString().Trim() != "")
                    {
                        conClie.repvtas_detRow detrow = repvtas.repvtas_det.Newrepvtas_detRow();
                        detrow.id = "0";
                        detrow.tienda = row.Cells["tienda"].Value.ToString();
                        detrow.fecha = row.Cells["fecha"].Value.ToString().Substring(0,10);
                        repvtas.repvtas_det.Addrepvtas_detRow(detrow);
                    }
                }
            }
            return repvtas;
        }
        private conClie generareporte()
        {
            conClie rescont = new conClie();                                    // dataset
            /*
            conClie.rescont_cabRow rowcabeza = rescont.rescont_cab.Newrescont_cabRow();
            
            rowcabeza.id = "0";
            rowcabeza.contrato = tx_codped.Text;
            rowcabeza.doccli = tx_docu.Text;
            rowcabeza.nomcli = tx_cliente.Text.Trim();
            rowcabeza.estado = tx_estad.Text;
            rowcabeza.fecha = tx_fecha.Text;
            rowcabeza.tienda = tx_tiend.Text;
            rowcabeza.valor = tx_valor.Text;
            rowcabeza.fent = tx_fent.Text;
            rescont.rescont_cab.Addrescont_cabRow(rowcabeza);
            // detalle
            foreach(DataGridViewRow row in dgv_resumen.Rows)
            {
                if (row.Cells["codigo"].Value != null && row.Cells["codigo"].Value.ToString().Trim() != "")
                {
                    conClie.rescont_detRow rowdetalle = rescont.rescont_det.Newrescont_detRow();
                    rowdetalle.id = row.Cells["id"].Value.ToString();
                    rowdetalle.codigo = row.Cells["codigo"].Value.ToString();
                    rowdetalle.nombre = row.Cells["nombre"].Value.ToString();
                    rowdetalle.madera = row.Cells["madera"].Value.ToString();
                    rowdetalle.cantC = row.Cells["CanC"].Value.ToString();
                    rowdetalle.sep_id = row.Cells["sep_id"].Value.ToString();
                    rowdetalle.sep_fecha = row.Cells["sep_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.sep_almac = row.Cells["sep_almac"].Value.ToString();
                    rowdetalle.sep_cant = row.Cells["canS"].Value.ToString();
                    rowdetalle.ent_id = row.Cells["ent_id"].Value.ToString();
                    rowdetalle.ent_fecha = row.Cells["ent_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ent_cant = row.Cells["canE"].Value.ToString();
                    rowdetalle.tallerped = row.Cells["tallerped"].Value.ToString();
                    rowdetalle.ped_pedido = row.Cells["codped"].Value.ToString();
                    rowdetalle.ped_fecha = row.Cells["ped_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ped_cant = row.Cells["canP"].Value.ToString();
                    rowdetalle.ing_id = row.Cells["ing_id"].Value.ToString();
                    rowdetalle.ing_fecha = row.Cells["ing_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.ing_cant = row.Cells["canI"].Value.ToString();
                    rowdetalle.sal_id = row.Cells["sal_id"].Value.ToString();
                    rowdetalle.sal_fecha = row.Cells["sal_fecha"].Value.ToString().PadRight(10).Substring(0,10);
                    rowdetalle.sal_cant = row.Cells["canA"].Value.ToString();
                    rescont.rescont_det.Addrescont_detRow(rowdetalle);
                }
            }
            */
            return rescont;
        }
        #endregion
    }
}
