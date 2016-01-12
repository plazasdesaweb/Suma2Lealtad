using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Carga_Masiva_Suma
{
    public partial class Form1 : Form
    {
        List<CedulaTarjeta> CedulasCards;
        List<AFILIACION_CLIENTE> AfiliacionesClientesSumaViejo = new List<AFILIACION_CLIENTE>();
        List<AFILIACION_CLIENTE> AfiliacionesClientesError = new List<AFILIACION_CLIENTE>();
        List<CLIENTE> ClientesMigrados = new List<CLIENTE>();
        List<AfiliadoSuma> AfiliadosSumaMigrados = new List<AfiliadoSuma>();
        List<AfiliadoSuma> AfiliadosPrepagoMigrados = new List<AfiliadoSuma>();
        List<Corporation> Corporaciones = new List<Corporation>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            btnProcesar.Enabled = false;
            btnFase2.Enabled = false;
            btnFase3.Enabled = false;
            DateTime inicio = DateTime.Now;
            lblInicio.Text = inicio.ToString();
            lstBuenos.Items.Clear();
            lstBuenos.Items.Add("INICIANDO FASE I - MIGRACIÓN DE CLIENTES  Y TARJETAS");
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            //Leer indice de cédulas desde Cards - SQL2005 - Entity Framework
            using (CardsEntities dbCards = new CardsEntities())
            {
                CedulasCards = (from c in dbCards.Clients
                                join t in dbCards.Cards on c.clientID equals t.clientID.Value
                                where t.clientID.HasValue && t.status == 1
                                select new CedulaTarjeta()
                                {
                                    cedula = c.CIDClient,
                                    pan = t.PAN,
                                    printed = t.printed,
                                    status = t.status.Value
                                }).OrderBy(x => x.cedula).ToList();
                lblCedulas.Text = CedulasCards.Count.ToString() + " documentos de identificación en 172.20.1.46/Cards/Clients";
                lblCedulas.Refresh();
            }
            //Leer clientes y afiliacion_cliente desde SumaLealtad - SQL2000 - OdbcDataReader
            OdbcConnection DbConnection = new OdbcConnection("Driver={SQL Server};Server=172.20.1.24;Database=SumaLealtad;Uid=UserSisLeal;Pwd=1234;");
            DbConnection.Open();
            OdbcCommand DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT a.[TIPO_DOCUMENTO],a.[NRO_DOCUMENTO],a.[NACIONALIDAD],a.[NOMBRE_CLIENTE1],a.[NOMBRE_CLIENTE2],a.[APELLIDO_CLIENTE1],a.[APELLIDO_CLIENTE2],a.[FECHA_NACIMIENTO],a.[SEXO],a.[EDO_CIVIL],a.[OCUPACION],a.[TELEFONO_HAB],a.[TELEFONO_OFIC],a.[TELEFONO_CEL],a.[E_MAIL],a.[COD_SUCURSAL],a.[COD_PAIS],a.[COD_ESTADO],a.[COD_CIUDAD],a.[COD_MUNICIPIO],a.[COD_PARROQUIA],a.[COD_URBANIZACION],a.[FECHA_CREACION], b.[COD_TIPO_CLIENTE] FROM CLIENTE a, AFILIACION_CLIENTE b WHERE a.[TIPO_DOCUMENTO] <> '' AND a.[TIPO_DOCUMENTO] = b.[TIPO_DOCUMENTO] AND a.[NRO_DOCUMENTO] = b.[NRO_DOCUMENTO] AND b.[ESTATUS_AFILIADO] = 'Activo' ORDER BY a.[NRO_DOCUMENTO]";
            OdbcDataReader DbReader = DbCommand.ExecuteReader();
            while (DbReader.Read())
            {
                AFILIACION_CLIENTE cliente = new AFILIACION_CLIENTE();
                cliente.CLIENTE = new CLIENTE();
                cliente.CLIENTE.TIPO_DOCUMENTO = DbReader.GetString(0).ToUpper().Trim();
                cliente.CLIENTE.NRO_DOCUMENTO = DbReader.IsDBNull(1) == true ? "" : DbReader.GetString(1).Trim();
                cliente.CLIENTE.NACIONALIDAD = DbReader.IsDBNull(2) == true ? "" : DbReader.GetString(2).ToUpper().Trim();
                cliente.CLIENTE.NOMBRE_CLIENTE1 = DbReader.IsDBNull(3) == true ? "" : DbReader.GetString(3).ToUpper().Trim();
                cliente.CLIENTE.NOMBRE_CLIENTE2 = DbReader.IsDBNull(4) == true ? "" : DbReader.GetString(4).ToUpper().Trim();
                cliente.CLIENTE.APELLIDO_CLIENTE1 = DbReader.IsDBNull(5) == true ? "" : DbReader.GetString(5).ToUpper().Trim();
                cliente.CLIENTE.APELLIDO_CLIENTE2 = DbReader.IsDBNull(6) == true ? "" : DbReader.GetString(6).ToUpper().Trim();
                cliente.CLIENTE.FECHA_NACIMIENTO = DbReader.IsDBNull(7) == true ? new DateTime?() : DbReader.GetDateTime(7);
                cliente.CLIENTE.SEXO = DbReader.IsDBNull(8) == true ? "" : DbReader.GetString(8).Trim();
                cliente.CLIENTE.EDO_CIVIL = DbReader.IsDBNull(9) == true ? "" : DbReader.GetString(9).Trim();
                cliente.CLIENTE.OCUPACION = DbReader.IsDBNull(10) == true ? "" : DbReader.GetString(10).ToUpper().Trim();
                cliente.CLIENTE.TELEFONO_HAB = DbReader.IsDBNull(11) == true ? "" : DbReader.GetString(11).Trim();
                cliente.CLIENTE.TELEFONO_OFIC = DbReader.IsDBNull(12) == true ? "" : DbReader.GetString(12).Trim();
                cliente.CLIENTE.TELEFONO_CEL = DbReader.IsDBNull(13) == true ? "" : DbReader.GetString(13).Trim();
                cliente.CLIENTE.E_MAIL = DbReader.IsDBNull(14) == true ? "" : DbReader.GetString(14).Trim();
                cliente.CLIENTE.COD_SUCURSAL = DbReader.IsDBNull(15) == true ? 0 : DbReader.GetInt32(15);
                //COD_PAIS no existe en CLIENTE.
                //COD_PAIS = DbReader.IsDBNull(16) == true ? 1 : DbReader.GetString(16).Trim(),
                cliente.CLIENTE.COD_ESTADO = DbReader.IsDBNull(17) == true ? "11" : DbReader.GetString(17).Trim();
                cliente.CLIENTE.COD_CIUDAD = DbReader.IsDBNull(18) == true ? "228" : DbReader.GetString(18).Trim();
                cliente.CLIENTE.COD_MUNICIPIO = DbReader.IsDBNull(19) == true ? "177" : DbReader.GetString(19).Trim();
                cliente.CLIENTE.COD_PARROQUIA = DbReader.IsDBNull(20) == true ? "599" : DbReader.GetString(20).Trim();
                cliente.CLIENTE.COD_URBANIZACION = DbReader.IsDBNull(21) == true ? "103" : DbReader.GetString(21).Trim();
                cliente.CLIENTE.FECHA_CREACION = DbReader.GetDateTime(22);
                cliente.COD_TIPO_CLIENTE = DbReader.IsDBNull(23) == true ? "" : DbReader.GetString(23).Trim();
                AfiliacionesClientesSumaViejo.Add(cliente);
            }
            DbReader.Close();
            DbCommand.Dispose();
            DbConnection.Close();
            lblClientes.Text = AfiliacionesClientesSumaViejo.Count.ToString() + " documentos de identificación en 172.20.1.24/SumaLealtad/CLIENTE";
            lblClientes.Refresh();
            int Migrados = 0;
            int Errores = 0;
            foreach (CedulaTarjeta c in CedulasCards)
            {
                if ((Migrados + Errores) % 300 == 0)
                {
                    lblTotalBuenos.Text = Migrados.ToString();
                    lblTotalMalos.Text = Errores.ToString();
                    this.Refresh();
                }
                int ocurrencias = 0;
                string tipoafiliacion = "";
                AFILIACION_CLIENTE clienteViejo = new AFILIACION_CLIENTE();
                try
                {
                    ocurrencias = AfiliacionesClientesSumaViejo.FindAll(x => x.CLIENTE.NRO_DOCUMENTO.Equals(c.cedula)).Count;
                    if (ocurrencias == 1)
                    {
                        clienteViejo = AfiliacionesClientesSumaViejo.Find(x => x.CLIENTE.NRO_DOCUMENTO.Equals(c.cedula));
                        if (clienteViejo.COD_TIPO_CLIENTE == "1")
                        {
                            tipoafiliacion = "Suma";
                        }
                        else if (clienteViejo.COD_TIPO_CLIENTE == "4")
                        {
                            tipoafiliacion = "Prepago";
                        }
                        else
                        {
                            throw new Exception("TIPO DE CLIENTE NO DEFINIDO");
                        }
                        AfiliadoSuma afiliado = new AfiliadoSuma();
                        //ENTIDAD Affiliate 
                        afiliado.id = 0;
                        afiliado.customerid = 0;
                        afiliado.docnumber = clienteViejo.CLIENTE.TIPO_DOCUMENTO + "-" + clienteViejo.CLIENTE.NRO_DOCUMENTO;
                        afiliado.storeid = clienteViejo.CLIENTE.COD_SUCURSAL.Value;
                        afiliado.channelid = 1;
                        afiliado.typedelivery = "0";
                        afiliado.sumastatusid = 2;
                        //ENTIDAD CLIENTE
                        afiliado.nationality = clienteViejo.CLIENTE.NACIONALIDAD;
                        afiliado.name = clienteViejo.CLIENTE.NOMBRE_CLIENTE1;
                        afiliado.name2 = clienteViejo.CLIENTE.NOMBRE_CLIENTE2;
                        afiliado.lastname1 = clienteViejo.CLIENTE.APELLIDO_CLIENTE1;
                        afiliado.lastname2 = clienteViejo.CLIENTE.APELLIDO_CLIENTE2;
                        afiliado.gender = clienteViejo.CLIENTE.SEXO;
                        afiliado.maritalstatus = clienteViejo.CLIENTE.EDO_CIVIL;
                        afiliado.occupation = clienteViejo.CLIENTE.OCUPACION;
                        afiliado.phone1 = clienteViejo.CLIENTE.TELEFONO_HAB;
                        afiliado.phone2 = clienteViejo.CLIENTE.TELEFONO_OFIC;
                        afiliado.phone3 = clienteViejo.CLIENTE.TELEFONO_CEL;
                        afiliado.email = clienteViejo.CLIENTE.E_MAIL;
                        afiliado.cod_estado = clienteViejo.CLIENTE.COD_ESTADO;
                        afiliado.cod_ciudad = clienteViejo.CLIENTE.COD_CIUDAD;
                        afiliado.cod_municipio = clienteViejo.CLIENTE.COD_MUNICIPIO;
                        afiliado.cod_parroquia = clienteViejo.CLIENTE.COD_PARROQUIA;
                        afiliado.cod_urbanizacion = clienteViejo.CLIENTE.COD_URBANIZACION;
                        if (clienteViejo.CLIENTE.FECHA_NACIMIENTO == null)
                        {
                            afiliado.birthdate = null;
                        }
                        else if (clienteViejo.CLIENTE.FECHA_NACIMIENTO.Value.ToString("dd/MM/yyyy") == "01/01/1900")
                        {
                            afiliado.birthdate = null;
                        }
                        else
                        {
                            afiliado.birthdate = clienteViejo.CLIENTE.FECHA_NACIMIENTO.Value.ToString("dd/MM/yyyy");
                        }
                        afiliado.fechaAfiliacion = clienteViejo.CLIENTE.FECHA_CREACION.Value;
                        //ENTIDAD SumaStatuses
                        afiliado.estatus = "Nuevo";
                        //ENTIDAD Type
                        if (tipoafiliacion == "Suma")
                        {
                            afiliado.typeid = 1;
                            afiliado.type = tipoafiliacion;
                        }
                        else
                        {
                            afiliado.typeid = 2;
                            afiliado.type = tipoafiliacion;
                        }
                        //INSERTAR TABLA CLIENTE
                        SaveCLIENTE(afiliado);
                        afiliado.pan = c.pan;
                        afiliado.printed = c.printed == null ? null : c.printed.Substring(6, 2) + "/" + c.printed.Substring(4, 2) + "/" + c.printed.Substring(0, 4);
                        afiliado.estatustarjeta = "Activa";
                        //INSERTAR TABLA TARJETAS
                        c.trackII = Tarjeta.ConstruirTrackII(c.pan);
                        c.cvv2 = "123";
                        SaveTARJETA(afiliado, c.trackII, c.cvv2);
                        AfiliadosSumaMigrados.Add(afiliado);
                        Migrados++;
                    }
                    //Si hay mas de una fila, se coloca en la lista de malos. Continuar con siguiente             
                    else if (ocurrencias > 1)
                    {
                        throw new Exception("NUMERO DE DOCUMENTO DUPLICADO");
                    }
                    //Si no hay fila, se coloca en la lista de malos. Continuar con siguiente 
                    else
                    {
                        throw new Exception("DOCUMENTO NO ENCONTRADO");
                    }
                }
                //Si sucede algún error, incluir en la lista de malos y continuar con siguiente
                catch (Exception ex)
                {
                    if (clienteViejo.CLIENTE == null)
                    {
                        clienteViejo.CLIENTE = new CLIENTE()
                        {
                            TIPO_DOCUMENTO = "",
                            NRO_DOCUMENTO = c.cedula
                        };
                    }
                    clienteViejo.MensajeError = ex.Message;
                    AfiliacionesClientesError.Add(clienteViejo);
                    Errores++;
                }
            }
            lblTotalBuenos.Text = Migrados.ToString();
            lblTotalMalos.Text = Errores.ToString();
            DateTime fin = DateTime.Now;
            lblFin.Text = fin.ToString();
            lblTotal.Text = Math.Round((fin - inicio).TotalMinutes, 2).ToString() + " minutos";
            this.Refresh();
            //Genero los archivos de texto con el resultado de la corrida
            string directory = System.AppDomain.CurrentDomain.BaseDirectory + "Errores" + @"\";
            if (System.IO.Directory.Exists(directory) == false)
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            StreamWriter myWriter = new StreamWriter(directory + "Migrados.txt", false);
            myWriter.WriteLine("Carga Masiva Suma, Fase 1.");
            myWriter.WriteLine("Inicio del proceso " + inicio);
            myWriter.WriteLine("Fin del proceso " + fin);
            foreach (AfiliadoSuma a in AfiliadosSumaMigrados)
            {
                myWriter.WriteLine(a.docnumber + "@" + a.type);
            }
            myWriter.Close();
            myWriter = new StreamWriter(directory + "Errores.txt", false);
            myWriter.WriteLine("Errores en Carga Masiva Suma, Fase 1.");
            foreach (AFILIACION_CLIENTE q in AfiliacionesClientesError)
            {
                myWriter.WriteLine(q.CLIENTE.TIPO_DOCUMENTO + "@" + q.CLIENTE.NRO_DOCUMENTO + "@" + q.COD_TIPO_CLIENTE + "@" + q.MensajeError.Replace(System.Environment.NewLine, " "));
            }
            myWriter.Close();
            lstBuenos.Items.Add("FINALIZÓ FASE I - ARCHIVOS DE LOG CREADOS");
            lstBuenos.Items.Add("CLIENTES Y TARJETAS MIGRADOS: " + Migrados.ToString());
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            btnFase2.Enabled = true;
        }

        private void btnFase2_Click(object sender, EventArgs e)
        {           
            btnProcesar.Enabled = false;
            btnFase2.Enabled = false;
            btnFase3.Enabled = false;
            DateTime inicio = DateTime.Now;
            lblInicio.Text = inicio.ToString();
            //ETAPA A - CARGAR CLIENTES PREPAGO
            int Migrados = 0;
            int Errores = 0;
            lstBuenos.Items.Clear();
            lstBuenos.Items.Add("INICIANDO ETAPA A - MIGRACIÓN DE CLIENTES PREPAGO");
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            lblTotalBuenos.Text = Migrados.ToString();
            lblTotalMalos.Text = Errores.ToString();
            this.Refresh();
            using (CardsEntities dbCards = new CardsEntities())
            {
                Corporaciones = dbCards.Corporations.OrderBy(c => c.nname).ToList();
                using (SumaLealtadEntities db = new SumaLealtadEntities())
                {
                    Regex RegExPattern2 = new Regex(@"^([VvEeJjGg]){1}(\d){3,10}$");
                    Regex RegExPattern4 = new Regex(@"^([Pp]){1}([A-Za-z0-9]){3,10}$");
                    foreach (Corporation c in Corporaciones)
                    {
                        try
                        {
                            PrepaidCustomer ClientePrepago = new PrepaidCustomer();
                            ClientePrepago.id = ClientePrepagoID();
                            ClientePrepago.alias = c.corporationID.ToString();
                            if (c.nname == "Corporacion Plazas")
                            {
                                ClientePrepago.name = "Automercados Plaza's";
                            }
                            else
                            {
                                ClientePrepago.name = c.nname;
                            }
                            ClientePrepago.phone = c.phone;
                            if (c.rif == null)
                            {
                                ClientePrepago.rif = null;
                            }
                            else
                            {
                                ClientePrepago.rif = c.rif.Replace("-", "");
                                Match match2 = RegExPattern2.Match(ClientePrepago.rif);
                                Match match4 = RegExPattern4.Match(ClientePrepago.rif);
                                if (!match2.Success && !match4.Success)
                                {
                                    ClientePrepago.rif = null;
                                }
                                else
                                {
                                    ClientePrepago.rif = ClientePrepago.rif.Substring(0, 1).ToUpper() + "-" + ClientePrepago.rif.Substring(1).ToUpper();
                                }
                            }
                            ClientePrepago.email = null;
                            ClientePrepago.address = c.address;
                            ClientePrepago.creationdate = DateTime.Now;
                            ClientePrepago.userid = 11;
                            db.PrepaidCustomers.Add(ClientePrepago);
                            db.SaveChanges();
                            Migrados++;
                        }
                        catch (Exception ex)
                        {
                            Errores++;
                            lstMalos.Items.Add("ERROR: " + c.nname + " " + ex.Message);
                            lstMalos.SelectedIndex = lstMalos.Items.Count - 1;
                        }
                    }
                }
            }
            lblTotalBuenos.Text = Migrados.ToString();
            lblTotalMalos.Text = Errores.ToString();
            lstBuenos.Items.Add("FINALIZADA ETAPA A - CLIENTES PREPAGO MIGRADOS: " + Migrados.ToString());
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            DateTime fin = DateTime.Now;
            lblFin.Text = fin.ToString();
            lblTotal.Text = Math.Round((fin - inicio).TotalMinutes, 2).ToString() + " minutos";
            this.Refresh();
            //ETAPA B - CREAR AFILIACIONES SUMA Y PREPAGO            
            //Leer clientes y afiliacion_cliente desde SumaLealtad - SQL2000 - OdbcDataReader
            OdbcConnection DbConnection = new OdbcConnection("Driver={SQL Server};Server=172.20.1.24;Database=SumaLealtad;Uid=UserSisLeal;Pwd=1234;");
            DbConnection.Open();
            OdbcCommand DbCommand = DbConnection.CreateCommand();
            DbCommand.CommandText = "SELECT a.[TIPO_DOCUMENTO],a.[NRO_DOCUMENTO],a.[NACIONALIDAD],a.[NOMBRE_CLIENTE1],a.[NOMBRE_CLIENTE2],a.[APELLIDO_CLIENTE1],a.[APELLIDO_CLIENTE2],a.[FECHA_NACIMIENTO],a.[SEXO],a.[EDO_CIVIL],a.[OCUPACION],a.[TELEFONO_HAB],a.[TELEFONO_OFIC],a.[TELEFONO_CEL],a.[E_MAIL],a.[COD_SUCURSAL],a.[COD_PAIS],a.[COD_ESTADO],a.[COD_CIUDAD],a.[COD_MUNICIPIO],a.[COD_PARROQUIA],a.[COD_URBANIZACION],a.[FECHA_CREACION], b.[COD_TIPO_CLIENTE] FROM CLIENTE a, AFILIACION_CLIENTE b WHERE a.[TIPO_DOCUMENTO] <> '' AND a.[TIPO_DOCUMENTO] = b.[TIPO_DOCUMENTO] AND a.[NRO_DOCUMENTO] = b.[NRO_DOCUMENTO] AND b.[ESTATUS_AFILIADO] = 'Activo' ORDER BY a.[NRO_DOCUMENTO]";
            OdbcDataReader DbReader = DbCommand.ExecuteReader();
            while (DbReader.Read())
            {
                AFILIACION_CLIENTE cliente = new AFILIACION_CLIENTE();
                cliente.CLIENTE = new CLIENTE();
                cliente.CLIENTE.TIPO_DOCUMENTO = DbReader.GetString(0).ToUpper().Trim();
                cliente.CLIENTE.NRO_DOCUMENTO = DbReader.IsDBNull(1) == true ? "" : DbReader.GetString(1).Trim();
                cliente.COD_TIPO_CLIENTE = DbReader.IsDBNull(23) == true ? "" : DbReader.GetString(23).Trim();
                AfiliacionesClientesSumaViejo.Add(cliente);
            }
            DbReader.Close();
            DbCommand.Dispose();
            DbConnection.Close();
            lblClientes.Text = AfiliacionesClientesSumaViejo.Count.ToString() + " documentos de identificación en 172.20.1.24/SumaLealtad/CLIENTE";
            lstBuenos.Items.Add("INICIANDO ETAPA B - AFILIACIONES SUMA Y PREPAGO");
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            lblClientes.Refresh();
            Migrados = 0;
            Errores = 0;
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                ClientesMigrados = db.CLIENTES.OrderBy(x => x.NRO_DOCUMENTO).ToList();
                foreach (CLIENTE c in ClientesMigrados)
                {
                    try
                    {
                        if ((Migrados + Errores) % 300 == 0)
                        {
                            lblTotalBuenos.Text = Migrados.ToString();
                            lblTotalMalos.Text = Errores.ToString();
                            this.Refresh();
                        }
                        AFILIACION_CLIENTE ac = AfiliacionesClientesSumaViejo.Find(x => x.CLIENTE.TIPO_DOCUMENTO.Equals(c.TIPO_DOCUMENTO) && x.CLIENTE.NRO_DOCUMENTO.Equals(c.NRO_DOCUMENTO));
                        if (ac == null)
                        {
                            //Si no hay fila, se coloca en la lista de malos. Continuar con siguiente 
                            throw new Exception("NO ENCONTRADO EN AFILIACION_CLIENTE");
                        }
                        else if (ac.COD_TIPO_CLIENTE == "1")
                        {
                            AfiliadoSuma afiliado = new AfiliadoSuma();
                            afiliado.docnumber = c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO;
                            afiliado.storeid = c.COD_SUCURSAL.Value;
                            afiliado.typeid = 1;
                            afiliado.fechaAfiliacion = c.FECHA_CREACION.Value;
                            afiliado.id = Save(afiliado);
                            AfiliadosSumaMigrados.Add(afiliado);
                            Migrados++;
                        }
                        else if (ac.COD_TIPO_CLIENTE == "4")
                        {
                            AfiliadoSuma afiliado = new AfiliadoSuma();
                            afiliado.docnumber = c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO;
                            afiliado.storeid = c.COD_SUCURSAL.Value;
                            afiliado.typeid = 2;
                            afiliado.fechaAfiliacion = c.FECHA_CREACION.Value;
                            afiliado.id = Save(afiliado);
                            AfiliadosPrepagoMigrados.Add(afiliado);
                            Migrados++;
                        }
                        else
                        {
                            //Si no se determina tipo de cliente, se coloca en la lista de malos. Continuar con siguiente 
                            throw new Exception("NO SE PUDO DETERMINAR TIPO DE AFILIACION");
                        }
                    }
                    catch (Exception ex)
                    {
                        AFILIACION_CLIENTE clienteerror = new AFILIACION_CLIENTE()
                        {
                            CLIENTE = c,
                            COD_TIPO_CLIENTE = "1",
                            MensajeError = ex.Message
                        };
                        AfiliacionesClientesError.Add(clienteerror);
                        Errores++;
                    }
                }
            }
            lblTotalBuenos.Text = Migrados.ToString();
            lblTotalMalos.Text = Errores.ToString();
            fin = DateTime.Now;
            lblFin.Text = fin.ToString();
            lblTotal.Text = Math.Round((fin - inicio).TotalMinutes, 2).ToString() + " minutos";
            this.Refresh();
            //ETAPA B - BORRAR CLIENTES SIN AFILIACIONES
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM PrepaidCustomer WHERE id NOT IN (SELECT DISTINCT prepaidcustomerid FROM PrepaidBeneficiary)");
            }
            //Genero los archivos de texto con el resultado de la corrida
            string directory = System.AppDomain.CurrentDomain.BaseDirectory + "Errores" + @"\";
            if (System.IO.Directory.Exists(directory) == false)
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            StreamWriter myWriter = new StreamWriter(directory + "AfiliacionesSumaFaseII-etapaB.txt", false);
            myWriter.WriteLine("Carga Masiva Suma, Fase 2 Etapa B - Afiliaciones Suma.");
            myWriter.WriteLine("Inicio del proceso " + inicio);
            myWriter.WriteLine("Fin del proceso " + fin);
            foreach (AfiliadoSuma a in AfiliadosSumaMigrados)
            {
                myWriter.WriteLine(a.docnumber + "@" + a.type);
            }
            myWriter.Close();
            myWriter = new StreamWriter(directory + "AfiliacionesPrepagoFaseII-etapaB.txt", false);
            myWriter.WriteLine("Carga Masiva Suma, Fase 2 Etapa B - Afiliaciones Prepago.");
            myWriter.WriteLine("Inicio del proceso " + inicio);
            myWriter.WriteLine("Fin del proceso " + fin);
            foreach (AfiliadoSuma a in AfiliadosPrepagoMigrados)
            {
                myWriter.WriteLine(a.docnumber + "@" + a.type);
            }
            myWriter.Close();
            myWriter = new StreamWriter(directory + "ErroresFaseII-etapaB.txt", false);
            myWriter.WriteLine("Errores en Carga Masiva Suma, Fase 2 Etapa B.");
            foreach (AFILIACION_CLIENTE q in AfiliacionesClientesError)
            {
                myWriter.WriteLine(q.CLIENTE.TIPO_DOCUMENTO + "@" + q.CLIENTE.NRO_DOCUMENTO + "@" + q.COD_TIPO_CLIENTE + "@" + q.MensajeError.Replace(System.Environment.NewLine, " "));
            }
            myWriter.Close();
            lstBuenos.Items.Add("FINALIZADA ESTAPA B - ARCHIVOS DE LOG CREADOS");
            lstBuenos.Items.Add("AFLIADOS SUMA MIGRADOS: " + AfiliadosSumaMigrados.Count.ToString());
            lstBuenos.Items.Add("BENEFICIARIOS PREPAGO MIGRADOS: " + AfiliadosPrepagoMigrados.Count.ToString());
            lstBuenos.SelectedIndex = lstBuenos.Items.Count - 1;
            btnFase3.Enabled = true;
        }

        private void btnFase3_Click(object sender, EventArgs e)
        {
            btnProcesar.Enabled = false;
            btnFase2.Enabled = false;
            btnFase3.Enabled = false;
            DateTime inicio = DateTime.Now;
            lblInicio.Text = inicio.ToString();
            lstBuenos.Items.Clear();

            ////Bucar datos del Cliente en Web Plazas
            ////Primero se buscan los datos de CLIENTE en WebPlazas
            ////SERVICIO WSL.WebPlazas.getClientByNumDoc 
            //string clienteWebPlazasJson = WSL.WebPlazas.getClientByNumDoc(afiliado.docnumber);
            //if (WSL.WebPlazas.ExceptionServicioWebPlazas(clienteWebPlazasJson))
            //{
            //    throw new Exception("Error en Llamada WSL.WebPlazas.getClientByNumDoc " + afiliado.docnumber);
            //}
            //ClienteWebPlazas clienteWebPlazas = (ClienteWebPlazas)JsonConvert.DeserializeObject<ClienteWebPlazas>(clienteWebPlazasJson);
            //if (clienteWebPlazas == null)
            //{
            //    //No está en WebPlazas
            //    afiliado.clientid = 0;
            //}
            //else
            //{
            //    //Si está en la WebPlazas
            //    afiliado.nationality = clienteWebPlazas.nationality.Replace("/", "").Replace("\\", "");
            //    afiliado.name = clienteWebPlazas.name.Replace("/", "").Replace("\\", "");
            //    afiliado.name2 = clienteWebPlazas.name2.Replace("/", "").Replace("\\", "");
            //    afiliado.lastname1 = clienteWebPlazas.lastname1.Replace("/", "").Replace("\\", "");
            //    afiliado.lastname2 = clienteWebPlazas.lastname2.Replace("/", "").Replace("\\", "");
            //    afiliado.birthdate = clienteWebPlazas.birthdate.Value.ToString("dd/MM/yyyy");
            //    afiliado.gender = clienteWebPlazas.gender.Replace("/", "").Replace("\\", "");
            //    afiliado.clientid = clienteWebPlazas.id;
            //    afiliado.maritalstatus = clienteWebPlazas.maritalstatus.Replace("/", "").Replace("\\", "");
            //    afiliado.occupation = clienteWebPlazas.occupation.Replace("/", "").Replace("\\", "");
            //    afiliado.phone1 = clienteWebPlazas.phone1.Replace("/", "").Replace("\\", "");
            //    afiliado.phone2 = clienteWebPlazas.phone2.Replace("/", "").Replace("\\", "");
            //    afiliado.phone3 = clienteWebPlazas.phone3.Replace("/", "").Replace("\\", "");
            //    afiliado.email = clienteWebPlazas.email.Replace("/", "").Replace("\\", "");
            //    afiliado.WebType = clienteWebPlazas.type;
            //}
            //Buscar imagen del documento del Cliente en copia de carpeta cedulas desde 172.20.1.21, comprimir imgen a menos de 50kb
            //Primero determinar nombre de archivo desde tabla Afiliacion_Cliente
            //DbCommand = DbConnection.CreateCommand();
            //DbCommand.CommandText = "SELECT a.TIPO_DOCUMENTO,a.NRO_DOCUMENTO,a.COD_FOTO,LTRIM(RTRIM(b.NOMBRE)),b.TAMANO,CONVERT(CHAR,b.FECHA_CREACION,120) FROM AFILIACION_CLIENTE a, FOTO b WHERE a.TIPO_DOCUMENTO = '" + clienteViejo.TIPO_DOCUMENTO + "' AND a.NRO_DOCUMENTO = '" + clienteViejo.NRO_DOCUMENTO + "' AND a.COD_FOTO =  b.CODIGO AND a.COD_FOTO <> 0 AND b.TAMANO > 0 ORDER BY a.TIPO_DOCUMENTO, a.NRO_DOCUMENTO";
            //DbReader = DbCommand.ExecuteReader();
            //string NombreArchivo = "";
            //int filas2 = 0;
            //while (DbReader.Read())
            //{
            //    filas2++;
            //    NombreArchivo = DbReader.GetString(3);
            //}
            //DbReader.Close();
            //DbCommand.Dispose();
            //if (filas2 == 0)
            //{
            //    throw new Exception("NO TIENE IMAGEN REGISTRADA");
            //}
            //if (filas2 > 1)
            //{
            //    throw new Exception("TIENE MAS DE UNA IMAGEN REGISTRADA");
            //}
            //string FilePath = ObtenerImagen(NombreArchivo);
            //byte[] imageData = ReadFile(FilePath);

            ////Insertar Cliente, tablas Affiliate, CLIENTE, Photos_Affiliate
            //int id = 0;
            //id = Save(afiliado, imageData);

            MessageBox.Show("Proceso Finalizado. Archivos de Log creados.");
        }

        private string ObtenerImagen(string NombreArchivo)
        {
            string FilePath = "C:\\Users\\mromani\\Desktop\\cedulas\\" + NombreArchivo;
            if (!File.Exists(FilePath))
            {
                FilePath = "C:\\Users\\mromani\\Desktop\\cedulas\\" + NombreArchivo.Replace(".jpg", "_.jpg");
            }
            string TempPath = "C:\\Users\\mromani\\Desktop\\cedulas\\Temp.jpg";
            long fileLength = new FileInfo(FilePath).Length;
            //MessageBox.Show("tamaño anterior: " + fileLength);
            //Si la imagen escaneada es mayor 50Kb, se itera reduciendo la imagen a la mitad de su tamaño
            while (fileLength > 51200)
            {
                if (File.Exists(FilePath))
                {
                    File.Copy(FilePath, TempPath);
                    File.Delete(FilePath);
                }
                using (Image oldImage = Image.FromFile(TempPath))
                {
                    int w = oldImage.Width / 2;
                    int h = oldImage.Height / 2;
                    Image thumb = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Graphics oGraphic = Graphics.FromImage(thumb);
                    oGraphic.CompositingQuality = CompositingQuality.HighSpeed;
                    oGraphic.SmoothingMode = SmoothingMode.HighSpeed;
                    oGraphic.InterpolationMode = InterpolationMode.Low;
                    Rectangle rect = new Rectangle(0, 0, w, h);
                    oGraphic.DrawImage(oldImage, rect);
                    thumb.Save(FilePath, ImageFormat.Jpeg);
                }
                if (File.Exists(TempPath))
                {
                    File.Delete(TempPath);
                }
                fileLength = new FileInfo(FilePath).Length;
                //MessageBox.Show("tamaño nuevo: " + fileLength);
            }
            return FilePath;
        }

        //Open file in to a filestream and read data in a byte array.
        private byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;
            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);
            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        private void SaveCLIENTE(AfiliadoSuma afiliado)
        {
            //REGLAS DE EQUIVALENCIA PARA TRANSFORMACION DE LAS COLUMNAS

            //PERSONA NATURAL
            if (afiliado.docnumber.Substring(0, 1).ToUpper() != "J" && afiliado.docnumber.Substring(0, 1).ToUpper() != "G")
            {
                //NACIONALIDAD => NINGUNA = "0", VENEZOLANO = "1", EXTRANJERO = "2"
                if (afiliado.docnumber.Substring(0, 1).ToUpper() == "V")
                {
                    afiliado.nationality = "1";
                }
                else if (afiliado.docnumber.Substring(0, 1).ToUpper() == "E" || afiliado.docnumber.Substring(0, 1).ToUpper() == "P")
                {
                    afiliado.nationality = "2";
                }
                else
                {
                    afiliado.nationality = "0";
                }

                //GENERO => NINGUNO = "0", MASCULINO = "1", FEMENINO = "2"
                if (afiliado.gender.ToUpper() == "M")
                {
                    afiliado.gender = "1";
                }
                else if (afiliado.gender.ToUpper() == "F")
                {
                    afiliado.gender = "2";
                }
                else
                {
                    afiliado.gender = "0";
                }

                //ESTADO CIVIL => NINGUNO = "0", SOLTERO = "1", CASADO = "2", DIVORCIADO = "3", VIUDO = "4"
                if (afiliado.maritalstatus.ToUpper() == "S")
                {
                    afiliado.maritalstatus = "1";
                }
                else if (afiliado.maritalstatus.ToUpper() == "C")
                {
                    afiliado.maritalstatus = "2";
                }
                else if (afiliado.maritalstatus.ToUpper() == "D")
                {
                    afiliado.maritalstatus = "3";
                }
                else if (afiliado.maritalstatus.ToUpper() == "V")
                {
                    afiliado.maritalstatus = "4";
                }
                else
                {
                    afiliado.maritalstatus = "0";
                }
            }

            //PERSONA JURIDICA => NACIONALIDAD = "0", SEXO = "0", EDO_CIVIL = "0", FECHA_NACIMIENTO = NULL, OCUPACION = "", UN SOLO NOMBRE.
            if (afiliado.docnumber.Substring(0, 1).ToUpper() == "J" || afiliado.docnumber.Substring(0, 1).ToUpper() == "G")
            {
                if (afiliado.name == afiliado.lastname1)
                {
                    afiliado.name2 = "";
                    afiliado.lastname1 = "";
                    afiliado.lastname2 = "";
                }
                else
                {
                    afiliado.name = afiliado.name + " (" + afiliado.lastname1 + ")";
                    afiliado.name2 = "";
                    afiliado.lastname1 = "";
                    afiliado.lastname2 = "";
                }
                afiliado.nationality = "0";
                afiliado.gender = "0";
                afiliado.maritalstatus = "0";
                afiliado.birthdate = null;
                afiliado.occupation = "";
            }

            //DEBE TENER AL MENOS UN TELEFONO EN EL CAMPO PHONE1
            if (afiliado.phone1 == "")
            {
                if (afiliado.phone2 != "")
                {
                    afiliado.phone1 = afiliado.phone2;
                    afiliado.phone2 = "";
                }
                else if (afiliado.phone3 != "")
                {
                    afiliado.phone1 = afiliado.phone3;
                    afiliado.phone3 = "";
                }
                else
                {
                    afiliado.phone1 = "9031411";
                }
            }

            //SE CAMBIA AL FORMATO SAP DE NUMERO DE SUCURSAL
            afiliado.storeid = afiliado.storeid + 1000;

            //se guardan los datos en las entidades, creationuserid = 5, modifieduserid = 5, statusid = 2
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //ENTIDAD CLIENTE
                CLIENTE CLIENTE = new CLIENTE();
                CLIENTE.TIPO_DOCUMENTO = afiliado.docnumber.Substring(0, 1);
                CLIENTE.NRO_DOCUMENTO = afiliado.docnumber.Substring(2);
                CLIENTE.E_MAIL = afiliado.email == "&NBSP;" ? "" : afiliado.email;
                CLIENTE.NACIONALIDAD = afiliado.nationality == "&NBSP;" ? "" : afiliado.nationality;
                CLIENTE.NOMBRE_CLIENTE1 = afiliado.name == "&NBSP;" ? "" : afiliado.name;
                CLIENTE.NOMBRE_CLIENTE2 = afiliado.name2 == "&NBSP;" ? "" : afiliado.name2;
                CLIENTE.APELLIDO_CLIENTE1 = afiliado.lastname1 == "&NBSP;" ? "" : afiliado.lastname1;
                CLIENTE.APELLIDO_CLIENTE2 = afiliado.lastname2 == "&NBSP;" ? "" : afiliado.lastname2;
                CLIENTE.FECHA_NACIMIENTO = afiliado.birthdate == null ? new DateTime?() : DateTime.ParseExact(afiliado.birthdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                CLIENTE.SEXO = afiliado.gender == "&NBSP;" ? "" : afiliado.gender;
                CLIENTE.EDO_CIVIL = afiliado.maritalstatus == "&NBSP;" ? "" : afiliado.maritalstatus;
                CLIENTE.OCUPACION = afiliado.occupation == "&NBSP;" ? "" : afiliado.occupation;
                CLIENTE.TELEFONO_HAB = afiliado.phone1;
                CLIENTE.TELEFONO_OFIC = afiliado.phone2;
                CLIENTE.TELEFONO_CEL = afiliado.phone3;
                CLIENTE.COD_SUCURSAL = afiliado.storeid == 0 ? Convert.ToInt32(null) : afiliado.storeid;
                CLIENTE.COD_ESTADO = afiliado.cod_estado;
                CLIENTE.COD_CIUDAD = afiliado.cod_ciudad;
                CLIENTE.COD_MUNICIPIO = afiliado.cod_municipio;
                CLIENTE.COD_PARROQUIA = afiliado.cod_parroquia;
                CLIENTE.COD_URBANIZACION = afiliado.cod_urbanizacion;
                CLIENTE.FECHA_CREACION = afiliado.fechaAfiliacion;
                db.CLIENTES.Add(CLIENTE);
                db.SaveChanges();
            }
        }

        //private AfiliadoSuma BuscarTARJETA(AfiliadoSuma afiliado)
        //{
        //    //string RespuestaCardsJson = WSL.Cards.addClient(afiliado.docnumber.Substring(2), (afiliado.name + " " + afiliado.lastname1).ToUpper(), afiliado.phone1, "Plazas Baruta");
        //    //if (WSL.Cards.ExceptionServicioCards(RespuestaCardsJson))
        //    //{
        //    //    return false;
        //    //}
        //    //RespuestaCards RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
        //    //using (LealtadEntities db = new LealtadEntities())
        //    //{
        //    //    if (RespuestaCards.excode == "0" || RespuestaCards.excode == "7")
        //    //    {
        //    //Se buscan los datos de Tarjeta del AFILIADO en Cards
        //    //SERVICIO WSL.Cards.getClient !
        //    string clienteCardsJson = WSL.Cards.getClient(afiliado.docnumber.Substring(2));
        //    if (WSL.Cards.ExceptionServicioCards(clienteCardsJson))
        //    {
        //        //return false;
        //        throw new Exception("NO SE PUDO OBTENER TARJETA");
        //    }
        //    ClienteCards clienteCards = (ClienteCards)JsonConvert.DeserializeObject<ClienteCards>(clienteCardsJson);
        //    afiliado.pan = clienteCards.pan;
        //    afiliado.printed = clienteCards.printed == null ? null : clienteCards.printed.Substring(6, 2) + "/" + clienteCards.printed.Substring(4, 2) + "/" + clienteCards.printed.Substring(0, 4);
        //    afiliado.estatustarjeta = clienteCards.tarjeta;
        //    return afiliado;
        //    //  afiliado.statusid = 4;
        //    //return SaveChanges(afiliado);
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //    //}
        //}

        private bool SaveTARJETA(AfiliadoSuma afiliado, string trackII, string cvv2)
        {
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //Entida: TARJETA
                //Decimal pan = Convert.ToDecimal(afiliado.pan);
                //TARJETA tarjeta = db.TARJETAS.FirstOrDefault(t => t.NRO_TARJETA.Equals(pan));
                //if (tarjeta != null)
                //{
                //    tarjeta.ESTATUS_TARJETA = afiliado.estatustarjeta;
                //    tarjeta.COD_USUARIO = 5;
                //    tarjeta.FECHA_CREACION = afiliado.printed == null ? new DateTime?() : DateTime.ParseExact(afiliado.printed, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //}
                //else if (afiliado.pan != null && afiliado.estatustarjeta != null)
                //{
                TARJETA tarjeta = new TARJETA()
                {
                    NRO_TARJETA = Convert.ToDecimal(afiliado.pan),
                    NRO_AFILIACION = afiliado.id,
                    TIPO_DOCUMENTO = afiliado.docnumber.Substring(0, 1),
                    NRO_DOCUMENTO = afiliado.docnumber.Substring(2),
                    ESTATUS_TARJETA = afiliado.estatustarjeta,
                    SALDO_PUNTOS = null,
                    OBSERVACIONES = null,
                    COD_USUARIO = 5,
                    TRACK1 = null,
                    TRACK2 = trackII,
                    CVV2 = cvv2,
                    FECHA_CREACION = afiliado.printed == null ? new DateTime?() : DateTime.ParseExact(afiliado.printed, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                };
                db.TARJETAS.Add(tarjeta);
                //}
                db.SaveChanges();
                return true;
            }
        }

        private int AfilliatesID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.Affiliates.Count() == 0)
                    return 1;
                return (db.Affiliates.Max(a => a.id) + 1);
            }
        }

        private int AfilliateAudID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.AffiliateAuds.Count() == 0)
                    return 1;
                return (db.AffiliateAuds.Max(a => a.id) + 1);
            }
        }

        private int ClientePrepagoID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.PrepaidCustomers.Count() == 0)
                    return 1;
                return (db.PrepaidCustomers.Max(c => c.id) + 1);
            }
        }

        private int Save(AfiliadoSuma afiliado, byte[] imageData = null)
        {
            //se guardan los datos en las entidades, creationuserid = 5, modifieduserid = 5, statusid = 4
            using (SumaLealtadEntities db = new SumaLealtadEntities())
            {
                //ENTIDAD Affiliatte                   
                Affiliate Affiliate = new Affiliate()
                {
                    id = AfilliatesID(),
                    customerid = 0,
                    docnumber = afiliado.docnumber,
                    clientid = 0,
                    storeid = afiliado.storeid,
                    channelid = 1,
                    typeid = afiliado.typeid,
                    affiliatedate = afiliado.fechaAfiliacion,
                    typedelivery = "0",
                    storeiddelivery = null,
                    estimateddatedelivery = new DateTime(),
                    creationdate = DateTime.Now,
                    creationuserid = 5,
                    modifieddate = DateTime.Now,
                    modifieduserid = 5,
                    sumastatusid = 4,
                    reasonsid = null,
                    twitter_account = null,
                    facebook_account = null,
                    instagram_account = null,
                    comments = null
                };
                db.Affiliates.Add(Affiliate);
                //ENTIDAD AffiliateAud
                AffiliateAud affiliateauditoria = new AffiliateAud()
                {
                    id = AfilliateAudID(),
                    affiliateid = Affiliate.id,
                    modifieduserid = 5,
                    modifieddate = DateTime.Now,
                    statusid = 4,
                    reasonsid = 1,
                    comments = null
                };
                db.AffiliateAuds.Add(affiliateauditoria);
                //ENTIDAD TARJETA
                TARJETA tarjeta = db.TARJETAS.Single(t => t.TIPO_DOCUMENTO.Equals(afiliado.docnumber.Substring(0, 1)) && t.NRO_DOCUMENTO.Equals(afiliado.docnumber.Substring(2)));
                if (tarjeta != null)
                {
                    tarjeta.NRO_AFILIACION = Affiliate.id;
                }
                if (afiliado.typeid == 2)
                {
                    string codconsorcio;
                    string docnumber = afiliado.docnumber.Substring(2);
                    //Buscar consorcio
                    using (CardsEntities dbCards = new CardsEntities())
                    {
                        codconsorcio = (from p in dbCards.Clients
                                        where p.CIDClient.Equals(docnumber)
                                        select p.Corporations.FirstOrDefault().corporationID).First().ToString();
                    }
                    //crear registro prepaid beneficiary
                    PrepaidBeneficiary prepaidbeneficiary = new PrepaidBeneficiary();
                    prepaidbeneficiary.affiliateid = Affiliate.id;
                    prepaidbeneficiary.prepaidcustomerid = db.PrepaidCustomers.First(x => x.alias.Equals(codconsorcio)).id;
                    prepaidbeneficiary.begindate = DateTime.Now;
                    prepaidbeneficiary.active = true;
                    db.PrepaidBeneficiaries.Add(prepaidbeneficiary);
                }
                db.SaveChanges();
                return Affiliate.id;
            }
        }

        //private bool SaveChanges(AfiliadoSuma afiliado)
        //{
        //    using (SumaLealtadEntities db = new SumaLealtadEntities())
        //    {
        //        // Entidad: Affiliate
        //        Affiliate affiliate = db.Affiliates.FirstOrDefault(a => a.id == afiliado.id);
        //        if (affiliate != null)
        //        {
        //            affiliate.statusid = afiliado.statusid;
        //        }
        //        // Entida: TARJETA
        //        Decimal pan = Convert.ToDecimal(afiliado.pan);
        //        TARJETA tarjeta = db.TARJETAS.FirstOrDefault(t => t.NRO_TARJETA.Equals(pan));
        //        if (tarjeta != null)
        //        {
        //            tarjeta.ESTATUS_TARJETA = afiliado.estatustarjeta;
        //            tarjeta.COD_USUARIO = 5;
        //            tarjeta.FECHA_CREACION = afiliado.printed == null ? new DateTime?() : DateTime.ParseExact(afiliado.printed, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        }
        //        else if (afiliado.pan != null && afiliado.estatustarjeta != null)
        //        {
        //            tarjeta = new TARJETA()
        //            {
        //                NRO_TARJETA = pan,
        //                NRO_AFILIACION = afiliado.id,
        //                TIPO_DOCUMENTO = afiliado.docnumber.Substring(0, 1),
        //                NRO_DOCUMENTO = afiliado.docnumber.Substring(2),
        //                ESTATUS_TARJETA = afiliado.estatustarjeta,
        //                SALDO_PUNTOS = null,
        //                OBSERVACIONES = null,
        //                COD_USUARIO = 5,
        //                TRACK1 = null,
        //                TRACK2 = null,
        //                CVV2 = null,
        //                FECHA_CREACION = afiliado.printed == null ? new DateTime?() : DateTime.ParseExact(afiliado.printed, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        //            };
        //            db.TARJETAS.Add(tarjeta);
        //        }
        //        db.SaveChanges();
        //        return true;
        //    }
        //}

        //public AfiliadoSuma Find(int id)
        //{
        //    using (SumaLealtadEntities db = new SumaLealtadEntities())
        //    {
        //        AfiliadoSuma afiliado = (from a in db.Affiliates
        //                                 join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
        //                                 join s in db.SumaStatuses on a.statusid equals s.id
        //                                 join t in db.Types on a.typeid equals t.id
        //                                 where a.id.Equals(id)
        //                                 select new AfiliadoSuma()
        //                                 {
        //                                     //ENTIDAD Affiliate 
        //                                     id = a.id,
        //                                     customerid = a.customerid,
        //                                     docnumber = a.docnumber,
        //                                     clientid = a.clientid,
        //                                     storeid = a.storeid,
        //                                     channelid = a.channelid,
        //                                     typeid = a.typeid,
        //                                     typedelivery = a.typedelivery,
        //                                     storeiddelivery = a.storeiddelivery,
        //                                     statusid = a.statusid,
        //                                     reasonsid = a.reasonsid,
        //                                     twitter_account = a.twitter_account,
        //                                     facebook_account = a.facebook_account,
        //                                     instagram_account = a.instagram_account,
        //                                     comments = a.comments,
        //                                     //ENTIDAD CLIENTE
        //                                     nationality = c.NACIONALIDAD,
        //                                     name = c.NOMBRE_CLIENTE1,
        //                                     name2 = c.NOMBRE_CLIENTE2,
        //                                     lastname1 = c.APELLIDO_CLIENTE1,
        //                                     lastname2 = c.APELLIDO_CLIENTE2,
        //                                     gender = c.SEXO,
        //                                     maritalstatus = c.EDO_CIVIL,
        //                                     occupation = c.OCUPACION,
        //                                     phone1 = c.TELEFONO_HAB,
        //                                     phone2 = c.TELEFONO_OFIC,
        //                                     phone3 = c.TELEFONO_CEL,
        //                                     email = c.E_MAIL,
        //                                     cod_estado = c.COD_ESTADO,
        //                                     cod_ciudad = c.COD_CIUDAD,
        //                                     cod_municipio = c.COD_MUNICIPIO,
        //                                     cod_parroquia = c.COD_PARROQUIA,
        //                                     cod_urbanizacion = c.COD_URBANIZACION,
        //                                     //ENTIDAD SumaStatuses
        //                                     estatus = s.name,
        //                                     //ENTIDAD Type
        //                                     type = t.name,
        //                                 }).SingleOrDefault();
        //        if (afiliado != null)
        //        {
        //            DateTime? d = (from c in db.CLIENTES
        //                           where (c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO).Equals(afiliado.docnumber)
        //                           select c.FECHA_NACIMIENTO
        //                           ).SingleOrDefault();
        //            if (d == null)
        //            {
        //                afiliado.birthdate = null;
        //            }
        //            else
        //            {
        //                afiliado.birthdate = d.Value.ToString("dd/MM/yyyy");
        //            }
        //            //ENTIDAD TARJETA
        //            if (afiliado.estatus != "Nueva")
        //            {
        //                Decimal p = (from t in db.TARJETAS
        //                             where t.NRO_AFILIACION.Equals(afiliado.id)
        //                             select t.NRO_TARJETA
        //                             ).SingleOrDefault();
        //                if (p != 0)
        //                {
        //                    afiliado.pan = p.ToString();
        //                }
        //                else
        //                {
        //                    afiliado.pan = "";
        //                }
        //                string e = (from t in db.TARJETAS
        //                            where t.NRO_AFILIACION.Equals(afiliado.id)
        //                            select t.ESTATUS_TARJETA
        //                            ).SingleOrDefault();
        //                if (e != null)
        //                {
        //                    afiliado.estatustarjeta = e.ToString();
        //                }
        //                else
        //                {
        //                    afiliado.estatustarjeta = "";
        //                }
        //            }
        //            //POR AHORA NO HAY COLUMNA EN NINGUNA ENTIDAD PARA ALMACENAR ESTE DATO QUE VIENE DE LA WEB
        //            if (afiliado.WebType == null)
        //            {
        //                afiliado.WebType = "1";
        //            }
        //            //TEMPORAL CARGAR FECHA Y USUARIO DE AFILIACION
        //            afiliado.fechaAfiliacion = db.Affiliates.FirstOrDefault(x => x.id == afiliado.id).creationdate;
        //            afiliado.usuarioAfiliacion = (from a in db.Affiliates
        //                                          join u in db.Users on a.creationuserid equals u.id
        //                                          where a.id == afiliado.id
        //                                          select u.firstname + " " + u.lastname + "(" + u.login + ")"
        //                                          ).SingleOrDefault();
        //        }
        //        return afiliado;
        //    }
        //}

        //APROBAR => statusid = 4
        //private bool Aprobar(AfiliadoSuma afiliado)
        //{
        //    string RespuestaCardsJson = WSL.Cards.addClient(afiliado.docnumber.Substring(2), (afiliado.name + " " + afiliado.lastname1).ToUpper(), afiliado.phone1, "Plazas Baruta");
        //    if (WSL.Cards.ExceptionServicioCards(RespuestaCardsJson))
        //    {
        //        return false;
        //    }
        //    RespuestaCards RespuestaCards = (RespuestaCards)JsonConvert.DeserializeObject<RespuestaCards>(RespuestaCardsJson);
        //    using (LealtadEntities db = new LealtadEntities())
        //    {
        //        if (RespuestaCards.excode == "0" || RespuestaCards.excode == "7")
        //        {
        //            //Se buscan los datos de Tarjeta del AFILIADO en Cards
        //            //SERVICIO WSL.Cards.getClient !
        //            string clienteCardsJson = WSL.Cards.getClient(afiliado.docnumber.Substring(2));
        //            if (WSL.Cards.ExceptionServicioCards(clienteCardsJson))
        //            {
        //                return false;
        //            }
        //            ClienteCards clienteCards = (ClienteCards)JsonConvert.DeserializeObject<ClienteCards>(clienteCardsJson);
        //            afiliado.pan = clienteCards.pan;
        //            afiliado.printed = clienteCards.printed == null ? null : clienteCards.printed.Substring(6, 2) + "/" + clienteCards.printed.Substring(4, 2) + "/" + clienteCards.printed.Substring(0, 4);
        //            afiliado.estatustarjeta = clienteCards.tarjeta;
        //            afiliado.statusid = 4;
        //            return SaveChanges(afiliado);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

    }
}
