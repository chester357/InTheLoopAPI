﻿using InTheLoopAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace InTheLoopAPI.Controllers
{
    [Authorize]
    public class AttendanceController : ApiController
    {
        private AttendanceService _attendanceService;

        public AttendanceController()
        {
            _attendanceService = new AttendanceService();
        }

        [HttpGet, Route("api/Attendance/Count/{eventHeaderId}")]
        public IHttpActionResult GetCount(int eventHeaderId)
        {
            try
            {
                var results = _attendanceService.GetAttendanceCount(eventHeaderId);

                return Ok(results);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("api/Attendies/{eventHeaderId}")]
        public IHttpActionResult GetAttendances(int eventHeaderId)
        {
            try
            {
                var results = _attendanceService.GetAttendies(eventHeaderId);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/Attendance/{eventHeaderId}")]
        public IHttpActionResult PlusOne(int eventHeaderId)
        {
            try
            {
                var result = _attendanceService.PlueOne(User.Identity.GetUserId(), eventHeaderId);

                if (result == null)
                    return Ok();

                return BadRequest(result.ErrorMessage);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("api/Attendance/{eventHeaderId}")]
        public IHttpActionResult RemoveAttendance(int eventHeaderId)
        {
            try
            {
                var result = _attendanceService.RemoveAttendance(User.Identity.GetUserId(), eventHeaderId);

                if (result == null)
                    return Ok();

                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}