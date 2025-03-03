using Microsoft.AspNetCore.Mvc;
using Numarataj.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Numarataj.DTO.DTOs.AdresTespitDTOs;
using AutoMapper;
using Numarataj.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]
    public class AdresTespitController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;

        public AdresTespitController(NumaratajDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Index action - lists all AdresTespit entries
        [HttpGet]
        [Route("AdresTespit/index")]
        public async Task<IActionResult> Index()
        {
            var values = await _context.AdresTespit.ToListAsync();
            var resultDtos = _mapper.Map<List<ResultAdresTespitDto>>(values);
            return View(resultDtos);
        }
        [HttpGet]
        [Route("AdresTespit/Create")]
        public IActionResult CreateAdresTespit()
        {
            ViewBag.Mahalleler = Constants.Mahalleler;
            return View();
        }
        [HttpPost]
        [Route("AdresTespit/Create")]
        public async Task<IActionResult> CreateAdresTespit(CreateAdresTespitDto createAdresTespitDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createAdresTespitDto);
            }

            var adresTespit = _mapper.Map<AdresTespit>(createAdresTespitDto);
            await _context.AdresTespit.AddAsync(adresTespit);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yeni Adres Tespit Alanı Oluşturuldu";
            return RedirectToAction(nameof(Index));
        }

        // Update action - displays the update form for an address
        [HttpGet]
        public async Task<IActionResult> UpdateAdresTespit(int belgeNoId)
        {
            var adresTespit = await _context.AdresTespit.FirstOrDefaultAsync(x => x.BelgeNoId == belgeNoId);
            if (adresTespit == null)
            {
                return NotFound();
            }

            var updateAdresTespitDto = _mapper.Map<UpdateAdresTespitDto>(adresTespit);
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateAdresTespitDto.Mahalle
            }).ToList();

            return View("UpdateAdresTespit", updateAdresTespitDto);
        }

        // Post Update action - handles updating an address
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAdresTespit(UpdateAdresTespitDto updateAdresTespitDto)
        {
            if (ModelState.IsValid)
            {
                var adresTespit = await _context.AdresTespit.FirstOrDefaultAsync(x => x.BelgeNoId == updateAdresTespitDto.BelgeNoId);
                if (adresTespit == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateAdresTespitDto, adresTespit);
                _context.AdresTespit.Update(adresTespit);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı!";
                return RedirectToAction("Index");
            }

            ViewBag.Mahalleler = Constants.Mahalleler;
            return View(updateAdresTespitDto);
        }

        // Delete action - deletes an address
        [HttpPost]
        [Route("AdresTespit/Delete/{id:int}")]
        public async Task<IActionResult> DeleteAdresTespit(int id)
        {
            var adresTespit = await _context.AdresTespit.FindAsync(id);
            if (adresTespit == null)
            {
                return NotFound();
            }

            _context.AdresTespit.Remove(adresTespit);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Adres Tespit başarıyla silindi";
            return RedirectToAction(nameof(Index));
        }
    }
}
