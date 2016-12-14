using InTheLoopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Migrations.Seeds
{
    public static class UserSeed
    {
        public static List<UserLoop> TagUserList(List<int> tagIds)
        {
            var tagUsers = new List<UserLoop>();

            tagIds.ForEach(tagId => tagUsers.Add( new UserLoop { LoopId = tagId }));

            return tagUsers;
        }

        public static void Run(DatabaseContext context)
        {
            var password = "password";

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            var tagNames = new List<String>
            {
                "Music",
                "Night Life",
                "Drink",
                "Food",
                "For Sale",
                "Bar",
                "Educational",
                "College",
                "Arts and Crafts",
                "Sports"
            };

            //tagNames.ForEach(tagName => context.Tags.Add(new Tag { Name = tagName }));
            //context.SaveChanges();

            var tagIds = new List<int>();
            var tagDict = new Dictionary<String, int>();
            tagNames.ForEach(tagName =>
            {
                var tag = context.Loops.SingleOrDefault(t => t.Name == tagName);

                if (tag != null)
                {
                    tagDict.Add(tagName, tag.Id);
                    tagIds.Add(tag.Id);
                }
            });
            

            #region Users
            var users = new List<User>
            {
                new User
                {
                    Email = "chester@gmail.com",
                    UserName = "chester",
                    ImageURL = "chesterprofilepic.jpg",
                    MyLoops = TagUserList(tagIds),
                    EventFooters = new List<EventFooter>
                    {
                        new EventFooter
                        {
                            Description = "Deftones are an American alternative metal band from Sacramento, California. The band, which was founded in 1988, consists of Chino Moreno (lead vocals, rhythm guitar), Stephen Carpenter (lead guitar), Frank Delgado (keyboards and turntables), Abe Cunningham (drums and percussion) and Sergio Vega (bass). ",
                            Title = "Deftones Live At the House of Blues",
                            Website = "www.deftones.com",
                            EventHeaders = new List<EventHeader>
                            {
                                new EventHeader
                                {
                                    City = "New Orleans",
                                    End = DateTime.Now.AddYears(1),
                                    Latitude = 29.953354,
                                    Longitude = -90.0683657,
                                    Start = DateTime.Now,
                                    State = State.LA,
                                    ZipCode = 70131,
                                    ImageURL = "deftones.jpg",
                                    Price = 3,
                                    EventLoops = new List<EventLoop>
                                    {
                                        new EventLoop
                                        {
                                            LoopId = tagDict["Music"] 
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                 new User 
                { 
                    Email = "jpugh457@gmail.com", 
                    UserName = "jake",
                    ImageURL = "jakep.jpg",
                    MyLoops = TagUserList(tagIds),
                    EventFooters = new List<EventFooter>
                    {
                        new EventFooter
                        {
                            Description = "Found out Barrel Proof offers half off their whiskey tasting on Tuesdays",
                            Title = "Whiskey Tasting at Barrel Proof",
                            Website = "http://www.barrelproofnola.com/",
                            EventHeaders = new List<EventHeader>
                            {
                                new EventHeader
                                {
                                    City = "Garden District",
                                    End = DateTime.Now.AddYears(1),
                                    Latitude = 29.9392218,
                                    Longitude = -90.073399,
                                    Start = DateTime.Now.AddHours(5),
                                    State = State.LA,
                                    ZipCode = 70131,
                                    Price = 2,
                                    ImageURL = "barrelproof.jpg",
                                    EventLoops = new List<EventLoop>
                                    {
                                        new EventLoop
                                        {
                                            LoopId =  tagDict["Drink"] 
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new User 
                { 
                    Email = "desireewashington@gmail.com", 
                    UserName = "desiree",
                    ImageURL = "desiree.jpg",
                    MyLoops = TagUserList(tagIds),
                    EventFooters = new List<EventFooter>
                    {
                        new EventFooter
                        {
                            Description = @"We serve the best taco out of a truck!. Seriously this tacos a authentic mexican and are one o
                            of a kind please come check them out.",
                            Title = "La Cocinta Taco Saturday",
                            Website = "http://lacocinitafoodtruck.com/",
                            EventHeaders = new List<EventHeader>
                            {
                                new EventHeader
                                {
                                    City = "Uptown",
                                    End = DateTime.Now.AddYears(1),
                                    Latitude = 29.9245555,
                                    Longitude = -90.0878073,
                                    Start = DateTime.Now,
                                    State = State.LA,
                                    ZipCode = 70131,
                                    Price = 1,
                                    ImageURL = "lacocinta.jpg",
                                    EventLoops = new List<EventLoop>
                                    {
                                        new EventLoop
                                        {
                                            LoopId = tagDict["Food"] 
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new User 
                { 
                    Email = "steezyknees9@gmail.com", 
                    UserName = "patrick",
                    ImageURL = "patrick.jpg",
                    MyLoops = TagUserList(tagIds),
                    EventFooters = new List<EventFooter>
                    {
                        new EventFooter
                        {
                            Description = "Some items I have for sale include, 50 inch flat screen, leather couch, 2 road bikes, kitchen set, flower pot, swing set",
                            Title = "Mid City Garage Sale",
                            EventHeaders = new List<EventHeader>
                            {
                                new EventHeader
                                {
                                    City = "Mid City",
                                    End = DateTime.Now.AddYears(1),
                                    Latitude = 29.972715,
                                    Longitude = -90.0883457,
                                    Start = DateTime.Now,
                                    State = State.LA,
                                    ZipCode = 70131,
                                    Price = 1,
                                    ImageURL = "garagesale.jpeg",
                                    EventLoops = new List<EventLoop>
                                    {
                                        new EventLoop
                                        {
                                            LoopId = tagDict["For Sale"] 
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new User
                {
                    Email = "republic@gmail.com",
                    UserName = "RepublicNOLA",
                    ImageURL = "republic.jpg",
                    MyLoops = TagUserList(tagIds),
                    EventFooters = new List<EventFooter>
                    {
                        new EventFooter
                        {
                            Description = "Don’t miss the annual HOLIDAY BOUNCE featuring Big Freedia, DJ Jubilee, Walt Wiggady, Lucky Lou, Deedie Phat, DJ Lil Man and more on Friday December 18th! Under the twerk tree in our wobble winter wonderland, this stacked line up of greats will put on the bounce show of the year! Start your holiday shopping early and get discount tickets for all your friends for just $5 now.",
                            Title = "BOUNCE: HOLIDAY EDITION FT BIG FREEDIA & DJ JUBILEE",
                            EventHeaders = new List<EventHeader>
                            {
                                new EventHeader
                                {
                                    City = "New Orleans",
                                    End = DateTime.Now.AddYears(1),
                                    Latitude = 29.9440558,
                                    Longitude = -90.0678621,
                                    Start = DateTime.Now,
                                    State = State.LA,
                                    ZipCode = 70131,
                                    Price = 2,
                                    ImageURL = "bounce.jpg",
                                    EventLoops = new List<EventLoop>
                                    {
                                        new EventLoop
                                        {
                                            LoopId = tagDict["Music"] 
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            };
            #endregion

            foreach (var user in users)
            {
                if (!context.Users.Any(x => x.Email == user.Email))
                    userManager.Create(user, password);
            }
        }
    }

    
}