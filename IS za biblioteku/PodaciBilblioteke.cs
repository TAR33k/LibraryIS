using IS_za_biblioteku.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_za_biblioteku
{
    public static class PodaciBiblioteke
    {
        public static List<Autor> Autori { get; set; } = new List<Autor>();
        public static List<Zanr> Zanrovi { get; set; } = new List<Zanr>();
        public static List<Knjiga> Knjige { get; set; } = new List<Knjiga>();
        public static List<Korisnik> Korisnici { get; set; } = new List<Korisnik>();
        public static List<Clanarina> Clanarine { get; set; } = new List<Clanarina>();
        public static List<Posudba> Posudbe {  get; set; }=new List<Posudba>();
        public static List<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();


        public static void PopuniPodatke()
        {
            // Dodavanje autora
            Autori.AddRange(new List<Autor>
            {
                new Autor { Id = 1, Ime = "Ivo", Prezime = "Andrić" },
                new Autor { Id = 2, Ime = "Meša", Prezime = "Selimović" },
                new Autor { Id = 3, Ime = "Branko", Prezime = "Ćopić" },
                new Autor { Id = 4, Ime = "Antun", Prezime = "Branko Šimić" },
                new Autor { Id = 5, Ime = "Aleksa", Prezime = "Šantić" }
            });

            // Dodavanje žanrova
            Zanrovi.AddRange(new List<Zanr>
            {
                new Zanr { Id = 1, Naziv = "Roman" },
                new Zanr { Id = 2, Naziv = "Poezija" },
                new Zanr { Id = 3, Naziv = "Drama" },
                new Zanr { Id = 4, Naziv = "Esej" },
                new Zanr { Id = 5, Naziv = "Pripovijetka" }
            });

            // Dodavanje knjiga
            Knjige.AddRange(new List<Knjiga>
            {
                new Knjiga
                {
                    Id = 1,
                    Naslov = "Na Drini ćuprija",
                    Autor = Autori[0],
                    Zanr = Zanrovi[0],
                    DostupnaKolicina = 20,
                    GodinaIzdavanja = 1945,
                    Dostupna = true // Postavljamo atribut Dostupna
                },
                new Knjiga
                {
                    Id = 2,
                    Naslov = "Derviš i smrt",
                    Autor = Autori[1],
                    Zanr = Zanrovi[0],
                    DostupnaKolicina=100,
                    GodinaIzdavanja = 1966,
                    Dostupna = false // Postavljamo atribut Dostupna
                },
                new Knjiga
                {
                    Id = 3,
                    Naslov = "Bašta sljezove boje",
                    Autor = Autori[2],
                    Zanr = Zanrovi[4],
                    DostupnaKolicina = 200,
                    GodinaIzdavanja = 1970,
                    Dostupna = true // Postavljamo atribut Dostupna
                },
                new Knjiga
                {
                    Id = 4,
                    Naslov = "Preobražaj",
                    Autor = Autori[3],
                    Zanr = Zanrovi[2],
                    DostupnaKolicina = 37,
                    GodinaIzdavanja = 1915,
                    Dostupna = true // Postavljamo atribut Dostupna
                },
                new Knjiga
                {
                    Id = 5,
                    Naslov = "Emina",
                    Autor = Autori[4],
                    Zanr = Zanrovi[1],
                    DostupnaKolicina=56,
                    GodinaIzdavanja = 1902,
                    Dostupna = true // Postavljamo atribut Dostupna
                }
            });

            // Dodavanje članarina
            Clanarine.AddRange(new List<Clanarina>
            {
                new Clanarina { Id = 1, Naziv = "Mjesečna članarina", Cijena = 20.00 },
                new Clanarina { Id = 2, Naziv = "Godišnja članarina", Cijena = 100.00 },
                new Clanarina { Id = 3, Naziv = "Studentska članarina", Cijena = 10.00 },
                new Clanarina { Id = 4, Naziv = "Porodična članarina", Cijena = 50.00 },
                new Clanarina { Id = 5, Naziv = "Članarina za penzionere", Cijena = 10.00 }
            });


            // Dodavanje korisnika
            // Dodavanje korisnika sa svim podacima
            Korisnici.AddRange(new List<Korisnik>
            {
                new Korisnik
                {
                    Id = 1,
                    Ime = "Marko",
                    Prezime = "Marković",
                    BrojTelefona = "+38761234567",
                    Email = "marko.markovic@example.com",
                    Clanarina = Clanarine[0], // Mjesečna članarina
                    Aktivni = true,
                    DatumIsteka = DateTime.Now.AddMonths(1),
                    PosudjeneKnjige = new List<Knjiga>
                    {
                        Knjige[0] // Na Drini ćuprija
                    }
                },
                new Korisnik
                {
                    Id = 2,
                    Ime = "Ana",
                    Prezime = "Anić",
                    BrojTelefona = "+38761345678",
                    Email = "ana.anic@example.com",
                    Clanarina = Clanarine[1], // Godišnja članarina
                    Aktivni = false,
                    DatumIsteka = DateTime.Now.AddMonths(-6),
                    PosudjeneKnjige = new List<Knjiga>
                    {
                        Knjige[1] // Derviš i smrt
                    }
                },
                new Korisnik
                {
                    Id = 3,
                    Ime = "Ivan",
                    Prezime = "Ivić",
                    BrojTelefona = "+38761456789",
                    Email = "ivan.ivic@example.com",
                    Clanarina = Clanarine[2], // Studentska članarina
                    Aktivni = true,
                    DatumIsteka = DateTime.Now.AddMonths(3),
                    PosudjeneKnjige = new List<Knjiga>
                    {
                        Knjige[2], // Bašta sljezove boje
                        Knjige[3]  // Preobražaj
                    }
                },
                new Korisnik
                {
                    Id = 4,
                    Ime = "Lana",
                    Prezime = "Lanić",
                    BrojTelefona = "+38761567890",
                    Email = "lana.lanic@example.com",
                    Clanarina = Clanarine[3], // Porodična članarina
                    Aktivni = true,
                    DatumIsteka = DateTime.Now.AddMonths(5),
                    PosudjeneKnjige = new List<Knjiga>()
                },
                new Korisnik
                {
                    Id = 5,
                    Ime = "Petar",
                    Prezime = "Petrović",
                    BrojTelefona = "+38761678901",
                    Email = "petar.petrovic@example.com",
                    Clanarina = Clanarine[4], // Članarina za penzionere
                    Aktivni = true,
                    DatumIsteka = DateTime.Now.AddMonths(12),
                    PosudjeneKnjige = new List<Knjiga>
                    {
                        Knjige[4] // Emina
                    }
                }
            });

            // Dodavanje posudbi
            Posudbe.AddRange(new List<Posudba>
            {
                new Posudba { Korisnik = Korisnici[0], Knjiga = Knjige[0], DatumPosudbe = DateTime.Now.AddDays(-2), DatumVracanja = DateTime.Now.AddDays(5) },
                new Posudba { Korisnik = Korisnici[1], Knjiga = Knjige[1], DatumPosudbe = DateTime.Now.AddDays(-10), DatumVracanja = DateTime.Now.AddDays(-1) }, // Rok je prošao
                new Posudba { Korisnik = Korisnici[2], Knjiga = Knjige[2], DatumPosudbe = DateTime.Now.AddDays(-7), DatumVracanja = DateTime.Now.AddDays(3) },
                new Posudba { Korisnik = Korisnici[3], Knjiga = Knjige[3], DatumPosudbe = DateTime.Now.AddDays(-15), DatumVracanja = DateTime.Now.AddDays(-5) }  // Rok je prošao
            });

            Rezervacije.AddRange(new List<Rezervacija>
            {
                new Rezervacija
                {
                    Id = 1,
                    Korisnik = Korisnici[0],
                    Knjiga = Knjige[1],
                    DatumRezervacije = DateTime.Now.AddDays(-1),
                    Notified = false
                },
                new Rezervacija
                {
                    Id = 2,
                    Korisnik = Korisnici[2],
                    Knjiga = Knjige[3],
                    DatumRezervacije = DateTime.Now.AddDays(-2),
                    Notified = true
                }
            });

            var random = new Random();
            var pastDates = Enumerable.Range(1, 4).Select(x =>
                DateTime.Now.AddDays(-random.Next(30, 365))).ToList();

            foreach (var date in pastDates)
            {
                var knjiga = Knjige[random.Next(Knjige.Count)];
                var posudba = new Posudba
                {
                    Knjiga = knjiga,
                    Korisnik = Korisnici[0],
                    DatumPosudbe = date,
                    DatumVracanja = date.AddDays(14)
                };
                Posudbe.Add(posudba);
            }
        }
        public static int GetNextKnjigaId()
        {
            if (Knjige.Count == 0)
                return 1; // Ako nema knjiga, prvi ID biće 1
            return Knjige.Max(k => k.Id) + 1; // Inače, ID je najveći + 1
        }

        public static int GetNextKorisnikId()
        {
            if(Korisnici.Count == 0) return 1;
            return Korisnici.Max(k => k.Id) + 1;
        }
    }
}
