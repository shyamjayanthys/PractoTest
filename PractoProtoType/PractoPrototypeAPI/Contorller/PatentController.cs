using Microsoft.AspNetCore.Mvc;
using PractoPrototypeAPI.Logic;
using PractoPrototypeAPI.Model;
using System;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Contorller
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Patent")]
    public class PatentController : ControllerBase
    {
        private readonly IPatenetLogic _logic;
        public PatentController(IPatenetLogic logic)
        {
            _logic = logic;
        }
        [HttpGet]
        [Route("GetSelectDoctorAppointment")]
        public async Task<IActionResult> GetSelectDoctorAppointment(int patentId)
        {
            try
            {
                return Ok( await _logic.GetDoctorsList(patentId));
            }
            catch (Exception ex)
            {
                //_logger?.LogWarning("Exception in PatentController GetSelectDoctorAppointment: {Message}", ex.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("SubmitAppointment")]
        public async Task<IActionResult> GetSelectDoctorAppointment([FromBody] PatentAppointmenRequestViewModel patentAppointmen)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(await _logic.SubmitPatentAppointment(patentAppointmen));
            }
            catch (Exception ex)
            {
                //_logger?.LogWarning("Exception in PatentController GetSelectDoctorAppointment: {Message}", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
