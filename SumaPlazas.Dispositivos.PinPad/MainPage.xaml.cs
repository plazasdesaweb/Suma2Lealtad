using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Automation;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SumaPlazas.Dispositivos.PinPad
{
    public partial class MainPage : UserControl
    {
        //se escribira en este elemento Htlml "Pin creado", "Pin cambiado", "Pin reiniciado", "No realizado"
        HtmlElement ElementoHtmlControl;

        public MainPage(string idElementoHtmlControl)
        {
            ElementoHtmlControl = HtmlPage.Document.GetElementById(idElementoHtmlControl);

            if (ElementoHtmlControl != null)
            {
                //ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
            }
            else
            {
                MessageBox.Show("Error: Elemento de control con id '" + idElementoHtmlControl + "' no encontrado.");
            } 
            
            InitializeComponent();
            HtmlPage.RegisterScriptableObject("MainPage", this);
        }

        //private void btnCrearPin_Click(object sender, RoutedEventArgs e)
        //{
        //    string Resultado;
        //    string[] ArrResultado;
        //    bool PermisosElevados = Application.Current.HasElevatedPermissions;
        //    if (PermisosElevados == true)
        //    {
        //        listBox1.Items.Add("Llamada a CrearPin");
        //        Vpos Vpos = new Vpos();
        //        Resultado = Vpos.CrearPin();
        //        ArrResultado = Resultado.Split(',');
        //        listBox1.Items.Add("Resultado: " + Resultado);
        //        foreach (var elemento in ArrResultado)
        //        {
        //            listBox1.Items.Add(elemento.ToString());
        //        }
        //        if (ArrResultado[0] == "00")
        //        {
        //            listBox1.Items.Add("Pin creado exitosamente");
        //        }
        //        else
        //        {
        //            listBox1.Items.Add("Pin no creado");
        //        }
        //        listBox1.Items.Add("Fin llamada a CrearPin");        
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.btnCrearPin_Click", MessageBoxButton.OK);
        //    }
        //}

        [ScriptableMember]
        public void CrearPin()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();            
                listBox1.Items.Add("Activando PinPad...");
                Vpos Vpos = new Vpos();
                string Resultado = Vpos.CrearPin();
                string[]  ArrResultado = Resultado.Split(',');
                //listBox1.Items.Add("Resultado: " + Resultado);
                //foreach (var elemento in ArrResultado)
                //{
                //    listBox1.Items.Add(elemento.ToString());
                //}
                if (ArrResultado[0] != "00")
                {
                    listBox1.Items.Add("Respuesta recibida: Pin no creado.");
                    MessageBox.Show("Error de Aplicación: No se pudo crear el pin.", "SumaPlazas.Dispositivos.PinPad.MainPage.CrearPin", MessageBoxButton.OK);                    
                }
                else
                {
                    listBox1.Items.Add("Respuesta recibida: Pin creado.");
                    ElementoHtmlControl.SetProperty("innerHTML", "Pin creado");
                }
                listBox1.Items.Add("Desactivando PinPad...");
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.CrearPin", MessageBoxButton.OK);
            }
        }

        [ScriptableMember]
        public void CambiarPin()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Activando PinPad...");
                Vpos Vpos = new Vpos();
                string Resultado = Vpos.CambiarPin();
                string[] ArrResultado = Resultado.Split(',');
                //listBox1.Items.Add("Resultado: " + Resultado);
                //foreach (var elemento in ArrResultado)
                //{
                //    listBox1.Items.Add(elemento.ToString());
                //}
                if (ArrResultado[0] != "00")
                {
                    listBox1.Items.Add("Respuesta recibida: Pin no cambiado.");
                    MessageBox.Show("Error de Aplicación: No se pudo crear el pin.", "SumaPlazas.Dispositivos.PinPad.MainPage.CambiarPin", MessageBoxButton.OK);
                }
                else
                {
                    listBox1.Items.Add("Respuesta recibida: Pin cambiado.");
                    ElementoHtmlControl.SetProperty("innerHTML", "Pin cambiado");
                }
                listBox1.Items.Add("Desactivando PinPad...");
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.CambiarPin", MessageBoxButton.OK);
            }
        }

        [ScriptableMember]
        public void ReiniciarPin()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Activando PinPad...");
                Vpos Vpos = new Vpos();
                string Resultado = Vpos.ReiniciarPin();
                string[] ArrResultado = Resultado.Split(',');
                //listBox1.Items.Add("Resultado: " + Resultado);
                //foreach (var elemento in ArrResultado)
                //{
                //    listBox1.Items.Add(elemento.ToString());
                //}
                if (ArrResultado[0] != "00")
                {
                    listBox1.Items.Add("Respuesta recibida: Pin no reiniciado.");
                    MessageBox.Show("Error de Aplicación: No se pudo crear el pin.", "SumaPlazas.Dispositivos.PinPad.MainPage.ReiniciarPin", MessageBoxButton.OK);
                }
                else
                {
                    listBox1.Items.Add("Respuesta recibida: Pin reiniciado.");
                    ElementoHtmlControl.SetProperty("innerHTML", "Pin reiniciado");
                }
                listBox1.Items.Add("Desactivando PinPad...");
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.ReiniciarPin", MessageBoxButton.OK);
            }
        }

        //private void btnCambiarPin_Click(object sender, RoutedEventArgs e)
        //{
        //    string Resultado;
        //    string[] ArrResultado;
        //    bool PermisosElevados = Application.Current.HasElevatedPermissions;
        //    if (PermisosElevados == true)
        //    {
        //        listBox1.Items.Add("Llamada a CambiarPin");
        //        Vpos Vpos = new Vpos();
        //        Resultado = Vpos.CambiarPin();
        //        ArrResultado = Resultado.Split(',');
        //        listBox1.Items.Add("Resultado: " + Resultado);
        //        foreach (var elemento in ArrResultado)
        //        {
        //            listBox1.Items.Add(elemento.ToString());
        //        }
        //        if (ArrResultado[0] == "00")
        //        {
        //            listBox1.Items.Add("Pin cambiado exitosamente");
        //        }
        //        else
        //        {
        //            listBox1.Items.Add("Pin no cambiado");
        //        }
        //        listBox1.Items.Add("Fin llamada a CambiarPin");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.btnCambiarPin_Click", MessageBoxButton.OK);
        //    }
        //}

        //private void btnReiniciarPin_Click(object sender, RoutedEventArgs e)
        //{
        //    string Resultado;
        //    string[] ArrResultado;
        //    bool PermisosElevados = Application.Current.HasElevatedPermissions;
        //    if (PermisosElevados == true)
        //    {
        //        listBox1.Items.Add("Llamada a ReiniciarPin");
        //        Vpos Vpos = new Vpos();
        //        Resultado = Vpos.ReiniciarPin();
        //        ArrResultado = Resultado.Split(',');
        //        listBox1.Items.Add("Resultado: " + Resultado);
        //        foreach (var elemento in ArrResultado)
        //        {
        //            listBox1.Items.Add(elemento.ToString());
        //        }
        //        if (ArrResultado[0] == "00")
        //        {
        //            listBox1.Items.Add("Pin reiniciado exitosamente");
        //        }
        //        else
        //        {
        //            listBox1.Items.Add("Pin no reiniciado");
        //        }
        //        listBox1.Items.Add("Fin llamada a ReiniciarPin");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.btnReiniciarPin_Click", MessageBoxButton.OK);
        //    }
        //}
    }
}
