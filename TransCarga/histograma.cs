using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransCarga
{
    public partial class histograma : Form
    {
        public DataTable data;
        public histograma(DataTable dt)
        {
            InitializeComponent();
            this.Width = 500;
            this.Height = 600;
            if (data != null ) data.Clear();
            data = dt.Copy();
            
        }
        private void flechaV(int ptox, int ptoy, int anchox, int largox)
        {
            PictureBox box = new PictureBox();
            box.Image = Properties.Resources.abajo100T;
            box.SizeMode = PictureBoxSizeMode.StretchImage;
            box.Left = ptox;
            box.Top = ptoy;
            box.Width = anchox;
            box.Height = largox;
            this.Controls.Add(box);
        }
        private void flechaH(int ptox, int ptoy, int anchox, int largox)
        {
            PictureBox box = new PictureBox();
            box.Image = Properties.Resources.flecha100T;
            box.SizeMode = PictureBoxSizeMode.StretchImage;
            box.Left = ptox;
            box.Top = ptoy;
            box.Width = anchox;
            box.Height = largox;
            this.Controls.Add(box);
        }
        private void cuadro_Click(object sender, MouseEventArgs e)  // 
        {
            Panel algo = sender as Panel;
            MessageBox.Show(algo.Tag.ToString());

        }
        private void histograma_Load(object sender, EventArgs e)
        {
            Font tdet = new Font("Arial", 7);                    // leta para detalles
            int ctg = 0, ctp = 0, ctd = 0, ctcdv = 0, ctcgr = 0; // contador de cuadros guia, manifiestos, comprobantes, cobranzas de FT, cobranzas de GR
            int anchox = 120;       // ancho caja
            int largoy = 140;       // largo caja
            int larFlecha = 100;     // largo de las flechas
                                    //
            int ptoxf2_ini = 50;
            int alfidet = 13;       // alto fila detalle
            int distdet = 11;       // distancia entre filas detalle
            int ptoxF1 = 50;
            int ptoyF1 = 50;
            int ptoxF2 = ptoxf2_ini;
            int ptoyF2 = ptoyF1 + largoy + 10;
            int ccf2 = 0;
            foreach (DataRow row in data.Rows)
            {
                if (row.ItemArray[0].ToString() == "GUIA T.")   // Fila1
                {
                    ctg = ctg + 1;
                    pinta_guia(ctg, anchox, largoy, ptoxF1, ptoyF1, alfidet, distdet, tdet, row);
                }
                if (row.ItemArray[0].ToString() == "PLA.CARGA") // Fila1
                {
                    ptoxF1 = 50 + anchox + 10;
                    flechaH(ptoxF1, ptoyF1, larFlecha, largoy);
                    ptoxF1 = ptoxF1 + larFlecha + 10;
                    ptoyF1 = 50;
                    ctp = ctp + 1;
                    pinta_mani(ctp, anchox, largoy, ptoxF1, ptoyF1, alfidet, distdet, tdet, row);
                }
                if (row.ItemArray[0].ToString() == "DOC.VENTA") // fila2
                {
                    if (ccf2 == 0)
                    {
                        flechaV(ptoxF2, ptoyF2, anchox, larFlecha);
                        ptoyF2 = ptoyF2 + larFlecha + 10;
                    }
                    else
                    {
                        ptoxF2 = ptoxF2 + anchox + 10;
                        flechaH(ptoxF2, ptoyF2, larFlecha, largoy);
                        ptoxF2 = ptoxF2 + larFlecha + 10;
                    }
                    ccf2 = ccf2 + 1;
                    ctd = ctd + 1;
                    pinta_dv(ctd, anchox, largoy, ptoxF2, ptoyF2, alfidet, distdet, tdet, row);
                }
                if (row.ItemArray[0].ToString() == "COBRANZA FT")   // fila2
                {
                    if (ccf2 == 0)
                    {
                        flechaV(ptoxF2, ptoyF2, anchox, larFlecha);
                        ptoyF2 = ptoyF2 + larFlecha + 10;
                    }
                    else
                    {
                        ptoxF2 = ptoxF2 + anchox + 10;
                        flechaH(ptoxF2, ptoyF2, larFlecha, largoy);
                        ptoxF2 = ptoxF2 + larFlecha + 10;
                    }
                    ccf2 = ccf2 + 1;
                    ctcdv = ctcdv + 1;
                    pinta_codv(ctcdv, anchox, largoy, ptoxF2, ptoyF2, alfidet, distdet, tdet, row);
                }
                if (row.ItemArray[0].ToString() == "COBRANZA GR")   // fila2
                {
                    if (ccf2 == 0)
                    {
                        flechaV(ptoxF2, ptoyF2, anchox, larFlecha);
                        ptoyF2 = ptoyF2 + larFlecha + 10;
                    }
                    else
                    {
                        ptoxF2 = ptoxF2 + anchox + 10;
                        flechaH(ptoxF2, ptoyF2, larFlecha, largoy);
                        ptoxF2 = ptoxF2 + larFlecha + 10;
                    }
                    ccf2 = ccf2 + 1;
                    ctcgr = ctcgr + 1;
                    pinta_cogr(ctcgr, anchox, largoy, ptoxF2, ptoyF2, alfidet, distdet, tdet, row);
                }
            }
        }
        private void pinta_guia(int ctg, int anchox, int largoy, int ptoxF1, int ptoyF1, int alfidet, int distdet, Font tdet, DataRow row)
        {
            TextBox tit = new TextBox();
            tit.Name = "tx_guia" + ctg;
            tit.Text = "GUIA T.";
            tit.Enabled = false;
            tit.TextAlign = HorizontalAlignment.Center;
            tit.ForeColor = Color.Black;
            tit.BackColor = Color.White;
            tit.Width = anchox;

            Label tdid = new Label();
            tdid.Text = "Id.: " + row.ItemArray[11].ToString();
            tdid.Name = "id";
            tdid.Left = 3; tdid.Top = 20;
            tdid.AutoSize = false;
            tdid.Height = alfidet;
            tdid.Width = anchox;
            tdid.Font = tdet;
            Label tdes = new Label();
            tdes.Text = "Estado: " + row.ItemArray[1].ToString();
            tdes.Name = "estado";
            tdes.Left = 3; tdes.Top = tdid.Top + distdet;
            tdes.Font = tdet;
            tdes.AutoSize = false;
            tdes.Height = alfidet;
            tdes.Width = anchox;
            Label tdnr = new Label();
            tdnr.Text = "Nro.: " + row.ItemArray[3].ToString();
            tdnr.Name = "Nro.";
            tdnr.Left = 3; tdnr.Top = tdes.Top + distdet;
            tdnr.Font = tdet;
            tdnr.AutoSize = false;
            tdnr.Height = alfidet;
            tdnr.Width = anchox;
            Label tdfe = new Label();
            tdfe.Text = "F.Emisión: " + row.ItemArray[2].ToString().Substring(0, 10);
            tdfe.Name = "Emisión";
            tdfe.Left = 3; tdfe.Top = tdnr.Top + distdet;
            tdfe.Font = tdet;
            tdfe.AutoSize = false;
            tdfe.Height = alfidet;
            tdfe.Width = anchox;
            Label tdva = new Label();
            tdva.Text = "Valor: " + row.ItemArray[8].ToString() + " " + row.ItemArray[9].ToString();
            tdva.Name = "Valor";
            tdva.Left = 3; tdva.Top = tdfe.Top + distdet;
            tdva.Font = tdet;
            tdva.AutoSize = false;
            tdva.Height = alfidet;
            tdva.Width = anchox;
            Label tdsa = new Label();
            tdsa.Text = "Saldo: " + row.ItemArray[10].ToString();
            tdsa.Name = "Saldo";
            tdsa.Left = 3; tdsa.Top = tdva.Top + distdet;
            tdsa.Font = tdet;
            tdsa.AutoSize = false;
            tdsa.Height = alfidet;
            tdsa.Width = anchox;

            Panel cuadro = new Panel();
            cuadro.BackColor = Color.LightBlue;
            cuadro.BorderStyle = BorderStyle.Fixed3D;
            cuadro.Tag = "GR" + row.ItemArray[11].ToString();
            cuadro.Name = "tx_guia" + ctg;
            cuadro.Width = anchox;
            cuadro.Height = largoy;
            cuadro.Left = ptoxF1;
            cuadro.Top = ptoyF1;
            this.Controls.Add(cuadro);
            cuadro.Controls.Add(tit);
            cuadro.Controls.Add(tdid);
            cuadro.Controls.Add(tdes);
            cuadro.Controls.Add(tdnr);
            cuadro.Controls.Add(tdfe);
            cuadro.Controls.Add(tdva);
            cuadro.Controls.Add(tdsa);
            cuadro.MouseDoubleClick += new MouseEventHandler(cuadro_Click);

        }
        private void pinta_mani(int ctp, int anchox, int largoy, int ptoxF1, int ptoyF1, int alfidet, int distdet, Font tdet, DataRow row)
        {
            TextBox tit = new TextBox();
            tit.Name = "tx_manif" + ctp;
            tit.Text = "PLA.CARGA";
            tit.Enabled = false;
            tit.TextAlign = HorizontalAlignment.Center;
            tit.ForeColor = Color.Black;
            tit.BackColor = Color.White;
            tit.Width = anchox;

            Label tdid = new Label();
            tdid.Text = "Id.: " + row.ItemArray[11].ToString();
            tdid.Name = "id";
            tdid.Left = 3; tdid.Top = 20;
            tdid.AutoSize = false;
            tdid.Height = alfidet;
            tdid.Width = anchox;
            tdid.Font = tdet;
            Label tdes = new Label();
            tdes.Text = "Estado: " + row.ItemArray[1].ToString();
            tdes.Name = "estado";
            tdes.Left = 3; tdes.Top = tdid.Top + distdet;
            tdes.Font = tdet;
            tdes.AutoSize = false;
            tdes.Height = alfidet;
            tdes.Width = anchox;
            Label tdnr = new Label();
            tdnr.Text = "Nro.: " + row.ItemArray[3].ToString();
            tdnr.Name = "Nro.";
            tdnr.Left = 3; tdnr.Top = tdes.Top + distdet;
            tdnr.Font = tdet;
            tdnr.AutoSize = false;
            tdnr.Height = alfidet;
            tdnr.Width = anchox;
            Label tdfe = new Label();
            tdfe.Text = "F.Emisión: " + row.ItemArray[2].ToString().Substring(0, 10);
            tdfe.Name = "Emisión";
            tdfe.Left = 3; tdfe.Top = tdnr.Top + distdet;
            tdfe.Font = tdet;
            tdfe.AutoSize = false;
            tdfe.Height = alfidet;
            tdfe.Width = anchox;
            Label tdor = new Label();
            tdor.Text = "Origen: " + row.ItemArray[4].ToString();
            tdor.Name = "Origen";
            tdor.Left = 3; tdor.Top = tdfe.Top + distdet;
            tdor.Font = tdet;
            tdor.AutoSize = false;
            tdor.Height = alfidet;
            tdor.Width = anchox;
            Label tdde = new Label();
            tdde.Text = "Destino: " + row.ItemArray[5].ToString();
            tdde.Name = "destino";
            tdde.Left = 3; tdde.Top = tdor.Top + distdet;
            tdde.Font = tdet;
            tdde.AutoSize = false;
            tdde.Height = alfidet;
            tdde.Width = anchox;
            //
            Panel cmani = new Panel();
            cmani.BackColor = Color.LightBlue;
            cmani.BorderStyle = BorderStyle.Fixed3D;
            cmani.Tag = "PC" + row.ItemArray[11].ToString();
            cmani.Name = "tx_manif" + ctp;
            cmani.Width = anchox;
            cmani.Height = largoy;
            cmani.Left = ptoxF1;
            cmani.Top = ptoyF1;
            this.Controls.Add(cmani);
            cmani.Controls.Add(tdid);
            cmani.Controls.Add(tdes);
            cmani.Controls.Add(tdnr);
            cmani.Controls.Add(tdfe);
            cmani.Controls.Add(tdor);
            cmani.Controls.Add(tdde);
            cmani.Controls.Add(tit);
            cmani.MouseDoubleClick += new MouseEventHandler(cuadro_Click);
        }
        private void pinta_dv(int ctd, int anchox, int largoy, int ptoxF2, int ptoyF2, int alfidet, int distdet, Font tdet, DataRow row)
        {
            TextBox tit = new TextBox();
            tit.Name = "tx_dv" + ctd;
            tit.Text = "DOC.VENTA";
            tit.ForeColor = Color.Black;
            tit.BackColor = Color.White;
            tit.Enabled = false;
            tit.TextAlign = HorizontalAlignment.Center;
            tit.Width = anchox;

            Label tdid = new Label();
            tdid.Text = "Id.: " + row.ItemArray[11].ToString();
            tdid.Name = "id";
            tdid.Left = 3; tdid.Top = 20;
            tdid.AutoSize = false;
            tdid.Height = alfidet;
            tdid.Width = anchox;
            tdid.Font = tdet;
            Label tdes = new Label();
            tdes.Text = "Estado: " + row.ItemArray[1].ToString();
            tdes.Name = "estado";
            tdes.Left = 3; tdes.Top = tdid.Top + distdet;
            tdes.Font = tdet;
            tdes.AutoSize = false;
            tdes.Height = alfidet;
            tdes.Width = anchox;
            Label tdnr = new Label();
            tdnr.Text = "Nro.: " + row.ItemArray[3].ToString();
            tdnr.Name = "Nro.";
            tdnr.Left = 3; tdnr.Top = tdes.Top + distdet;
            tdnr.Font = tdet;
            tdnr.AutoSize = false;
            tdnr.Height = alfidet;
            tdnr.Width = anchox;
            Label tdfe = new Label();
            tdfe.Text = "F.Emisión: " + row.ItemArray[2].ToString().Substring(0, 10);
            tdfe.Name = "Emisión";
            tdfe.Left = 3; tdfe.Top = tdnr.Top + distdet;
            tdfe.Font = tdet;
            tdfe.AutoSize = false;
            tdfe.Height = alfidet;
            tdfe.Width = anchox;
            Label tdva = new Label();
            tdva.Text = "Valor: " + row.ItemArray[8].ToString() + " " + row.ItemArray[9].ToString();
            tdva.Name = "Valor";
            tdva.Left = 3; tdva.Top = tdfe.Top + distdet;
            tdva.Font = tdet;
            tdva.AutoSize = false;
            tdva.Height = alfidet;
            tdva.Width = anchox;
            Label tdor = new Label();
            tdor.Text = "Origen: " + row.ItemArray[4].ToString();
            tdor.Name = "Origen";
            tdor.Left = 3; tdor.Top = tdfe.Top + distdet;
            tdor.Font = tdet;
            tdor.AutoSize = false;
            tdor.Height = alfidet;
            tdor.Width = anchox;

            Panel cdv = new Panel();
            cdv.BackColor = Color.LightBlue;
            cdv.BorderStyle = BorderStyle.Fixed3D;
            cdv.Tag = "DV" + row.ItemArray[11].ToString();
            cdv.Name = "tx_dv" + ctd;
            cdv.Width = anchox;
            cdv.Height = largoy;
            cdv.Left = ptoxF2;
            cdv.Top = ptoyF2;
            this.Controls.Add(cdv);
            cdv.Controls.Add(tit);
            cdv.Controls.Add(tdid);
            cdv.Controls.Add(tdes);
            cdv.Controls.Add(tdnr);
            cdv.Controls.Add(tdfe);
            cdv.Controls.Add(tdor);
            cdv.Controls.Add(tdva);
            cdv.MouseDoubleClick += new MouseEventHandler(cuadro_Click);
        }
        private void pinta_codv(int ctcdv, int anchox, int largoy, int ptoxF2, int ptoyF2, int alfidet, int distdet, Font tdet, DataRow row)
        {
            TextBox tit = new TextBox();
            tit.Name = "tx_codv" + ctcdv;
            tit.Text = "COBRANZA FT";
            tit.ForeColor = Color.Black;
            tit.BackColor = Color.White;
            tit.Enabled = false;
            tit.TextAlign = HorizontalAlignment.Center;
            tit.Width = anchox;

            Label tdid = new Label();
            tdid.Text = "Id.: " + row.ItemArray[11].ToString();
            tdid.Name = "id";
            tdid.Left = 3; tdid.Top = 20;
            tdid.AutoSize = false;
            tdid.Height = alfidet;
            tdid.Width = anchox;
            tdid.Font = tdet;
            Label tdes = new Label();
            tdes.Text = "Estado: " + row.ItemArray[1].ToString();
            tdes.Name = "estado";
            tdes.Left = 3; tdes.Top = tdid.Top + distdet;
            tdes.Font = tdet;
            tdes.AutoSize = false;
            tdes.Height = alfidet;
            tdes.Width = anchox;
            Label tdnr = new Label();
            tdnr.Text = "Nro.: " + row.ItemArray[3].ToString();
            tdnr.Name = "Nro.";
            tdnr.Left = 3; tdnr.Top = tdes.Top + distdet;
            tdnr.Font = tdet;
            tdnr.AutoSize = false;
            tdnr.Height = alfidet;
            tdnr.Width = anchox;
            Label tdfe = new Label();
            tdfe.Text = "F.Emisión: " + row.ItemArray[2].ToString().Substring(0, 10);
            tdfe.Name = "Emisión";
            tdfe.Left = 3; tdfe.Top = tdnr.Top + distdet;
            tdfe.Font = tdet;
            tdfe.AutoSize = false;
            tdfe.Height = alfidet;
            tdfe.Width = anchox;
            Label tdva = new Label();
            tdva.Text = "Valor: " + row.ItemArray[8].ToString() + " " + row.ItemArray[9].ToString();
            tdva.Name = "Valor";
            tdva.Left = 3; tdva.Top = tdfe.Top + distdet;
            tdva.Font = tdet;
            tdva.AutoSize = false;
            tdva.Height = alfidet;
            tdva.Width = anchox;
            Label tdor = new Label();
            tdor.Text = "Origen: " + row.ItemArray[4].ToString();
            tdor.Name = "Origen";
            tdor.Left = 3; tdor.Top = tdfe.Top + distdet;
            tdor.Font = tdet;
            tdor.AutoSize = false;
            tdor.Height = alfidet;
            tdor.Width = anchox;

            Panel ccodv = new Panel();
            ccodv.BackColor = Color.LightBlue;
            ccodv.BorderStyle = BorderStyle.Fixed3D;
            ccodv.Tag = "CODV" + row.ItemArray[11].ToString();
            ccodv.Name = "tx_codv" + ctcdv;
            ccodv.Width = anchox;
            ccodv.Height = largoy;
            ccodv.Left = ptoxF2;
            ccodv.Top = ptoyF2;
            this.Controls.Add(ccodv);
            ccodv.Controls.Add(tit);
            ccodv.Controls.Add(tdid);
            ccodv.Controls.Add(tdes);
            ccodv.Controls.Add(tdnr);
            ccodv.Controls.Add(tdfe);
            ccodv.Controls.Add(tdor);
            ccodv.Controls.Add(tdva);
            ccodv.MouseDoubleClick += new MouseEventHandler(cuadro_Click);
        }
        private void pinta_cogr(int ctcgr, int anchox, int largoy, int ptoxF2, int ptoyF2, int alfidet, int distdet, Font tdet, DataRow row)
        {
            TextBox tit = new TextBox();
            tit.Name = "tx_cogr" + ctcgr;
            tit.Text = "COBRANZA GR";
            tit.ForeColor = Color.Black;
            tit.BackColor = Color.White;
            tit.Enabled = false;
            tit.TextAlign = HorizontalAlignment.Center;
            tit.Width = anchox;

            Label tdid = new Label();
            tdid.Text = "Id.: " + row.ItemArray[11].ToString();
            tdid.Name = "id";
            tdid.Left = 3; tdid.Top = 20;
            tdid.AutoSize = false;
            tdid.Height = alfidet;
            tdid.Width = anchox;
            tdid.Font = tdet;
            Label tdes = new Label();
            tdes.Text = "Estado: " + row.ItemArray[1].ToString();
            tdes.Name = "estado";
            tdes.Left = 3; tdes.Top = tdid.Top + distdet;
            tdes.Font = tdet;
            tdes.AutoSize = false;
            tdes.Height = alfidet;
            tdes.Width = anchox;
            Label tdnr = new Label();
            tdnr.Text = "Nro.: " + row.ItemArray[3].ToString();
            tdnr.Name = "Nro.";
            tdnr.Left = 3; tdnr.Top = tdes.Top + distdet;
            tdnr.Font = tdet;
            tdnr.AutoSize = false;
            tdnr.Height = alfidet;
            tdnr.Width = anchox;
            Label tdfe = new Label();
            tdfe.Text = "F.Emisión: " + row.ItemArray[2].ToString().Substring(0, 10);
            tdfe.Name = "Emisión";
            tdfe.Left = 3; tdfe.Top = tdnr.Top + distdet;
            tdfe.Font = tdet;
            tdfe.AutoSize = false;
            tdfe.Height = alfidet;
            tdfe.Width = anchox;
            Label tdva = new Label();
            tdva.Text = "Valor: " + row.ItemArray[8].ToString() + " " + row.ItemArray[9].ToString();
            tdva.Name = "Valor";
            tdva.Left = 3; tdva.Top = tdfe.Top + distdet;
            tdva.Font = tdet;
            tdva.AutoSize = false;
            tdva.Height = alfidet;
            tdva.Width = anchox;
            Label tdor = new Label();
            tdor.Text = "Origen: " + row.ItemArray[4].ToString();
            tdor.Name = "Origen";
            tdor.Left = 3; tdor.Top = tdfe.Top + distdet;
            tdor.Font = tdet;
            tdor.AutoSize = false;
            tdor.Height = alfidet;
            tdor.Width = anchox;

            Panel ccogr = new Panel();
            ccogr.BackColor = Color.LightBlue;
            ccogr.BorderStyle = BorderStyle.Fixed3D;
            ccogr.Tag = "COGR" + row.ItemArray[11].ToString();
            ccogr.Name = "tx_cogr" + ctcgr;
            ccogr.Width = anchox;
            ccogr.Height = largoy;
            ccogr.Left = ptoxF2;
            ccogr.Top = ptoyF2;
            this.Controls.Add(ccogr);
            ccogr.Controls.Add(tit);
            ccogr.Controls.Add(tdid);
            ccogr.Controls.Add(tdes);
            ccogr.Controls.Add(tdnr);
            ccogr.Controls.Add(tdfe);
            ccogr.Controls.Add(tdor);
            ccogr.Controls.Add(tdva);
            ccogr.MouseDoubleClick += new MouseEventHandler(cuadro_Click);
        }
    }
}
