﻿using System;
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
            
            SaldosMovimientos.DocId = "V-16006920".Replace("V-","");
            //SaldosMovimientos.DocId = "V-6960635".Replace("V-", "");
            string saldosJson = WSL.Cards.getBalance(SaldosMovimientos.DocId);
            SaldosMovimientos.Saldos = (IEnumerable<Saldo>)JsonConvert.DeserializeObject<IEnumerable<Saldo>>(saldosJson);
            string movimientosPrepagoJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.First().accounttype , SaldosMovimientos.DocId);
            string movimientosLealtadJson = WSL.Cards.getBatch(SaldosMovimientos.Saldos.Skip(1).First().accounttype , SaldosMovimientos.DocId);
            SaldosMovimientos.MovimientosPrepago = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosPrepagoJson);
            SaldosMovimientos.MovimientosSuma = (IEnumerable<Movimiento>)JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(movimientosLealtadJson);
            
            return View(SaldosMovimientos);
        }

        //
        // GET: /SaldosMovimientos/Acreditar/

        public ActionResult Acreditar(int id = 0)
        {
            return View(saldosmovimientos);
        }

        public IView saldosmovimientos { get; set; }
    }
}
