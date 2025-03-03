using Microsoft.AspNetCore.Mvc;
using Numarataj.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Numarataj.DTO.DTOs.YeniBinaDtos;
using AutoMapper;
using Numarataj.DataAccess.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;

namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]
    public class YeniBinaController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;

        public YeniBinaController(NumaratajDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("YeniBina/index")]
        public async Task<IActionResult> Index()
        {
            var values = await _context.YeniBina.ToListAsync();
            var resultDtos = _mapper.Map<List<ResultYeniBinaDto>>(values);
            return View(resultDtos);
        }
        [HttpGet]
        [Route("YeniBina/Create")]
        public IActionResult CreateYeniBina()
        {
            ViewBag.Mahalleler = Constants.Mahalleler;
            return View();
        }

        [HttpPost]
        [Route("YeniBina/Create")]
        public async Task<IActionResult> CreateYeniBina(CreateYeniBinaDto createYeniBinaDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createYeniBinaDto);
            }

            var yeniBina = _mapper.Map<YeniBina>(createYeniBinaDto);
            await _context.YeniBina.AddAsync(yeniBina);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yeni Bina Alanı Oluşturuldu";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateYeniBina(int belgeNoId)
        {
            var yeniBina = await _context.YeniBina.FirstOrDefaultAsync(x => x.BelgeNoId == belgeNoId);
            if (yeniBina == null)
            {
                return NotFound();
            }

            var updateYeniBinaDto = _mapper.Map<UpdateYeniBinaDto>(yeniBina);
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateYeniBinaDto.Mahalle
            }).ToList();

            return View("UpdateYeniBina", updateYeniBinaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateYeniBina(UpdateYeniBinaDto updateYeniBinaDto)
        {
            if (ModelState.IsValid)
            {
                var yeniBina = await _context.YeniBina.FirstOrDefaultAsync(x => x.BelgeNoId == updateYeniBinaDto.BelgeNoId);
                if (yeniBina == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateYeniBinaDto, yeniBina);
                _context.YeniBina.Update(yeniBina);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı!";
                return RedirectToAction("Index");
            }

            ViewBag.Mahalleler = Constants.Mahalleler;
            return View(updateYeniBinaDto);
        }


        [HttpPost]
        [Route("YeniBina/Delete/{id:int}")]
        public async Task<IActionResult> DeleteYeniBina(int id)
        {
            var yeniBina = await _context.YeniBina.FindAsync(id);
            if (yeniBina == null)
            {
                return NotFound();
            }

            _context.YeniBina.Remove(yeniBina);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Yeni Bina başarıyla silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
