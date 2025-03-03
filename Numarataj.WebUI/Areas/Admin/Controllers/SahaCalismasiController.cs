using Microsoft.AspNetCore.Mvc;
using Numarataj.WebUI.Helpers;
using Numarataj.DTO.DTOs.SahaCalismasiDTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Numarataj.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]  
    public class SahaCalismasiController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;

        public SahaCalismasiController(NumaratajDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("SahaCalismasi/index")]
        public async Task<IActionResult> Index()
        {
            var values = await _context.SahaCalismasi.ToListAsync();
            var resultDtos = _mapper.Map<List<ResultSahaCalismasiDto>>(values);
            return View(resultDtos);
        }
        [HttpGet]
        [Route("SahaCalismasi/Create")]
        public IActionResult CreateSahaCalismasi()
        {
            ViewBag.Mahalleler = Constants.Mahalleler;
            return View();
        }

        [HttpPost]
        [Route("SahaCalismasi/Create")]
        public async Task<IActionResult> CreateSahaCalismasi(CreateSahaCalismasiDto createSahaCalismasiDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createSahaCalismasiDto);
            }

            var sahaCalismasi = _mapper.Map<SahaCalismasi>(createSahaCalismasiDto);
            await _context.SahaCalismasi.AddAsync(sahaCalismasi);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yeni Saha Çalışma Alanı Oluşturuldu";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSahaCalismasi(int belgeNoId)
        {
            var sahaCalismasi = await _context.SahaCalismasi.FirstOrDefaultAsync(x => x.BelgeNoId == belgeNoId);
            if (sahaCalismasi == null)
            {
                return NotFound();
            }

            var updateSahaCalismasiDto = _mapper.Map<UpdateSahaCalismasiDto>(sahaCalismasi);
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateSahaCalismasiDto.Mahalle
            }).ToList();

            return View("UpdateSahaCalismasi", updateSahaCalismasiDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSahaCalismasi(UpdateSahaCalismasiDto updateSahaCalismasiDto)
        {
            if (ModelState.IsValid)
            {
                var sahaCalismasi = await _context.SahaCalismasi.FirstOrDefaultAsync(x => x.BelgeNoId == updateSahaCalismasiDto.BelgeNoId);
                if (sahaCalismasi == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateSahaCalismasiDto, sahaCalismasi);
                _context.SahaCalismasi.Update(sahaCalismasi);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı!";
                return RedirectToAction("Index");
            }

            ViewBag.Mahalleler = Constants.Mahalleler;
            return View(updateSahaCalismasiDto);
        }


        [HttpPost]
        [Route("SahaCalismasi/Delete/{id:int}")]
        public async Task<IActionResult> DeleteSahaCalismasi(int id)
        {
            var sahaCalismasi = await _context.SahaCalismasi.FindAsync(id);
            if (sahaCalismasi == null)
            {
                return NotFound();
            }

            _context.SahaCalismasi.Remove(sahaCalismasi);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Saha Çalışması başarıyla silindi";
            return RedirectToAction(nameof(Index));
        }


    }
}
