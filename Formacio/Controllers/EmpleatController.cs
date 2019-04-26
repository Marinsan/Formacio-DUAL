using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Formacio.Controllers
{
    [Authorize]
    [RoutePrefix("api/empleat")]
    public class EmpleatController : ApiController
    {

        [Route("empleats")]
        [HttpGet]
        public IHttpActionResult getEmpleats()
        {

            try
            {
                FormacionEntities entity = new FormacionEntities();

                List<Beans.EmpleatBean> result = new List<Beans.EmpleatBean>();

                foreach ( Empleats tm in entity.Empleats.ToList())
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

        [Route("{codi}")]
        [HttpGet]
        public IHttpActionResult getEmpleat(String codi)
        {

            try
            {
                FormacionEntities entity = new FormacionEntities();

                codi = codi.ToUpper();

                List<Empleats> list = entity.Empleats.Where(t => t.codi == codi).ToList();


                return Ok(list);

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        /*Delete*/
        [Route("deleteEmpleat")]
        [HttpDelete]
        public IHttpActionResult deleteEmpleat(String codi)
        {

            try
            {
                FormacionEntities entity = new FormacionEntities();

                codi = codi.ToUpper();

                List<Empleats> list = entity.Empleats.Where(t => t.codi == codi).ToList();

                if (codi == null)
                {
                    return NotFound();
                }

                entity.Empleats.Remove(list);
                entity.SaveChanges();
                return Ok(list);

            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
