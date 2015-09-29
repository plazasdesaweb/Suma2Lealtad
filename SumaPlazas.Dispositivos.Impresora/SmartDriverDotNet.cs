using System;
using System.Runtime.InteropServices.Automation;

namespace SumaPlazas.Dispositivos.Impresora
{
    public class SmartDriverDotNet
    {
        public const string NombreImpresora = "XPS Card Printer";
        
        public bool Imprimir(string Nombre, string Apellido, string NroTarjeta, string Track1, string Track2, string TipoTarjeta, string Corporacion, string FechaVencimiento)
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic SmartDriverDotNet = AutomationFactory.CreateObject("SumaPlazas.Librerias.Impresora.CD800.SmartDriverDotNet"))
                    {
                        if (SmartDriverDotNet != null)
                        {
                            //MessageBox.Show("Aqui voy");
                            bool Resultado = SmartDriverDotNet.Imprimir(NombreImpresora, Nombre, Apellido, NroTarjeta, Track1, Track2, TipoTarjeta, Corporacion, FechaVencimiento);
                            //MessageBox.Show("Resultado: " + Resultado );
                            return Resultado;
                        }
                        else
                        {
                            //MessageBox.Show("Error de Aplicación. No se pudo imprimir en la cd800 1.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.Imprimir", MessageBoxButton.OK);
                            return false;
                        }
                    }
                }
               else
                {
                    //MessageBox.Show("Error de Aplicación. No se pudo imprimir en la cd800 2.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.Imprimir", MessageBoxButton.OK);
                    return false;
                }
            }
            //catch
            catch (Exception originalException)
            {
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.Imprimir", MessageBoxButton.OK);
                string error = "Error de Aplicación: " + originalException;
                return false;
            }
        }

        public string EstadoImpresora()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic SmartDriverDotNet = AutomationFactory.CreateObject("SumaPlazas.Librerias.Impresora.CD800.SmartDriverDotNet"))
                    {
                        if (SmartDriverDotNet != null)
                        {
                            string Resultado = SmartDriverDotNet.EstadoImpresora(NombreImpresora);
                            return Resultado;
                        }
                        else
                        {
                            //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 1.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                            return "";
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 2.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                    return "";
                }
            }
            catch
            //catch (Exception originalException)
            {
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                return "";
            }
        }

        public string ReanudarImpresora()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic SmartDriverDotNet = AutomationFactory.CreateObject("SumaPlazas.Librerias.Impresora.CD800.SmartDriverDotNet"))
                    {
                        if (SmartDriverDotNet != null)
                        {
                            string Resultado = SmartDriverDotNet.ReanudarImpresora(NombreImpresora);
                            return Resultado;
                        }
                        else
                        {
                            //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 1.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                            return "";
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 2.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                    return "";
                }
            }
            catch
            //catch (Exception originalException)
            {
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                return "";
            }
        }

        public string LimpiarErrores()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic SmartDriverDotNet = AutomationFactory.CreateObject("SumaPlazas.Librerias.Impresora.CD800.SmartDriverDotNet"))
                    {
                        if (SmartDriverDotNet != null)
                        {
                            string Resultado = SmartDriverDotNet.LimpiarErrores(NombreImpresora);
                            return Resultado;
                        }
                        else
                        {
                            //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 1.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                            return "";
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Error de Aplicación. No se pudo recibir estado cd800 2.", "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                    return "";
                }
            }
            catch
            //catch (Exception originalException)
            {
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.Impresora.CD800.SmartDriverDotNet.EstadoImpresora", MessageBoxButton.OK);
                return "";
            }
        }
    }
}
