using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carga_Masiva_POS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int contadorMalos = 0;
            OdbcConnection DbConnection = new OdbcConnection("Driver={SQL Server};Server=172.20.1.23;Database=SumaLealtad;Uid=UserSisLeal;Pwd=1234;");
            DbConnection.Open();
            OdbcCommand DbCommand = DbConnection.CreateCommand();

            //TABLA PARAMETROS
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [ID_PARAM],[NOMBRE_PARAM],[DESCRIPCION],[MTO_CANJE],[PTS_CANJE],[MTO_ACR],[PTS_ACR],[TIPO_PRE],[TIPO_SUMA],[FECHA_ACT_PARAM],[FECHA_DESAC_PARAM],[FECHA] FROM PARAMETROS";
            OdbcDataReader DbReader = DbCommand.ExecuteReader();
            int fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM PARAMETROS");
                listBox1.Items.Add("Tabla PARAMETROS borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    PARAMETRO P = new PARAMETRO()
                    {
                        ID_PARAM = DbReader.GetInt32(0),
                        NOMBRE_PARAM = DbReader.GetString(1),
                        DESCRIPCION = DbReader.GetString(2),
                        MTO_CANJE = DbReader.GetInt32(3),
                        PTS_CANJE = DbReader.GetInt32(4),
                        MTO_ACR = DbReader.GetInt32(5),
                        PTS_ACR = DbReader.GetInt32(6),
                        TIPO_PRE = DbReader.GetInt32(7),
                        TIPO_SUMA = DbReader.GetInt32(8),
                        FECHA_ACT_PARAM = DbReader.GetDateTime(9),
                        FECHA_DESAC_PARAM = DbReader.GetDateTime(10),
                        FECHA = DbReader.GetDateTime(11)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.ID_PARAM + ","
                                                              + P.NOMBRE_PARAM + ","
                                                              + P.DESCRIPCION + ","
                                                              + P.MTO_CANJE + ","
                                                              + P.PTS_CANJE + ","
                                                              + P.MTO_ACR + ","
                                                              + P.PTS_ACR + ","
                                                              + P.TIPO_PRE + ","
                                                              + P.TIPO_SUMA + ","
                                                              + P.FECHA_ACT_PARAM + ","
                                                              + P.FECHA_DESAC_PARAM + ","
                                                              + P.FECHA
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.PARAMETROS.Add(P);

                }
                listBox1.Items.Add("Filas tabla PARAMETROS: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA RESTRICCIONES
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_RESTRICCION],[FECHA_ACT_REST],[FECHA_DESAC_REST],[MINTRAN_ACR],[MAXTRAN_ACR],[MINMONTO_ACR],[MAXMONTO_ACR],[MINTRAN_RED],[MAXTRAN_RED],[MINMONTO_RED],[MAXMONTO_RED] FROM RESTRICCIONES";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM RESTRICCIONES");
                listBox1.Items.Add("Tabla RESTRICCIONES borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    RESTRICCION P = new RESTRICCION()
                    {
                        COD_RESTRICCION = DbReader.GetInt32(0),
                        FECHA_ACT_REST = DbReader.GetDateTime(1),
                        FECHA_DESAC_REST = DbReader.GetDateTime(2),
                        MINTRAN_ACR = null,
                        MAXTRAN_ACR = 2,
                        MINMONTO_ACR = null,
                        MAXMONTO_ACR = null,
                        MINTRAN_RED = null,
                        MAXTRAN_RED = null,
                        MAXMONTO_RED = null
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_RESTRICCION + ","
                                                              + P.FECHA_ACT_REST + ","
                                                              + P.FECHA_DESAC_REST + ","
                                                              + P.MINTRAN_ACR + ","
                                                              + P.MAXTRAN_ACR + ","
                                                              + P.MINMONTO_ACR + ","
                                                              + P.MAXMONTO_ACR + ","
                                                              + P.MINTRAN_RED + ","
                                                              + P.MAXTRAN_RED + ","
                                                              + P.MAXMONTO_RED
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.RESTRICCIONES.Add(P);

                }
                listBox1.Items.Add("Filas tabla RESTRICCIONES: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA PAIS_ESTADO
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM PAIS_ESTADO");
                listBox1.Items.Add("Tabla PAIS_ESTADO borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }

            //TABLA ESTADO_CIUDAD 
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM ESTADO_CIUDAD");
                listBox1.Items.Add("Tabla ESTADO_CIUDAD borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }

            //TABLA CIUDAD_MUNICIPIO
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM CIUDAD_MUNICIPIO");
                listBox1.Items.Add("Tabla CIUDAD_MUNICIPIO borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }

            //TABLA MUNICIPIO_PARROQUIA
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM MUNICIPIO_PARROQUIA");
                listBox1.Items.Add("Tabla MUNICIPIO_PARROQUIA borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }

            //TABLA PARROQUIA_URBANIZACION  
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM PARROQUIA_URBANIZACION");
                listBox1.Items.Add("Tabla PARROQUIA_URBANIZACION borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }

            //TABLA PAIS
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_PAIS],[DESCRIPC_PAIS] FROM PAIS";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM PAIS");
                listBox1.Items.Add("Tabla PAIS borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    PAIS P = new PAIS()
                    {
                        COD_PAIS = DbReader.GetString(0),
                        DESCRIPC_PAIS = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_PAIS + ","
                                                              + P.DESCRIPC_PAIS
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.PAISES.Add(P);

                }
                listBox1.Items.Add("Filas tabla PAIS: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA ESTADO
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_ESTADO],[DESCRIPC_ESTADO] FROM ESTADO";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM ESTADO");
                listBox1.Items.Add("Tabla ESTADO borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    ESTADO P = new ESTADO()
                    {
                        COD_ESTADO = DbReader.GetString(0),
                        DESCRIPC_ESTADO = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_ESTADO + ","
                                                              + P.DESCRIPC_ESTADO
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.ESTADOS.Add(P);

                }
                listBox1.Items.Add("Filas tabla ESTADO: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA CIUDAD 
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_CIUDAD],[DESCRIPC_CIUDAD] FROM CIUDAD";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM CIUDAD");
                listBox1.Items.Add("Tabla CIUDAD borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    CIUDAD P = new CIUDAD()
                    {
                        COD_CIUDAD = DbReader.GetString(0),
                        DESCRIPC_CIUDAD = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_CIUDAD + ","
                                                              + P.DESCRIPC_CIUDAD
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.CIUDADES.Add(P);

                }
                listBox1.Items.Add("Filas tabla CIUDAD: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA MUNICIPIO            
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_MUNICIPIO],[DESCRIPC_MUNICIPIO] FROM MUNICIPIO";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM MUNICIPIO");
                listBox1.Items.Add("Tabla MUNICIPIO borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    MUNICIPIO P = new MUNICIPIO()
                    {
                        COD_MUNICIPIO = DbReader.GetString(0),
                        DESCRIPC_MUNICIPIO = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_MUNICIPIO + ","
                                                              + P.DESCRIPC_MUNICIPIO
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.MUNICIPIOS.Add(P);

                }
                listBox1.Items.Add("Filas tabla MUNICIPIO: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA PARROQUIA 
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_PARROQUIA],[DESCRIPC_PARROQUIA] FROM PARROQUIA";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM PARROQUIA");
                listBox1.Items.Add("Tabla PARROQUIA borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    PARROQUIA P = new PARROQUIA()
                    {
                        COD_PARROQUIA = DbReader.GetString(0),
                        DESCRIPC_PARROQUIA = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_PARROQUIA + ","
                                                              + P.DESCRIPC_PARROQUIA
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.PARROQUIAS.Add(P);

                }
                listBox1.Items.Add("Filas tabla PARROQUIA: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA URBANIZACION
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT DISTINCT[COD_URBANIZACION],[DESCRIPC_URBANIZACION] FROM URBANIZACION";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //BORRAR TABLA
                db.Database.ExecuteSqlCommand("DELETE FROM URBANIZACION");
                listBox1.Items.Add("Tabla URBANIZACION borrada.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    URBANIZACION P = new URBANIZACION()
                    {
                        COD_URBANIZACION = DbReader.GetString(0),
                        DESCRIPC_URBANIZACION = DbReader.GetString(1)
                    };
                    listBox1.Items.Add("Fila " + filas + ": " + P.COD_URBANIZACION + ","
                                                              + P.DESCRIPC_URBANIZACION
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    db.URBANIZACIONES.Add(P);

                }
                listBox1.Items.Add("Filas tabla URBANIZACION: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                db.SaveChanges();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA PAIS_ESTADO
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_PAIS],[COD_ESTADO] FROM PAIS_ESTADO";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    listBox1.Items.Add("Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1)
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    try
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO PAIS_ESTADO VALUES ('" + DbReader.GetString(0) + "','" + DbReader.GetString(1) + "')");
                    }
                    catch (Exception EX)
                    {
                        contadorMalos++;
                        listBox1.Items.Add("Falló inserción Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1) + " Error: " + EX.InnerException
                                                              );
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        listBox1.Refresh();
                    }
                }
                listBox1.Items.Add("Filas tabla PAIS_ESTADO: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA ESTADO_CIUDAD
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_ESTADO],[COD_CIUDAD] FROM ESTADO_CIUDAD";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    listBox1.Items.Add("Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1)
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    try
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO ESTADO_CIUDAD VALUES ('" + DbReader.GetString(0) + "','" + DbReader.GetString(1) + "')");
                    }
                    catch (Exception EX)
                    {
                        contadorMalos++;
                        listBox1.Items.Add("Falló inserción Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1) + " Error: " + EX.InnerException
                                                              );
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        listBox1.Refresh();
                    }
                }
                listBox1.Items.Add("Filas tabla ESTADO_CIUDAD: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA CIUDAD_MUNICIPIO
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_CIUDAD],[COD_MUNICIPIO] FROM CIUDAD_MUNICIPIO";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    listBox1.Items.Add("Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1)
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    try
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO CIUDAD_MUNICIPIO VALUES ('" + DbReader.GetString(0) + "','" + DbReader.GetString(1) + "')");
                    }
                    catch (Exception EX)
                    {
                        contadorMalos++;
                        listBox1.Items.Add("Falló inserción Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1) + " Error: " + EX.InnerException
                                                              );
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        listBox1.Refresh();
                    }
                }
                listBox1.Items.Add("Filas tabla CIUDAD_MUNICIPIO: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA MUNICIPIO_PARROQUIA
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_MUNICIPIO],[COD_PARROQUIA] FROM MUNICIPIO_PARROQUIA";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    listBox1.Items.Add("Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1)
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    try
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO MUNICIPIO_PARROQUIA VALUES ('" + DbReader.GetString(0) + "','" + DbReader.GetString(1) + "')");
                    }
                    catch (Exception EX)
                    {
                        contadorMalos++;
                        listBox1.Items.Add("Falló inserción Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1) + " Error: " + EX.InnerException
                                                              );
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        listBox1.Refresh();
                    }
                }
                listBox1.Items.Add("Filas tabla MUNICIPIO_PARROQUIA: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            //TABLA PARROQUIA_URBANIZACION
            DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT [COD_PARROQUIA],[COD_URBANIZACION] FROM PARROQUIA_URBANIZACION";
            DbReader = DbCommand.ExecuteReader();
            fCount = DbReader.FieldCount;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                int filas = 0;
                while (DbReader.Read())
                {
                    filas++;
                    listBox1.Items.Add("Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1)
                                                              );
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.Refresh();
                    try
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO PARROQUIA_URBANIZACION VALUES ('" + DbReader.GetString(0) + "','" + DbReader.GetString(1) + "')");
                    }
                    catch (Exception EX)
                    {
                        contadorMalos++;
                        listBox1.Items.Add("Falló inserción Fila " + filas + ": " + DbReader.GetString(0) + ","
                                                              + DbReader.GetString(1) + " Error: " + EX.InnerException
                                                              );
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        listBox1.Refresh();
                    }
                }
                listBox1.Items.Add("Filas tabla PARROQUIA_URBANIZACION: " + filas);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                listBox1.Items.Add("Filas insertadas.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
            }
            DbReader.Close();
            DbCommand.Dispose();

            DbConnection.Close();

            listBox1.Items.Add("Carga Finalizada.");
            listBox1.Items.Add("Registros con error: " + contadorMalos.ToString());
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox1.Refresh();
        }
    }
}
