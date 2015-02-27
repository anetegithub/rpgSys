using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace rpgSys
{
    public static class xmlBase
    {        
        public static class Users
        {
            private static string path = HttpContext.Current.Server.MapPath("~/Data/User/Users.xml");

            public static List<User> Get(int id = 0)
            {
                List<User> list = new List<User>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    User f = new User();
                    f.Id = Convert.ToInt32(el.Attribute("Id").Value);
                    f.Login = el.Element("Login").Value;
                    f.Password = el.Element("Password").Value;
                    if (id == 0)
                    {
                        list.Add(f);
                    }
                    else
                    {
                        if (f.Id == id)
                            list.Add(f);
                    }

                }
                return list;
            }

            public static bool Authed(string UserId, string Auth)
            {
                Boolean result = false;
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == UserId)
                    {
                        if (el.Element("Auth").Value == Auth)
                        {
                            result = true;
                        }
                        else
                            result = false;
                    }
                }
                return result;
            }

            public static List<User> GetByName(string Name, string Password)
            {
                List<User> list = new List<User>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (Name == el.Element("Login").Value)
                    {
                        if (Password == el.Element("Password").Value)
                        {
                            User f = new User();
                            f.Id = Convert.ToInt32(el.Attribute("Id").Value);
                            f.HeroId = Convert.ToInt32(el.Attribute("HeroId").Value);
                            f.Login = el.Element("Login").Value;
                            f.Password = el.Element("Password").Value;
                            f.Auth = el.Element("Auth").Value;
                            list.Add(f);
                        }
                    }
                }
                if (list.Count == 0)
                    list.Add(null);
                return list;
            }
        }

        public static class Modules
        {
            private static string path = HttpContext.Current.Server.MapPath("~/Data/Server/Modules.xml");

            public static List<Module> Get()
            {
                List<Module> list = new List<Module>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    Module m = new Module();
                    m.Id = Convert.ToInt32(el.Attribute("Id").Value);
                    m.Name = el.Element("Name").Value;
                    m.InnerName = el.Element("InnerName").Value;
                    m.Info = el.Element("Info").Value;
                    m.Version = Convert.ToDouble(el.Element("Version").Value);
                    m.Dllinfo = el.Element("Dllinfo").Value;
                    list.Add(m);
                }
                return list;
            }

            public static class UserActivityes
            {
                public static List<UserActivity> Get(string UserId)
                {
                    try
                    {
                        UserActivity ua = new UserActivity();
                        XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Modules/UserActivity.xml"));
                        foreach (XElement el in doc.Elements())
                        {
                            if (el.Attribute("UserId").Value == UserId)
                            {
                                ua.Activityes = new List<Activity>();
                                ua.Id = Convert.ToInt32(el.Attribute("Id").Value);
                                ua.UserId = Convert.ToInt32(el.Attribute("UserId").Value);
                                Activity a = new Activity();
                                foreach (XElement Activ in el.Elements())
                                {
                                    ua.Activityes.Add(
                                        new Activity()
                                        {
                                            Icon = Activ.Element("Icon").Value,
                                            Info = Activ.Element("Info").Value,
                                            Stamp = Activ.Element("Stamp").Value
                                        });
                                }
                            }
                        }
                        return new List<UserActivity>() { ua };
                    }
                    catch { return null; }
                }
            }
        }

        public static class Scenarios
        {
            private static string path = HttpContext.Current.Server.MapPath("~/Data/Scenario/Scenarios.xml");

            public static List<Scenario> Get()
            {
                List<Scenario> list = new List<Scenario>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    list.Add(new Scenario());
                }
                return list;
            }

            public static Scenario ScById(int Id)
            {
                Scenario sc = new Scenario();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == Id.ToString())
                    {
                        sc.Title = el.Element("Title").Value;
                        sc.Fable = el.Element("Fable").Value;
                        sc.Recomendation = el.Element("Recomendation").Value;
                        foreach (XElement l in el.Elements("Locations"))
                        {
                            sc.Locations.Add(new Location()
                            {
                                Id = Convert.ToInt32(l.Attribute("Id").Value),
                                Description = l.Element("Description").Value,
                                Map = l.Element("Map").Value,
                                Name = l.Element("Name").Value,
                                Specification = l.Element("Specification").Value
                            });
                        }
                        foreach (XElement n in el.Elements("Npcs"))
                        {
                            List<Stat> Stts = new List<Stat>();
                            foreach (XElement s in n.Elements("Stats"))
                            {
                                Stts.Add(new Stat()
                                {
                                    Name = s.Element("Name").Value,
                                    Bonus = s.Element("Bonus").Value,
                                    Info = s.Element("Info").Value,
                                    Value = s.Element("Value").Value
                                });
                            }
                            //sc.Npcs.Add(new Npc()
                            //{
                            //    Id = Convert.ToInt32(n.Attribute("Id").Value),
                            //    Name = n.Element("Name").Value,
                            //    Specification = n.Element("Specification").Value,
                            //    View = n.Element("View").Value,
                            //    Stats = Stts
                            //});
                        }
                        foreach (XElement l in el.Elements("Events"))
                        {
                            sc.Events.Add(new Event()
                            {
                                Id = Convert.ToInt32(l.Attribute("Id").Value),
                                Description = l.Element("Description").Value,
                                Title = l.Element("Title").Value,
                            });
                        }
                        foreach (XElement n in el.Elements("Rewards"))
                        {
                            List<Stat> Stts = new List<Stat>();
                            foreach (XElement s in n.Elements("Characteristics"))
                            {
                                Stts.Add(new Stat()
                                {
                                    Name = s.Element("Name").Value,
                                    Bonus = s.Element("Bonus").Value,
                                    Info = s.Element("Info").Value,
                                    Value = s.Element("Value").Value
                                });
                            }
                            //sc.Rewards.Add(new Item()
                            //{
                            //    Id = Convert.ToInt32(n.Attribute("Id").Value),
                            //    Name = n.Element("Name").Value,
                            //    Additional = n.Element("Additional").Value,
                            //    Rare = n.Element("Rare").Value,
                            //    Who = n.Element("Who").Value,
                            //    Characteristics = Stts
                            //});
                        }
                    }
                }
                return sc;
            }

            /// <summary>
            /// Only copy-paste :'(
            /// </summary>
            public static Scenario New
            {
                set
                {
                    int i = 0;
                    Scenario S = value;
                    XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Scenario/Scenarios.xml"));
                    int maxId = doc.Root.Elements("Scenario").Max(t => Int32.Parse(t.Attribute("Id").Value));
                    XElement newS = new XElement("Scenario", new XAttribute("Id", ++maxId));
                    newS.Add(new XElement("Title", S.Title));
                    newS.Add(new XElement("Recomendation", S.Recomendation));
                    newS.Add(new XElement("Fable", S.Fable));

                    newS.Add(new XElement("Locations"));
                    foreach (Location l in S.Locations)
                    {
                        XElement Location = new XElement("Location");
                        Location.Add(new XAttribute("Id", i.ToString()));
                        Location.Add(new XElement("Name", l.Name));
                        Location.Add(new XElement("Description", l.Description));
                        Location.Add(new XElement("Specification", l.Specification));
                        Location.Add(new XElement("Map", l.Map));
                        newS.Element("Locations").Add(new XElement(Location));
                        i++;
                    }
                    i = 0;

                    newS.Add(new XElement("Npcs"));
                    foreach (Npc n in S.Npcs)
                    {
                        XElement Npc = new XElement("Npc");
                        Npc.Add(new XAttribute("Id", i.ToString()));
                        Npc.Add(new XElement("Name", n.Name));
                        Npc.Add(new XElement("View", n.View));
                        Npc.Add(new XElement("Specification", n.Specification));
                        XElement Stats = new XElement("Stats");
                        //foreach (Stat s in n.Stats)
                        //{
                        //    XElement Stat = new XElement("Stat");
                        //    Stat.Add(new XElement("Name", s.Name));
                        //    Stat.Add(new XElement("Info", s.Info));
                        //    Stat.Add(new XElement("Value", s.Value));
                        //    Stat.Add(new XElement("Bonus", s.Bonus));
                        //    Stats.Add(new XElement(Stat));
                        //}
                        Npc.Add(new XElement(Stats));
                        newS.Element("Npcs").Add(new XElement(Npc));
                        i++;
                    }
                    i = 0;

                    newS.Add(new XElement("Events"));
                    foreach (Event e in S.Events)
                    {
                        XElement Event = new XElement("Event");
                        Event.Add(new XAttribute("Id", i.ToString()));
                        Event.Add(new XElement("Title", e.Title));
                        Event.Add(new XElement("Description", e.Description));
                        newS.Element("Events").Add(new XElement(Event));
                        i++;
                    }
                    i = 0;

                    newS.Add(new XElement("Rewards"));
                    //foreach (Item r in S.Rewards)
                    //{
                    //    XElement Reward = new XElement("Npc");
                    //    Reward.Add(new XAttribute("Id", i.ToString()));
                    //    Reward.Add(new XElement("Name", r.Name));
                    //    Reward.Add(new XElement("Who", r.Who));
                    //    Reward.Add(new XElement("Additional", r.Additional));
                    //    Reward.Add(new XElement("Rare", r.Rare));
                    //    XElement Stats = new XElement("Characteristics");
                    //    //foreach (Stat s in r.Characteristics)
                    //    //{
                    //    //    XElement Stat = new XElement("Characteristic");
                    //    //    Stat.Add(new XElement("Name", s.Name));
                    //    //    Stat.Add(new XElement("Info", s.Info));
                    //    //    Stat.Add(new XElement("Value", s.Value));
                    //    //    Stat.Add(new XElement("Bonus", s.Bonus));
                    //    //    Stats.Add(new XElement(Stat));
                    //    //}
                    //    Reward.Add(new XElement(Stats));
                    //    newS.Element("Rewards").Add(new XElement(Reward));
                    //    i++;
                    //}
                    i = 0;

                    doc.Root.Add(new XElement(newS));
                    doc.Save(HttpContext.Current.Server.MapPath("~/Data/Scenario/Scenarios.xml"));
                }
            }
        }        

        public static class Wiki
        {
            public static string path = HttpContext.Current.Server.MapPath("~/Data/Wiki/");

            public static string GetInfo(string ElementName,string File)
            {
                XDocument doc = XDocument.Load(path+File);
                foreach (XElement el in doc.Root.Elements())
                {
                    if(el.Attribute("Name").Value==ElementName)
                    {
                        return el.Value;
                    }
                }
                return "Нет информации.";
            }

            public static string Disabled_GetChar(string ElementName)
            {
                XDocument doc = XDocument.Load(path + "");
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Name").Value == ElementName)
                    {
                        return el.Value;
                    }
                }
                return "Нет информации.";
            }
        }

        public static class Games
        {
            public static Game GameById(string GameId)
            {
                Game g = new Game();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Games.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == GameId)
                    {
                        g.Id = Convert.ToInt32(GameId);
                        g.ScriptId = Convert.ToInt32(el.Element("ScriptId").Value);
                        g.LocationId = Convert.ToInt32(el.Element("LocationId").Value);
                        g.EventId = Convert.ToInt32(el.Element("EventId").Value);
                        g.Master = Convert.ToInt32(el.Element("Master").Value);

                        g.Heroes = new int[0];
                        //foreach (XElement x in el.Elements("Heroes"))
                        //    g.Heroes.Add(Convert.ToInt32(x.Element("Hero").Value));

                        g.Chat = Convert.ToInt32(el.Element("Chat").Value);
                    }
                }
                return null;
            }

            //public static void AddMessage(string GameId, string HeroId, string Master, string System)
            //{
            //    XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Games.xml"));
            //    foreach (XElement gme in doc.Root.Elements())
            //    {
            //        if (gme.Attribute("Id").Value == GameId)
            //        {
            //            XElement newmessage = new XElement("Message",
            //                new XAttribute("HeroId", HeroId),
            //                new XAttribute("Master", Master),
            //                new XAttribute("System", System));
            //            gme.Element("Games").Add(newmessage);
            //        }
            //    }
            //    doc.Save(path);
            //}

            //public static string[] GetMessage(string GameId, int Count, bool Desc)
            //{
            //    string[] s = new string[0];

            //    int chat_id = 0;
            //    XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Games.xml"));
            //    foreach (XElement el in doc.Root.Elements())
            //    {
            //        if (el.Attribute("Id").Value == GameId)
            //        {
            //            chat_id = Convert.ToInt32(el.Element("Chat").Value);

            //        }
            //    }


            //    //        foreach (XElement x in el.Element("Chat"))
            //    //        {
            //    //            string prefix = "";
            //    //            if (el.Attribute("HeroId").Value != "0")
            //    //                prefix = Characters.GetInfo(el.Attribute("HeroId").Value, "")[0].Value;
            //    //            if (el.Attribute("Master").Value == "True")
            //    //                prefix = "Мастер";

            //    //            g.Chat.Add(prefix + ": " + el.Element("Message").Value);

            //    //for (int i = 0; i < g.Chat.Length; i++)
            //    //{

            //    //}
            //    return s;
            //}

            public static void ChangeLocation(string GameId, string MasterId, string LocationId)
            { Change(GameId, MasterId, "LocationId", LocationId); }

            public static void ChangeEvent(string GameId, string MasterId, string EventId)
            { Change(GameId, MasterId, "EventId", EventId); }

            public static void ChangeNpc(string GameId, string MasterId, string NpcId)
            { Change(GameId, MasterId, "NpcId", NpcId); }

            public static void Change(string GameId, string MasterId, string ArgumentName, string Id)
            {

                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Games.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == GameId)
                    {
                        if (el.Element("Master").ToString() == MasterId)
                            el.SetElementValue(ArgumentName, Id);
                    }
                }
            }
        }

        public delegate bool Where(Message M);

        public static class Chat
        {
            public static object obj;

            public static string[] Get(int GameId, int Count, bool Desc, Where[] where,bool Test=false)
            {
                if (where == null)
                {
                    where = new Where[0];
                }
                XDocument doc = new XDocument();
                if(Test)
                    doc = XDocument.Load(System.IO.Directory.GetCurrentDirectory().Replace(@"rpgSys.Tests\bin\Debug", @"rpgSys\Data\Games\Chats\" + GameId.ToString() + ".xml"));
                else
                    doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Chats/" + GameId.ToString() + ".xml"));


                List<string> data = new List<string>();
                var mydata = (from item in doc.Root.Elements()
                              orderby Convert.ToInt32(item.Attribute("Id").Value)
                              select new Message
                              {
                                  Id = Convert.ToInt32(item.Attribute("Id").Value),
                                  HeroId = Convert.ToInt32(item.Attribute("HeroId").Value),
                                  Master = Convert.ToBoolean(item.Attribute("Master").Value),
                                  System = Convert.ToBoolean(item.Attribute("System").Value),
                                  Text = item.Value
                              }).ToList<Message>();
                if (Desc)
                    mydata = mydata.OrderByDescending(a => a.Id).Take(Count).ToList<Message>();
                else
                    mydata = mydata.OrderBy(a => a.Id).Take(Count).ToList<Message>();

                foreach (Message m in mydata)
                {
                    bool tested = true;
                    foreach (Where w in where)
                    {
                        tested = w(m);
                    }

                    if (tested)
                    {
                        string prefix = "";

                        if (m.HeroId != 0)
                            prefix = Characters.GetInfo(m.HeroId.ToString(), "")[0].Value + ": ";
                        if (m.Master)
                            prefix = "Мастер:";

                        data.Add(prefix + m.Text);
                    }
                }
                return data.ToArray();
            }

            public static bool Set(int GameId, int HeroId, bool IsMaster, bool IsSystem, string Text)
            {
                lock (obj)
                {
                    XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Games/Chats/" + GameId.ToString() + ".xml"));
                    int maxId = doc.Root.Elements("Message").Max(t => Int32.Parse(t.Attribute("id").Value));
                    XElement newmessage = new XElement("Message",
                        new XAttribute("Id", ++maxId),
                        new XElement("HeroId", HeroId),
                        new XElement("System", IsSystem),
                        new XElement("Text", Text),
                        new XElement("Master", IsMaster));
                    doc.Root.Add(newmessage);
                    doc.Save(path);
                    return true;
                }
            }
        }

        public static class Skills
        {
            public static List<Stat> GetSkills(string Type)
            {
                List<Stat> list = new List<Stat>();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Skill/" + Type + ".xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    list.Add(new Stat() { Name = el.Element("Name").Value, Info = el.Element("DIX").Value, Value = el.Attribute("Id").Value });
                }
                return list;
            }
        }

        public static class Characters
        {
            private static string path = HttpContext.Current.Server.MapPath("~/Data/Hero/Character/Characters.xml");

            public static List<Stat> GetInfo(string UserId)
            {
                List<Stat> info = new List<Stat>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("UserId").Value == UserId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            string value = "";
                            if (ell.Attribute("Id") != null)
                                value = GetAdditionalParametr(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/" + ell.Name.LocalName + ".xml"), ell.Name.LocalName, ell.Value);
                            else
                                value = ell.Value;

                            info.Add(new Stat()
                            {
                                Name = ell.Name.LocalName,
                                Info = Wiki.GetInfo(ell.Name.LocalName, "Info.xml"),
                                Value = value
                            });
                        }
                    }
                }
                return info;
            }

            public static List<Stat> GetInfo(string HeroId,string HeroId2)
            {
                List<Stat> info = new List<Stat>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == HeroId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            string value = "";
                            if (ell.Attribute("Id") != null)
                                value = GetAdditionalParametr(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/" + ell.Name.LocalName + ".xml"), ell.Name.LocalName, ell.Value);
                            else
                                value = ell.Value;

                            info.Add(new Stat()
                            {
                                Name = ell.Name.LocalName,
                                Info = Wiki.GetInfo(ell.Name.LocalName, "Info.xml"),
                                Value = value
                            });
                        }
                    }
                }
                return info;
            }

            public static List<Stat> GetChars(string UserId)
            {
                string HeroId = HeroIdByUserId(UserId);
                List<Stat> info = new List<Stat>();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/Characteristics.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("HeroId").Value == HeroId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            info.Add(new Stat()
                            {
                                Name = ell.Name.LocalName + " " + ell.Attribute("DIX").Value,
                                Info = Wiki.GetInfo(ell.Name.LocalName, "Characteristics.xml"),
                                Value = ell.Value
                            });
                        }
                    }
                }
                return info;
            }

            public static List<Stat> GetProfi(string UserId)
            {
                string HeroId = HeroIdByUserId(UserId);
                List<Stat> info = new List<Stat>();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/Proficiency.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("HeroId").Value == HeroId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            info.Add(new Stat()
                            {
                                Name = ell.Name.LocalName,
                                Info = Wiki.GetInfo(ell.Name.LocalName, "Proficiency.xml"),
                                Value = ell.Value
                            });
                        }
                    }
                }
                return info;
            }

            public static List<Stat> GetHealth(string UserId)
            { return GetState(UserId, "Health"); }

            public static List<Stat> GetDefence(string UserId)
            { return GetState(UserId, "Defence"); }

            public static List<Stat> GetAttack(string UserId)
            { return GetState(UserId, "Attack"); }

            public static List<Stat> GetMaterial(string UserId)
            {
                return GetSkill(UserId, "Material");
            }

            public static List<Stat> GetMental(string UserId)
            {
                return GetSkill(UserId, "Mental");
            }

            public static List<Stat> GetClass(string UserId)
            {
                return GetSkill(UserId, GetInfo(UserId)[2].Value);
            }

            public static List<Stat> GetSkill(string UserId, string Type)
            {
                string cls = Type;
                if (Type != "Mental" && Type != "Material")
                    Type = "Class";

                string HeroId = HeroIdByUserId(UserId);
                List<Stat> list = new List<Stat>();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/" + Type + "Skills.xml"));

                if (cls != "Mental" && cls != "Material")
                    Type = cls;

                XDocument doc1 = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Skill/" + Type + ".xml"));

                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("HeroId").Value == HeroId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            foreach (XElement elll in doc1.Root.Elements())
                            {
                                if (elll.Attribute("Id").Value == ell.Attribute("Id").Value)
                                {
                                    list.Add(new Stat()
                                    {
                                        Name = elll.Element("Name").Value + "~" + elll.Element("DIX").Value,
                                        Info = "Нет информации",
                                        Value = ell.Element("Value").Value,
                                        Bonus = ell.Element("Bonus").Value

                                    });
                                }
                            }
                        }
                    }
                }
                return list;
            }

            public static List<Stat> GetInitiative(string UserId)
            { return GetState(UserId, "Initiative"); }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="UserId"></param>
            /// <param name="State">Health, Defence, Attack, Initiative</param>
            /// <returns></returns>
            public static List<Stat> GetState(string UserId,string State)
            {
                string HeroId = HeroIdByUserId(UserId);
                List<Stat> info = new List<Stat>();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/"+State+"State.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("HeroId").Value == HeroId)
                    {
                        foreach (XElement ell in el.Elements())
                        {
                            string value = "";
                            if (State == "Health")
                            {                                
                                if (ell.Attribute("Id") != null)
                                    value = GetAdditionalParametr(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/" + ell.Name.LocalName + ".xml"), ell.Name.LocalName, ell.Value);
                                else
                                    value = ell.Value;
                            }
                            info.Add(new Stat()
                            {
                                Name = ell.Name.LocalName.Replace("_", " "),
                                Info = Wiki.GetInfo(ell.Name.LocalName, State + "State.xml"),
                                Value = State == "Health" ? value : ell.Value
                            });
                        }
                    }
                }
                return info;
            }

            public static string GetAdditionalParametr(string path, string element, string searchedId)
            {               
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value == searchedId)
                    {
                        return el.Value;
                    }
                }
                return "NotFound";
            }

            public static string HeroIdByUserId(string UserId)
            {
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("UserId").Value ==UserId)
                    {
                        return el.Attribute("Id").Value;
                    }
                }
                return "0";
            }

            public static List<Hero> Get()
            {
                List<Hero> list = new List<Hero>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("Id").Value != "0")
                    {
                        list.Add(new Hero());
                    }
                }
                return list;
            }

            public static Hero Profile(string UserId)
            {
                Hero c = new Hero();
                XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Hero/Character/Characters.xml"));
                foreach (XElement el in doc.Root.Elements())
                {
                    if (el.Attribute("UserId").Value == UserId)
                    {
                        c.Name = el.Element("Имя").Value;
                        var gme=Games.GameById(el.Attribute("GameId").Value);

                        if (gme != null)
                            c.Skin = Scenarios.ScById(gme.ScriptId).Title;
                        else
                            c.Skin = "0";
                    }
                }
                if (c.Name == "")
                {
                    c.Name = "0";
                    c.Skin = "0";
                }
                return c;
            }
        }

        public static class Items
        {
            private static string path = HttpContext.Current.Server.MapPath("~/Data/Server/Items.xml");

            public static List<Item> Get()
            {
                List<Item> list = new List<Item>();
                XDocument doc = XDocument.Load(path);
                foreach (XElement el in doc.Root.Elements())
                {

                }
                return list;
            }
        }

        public static ServerSettings Get()
        {
            try
            {
                ServerSettings s = new ServerSettings();
                //XDocument doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/Server/Server.xml"));
                //s.Name = doc.Root.Element("Name").Value;
                //s.ServerMessage = doc.Root.Element("ServerMessage").Value;
                //s.MessageOfTheDay = doc.Root.Element("MessageOfTheDay").Value;
                //s.ServerModule = Modules.Get().Count;
                //s.ServerScenario = Scenarios.Get().Count;
                //s.ServerCharacter = Characters.Get().Count;
                //s.ServerItem = Items.Get().Count;
                return s;
            }
            catch { return null; }
        }

        #region Examples

        private static string path = HttpContext.Current.Server.MapPath("~/Data/User/Users.xml");

        public static bool Set(User Value)
        {
            XDocument doc = XDocument.Load(path);
            foreach (XElement el in doc.Root.Elements("feature"))
            {
                if (Int32.Parse(el.Attribute("id").Value) == Value.Id)
                {
                    if (el.Element("name").Value != Value.Login && Value.Login != null)
                        el.SetElementValue("name", Value.Login);

                    if (el.Element("info").Value != Value.Password && Value.Password != null)
                        el.SetElementValue("info", Value.Password);
                }
            }
            doc.Save(path);
            return true;
        }

        public static bool Insert(User Value)
        {
            XDocument doc = XDocument.Load(path);
            int maxId = doc.Root.Elements("feature").Max(t => Int32.Parse(t.Attribute("id").Value));
            XElement newfeature = new XElement("feature",
                new XAttribute("id", ++maxId),
                new XElement("name", Value.Login),
                new XElement("info", Value.Password));
            doc.Root.Add(newfeature);
            doc.Save(path);
            return true;
        }

        #endregion
    }
}