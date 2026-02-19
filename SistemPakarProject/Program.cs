using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPakarProject
{
    class DungeonData
    {
        public int PlayerLevel;   // 1–10
        public int HP;            // 1–100
        public int Potion;        // 0–5 (dipakai)
        public int ClearTime;     // menit (0–30+)
        public int Death;         // 0–6+

        public bool Win;
        public double HPScore;
        public double PotionScore;
        public double TimeScore;
        public double DeathScore;
        public double LevelScore;
        public double PerformancePercent;
        public string Category;   // High / Medium / Low
        public double CF;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Expert System Dungeon (Forward Chaining + CF) ===");
            List<DungeonData> dungeons = new List<DungeonData>();

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\n--- Input Dungeon {i} ---");
                DungeonData d = new DungeonData();

                d.PlayerLevel = ReadIntRange("Level Player (1-10): ", 1, 10);
                d.HP = ReadIntRange("Mulai dengan HP (1-100): ", 1, 100);
                d.Potion = ReadIntRange("Potion Used (0-5): ", 0, 5);
                d.ClearTime = ReadIntRange("Clear Time (0-30 menit): ", 0, 1000); // >30 akan mark lose
                d.Death = ReadIntRange("Jumlah Respawn (0-6 Respawn): ", 0, 1000);            // >6 akan mark lose


                EvaluateWin(d);


                AnalyzeDungeonWeighted(d);


                Console.WriteLine($"\nDungeon {i} Summary:");
                Console.WriteLine($" Win: {d.Win}");
                Console.WriteLine($" HP Score     : {d.HPScore:F1}%");
                Console.WriteLine($" Potion Score : {d.PotionScore:F1}%");
                Console.WriteLine($" Time Score   : {d.TimeScore:F1}%");
                Console.WriteLine($" Death Score  : {d.DeathScore:F1}%");
                Console.WriteLine($" Level Score  : {d.LevelScore:F1}%");
                Console.WriteLine($" Performance  : {d.PerformancePercent:F1}% -> {d.Category}");
                Console.WriteLine($" CF           : {d.CF:F2}");

                dungeons.Add(d);
            }


            FinalDecision(dungeons);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }


        static int ReadIntRange(string prompt, int minAllowed, int maxAllowed)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (int.TryParse(s, out int v))
                {
                    return v;
                }
                    Console.WriteLine("Input tidak valid, masukkan angka.");
            }
        }

        static void EvaluateWin(DungeonData d)
        {
            if (d.Death > 6 || d.ClearTime > 30)
                d.Win = false;
            else
                d.Win = true;
        }

        static void AnalyzeDungeonWeighted(DungeonData d)
        {
            d.HP = ClampInt(d.HP, 1, 100);
            d.HPScore = (1.0 - (double)(d.HP - 1) / (100 - 1)) * 100.0; 

            d.Potion = ClampInt(d.Potion, 0, 5);
            d.PotionScore = (1.0 - (double)d.Potion / 5.0) * 100.0;

            if (d.ClearTime <= 10) d.TimeScore = 100.0;
            else if (d.ClearTime >= 30) d.TimeScore = 0.0;
            else d.TimeScore = (1.0 - (double)(d.ClearTime - 10) / (30 - 10)) * 100.0;

            if (d.Death <= 0) d.DeathScore = 100.0;
            else if (d.Death >= 6) d.DeathScore = 0.0;
            else d.DeathScore = (1.0 - (double)d.Death / 6.0) * 100.0;

            int level = ClampInt(d.PlayerLevel, 1, 10);
            d.LevelScore = (1.0 - (double)(level - 1) / (10 - 1)) * 100.0;

            d.PerformancePercent = (d.HPScore + d.PotionScore + d.TimeScore + d.DeathScore + d.LevelScore) / 5.0;


            if (d.PerformancePercent >= 80.0)
            {
                d.Category = "High";   
                d.CF = 0.80;
            }
            else if (d.PerformancePercent >= 60.0)
            {
                d.Category = "Medium"; 
                d.CF = 0.60;
            }
            else
            {
                d.Category = "Low";    
                d.CF = 0.40;
            }
        }

        static double CombineCF(List<double> cfs)
        {
            if (cfs == null || cfs.Count == 0) return 0.0;
            double result = cfs[0];
            for (int i = 1; i < cfs.Count; i++)
            {
                result = result + cfs[i] * (1 - result);
            }
            return Math.Round(result, 2);
        }

        static void FinalDecision(List<DungeonData> dungeons)
        {
            int high = 0, medium = 0, low = 0;
            List<double> cfs = new List<double>();

            foreach (var d in dungeons)
            {
                if (d.Category == "High") high++;
                else if (d.Category == "Medium") medium++;
                else low++;

                cfs.Add(d.CF);
            }

            double finalCF = CombineCF(cfs);

            Console.WriteLine("\n=== FINAL DECISION ===");

            string difficulty;
            if (high >= 2) difficulty = "HARD (Dragon)";
            else if (medium >= 2) difficulty = "NORMAL (Golem)";
            else if (high == 1 && medium == 1 && low == 1) difficulty = "NORMAL (Golem)";
            else difficulty = "EASY (Goblin)";

            Console.WriteLine("Selected Difficulty : " + difficulty);
            Console.WriteLine("Combined Certainty  : " + finalCF);
        }

        static int ClampInt(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
    }
}
