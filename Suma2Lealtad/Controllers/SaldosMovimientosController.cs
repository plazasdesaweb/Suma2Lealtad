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
        //
        // GET: /SaldosMovimientos/

        public ActionResult Index()
        {
            SaldosMovimientos SaldosMovimientos = new SaldosMovimientos();
            
            SaldosMovimientos.DocId = "V-14566318".Replace("V-","");
            //SaldosMovimientos.DocId = "V-6960635".Replace("V-", "");
            string saldosJson = WSL.Cards("getbalance/" + SaldosMovimientos.DocId);
            SaldosMovimientos.Saldos = (IEnumerable<Saldo>)JsonConvert.DeserializeObject<IEnumerable<Saldo>>(saldosJson);
            string movimientosPrepagoJson = WSL.Cards("getbatch/" + SaldosMovimientos.Saldos.First().accounttype + "/" + SaldosMovimientos.DocId);
            string movimientosLealtadJson = WSL.Cards("getbatch/" + SaldosMovimientos.Saldos.Skip(1).First().accounttype + "/" + SaldosMovimientos.DocId);
            SaldosMovimientos.MovimientosPrepago = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosPrepagoJson);
            SaldosMovimientos.MovimientosSuma = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosLealtadJson);
            
            return View(SaldosMovimientos);
        }
        
    }
}
