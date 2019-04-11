using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Formacio.Controllers
{
    [Authorize]
    [RoutePrefix("api/telefono")]
    public class TelefonoController : ApiController
    {

        [Route("telefonos")]
        [HttpGet]
        public IHttpActionResult getTelefonos()
        {

            try
            {
                FormacionEntities entity = new FormacionEntities();

                List<Beans.TelefonoBean> result = new List<Beans.TelefonoBean>();

                foreach ( Telefonos tm in entity.Telefonos.ToList())
                {
                    result.Add(new Beans.TelefonoBean(
                        tm.id, 
                        tm.codigo, 
                        tm.imageURL, 
                        tm.name, 
                        tm.snippet,
                        tm.description,
                        tm.storage_ram,
                        tm.storage_flash,
                        tm.size,
                        tm.weight,
                        new Beans.So( tm.So.id, tm.So.codigo, tm.So.tipo, tm.So.versionOs, tm.So.versionUi
                            ),
                        new Beans.Connectivity( tm.Connectivity.id, tm.Connectivity.codigo, tm.Connectivity.bluetooth,
                                                tm.Connectivity.cell, tm.Connectivity.infrared, tm.Connectivity.wifi,
                                                tm.Connectivity.gps)));
                }

                return Ok(result);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        [Route("{codigo}")]
        [HttpGet]
        public IHttpActionResult getTelefono(String codigo)
        {

            try
            {
                FormacionEntities entity = new FormacionEntities();

                codigo = codigo.ToUpper();

                List<Telefonos> list = entity.Telefonos.Where(t => t.codigo == codigo).ToList();


                return Ok(list);

            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
