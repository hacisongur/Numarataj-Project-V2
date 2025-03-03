using Microsoft.AspNetCore.Mvc;
using Numarataj.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Numarataj.DTO.DTOs.OzelIsyeriDTOs;
using AutoMapper;
using Numarataj.DataAccess.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;

namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]   
    public class OzelIsyeriController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;

        public OzelIsyeriController(NumaratajDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Index action - lists all AdresTespit entries
        [HttpGet]
        [Route("OzelIsyeri/index")]
        public async Task<IActionResult> Index()
        {
            var values = await _context.OzelIsyeri.ToListAsync();
            var resultDtos = _mapper.Map<List<ResultOzelIsyeriDto>>(values);
            return View(resultDtos);
        }
        [HttpGet]
        [Route("OzelIsyeri/Create")]
        public IActionResult CreateOzelIsyeri()
        {
            ViewBag.Mahalleler = Constants.Mahalleler;
            return View();
        }

        [HttpPost]
        [Route("OzelIsyeri/Create")]
        public async Task<IActionResult> CreateOzelIsyeri(CreateOzelIsyeriDto createOzelIsyeriDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createOzelIsyeriDto);
            }

            var ozelIsyeri = _mapper.Map<OzelIsyeri>(createOzelIsyeriDto);
            await _context.OzelIsyeri.AddAsync(ozelIsyeri);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yeni Özel İşyeri Alanı Oluşturuldu";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOzelIsyeri(int belgeNoId)
        {
            var ozelIsyeri = await _context.OzelIsyeri.FirstOrDefaultAsync(x => x.BelgeNoId == belgeNoId);
            if (ozelIsyeri == null)
            {
                return NotFound();
            }

            var updateOzelIsyeriDto = _mapper.Map<UpdateOzelIsyeriDto>(ozelIsyeri);
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateOzelIsyeriDto.Mahalle
            }).ToList();

            return View("UpdateOzelIsyeri", updateOzelIsyeriDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOzelIsyeri(UpdateOzelIsyeriDto updateOzelIsyeriDto)
        {
            if (ModelState.IsValid)
            {
                var ozelIsyeri = await _context.OzelIsyeri.FirstOrDefaultAsync(x => x.BelgeNoId == updateOzelIsyeriDto.BelgeNoId);
                if (ozelIsyeri == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateOzelIsyeriDto, ozelIsyeri);
                _context.OzelIsyeri.Update(ozelIsyeri);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı!";
                return RedirectToAction("Index");
            }

            // Hata almamak için Mahalle listesini tekrar ViewBag'e set ediyoruz
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateOzelIsyeriDto.Mahalle
            }).ToList();

            return View(updateOzelIsyeriDto);
        }



        [HttpPost]
        [Route("OzelIsyeri/Delete/{id:int}")]
        public async Task<IActionResult> DeleteOzelIsyeri(int id)
        {
            var ozelIsyeri = await _context.OzelIsyeri.FindAsync(id);
            if (ozelIsyeri == null)
            {
                return NotFound();
            }

            _context.OzelIsyeri.Remove(ozelIsyeri);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Özel İşyeri başarıyla silindi";
            return RedirectToAction(nameof(Index));
        }
    }
}
