using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COMP231_W2020_OnDuty_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using COMP231_W2020_OnDuty_Web.ViewModel;

namespace COMP231_W2020_OnDuty_Web.Controllers
{
	public class HomeController : Controller
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public HomeController(ApplicationDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;

			//test area for changes on the evalutaion:
			/*
			updateRating(4, "John Constantine");
			updateRating(3, "John Constantine");
			updateRating(2, "John Constantine");
			updateRating(1, "John Constantine");
			updateRating(5, "John Constantine");
			*/
		}
		/// <summary>
		/// Index View (User profile)
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// About View
		/// </summary>
		/// <returns></returns>
		public IActionResult About()
		{
			ViewData["Message"] = "OnDuty connects skilled professionals with homeowners, and transforms the way home services are provided.";

			return View();
		}
		/// <summary>
		/// Contact View
		/// </summary>
		/// <returns></returns>
		public IActionResult Contact()
		{
			return View();
		}
		/// <summary>
		/// Privacy View
		/// </summary>
		/// <returns></returns>
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		/// <summary>
		/// Get a list of all available Categories
		/// </summary>
		/// <returns></returns>
		public IActionResult SelectCategory()
		{
			if (ModelState.IsValid)
			{
				List<Category> categories = new List<Category>();

				categories = _context.Categories.GroupBy(x => x.Name)
					.Select(item => item.First())
					.ToList();

				categories.Insert(0, new Category { Id = 0, Name = "All Categories" });
				ViewBag.ListofCategory = categories;
			}
			return View();
		}
		/// <summary>
		/// Get the list of pending Services for the Provider logged in
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public async Task<IActionResult> RequestedServices()
		{
			var user = await _userManager.GetUserAsync(User);
			List<Service> bookings = new List<Service> { };

			if (user.IsProvider)
			{

				bookings = _context.Services.Where(s => s.Provider.UserName == user.UserName && s.Status == "Pending").Include("Customer").ToList();
			}
			else
			{
				bookings = null;
			}

			return View(bookings);
		}
		/// <summary>
		/// Get the list of Providers according to the selected Category
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> ListResult(string Name)
		{
			List<User> providers = new List<User>();

			if (Name == null || Name == "All Categories")
			{
				providers = await _context.Users
					.Where(u => u.IsProvider).ToListAsync();
			}
			else
			{
				providers = await _context.Users
					.Where(provider => provider.Category == Name)
					.ToListAsync();
			}

			var user = await _userManager.GetUserAsync(User);
			List<User> listOfFavoriteProviders;

			if (user != null)
			{
				listOfFavoriteProviders = user.Favorites;
				ViewBag.Favorites = listOfFavoriteProviders.Select(p => p.UserName).ToList();
			}
			else
			{
				ViewBag.Favorites = new List<User>();
			}

			return View(providers);
		}
		/// <summary>
		/// Create a Service object and save in the database
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		[Authorize]
		public async Task<IActionResult> CreateBooking(string Name)
		{
			var user = await _userManager.GetUserAsync(User);

			var provider = await _context.Users.Where(u => u.Name == Name).FirstOrDefaultAsync();
			var customer = await _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefaultAsync();

			Service service = new Service
			{
				Customer = customer,
				Provider = provider,
				Category = provider.Category,
				Status = "Pending"
			};

			_context.Services.Add(service);
			_context.SaveChanges();

			return View("Booking", service);
		}
		/// <summary>
		/// Save the Details property of the Service
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="Details"></param>
		/// <returns></returns>
		public async Task<IActionResult> SaveBookingDetails(int Id, string Details, DateTime DateTime)
		{
			var service = _context.Services.Where(s => s.Id == Id).FirstOrDefault();

			service.Details = Details;
			service.DateTime = DateTime;
			_context.SaveChanges();

			var user = await _userManager.GetUserAsync(User);
			var bookings = _context.Services.Where(s => s.Customer.UserName == user.UserName).Include("Provider").ToList();

			return View("ListOfBookings", bookings);
		}
		/// <summary>
		/// Cancel service
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public async Task<IActionResult> Cancel(int Id, bool ViewRequestedServices)
		{
			var service = _context.Services.Where(s => s.Id == Id).FirstOrDefault();

			if (service != null)
			{
				_context.Remove(service);
				_context.SaveChanges();
			}

			var user = await _userManager.GetUserAsync(User);

			if (ViewRequestedServices)
			{
				var bookings = _context.Services.Where(s => s.Provider.UserName == user.UserName && s.Status == "Pending").Include("Customer").ToList();

				return View("RequestedServices", bookings);
			}
			else
			{
				var bookings = _context.Services.Where(s => s.Customer.UserName == user.UserName).Include("Provider").ToList();

				return View("ListOfBookings", bookings);
			}

		}
		/// <summary>
		/// Change the Status of the Service when it's accepted by the Provider
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public IActionResult Accept(int Id)
		{
			var service = _context.Services.Where(s => s.Id == Id).FirstOrDefault();

			if (service != null)
			{
				service.Status = "Accepted";
				_context.SaveChanges();
			}

			return View();
		}
		/// <summary>
		/// Get to the Customer the list of all booked Services
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> ListOfBookings()
		{
			var user = await _userManager.GetUserAsync(User);
			var bookings = _context.Services.Where(s => s.Customer.UserName == user.UserName).Include("Provider").ToList();

			return View(bookings);
		}
		/// <summary>
		/// Show the list of favorite providers for the current logged user
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> FavoriteProviders()
		{
			var user = await _userManager.GetUserAsync(User);
			var listOfFavoriteProviders = user.Favorites;

			return View(listOfFavoriteProviders);
		}//end of method

