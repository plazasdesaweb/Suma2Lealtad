﻿using Suma2Lealtad.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Suma2Lealtad.Modules
{
    public class AppSession
    {

        public class AppUser
        {
            public int id { get; set; }
            public string login { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string status { get; set; }
            public int role { get; set; }
            public List<AppRols> Roles { get; set; }
        }

        public class AppRols
        {
            public int roleid { get; set; }
        }

        private IList<CMenu> _menu = new List<CMenu>();
        private string _username = "";
        private string _userlogin = "";
        private int _userID;

        public IList<CMenu> MenuList { get { return _menu; } }
        public string UserName { get { return _username; } }
        public string UserLogin { get { return _userlogin; } }
        public int UserID { get { return _userID; } }
        public string AppDate { get { var dt = DateTime.Now; return dt.ToString("D", new CultureInfo("es-ES")); } }

        public bool Login(string login, string password)
        {

            using (LealtadEntities db = new LealtadEntities())
            {

                var user = db.Users.SingleOrDefault(u => u.login == login && u.passw == password);

                if (user != null)
                {

                    _username = "Usuario : " + user.lastname + ", " + user.firstname;
                    _userlogin = user.login;
                    _userID = user.id;

                    int[] roles = (from item in db.UserRols where item.userid == user.id select item.roleid).ToArray();

                    _menu = (from mainMenu in db.Menus 
                            join securityMenu in db.SecurityMenus 
                            on mainMenu.id equals securityMenu.menuid 
                            where roles.Contains( securityMenu.roleid )
                            select new CMenu
                            {
                                id = mainMenu.id,
                                name = mainMenu.name,
                                controller = mainMenu.controller,
                                actions = mainMenu.actions,
                                parentid = mainMenu.parentid,
                                order_no = mainMenu.order_no
                            }).Distinct().OrderBy(m => m.id).ThenBy(m => m.parentid).ThenBy(m => m.order_no).ToList();

                    _menu.Add(new CMenu { id = 1000, name = "Salir", controller = "", actions = "", parentid = 0, order_no = 1 });
                    _menu.Add(new CMenu { id = 1001, name = "Cerrar Sesión", controller = "Home", actions = "Logout", parentid = 1000, order_no = 1 });

                }
                return (user != null);
            }
        }
    }
}