//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Formacio
{
    using System;
    using System.Collections.Generic;
    
    public partial class Clientes
    {
        public int ID { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO1 { get; set; }
        public string APELLIDO2 { get; set; }
        public Nullable<int> ID_DEPARTAMENTO { get; set; }
    
        public virtual Departamentos Departamentos { get; set; }
    }
}