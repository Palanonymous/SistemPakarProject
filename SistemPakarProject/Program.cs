using System;

class Program
{
    // Method Pertanyaan
    static string Tanya(string pertanyaan)
    {
        Console.Write(pertanyaan);
        return Console.ReadLine().ToLower();
    }

    // Method Certainty Factor
    static double HitungCF(double cfLama, double cfBaru)
    {
        return cfLama + cfBaru * (1 - cfLama);
    }

    // Method Menampilkan Hasil
    static void TampilHasil(string performa, int totalPoin, double cf, string alasan, string jejak)
    {
        Console.WriteLine();
        Console.WriteLine("=== HASIL ANALISIS ===");
        Console.WriteLine("Performa  : " + performa);
        Console.WriteLine("TotalPoin : " + totalPoin);
        Console.WriteLine("CF        : " + (cf * 100) + "%");

        Console.WriteLine();
        Console.WriteLine("=== ALASAN ===");
        Console.WriteLine(alasan);

        Console.WriteLine();
        Console.WriteLine("=== JEJAK PENALARAN ===");
        Console.WriteLine(jejak);
    }

    static void Main()
    {
        Console.WriteLine("=== SISTEM PAKAR PENILAIAN PERFORMA PLAYER ===");
        Console.WriteLine();

        // Variabel Pertanyaan
        string q1 = "";
        string q2 = "";
        string q3 = "";
        string q4 = "";
        string q5 = "";
        string q6 = "";

        // Variabel Poin Performa
        int poinPro = 0;
        int poinAverage = 0;
        int poinNoob = 0;
        int poinSkillIssue = 0;

        // Variabel CF
        double cfPro = 0;
        double cfAverage = 0;
        double cfNoob = 0;
        double cfSkillIssue = 0;

        // Explanation Facility
        string alasan = "";

        // Jejak Penalaran
        string jejak = "";

        // =========================
        // Q1
        // =========================

        q1 = Tanya("Apakah player clear dungeon tanpa mati? (y/n): ");

        if (q1 == "y")
        {
            poinPro += 2;
            poinAverage += 2;

            cfPro = HitungCF(cfPro, 0.3);
            cfAverage = HitungCF(cfAverage, 0.3);

            alasan += "- Player clear dungeon tanpa mati\n";
            jejak += "Q1 = y -> PRO +2, Average +2\n";
        }
        else if (q1 == "n")
        {
            poinNoob += 2;
            poinSkillIssue += 2;

            cfNoob = HitungCF(cfNoob, 0.3);
            cfSkillIssue = HitungCF(cfSkillIssue, 0.3);

            alasan += "- Player gagal clear tanpa mati\n";
            jejak += "Q1 = n -> Noob +2, Skill Issue +2\n";
        }

        // =========================
        // Forward Chaining
        // =========================

        if (q1 == "y")
        {
            q2 = Tanya("Apakah dungeon selesai kurang dari 10 menit? (y/n): ");

            if (q2 == "y")
            {
                poinPro += 2;
                cfPro = HitungCF(cfPro, 0.4);

                alasan += "- Dungeon selesai sangat cepat\n";
                jejak += "Q2 = y -> PRO +2\n";

                q3 = Tanya("Apakah HP player tersisa lebih dari 70%? (y/n): ");

                if (q3 == "y")
                {
                    poinPro += 2;

                    cfPro = HitungCF(cfPro, 0.4);

                    alasan += "- HP player masih tinggi\n";
                    jejak += "Q3 = y -> PRO +2\n";
                }
                else if (q3 == "n")
                {
                    poinAverage += 2;

                    cfAverage = HitungCF(cfAverage, 0.4);

                    alasan += "- HP player tidak terlalu tinggi\n";
                    jejak += "Q3 = n -> Average +2\n";
                }
            }

            else if (q2 == "n")
            {
                poinAverage += 2;

                cfAverage = HitungCF(cfAverage, 0.4);

                alasan += "- Waktu clear dungeon cukup lama\n";
                jejak += "Q2 = n -> Average +2\n";

                q3 = Tanya("Apakah player sering menghindari serangan boss? (y/n): ");

                if (q3 == "y")
                {
                    poinAverage += 2;

                    cfAverage = HitungCF(cfAverage, 0.4);

                    alasan += "- Player mampu menghindari serangan boss\n";
                    jejak += "Q3 = y -> Average +2\n";
                }
                else if (q3 == "n")
                {
                    poinNoob += 2;

                    cfNoob = HitungCF(cfNoob, 0.4);

                    alasan += "- Player sering terkena serangan boss\n";
                    jejak += "Q3 = n -> Noob +2\n";
                }
            }
        }

        else if (q1 == "n")
        {
            q2 = Tanya("Apakah player mati lebih dari 3 kali? (y/n): ");

            if (q2 == "y")
            {
                poinSkillIssue += 2;
                cfSkillIssue = HitungCF(cfSkillIssue, 0.4);

                alasan += "- Player mati terlalu banyak\n";
                jejak += "Q2 = y -> Skill Issue +2\n";

                q3 = Tanya("Apakah level karakter di atas level 5? (y/n): ");

                if (q3 == "y")
                {
                    poinSkillIssue += 2;

                    cfSkillIssue = HitungCF(cfSkillIssue, 0.4);

                    alasan += "- Level karakter masih rendah\n";
                    jejak += "Q3 = y -> Skill Issue +2\n";
                }
                else if (q3 == "n")
                {
                    poinNoob += 2;

                    cfNoob = HitungCF(cfNoob, 0.4);

                    alasan += "- Level karakter cukup tinggi\n";
                    jejak += "Q3 = n -> Noob +2\n";
                }
            }

            else if (q2 == "n")
            {
                poinNoob += 2;

                cfNoob = HitungCF(cfNoob, 0.4);

                alasan += "- Player tidak mati terlalu banyak\n";
                jejak += "Q2 = n -> Noob +2\n";

                q3 = Tanya("Apakah player menggunakan lebih dari 3 potion? (y/n): ");

                if (q3 == "y")
                {
                    poinNoob += 2;

                    cfNoob = HitungCF(cfNoob, 0.4);

                    alasan += "- Player terlalu banyak menggunakan potion\n";
                    jejak += "Q3 = y -> Noob +2\n";
                }
                else if (q3 == "n")
                {
                    poinAverage += 2;

                    cfAverage = HitungCF(cfAverage, 0.4);

                    alasan += "- Penggunaan potion masih normal\n";
                    jejak += "Q3 = n -> Average +2\n";
                }
            }
        }

        q4 = Tanya("Apakah Player menggunakan Item OP? (y/n): ");
        if (q4 == "y")
        {
            poinNoob += 2;
            poinSkillIssue += 2;

            cfNoob = HitungCF(cfNoob, 0.4);
            cfSkillIssue = HitungCF(cfSkillIssue, 0.4);

            alasan += "- Player menggunakan Item OP\n";
            jejak += "Q4 = y -> Noob +2, Skill Issue +2\n";
        }
        else
        {
            poinPro += 2;
            poinAverage += 2;

            cfPro = HitungCF(cfPro, 0.4);
            cfAverage = HitungCF(cfAverage, 0.4);

            alasan += "- Player tidak menggunakan Item OP\n";
            jejak += "Q4 = y -> Pro +2, Average +2\n";
        }

        q5 = Tanya("Apakah Player mengunakan Armour OP? (y/n): ");
        if (q5 == "y")
        {
            poinNoob += 3;
            poinSkillIssue += 3;

            cfNoob = HitungCF(cfNoob, 0.5);
            cfSkillIssue = HitungCF(cfSkillIssue, 0.5);

            alasan += "- Player menggunakan Armour OP\n";
            jejak += "Q5 = y -> Noob +3, Skill Issue +3\n";
        }
        else
        {
            poinPro += 3;
            poinAverage += 3;

            cfPro = HitungCF(cfPro, 0.5);
            cfAverage = HitungCF(cfAverage, 0.5);

            alasan += "- Player tidak menggunakan Armour OP\n";
            jejak += "Q5 = y -> Pro +3, Average +3\n";
        }


        // =========================
        // Menentukan Hasil
        // =========================

        string performa = "";
        double cfHasil = 0;

        int poinTertinggi = poinPro;

        performa = "PRO";
        cfHasil = cfPro;

        if (poinAverage > poinTertinggi)
        {
            poinTertinggi = poinAverage;
            performa = "Average";
            cfHasil = cfAverage;
        }

        if (poinNoob > poinTertinggi)
        {
            poinTertinggi = poinNoob;
            performa = "Noob";
            cfHasil = cfNoob;
        }

        if (poinSkillIssue > poinTertinggi)
        {
            poinTertinggi = poinSkillIssue;
            performa = "SKILL ISSUE";
            cfHasil = cfSkillIssue;
        }

        // =========================
        // Menampilkan Hasil
        // =========================

        TampilHasil(performa, poinTertinggi, cfHasil, alasan, jejak);

        Console.ReadLine();
    }
}
