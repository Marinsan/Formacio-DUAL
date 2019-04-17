using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Formacio.Beans
{
    public class ImagesBean 
    {

            public int? id              { get; set; }
            public int? idTelefono      { get; set; }
            public String image         { get; set; }
        

            public ImagesBean() { }

            public ImagesBean(  int? id,
                                int? idTelefono,
                                String image)


            {
                this.id = id;
                this.idTelefono = idTelefono;
                this.image = image;

            }



        
    }
}