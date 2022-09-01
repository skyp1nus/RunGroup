using CloudinaryDotNet.Actions;

using Microsoft.AspNetCore.Mvc;

using MyProjectRunGroup.Interfaces;
using MyProjectRunGroup.Models;
using MyProjectRunGroup.ViewModel;

namespace MyProjectRunGroup.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult imageUpload = await _photoService.AddPhotoAsync(raceViewModel.Image);

                Race race = new Race
                {
                    Title = raceViewModel.Title,
                    Description = raceViewModel.Description,
                    Address = new Address
                    {
                        Street = raceViewModel.Address.Street,
                        City = raceViewModel.Address.City,
                        State = raceViewModel.Address.State,
                    },
                    Image = imageUpload.Url.ToString(),
                };
                _raceRepository.Add(race);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }

            return View(raceViewModel);
        }
    }
}
