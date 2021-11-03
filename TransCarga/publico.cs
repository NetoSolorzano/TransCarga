using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransCarga
{
    class publico
    {
        // string de conexion
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + login.data + ";";
        libreria lib = new libreria();
        DataTable dtgrtcab = new DataTable();
        DataTable dtgrtdet = new DataTable();

        public void sololee(Form lfrm)
        {
            foreach (Control oControls in lfrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = false;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = false;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = false;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = false;
                }
            }
        }
        public void escribe(Form efrm)
        {
            foreach (Control oControls in efrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is ComboBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is RadioButton)
                {
                    oControls.Enabled = true;
                }
                if (oControls is DateTimePicker)
                {
                    oControls.Enabled = true;
                }
                if (oControls is MaskedTextBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is GroupBox)
                {
                    oControls.Enabled = true;
                }
                if (oControls is CheckBox)
                {
                    oControls.Enabled = true;
                }
            }
        }
        public void limpiar(Form ofrm)
        {
            foreach (Control oControls in ofrm.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
            }
        }
        public void limpia_chk(Form oForm)
        {
            foreach (Control oControls in oForm.Controls)
            {
                if (oControls is CheckBox)
                {
                    CheckBox chk = oControls as CheckBox;
                    chk.Checked = false;
                }
            }
        }
        public void limpia_cmb(Form oForm)
        {
            foreach (Control oControls in oForm.Controls)
            {
                if (oControls is ComboBox)
                {
                    ComboBox cmb = oControls as ComboBox;
                    cmb.SelectedIndex = -1;
                }
            }
        }
        public void limpiapag(TabPage pag)
        {
            foreach (Control oControls in pag.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if (oControls is CheckBox)
                {
                    CheckBox chk = oControls as CheckBox;
                    chk.Checked = false;
                }
                if (oControls is ComboBox)
                {
                    ComboBox cmb = oControls as ComboBox;
                    cmb.SelectedIndex = -1;
                }
            }
        }
        public void limpiagbox(GroupBox gbox)
        {
            foreach(Control oControls in gbox.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if (oControls is CheckBox)
                {
                    CheckBox chk = oControls as CheckBox;
                    chk.Checked = false;
                }
                if (oControls is ComboBox)
                {
                    ComboBox cmb = oControls as ComboBox;
                    cmb.SelectedIndex = -1;
                }
            }
        }
        public void limpiasplit(SplitContainer split)
        {
            foreach(Control oControls in split.Panel1.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if (oControls is CheckBox)
                {
                    CheckBox chk = oControls as CheckBox;
                    chk.Checked = false;
                }
                if (oControls is ComboBox)
                {
                    ComboBox cmb = oControls as ComboBox;
                    cmb.SelectedIndex = -1;
                }
            }
            foreach (Control oControls in split.Panel2.Controls)
            {
                if (oControls is TextBox)
                {
                    oControls.Text = "";
                }
                if (oControls is CheckBox)
                {
                    CheckBox chk = oControls as CheckBox;
                    chk.Checked = false;
                }
                if (oControls is ComboBox)
                {
                    ComboBox cmb = oControls as ComboBox;
                    cmb.SelectedIndex = -1;
                }
            }
        }
        // varios
        public int CentimeterToPixel(Form oForm, double Centimeter)
        {
            double pixel = -1;
            using (Graphics g = oForm.CreateGraphics())
            {
                pixel = Centimeter * g.DpiY / 2.54d;
            }
            return (int)pixel;
        }
        public void muestra_gr(string ser, string cor, string nomfcr)                 // muestra la grt 
        {
            using (MySqlConnection conn = new MySqlConnection(DB_CONN_STR))
            {
                if (lib.procConn(conn) == true)
                {
                    string consulta = "select a.id,a.fechopegr,a.sergui,a.numgui,a.numpregui,a.tidodegri,a.nudodegri,a.nombdegri,a.diredegri," +
                        "a.ubigdegri,a.tidoregri,a.nudoregri,a.nombregri,a.direregri,a.ubigregri,lo.descrizionerid as ORIGEN,a.dirorigen,a.ubiorigen," +
                        "ld.descrizionerid as DESTINO,a.dirdestin,a.ubidestin,a.docsremit,a.obspregri,a.clifingri,a.cantotgri,a.pestotgri," +
                        "a.tipmongri,a.tipcamgri,a.subtotgri,a.igvgri,round(a.totgri,1) as totgri,a.totpag,a.salgri,s.descrizionerid as ESTADO,a.impreso," +
                        "a.frase1,a.frase2,a.fleteimp,a.tipintrem,a.tipintdes,a.tippagpre,a.seguroE,a.userc,a.userm,a.usera," +
                        "a.serplagri,a.numplagri,a.plaplagri,a.carplagri,a.autplagri,a.confvegri,a.breplagri,a.proplagri," +
                        "ifnull(b.chocamcar,'') as chocamcar,ifnull(b.fecplacar,'') as fecplacar,ifnull(b.fecdocvta,'') as fecdocvta,ifnull(f.descrizionerid,'') as tipdocvta," +
                        "ifnull(b.serdocvta,'') as serdocvta,ifnull(b.numdocvta,'') as numdocvta,ifnull(b.codmonvta,'') as codmonvta," +
                        "ifnull(b.totdocvta,0) as totdocvta,ifnull(b.codmonpag,'') as codmonpag,ifnull(b.totpagado,0) as totpagado,ifnull(b.saldofina,0) as saldofina," +
                        "ifnull(b.feculpago,'') as feculpago,ifnull(b.estadoser,'') as estadoser,ifnull(c.razonsocial,'') as razonsocial,a.grinumaut," +
                        "ifnull(d.marca,'') as marca,ifnull(d.modelo,'') as modelo,a.teleregri as telrem,a.teledegri as teldes,ifnull(t.nombclt,'') as clifact," +
                        "u1.nombre AS distrem,u2.nombre as provrem,u3.nombre as deptrem,v1.nombre as distdes,v2.nombre as provdes,v3.nombre as deptdes,mo.descrizionerid as MON " +
                        "from cabguiai a " +
                        "left join controlg b on b.serguitra=a.sergui and b.numguitra=a.numgui " +
                        "left join desc_tdv f on f.idcodice=b.tipdocvta " +
                        "left join cabfactu t on t.tipdvta=a.tipdocvta and t.serdvta=a.serdocvta and t.numdvta=a.numdocvta " +
                        "left join anag_for c on c.ruc=a.proplagri and c.tipdoc=@tdep " +
                        "left join vehiculos d on d.placa=a.plaplagri " +
                        "left join anag_cli er on er.ruc=a.nudoregri and er.tipdoc=a.tidoregri " +
                        "left join anag_cli ed on ed.ruc=a.nudodegri and ed.tipdoc=a.tidodegri " +
                        "left join desc_est s on s.idcodice=a.estadoser " +
                        "left join desc_loc lo on lo.idcodice=a.locorigen " +
                        "left join desc_loc ld on ld.idcodice=a.locdestin " +
                        "left join desc_mon mo on mo.idcodice=a.tipmongri " +
                        "LEFT JOIN ubigeos u1 ON CONCAT(u1.depart, u1.provin, u1.distri)= a.ubigregri " +
                        "LEFT JOIN(SELECT* FROM ubigeos WHERE depart<>'00' AND provin<>'00' AND distri = '00') u2 ON u2.depart = left(a.ubigregri, 2) AND u2.provin = concat(substr(a.ubigregri, 3, 2)) " +
                        "LEFT JOIN (SELECT* FROM ubigeos WHERE depart<>'00' AND provin='00' AND distri = '00') u3 ON u3.depart = left(a.ubigregri, 2) " +
                        "LEFT JOIN ubigeos v1 ON CONCAT(v1.depart, v1.provin, v1.distri)= a.ubigdegri " +
                        "LEFT JOIN (SELECT* FROM ubigeos WHERE depart<>'00' AND provin<>'00' AND distri = '00') v2 ON v2.depart = left(a.ubigdegri, 2) AND v2.provin = concat(substr(a.ubigdegri, 3, 2)) " +
                        "LEFT JOIN (SELECT* FROM ubigeos WHERE depart<>'00' AND provin='00' AND distri = '00') v3 ON v3.depart = left(a.ubigdegri, 2) " +
                        "where a.sergui = @ser and a.numgui = @num";
                    using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                    {
                        micon.Parameters.AddWithValue("@ser", ser);
                        micon.Parameters.AddWithValue("@num", cor);
                        micon.Parameters.AddWithValue("@tdep", "DOC002");
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            dtgrtcab.Clear();
                            da.Fill(dtgrtcab);
                        }
                    }
                    consulta = "select id,sergui,numgui,cantprodi,unimedpro,codiprodi,descprodi,round(pesoprodi,1),precprodi,totaprodi " +
                        "from detguiai where sergui = @ser and numgui = @num";
                    using (MySqlCommand micon = new MySqlCommand(consulta, conn))
                    {
                        micon.Parameters.AddWithValue("@ser", ser);
                        micon.Parameters.AddWithValue("@num", cor);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(micon))
                        {
                            dtgrtdet.Clear();
                            da.Fill(dtgrtdet);
                        }
                    }
                }
                // llenamos el set
                setParaCrystal("GRT", nomfcr);
            }
        }
        private void setParaCrystal(string repo, string nomfcr)                    // genera el set para el reporte de crystal
        {
            if (repo == "GRT")
            {
                conClie datos = generarepgrt(nomfcr);
                frmvizoper visualizador = new frmvizoper(datos);
                visualizador.Show();
            }
        }
        private conClie generarepgrt(string rpt_grt)
        {
            conClie guiaT = new conClie();
            conClie.gr_ind_cabRow rowcabeza = guiaT.gr_ind_cab.Newgr_ind_cabRow();
            // CABECERA
            DataRow row = dtgrtcab.Rows[0];
            rowcabeza.formatoRPT = rpt_grt;
            rowcabeza.id = row["id"].ToString(); // tx_idr.Text;
            rowcabeza.estadoser = row["ESTADO"].ToString(); // tx_estado.Text;
            rowcabeza.sergui = row["sergui"].ToString(); // tx_serie.Text;
            rowcabeza.numgui = row["numgui"].ToString(); // tx_numero.Text;
            rowcabeza.numpregui = row["numpregui"].ToString(); // tx_pregr_num.Text;
            rowcabeza.fechope = row["fechopegr"].ToString().Substring(0, 10); // tx_fechope.Text;
            if (row["fecplacar"].ToString() == "") rowcabeza.fechTraslado = "";
            else rowcabeza.fechTraslado = row["fecplacar"].ToString().Substring(8, 2) + "/" + row["fecplacar"].ToString().Substring(5, 2) + "/" + row["fecplacar"].ToString().Substring(0, 4); // tx_pla_fech.Text;
            rowcabeza.frase1 = row["ESTADO"].ToString(); //(tx_dat_estad.Text == codAnul) ? v_fra1 : "";  // campo para etiqueta "ANULADO"
            rowcabeza.frase2 = row["frase2"].ToString(); // (chk_seguridad.Checked == true) ? v_fra2 : "";  // campo para etiqueta "TIENE CLAVE"
            // origen - destino
            rowcabeza.nomDestino = row["DESTINO"].ToString(); // cmb_destino.Text;
            rowcabeza.direDestino = row["dirdestin"].ToString(); // tx_dirDestino.Text;
            rowcabeza.dptoDestino = ""; // 
            rowcabeza.provDestino = "";
            rowcabeza.distDestino = ""; // 
            rowcabeza.nomOrigen = row["ORIGEN"].ToString(); // cmb_origen.Text;
            rowcabeza.direOrigen = row["dirorigen"].ToString(); // tx_dirOrigen.Text;
            rowcabeza.dptoOrigen = "";  // no hay campo
            rowcabeza.provOrigen = "";
            rowcabeza.distOrigen = "";  // no hay campo
            // remitente
            rowcabeza.docRemit = "";    // cmb_docRem.Text;
            rowcabeza.numRemit = row["nudoregri"].ToString();    // tx_numDocRem.Text;
            rowcabeza.nomRemit = row["nombregri"].ToString();    // tx_nomRem.Text;
            rowcabeza.direRemit = row["direregri"].ToString();    // tx_dirRem.Text;
            rowcabeza.dptoRemit = row["deptrem"].ToString();   // row[""].ToString();    // tx_dptoRtt.Text;
            rowcabeza.provRemit = row["provrem"].ToString();    // tx_provRtt.Text;
            rowcabeza.distRemit = row["distrem"].ToString();    // tx_distRtt.Text;
            rowcabeza.telremit = row["telrem"].ToString();    // tx_telR.Text;
            // destinatario  
            rowcabeza.docDestinat = ""; // cmb_docDes.Text;
            rowcabeza.numDestinat = row["nudodegri"].ToString(); // tx_numDocDes.Text;
            rowcabeza.nomDestinat = row["nombdegri"].ToString(); // tx_nomDrio.Text;
            rowcabeza.direDestinat = row["diredegri"].ToString(); // tx_dirDrio.Text;
            rowcabeza.distDestinat = row["distdes"].ToString(); // tx_disDrio.Text;
            rowcabeza.provDestinat = row["provdes"].ToString(); // tx_proDrio.Text;
            rowcabeza.dptoDestinat = row["deptdes"].ToString(); // tx_dptoDrio.Text;
            rowcabeza.teldesti = row["teldes"].ToString(); // tx_telD.Text;
            // importes 
            rowcabeza.nomMoneda = row["MON"].ToString(); // cmb_mon.Text;
            rowcabeza.igv = row["igvgri"].ToString();         // no hay campo
            rowcabeza.subtotal = row["subtotgri"].ToString();    // no hay campo
            rowcabeza.total = row["totgri"].ToString(); // (chk_flete.Checked == true) ? tx_flete.Text : "";
            rowcabeza.docscarga = row["docsremit"].ToString(); // tx_docsOr.Text;
            rowcabeza.consignat = row["clifingri"].ToString(); // tx_consig.Text;
            // pie
            rowcabeza.marcamodelo = row["marca"].ToString() + " / " + row["modelo"].ToString(); // tx_marcamion.Text;
            rowcabeza.autoriz = row["autplagri"].ToString(); // tx_pla_autor.Text;
            rowcabeza.brevAyuda = "";   // falta este campo
            rowcabeza.brevChofer = row["breplagri"].ToString(); // tx_pla_brevet.Text;
            rowcabeza.nomChofer = row["chocamcar"].ToString(); // tx_pla_nomcho.Text;
            rowcabeza.placa = row["plaplagri"].ToString(); // tx_pla_placa.Text;
            rowcabeza.camion = row["carplagri"].ToString(); // tx_pla_carret.Text;
            rowcabeza.confvehi = row["confvegri"].ToString(); // tx_pla_confv.Text;
            rowcabeza.rucPropiet = row["proplagri"].ToString(); // tx_pla_ruc.Text;
            rowcabeza.nomPropiet = row["razonsocial"].ToString(); // tx_pla_propiet.Text;
            rowcabeza.fechora_imp = DateTime.Now.ToString();    // fecha de la "reimpresion" en el preview, No de la impresion en papel .. ojo
            rowcabeza.userc = (row["usera"].ToString() != "") ? row["usera"].ToString() : (row["userm"].ToString() != "") ? row["userm"].ToString() : row["userc"].ToString();
            //
            guiaT.gr_ind_cab.Addgr_ind_cabRow(rowcabeza);
            //
            // DETALLE  
            for (int i = 0; i < dtgrtdet.Rows.Count; i++)
            {
                conClie.gr_ind_detRow rowdetalle = guiaT.gr_ind_det.Newgr_ind_detRow();
                rowdetalle.fila = "";       // no estamos usando
                rowdetalle.cant = dtgrtdet.Rows[0].ItemArray[3].ToString(); // dataGridView1.Rows[i].Cells[0].Value.ToString();
                rowdetalle.codigo = "";     // no estamos usando
                rowdetalle.umed = dtgrtdet.Rows[0].ItemArray[4].ToString(); // dataGridView1.Rows[i].Cells[1].Value.ToString();
                rowdetalle.descrip = dtgrtdet.Rows[0].ItemArray[6].ToString(); // dataGridView1.Rows[i].Cells[2].Value.ToString();
                rowdetalle.precio = "";     // no estamos usando
                rowdetalle.total = "";      // no estamos usando
                rowdetalle.peso = string.Format("{0:#0.0}", dtgrtdet.Rows[0].ItemArray[7].ToString());  // dataGridView1.Rows[i].Cells[3].Value.ToString() + "Kg."
                guiaT.gr_ind_det.Addgr_ind_detRow(rowdetalle);
            }
            //
            return guiaT;
        }
    }
    public class CacheManager
    {
        static System.Collections.Hashtable ht = new System.Collections.Hashtable();
        public static void AddItem(string key, object value, uint timeToCache)
        {
            if (timeToCache > 3600)
                throw new ArgumentOutOfRangeException("Cache time cannot be more than 1 hour.");
            System.Threading.Timer t = new System.Threading.Timer(new TimerCallback(TimerProc));
            t.Change(timeToCache * 1000, System.Threading.Timeout.Infinite);
            ht.Add(t, key);
            AppDomain.CurrentDomain.SetData(key, value);
        }
        public static object GetItem(string key)
        {
            return AppDomain.CurrentDomain.GetData(key);
        }
        private static void TimerProc(object state)
        {
            System.Threading.Timer t = state as System.Threading.Timer;
            if (t != null)
            {
                object key = ht[t];
                ht.Remove(t);
                t.Dispose();

                if (key != null)
                    AppDomain.CurrentDomain.SetData(key.ToString(), null);
            }
        }
    }
}
