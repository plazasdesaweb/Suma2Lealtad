using System;
using System.Collections.Generic;

namespace Suma2Lealtad.Models
{
    public class Afiliado
    {
        /* Affiliate */
        public int id { get; set; }
        public int customerid { get; set; }
        public string docnumber { get; set; }
        public int clientid { get; set; }
        public string storeid { get; set; }
        public int channelid { get; set; }
        public int typeid { get; set; }
        public string typedelivery { get; set; }
        public int storeiddelivery { get; set; }
        public int statusid { get; set; }
        public string estatus { get; set; }
        public int reasonsid { get; set; }
        public string twitter_account { get; set; }
        public string facebook_account { get; set; }
        public string instagram_account { get; set; }
        public string comments { get; set; }
        /* Cliente */
        public string type { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public DateTime? birthdate { get; set; }
        public string nationality { get; set; }
        public string occupation { get; set; }
        public string phone3 { get; set; }
        public List<Interest> Intereses { get; set; }
        public string cod_estado { get; set; }
        public string cod_ciudad { get; set; }
        public string cod_municipio { get; set; }
        public string cod_parroquia { get; set; }
        public string cod_urbanizacion { get; set; }
        public string picture { get; set; }
        /* Excepciones */
        public string exnumber { get; set; }
        public string exdetail { get; set; }

        public class Sexo
        {
            public int id { get; set; }
            public string sexo { get; set; }
        }

        public IEnumerable<Sexo> SexoOptions =
            new List<Sexo>
        {
              new Sexo { id = 0, sexo = "" },
              new Sexo { id = 1, sexo = "Masculino" },
              new Sexo { id = 2, sexo = "Femenino"  }
        };


        public class MaritalStatus
        {
            public int id { get; set; }
            public string maritalstatus { get; set; }
        }

        public IEnumerable<MaritalStatus> MaritalStatusOptions =
            new List<MaritalStatus>
        {
              new MaritalStatus { id = 0, maritalstatus = "" },
              new MaritalStatus { id = 1, maritalstatus = "Soltero"     },
              new MaritalStatus { id = 2, maritalstatus = "Casado"      },
              new MaritalStatus { id = 3, maritalstatus = "Divorciado"  },
              new MaritalStatus { id = 4, maritalstatus = "Viudo"       }

        };

        public class Store
        {
            public string id { get; set; }
            public string sucursal { get; set; }
        }

        public IEnumerable<Store> StoreOptions =
            new List<Store>
        {
            new Store {id =   null, sucursal = ""                 },
            new Store {id = "1002", sucursal = "Prados del Este"  },
            new Store {id = "1003", sucursal = "Cafetal"          },
            new Store {id = "1005", sucursal = "Los Samanes"      },
            new Store {id = "1006", sucursal = "Avila"            },
            new Store {id = "1007", sucursal = "Galerías"         },
            new Store {id = "1008", sucursal = "La Lagunita"      },
            new Store {id = "1009", sucursal = "Los Cedros"       },
            new Store {id = "1010", sucursal = "Centro Plaza"     },
            new Store {id = "1011", sucursal = "Vista Alegre"     },
            new Store {id = "1012", sucursal = "Los Naranjos"     },
            new Store {id = "1013", sucursal = "Valle Arriba"     },
            new Store {id = "1014", sucursal = "El Parral"        },
            new Store {id = "1016", sucursal = "Veracruz"         },
            new Store {id = "1017", sucursal = "Los Chaguaramos"  },
            new Store {id = "1018", sucursal = "Guarenas"         },
            new Store {id = "1019", sucursal = "Guatire"          }
        };

        public class Estado
        {
            public string id { get; set; }
            public string estado { get; set; }
        }

        public IEnumerable<Estado> EstadoOptions =
            new List<Estado>
        {
            new Estado {id = " ", estado = ""        },
            new Estado {id = "1", estado = "DC"      },
            new Estado {id = "2", estado = "Miranda" },
            new Estado {id = "3", estado = "Vargas"  },
            new Estado {id = "4", estado = "Aragua"  }
        };

        public class Ciudad
        {
            public string id { get; set; }
            public string ciudad { get; set; }
        }

        public IEnumerable<Ciudad> CiudadOptions =
            new List<Ciudad>
        {
            new Ciudad {id =" ", ciudad = ""        },
            new Ciudad {id = "1", ciudad = "Caracas" },
            new Ciudad {id = "2", ciudad = "Maracay" }
        };

        public class Municipio
        {
            public string id { get; set; }
            public string municipio { get; set; }
        }

        public IEnumerable<Municipio> MunicipioOptions =
            new List<Municipio>
        {
            new Municipio {id =" ", municipio = ""           },
            new Municipio {id = "1", municipio = "Libertador" },
            new Municipio {id = "2", municipio = "Baruta"     }
        };

        public class Parroquia
        {
            public string id { get; set; }
            public string parroquia { get; set; }
        }

        public IEnumerable<Parroquia> ParroquiaOptions =
            new List<Parroquia>
        {
            new Parroquia {id =" ", parroquia = ""         },
            new Parroquia {id = "1", parroquia = "Paraíso"  },
            new Parroquia {id = "2", parroquia = "San José" },
            new Parroquia {id = "3", parroquia = "San Juan" },
            new Parroquia {id = "4", parroquia = "Baruta"   },
            new Parroquia {id = "5", parroquia = "Hatillo"  }

        };

        public class Urbanizacion
        {
            public string id { get; set; }
            public string urbanizacion { get; set; }
        }

        public IEnumerable<Urbanizacion> UrbanizacionOptions =
            new List<Urbanizacion>
        {
            new Urbanizacion {id =" ", urbanizacion = ""            },
            new Urbanizacion {id = "1", urbanizacion = "El Paraíso"  },
            new Urbanizacion {id = "2", urbanizacion = "Las Acacias" },
            new Urbanizacion {id = "3", urbanizacion = "Baruta"      },
            new Urbanizacion {id = "4", urbanizacion = "Hatillo"     }

        };

        public class Channel
        {
            public int id { get; set; }
            public string channel { get; set; }
        }

        public IEnumerable<Channel> ChannelOptions =
            new List<Channel>
        {
              new Channel { id = 0, channel = ""           },
              new Channel { id = 1, channel = "Promotor"   },
              new Channel { id = 2, channel = "Página Web" },
              new Channel { id = 3, channel = "Eventos"    }
        };

        public class TypeDelivery
        {
            public int id { get; set; }
            public string delivery { get; set; }
        }

        public IEnumerable<TypeDelivery> TypeDeliveryOptions =
            new List<TypeDelivery>
        {
              new TypeDelivery { id = 0, delivery = ""                },
              new TypeDelivery { id = 1, delivery = "Retirar en Sucursal" },
              new TypeDelivery { id = 2, delivery = "Envío a Casa"        },
              new TypeDelivery { id = 3, delivery = "Envío a Oficina"     }
        };




    }

}