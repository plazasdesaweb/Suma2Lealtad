using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SumaPlazas.Dispositivos.Escaner
{
    public partial class MainPage : UserControl
    {
        string NombreArchivo;

        //se escribira en este elemento Htlml "Escaneada", "No escaneada"
        HtmlElement ElementoHtmlControl;

        public MainPage(string nombreArchivo, string idElementoHtmlControl)
        {
            NombreArchivo = nombreArchivo;
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
        }

        //private void btnEscanear_Click(object sender, RoutedEventArgs e)
        //{
        //    bool PermisosElevados = Application.Current.HasElevatedPermissions;
        //    if (PermisosElevados == true)
        //    {
        //        lblProcesando.Visibility = System.Windows.Visibility.Visible;                
        //        EscanerWIA EscanerWia = new EscanerWIA();
        //        string RutaArchivo = EscanerWia.ObtenerImagen(NombreArchivo);
        //        if (RutaArchivo == "")
        //        {
        //            MessageBox.Show("Error de Aplicación: No se pudo escanear el documento.", "SumaPlazas.Dispositivos.Escaner.MainPage.btnEscanear_Click", MessageBoxButton.OK);
        //            lblProcesando.Visibility = System.Windows.Visibility.Collapsed;
        //            return;
        //        }
        //        else
        //        {
        //            using (Stream Stream = File.OpenRead(RutaArchivo))
        //            {
        //                lblProcesando.Visibility = System.Windows.Visibility.Collapsed;
        //                BitmapImage BitmapImage = new BitmapImage();
        //                BitmapImage.SetSource(Stream);
        //                scannedImage.Source = BitmapImage;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error de Aplicación: No están configurados permisos elevados.", "SumaPlazas.Dispositivos.Escaner.MainPage.btnEscanear_Click", MessageBoxButton.OK);
        //    }
        //}

        [ScriptableMember]
        public void Escanear()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No Escaneada");
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                lblProcesando.Visibility = System.Windows.Visibility.Visible;
                EscanerWIA EscanerWia = new EscanerWIA();
                string RutaArchivo = EscanerWia.ObtenerImagen(NombreArchivo);
                if (RutaArchivo == "")
                {
                    MessageBox.Show("Error de Aplicación: No se pudo escanear el documento.", "SumaPlazas.Dispositivos.Escaner.MainPage.Escanear", MessageBoxButton.OK);
                    lblProcesando.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    using (Stream Stream = File.OpenRead(RutaArchivo))
                    {
                        lblProcesando.Visibility = System.Windows.Visibility.Collapsed;
                        BitmapImage BitmapImage = new BitmapImage();
                        BitmapImage.SetSource(Stream);
                        scannedImage.Source = BitmapImage;
                    }

                    ElementoHtmlControl.SetProperty("innerHTML", "Escaneada");
                }
            }
            else
            {
                MessageBox.Show("Error de Aplicación: No están configurados permisos elevados.", "SumaPlazas.Dispositivos.Escaner.MainPage.Escanear", MessageBoxButton.OK);
            }
        }

        //private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        //{
        //    EscanerWIA EscanerWia = new EscanerWIA();
        //    EscanerWia.LimpiarImagen(NombreArchivo);
        //    scannedImage.Source = null;
        //}

        [ScriptableMember]
        public void Limpiar()
        {
            ElementoHtmlControl.SetProperty("innerHTML", "No Escaneada");
            EscanerWIA EscanerWia = new EscanerWIA();
            EscanerWia.LimpiarImagen(NombreArchivo);
            scannedImage.Source = null;
        }

    }
}
