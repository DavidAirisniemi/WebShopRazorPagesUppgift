using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopUI.DataAccess;
using ShopUI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopUI.Pages
{
    public class IndexModel : PageModel
    {
        public List<Product> _products { get; set; }

        private readonly IDataAccess<Product> _productDataAccess;

        public readonly IDataAccess<Customer> _customerDataAccess;

        public IndexModel(IDataAccess<Product> productDataAccess, IDataAccess<Customer> customerDataAccess)
        {
            _productDataAccess = productDataAccess;
            _customerDataAccess = customerDataAccess;
        }

        public void OnGet()
        {
            //Get all products from JSON
            _products = _productDataAccess.GetAll();
            if (_products == null)
            {
                //If there are no products: create a new list to avoid a null exception
                _products = new List<Product>();
            }
        }
        [BindProperty]
        public int _loginId { get; set; }

        public IActionResult OnPostLogin()
        {
            //Set cookie with the logged in users id
            HttpContext.Session.SetInt32("LoginId", _loginId);

            return Page();
        }
        [BindProperty]
        public int _productId { get; set; }

        public IActionResult OnPostAddToCart()
        {
            //Check cookie if customer is logged in
            if (HttpContext.Session.GetInt32("LoginId") != 0 && HttpContext.Session.GetInt32("LoginId") != null)
            {
                //Get customer from cookie
                Customer customer = _customerDataAccess.GetById((int)HttpContext.Session.GetInt32("LoginId"));
                //Add item to customers cart
                customer._shoppingCart.AddProductToCart(_productDataAccess.GetById(_productId));

                //Seralize to JSON file to save changes
                List<Customer> updateCList = _customerDataAccess.GetAll();
                updateCList[customer._id - 1] = customer;
                _customerDataAccess.Serialize(updateCList);
            }

            return Page();
        }

        [BindProperty]
        public string _searchTerm { get; set; }
        public IActionResult OnPostSearch()
        {
            if (!string.IsNullOrEmpty(_searchTerm))
            {
                //Search for all items in JSON where title contains the searched word
                _products = _productDataAccess.GetAll().
                    Where(p => p._title.ToLower().
                    Contains(_searchTerm.ToLower())).ToList();
            }
            return Page();
        }


        [BindProperty]
        public string _sortTerm { get; set; }
        public IActionResult OnPostSort()
        {
            //Check so that the sort term is not set to the default value
            if (!string.IsNullOrEmpty(_sortTerm))
            {
                //Get the _products list
                OnGet();
                switch (_sortTerm)
                {
                    case "_hPrice":
                        _products = _products.OrderByDescending(o => o._price).ToList();
                        break;

                    case "_lPrice":
                        _products = _products.OrderBy(o => o._price).ToList();
                        break;

                    case "_AZProducts":
                        _products = _products.OrderBy(o => o._title).ToList();
                        break;

                    case "_ZAProducts":
                        _products = _products.OrderByDescending(o => o._title).ToList();
                        break;
                }
            }

            return Page();
        }


        public void OnPostSerialize()
        {
            _products = new List<Product>();
            _products.Add(new Product(1, "https://upload.wikimedia.org/wikipedia/commons/3/3f/Mercury_Globe-MESSENGER_mosaic_centered_at_0degN-0degE.jpg", "Merkurius", "Merkurius (symbol: ☿) är den innersta och minsta planeten i solsystemet, med en omloppstid runt solen av ungefär 88 dygn. På grund av sin närhet till solen är den svår att observera från jorden och kan bara ses i gryningen eller skymningen för blotta ögat eller med en fältkikare. Merkurius har ändå en observationshistoria på åtminstone 3 400 år, eftersom den finns dokumenterad i MUL.APIN, det babyloniska verk som behandlar babylonisk astronomi och astrologi.", 25));
            _products.Add(new Product(2, "https://upload.wikimedia.org/wikipedia/commons/e/e5/Venus-real_color.jpg", "Venus", "Venus (symbol: ♀) är den andra planeten i solsystemet från solen räknat och den är nästan lika stor som jorden. Då Venus rör sig runt sin egen axel i motsatt riktning mot rörelsen runt solen så kan man säga att den roterar baklänges, vilket endast Venus och Uranus gör. Venus är den enda planeten i hela solsystemet vars dygn är längre än dess år eftersom Venus snurrar ett varv runt solen på 225 jorddygn men behöver hela 243 jorddygn för att rotera ett enda varv runt sin egen axel", 53));
            _products.Add(new Product(3, "https://upload.wikimedia.org/wikipedia/commons/thumb/9/97/The_Earth_seen_from_Apollo_17.jpg/1280px-The_Earth_seen_from_Apollo_17.jpg", "Jorden", "Jorden är den tredje planeten från solen och den största av de så kallade stenplaneterna i solsystemet. Jorden är hemvist för alla kända levande varelser, inklusive människan. Dess latinska namn, Tellus eller Terra, används ibland om den, och astronomer betecknar den ibland med symbolen 🜨 (solkors) eller ♁ (riksäpple). Jorden har en naturlig satellit kallad månen, eller Luna på latin. ", 86));
            _products.Add(new Product(4, "https://upload.wikimedia.org/wikipedia/commons/7/76/Mars_Hubble.jpg", "Mars", "Mars (symbol: ♂) är den fjärde planeten från solen och solsystemets näst minsta planet. Den har fått sitt namn efter den romerska krigsguden Mars och kallas ibland för 'den röda planeten' på grund av sitt rödaktiga utseende. Den röda färgen beror på stora mängder järnoxid (rost) som finns fördelat över ytan och i atmosfären. Mars är en av de fyra stenplaneterna och har en tunn atmosfär som till största delen består av koldioxid. Ytan är täckt av kratrar av olika storlekar likt månen, men Mars har precis som jorden även många vulkaner, dalgångar, vidsträckta slätter och iskalotter vid polerna. ", 64));
            _products.Add(new Product(5, "https://upload.wikimedia.org/wikipedia/commons/e/e2/Jupiter.jpg", "Jupiter", "Jupiter (symbol: ♃) är den femte planeten från solen och är med stor marginal solsystemets största planet. Dess massa är 2,5 gånger så stor som alla de andra planeternas sammanlagda massa. Planeten är en så kallad gasjätte och man är inte säker på om planeten ens har en fast kärna. Planeten har fått sitt namn efter den största guden inom romerska mytologin, Jupiter. Fastän namnet är romerskt har planeten varit känd, under andra namn, sedan urminnes tider (till exempel Δίας/Dias på grekiska). ", 21));
            _products.Add(new Product(6, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/Saturn_from_Cassini_Orbiter_%282004-10-06%29.jpg/800px-Saturn_from_Cassini_Orbiter_%282004-10-06%29.jpg", "Saturnus", "Saturnus (symbol: ♄) är den sjätte planeten från solen och den näst största i solsystemet. Den är en gasjätte, känd sedan förhistorisk tid. Galileo Galilei var den första att observera den genom ett teleskop år 1610. Han såg planeten när ringarnas läge fick planeten att se ut som tre planeter i en klump, vilket gjorde dåtidens forskare mycket förbryllade. Saturnus har 95 gånger så stor massa som jorden och har nio gånger så stor diameter.[10] Planeten är namngiven efter den romerska guden Saturnus. t", 64));
            _products.Add(new Product(7, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3d/Uranus2.jpg/1280px-Uranus2.jpg", "Uranus", "Uranus (symbol: ⛢ eller ♅) är den sjunde planeten från solen. Uranus är en av solsystemets fyra jätteplaneter - Jupiter, Saturnus, Uranus och Neptunus - och är av ungefär samma storlek som Neptunus. Planetens diameter är ungefär 50 000 km (ca 4 gånger så stor som jordens) och massan cirka 14,5 jordmassor. Planetens rotationsaxel har en lutning på hela 98 grader, vilket innebär att planeten snarast ”rullar” genom rymden på sin bana runt solen. Det tar 84 år för Uranus att fullborda ett varv runt solen.[2] På grund av Uranus lutning ger det mycket märkliga dygn och år. Under ungefär halva banan får nordpolen hela tiden solljus och det är sommar norr om ekvatorn. Sommaren följs av en 42 år lång vinter med norra halvklotet ständigt i natt eftersom det är vänt bort från solen. ", 52));
            _products.Add(new Product(8, "https://upload.wikimedia.org/wikipedia/commons/6/63/Neptune_-_Voyager_2_%2829347980845%29_flatten_crop.jpg", "Neptunus", "Neptunus (symbol: ♆) är den åttonde planeten från solen. Neptunus är en så kallad gasjätte, och har fått sitt namn efter havsguden Neptunus i romersk mytologi. Den 24 augusti 2006, då Internationella astronomiska unionen beslutade att Pluto inte längre var en planet, blev Neptunus den yttersta planeten i solsystemet. Neptunus var dock den yttersta planeten i solsystemet även mellan åren 1979 och 1999, eftersom Pluto då låg närmare solen än Neptunus. ", 57));
            _productDataAccess.Serialize(_products);



            List<Customer> customer = new List<Customer>();
            customer.Add(new Customer(1, "Tia persson", new List<Order>()));
            customer.Add(new Customer(2, "Per Kallhage", new List<Order>()));
            customer.Add(new Customer(3, "Viktor Byström", new List<Order>()));
            customer.Add(new Customer(4, "Gärdar Ahlander", new List<Order>()));
            customer.Add(new Customer(5, "Julia Tegnér", new List<Order>()));
            _customerDataAccess.Serialize(customer);
        }
    }
}
