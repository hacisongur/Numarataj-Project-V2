using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Numarataj.DataAccess.Context;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;
using Numarataj.WebUI.Helpers;
using Numarataj.DTO.DTOs.ResmiKurumDTOs;
namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]   
    public class ResmiKurumController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;

        public ResmiKurumController(NumaratajDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("ResmiKurum/index")]
        public async Task<IActionResult> Index()
        {
            var values = await _context.ResmiKurum.ToListAsync();
            var resultDtos = _mapper.Map<List<ResultResmiKurumDto>>(values);
            return View(resultDtos);
        }
        [HttpGet]
        [Route("ResmiKurum/Create")]
        public IActionResult CreateResmiKurum()
        {
            ViewBag.Mahalleler = Constants.Mahalleler;
            return View();
        }

        [HttpPost]
        [Route("ResmiKurum/Create")]
        public async Task<IActionResult> CreateResmiKurum(CreateResmiKurumDto createResmiKurumDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createResmiKurumDto);
            }

            var resmiKurum = _mapper.Map<ResmiKurum>(createResmiKurumDto);
            await _context.ResmiKurum.AddAsync(resmiKurum);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Yeni Resmi Kurum Alanı Oluşturuldu";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateResmiKurum(int belgeNoId)
        {
            var resmiKurum = await _context.ResmiKurum.FirstOrDefaultAsync(x => x.BelgeNoId == belgeNoId);
            if (resmiKurum == null)
            {
                return NotFound();
            }

            var updateResmiKurumDto = _mapper.Map<UpdateResmiKurumDto>(resmiKurum);
            ViewBag.MahalleListesi = Constants.Mahalleler.Select(m => new SelectListItem
            {
                Text = m,
                Value = m,
                Selected = m == updateResmiKurumDto.Mahalle
            }).ToList();

            return View("UpdateResmiKurum", updateResmiKurumDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateResmiKurum(UpdateResmiKurumDto updateResmiKurumDto)
        {
            if (ModelState.IsValid)
            {
                var resmiKurum = await _context.ResmiKurum.FirstOrDefaultAsync(x => x.BelgeNoId == updateResmiKurumDto.BelgeNoId);
                if (resmiKurum == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateResmiKurumDto, resmiKurum);
                _context.ResmiKurum.Update(resmiKurum);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Güncelleme işlemi başarılı!";
                return RedirectToAction("Index");
            }

            ViewBag.Mahalleler = Constants.Mahalleler;
            return View(updateResmiKurumDto);
        }


        [HttpPost]
        [Route("ResmiKurum/Delete/{id:int}")]
        public async Task<IActionResult> DeleteResmiKurum(int id)
        {
            var resmiKurum = await _context.ResmiKurum.FindAsync(id);
            if (resmiKurum == null)
            {
                return NotFound();
            }

            _context.ResmiKurum.Remove(resmiKurum);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Resmi Kurum başarıyla silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
