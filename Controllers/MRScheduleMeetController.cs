﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyMedicineSupplyPortal.Models;

namespace PharmacyMedicineSupplyPortal.Controllers
{
    public class MRScheduleMeetController : Controller
    {
        string sd = null;
        string se = null;
        public IActionResult EnterDate()
        {
            return View();
        }


        public IActionResult Index(IFormCollection form)
        {
            sd = form["startDate"].ToString();
            se = sd.Replace('-', '/');
            TempData["Date1"] = se;
            return RedirectToAction("MrMeet", "MRScheduleMeet");
        }


        [HttpGet]
        public async Task<IActionResult> MrMeet()
        {
            string startDate = se;
            try
            {

                startDate = TempData["Date1"].ToString();

            }
            catch (System.NullReferenceException e)
            {
                return RedirectToAction("EnterDate", "MRScheduleMeet");
            }
            var MRMeetList = new List<RepSchedule>();
            using (var httpclient = new HttpClient())
            {

                httpclient.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage res = await httpclient.GetAsync("api/ScheduleMeeting?startDate=" + startDate);
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    MRMeetList = JsonConvert.DeserializeObject<List<RepSchedule>>(result);
                }
            }




            return View(MRMeetList);
        }
    }
}


