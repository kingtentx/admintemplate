using AutoMapper;
using King.AdminSite.Models;
using King.Data;
using King.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.AdminSite.Controllers
{
    public class NewsController : AdminBaseController
    {
        private IMapper _mapper;

        public NewsController(ICacheService cache, IPermission permission, IMapper mapper)

        {

            _mapper = mapper;


        }
        public async Task<IActionResult> Index()
        {

            return View();
        }


    }
}
