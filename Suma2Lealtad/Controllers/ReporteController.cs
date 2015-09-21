using Suma2Lealtad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Suma2Lealtad.Controllers
{
    public class ReporteController : Controller
    {
        private BeneficiarioPrepagoRepository repBeneficiario = new BeneficiarioPrepagoRepository();

        public ActionResult FilterReporteRecargas()
        {
            ReportePrepago reporte = new ReportePrepago();
            return View(reporte);
        }

        public ActionResult FilterReporteCompras()
        {
            ReportePrepago reporte = new ReportePrepago();
            reporte.ListaClientes = repBeneficiario.GetClientes();
            reporte.ListaClientes.Insert(0, new PrepaidCustomer { id = 0, name = "" });
            return View(reporte);
        }

        public ActionResult FilterReporteTarjetas()
        {
            ReportePrepago reporte = new ReportePrepago();
            return View(reporte);
        }

        [HttpPost]
        public ActionResult ReporteCompras(string TipoConsulta, string ModoTransaccion, string fechadesde, string fechahasta, int idCliente = 0, string numdoc = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            if (TipoConsulta == "Cliente")
            {
                if (idCliente == 0)
                {
                    reporte = repBeneficiario.ReporteComprasxCliente("todos", fechadesde, fechahasta, ModoTransaccion);
                }
                else
                {
                    reporte = repBeneficiario.ReporteComprasxCliente("uno", fechadesde, fechahasta, ModoTransaccion, idCliente);
                }
            }
            else if (TipoConsulta == "Beneficiario")
            {
                //if (numdoc == "")
                //{
                //    reporte = repBeneficiario.ReporteComprasxBeneficiario("todos", fechadesde, fechahasta, ModoTransaccion);
                //}
                //else
                //{
                //    reporte = repBeneficiario.ReporteComprasxBeneficiario("uno", fechadesde, fechahasta, ModoTransaccion, numdoc);
                //}
            }
            ParametrosReporte p = new ParametrosReporte()
            {
                TipoConsulta = TipoConsulta,
                ModoTransaccion = ModoTransaccion,
                fechadesde = fechadesde,
                fechahasta = fechahasta,
                idCliente = idCliente,
                numdoc = numdoc
            };
            if (reporte.Count == 0)
            {
                ReportePrepago r = new ReportePrepago()
                {
                    Parametros = p
                };
                reporte.Add(r);
            }
            else
            {
                reporte.First().Parametros = p;
            }
            return View(reporte);
        }

        public ActionResult GenerateReporteComprasPDF(string TipoConsulta, string ModoTransaccion, string fechadesde, string fechahasta, int idCliente = 0, string numdoc = "")
        {
            List<ReportePrepago> reporte = new List<ReportePrepago>();
            if (TipoConsulta == "Cliente")
            {
                if (idCliente == 0)
                {
                    reporte = repBeneficiario.ReporteComprasxCliente("todos", fechadesde, fechahasta, ModoTransaccion);
                }
                else
                {
                    reporte = repBeneficiario.ReporteComprasxCliente("uno", fechadesde, fechahasta, ModoTransaccion, idCliente);
                }
            }
            else if (TipoConsulta == "Beneficiario")
            {
                //if (numdoc == "")
                //{
                //    reporte = repBeneficiario.ReporteComprasxBeneficiario("todos", fechadesde, fechahasta, ModoTransaccion);
                //}
                //else
                //{
                //    reporte = repBeneficiario.ReporteComprasxBeneficiario("uno", fechadesde, fechahasta, ModoTransaccion, numdoc);
                //}
            }
            string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            return new Rotativa.ViewAsPdf("ReporteComprasPDF", reporte)
            {
                FileName = "Reporte de Compras.pdf",
                CustomSwitches = footer
            };
        }

        [HttpPost]
        public ActionResult ReporteRecargas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReporteTarjetas()
        {
            return View();
        }

    }

}
