using CloudinaryDotNet.Actions;

using Microsoft.AspNetCore.Mvc;

using MyProjectRunGroup.Interfaces;
using MyProjectRunGroup.Models;
using MyProjectRunGroup.ViewModel;

namespace MyProjectRunGroup.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult imageUpload = await _photoService.AddPhotoAsync(clubViewModel.Image);

                Club club = new Club
                {
                    Title = clubViewModel.Title,
                    Description = clubViewModel.Description,
                    Address = new Address
                    {
                        Street = clubViewModel.Address.Street,
                        City = clubViewModel.Address.City,
                        State = clubViewModel.Address.State,
                    },
                    Image = imageUpload.Url.ToString(),
                };
                _clubRepository.Add(club);
                
                return RedirectToAction("Index");
            } 
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }

            return View(clubViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);

            if (club == null)
            {
                return View("Error");
            }

            EditClubViewModel clubViewModel = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                ImageUrl = club.Image,
                AddressId = club.AddressId,
                Address = club.Address
            };

            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubViewModel);
            }

            Club userClub = await _clubRepository.GetByIdAsyncAsNoTracking(id);

            if(userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                } 
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Cloud not delete photo");
                    return View(clubViewModel);
                }
            }

            ImageUploadResult imageUploadResult = await _photoService.AddPhotoAsync(clubViewModel.Image);

            Club club = new Club
            {
                Id = id,
                Title = clubViewModel.Title,
                Description = clubViewModel.Description,
                Image = imageUploadResult.Url.ToString(),
                AddressId = clubViewModel.AddressId,
                Address = clubViewModel.Address
            };

            _clubRepository.Update(club);

            return RedirectToAction("Index");
        }
    }
}
