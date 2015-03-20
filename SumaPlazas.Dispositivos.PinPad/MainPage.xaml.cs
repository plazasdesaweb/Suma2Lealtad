using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.Automation;
using System.Threading;
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
        //valores para la demora delos Workers
        private int demora = 500;

        //se escribira en este elemento Htlml "Pin creado", "Pin cambiado", "Pin reiniciado", "No realizado"
        HtmlElement ElementoHtmlControl;

        public MainPage(string idElementoHtmlControl)
        {
            ElementoHtmlControl = HtmlPage.Document.GetElementById(idElementoHtmlControl);

            if (ElementoHtmlControl == null)
            {
                MessageBox.Show("Error: Elemento de control con id '" + idElementoHtmlControl + "' no encontrado.");
            }

            InitializeComponent();
            HtmlPage.RegisterScriptableObject("MainPage", this);
        }

        [ScriptableMember]
        public void CrearPin()
        {
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
                BackgroundWorker w = new BackgroundWorker();
                w.DoWork += new DoWorkEventHandler(w_DoWorkCrearPin);
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(w_RunWorkerCompletedCrearPin);
                w.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Error de Aplicación. No están configurados permisos elevados", "SumaPlazas.Dispositivos.PinPad.MainPage.CrearPin", MessageBoxButton.OK);
            }
        }

        private void w_DoWorkCrearPin(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activando pinpad...");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
        }

        private void w_RunWorkerCompletedCrearPin(object sender, RunWorkerCompletedEventArgs e)
        {
            Vpos Vpos = new Vpos();
            string Resultado = Vpos.CrearPin();
            listBox1.Items.Add("Pinpad desactivado.");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            string[] ArrResultado = Resultado.Split(',');
            if (ArrResultado[0] != "00")
            {
                listBox1.Items.Add("Respuesta recibida: Pin no creado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Items.Add("Error de Aplicación: No se pudo crear el pin.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            else
            {
                listBox1.Items.Add("Respuesta recibida: Pin creado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                ElementoHtmlControl.SetProperty("innerHTML", "Pin creado");
            }
        }

        [ScriptableMember]
        public void CambiarPin()
        {
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
                BackgroundWorker w = new BackgroundWorker();
                w.DoWork += new DoWorkEventHandler(w_DoWorkCambiarPin);
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(w_RunWorkerCompletedCambiarPin);
                w.RunWorkerAsync();
            }
        }

        private void w_DoWorkCambiarPin(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activando pinpad...");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
        }

        private void w_RunWorkerCompletedCambiarPin(object sender, RunWorkerCompletedEventArgs e)
        {
            Vpos Vpos = new Vpos();
            string Resultado = Vpos.CambiarPin();
            string[] ArrResultado = Resultado.Split(',');
            if (ArrResultado[0] != "00")
            {
                listBox1.Items.Add("Respuesta recibida: Pin no cambiado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Items.Add("Error de Aplicación: No se pudo crear el pin.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            else
            {
                listBox1.Items.Add("Respuesta recibida: Pin cambiado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                ElementoHtmlControl.SetProperty("innerHTML", "Pin cambiado");
            }
        }

        [ScriptableMember]
        public void ReiniciarPin()
        {
            bool PermisosElevados = Application.Current.HasElevatedPermissions;
            if (PermisosElevados == true)
            {
                listBox1.Items.Clear();
                ElementoHtmlControl.SetProperty("innerHTML", "No realizado");
                BackgroundWorker w = new BackgroundWorker();
                w.DoWork += new DoWorkEventHandler(w_DoWorkReiniciarPin);
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(w_RunWorkerCompletedReiniciarPin);
                w.RunWorkerAsync();
            }
        }

        private void w_DoWorkReiniciarPin(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activando pinpad...");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
            Thread.Sleep(demora);
            Dispatcher.BeginInvoke(delegate()
            {
                listBox1.Items.Add("Activado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            });
        }

        private void w_RunWorkerCompletedReiniciarPin(object sender, RunWorkerCompletedEventArgs e)
        {
            Vpos Vpos = new Vpos();
            string Resultado = Vpos.ReiniciarPin();
            string[] ArrResultado = Resultado.Split(',');
            if (ArrResultado[0] != "00")
            {
                listBox1.Items.Add("Respuesta recibida: Pin no reiniciado.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Items.Add("Error de Aplicación: No se pudo crear el pin.");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            else
            {
                listBox1.Items.Add("Respuesta recibida: Pin reiniciado.");
                ElementoHtmlControl.SetProperty("innerHTML", "Pin reiniciado");
            }
        }

    }
}
