using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;


namespace InTheLoopAPI.Models
{
    public class ReviewImage
    {
        public int Id { get; set; }

        public int AttendanceId { get; set; }
        public virtual Attendance Attendance { get; set; }

        public byte[] Image { get; set; }
    }
}