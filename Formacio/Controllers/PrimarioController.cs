using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Formacio.Controllers
{
    public class PrimarioController : ApiController
    {

        /*GET ALL*/
        //[Route("api/formacio/departamentos")]
        //[HttpGet]
        [Authorize]
        public IHttpActionResult getDepartamentos()
        {
            try
            {
                FormacionEntities entity = new FormacionEntities();

                List<Departamentos> list = entity.Departamentos.Where(d => d.codigo != null).ToList();

                    //.Where(c => c.Departamentos);

                return Ok(list);

            } catch (Exception e) {

                throw e;
            }

        }
    }
}
