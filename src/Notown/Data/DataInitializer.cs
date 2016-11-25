using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notown.Data;
using Notown.Models;

namespace Notown.Data
{
    public class DbInitializer
    {
        public static void Initialize(NotownContext context)
        {
            context.Database.EnsureCreated();

            // Don't seed if already seeded.
            if (context.Musician.Any())
            {
                return;
            }

            var telephones = new Telephone[]
            {
                new Telephone
                {
                    Number = "2176323555"
                },
                new Telephone
                {
                    Number = "2178992219"
                },
                new Telephone
                {
                    Number = "9017238819"
                }
            };

            foreach (var number in telephones)
            {
                context.Telephone.Add(number);
            }

            context.SaveChanges();

            var places = new Place[]
            {
                new Place
                {
                    Address = "123 Test Street",
                    TelephoneNumber = "2176323555"
                },
                new Place
                {
                    Address = "456 Adams Avenue",
                    TelephoneNumber = "2178992219"
                },
                new Place
                {
                    Address = "101 East Montvale Road",
                    TelephoneNumber = "9017238819"
                }
            };

            foreach (var place in places)
            {
                context.Place.Add(place);
            }

            context.SaveChanges();

            var instruments = new Instrument[]
            {
                new Instrument
                {
                    ID = 1,
                    Key = "C",
                    Name = "Pinao"
                },
                new Instrument
                {
                    ID = 2,
                    Key = "D",
                    Name = "Guitar"
                },
                new Instrument
                {
                    ID = 3,
                    Key = "C",
                    Name = "Violin"
                }
            };

            foreach (var instrument in instruments)
            {
                context.Instrument.Add(instrument);
            }

            context.SaveChanges();

            var musicians = new Musician[]
            {
                new Musician
                {
                    Ssn = "12345678",
                    Name = "Billy",
                    PlaceAddress = "123 Test Street",
                    InstrumentID = 1
                },
                new Musician
                {
                    Ssn = "87654321",
                    Name = "Marge",
                    PlaceAddress = "456 Adams Avenue",
                    InstrumentID = 2
                },
                new Musician
                {
                    Ssn = "11345688",
                    Name = "John",
                    PlaceAddress = "101 East Montvale Road",
                    InstrumentID = 3
                }
            };

            foreach (var artist in musicians)
            {
                context.Musician.Add(artist);
            }

            context.SaveChanges();

            var albums = new Album[]
            {
                new Album
                {
                    ID = 1,
                    Name = "Rocky Road",
                    CopyrightDate = DateTime.Parse("2005-09-01"),
                    Speed = 200,
                    MusicianSsn = "12345678"
                },
                new Album
                {
                    ID = 2,
                    Name = "Purple Gorilla",
                    CopyrightDate = DateTime.Parse("2015-11-23"),
                    Speed = 250,
                    MusicianSsn = "12345678"
                },
                new Album
                {
                    ID = 3,
                    Name = "Red Tape",
                    CopyrightDate = DateTime.Parse("2016-02-04"),
                    Speed = 200,
                    MusicianSsn = "87654321"
                }
            };

            foreach (var album in albums)
            {
                context.Album.Add(album);
            }

            context.SaveChanges();

            var songs = new Song[]
            {
                new Song
                {
                    Title = "Yellow Banana",
                    MusicianSsn = "12345678",
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Purple Banana",
                    MusicianSsn = "12345678",
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Green Banana",
                    MusicianSsn = "11345688",
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Teal Banana",
                    MusicianSsn = "11345688",
                    AlbumID = 1
                },
                new Song
                {
                    Title = "I like Red",
                    MusicianSsn = "87654321",
                    AlbumID = 3
                }
            };

            foreach (var song in songs)
            {
                context.Song.Add(song);
            }

            context.SaveChanges();
        }
    }
}
