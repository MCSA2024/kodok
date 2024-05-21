# kodok

package.json

  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1",
    "start": "nodemon backend.js"    
  },



backend.js

const express = require('express')
var cors = require('cors')
var mysql = require('mysql')
const app = express()
const port = 3000

app.use(cors())
app.use(express.json())
app.use(express.urlencoded({extended:true}));
app.use(express.static('kepek'))

var connection

function kapcsolat() {
  connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'kelettravel2024'
  })

  connection.connect()
}



app.get('/', (req, res) => {
  res.send('Hello World!')
})

app.get('/celok', (req, res) => {

  kapcsolat()
  connection.query('SELECT * from celok', function (err, rows, fields) {
    if (err) throw err

    console.log(rows)
    res.send(rows)
  })
  connection.end()
})

app.get('/kapcsolatok', (req, res) => {

  kapcsolat()
  connection.query('SELECT * from kapcsolatfelvetel', function (err, rows, fields) {
    if (err) throw err

    console.log(rows)
    res.send(rows)
  })
  connection.end()
})

app.post('/kapcsolatok', (req, res) => {
const {nev, email, telefon, megjegyzes} = req.body;
console.log(nev,email,telefon, megjegyzes);

  kapcsolat();
  connection.query('INSERT INTO kapcsolatfelvetel (id, nev, email, telefon, megjegyzes) VALUES (NULL, ?, ?, ?, ?)',[nev, email, telefon, megjegyzes], (err, result) => {
    if (err) {
      return res.status(500).json('Hiba');
    }
    res.status(200).json('Sikeres felvitel');
    connection.end();
  });
  
});

app.post('/celok', (req, res) => {
  const searching = req.body.searching;
  console.log(searching);
  
    kapcsolat();
    connection.query('SELECT * FROM celok WHERE celok_nev LIKE ?',[`%${searching}%`], (err, result) => {
      if (err) {
        return res.status(500).json('Hiba');
      }
      res.status(200).json(result);
      connection.end();
    });
    
  });

  app.delete('/kapcsolatok/:id', (req, res) => {
    const id = req.body.id;
    //console.log(id);
    
      kapcsolat();
      connection.query('DELETE FROM kapcsolatfelvetel WHERE id = ?',[id], (err, result) => {
        if (err) {
          return res.status(500).json('Hiba');
        }
        res.status(200).json('Sikeres törlés');
        connection.end();
      });
      
    });

app.listen(port, () => {
  console.log(`Example app listening at http://localhost:${port}`)
})


index.html

<!DOCTYPE html>
<html lang="hu">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Kelet Travel weboldala" />
    <meta name="author" content="Kelet Travel" />

    <link rel="icon" type="image/x-icon" href="assets/img/favicon.ico" />

    <title>Kelet Travel</title>

    <!-- Ikonkészlet -->
    <script src="assets/fa-all.js"></script>
    <!-- Bootrsap CSS -->
    <link href="assets/bootstrap.css" rel="stylesheet" />

    <link href="stilus.css" rel="stylesheet" />
</head>

