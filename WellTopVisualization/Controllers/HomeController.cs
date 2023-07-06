using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WellTopVisualization.Models;

namespace WellTopVisualization.Controllers
{
    public class HomeController : Controller
    {
        private readonly WellTopsVisualizationContext _context;

        public HomeController(WellTopsVisualizationContext context)
        {
            _context = context;
        }

        // HomeController sınıfı, Controller sınıfından kalıtım alır ve WellTopsVisualizationContext nesnesini enjekte eder.

        [HttpGet]
        public async Task<ActionResult<List<Guess>>> getGuesses()
        {
            // Guesses tablosundaki tüm verileri alarak ActionResult<List<Guess>> türünde bir sonuç döndürür
            // _context üzerinden Guesses tablosuna erişir ve ToListAsync() ile tüm verileri alır
            // Ok() metoduyla bu verileri döndürür


            return Ok(await _context.Guesses.ToListAsync());
        }
        public IActionResult Index()
        {
            /*
            //Guesses tablosunu "id" sütunu bazında tersten sırala
            //en son kaydı al
            //kayıt yoksa null
            var lastWin = _context.Guesses.OrderByDescending(g = g.Id).FirstOrDefault();
            if (lastWin != null)
            {
                ViewData["LastTotalWin"] = lastWin.TotalGuess;

            }
            else
            {
                ViewData["LastTotalWin"] = 0;
            }
            */         
            List<Guess> guesses = _context.Guesses.ToList();
            //Guesses tablosundaki tüm Guess verilerini alır ve guesses listesine atar.


            //Pass the retrieved data to the view
            ViewData["AllData"] = guesses;
            //ViewData üzerinden "AllData" anahtarına guesses listesini ekler.



          /*  if ( _context.Guesses.Count() > 0 ) {

                //Guesses tablosunda veri varsa, ViewData üzerinden "maxTotal" anahtarına en yüksek TotalWin değerini ekler.
                ViewData["maxTotal"] = _context.Guesses.Max(p => p.TotalWin);
            }*/


            return View(guesses);
        }

        [HttpPost]
        [ActionName("Index")]

        public IActionResult getInput()
        {
            //Guess nesnesi oluşturur
            Guess guess = new Guess();
          
            Random random = new Random(); 
            //0 ile 10 arasında random bir sayı oluşturur
            int rand = random.Next(0, 10);
            ViewData["randomNumber"] = rand;
            int num = Int16.Parse(HttpContext.Request.Form["getNum"]);
            //Formdan alınan "getNum" değerini parse ederek num'a atar.

            //kullanıcıdan alınan number
            int diff = Math.Abs(num - rand);
            guess.Difference = diff;
           
            guess.TakenNumber = num;


            if (num == rand)
            {
                ViewData["input"] = "Kazandınız";
                //Kullanıcının tahmin ettiği sayı, random sayıya eşitse ViewData üzerinden "input" anahtarına "Kazandınız" değerini atar.

                if (_context.Guesses.Count() > 0)
                {
                    ViewData["maxTotal"] = _context.Guesses.Max(p => p.TotalWin) + 1;
                    guess.TotalWin = _context.Guesses.Max(p => p.TotalWin) + 1;
                    //Guesses tablosunda veri varsa, ViewData üzerinden "maxTotal" anahtarına en yüksek TotalWin değerine 1 ekler ve guess.TotalWin'e de bu değeri atar.

                }

            }
            else
            {
                ViewData["input"] = "Tekrar Deneyiniz";
                // Kullanıcının tahmin ettiği sayı, rastgele sayıya eşit değilse ViewData üzerinden "input" anahtarına "Tekrar Deneyiniz" değerini atar.

                if (_context.Guesses.Count() > 0)
                {
                    guess.TotalWin = _context.Guesses.Max(p => p.TotalWin);
                    ViewData["maxTotal"] = _context.Guesses.Max(p => p.TotalWin);
                    //Guesses tablosunda veri varsa, ViewData üzerinden "maxTotal" anahtarına en yüksek TotalWin değerini atar.
                }
            }

            _context.Guesses.Add(guess);
            _context.SaveChanges();
            //Oluşturulan guess nesnesini Guesses tablosuna ekler ve değişiklikleri kaydeder.

            var all = _context.Guesses.ToList();
            
            ViewData["AllData"] = all;
            //Tüm Guess verilerini alır ve ViewData üzerinden AllData'ya atar.

            return View(all);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}