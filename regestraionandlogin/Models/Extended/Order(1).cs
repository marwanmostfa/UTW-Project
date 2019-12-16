using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace regestraionandlogin.Models
{
    [MetadataType(typeof(OrdermetaData))]
    public partial class Order
    {
    }
    public class OrdermetaData
    {
      

        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string TransDate { get; set; }

        
    }
}