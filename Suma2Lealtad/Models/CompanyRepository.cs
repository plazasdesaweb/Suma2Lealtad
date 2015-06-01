using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Suma2Lealtad.Models
{
    public class CompanyRepository
    {
        private const int ID_TYPE_PREPAGO = 2;

        #region SequenceID
        private int CompanyID()
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (db.Companies.Count() == 0)
                    return 1;
                return (db.Companies.Max(c => c.id) + 1);
            }
        }
        #endregion

        //busca una lista de compañias en SumaPLazas a partir del documento de identificación o el nombre
        public List<PrepagoCompanyAffiliattes> Find(string numdoc, string name = "")
        {
            List<PrepagoCompanyAffiliattes> compañias;
            using (LealtadEntities db = new LealtadEntities())
            {
                if (name == "")
                {
                    name = null;
                }
                compañias = (from c in db.Companies
                             where c.rif.Equals(numdoc) || c.name.Contains(name)
                             select new PrepagoCompanyAffiliattes()
                             {
                                 companyid = c.id,
                                 namecompañia = c.name,
                                 alias = c.ALIAS,
                                 rif = c.rif,
                                 address = c.address,
                                 phone = c.phone,
                                 email = c.email
                             }).ToList();
            }
            return compañias;
        }

        //busca sólo los datos de la compañia a partir de su id
        public Company FindCompany(int id)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                return db.Companies.Find(id);
            }
        }

        //busca una compañia CON TODOS SUS BENEFICIARIOS en SumaPlazas a partir del id
        public PrepagoCompanyAffiliattes Find(int companyid)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                PrepagoCompanyAffiliattes compañiaBeneficiarios = (from c in db.Companies
                                                                   where c.id.Equals(companyid)
                                                                   select new PrepagoCompanyAffiliattes()
                                                                   {
                                                                       companyid = c.id,
                                                                       namecompañia = c.name,
                                                                       alias = c.ALIAS,
                                                                       rif = c.rif,
                                                                       address = c.address,
                                                                       phone = c.phone,
                                                                       email = c.email
                                                                   }).Single();
                compañiaBeneficiarios.Beneficiarios = (from a in db.Affiliates
                                                       join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                                       join s in db.Status on a.statusid equals s.id
                                                       join t in db.Types on a.typeid equals t.id
                                                       join b in db.CompanyAffiliates on a.id equals b.affiliateid
                                                       where b.companyid.Equals(companyid) && a.typeid.Equals(ID_TYPE_PREPAGO)
                                                       select new Afiliado()
                                                       {
                                                           //ENTIDAD Affiliate 
                                                           id = a.id,
                                                           docnumber = a.docnumber,
                                                           //ENTIDAD CLIENTE
                                                           name = c.NOMBRE_CLIENTE1,
                                                           lastname1 = c.APELLIDO_CLIENTE1,
                                                           email = c.E_MAIL,
                                                           //ENTIDAD Status
                                                           estatus = s.name,
                                                           //ENTIDAD Type
                                                           type = t.name
                                                       }).ToList();
                if (compañiaBeneficiarios.Beneficiarios != null)
                {
                    foreach (var beneficiario in compañiaBeneficiarios.Beneficiarios)
                    {
                        Decimal p = (from t in db.TARJETAS
                                     where t.NRO_AFILIACION.Equals(beneficiario.id)
                                     select t.NRO_TARJETA
                                     ).SingleOrDefault();
                        if (p != 0)
                        {
                            beneficiario.pan = p.ToString();
                        }
                        else
                        {
                            beneficiario.pan = "";
                        }
                        string e = (from t in db.TARJETAS
                                    where t.NRO_AFILIACION.Equals(beneficiario.id)
                                    select t.ESTATUS_TARJETA
                                    ).SingleOrDefault();
                        if (e != null)
                        {
                            beneficiario.estatustarjeta = e.ToString();
                        }
                        else
                        {
                            beneficiario.estatustarjeta = "";
                        }
                    }
                }
                return compañiaBeneficiarios;
            }
        }

        //busca los beneficiarios por cedula o nombre para una compañia con id 
        public PrepagoCompanyAffiliattes FindBeneficiarios(int companyid, string numdoc, string name, string email)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                if (name == "")
                {
                    name = null;
                }
                if (email == "")
                {
                    email = null;
                }
                PrepagoCompanyAffiliattes compañiaBeneficiarios = (from c in db.Companies
                                                                   where c.id.Equals(companyid)
                                                                   select new PrepagoCompanyAffiliattes()
                                                                   {
                                                                       companyid = c.id,
                                                                       namecompañia = c.name,
                                                                       alias = c.ALIAS,
                                                                       rif = c.rif,
                                                                       address = c.address,
                                                                       phone = c.phone,
                                                                       email = c.email
                                                                   }).Single();
                if (name == null && email == null)
                {
                    compañiaBeneficiarios.Beneficiarios = (from a in db.Affiliates
                                                           join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                                           join s in db.Status on a.statusid equals s.id
                                                           join t in db.Types on a.typeid equals t.id
                                                           join b in db.CompanyAffiliates on a.id equals b.affiliateid
                                                           where b.companyid.Equals(companyid) && a.typeid.Equals(ID_TYPE_PREPAGO) && a.docnumber.Equals(numdoc)
                                                           select new Afiliado()
                                                           {
                                                               //ENTIDAD Affiliate 
                                                               id = a.id,
                                                               docnumber = a.docnumber,
                                                               typeid = a.typeid,
                                                               //ENTIDAD CLIENTE
                                                               name = c.NOMBRE_CLIENTE1,
                                                               lastname1 = c.APELLIDO_CLIENTE1,
                                                               email = c.E_MAIL,
                                                               //ENTIDAD Status
                                                               estatus = s.name,
                                                               //ENTIDAD Type
                                                               type = t.name
                                                           }).ToList();
                }
                else
                {
                    compañiaBeneficiarios.Beneficiarios = (from a in db.Affiliates
                                                           join c in db.CLIENTES on a.docnumber equals c.TIPO_DOCUMENTO + "-" + c.NRO_DOCUMENTO
                                                           join s in db.Status on a.statusid equals s.id
                                                           join t in db.Types on a.typeid equals t.id
                                                           join b in db.CompanyAffiliates on a.id equals b.affiliateid
                                                           where b.companyid.Equals(companyid) && a.typeid.Equals(ID_TYPE_PREPAGO) && (c.E_MAIL == email || c.NOMBRE_CLIENTE1.Contains(name) || c.APELLIDO_CLIENTE1.Contains(name))
                                                           select new Afiliado()
                                                           {
                                                               //ENTIDAD Affiliate 
                                                               id = a.id,
                                                               docnumber = a.docnumber,
                                                               typeid = a.typeid,
                                                               //ENTIDAD CLIENTE
                                                               name = c.NOMBRE_CLIENTE1,
                                                               lastname1 = c.APELLIDO_CLIENTE1,
                                                               email = c.E_MAIL,
                                                               //ENTIDAD Status
                                                               estatus = s.name,
                                                               //ENTIDAD Type
                                                               type = t.name
                                                           }).ToList();
                }
                if (compañiaBeneficiarios.Beneficiarios != null)
                {
                    foreach (var beneficiario in compañiaBeneficiarios.Beneficiarios)
                    {
                        Decimal p = (from t in db.TARJETAS
                                     where t.NRO_AFILIACION.Equals(beneficiario.id)
                                     select t.NRO_TARJETA
                                     ).SingleOrDefault();
                        if (p != 0)
                        {
                            beneficiario.pan = p.ToString();
                        }
                        else
                        {
                            beneficiario.pan = "";
                        }
                        string e = (from t in db.TARJETAS
                                    where t.NRO_AFILIACION.Equals(beneficiario.id)
                                    select t.ESTATUS_TARJETA
                                    ).SingleOrDefault();
                        if (e != null)
                        {
                            beneficiario.estatustarjeta = e.ToString();
                        }
                        else
                        {
                            beneficiario.estatustarjeta = "";
                        }
                    }
                }
                return compañiaBeneficiarios;
            }
        }

        public bool Save(Company company)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                var Company = new Company()
                {
                    id = CompanyID(),
                    name = company.name,
                    phone = company.phone,
                    rif = company.rif,
                    ALIAS = company.ALIAS,
                    address = company.address,
                    email = company.email,
                    creationdate = DateTime.Now,
                    userid = (int)HttpContext.Current.Session["userid"]
                };
                db.Companies.Add(Company);
                db.SaveChanges();
                return true;
            }
        }

        public bool SaveChanges(Company company)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                Company Company = db.Companies.FirstOrDefault(c => c.id == company.id);
                if (Company != null)
                {
                    Company.name = company.name;
                    Company.phone = company.phone;
                    Company.rif = company.rif;
                    Company.ALIAS = company.ALIAS;
                    Company.address = company.address;
                    Company.email = company.email;
                }
                db.SaveChanges();
                return true;
            }
        }

        //public int BuscarCompañia(Afiliado afiliado)
        //{
        //    using (LealtadEntities db = new LealtadEntities())
        //    {
        //        return (from c in db.CompanyAffiliates
        //                where c.affiliateid.Equals(afiliado.id)
        //                select c.companyid).SingleOrDefault();
        //    }
        //}

        public bool BorrarCompañia (int id)
        {
            using (LealtadEntities db = new LealtadEntities())
            {
                Company company = db.Companies.Find(id);
                if (company != null)
                {
                    db.Companies.Remove(company);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        internal PrepagoCompanyAffiliattes Find()
        {
            throw new NotImplementedException();
        }


    }
}