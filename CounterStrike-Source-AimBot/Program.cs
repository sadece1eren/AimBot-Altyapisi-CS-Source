using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Swed32;
using CounterStrike_Source_AimBot;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CounterStrike_Source_Aimbot
{
    internal class Program
    {
        // sadece1eren tarafından editlendi ve kodlandı
        // Özel teşekkürlerimi Swedz sunarım https://www.youtube.com/@SwedishTwat
        // Yardıma İhtiyacın varsa benle discord aracılığı ile iletişim kurabilirsin. Discord adım "sadece1eren"

        // Edited and coded by sadece1eren 
        // Special thanks for Swedz https://www.youtube.com/@SwedishTwat
        // If you need help contact with me via discord. My name in discord "sadece1eren"

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        // offset kısmı eğer nasıl offset bulacağını bilmiyorsan bu videoları izlemeni öneririm https://youtu.be/DmNBIrpmAIk , https://www.youtube.com/watch?v=YaFlh2pIKAg 
        const int entityBase = 0x04D5AE4;
        const int localplayer = 0x04C88E8;
        const int viewY = 0x47C33C;
        const int viewX = 0x47C340;
        const int hp = 0x94;
        const int xyz = 0x260;
        const int dormant = 0x17C;
        const int team = 0x9C;
        // offset part, if you don't know how to find offset, I suggest you watch these videos https://youtu.be/DmNBIrpmAIk , https://www.youtube.com/watch?v=YaFlh2pIKAg
        static void Main(string[] args)
        { 
            Console.ForegroundColor = ConsoleColor.DarkGreen; // buradan konsolun yazı renklerini ayarlayabilirsiniz - Here you can set the font colors of the console.
            Console.WriteLine(@" ▄▄▄██▀▀▀██░ ██  ██▀███   ▒█████  ▒██   ██▒    ▄▄▄       ██▓ ███▄ ▄███▓ ▄▄▄▄    ▒█████  ▄▄▄█████▓   
   ▒██  ▓██░ ██▒▓██ ▒ ██▒▒██▒  ██▒▒▒ █ █ ▒░   ▒████▄    ▓██▒▓██▒▀█▀ ██▒▓█████▄ ▒██▒  ██▒▓  ██▒ ▓▒   
   ░██  ▒██▀▀██░▓██ ░▄█ ▒▒██░  ██▒░░  █   ░   ▒██  ▀█▄  ▒██▒▓██    ▓██░▒██▒ ▄██▒██░  ██▒▒ ▓██░ ▒░   
▓██▄██▓ ░▓█ ░██ ▒██▀▀█▄  ▒██   ██░ ░ █ █ ▒    ░██▄▄▄▄██ ░██░▒██    ▒██ ▒██░█▀  ▒██   ██░░ ▓██▓ ░    
 ▓███▒  ░▓█▒░██▓░██▓ ▒██▒░ ████▓▒░▒██▒ ▒██▒    ▓█   ▓██▒░██░▒██▒   ░██▒░▓█  ▀█▓░ ████▓▒░  ▒██▒ ░    
 ▒▓▒▒░   ▒ ░░▒░▒░ ▒▓ ░▒▓░░ ▒░▒░▒░ ▒▒ ░ ░▓ ░    ▒▒   ▓▒█░░▓  ░ ▒░   ░  ░░▒▓███▀▒░ ▒░▒░▒░   ▒ ░░      
 ▒ ░▒░   ▒ ░▒░ ░  ░▒ ░ ▒░  ░ ▒ ▒░ ░░   ░▒ ░     ▒   ▒▒ ░ ▒ ░░  ░      ░▒░▒   ░   ░ ▒ ▒░     ░       
 ░ ░ ░   ░  ░░ ░  ░░   ░ ░ ░ ░ ▒   ░    ░       ░   ▒    ▒ ░░      ░    ░    ░ ░ ░ ░ ▒    ░         
 ░   ░   ░  ░  ░   ░         ░ ░   ░    ░           ░  ░ ░         ░    ░          ░ ░              
                                                                             ░                      "); // buraya istediğini yazabilirsin - you can write here anything you want.
            Console.WriteLine("Mouse X2 Butonuna Basılı Tutarak Aimbotu Aktifleştirebilirsiniz. ©2023"); // buraya istediğini yazabilirsin - you can write here anything you want.

            // Where it says hl2, you need to write the name of the game you are playing.you can look in task manager 
            Swed jhrox = new Swed("hl2"); // hl2 yazan yere hangi oyuna yapıyorsan onun adını yazman lazım. görev yöneticisinden bakabilirsin
            entity player = new entity();
            List<entity> entityList = new List<entity>();
            if (player != null)
            {
                while (true)
                {
                    short state = GetAsyncKeyState(0x06); // buradan hangi butona basınca aktifleşeceğini ayarlayabilirsin bunu istemiyorsan if yapısını sil. - You can set which button to activate from here. If you don't want this, delete the if.
                    bool isPressed = (state & (1 << 15)) != 0;
                    if (isPressed) // butona basınca aktifleşmesini istemiyorsan sil - If you don't want it to activate when you press the button, delete it.
                    {
                        upadateLocal();
                        updateEntities();
                        entityList = entityList.OrderBy(o => o.mag).ToList();
                        if (entityList.Count > 0)
                            aimbot(entityList[0]);
                    }
                    Thread.Sleep(1);
                }
            }

            float calcMag(entity entity) // buraya ellemenize gerek yok - you don't need to touch it here
            {
                return (float)Math.Sqrt(Math.Pow(entity.x - player.x, 2) + Math.Pow(entity.y - player.y, 2) + Math.Pow(entity.z - player.z, 2));
            }

            void upadateLocal() // kendi karakterimizin verileri burdan alınır. our own character's data is taken from here
            {
                // Write the dll with the offsets you found here. If it's confusing, you can watch this video https://www.youtube.com/watch?v=e9JF5vY3308
                var client = jhrox.GetModuleBase("client.dll"); // bulduğunuz offsetlerin bulunduğu dll buraya yazın. kafa karıştırıcı geldiyse bu videoyuda izleyebilirsiniz https://www.youtube.com/watch?v=e9JF5vY3308
                var buffer = jhrox.ReadPointer(client, localplayer);
                var teamnum = jhrox.ReadBytes(buffer, team, 4);
                var pos = jhrox.ReadBytes(buffer, xyz, 12);
                player.x = BitConverter.ToSingle(pos, 0);
                player.y = BitConverter.ToSingle(pos, 4);
                player.z = BitConverter.ToSingle(pos, 8);
                player.team = BitConverter.ToInt32(teamnum, 0);
            }

            void updateEntities() // düşman verileri buradan alınır. Enemy Data is taken from here
            {
                entityList.Clear();
                for (int i = 0; i < 32; i++)
                {
                    // Write the dll with the offsets you found here. If it's confusing, you can watch this video https://www.youtube.com/watch?v=e9JF5vY3308
                    var client = jhrox.GetModuleBase("client.dll"); // bulduğunuz offsetlerin bulunduğu dll buraya yazın. kafa karıştırıcı geldiyse bu videoyuda izleyebilirsiniz https://www.youtube.com/watch?v=e9JF5vY3308
                    var buffer = jhrox.ReadPointer(client, entityBase + i * 0x10);
                    var health = BitConverter.ToInt32(jhrox.ReadBytes(buffer, hp, 4), 0);
                    var isdormant = BitConverter.ToInt32(jhrox.ReadBytes(buffer, dormant, 4), 0);
                    var teamnum = BitConverter.ToInt32(jhrox.ReadBytes(buffer, team, 4), 0);
                    if (health < 2 || isdormant != 0 || teamnum == player.team)
                        continue;
                    var pos = jhrox.ReadBytes(buffer, xyz, 12);
                    var ent = new entity
                    {
                        x = BitConverter.ToSingle(pos, 0),
                        y = BitConverter.ToSingle(pos, 4),
                        z = BitConverter.ToSingle(pos, 8),
                        team = teamnum,
                        health = health
                    };
                    ent.mag = calcMag(ent);
                    entityList.Add(ent);
                }
            }

            void aimbot(entity entity) // aimbot burada hesaplanır - aimbot is calculated here
            {
                // Write the dll with the offsets you found here. If it's confusing, you can watch this video https://www.youtube.com/watch?v=e9JF5vY3308
                var engine = jhrox.GetModuleBase("engine.dll"); // bulduğunuz offsetlerin bulunduğu dll buraya yazın. kafa karıştırıcı geldiyse bu videoyuda izleyebilirsiniz https://www.youtube.com/watch?v=e9JF5vY3308
                float deltaX = entity.x - player.x;
                float deltaY = entity.y - player.y;
                float X = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);
                float deltaZ = entity.z - player.z;
                double dist = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
                float Y = -(float)(Math.Atan2(deltaZ, dist) * 180 / Math.PI);
                jhrox.WriteBytes(engine, viewX, BitConverter.GetBytes(X));
                jhrox.WriteBytes(engine, viewY, BitConverter.GetBytes(Y));
            }
        }
    }
}