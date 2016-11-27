using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notown.Data;
using Notown.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Notown.Models;
using Notown.Models.AccountViewModels;
using Notown.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Notown.Data
{
    public class DbInitializer
    {
        public static void InitializeUsers(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if(context.Users.Any())
            {
                return;
            }

            IdentityRole adminRole = new IdentityRole();



            //var userStore = new UserStore<IdentityUser>(context);
            //var userManager = new UserManager<IdentityUser>(userStore);

            //var roleStore = new RoleStore<IdentityRole>(context);
            //var roleManager = new RoleManager<IdentityRole>(roleStore);
        }

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
                    Name = "Piano"
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
                    Ssn = "123456789",
                    Name = "Billy",
                    PlaceID = 1,
                    InstrumentID = 1
                },
                new Musician
                {
                    Ssn = "876543211",
                    Name = "Marge",
                    PlaceID = 2,
                    InstrumentID = 2
                },
                new Musician
                {
                    Ssn = "113456882",
                    Name = "John",
                    PlaceID = 3,
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
                    MusicianID = 1
                },
                new Album
                {
                    ID = 2,
                    Name = "Purple Gorilla",
                    CopyrightDate = DateTime.Parse("2015-11-23"),
                    Speed = 250,
                    MusicianID = 1
                },
                new Album
                {
                    ID = 3,
                    Name = "Red Tape",
                    CopyrightDate = DateTime.Parse("2016-02-04"),
                    Speed = 200,
                    MusicianID = 2
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
                    MusicianID = 1,
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Purple Banana",
                    MusicianID = 1,
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Green Banana",
                    MusicianID = 2,
                    AlbumID = 1
                },
                new Song
                {
                    Title = "Teal Banana",
                    MusicianID = 2,
                    AlbumID = 1
                },
                new Song
                {
                    Title = "I like Red",
                    MusicianID = 3,
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
