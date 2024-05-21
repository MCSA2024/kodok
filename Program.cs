using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkHelper;

namespace Kelettravel2024
{
    class Program
    {
        static string host = "http://localhost:3000";
        static List<Celok> celokLista = Backend.GET($"{host}/celok").Send().ToList<Celok>();
        static void Main(string[] args)
        {
            Console.WriteLine($"1. feladat: Az elérhető célok száma: {celokLista.Count()}");
            Console.WriteLine("2. feladat: Az egyszavas célok: ");
            celokLista.Where(x => x.KetSzo() == false).ToList().ForEach(x => Console.WriteLine(x.celok_nev));

            Console.Write("3. feladat: Adj meg keresendő kulcsszót: ");
            string keresendo = Console.ReadLine();

            var talalat = celokLista.Where(x => x.celok_nev.Contains(keresendo)).Select(x => $"{x.celok_nev} {x.celok_kultura_honap}").ToList();
            Console.WriteLine("Találatok:");

            talalat.ForEach(x => Console.WriteLine(x));
            Console.WriteLine($"Találatok száma:{talalat.Count()} db");

            var tartalom = talalat.Prepend("Talalatok:").Append($"Találatok száma: {talalat.Count()}");
            File.WriteAllLines($"{keresendo}.txt", tartalom);

            Console.Write("4. feladat: ");
            celokLista.GroupBy(x => x.celok_kultura_honap).Select(x => new
            {
                honap = x.Key,
                db = x.Count()
            }).OrderByDescending(x => x.db).ToList().ForEach(x => Console.WriteLine($"{x.honap}: {x.db} db"));

            Console.ReadKey();
        }
    }
}