<body id="page-top" onload="uticelok()">

    <!-- Navigáció -->
    <nav class="navbar navbar-expand-lg bg-dark text-uppercase fixed-top" id="mainNav">
        <div class="container">
            <a class="navbar-brand js-scroll-trigger sajat" href="#page-top">Kelet Travel Utazási Iroda</a>
            <button
                class="navbar-toggler navbar-toggler-right text-uppercase font-weight-bold bg-primary text-white rounded"
                type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive"
                aria-expanded="false" aria-label="Toggle navigation">

                <i class="fas fa-bars"></i>
            </button>
            <div class="collapse navbar-collapse" id="navbarResponsive">

                <ul class="navbar-nav ml-auto">

                    <li class="nav-item"><a class="nav-link rounded js-scroll-trigger" href="#keresendo">
                            Keresés</a>
                    </li>
                    <li class="nav-item"><a class="nav-link rounded js-scroll-trigger"
                            href="#szolgaltatasok">Szolgáltatások</a>
                    </li>
                    <li class="nav-item"><a class="nav-link rounded js-scroll-trigger" href="#uticelok">Úti célok</a>
                    </li>
                    <li class="nav-item "><a class="nav-link rounded js-scroll-trigger" href="#rolunk">
                            Rólunk</a>
                    </li>
                    <li class="nav-item"><a class="nav-link rounded js-scroll-trigger" href="#kapcsolat">
                            Kapcsolat</a>
                    </li>

                </ul>
            </div>
        </div>
    </nav>
    <!-- Fejléc -->
    <header class="masthead bg-primary text-white text-center">
        <div class="container d-flex align-items-center flex-column">
            <!-- Masthead Avatar Image-->
            <img class="masthead-avatar mb-5" src="assets/img/main.png" alt="Logó" />
            <!-- Masthead Heading-->
            <h1 class="masthead-heading text-uppercase mb-0">Úti célok a KELET szerelmeseinek</h1>
            <!-- Icon Divider-->
            <div class="divider-custom divider-light">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>
            <!-- Masthead Subheading-->
            <p class="masthead-subheading font-weight-light mb-0">Felejthetetlen élmények!</p>
        </div>
    </header>

    <section class="page-section bg-dark text-white mb-0" id="kereses">
        <div class="container">

            <div class="control-group">
                <div class="form-group floating-label-form-group controls mb-0 pb-2">
                    <input class="form-control" id="keresendo" type="text" placeholder="írja be a keresendő uticélt" />
                </div>
            </div>

            <div class="form-group">
                <button class="btn btn-primary btn-xl" id="searchingButton" type="button" onclick="kereses()">Keresés
                </button>
            </div>

            <div class="divider-custom divider-light">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>

            <div class="row">
                <div class="col-12" id="talalat">
                  
                </div>
            </div>

        </div>
    </section>

    <!-- Szolgaltatások -->
    <section class="page-section szolgaltatasok" id="szolgaltatasok">
        <div class="container">
            <h2 class="page-section-heading text-center text-uppercase text-dark">Szolgáltatások</h2>
            <div class="divider-custom">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>

            <!-- Szolgaltatáselemek -->
            <div class="row">
                <div class="col-md-4">
                    <div class="szolgaltatasok-item mx-auto" data-toggle="modal" data-target="#szolgaltatasokModal1">
                        <div
                            class="szolgaltatasok-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                            <div class="szolgaltatasok-item-caption-content text-center text-white"><i
                                    class="fas fa-plus fa-3x"></i></div>
                        </div>
                        <img class="img-fluid" src="assets/img/szolgaltatasok/szolg1.jpg" alt="Repülőjárat" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="szolgaltatasok-item mx-auto" data-toggle="modal" data-target="#szolgaltatasokModal2">
                        <div
                            class="szolgaltatasok-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                            <div class="szolgaltatasok-item-caption-content text-center text-white"><i
                                    class="fas fa-plus fa-3x"></i></div>
                        </div>
                        <img class="img-fluid" src="assets/img/szolgaltatasok/szolg2.jpg" alt="Úti célok" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="szolgaltatasok-item mx-auto" data-toggle="modal" data-target="#szolgaltatasokModal3">
                        <div
                            class="szolgaltatasok-item-caption d-flex align-items-center justify-content-center h-100 w-100">
                            <div class="szolgaltatasok-item-caption-content text-center text-white"><i
                                    class="fas fa-plus fa-3x"></i></div>
                        </div>
                        <img class="img-fluid" src="assets/img/szolgaltatasok/szolg3.jpg" alt="Utas biztosítás" />
                    </div>
                </div>

            </div>
        </div>
    </section>
    <!-- Uticelok -->
    <section class="page-section bg-dark text-white mb-0" id="uticelok">
        <div class="container">

            <h2 class="felirat">Úti célok</h2>

            

            <div class="divider-custom divider-light">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>

            <div class="row" id="celok">

            </div>

        </div>
    </section>
    <!-- Rólunk -->
    <section class="page-section bg-primary text-white mb-0" id="rolunk">
        <div class="container">

            <h2 class="felirat">RÓLUNK</h2>

            <div class="divider-custom divider-light">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>

            <div class="row">
                <div class="col-lg-4 ml-auto">
                    <p>
                        Utazási Irodánk 1995-ben lett alapítva.
                    </p>
                </div>
                <div class="col-lg-4 mr-auto">
                    <p>
                        Célunk, hogy az utazás kellemetlenségeit levegyük a válláról.
                    </p>
                </div>

                <div class="col-lg-4 mr-auto">
                    <p>
                        Hihetetlen élményekben lesz része a Távol-Keleti országok beutazásával.
                    </p>
                </div>
            </div>

        </div>
    </section>

    <!-- Kapcsolat -->
    <section class="page-section" id="kapcsolat">
        <div class="container">

            <h2 class="page-section-heading text-center text-uppercase text-dark mb-0">Kapcsolat</h2>

            <div class="divider-custom">
                <div class="divider-custom-line"></div>
                <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                <div class="divider-custom-line"></div>
            </div>

            <p class="lead">Kérjük, töltse ki az alábbi jelentkezési lapot, és egy munkatársunk hamarosan felveszi
                Önnel a kapcsolatot.</p>

            <div class="row">
                <div class="col-lg-8 mx-auto">
                    <form id="contactForm" name="sentMessage">
                        <div class="control-group">
                            <div class="form-group floating-label-form-group controls mb-0 pb-2">
                                <input class="form-control" id="name" type="text" placeholder="Név" />
                            </div>
                        </div>
                        <div class="control-group">
                            <div class="form-group floating-label-form-group controls mb-0 pb-2">
                                <input class="form-control" id="email" type="text" placeholder="E-mail cím" />
                            </div>
                        </div>
                        <div class="control-group">
                            <div class="form-group floating-label-form-group controls mb-0 pb-2">
                                <input class="form-control" id="phone" type="text" placeholder="Telefonszám" />
                            </div>
                        </div>
                        
                        <div class="control-group">
                            <div class="form-group floating-label-form-group controls mb-0 pb-2">
                                <textarea name = "message" id="message" cols="63" rows="2" placeholder="Úticél, vagy egyéb megjegyzés"></textarea>
                            </div>
                        </div>

                        <br />
                        <div class="form-group">
                            <button class="btn btn-primary btn-xl" id="sendMessageButton" type="button" onclick="kapcsolat()">Üzenet küldése
                            </button>

                        </div>
                    </form>
                    <div id="siker"></div>
                </div>
            </div>
        </div>
    </section>
    <!-- Footer-->
    <footer class="footer text-center bg-primary">
        <div class="container">
            <div class="row">

                <div class="col-lg-4 mb-5 mb-lg-0">
                    <h4 class="text-uppercase mb-4">Egyéb elérhetőségeink:</h4>
                    <a class="btn btn-outline-light btn-social mx-1" href="#"><i
                            class="fab fa-fw fa-facebook-f"></i></a>
                    <a class="btn btn-outline-light btn-social mx-1" href="#"><i class="fab fa-fw fa-twitter"></i></a>
                    <a class="btn btn-outline-light btn-social mx-1" href="#"><i
                            class="fab fa-fw fa-linkedin-in"></i></a>
                    <a class="btn btn-outline-light btn-social mx-1" href="#"><i class="fab fa-fw fa-dribbble"></i></a>
                </div>

                <div class="col-lg-8">
                    <h4 class="text-uppercase mb-4">Tudta?</h4>
                    <p class="lead mb-0 text-left">
                        Amíg ezen a weboldalon tartózkodott, összesen
                        <span id="szamlalo">0</span>
                        ember tért haza a Távol-Keleti országokból!
                        <br />
                        A részletekről érdeklődjön a jelentkezési lapon!
                    </p>
                </div>
            </div>
        </div>
    </footer>

    <div class="copyright py-4 text-center text-white">
        <div class="container"><small>Kelet-Travel Utazási Iroda ©2022</small></div>
    </div>

    <div class="scroll-to-top d-lg-none position-fixed">
        <a class="js-scroll-trigger d-block text-center text-white rounded" href="#page-top"><i
                class="fa fa-chevron-up"></i></a>
    </div>

    <!-- Szolgáltatások felugró kártyái-->
    <!-- repülőjegy -->
    <div class="szolgaltatasok-modal modal fade" id="szolgaltatasokModal1" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">

                                <h2 class="szolgaltatasok-modal-title text-dark text-uppercase text-center mb-0">
                                    Repülőjárat foglalás
                                </h2>

                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>

                                <div class="row justify-content-center">
                                    <img class="img-fluid rounded mb-5" src="assets/img/szolgaltatasok/szolg1.jpg"
                                        alt="sz1" />
                                </div>

                                <p>
                                    Kitalálta már az útirányt? Szeretne kényelmesen, idegeskedés nélkül indulni?
                                    Lefoglaljuk repülőjegyét!
                                </p>
                                <button class="btn btn-primary" data-dismiss="modal">
                                    <i class="fas fa-times fa-fw"></i>
                                    Bezár
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- utazás szervezés -->
    <div class="szolgaltatasok-modal modal fade" id="szolgaltatasokModal2" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">

                                <h2 class="szolgaltatasok-modal-title text-dark text-uppercase text-center mb-0">
                                    Utazás szervezés
                                </h2>

                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>

                                <div class="row justify-content-center">
                                    <img class="img-fluid rounded mb-5" src="assets/img/szolgaltatasok/szolg2.jpg"
                                        alt="sz2" />
                                </div>

                                <p>
                                    Kényelmesen akar utazni?
                                    Nem akarja bújni az úti könyveket?
                                    Megszervezzük útját!
                                    Leszervezzük a napi programját.
                                    Elvisszük a látványosságokhoz!

                                </p>
                                <button class="btn btn-primary" data-dismiss="modal">
                                    <i class="fas fa-times fa-fw"></i>
                                    Bezár
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- utas biztosítás -->
    <div class="szolgaltatasok-modal modal fade" id="szolgaltatasokModal3" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true"><i class="fas fa-times"></i></span>
                </button>
                <div class="modal-body">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-lg-8">

                                <h2 class="szolgaltatasok-modal-title text-dark text-uppercase text-center mb-0">
                                    Utas biztosítás
                                </h2>

                                <div class="divider-custom">
                                    <div class="divider-custom-line"></div>
                                    <div class="divider-custom-icon"><i class="fas fa-star"></i></div>
                                    <div class="divider-custom-line"></div>
                                </div>

                                <div class="row justify-content-center">
                                    <img class="img-fluid rounded mb-5" src="assets/img/szolgaltatasok/szolg3.jpg"
                                        alt="sz2" />
                                </div>

                                <p>
                                    Biztonságosan szeretne utazni?

                                    Megkötjük önnek az utas biztosítást!

                                    Elhagyta útlevelét?

                                    Megbetegedett?

                                    Az utasbiztosításban megkötöttek alapján szervezzük az ügyeket!

                                </p>
                                <button class="btn btn-primary" data-dismiss="modal">
                                    <i class="fas fa-times fa-fw"></i>
                                    Bezár
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Bootstrap, és függőségei -->
    <script src="assets/jquery.min.js"></script>
    <script src="assets/bootstrap.bundle.min.js"></script>
    <script src="assets/jquery.easing.min.js"></script>

    <!-- Téma-specifikus szkriptek -->
    <script src="assets/scripts.js"></script>

    <script src="alap.js"></script>

