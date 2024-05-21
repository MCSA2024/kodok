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