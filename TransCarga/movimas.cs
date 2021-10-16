using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TransCarga
{
    public partial class movimas : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public bool retorno;
        //public string[,] para3;
        public string[,] para3 = new string[10, 7]
        {
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" },
                {"","","","","","","" }
        };

        string para1, para2;
        libreria lib = new libreria();
        string DB_CONN_STR = "server=" + login.serv + ";uid=" + login.usua + ";pwd=" + login.cont + ";database=" + login.data + ";";

        public movimas(string parm1,string parm2,string[,] parm3)    // parm1 = modo = reserva o salida
        {                                                       // parm2 = 
            InitializeComponent();                              // parm3 = string[,] pasa = new string[10, 7]
            para1 = parm1;  // modo
            if (parm1 == "reserva")
            {
                lb_titulo.Text = "SALIDA A REPARTO";
                panel3.Visible = true;
                panel3.Left = 2;    // 7
                panel3.Top = 25;     // 30
                panel4.Visible = false;

                dataGridView1.Columns.Add("id", "ID");
                dataGridView1.Columns.Add("guia", "GUIA");
                dataGridView1.Columns.Add("cant", "CANT");
                dataGridView1.Columns.Add("almac", "ALMACEN");
                dataGridView1.Columns.Add("repart", "REPARTIDOR");
                dataGridView1.Columns.Add("frepar", "F_REPART");
                dataGridView1.Columns[0].Width = 40;    // id
                dataGridView1.Columns[1].Width = 90;    // guia
                dataGridView1.Columns[2].Width = 50;    // cantid
                dataGridView1.Columns[3].Width = 70;    // almacen
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                for (int i = 0; i < 10; i++)
                {
                    dataGridView1.Rows.Add(parm3[i, 0], parm3[i, 1], parm3[i, 2], parm3[i, 3]);
                }
                tx_fecon.Text = DateTime.Now.ToString("dd/MM/yyyy");
                tx_contra.MaxLength = 6;
            }
            if (parm1 == "salida")
            {
                lb_titulo.Text = "ENTREGA EN OFICINA";
                panel4.Visible = true;
                panel4.Left = 2;    // 7
                panel4.Top = 25;     // 30
                panel3.Visible = false;
                rb_mov.Checked = true;
                combos();
            }
            this.KeyPreview = true; // habilitando la posibilidad de pasar el tab con el enter
        }
        private void movimas_Load(object sender, EventArgs e)
        {
            combos();
        }
        private void movimas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void bt_close_Click(object sender, EventArgs e)
        {
            retorno = false;    // false = no se hizo nada
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (tx_contra.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el reponsable del despacho","Complete la información",MessageBoxButtons.OK,MessageBoxIcon.Information);
                tx_contra.Focus();
                return;
            }
            var aa = MessageBox.Show("Confirma que desea grabar la operación?", "Confirme por favor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (aa == DialogResult.Yes)
            {
                if (para1 == "reserva")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() != "")
                        {
                            dataGridView1.Rows[i].Cells[4].Value = tx_contra.Text;
                            dataGridView1.Rows[i].Cells[5].Value = tx_fecon.Text;
                            //
                            para3[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            para3[i, 1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            para3[i, 2] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            para3[i, 3] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            para3[i, 4] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                            para3[i, 5] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        }
                    }
                    retorno = true; // true = se efectuo la operacion
                }
                if (lb_titulo.Text.ToUpper() == "SALIDA")
                {
                    if (salida() == true)
                    {
                        MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
                        cn.Open();
                        try
                        {
                            // actualizamos el temporal
                            string texto = "";
                            if(rb_mov.Checked == true) texto = "update tempo set evento=@cont,almdes=@almd";
                            if (rb_ajuste.Checked == true) texto = "update tempo set idres=0,evento=@cont,almdes=@almd";
                            MySqlCommand micon = new MySqlCommand(texto, cn);
                            micon.Parameters.AddWithValue("@cont", tx_evento.Text);
                            micon.Parameters.AddWithValue("@almd", tx_dat_dest.Text);
                            micon.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error en conexión");
                            Application.Exit();
                        }
                        retorno = true; // true = se efectuo la operacion
                    }
                }
                this.Close();
            }
        }
        //
        private bool salida()
        {
            bool bien = false;
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                // si es tipo de salida por movimiento
                if (rb_mov.Checked == true)
                {
                    // debe retornar el evento y almacen de destino
                    bien = true;
                }
                // salida por ajuste
                if (rb_ajuste.Checked == true)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string texto = "insert into salidash " +
                            "(fecha,pedido,reserva,evento,coment,user,dia,llegada,partida,tipomov,contrato) " +
                            "values " +
                            "(@ptxfec,@ptxped,@ptxcon,@ptxt03,@ptxcom,@vg_us,now(),@ptxlle,@ptxpar,@ptxtmo,@ptxctr)";
                        MySqlCommand micon = new MySqlCommand(texto, cn);
                        micon.Parameters.AddWithValue("@ptxfec", dtp_fsal.Value.ToString("yyyy-MM-dd"));
                        micon.Parameters.AddWithValue("@ptxped", "");
                        micon.Parameters.AddWithValue("@ptxcon", "");
                        micon.Parameters.AddWithValue("@ptxt03", tx_evento.Text);
                        micon.Parameters.AddWithValue("@ptxcom", tx_comsal.Text);
                        micon.Parameters.AddWithValue("@vg_us", TransCarga.Program.vg_user);
                        micon.Parameters.AddWithValue("@ptxlle", "");
                        micon.Parameters.AddWithValue("@ptxpar", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        micon.Parameters.AddWithValue("@ptxtmo", "1");
                        micon.Parameters.AddWithValue("@ptxctr", "");
                        micon.ExecuteNonQuery();
                        //
                        texto = "select MAX(idsalidash) as idreg from salidash";
                        micon = new MySqlCommand(texto, cn);
                        MySqlDataReader dr = micon.ExecuteReader();
                        if (dr.Read())
                        {
                            tx_idr.Text = dr.GetString(0);
                        }
                        dr.Close();
                        //
                        texto = "insert into salidasd " +
                            "(salidash,item,cant,user,dia) " +
                            "values " +
                            "(@v_id,@nar,@can,@vg_us,now())";
                        micon = new MySqlCommand(texto, cn);
                        micon.Parameters.AddWithValue("@v_id", tx_idr.Text);
                        micon.Parameters.AddWithValue("@nar", dataGridView1.Rows[i].Cells[0].Value.ToString());
                        micon.Parameters.AddWithValue("@can", "1");
                        micon.Parameters.AddWithValue("@vg_us", "Lorenzo");
                        micon.ExecuteNonQuery();
                        // borra en almloc
                        string borra = "delete from almloc where id=@idr";
                        micon = new MySqlCommand(borra, cn);
                        micon.Parameters.AddWithValue("@idr", dataGridView1.Rows[i].Cells[4].Value.ToString());
                        micon.ExecuteNonQuery();
                    }
                    bien = true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error en conexión");
                Application.Exit();
            }
            return bien;
        }
        // RESERVAS **********************
        private void tx_contra_Leave(object sender, EventArgs e)
        {
            if (tx_contra.Text == "")
            {
                button1.Focus();
                return;
            }
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se puede conectar con el servidor");
                Application.Exit();
                return;
            }
            cn.Close();
        }
        private void combos()
        {
            this.panel4.Focus();
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                // seleccion de los almacenes de destino
                this.cmb_dest.Items.Clear();
                tx_dat_dest.Text = "";
                ComboItem citem_dest = new ComboItem();
                const string condest = "select descrizionerid,idcodice from desc_alm " +
                    "where numero=1";
                MySqlCommand cmd2 = new MySqlCommand(condest, cn);
                DataTable dt2 = new DataTable();
                MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
                da2.Fill(dt2);
                foreach (DataRow row in dt2.Rows)
                {
                    citem_dest.Text = row.ItemArray[0].ToString();
                    citem_dest.Value = row.ItemArray[1].ToString();
                    this.cmb_dest.Items.Add(citem_dest);
                    this.cmb_dest.ValueMember = citem_dest.Value.ToString();
                }
                cn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se puede conectar al servidor");
                Application.Exit();
                return;
            }
        }
        private void cmb_dest_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(DB_CONN_STR);
            cn.Open();
            try
            {
                //int aq = Int16.Parse(this.cmb_dest.SelectedIndex.ToString());
                string consulta = "select idcodice from desc_alm where descrizionerid=@des and numero=1";
                MySqlCommand micon = new MySqlCommand(consulta, cn);
                micon.Parameters.AddWithValue("@des", cmb_dest.Text.ToString());
                MySqlDataReader midr = micon.ExecuteReader();
                if (midr.Read())
                {
                    this.tx_dat_dest.Text = midr["idcodice"].ToString();
                }
                midr.Close();
                cn.Close();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message,"No se pudo conectar con el servidor");
                Application.Exit();
                return;
            }
        }

        private void rb_mov_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_mov.Checked == true)
            {
                tx_dat_dest.Text = "";
                cmb_dest.Enabled = true;
                tx_evento.Enabled = true;
            }
        }
        private void rb_ajuste_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ajuste.Checked == true)
            {
                tx_dat_dest.Text = "";
                cmb_dest.SelectedIndex = -1;
                cmb_dest.Enabled = false;
                tx_evento.Text = "";
                tx_evento.Enabled = false;
            }
        }
    }
}
