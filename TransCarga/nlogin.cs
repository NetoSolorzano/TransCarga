using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace TransCarga
{
    public partial class nlogin : Form
    {
        // conexion a la base de datos
        public static string serv = "solorsoft.com"; // Decrypt(ConfigurationManager.AppSettings["serv"].ToString(), true);     // "solorsoft.com";
        public static string port = "3306";          // ConfigurationManager.AppSettings["port"].ToString();    // "3306"; 
        public static string usua = "solorsof_rei";  // ConfigurationManager.AppSettings["user"].ToString();                    // "solorsof_rei";
        public static string cont = "190969Sorol";   // Decrypt(ConfigurationManager.AppSettings["pass"].ToString(), true);     // "190969Sorol";
        public static string data = "solorsof_TransCarga"; //ConfigurationManager.AppSettings["data"].ToString();
        public static string ctl = "";               // ConfigurationManager.AppSettings["ConnectionLifeTime"].ToString();
        string DB_CONN_STR = "server=" + serv + ";uid=" + usua + ";pwd=" + cont + ";database=" + data + ";";
        //libreria lib = new libreria();
        public DataTable dt_enlaces = new DataTable();
        public static string CadenaConexion = "Data Source=TransCarga.db";
        public nlogin()
        {
            InitializeComponent();
        }

        private void nlogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
