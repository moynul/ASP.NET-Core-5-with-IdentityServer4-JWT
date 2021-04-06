using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorDetailsController : ControllerBase
    {


        [Authorize(Policy = "IsAdmin")]
        [HttpGet("admindoctor")]
        public ActionResult admindoctor()
        {
            return Ok(new[]
            {
                new { Id = 1, Name = "Dr.Zinia Zara",   speciality = "M.D. of Medicine",  MedicalColleg = "Netherland Medical College" },
                new { Id = 2, Name = "Dr. Md. Sadekur Rahman", speciality = "Medicine Specialist", MedicalColleg = "Parkview Medical College & Hospital, Sylhet" },
                new { Id = 3, Name = "Dr. Debashish Kumar Ghosh ", speciality = "MBBS, BCS (Health), FCPS (Medicine), MRCP (UK), MD (Endocrinology)", MedicalColleg = "Khulna Medical College & Hospital" }
            });
        }

        [Authorize(Policy = "IsUser")]
        [HttpGet("User")]
        public ActionResult user()
        {
            return Ok(new[]
            {
                new { Id = 1, Name = "Moynul Biswas" },
                new { Id = 2, Name = "Bayzid" },
                new { Id = 3, Name = "Bappy" }
            });
        }


        [Authorize(Policy = "ApiScope")]
        [HttpGet("publicsecure")]
        public ActionResult publicsecure()
        {
            return Ok("This is secure data");
        }
    }
}