</body>

</html>


alap.js

function kapcsolat() {

    var bemenet = {
        nev: document.getElementById("name").value,
        email: document.getElementById("email").value,
        telefon: document.getElementById("phone").value,
        megjegyzes: document.getElementById("message").value

    }
    fetch("http://localhost:3000/kapcsolatok", {
        method: "POST",
        body: JSON.stringify(bemenet),
        headers: { "Content-type": "application/json; charset=UTF-8" }
    }

    )
        .then(x => x.text())
        .then(y => {
            //alert(y);

            document.getElementById("siker").innerHTML = y;

        });

}

async function uticelok() {
    const res = await fetch("http://localhost:3000/celok");
    const celok = await res.json();
    console.log(celok);

    let celokHTML = "";
    for (const cel of celok) {
        celokHTML += `
        <div class="col-lg-4 ml-auto">
                <img src="http://localhost:3000/${cel.celok_kep}" alt="${cel.celok_nev}" class="img-fluid">
                <p class="alairas">${cel.celok_nev}</p>
        </div>
        `
    }
    document.getElementById("celok").innerHTML = celokHTML;

}

async function kereses() {

    const searching = document.getElementById('keresendo').value;

    const res = await fetch('http://localhost:3000/celok', {
        method: "POST",
        body: JSON.stringify({ searching }),
        headers: { "Content-type": "application/json; charset=UTF-8" }

    });

    const adatok = await res.json();

    let keresesHTML = "<ul>";
    for (const adat of adatok) {
        keresesHTML += `
        <li class="lista">
        <img src="http://localhost:3000/${adat.celok_kep}" style="width: 100px">
        <span class="kephez">${adat.celok_nev}</span>
        </li>
        `
    }

    keresesHTML += "</ul>";
    document.getElementById("talalat").innerHTML = keresesHTML;



}




