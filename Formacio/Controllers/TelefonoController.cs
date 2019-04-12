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

                    result.Add(tm.toBean());

                    
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
