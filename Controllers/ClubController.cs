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
    }
}