		/// <summary>
		/// When deployed, the "Completed Booking" action will call this method
		/// with the rating (integer number) for the service delivered and the
		/// method below handles the calculation and insertion of the rating into 
		/// the provider's profile, to update his/her average grade
		/// </summary>
		/// <param name=""></param>
		public void updateRating(int serviceRating, string Id)
		{
			var userProvider = _context.Users.Where(s => s.UserName == Id).FirstOrDefault();
			userProvider.Evaluation = Math.Round(((userProvider.Evaluation * (double)userProvider.numberEvaluations) / (double)(userProvider.numberEvaluations + 1)),2);
			userProvider.numberEvaluations++;
			_context.SaveChanges();
		}
		/// <summary>
		/// Show the list of providers previously contracted by the logged user
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> ProvidersPreviouslyContracted()
		{
			var user = await _userManager.GetUserAsync(User);
			var listOfProvidersVM = getProviderViewModelList(user);

			ViewBag.FavoriteCount = user.Favorites.Count();

			return View(listOfProvidersVM);
		}
		/// <summary>
		/// Set the selected provider as favorite
		/// </summary>
		/// <param name="providerIndex"></param>
		/// <returns></returns>
		public async Task<IActionResult> setFavorite(int providerIndex)
		{
			var user = await _userManager.GetUserAsync(User);
			var listOfProvidersVM = getProviderViewModelList(user);
			var provider = _context.Users
								.Where(u => u.UserName == listOfProvidersVM.ElementAt(providerIndex).UserName)
								.FirstOrDefault();
			var providerIsFavorite = user.Favorites
										.Where(u => u.UserName == provider.UserName)
										.Any();

			if (providerIsFavorite)
			{
				user.Favorites.Remove(provider);
			}
			else
			{
				user.Favorites.Add(provider);
			}

			_context.SaveChanges();
			listOfProvidersVM = getProviderViewModelList(user); // call the method again so the list gets updated after the db is changed

			ViewBag.FavoriteCount = user.Favorites.Count();

			return View("ProvidersPreviouslyContracted", listOfProvidersVM);
		}
		/// <summary>
		/// return a lis of ProviderViewModel objects
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		List<ProviderViewModel> getProviderViewModelList(User user)
		{
			var listOfServices = _context.Services.Where(s => s.Customer.UserName == user.UserName).Include("Provider").ToList();
			var listOfProviders = listOfServices.Select(s => s.Provider).Distinct().ToList();
			List<ProviderViewModel> listOfProvidersVM = new List<ProviderViewModel>();

			foreach (var provider in listOfProviders)
			{
				ProviderViewModel pvm = new ProviderViewModel
				{
					UserName = provider.UserName,
					Name = provider.Name,
					Category = provider.Category,
					Rate = provider.Rate,
					IsFavorite = false,
				};

				if (user.Favorites.Where(p => p.UserName == provider.UserName).Any())
				{
					pvm.IsFavorite = true;
				}

				listOfProvidersVM.Add(pvm);

			}
			return listOfProvidersVM;
		}				

	}//end of class

}//end of namespace