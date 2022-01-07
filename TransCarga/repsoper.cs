using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using CrystalDecisions.CrystalReports.Engine;

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

        #region variables
        string asd = TransCarga.Program.vg_user;      // usuario conectado al sistema
        public int totfilgrid, cta;             // variables para impresion
        public string perAg = "";
        public string perMo = "";
        public string perAn = "";
        public string perIm = "";
        //string tipede = "";
        //string tiesta = "";
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
        string cliente = Program.cliente;    // razon social para los reportes
        string codAnul = "";            // codigo de documento anulado
        string nomAnul = "";            // texto nombre del estado anulado
        string codGene = "";            // codigo documento nuevo generado
        string rpt_placarga = "";       // ruta y nombre del formato RPT planillas carga
        string v_tipdocR = "";          // tipo de documento ruc
        string rpt_grt = "";            // ruta y nombre del formato RPT guias remit
        string v_CR_gr_simple = "";     // ruta y nombre formato TK guia simple
        string vi_copias = "1";         // cantidad de copias impresion
        string v_impTK = "";            // nombre de la impresora de TK para guias
        string v_CR_ctacte = "";        // ruta y nombre del formato CR para el reporte cta cte clientes
        //int pageCount = 1, cuenta = 0;
        #endregion

        libreria lib = new libreria();
        publico pub = new publico();
        DataTable dt = new DataTable();
        DataTable dtestad = new DataTable();
        DataTable dttaller = new DataTable();
        DataTable dtplanCab = new DataTable();      // planilla de carga - cabecera
        DataTable dtplanDet = new DataTable();      // planilla de carga - detalle
        DataTable dtgrtcab = new DataTable();       // guia rem transpor - cabecera
        DataTable dtgrtdet = new DataTable();       // guia rem transpor - detalle
        // string de conexion
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + login.data + ";";

        public repsoper()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // F1
        {
            string para1 = "";
            string para2 = "";
            string para3 = "";
            if (keyData == Keys.Enter && tx_cliente.Focused == true && tx_cliente.Text.Trim() != "")
            {
                para1 = "Clientes";
                para2 = tx_cliente.Text.Trim();
                para3 = "";
                ayuda3 ayu3 = new ayuda3(para1, para2, para3);
                var result = ayu3.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    tx_dat_tido.Text = ayu3.ReturnValueA[3];       // codigo tipo doc
                    tx_docu.Text = ayu3.ReturnValueA[3];       // codigo tipo doc
                    cmb_tidoc.Enabled = true;
                    cmb_tidoc.SelectedValue = ayu3.ReturnValue0;
                    tx_codped.Text = ayu3.ReturnValue1;         // nume doc
                    tx_cliente.Text = ayu3.ReturnValue2;       // nombre cliente
                    //
                    dtp_ser_fini.Focus();
                }
                return true;    // indicate that you handled this keystroke
            }
            // 
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
            dataload("todos");
            jalainfo();
            init();
            toolboton();
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
            //dgv_resumen.DefaultCellStyle.ForeColor = Color.FromName(colfogr);
            //dgv_resumen.DefaultCellStyle.SelectionBackColor = Color.FromName(colsfon);
            //dgv_resumen.DefaultCellStyle.SelectionForeColor = Color.FromName(colsfgr);
            //
            dgv_vtas.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_guias.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_plan.DefaultCellStyle.BackColor = Color.FromName(colgrid);
            dgv_reval.DefaultCellStyle.BackColor = Color.FromName(colgrid);
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
                string consulta = "select formulario,campo,param,valor from enlaces where formulario in(@nofo,@pla,@clie,@grt)";
                MySqlCommand micon = new MySqlCommand(consulta, conn);
                micon.Parameters.AddWithValue("@nofo", "main");
                micon.Parameters.AddWithValue("@pla", "planicarga");
                micon.Parameters.AddWithValue("@clie", "clients");
                micon.Parameters.AddWithValue("@grt", "guiati");
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
                            if (row["param"].ToString() == "img_btP") img_btP = row["valor"].ToString().Trim();         // imagen del boton de accion IMPRIMIR
                            if (row["param"].ToString() == "img_btA") img_btA = row["valor"].ToString().Trim();         // imagen del boton de accion ANULAR/BORRAR
                            if (row["param"].ToString() == "img_btexc") img_btexc = row["valor"].ToString().Trim();     // imagen del boton exporta a excel
                            if (row["param"].ToString() == "img_btQ") img_btq = row["valor"].ToString().Trim();         // imagen del boton de accion SALIR
                            if (row["param"].ToString() == "img_gra") img_grab = row["valor"].ToString().Trim();         // imagen del boton grabar nuevo
                            if (row["param"].ToString() == "img_anu") img_anul = row["valor"].ToString().Trim();         // imagen del boton grabar anular
                            if (row["param"].ToString() == "img_imprime") img_imprime = row["valor"].ToString().Trim();  // imagen del boton IMPRIMIR REPORTE
                            if (row["param"].ToString() == "img_pre") img_preview = row["valor"].ToString().Trim();  // imagen del boton VISTA PRELIMINAR
                        }
                        if (row["campo"].ToString() == "estado")
                        {
                            if (row["param"].ToString() == "anulado") codAnul = row["valor"].ToString().Trim();         // codigo doc anulado
                            if (row["param"].ToString() == "generado") codGene = row["valor"].ToString().Trim();        // codigo doc generado
                            DataRow[] fila = dtestad.Select("idcodice='" + codAnul + "'");
                            nomAnul = fila[0][0].ToString();
                        }
                    }
                    if (row["formulario"].ToString() == "planicarga")
                    {
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "nomGRi_cr") rpt_placarga = row["valor"].ToString().Trim();         // ruta Y NOMBRE formato rpt
                    }
                    if (row["formulario"].ToString() == "guiati")
                    {
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "nomGRir_cr") rpt_grt = row["valor"].ToString().Trim();         // ruta y nombre formato rpt
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "GrT_simple_cr") v_CR_gr_simple = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "copias") vi_copias = row["valor"].ToString().Trim();
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "impTK") v_impTK = row["valor"].ToString().Trim();
                    }
                    if (row["formulario"].ToString() == "clients")
                    {
                        if (row["campo"].ToString() == "documento" && row["param"].ToString() == "ruc") v_tipdocR = row["valor"].ToString().Trim();         // tipo documento RUC
                        if (row["campo"].ToString() == "impresion" && row["param"].ToString() == "ctacte_cr") v_CR_ctacte = row["valor"].ToString().Trim(); // 
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
                // ***************** seleccion de la sede 
                string parte = "";
                if (("NIV002,NIV003").Contains(TransCarga.Program.vg_nius))
                {
                    parte = parte + "and idcodice='" + TransCarga.Program.vg_luse + "' ";
                }

                string contaller = "select descrizionerid,idcodice,codigo from desc_loc " +
                                       "where numero=1 " + parte + "order by idcodice";
                MySqlCommand cmd = new MySqlCommand(contaller, conn);
                MySqlDataAdapter dataller = new MySqlDataAdapter(cmd);
                // panel PRE GUIAS
                dataller.Fill(dttaller);
                cmb_vtasloc.DataSource = dttaller;
                cmb_vtasloc.DisplayMember = "descrizionerid";
                cmb_vtasloc.ValueMember = "idcodice";
                // PANEL GUIAS
                cmb_sede_guias.DataSource = dttaller;
                cmb_sede_guias.DisplayMember = "descrizionerid";
                cmb_sede_guias.ValueMember = "idcodice";
                // PANEL PLANILLA CARGA
                cmb_sede_plan.DataSource = dttaller;
                cmb_sede_plan.DisplayMember = "descrizionerid"; ;
                cmb_sede_plan.ValueMember = "idcodice";
                // ***************** seleccion de estado de servicios
                string conestad = "select descrizionerid,idcodice,codigo from desc_est " +
                                       "where numero=1 order by idcodice";
                cmd = new MySqlCommand(conestad, conn);
                MySqlDataAdapter daestad = new MySqlDataAdapter(cmd);
                daestad.Fill(dtestad);
                // PANEL GUIAS
                cmb_estad.DataSource = dtestad;
                cmb_estad.DisplayMember = "descrizionerid";
                cmb_estad.ValueMember = "idcodice";
                // PANEL GUIAS
                cmb_estad_guias.DataSource = dtestad;
                cmb_estad_guias.DisplayMember = "descrizionerid";
                cmb_estad_guias.ValueMember = "idcodice";
                // PANEL PLANILLA CARGA
                cmb_estad_plan.DataSource = dtestad;
                cmb_estad_plan.DisplayMember = "descrizionerid";
                cmb_estad_plan.ValueMember = "idcodice";
                // ***************** seleccion del tipo de documento cliente
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
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            int b;
            switch (dgv)
            {
                case "dgv_vtas":
                    dgv_vtas.Font = tiplg;
                    dgv_vtas.DefaultCellStyle.Font = tiplg;
                    dgv_vtas.RowTemplate.Height = 15;
                    //dgv_vtas.DefaultCellStyle.BackColor = Color.MediumAquamarine;
                    dgv_vtas.AllowUserToAddRows = false;
                    dgv_vtas.Width = this.Parent.Width - 50; // 1015;
                    if (dgv_vtas.DataSource == null) dgv_vtas.ColumnCount = 11;
                    if (dgv_vtas.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_vtas.Columns.Count; i++)
                        {
                            dgv_vtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_vtas.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_vtas.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_vtas.Columns.Count; i++)
                        {
                            int a = dgv_vtas.Columns[i].Width;
                            b += a;
                            dgv_vtas.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_vtas.Columns[i].Width = a;
                        }
                        if (b < dgv_vtas.Width) dgv_vtas.Width = b - 20;
                        dgv_vtas.ReadOnly = true;
                    }
                    break;
                case "dgv_guias":
                    dgv_guias.Font = tiplg;
                    dgv_guias.DefaultCellStyle.Font = tiplg;
                    dgv_guias.RowTemplate.Height = 15;
                    dgv_guias.AllowUserToAddRows = false;
                    dgv_guias.Width = Parent.Width - 50; // 1015;
                    if (dgv_guias.DataSource == null) dgv_guias.ColumnCount = 11;
                    if (dgv_guias.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_guias.Columns.Count; i++)
                        {
                            dgv_guias.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_guias.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_guias.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_guias.Columns.Count; i++)
                        {
                            int a = dgv_guias.Columns[i].Width;
                            b += a;
                            dgv_guias.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_guias.Columns[i].Width = a;
                        }
                        if (b < dgv_guias.Width) dgv_guias.Width = b - 20;
                        dgv_guias.ReadOnly = true;
                    }
                    sumaGrilla("dgv_guias");
                    break;
                case "dgv_plan":
                    dgv_plan.Font = tiplg;
                    dgv_plan.DefaultCellStyle.Font = tiplg;
                    dgv_plan.RowTemplate.Height = 15;
                    dgv_plan.AllowUserToAddRows = false;
                    dgv_guias.Width = Parent.Width - 50; // 1015;
                    if (dgv_plan.DataSource == null) dgv_plan.ColumnCount = 11;
                    if (dgv_plan.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_plan.Columns.Count; i++)
                        {
                            dgv_plan.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_plan.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_plan.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_plan.Columns.Count; i++)
                        {
                            int a = dgv_plan.Columns[i].Width;
                            b += a;
                            dgv_plan.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_plan.Columns[i].Width = a;
                        }
                        if (b < dgv_plan.Width) dgv_plan.Width = b - 20;
                        dgv_plan.ReadOnly = true;
                    }
                    sumaGrilla("dgv_plan");
                    break;
                case "dgv_reval":
                    dgv_reval.Font = tiplg;
                    dgv_reval.DefaultCellStyle.Font = tiplg;
                    dgv_reval.RowTemplate.Height = 15;
                    dgv_reval.AllowUserToAddRows = false;
                    dgv_reval.Width = Parent.Width - 50; // 1015;
                    if (dgv_reval.DataSource == null) dgv_reval.ColumnCount = 11;
                    if (dgv_reval.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_reval.Columns.Count; i++)
                        {
                            dgv_reval.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_reval.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_reval.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_reval.Columns.Count; i++)
                        {
                            int a = dgv_reval.Columns[i].Width;
                            b += a;
                            dgv_reval.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_reval.Columns[i].Width = a;
                        }
                        if (b < dgv_reval.Width) dgv_reval.Width = b - 20;
                        dgv_reval.ReadOnly = true;
                    }
                    sumaGrilla("dgv_reval");
                    break;
                case "dgv_histGR":
                    dgv_histGR.Font = tiplg;
                    dgv_histGR.DefaultCellStyle.Font = tiplg;
                    dgv_histGR.RowTemplate.Height = 15;
                    dgv_histGR.AllowUserToAddRows = false;
                    dgv_histGR.Width = Parent.Width - 50; // 1015;
                    if (dgv_histGR.DataSource == null) dgv_histGR.ColumnCount = 8;
                    if (dgv_histGR.Rows.Count > 0)
                    {
                        for (int i = 0; i < dgv_histGR.Columns.Count; i++)
                        {
                            dgv_histGR.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            _ = decimal.TryParse(dgv_histGR.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                            if (vd != 0) dgv_histGR.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                        b = 0;
                        for (int i = 0; i < dgv_histGR.Columns.Count; i++)
                        {
                            int a = dgv_histGR.Columns[i].Width;
                            b += a;
                            dgv_histGR.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dgv_histGR.Columns[i].Width = a;
                        }
                        if (b < dgv_histGR.Width) dgv_histGR.Width = dgv_histGR.Width - 10;
                        dgv_histGR.ReadOnly = true;
                    }
                    break;
            }
        }
        private void grillares(string modo)                         // modo 0=todo,1=sin preguias
        {
            Font tiplg = new Font("Arial", 7, FontStyle.Bold);
            dgv_resumen.Font = tiplg;
            dgv_resumen.DefaultCellStyle.Font = tiplg;
            dgv_resumen.RowTemplate.Height = 15;
            dgv_resumen.DefaultCellStyle.BackColor = Color.MediumAquamarine;
            dgv_resumen.AllowUserToAddRows = false;
            //dgv_resumen.EnableHeadersVisualStyles = false;
            dgv_resumen.Width = Parent.Width - 50; // 1015;
            if (dgv_resumen.DataSource == null) dgv_resumen.ColumnCount = 11;
            for (int i = 0; i < dgv_resumen.Columns.Count; i++)
            {
                dgv_resumen.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (dgv_resumen.Rows.Count > 0)
                {
                    _ = decimal.TryParse(dgv_resumen.Rows[0].Cells[i].Value.ToString(), out decimal vd);
                    if (vd != 0) dgv_resumen.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
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
            dgv_resumen.ReadOnly = true;
            //
            if (modo == "1")
            {
                for (int i = 1; i < 10; i++)
                {
                    dgv_resumen.Columns[i].Visible = false;
                }
            }
        }
        private void sumaGrilla(string grilla)
        {
            if (true)
            {
                DataRow[] row = dtestad.Select("idcodice='" + codAnul + "'");
                string etiq_anulado = row[0].ItemArray[0].ToString();
                int cr = 0, ca = 0, tgr = 0;
                double tvv = 0, tva = 0;
                switch (grilla)
                {
                    case "grillares":
                        if (tx_cliente.Text.Trim() != "")
                        {
                            //object sumPRE, sumGR, sumsaldos;
                            Decimal sumPRE = 0;
                            var sdf = dt.Compute("Sum(TOT_PRE)", "ESTADO <> '" + nomAnul + "' and TOT_GUIA = 0");
                            if (sdf.ToString() != "") sumPRE = decimal.Parse(sdf.ToString());   // string.Empty
                            Decimal sumGR = 0;
                            var spf = dt.Compute("Sum(TOT_GUIA)", "ESTADO <> '" + nomAnul + "' and TOT_PRE < TOT_GUIA");
                            if (spf != null && spf.ToString() != "") sumGR = decimal.Parse(spf.ToString());
                            Decimal sumsaldos = 0;
                            var ssf = dt.Compute("Sum(SALDO)", "ESTADO <> '" + nomAnul + "'").ToString();
                            if (ssf != null && ssf.ToString() != "") sumsaldos = decimal.Parse(ssf.ToString());
                            //
                            tx_valor.Text = (sumPRE + sumGR).ToString();
                            tx_pendien.Text = sumsaldos.ToString();
                            //tx_nser.Text = dt.Rows.Count.ToString();
                            tx_nser.Text = dt.Select("ESTADO <> '" + nomAnul + "'").Length.ToString();
                        }
                        break;
                    case "dgv_guias":
                        for (int i = 0; i < dgv_guias.Rows.Count; i++)
                        {
                            if (dgv_guias.Rows[i].Cells["ESTADO"].Value.ToString() != etiq_anulado)
                            {
                                tvv = tvv + Convert.ToDouble(dgv_guias.Rows[i].Cells["FLETE_MN"].Value);
                                cr = cr + 1;
                            }
                            else
                            {
                                dgv_guias.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                ca = ca + 1;
                                tva = tva + Convert.ToDouble(dgv_guias.Rows[i].Cells["FLETE_MN"].Value);
                            }
                        }
                        tx_tfi_f.Text = cr.ToString();
                        tx_totval.Text = tvv.ToString("#0.00");
                        tx_tfi_a.Text = ca.ToString();
                        tx_totv_a.Text = tva.ToString("#0.00");
                        break;
                    case "dgv_plan":
                        for (int i = 0; i < dgv_plan.Rows.Count; i++)
                        {
                            if (dgv_plan.Rows[i].Cells["ESTADO"].Value.ToString() != etiq_anulado)
                            {
                                tvv = tvv + Convert.ToDouble(dgv_plan.Rows[i].Cells["TOTAL"].Value);
                                tgr = tgr + Convert.ToInt32(dgv_plan.Rows[i].Cells["TGUIAS"].Value);
                                cr = cr + 1;
                            }
                            else
                            {
                                dgv_plan.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                ca = ca + 1;
                                tva = tva + Convert.ToDouble(dgv_plan.Rows[i].Cells["TOTAL"].Value);
                            }
                        }
                        tx_tfp_v.Text = cr.ToString();
                        tx_tflets.Text = tvv.ToString("#0.00");
                        tx_tgrp.Text = tgr.ToString();
                        tx_tfp_a.Text = ca.ToString();
                        break;
                    case "dgv_reval":
                        for (int i = 0; i < dgv_reval.Rows.Count; i++)
                        {
                            tvv = tvv + Convert.ToDouble(dgv_reval.Rows[i].Cells["SAL_GR"].Value);
                            tgr = tgr + Convert.ToInt32(dgv_reval.Rows[i].Cells["NVO_SALDO"].Value);
                            cr = cr + 1;
                        }
                        tx_treval.Text = tgr.ToString("#0.00");
                        tx_trant.Text = tvv.ToString("#0.00");
                        tx_frv.Text = cr.ToString();
                        break;
                }
            }
        }
        private void bt_vtasfiltra_Click(object sender, EventArgs e)    // genera reporte pre guias
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_oper_pregr1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@loca", (tx_dat_vtasloc.Text != "") ? tx_dat_vtasloc.Text : "");
                    micon.Parameters.AddWithValue("@fecini", dtp_vtasfini.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_vtasfina.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@esta", (tx_dat_estad.Text != "") ? tx_dat_estad.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_excluye.Checked == true) ? "1" : "0");
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_vtas.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_vtas.DataSource = dt;
                        grilla("dgv_vtas");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void tx_codped_Leave(object sender, EventArgs e)        // RESUMEN CLIENTE valida existencia de # documento
        {
            if (tx_codped.Text != "" && tx_dat_tido.Text != "")
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
                            if (dr[0] == null)
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
        private void bt_resumen_Click(object sender, EventArgs e)       // genera resumen de cliente
        {
            if (tx_codped.Text.Trim() != "" && tx_dat_tido.Text != "")
            {
                tx_codped_Leave(null, null);
                dt.Clear();
                //dgv_resumen.Rows.Clear();
                //dgv_resumen.Columns.Clear();
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
                        micon.Parameters.AddWithValue("@fecini", dtp_ser_fini.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@fecfin", dtp_ser_fina.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@tope", (rb_total.Checked == true) ? "T" : "P");      // T=todos || P=pendientes de cob
                        MySqlDataAdapter da = new MySqlDataAdapter(micon);
                        da.Fill(dt);
                        dgv_resumen.DataSource = dt;
                        dt.Dispose();
                        da.Dispose();
                        if (checkBox1.Checked == false) grillares("0");
                        else grillares("1");                            // 0=todo,1=sin preGuias
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
                tx_codped.Focus();
            }
            sumaGrilla("grillares");
        }
        private void bt_guias_Click(object sender, EventArgs e)         // genera reporte guias
        {
            if (rb_GR_dest.Checked == false && rb_GR_origen.Checked == false && cmb_sede_guias.SelectedIndex > -1)
            {
                MessageBox.Show("Seleccione origen o destino?", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rb_GR_origen.Focus();
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_oper_guiai1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@loca", (tx_sede_guias.Text != "") ? tx_sede_guias.Text : "");
                    micon.Parameters.AddWithValue("@fecini", dtp_ini_guias.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fin_guias.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@esta", (tx_estad_guias.Text != "") ? tx_estad_guias.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_excl_guias.Checked == true) ? "1" : "0");
                    micon.Parameters.AddWithValue("@orides", (rb_GR_origen.Checked == true) ? "O" : "D");   // local -> O=origen || D=destino
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_guias.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_guias.DataSource = dt;
                        grilla("dgv_guias");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_plan_Click(object sender, EventArgs e)          // genera reporte planilla de carga
        {
            if (rb_PLA_dest.Checked == false && rb_PLA_origen.Checked == false && cmb_sede_plan.SelectedIndex > -1)
            {
                MessageBox.Show("Seleccione origen o destino?", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rb_PLA_origen.Focus();
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_oper_plan1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fecini", dtp_fini_plan.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_fter_plan.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@loca", (tx_dat_sede_plan.Text != "") ? tx_dat_sede_plan.Text : "");
                    micon.Parameters.AddWithValue("@esta", (tx_dat_estad_plan.Text != "") ? tx_dat_estad_plan.Text : "");
                    micon.Parameters.AddWithValue("@excl", (chk_exclu_plan.Checked == true) ? "1" : "0");
                    micon.Parameters.AddWithValue("@orides", (rb_PLA_origen.Checked == true) ? "O" : "D");   // local -> O=origen || D=destino
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_plan.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_plan.DataSource = dt;
                        grilla("dgv_plan");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_reval_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_oper_reval1";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@fecini", dtp_rev_fecini.Value.ToString("yyyy-MM-dd"));
                    micon.Parameters.AddWithValue("@fecfin", dtp_rev_fecfin.Value.ToString("yyyy-MM-dd"));
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_reval.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_reval.DataSource = dt;
                        grilla("dgv_reval");
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_hisGR_Click(object sender, EventArgs e)         // historial de GR
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                conn.Open();
                string consulta = "rep_oper_histGR";
                using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                {
                    micon.CommandType = CommandType.StoredProcedure;
                    micon.Parameters.AddWithValue("@ser", tx_ser.Text);
                    micon.Parameters.AddWithValue("@num", tx_num.Text);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                    {
                        dgv_histGR.DataSource = null;
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_histGR.DataSource = dt;
                        grilla("dgv_histGR");
                        //
                        histograma hg = new histograma(dt, rpt_grt, rpt_placarga);
                        hg.Show();
                    }
                    string resulta = lib.ult_mov(nomform, nomtab, asd);
                    if (resulta != "OK")                                        // actualizamos la tabla usuarios
                    {
                        MessageBox.Show(resulta, "Error en actualización de tabla usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void bt_dale_Click(object sender, EventArgs e)          // impresion GRUPAL de guias
        {
            if (rb_imSimp.Checked == false && rb_imComp.Checked == false)
            {
                MessageBox.Show("Seleccione formato", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rb_imSimp.Focus();
                return;
            }
            setParaCrystal("GrGrupal");
            chk_impGrp.Checked = false;
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
        private void cmb_sede_plan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_sede_plan.SelectedValue != null) tx_dat_sede_plan.Text = cmb_sede_plan.SelectedValue.ToString();
            else tx_dat_sede_plan.Text = "";
        }
        private void cmb_sede_plan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_sede_plan.SelectedIndex = -1;
                tx_dat_sede_plan.Text = "";
            }
        }
        private void cmb_estad_plan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estad_plan.SelectedValue != null) tx_dat_estad_plan.Text = cmb_estad_plan.SelectedValue.ToString();
            else tx_dat_estad_plan.Text = "";
        }
        private void cmb_estad_plan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad_plan.SelectedIndex = -1;
                tx_dat_estad_plan.Text = "";
            }
        }
        private void cmb_sede_guias_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_sede_guias.SelectedValue != null) tx_sede_guias.Text = cmb_sede_guias.SelectedValue.ToString();
            else tx_sede_guias.Text = "";
        }
        private void cmb_sede_guias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_sede_guias.SelectedIndex = -1;
                tx_sede_guias.Text = "";
            }
        }
        private void cmb_estad_guias_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_estad_guias.SelectedValue != null) tx_estad_guias.Text = cmb_estad_guias.SelectedValue.ToString();
            else tx_estad_guias.Text = "";
        }
        private void cmb_estad_guias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cmb_estad_guias.SelectedIndex = -1;
                tx_estad_guias.Text = "";
            }
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
            //
            cmb_sede_guias.SelectedIndex = -1;
            cmb_estad_guias.SelectedIndex = -1;
            //
            rb_imComp.Visible = false;
            rb_imSimp.Visible = false;
            bt_dale.Visible = false;
            //
            checkBox1.Checked = true;
            rb_total.Checked = true;
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
            if (tabControl1.SelectedTab == tabres && dgv_resumen.Rows.Count > 0)        // resumen de cliente
            {
                nombre = "resumen_cliente_" + tx_codped.Text.Trim() + "_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
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
            if (tabControl1.SelectedTab == tabvtas && dgv_vtas.Rows.Count > 0)          // pre guias
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
            if (tabControl1.SelectedTab == tabgrti && dgv_guias.Rows.Count > 0)         // guias remision transportista
            {
                nombre = "Reportes_GuiasTransportista_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_guias.DataSource;
                    wb.Worksheets.Add(dt, "GuiasTransp");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabplacar && dgv_plan.Rows.Count > 0)        // planilla de carga
            {
                nombre = "Reportes_PlanillasCarga_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_plan.DataSource;
                    wb.Worksheets.Add(dt, "PlanillasC");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabreval && dgv_reval.Rows.Count > 0)        // revalorizaciones
            {
                nombre = "Reportes_Revalorizaciones_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_reval.DataSource;
                    wb.Worksheets.Add(dt, "Revalorizaciones");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
            if (tabControl1.SelectedTab == tabgrhist && dgv_histGR.Rows.Count > 0)      // seguimiento por guía
            {
                nombre = "Seguimiento_GuiasTransp_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xlsx";
                var aa = MessageBox.Show("Confirma que desea generar la hoja de calculo?",
                    "Archivo: " + nombre, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aa == DialogResult.Yes)
                {
                    var wb = new XLWorkbook();
                    DataTable dt = (DataTable)dgv_histGR.DataSource;
                    wb.Worksheets.Add(dt, "Seguimiento");
                    wb.SaveAs(nombre);
                    MessageBox.Show("Archivo generado con exito!");
                    this.Close();
                }
            }
        }
        #endregion

        #region crystal
        private void button2_Click(object sender, EventArgs e)      // 
        {
            setParaCrystal("resumen");
        }
        private void button4_Click(object sender, EventArgs e)      // 
        {
            if (rb_listado.Checked == true) setParaCrystal("vtasxclte");
            else setParaCrystal("ventas");
        }
        private void setParaCrystal(string repo)                    // genera el set para el reporte de crystal
        {
            if (repo == "GrGrupal")
            {
                if (rb_imSimp.Checked == true)      // formato simple de la GR (TK)
                {
                    foreach (DataGridViewRow row in dgv_guias.Rows)
                    {
                        if (row.Cells[0].EditedFormattedValue.ToString() == "True")
                        {
                            conClie data = generareporte(row.Index);
                            ReportDocument fimp = new ReportDocument();
                            fimp.Load(v_CR_gr_simple);
                            fimp.SetDataSource(data);
                            try
                            {
                                fimp.PrintOptions.PrinterName = v_impTK;
                                fimp.PrintToPrinter(int.Parse(vi_copias), false, 1, 1);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("No se encuentra la impresora de las guías simples" + Environment.NewLine +
                                    ex.Message, "Error en configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                if (rb_imComp.Checked == true)      // formato completo de la GR (2 x A4)
                {


                }
            }
            
            if (repo == "resumen")
            {
                conClie datos = generarepctacte();
                frmvizoper visualizador = new frmvizoper(datos);
                visualizador.Show();
            }
        }

        private conClie generareporte(int rowi)
        {
            /*
                SER,NUMERO,FECHA,PREGUIA,DOC,DESTINAT,NOMBRE,DIRDEST,DOC,REMITENTE,NOMBRE2,ORIGEN,DESTINO,MON,FLETE_GR,FLETE_MN,ESTADO,IMPRESO,
	            TDV,SERVTA,NUMVTA,PAGADO,SALDO,SER_PLA,NUM_PLA,CHOFER,PLACA,CANTIDAD,PESO,U_MEDID,DETALLE
            */
            conClie guiaT = new conClie();
            conClie.gr_ind_cabRow rowcabeza = guiaT.gr_ind_cab.Newgr_ind_cabRow();
            // CABECERA
            DataGridViewRow row = dgv_guias.Rows[rowi];
            rowcabeza.formatoRPT = v_CR_gr_simple;
            rowcabeza.id = "0"; // tx_idr.Text;
            rowcabeza.estadoser = row.Cells["ESTADO"].Value.ToString(); // tx_estado.Text;
            rowcabeza.sergui = row.Cells["SER"].Value.ToString(); // tx_serie.Text;
            rowcabeza.numgui = row.Cells["NUMERO"].Value.ToString(); // tx_numero.Text;
            rowcabeza.numpregui = row.Cells["PREGUIA"].Value.ToString(); // tx_pregr_num.Text;
            rowcabeza.fechope = row.Cells["FECHA"].Value.ToString().Substring(0, 10); // tx_fechope.Text;
            rowcabeza.fechTraslado = "";
            rowcabeza.frase1 = "";
            rowcabeza.frase2 = "";
            // origen - destino
            rowcabeza.nomDestino = row.Cells["DESTINO"].Value.ToString(); // cmb_destino.Text;
            rowcabeza.direDestino = row.Cells["DIRDEST"].Value.ToString();
            rowcabeza.dptoDestino = ""; // 
            rowcabeza.provDestino = "";
            rowcabeza.distDestino = ""; // 
            rowcabeza.nomOrigen = row.Cells["ORIGEN"].Value.ToString(); // cmb_origen.Text;
            rowcabeza.direOrigen = "";
            rowcabeza.dptoOrigen = "";  // no hay campo
            rowcabeza.provOrigen = "";
            rowcabeza.distOrigen = "";  // no hay campo
            // remitente
            rowcabeza.docRemit = "";    // cmb_docRem.Text;
            rowcabeza.numRemit = row.Cells["REMITENTE"].Value.ToString();    // tx_numDocRem.Text;
            rowcabeza.nomRemit = row.Cells["NOMBRE2"].Value.ToString();    // tx_nomRem.Text;
            rowcabeza.direRemit = "";
            rowcabeza.dptoRemit = "";
            rowcabeza.provRemit = "";
            rowcabeza.distRemit = "";
            rowcabeza.telremit = "";
            // destinatario  
            rowcabeza.docDestinat = ""; // cmb_docDes.Text;
            rowcabeza.numDestinat = row.Cells["DESTINAT"].Value.ToString(); // tx_numDocDes.Text;
            rowcabeza.nomDestinat = row.Cells["NOMBRE"].Value.ToString(); // tx_nomDrio.Text;
            rowcabeza.direDestinat = "";
            rowcabeza.distDestinat = "";
            rowcabeza.provDestinat = "";
            rowcabeza.dptoDestinat = "";
            rowcabeza.teldesti = "";
            // importes 
            rowcabeza.nomMoneda = row.Cells["MON"].Value.ToString(); // cmb_mon.Text;
            rowcabeza.igv = "";
            rowcabeza.subtotal = "";
            rowcabeza.total = row.Cells["FLETE_GR"].Value.ToString(); // (chk_flete.Checked == true) ? tx_flete.Text : "";
            rowcabeza.docscarga = row.Cells["DOCSREMIT"].Value.ToString(); ;   // docs del remitente 
            rowcabeza.consignat = "";   // 
            // pie
            rowcabeza.marcamodelo = "";
            rowcabeza.autoriz = "";
            rowcabeza.brevAyuda = "";   // falta este campo
            rowcabeza.brevChofer = "";
            rowcabeza.nomChofer = "";
            rowcabeza.placa = row.Cells["PLACA"].Value.ToString(); // tx_pla_placa.Text;
            rowcabeza.camion = "";      // placa carreta
            rowcabeza.confvehi = "";
            rowcabeza.rucPropiet = "";
            rowcabeza.nomPropiet = "";
            rowcabeza.fechora_imp = "";
            rowcabeza.userc = "";
            //
            guiaT.gr_ind_cab.Addgr_ind_cabRow(rowcabeza);
            //
            // DETALLE  
            //for (int i = 0; i < dtgrtdet.Rows.Count; i++)
            {
                conClie.gr_ind_detRow rowdetalle = guiaT.gr_ind_det.Newgr_ind_detRow();
                rowdetalle.fila = "";       // no estamos usando
                rowdetalle.cant = row.Cells["CANTIDAD"].Value.ToString(); // dtgrtdet.Rows[0].ItemArray[3].ToString();
                rowdetalle.codigo = "";     // no estamos usando
                rowdetalle.umed = row.Cells["U_MEDID"].Value.ToString(); // dtgrtdet.Rows[0].ItemArray[4].ToString();
                rowdetalle.descrip = row.Cells["DETALLE"].Value.ToString(); // dtgrtdet.Rows[0].ItemArray[6].ToString();
                rowdetalle.precio = "";     // no estamos usando
                rowdetalle.total = "";      // no estamos usando
                rowdetalle.peso = string.Format("{0:#0}", row.Cells["PESO"].Value.ToString());  // dtgrtdet.Rows[0].ItemArray[7].ToString()
                guiaT.gr_ind_det.Addgr_ind_detRow(rowdetalle);
            }
            return guiaT;
        }
        private conClie generarepctacte()
        {
            conClie ctacte = new conClie();

            conClie.ctacteclteRow rowcab = ctacte.ctacteclte.NewctacteclteRow();
            DataGridViewRow row = dgv_resumen.Rows[0];
            rowcab.formatoRPT = v_CR_ctacte;
            rowcab.rucEmisor = Program.ruc;
            rowcab.nomEmisor = Program.cliente;
            rowcab.dirEmisor = Program.dirfisc;
            rowcab.fecfin = dtp_ser_fini.Value.Date.ToString();
            rowcab.fecini = dtp_ser_fina.Value.Date.ToString();
            rowcab.id = "0";
            rowcab.nomcliente = tx_cliente.Text;
            rowcab.numdoc = tx_docu.Text;
            rowcab.tipdoc = cmb_tidoc.Text;
            rowcab.tot_pend = (rb_pend.Checked == true) ? "P" : "T";
            ctacte.ctacteclte.AddctacteclteRow(rowcab);
            //
            foreach (DataGridViewRow rowd in dgv_resumen.Rows)
            {
                conClie.detctacteRow rowdet = ctacte.detctacte.NewdetctacteRow();
                rowdet.id = "0";
                rowdet.estado = rowd.Cells["ESTADO"].Value.ToString();
                rowdet.fechgr = rowd.Cells["F_GUIA"].Value.ToString();
                rowdet.guia = rowd.Cells["GUIA"].Value.ToString();
                rowdet.mongr = rowd.Cells["MON"].Value.ToString();  // moneda GR
                rowdet.flete = double.Parse(rowd.Cells["TOT_GUIA"].Value.ToString());
                rowdet.origen = rowd.Cells["ORIGEN"].Value.ToString();
                rowdet.destino = rowd.Cells["DESTINO"].Value.ToString();
                rowdet.tdrem = rowd.Cells["TD_REM"].Value.ToString();  // tipo doc remiten
                rowdet.ndrem = rowd.Cells["ND_REM"].Value.ToString();
                rowdet.nomrem = rowd.Cells["REMITENTE"].Value.ToString();
                rowdet.tddes = rowd.Cells["TD_DES"].Value.ToString();  // tipo doc destinat
                rowdet.nddes = rowd.Cells["ND_DES"].Value.ToString();
                rowdet.nomdes = rowd.Cells["DESTINAT"].Value.ToString();
                rowdet.fecdv = rowd.Cells["F_VTA"].Value.ToString();
                rowdet.docvta = rowd.Cells["DOC_VTA"].Value.ToString();
                rowdet.monvta = rowd.Cells["MON_VTA"].Value.ToString();
                rowdet.totvta = double.Parse(rowd.Cells["TOT_VTA"].Value.ToString());
                rowdet.fecpag = rowd.Cells["F_PAGO"].Value.ToString();
                rowdet.nompag = rowd.Cells["MON_PAG"].Value.ToString(); // moneda pago
                rowdet.totpag = double.Parse(rowd.Cells["PAGADO"].Value.ToString());  // total pagos
                rowdet.saldo = double.Parse(rowd.Cells["SALDO"].Value.ToString());
                rowdet.fecpla = rowd.Cells["F_PAGO"].Value.ToString();
                rowdet.planilla = rowd.Cells["PLANILLA"].Value.ToString();
                rowdet.placa = rowd.Cells["PLACA"].Value.ToString();
                ctacte.detctacte.AdddetctacteRow(rowdet);
            }
            //
            return ctacte;
        }
        #endregion

        #region leaves y enter
        private void tabvtas_Enter(object sender, EventArgs e)
        {
            cmb_vtasloc.Focus();
        }
        private void tabres_Enter(object sender, EventArgs e)
        {
            cmb_tidoc.Focus();
        }
        private void tx_ser_Leave(object sender, EventArgs e)
        {
            tx_ser.Text = lib.Right("000" + tx_ser.Text, 4);
        }
        private void tx_num_Leave(object sender, EventArgs e)
        {
            tx_num.Text = lib.Right("0000000" + tx_num.Text, 8);
        }
        private void chk_impGrp_CheckStateChanged(object sender, EventArgs e)
        {
            if (chk_impGrp.CheckState == CheckState.Checked)
            {
                DataGridViewCheckBoxColumn chkc = new DataGridViewCheckBoxColumn();
                chkc.Name = "chkc";
                chkc.HeaderText = " ";
                chkc.Width = 30;
                chkc.ReadOnly = false;
                chkc.FillWeight = 10;
                dgv_guias.Columns.Insert(0, chkc);
                dgv_guias.Enabled = true;
                dgv_guias.ReadOnly = false;
                dgv_guias.Columns[0].ReadOnly = false;
                for (int i = 1; i < dgv_guias.Columns.Count; i++)     // NO SALE EL CHECK, NO SE VE
                {
                    dgv_guias.Columns[i].ReadOnly = true;
                }
                rb_imComp.Visible = true;
                rb_imSimp.Visible = true;
                bt_dale.Visible = true;
            }
            else
            {
                dgv_guias.Columns.Remove("chkc");
                rb_imComp.Visible = false;
                rb_imSimp.Visible = false;
                bt_dale.Visible = false;
                dgv_guias.ReadOnly = true;
            }
        }
        private void rb_busDoc_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_busDoc.Checked == true)
            {
                tx_cliente.Text = "";
                tx_cliente.ReadOnly = true;

                cmb_tidoc.Enabled = true;
                tx_codped.ReadOnly = false;
            }
        }
        private void rb_busNom_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_busNom.Checked == true)
            {
                cmb_tidoc.SelectedIndex = -1;
                cmb_tidoc.Enabled = false;
                tx_dat_tido.Text = "";
                tx_codped.ReadOnly = true;
                tx_codped.Text = "";

                tx_cliente.ReadOnly = false;
            }
        }
        #endregion

        #region advancedatagridview
        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabres")
            {
                DataTable dtg = (DataTable)dgv_resumen.DataSource;
                dtg.DefaultView.Sort = dgv_resumen.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabgrti")
            {
                DataTable dtg = (DataTable)dgv_guias.DataSource;
                dtg.DefaultView.Sort = dgv_guias.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabvtas")
            {
                DataTable dtg = (DataTable)dgv_vtas.DataSource;
                dtg.DefaultView.Sort = dgv_vtas.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabplacar")
            {
                DataTable dtg = (DataTable)dgv_plan.DataSource;
                dtg.DefaultView.Sort = dgv_plan.SortString;
            }
            if (tabControl1.SelectedTab.Name == "tabreval")
            {
                DataTable dtg = (DataTable)dgv_reval.DataSource;
                dtg.DefaultView.Sort = dgv_reval.SortString;
            }
        }
        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)                  // filtro de las columnas
        {
            if (tabControl1.SelectedTab.Name == "tabres")
            {
                DataTable dtg = (DataTable)dgv_resumen.DataSource;
                dtg.DefaultView.RowFilter = dgv_resumen.FilterString;
            }
            if (tabControl1.SelectedTab.Name == "tabvtas")
            {
                DataTable dtg = (DataTable)dgv_vtas.DataSource;
                dtg.DefaultView.RowFilter = dgv_vtas.FilterString;
            }
            if (tabControl1.SelectedTab.Name == "tabgrti")
            {
                DataTable dtg = (DataTable)dgv_guias.DataSource;
                dtg.DefaultView.RowFilter = dgv_guias.FilterString;
                sumaGrilla("dgv_guias");
            }
            if (tabControl1.SelectedTab.Name == "tabplacar")
            {
                DataTable dtg = (DataTable)dgv_plan.DataSource;
                dtg.DefaultView.RowFilter = dgv_plan.FilterString;
                sumaGrilla("dgv_plan");
            }
            if (tabControl1.SelectedTab.Name == "tabreval")
            {
                DataTable dtg = (DataTable)dgv_reval.DataSource;
                dtg.DefaultView.RowFilter = dgv_reval.FilterString;
                sumaGrilla("dgv_reval");
            }
        }
        private void advancedDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)            // no usamos
        {
            //advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        private void advancedDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)      // no usamos
        {
            if (tabControl1.SelectedTab.Name == "tabres")
            {
                if (dgv_resumen.Columns[e.ColumnIndex].Name == "GUIA")
                {
                    string ser = dgv_resumen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Substring(0, 4);
                    string num = dgv_resumen.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Substring(5, 8);
                    //muestra_gr(ser,num);
                    pub.muestra_gr(ser, num, rpt_grt);
                }
            }
            if (tabControl1.SelectedTab.Name == "tabvtas")
            {

            }
            if (tabControl1.SelectedTab.Name == "tabgrti")
            {
                if (dgv_guias.Columns[0].Name.ToString() == "chkc")
                {
                    if (e.ColumnIndex == 2)
                    {
                        //muestra_gr(dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(), dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        pub.muestra_gr(dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(),
                            dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
                            rpt_grt);
                    }
                }
                else
                {
                    if (e.ColumnIndex == 1)
                    {
                        //muestra_gr(dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(), dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        pub.muestra_gr(dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(),
                            dgv_guias.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
                            rpt_grt);
                    }
                }
            }
            if (tabControl1.SelectedTab.Name == "tabplacar")
            {
                if (e.ColumnIndex == 2)
                {
                    pub.muestra_pl(dgv_plan.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(),
                        dgv_plan.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
                        rpt_placarga);
                }
            }
            if (tabControl1.SelectedTab.Name == "tabreval")
            {

            }
        }
        private void advancedDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) // no usamos
        {
            /*if (e.RowIndex > -1 && e.ColumnIndex > 0 
                && advancedDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != e.FormattedValue.ToString())
            {
                string campo = advancedDataGridView1.Columns[e.ColumnIndex].Name.ToString();
                string[] noeta = equivinter(advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString());    // retorna la tabla segun el titulo de la columna

                var aaa = MessageBox.Show("Confirma que desea cambiar el valor?",
                    "Columna: " + advancedDataGridView1.Columns[e.ColumnIndex].HeaderText.ToString(),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (aaa == DialogResult.Yes)
                {
                    if(advancedDataGridView1.Columns[e.ColumnIndex].Tag.ToString() == "validaSI")   // la columna se valida?
                    {
                        // valida si el dato ingresado es valido en la columna
                        if (lib.validac(noeta[0], noeta[1], e.FormattedValue.ToString()) == true)
                        {
                            // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                            lib.actuac(nomtab, campo, e.FormattedValue.ToString(),advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show("El valor no es válido para la columna", "Atención - Corrija");
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        // llama a libreria con los datos para el update - tabla,id,campo,nuevo valor
                        lib.actuac(nomtab, campo, e.FormattedValue.ToString(), advancedDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }*/
        }
        #endregion

    }
}
