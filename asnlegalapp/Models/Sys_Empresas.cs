//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace asnlegalapp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sys_Empresas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_Empresas()
        {
            this.Sys_Usuarios = new HashSet<Sys_Usuarios>();
            this.Tb_Clientes = new HashSet<Tb_Clientes>();
            this.Tb_Cotizaciones = new HashSet<Tb_Cotizaciones>();
            this.Tb_Proyectos = new HashSet<Tb_Proyectos>();
        }
    
        public int ID_empresa { get; set; }
        public string Nombre { get; set; }
        public string Logo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sys_Usuarios> Sys_Usuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Clientes> Tb_Clientes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Cotizaciones> Tb_Cotizaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Proyectos> Tb_Proyectos { get; set; }
    }
}
