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
        //objetos impresora
        SmartDriverDotNet smartDriverDotNet;
        ProyDataCardCP60 proyDataCardCP60;

        //parámetros para la impresión
        string nombre;
        string apellido;
        string nroTarjeta;
        string track1;
        string track2;
        string tipoTarjeta;
        string corporacion;
        string fechaVencimiento;

        //se escribira en este elemento Htlml "Impresa", "No impresa"
        HtmlElement ElementoHtmlControl;

        public MainPage(string Nombre, string Apellido, string NroTarjeta, string Track1, string Track2, string TipoTarjeta, string Corporacion, string FechaVencimiento, string idElementoHtmlControl)
        {
            nombre = Nombre;
            apellido = Apellido;
            nroTarjeta = NroTarjeta;
            track1 = Track1;
            track2 = Track2;
            tipoTarjeta = TipoTarjeta;
            corporacion = Corporacion;
            fechaVencimiento = FechaVencimiento;

            ElementoHtmlControl = HtmlPage.Document.GetElementById(idElementoHtmlControl);

            if (ElementoHtmlControl == null)
            {
                MessageBox.Show("Error: Elemento de control con id '" + idElementoHtmlControl + "' no encontrado.");
            }

            InitializeComponent();
            HtmlPage.RegisterScriptableObject("MainPage", this);
        }

        //[ScriptableMember]
        //public string DetectarImpresora()
        private string DetectarImpresora()
        {
            string Impresora = "";
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Estableciendo comunicación con dispositivos...");
                smartDriverDotNet = new SmartDriverDotNet();
                string ResultadoCD800 = smartDriverDotNet.EstadoImpresora();
                listBox1.Items.Add("Impresora detectada: '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
                if (ResultadoCD800 != "")
                {
                    listBox1.Items.Add("Estado: " + ResultadoCD800);
                    if (ResultadoCD800.ToLower() == "la impresora está respondiendo")
                    {
                        listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
                        Impresora = SmartDriverDotNet.NombreImpresora;
                    }
                    else
                    {
                        listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
                        proyDataCardCP60 = new ProyDataCardCP60();
                        string ResultadoCP60 = proyDataCardCP60.EstadoImpresora();
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
                            listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
                        }
                    }
                }
                else
                {
                    listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
                    proyDataCardCP60 = new ProyDataCardCP60();
                    string ResultadoCP60 = proyDataCardCP60.EstadoImpresora();
                    listBox1.Items.Add("Impresora detectada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
                    if (ResultadoCP60 != "")
                    {
                        listBox1.Items.Add("Estatus: " + ResultadoCP60);
                        if (ResultadoCP60.ToLower() == "la impresora está respondiendo")
                        {
                            listBox1.Items.Add("Se utilizará este dispositivo para imprimir");
                            Impresora = ProyDataCardCP60.NombreImpresora;
                        }
                        else
                        {
                            listBox1.Items.Add("No se pudo asignar dispositivo para imprimir");
                        }
                    }
                    else
                    {
                        listBox1.Items.Add("Estado: No es posible comunicarse con la impresora '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
                    }
                }
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados.", "SumaPlazas.Dispositivos.Impresora.MainPage.DetectarImpresora", MessageBoxButton.OK);
            }
            return Impresora;
        }

        [ScriptableMember]
        public void ImprimirTarjeta()
        {
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                ElementoHtmlControl.SetProperty("innerHTML", "No impresa");
                listBox1.Items.Clear();
                string Impresora = DetectarImpresora();
                if (Impresora == ProyDataCardCP60.NombreImpresora)
                {
                    listBox1.Items.Add("Impresora asignada: '" + ProyDataCardCP60.NombreImpresora + "', modelo: 'CP60'");
                    listBox1.Items.Add("Los datos a imprimir son: " + nombre + "," + apellido + "," + nroTarjeta + "," + track1 + "," + track2 + "," + tipoTarjeta + "," + corporacion + "," + fechaVencimiento);
                    bool Resultado = proyDataCardCP60.Imprimir(ref nombre, ref apellido, ref nroTarjeta, ref track1, ref track2, ref tipoTarjeta, ref corporacion, ref fechaVencimiento);
                    //La pregunta la haremos en la página web, no en el silverlight
                    //Respuesta = MessageBox.Show("¿Se imprimió correctamente la tarjeta?", "Pregunta", MessageBoxButton.);
                    if (Resultado == true)
                    {
                        MessageBox.Show("Impresa");
                        ElementoHtmlControl.SetProperty("innerHTML", "Impresa");
                    }
                    else
                    {
                        MessageBox.Show("No impresa");
                        ElementoHtmlControl.SetProperty("innerHTML", "No impresa");
                    }
                }
                else if (Impresora == SmartDriverDotNet.NombreImpresora)
                {
                    listBox1.Items.Add("Impresora asignada: '" + SmartDriverDotNet.NombreImpresora + "', modelo: 'CD800'");
                    listBox1.Items.Add("Los datos a imprimir son: " + nombre + "," + apellido + "," + nroTarjeta + "," + track1 + "," + track2 + "," + tipoTarjeta + "," + corporacion + "," + fechaVencimiento);
                    bool Resultado = smartDriverDotNet.Imprimir(ref nombre, ref apellido, ref nroTarjeta, ref track1, ref track2, ref tipoTarjeta, ref corporacion, ref fechaVencimiento);
                    //La pregunta la haremos en la página web, no en el silverlight
                    //Respuesta = MessageBox.Show("¿Se imprimió correctamente la tarjeta?", "Pregunta", MessageBoxButton.);
                    if (Resultado == true)
                    {
                        MessageBox.Show("Impresa");                      
                        ElementoHtmlControl.SetProperty("innerHTML", "Impresa");
                    }
                    else
                    {
                        MessageBox.Show("No impresa"); 
                        ElementoHtmlControl.SetProperty("innerHTML", "No impresa");
                    }
                }
                else
                {
                    listBox1.Items.Add("Estado: No es posible comunicarse con la impresora'");
                    //smartDriverDotNet = null;
                    //proyDataCardCP60 = null;
                }
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados.", "SumaPlazas.Dispositivos.Impresora.MainPage.ImprimirTarjeta", MessageBoxButton.OK);
            }
        }

    }
}
