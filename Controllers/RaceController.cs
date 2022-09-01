using CloudinaryDotNet.Actions;

using Microsoft.AspNetCore.Mvc;

using MyProjectRunGroup.Interfaces;
using MyProjectRunGroup.Models;
using MyProjectRunGroup.Repository;
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

        public async Task<IActionResult> Edit(int id)
        {
            Race race = await _raceRepository.GetByIdAsync(id);

            if(race == null)
            {
                return View("error");
            }

            EditRaceViewModel raceViewModel = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                ImageUrl = race.Image,
                AddressId = race.AddressId,
                Address = race.Address,
            };

            return View(raceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceViewModel);
            }

            Race userRace = await _raceRepository.GetByIdAsyncAsNoTracking(id);

            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Cloud not delete photo");
                    return View(raceViewModel);
                }
            }

            ImageUploadResult imageUploadResult = await _photoService.AddPhotoAsync(raceViewModel.Image);

            Race race = new Race
            {
                Id = id,
                Title = raceViewModel.Title,
                Description = raceViewModel.Description,
                Image = imageUploadResult.Url.ToString(),
                AddressId = raceViewModel.AddressId,
                Address = raceViewModel.Address
            };

            _raceRepository.Update(race);

            return RedirectToAction("Index");
        }
    }
}
