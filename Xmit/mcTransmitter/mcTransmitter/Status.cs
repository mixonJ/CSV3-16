
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace mcTransmitter
{

using System;
    using System.Collections.Generic;
    
public partial class Status
{

    public Status()
    {

        this.CompanyBidStatuses = new HashSet<CompanyBidStatus>();

    }


    public decimal StatusID { get; set; }

    public string StatusName { get; set; }

    public System.DateTime Inserted { get; set; }

    public System.DateTime Modified { get; set; }

    public System.DateTime MsModificationDate { get; set; }



    public virtual ICollection<CompanyBidStatus> CompanyBidStatuses { get; set; }

}

}
