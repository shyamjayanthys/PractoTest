using Microsoft.AspNetCore.Mvc;
using PractoPrototypeAPI.Logic;
using PractoPrototypeAPI.Model;
using System;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Contorller
{
    //[Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Doctor")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorLogic _logic;

        public DoctorController(IDoctorLogic logic)
        {
            _logic = logic;
        }
        [HttpGet]
        [Route("GetMyAppointments")]
        public async Task<IActionResult> GetMyAppointments(int doctorId)
        {
            try
            {
                return Ok(await _logic.GetAppointmentList(doctorId));
            }
            catch (Exception ex)
            {
                //_logger?.LogWarning("Exception in DoctorController GetMyAppointments: {Message}", ex.Message);
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("EditMyAppointment")]
        public async Task<IActionResult> EditMyAppointment([FromBody] DoctorAppointmentManagementViewModel  doctorAppointmentManagement, string appointmentStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(await _logic.EditMyAppointment(doctorAppointmentManagement, appointmentStatus));
            }
            catch (Exception ex)
            {
                //_logger?.LogWarning("Exception in DoctorController EditMyAppointment: {Message}", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
