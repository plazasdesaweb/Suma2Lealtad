using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SumaPlazas.Dispositivos.Impresora
{
    public partial class MainPage : UserControl
    {
        //HtmlElement DatosNombre;
        //HtmlElement DatosApellido;
        //HtmlElement DatosTarjeta;
        //string Nombre = "";
        //string Apellido = "";
        //string Tarjeta = "";
        //ProyDataCardCP60 ProyDataCardCP60 = new ProyDataCardCP60();
        //SmartDriverDotNet SmartDriverDotNet = new SmartDriverDotNet();                

        //se escribira en este elemento Htlml "Impresa", "No impresa"
        HtmlElement ElementoHtmlControl;

        public MainPage(string idElementoHtmlControl)
        {
            ElementoHtmlControl = HtmlPage.Document.GetElementById(idElementoHtmlControl);

            if (ElementoHtmlControl != null)
            {
                //ElementoHtmlControl.SetProperty("innerHTML", "No Escaneada");
            }
            else
            {
                MessageBox.Show("Error: Elemento de control con id '" + idElementoHtmlControl + "' no encontrado.");
            }

            InitializeComponent();
            HtmlPage.RegisterScriptableObject("MainPage", this);

            //try
            //{
            //    DatosNombre = HtmlPage.Document.GetElementById("Nombre");
            //    DatosApellido = HtmlPage.Document.GetElementById("Apellido");
            //    DatosTarjeta = HtmlPage.Document.GetElementById("Tarjeta");
            //}
            //catch (Exception ex)
            //{
            //    HtmlPage.Window.Alert("Error: " + ex.Message);
            //}
        }

        //private void DetectarImpresora_Click(object sender, RoutedEventArgs e)
        //{
        //    const string TituloMessageBox = "SumaPlazas.Dispositivos.Impresora.MainPage.DetectarImpresora_Click";
        //    string ResultadoCP60 = "";
        //    string ResultadoCD800 = "";
        //    Nombre = DatosNombre.GetProperty("innerHTML").ToString();
        //    Apellido = DatosApellido.GetProperty("innerHTML").ToString();
        //    Tarjeta = DatosTarjeta.GetProperty("innerHTML").ToString();                        
        //    bool PermisosElevados = Application.Current.HasElevatedPermissions;
        //    if (PermisosElevados == true)
        //    {
        //        listBox1.Items.Add("Estableciendo comunicación con dispositivos...");
        //        ResultadoCD800 = SmartDriverDotNet.EstadoImpresora();
        //        ResultadoCP60 = ProyDataCardCP60.EstadoImpresora();
        //        listBox1.Items.Add("Impresora detectada: '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
        //        if (ResultadoCD800 != "")
        //        {
        //            listBox1.Items.Add("Estado: " + ResultadoCD800);
        //            if (ResultadoCD800.ToLower() == "la impresora está respondiendo")
        //            {
        //                listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
        //                listBox1.Items.Add("Los datos a imprimir son: " + Nombre + " " + Apellido + " " + Tarjeta);
        //            }
        //        }
        //        else
        //        {
        //            listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + SmartDriverDotNet.NombreImpresora);
        //        }
        //        listBox1.Items.Add("Impresora detectada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
        //        if (ResultadoCP60 != "")
        //        {
        //            listBox1.Items.Add("Estatus: " + ResultadoCP60);
        //            if (ResultadoCP60.ToLower() == "la impresora está respondiendo")
        //            {
        //                listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
        //                listBox1.Items.Add("Los datos a imprimir son: " + Nombre + " " + Apellido + " " + Tarjeta);
        //            }
        //        }
        //        else
        //        {
        //            listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + ProyDataCardCP60.NombreImpresora);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", TituloMessageBox, MessageBoxButton.OK);
        //    }
        //}

        [ScriptableMember]
        public string DetectarImpresora()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No impresa");
            string Impresora = "";
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Estableciendo comunicación con dispositivos...");
                SmartDriverDotNet SmartDriverDotNet = new SmartDriverDotNet();
                string ResultadoCD800 = SmartDriverDotNet.EstadoImpresora();
                listBox1.Items.Add("Impresora detectada: '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
                if (ResultadoCD800 != "")
                {
                    listBox1.Items.Add("Estado: " + ResultadoCD800);
                    if (ResultadoCD800.ToLower() == "la impresora está respondiendo")
                    {
                        listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
                        Impresora = SmartDriverDotNet.NombreImpresora;
                    }
                }
                else
                {
                    listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + SmartDriverDotNet.NombreImpresora);
                    ProyDataCardCP60 ProyDataCardCP60 = new ProyDataCardCP60();
                    string ResultadoCP60 = ProyDataCardCP60.EstadoImpresora();
                    listBox1.Items.Add("Impresora detectada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
                    if (ResultadoCP60 != "")
                    {
                        listBox1.Items.Add("Estatus: " + ResultadoCP60);
                        if (ResultadoCP60.ToLower() == "la impresora está respondiendo")
                        {
                            listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
                            Impresora = ProyDataCardCP60.NombreImpresora;
                        }
                    }
                    else
                    {
                        listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + ProyDataCardCP60.NombreImpresora);
                    }
                }                
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados.", "SumaPlazas.Dispositivos.Impresora.MainPage.DetectarImpresora", MessageBoxButton.OK);
            }
            return Impresora;           
        }

        //[ScriptableMember]
        //public void ImprimirTarjeta()
        //{
        //    const string TituloMessageBox = "SumaPlazas.Dispositivos.Impresora.MainPage.ImprimirTarjeta";
        //    //bool Resultado = false;
        //    //string Respuesta = "";
        //    Nombre = DatosNombre.GetProperty("innerHTML").ToString();
        //    Apellido = DatosApellido.GetProperty("innerHTML").ToString();
        //    Tarjeta = DatosTarjeta.GetProperty("innerHTML").ToString();
        //    string Impresora = DetectarImpresora();
        //    if (Impresora == ProyDataCardCP60.NombreImpresora)
        //    {
        //        listBox1.Items.Add("Impresora asignada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
        //        listBox1.Items.Add("Los datos a imprimir son: " + Nombre + " " + Apellido + " " + Tarjeta);
        //        //Respuesta = MessageBox.Show("¿Se imprimió correctamente la tarjeta?", "Pregunta", MessageBoxButton.);
        //        // if (Respuesta == "")
        //        // { }
        //        // else
        //        // { }
        //    }
        //    else if (Impresora == SmartDriverDotNet.NombreImpresora)
        //    {
        //        listBox1.Items.Add("Impresora asignada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
        //        listBox1.Items.Add("Los datos a imprimir son: " + Nombre + " " + Apellido + " " + Tarjeta);            
        //    }
        //    else
        //    {
        //    MessageBox.Show("Error de Aplicación: No hay impresora disponible.", TituloMessageBox, MessageBoxButton.OK);
        //    }
        //}

        //private void btnDetectarImpresora_Click(object sender, RoutedEventArgs e)
        //{
        //    ImprimirTarjeta();
        //}

    }
}
