using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suma2Lealtad.Models;
using Suma2Lealtad.Modules;
using Newtonsoft.Json;

namespace Suma2Lealtad.Controllers
{
    public class SaldosMovimientosController : Controller
    {
        public ActionResult Index(string Documento)
        {
            SaldosMovimientos SaldosMovimientos = new SaldosMovimientos();

            Documento = "V-14566318";
            SaldosMovimientos.DocId = Documento.Substring(2);
            
            string saldosJson = WSL.Cards.getBalance(SaldosMovimientos.DocId);
            SaldosMovimientos.Saldos = (IEnumerable<Saldo>)JsonConvert.DeserializeObject<IEnumerable<Saldo>>(saldosJson);
            string movimientosPrepagoJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.First().accounttype , SaldosMovimientos.DocId);
            string movimientosLealtadJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.Skip(1).First().accounttype , SaldosMovimientos.DocId);
            SaldosMovimientos.MovimientosPrepago = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosPrepagoJson);
            SaldosMovimientos.MovimientosSuma = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosLealtadJson);
            
            return View(SaldosMovimientos);
        }
                
    }
}
