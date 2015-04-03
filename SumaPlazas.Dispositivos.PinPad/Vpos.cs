using System.Runtime.InteropServices.Automation;

namespace SumaPlazas.Dispositivos.PinPad
{
    public class Vpos
    {
        public string CrearPin()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic Vpos = AutomationFactory.CreateObject("SumaPlazas.Librerias.PinPad.Vpos"))
                    {
                        if (Vpos != null)
                        {
                            string Resultado = Vpos.CrearPin();
                            return Resultado;
                        }
                        else
                        {
                            return "";
                            //MessageBox.Show("Error de Aplicación: No se pudo crear objeto Vpos.", NombreMetodo, MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    return "";
                    //MessageBox.Show("Error de Aplicación: AutomationFactory no disponible.", NombreMetodo, MessageBoxButton.OK);
                }
            }
            catch
            //catch (Exception originalException)
            {
                return "";
                //MessageBox.Show("Error de Aplicación: " + originalException, NombreMetodo, MessageBoxButton.OK);
                //return null;
            }
        }

        public string CambiarPin()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic Vpos = AutomationFactory.CreateObject("SumaPlazas.Librerias.PinPad.Vpos"))
                    {
                        if (Vpos != null)
                        {
                            string Resultado = Vpos.CambiarPin();
                            return Resultado;
                        }
                        else
                        {
                            return "";
                            //MessageBox.Show("Error de Aplicación. No se pudo cambiar pin 1.", "SumaPlazas.Dispositivos.PinPad.Vpos.CambiarPin", MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    return "";
                    //MessageBox.Show("Error de Aplicación. No se pudo cambiar pin 2.", "SumaPlazas.Dispositivos.PinPad.Vpos.CambiarPin", MessageBoxButton.OK);
                }
            }
            catch
            //catch (Exception originalException)
            {
                return "";
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.PinPad.Vpos.CambiarPin", MessageBoxButton.OK);
                //return null;
            }
        }

        public string ReiniciarPin()
        {
            try
            {
                if (AutomationFactory.IsAvailable)
                {
                    using (dynamic Vpos = AutomationFactory.CreateObject("SumaPlazas.Librerias.PinPad.Vpos"))
                    {
                        if (Vpos != null)
                        {
                            string Resultado = Vpos.ReiniciarPin();
                            return Resultado;
                        }
                        else
                        {
                            return "";
                            //MessageBox.Show("Error de Aplicación. No se pudo reiniciar pin 1.", "SumaPlazas.Dispositivos.PinPad.Vpos.ReiniciarPin", MessageBoxButton.OK);
                        }
                    }
                }
                else
                {
                    return "";
                    //MessageBox.Show("Error de Aplicación. No se pudo reiniciar pin 2.", "SumaPlazas.Dispositivos.PinPad.Vpos.ReiniciarPin", MessageBoxButton.OK);
                }
            }
            catch
            //catch (Exception originalException)
            {
                return "";
                //MessageBox.Show("Error de Aplicación: " + originalException, "SumaPlazas.Dispositivos.PinPad.Vpos.ReiniciarPin", MessageBoxButton.OK);
                //return null;
            }
        }

    }
}