var counter = 0;
szamlaloStart();

function szamlaloStart() {
    setInterval(() => {
        counter++;
        var szamlaloSzoveg = document.getElementById('szamlalo');
        szamlaloSzoveg.textContent = counter;
    },
        500, window
    );
}


celok.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelettravel2024
{
    class Celok
    {
        public int celok_id { get; set; }
        public string celok_nev { get; set; }
        public string celok_kep { get; set; }
        public string celok_kultura_honap{ get; set; }
        public int celok_orszag { get; set; }


        public bool KetSzo()
        {
            return celok_nev.Contains(" ");
        }
    }

  
}


kapcsolatfelvetel.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkHelper;

namespace Kelettravel2024
{
    class Kapcsolatfelvetel
    {
        public int id { get; set; }
        public string nev { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string megjegyzes { get; set; }

        public bool Hianyos
        {
            get
            {
                if (nev == "" || email == "" || telefon == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

   
}

progmram.cs (Konzolos)

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



Grafikus felépítése:
<Window x:Class="kelet2024.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:kelet2024"
        mc:Ignorable="d"
        Title="KeletTravel" Height="450" Width="800">
    <TabControl>
        <TabItem Header="Úticélok">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"></ColumnDefinition>
                    <ColumnDefinition Width="*"></ColumnDefinition>
                </Grid.ColumnDefinitions>
                <ListBox x:Name="lbCelok" Grid.Column="0" SelectionChanged="lbCelok_SelectionChanged"></ListBox>
                <StackPanel Grid.Column="1">
                    <Label Content="Cél megnevezése" />
                    <Label Content="" x:Name="lblCelNev"/>
                    <Label Content="Kultúrális események jellemző napja" />
                    <Label Content="" x:Name="lblCelHonap" />
                </StackPanel>
            </Grid>
        </TabItem>
        <TabItem Header="Kapcsolatfelvétel">
            <StackPanel>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="100"></ColumnDefinition>
                        <ColumnDefinition Width="*"></ColumnDefinition>
                    </Grid.ColumnDefinitions>
                    <Label Content="Név:" Grid.Column="0"/>
                    <TextBox x:Name="tbNev" Grid.Column="1"/>
                </Grid>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="100"></ColumnDefinition>
                        <ColumnDefinition Width="*"></ColumnDefinition>
                    </Grid.ColumnDefinitions>
                    <Label Content="Email:" Grid.Column="0"/>
                    <TextBox x:Name="tbEmail" Grid.Column="1"/>
                </Grid>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="100"></ColumnDefinition>
                        <ColumnDefinition Width="*"></ColumnDefinition>
                    </Grid.ColumnDefinitions>
                    <Label Content="Telefon:" Grid.Column="0"/>
                    <TextBox x:Name="tbTelefon" Grid.Column="1"/>
                </Grid>
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="100"></ColumnDefinition>
                        <ColumnDefinition Width="*"></ColumnDefinition>
                    </Grid.ColumnDefinitions>
                    <Label Content="Megjegyzés:" Grid.Column="0"/>
                    <TextBox x:Name="tbMegjegyzes" Grid.Column="1"/>
                </Grid>
                <Button x:Name="Felvitel" Content="Kapcsolatfelvétel" Height="25" Click="Felvitel_Click"/>
            </StackPanel>
        </TabItem>
    </TabControl>
</Window>

Garfikus kód:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kelettravel2024;
using NetworkHelper;


namespace kelet2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string host = "http://localhost:3000";
        public MainWindow()
        {
            InitializeComponent();
            lbCelok.ItemsSource = Backend.GET($"{host}/celok").Send().ToList<Celok>();
            lbCelok.DisplayMemberPath = "celok_nev";
        }

        private void lbCelok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Celok cel = lbCelok.SelectedItem as Celok;
            lblCelNev.Content = cel.celok_nev;
            lblCelHonap.Content = cel.celok_kultura_honap;
        }

        private void Felvitel_Click(object sender, RoutedEventArgs e)
        {
            Kapcsolatfelvetel adatok = new Kapcsolatfelvetel()
            {
                nev = tbNev.Text,
                email = tbEmail.Text,
                telefon = tbTelefon.Text,
                megjegyzes = tbMegjegyzes.Text

            };

            if (adatok.Hianyos)
            {
                MessageBox.Show("Minden mező kitöltése kötelező");
            }
            else
            {
                string uzenet = Backend.POST($"{host}/kapcsolatok").Body(adatok).Send().Message;
                MessageBox.Show(uzenet);
            }
        }
    }
}

