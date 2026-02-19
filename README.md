#  Expert System Dungeon — Forward Chaining + Certainty Factor

> Sistem Pakar berbasis Forward Chaining dan Certainty Factor (CF) untuk menentukan tingkat kesulitan dungeon secara otomatis berdasarkan performa pemain.
>
> An Expert System using Forward Chaining and Certainty Factor (CF) to automatically determine dungeon difficulty based on player performance.

---

##  Table of Contents / Daftar Isi

1. [Project Overview](#-project-overview)
2. [Features / Fitur](#-features--fitur)
3. [Folder Structure / Struktur Folder](#-folder-structure--struktur-folder)
4. [Requirements / Kebutuhan Sistem](#-requirements--kebutuhan-sistem)
5. [How to Run / Cara Menjalankan](#-how-to-run--cara-menjalankan)
6. [Algorithm Explanation / Penjelasan Algoritma](#-algorithm-explanation--penjelasan-algoritma)
7. [Scoring Logic / Logika Penilaian](#-scoring-logic--logika-penilaian)
8. [Code Structure / Struktur Kode](#-code-structure--struktur-kode)
9. [Sample Output / Contoh Output](#-sample-output--contoh-output)
10. [Known Limitations / Keterbatasan](#-known-limitations--keterbatasan)
11. [Annotated Source Code / Kode dengan Komentar](#-annotated-source-code--kode-dengan-komentar)

---

##  Project Overview

Bahasa Indonesia:
Program ini adalah simulasi Sistem Pakar yang meniru cara seorang ahli game menilai performa pemain dalam sebuah dungeon. Sistem mengumpulkan data dari 3 percobaan dungeon, menganalisis setiap faktor performa, dan akhirnya merekomendasikan tingkat kesulitan dungeon berikutnya menggunakan metode Forward Chaining dan Certainty Factor.

English:
This program is an Expert System simulation that mimics how a game expert evaluates player performance in a dungeon. The system collects data from 3 dungeon runs, analyzes each performance factor, and ultimately recommends the next dungeon difficulty level using Forward Chaining and Certainty Factor methods.

---

##  Features / Fitur

| Fitur | Deskripsi |
|---|---|
|  Input Validasi | Input dijaga agar berada dalam rentang valid |
|  Multi-Factor Scoring | 5 faktor dinilai: HP, Potion, Time, Death, Level |
|  Forward Chaining | Aturan inferensi diterapkan secara berantai |
|  Certainty Factor | Keyakinan sistem dihitung dengan rumus MYCIN |
|  Difficulty Recommendation | Output berupa rekomendasi: EASY / NORMAL / HARD |

---

## 📁 Folder Structure / Struktur Folder

```
SistemPakarProject/
│
├── SistemPakarProject.sln       # Solution file Visual Studio
│
└── SistemPakarProject/
    ├── Program.cs               # Entry point + semua logika utama
    ├── DungeonData.cs           # (opsional jika dipisah) Model data dungeon
    └── SistemPakarProject.csproj
```

> Catatan: Saat ini seluruh kode berada dalam satu file `Program.cs` dengan dua class: `DungeonData` dan `Program`.

---

##  Requirements / Kebutuhan Sistem

| Kebutuhan | Versi Minimum |
|---|---|
| .NET SDK | .NET 4.8 atau lebih baru |
| IDE (opsional) | Visual Studio 2026 / VS Code + C# Extension |
| OS | Windows / Linux / macOS |

---

##  How to Run / Cara Menjalankan

### Menggunakan Visual Studio

1. Buka file `SistemPakarProject.sln`
2. Tekan `F5` atau klik tombol ▶ Start
3. Program akan berjalan di jendela Console

### Menggunakan .NET CLI

```bash
# Clone atau download project ini
git clone https://github.com/Palanonymous/SistemPakarProject
# Masuk ke folder project
cd SistemPakarProject/SistemPakarProject

# Jalankan program
dotnet run
```

### Alur Penggunaan / Usage Flow

```
1. Program meminta input untuk 3 dungeon run
2. Setiap dungeon memerlukan 5 data input:
   - Level Player  (1 - 10)
   - HP awal       (1 - 100)
   - Potion Used   (0 - 5)
   - Clear Time    (dalam menit, 0 - 30+)
   - Jumlah Respawn (0 - 6+)
3. Setelah 3 dungeon, sistem menampilkan keputusan akhir
```

---

##  Algorithm Explanation / Penjelasan Algoritma

### 1. Forward Chaining (Inferensi Maju)

Bahasa Indonesia:
Forward Chaining adalah metode inferensi yang dimulai dari fakta menuju kesimpulan. Sistem membaca fakta input pemain (HP, potion, waktu, dll.) lalu menerapkan aturan-aturan secara berurutan hingga menghasilkan keputusan akhir.

English:
Forward Chaining is an inference method that starts from facts and moves toward a conclusion. The system reads player input facts (HP, potion, time, etc.) then applies rules sequentially until reaching a final decision.

```
[FAKTA/FACTS]          [ATURAN/RULES]            [KESIMPULAN/CONCLUSION]
  HP, Potion,    →→→   EvaluateWin()        →→→   Win / Lose
  Time, Death,   →→→   AnalyzeDungeonWeighted() →  Category (High/Medium/Low)
  Level          →→→   FinalDecision()      →→→   EASY / NORMAL / HARD
```

Aturan Inferensi (Rules):

| Rule | Kondisi | Hasil |
|---|---|---|
| R1 | Death > 6 ATAU ClearTime > 30 | Win = false |
| R2 | PerformancePercent ≥ 80 | Category = "High", CF = 0.80 |
| R3 | PerformancePercent ≥ 60 | Category = "Medium", CF = 0.60 |
| R4 | PerformancePercent < 60 | Category = "Low", CF = 0.40 |
| R5 | High ≥ 2 dari 3 dungeon | Difficulty = HARD (Dragon) |
| R6 | Medium ≥ 2 dari 3 dungeon | Difficulty = NORMAL (Golem) |
| R7 | High=1, Medium=1, Low=1 | Difficulty = NORMAL (Golem) |
| R8 | Selainnya | Difficulty = EASY (Goblin) |

---

### 2. Certainty Factor (CF) — Metode MYCIN

Bahasa Indonesia:
Certainty Factor adalah nilai keyakinan sistem terhadap suatu kesimpulan. Nilai CF berkisar antara 0.0 (tidak yakin) hingga 1.0 (sangat yakin). Untuk menggabungkan CF dari beberapa evidence, digunakan rumus MYCIN:

English:
Certainty Factor is the system's confidence value for a conclusion. CF ranges from 0.0 (uncertain) to 1.0 (very certain). To combine CF from multiple pieces of evidence, the MYCIN formula is used:

```
CF_combined = CF1 + CF2 × (1 - CF1)
```

Untuk 3 evidence, kombinasi dilakukan secara iteratif:

```
Step 1: result = CF_dungeon1
Step 2: result = result + CF_dungeon2 × (1 - result)
Step 3: result = result + CF_dungeon3 × (1 - result)
```

Contoh Perhitungan / Example Calculation:

```
CF1 = 0.80 (High), CF2 = 0.60 (Medium), CF3 = 0.40 (Low)

Step 1: result = 0.80
Step 2: result = 0.80 + 0.60 × (1 - 0.80) = 0.80 + 0.12 = 0.92
Step 3: result = 0.92 + 0.40 × (1 - 0.92) = 0.92 + 0.032 = 0.952 ≈ 0.95
```

---

##  Scoring Logic / Logika Penilaian

Setiap faktor menghasilkan skor 0 – 100, kemudian dirata-rata menjadi `PerformancePercent`.

### HP Score
> Pemain yang **mulai dungeon dengan HP rendah** namun tetap berhasil clear dianggap lebih impresif — semakin kecil HP awal, semakin tinggi skornya.

```
HPScore = (1 - (HP - 1) / 99) × 100

HP = 1   → HPScore = 100%
HP = 50  → HPScore ≈ 50.5%
HP = 100 → HPScore = 0%
```

### Potion Score
> Sedikit potion digunakan = performa lebih baik.

```
PotionScore = (1 - Potion / 5) × 100

Potion = 0 → 100%
Potion = 2 → 60%
Potion = 5 → 0%
```

### Time Score
> Waktu ideal adalah ≤ 10 menit.

```
ClearTime ≤ 10  → TimeScore = 100%
ClearTime 10–30 → Linear turun dari 100% ke 0%
ClearTime ≥ 30  → TimeScore = 0%

Contoh: ClearTime = 20 → TimeScore = 50%
```

### Death Score
> Nol kematian = performa terbaik.

```
Death = 0 → 100%
Death 1–5 → Linear turun
Death ≥ 6 → 0%

Contoh: Death = 3 → DeathScore ≈ 50%
```

### Level Score
>  Implementasi saat ini menggunakan logika invers: level rendah = skor tinggi.

```
LevelScore = (1 - (Level - 1) / 9) × 100

Level = 1  → LevelScore = 100%
Level = 5  → LevelScore ≈ 55.6%
Level = 10 → LevelScore = 0%
```

### Performance & Category

```
PerformancePercent = rata-rata (HPScore + PotionScore + TimeScore + DeathScore + LevelScore)

≥ 80% → Category: HIGH   | CF: 0.80
≥ 60% → Category: MEDIUM | CF: 0.60
< 60% → Category: LOW    | CF: 0.40
```

---

##  Code Structure / Struktur Kode

### Class: `DungeonData`
Model data yang menyimpan semua input dan hasil analisis per dungeon run.

| Field | Tipe | Deskripsi |
|---|---|---|
| `PlayerLevel` | int | Level pemain (1–10) |
| `HP` | int | HP awal saat masuk dungeon (1–100) |
| `Potion` | int | Jumlah potion yang digunakan (0–5) |
| `ClearTime` | int | Waktu clear dungeon dalam menit |
| `Death` | int | Jumlah kematian/respawn |
| `Win` | bool | Hasil menang/kalah |
| `HPScore` | double | Skor HP (0–100) |
| `PotionScore` | double | Skor Potion (0–100) |
| `TimeScore` | double | Skor waktu (0–100) |
| `DeathScore` | double | Skor kematian (0–100) |
| `LevelScore` | double | Skor level (0–100) |
| `PerformancePercent` | double | Rata-rata performa (0–100) |
| `Category` | string | High / Medium / Low |
| `CF` | double | Certainty Factor (0.0–1.0) |

---

### Method / Fungsi Utama

#### `Main()`
Entry point program. Mengatur alur input 3 dungeon, memanggil fungsi analisis, menampilkan ringkasan, dan memanggil keputusan akhir.

---

#### `ReadIntRange(string prompt, int minAllowed, int maxAllowed)`
```csharp
static int ReadIntRange(string prompt, int minAllowed, int maxAllowed)
```
Membaca input integer dari user dengan validasi rentang. Akan terus meminta ulang hingga input valid.

| Parameter | Deskripsi |
|---|---|
| `prompt` | Teks yang ditampilkan ke user |
| `minAllowed` | Nilai minimum yang diperbolehkan |
| `maxAllowed` | Nilai maksimum yang diperbolehkan |
| Return | Integer valid dalam rentang [min, max] |

---

#### `EvaluateWin(DungeonData d)`
```csharp
static void EvaluateWin(DungeonData d)
```
Menerapkan aturan Forward Chaining untuk menentukan Win/Lose.

```
IF Death > 6 OR ClearTime > 30 THEN Win = false
ELSE Win = true
```

---

#### `AnalyzeDungeonWeighted(DungeonData d)`
```csharp
static void AnalyzeDungeonWeighted(DungeonData d)
```
Menghitung skor tiap faktor, merata-ratakannya, lalu menentukan kategori dan CF.

---

#### `CombineCF(List<double> cfs)`
```csharp
static double CombineCF(List<double> cfs)
```
Menggabungkan multiple CF menggunakan rumus iteratif MYCIN.

| Parameter | Deskripsi |
|---|---|
| `cfs` | List nilai CF dari setiap dungeon |
| Return | Combined CF dibulatkan 2 desimal |

---

#### `FinalDecision(List<DungeonData> dungeons)`
```csharp
static void FinalDecision(List<DungeonData> dungeons)
```
Menghitung distribusi kategori dari 3 dungeon, menerapkan majority rule, dan menampilkan hasil akhir beserta combined CF.

---

#### `ClampInt(int v, int min, int max)`
```csharp
static int ClampInt(int v, int min, int max)
```
Helper untuk membatasi nilai integer agar tidak keluar dari rentang [min, max].

---

##  Sample Output / Contoh Output

```
=== Expert System Dungeon (Forward Chaining + CF) ===

--- Input Dungeon 1 ---
Level Player (1-10): 3
Mulai dengan HP (1-100): 80
Potion Used (0-5): 1
Clear Time (menit, 0-30+): 12
Jumlah Respawn (0-6+): 0

Dungeon 1 Summary:
 Win: True
 HP Score     : 77.8%     ← HP rendah dianggap lebih baik (rumus invers)
 Potion Score : 80.0%
 Time Score   : 90.0%
 Death Score  : 100.0%
 Level Score  : 77.8%
 Performance  : 85.1% -> High
 CF           : 0.80

--- Input Dungeon 2 ---
Level Player (1-10): 7
Mulai dengan HP (1-100): 40
Potion Used (0-5): 3
Clear Time (menit, 0-1000): 20
Jumlah Respawn (0-1000): 2

Dungeon 2 Summary:
 Win: True
 HP Score     : 60.6%
 Potion Score : 40.0%
 Time Score   : 50.0%
 Death Score  : 66.7%
 Level Score  : 33.3%
 Performance  : 50.1% -> Low
 CF           : 0.40

--- Input Dungeon 3 ---
Level Player (1-10): 5
Mulai dengan HP (1-100): 60
Potion Used (0-5): 2
Clear Time (menit, 0-1000): 15
Jumlah Respawn (0-1000): 1

Dungeon 3 Summary:
 Win: True
 HP Score     : 39.4%
 Potion Score : 60.0%
 Time Score   : 75.0%
 Death Score  : 83.3%
 Level Score  : 55.6%
 Performance  : 62.7% -> Medium
 CF           : 0.60

=== FINAL DECISION ===
Selected Difficulty : NORMAL (Golem)
Combined Certainty  : 0.95

Press Enter to exit...
```

---

##  Known Limitations / Keterbatasan

| # | Keterbatasan | Keterangan |
|---|---|---|
| 1 | **HPScore & LevelScore invers (by design)** | HP rendah saat mulai dungeon = skor lebih tinggi (pemain lebih impresif). LevelScore invers juga disengaja: level rendah yang berhasil clear = lebih hebat. Sudah sesuai desain, cukup pastikan ada dokumentasi yang jelas. |
| 2 | Jumlah dungeon tetap (hardcoded 3) | Tidak bisa diubah tanpa memodifikasi kode. |
| 3 | Bobot faktor sama rata | Semua 5 faktor diberi bobot sama (20% masing-masing). Belum ada pembobotan kustom. |
| 4 | Nilai CF kategori tetap | CF 0.80/0.60/0.40 adalah nilai tetap, bukan hasil perhitungan dinamis. |
| 5 | Tidak ada penyimpanan data | Data tidak disimpan ke file setelah program selesai. |

---

##  Annotated Source Code / Kode dengan Komentar

Berikut adalah keseluruhan kode program beserta penjelasan komentar pada setiap bagian penting.
Below is the complete source code with detailed comments explaining each important section.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemPakarProject
{
    // =========================================================
    // CLASS: DungeonData
    // Menyimpan semua fakta (input) dan hasil analisis per dungeon.
    // Objek ini diisi satu per satu selama loop 3 dungeon di Main().
    // =========================================================
    class DungeonData
    {
        // --- INPUT: Fakta dari pemain (facts / working memory) ---
        public int PlayerLevel;   // Level pemain: 1 (rendah) hingga 10 (tinggi)
        public int HP;            // HP saat mulai dungeon: 1 hingga 100 (HP rendah = performa lebih impresif)
        public int Potion;        // Jumlah potion yang dipakai: 0 (terbaik) hingga 5 (terburuk)
        public int ClearTime;     // Waktu clear dungeon dalam menit (>30 → dianggap kalah)
        public int Death;         // Jumlah respawn/kematian (>6 → dianggap kalah)

        // --- OUTPUT: Hasil inferensi dan analisis ---
        public bool Win;                 // true = menang, false = kalah
        public double HPScore;           // Skor HP (0–100), invers: start HP rendah = skor tinggi (lebih impresif)
        public double PotionScore;       // Skor potion (0–100), sedikit pakai = skor besar
        public double TimeScore;         // Skor waktu (0–100), cepat = skor besar
        public double DeathScore;        // Skor kematian (0–100), tidak mati = skor besar
        public double LevelScore;        // Skor level (0–100), invers: level rendah = skor besar
        public double PerformancePercent; // Rata-rata dari 5 skor di atas
        public string Category;          // Kategori performa: "High" / "Medium" / "Low"
        public double CF;                // Certainty Factor: 0.80 / 0.60 / 0.40
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Expert System Dungeon (Forward Chaining + CF) ===");

            // Menyimpan data dari 3 percobaan dungeon sebagai evidence
            List<DungeonData> dungeons = new List<DungeonData>();

            // -------------------------------------------------------
            // FORWARD CHAINING PHASE 1: Kumpulkan fakta dari 3 dungeon
            // Setiap dungeon = 1 evidence untuk keputusan akhir
            // -------------------------------------------------------
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\n--- Input Dungeon {i} ---");
                DungeonData d = new DungeonData();

                // Baca input dengan validasi rentang
                // Level 1–10: level rendah → LevelScore tinggi (invers)
                d.PlayerLevel = ReadIntRange("Level Player (1-10): ", 1, 10);

                // HP 1–100: pemain yang mulai dengan HP rendah dan tetap clear dungeon
                // dianggap lebih impresif → HP kecil = HPScore tinggi (invers by design)
                d.HP = ReadIntRange("Mulai dengan HP (1-100): ", 1, 100);

                // Potion 0–5: semakin sedikit dipakai semakin baik
                d.Potion = ReadIntRange("Potion Used (0-5): ", 0, 5);

                // Clear Time: ≤10 menit = ideal, >30 menit = dianggap kalah (Win=false)
                d.ClearTime = ReadIntRange("Clear Time (menit, 0-30+): ", 0, 1000);

                // Death/Respawn: 0 = terbaik, >6 = dianggap kalah (Win=false)
                d.Death = ReadIntRange("Jumlah Respawn (0-6+): ", 0, 1000);

                // -------------------------------------------------------
                // FORWARD CHAINING PHASE 2: Terapkan aturan Win/Lose
                // Rule: IF Death > 6 OR ClearTime > 30 THEN Win = false
                // -------------------------------------------------------
                EvaluateWin(d);

                // -------------------------------------------------------
                // FORWARD CHAINING PHASE 3: Analisis skor & tentukan kategori
                // Hitung HPScore, PotionScore, TimeScore, DeathScore, LevelScore
                // → rata-rata → PerformancePercent → Category + CF
                // -------------------------------------------------------
                AnalyzeDungeonWeighted(d);

                // Tampilkan ringkasan hasil per dungeon
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

            // -------------------------------------------------------
            // FORWARD CHAINING PHASE 4: Keputusan akhir dari 3 evidence
            // Gabungkan CF dan terapkan majority rule → EASY/NORMAL/HARD
            // -------------------------------------------------------
            FinalDecision(dungeons);

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        // =========================================================
        // METHOD: ReadIntRange
        // Membaca input integer dari user dan memvalidasi rentangnya.
        // Jika input bukan angka atau di luar [minAllowed, maxAllowed],
        // program akan terus meminta ulang hingga input valid.
        // =========================================================
        static int ReadIntRange(string prompt, int minAllowed, int maxAllowed)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();

                // Coba parse input sebagai integer
                if (int.TryParse(s, out int v))
                {
                    return v; // Input valid, kembalikan nilainya
                }

                // Input salah
                Console.WriteLine("Input tidak valid, masukkan angka.");
            }
        }

        // =========================================================
        // METHOD: EvaluateWin
        // Aturan Forward Chaining pertama: menentukan menang atau kalah.
        //
        // Rule R1: IF Death > 6 OR ClearTime > 30 THEN Win = false
        //          ELSE Win = true
        //
        // Catatan: Nilai Death dan ClearTime yang lebih besar dari batas
        // ini tetap diterima input-nya, tapi hasilnya langsung Lose.
        // =========================================================
        static void EvaluateWin(DungeonData d)
        {
            if (d.Death > 6 || d.ClearTime > 30)
                d.Win = false;
            else
                d.Win = true;
        }

        // =========================================================
        // METHOD: AnalyzeDungeonWeighted
        // Menghitung skor untuk setiap faktor performa (masing-masing 0–100),
        // merata-ratakannya, lalu memetakan ke kategori dan nilai CF.
        //
        // Semua faktor diberi bobot sama (equal weight = 20% masing-masing).
        // =========================================================
        static void AnalyzeDungeonWeighted(DungeonData d)
        {
            // --- HP Score (INVERS by design: start HP rendah = performa lebih impresif) ---
            // Logika: pemain yang berani masuk dungeon dengan HP kecil dan berhasil clear
            // dianggap memiliki performa lebih tinggi.
            // Rumus: (1 - (HP - 1) / 99) × 100
            // Contoh: HP=1 → 100% | HP=50 → ~50.5% | HP=100 → 0%
            d.HP = ClampInt(d.HP, 1, 100);
            d.HPScore = (1.0 - (double)(d.HP - 1) / (100 - 1)) * 100.0;

            // --- Potion Score (sedikit pakai = baik) ---
            // Rumus: (1 - Potion / 5) × 100
            // Contoh: Potion=0 → 100% | Potion=2 → 60% | Potion=5 → 0%
            d.Potion = ClampInt(d.Potion, 0, 5);
            d.PotionScore = (1.0 - (double)d.Potion / 5.0) * 100.0;

            // --- Time Score (cepat = baik, linear dalam rentang 10–30 menit) ---
            // ≤10 menit → 100% (ideal)
            // 10–30 menit → linear turun dari 100% ke 0%
            // ≥30 menit → 0% (terlalu lama)
            // Contoh: ClearTime=20 → 50%
            if (d.ClearTime <= 10) d.TimeScore = 100.0;
            else if (d.ClearTime >= 30) d.TimeScore = 0.0;
            else d.TimeScore = (1.0 - (double)(d.ClearTime - 10) / (30 - 10)) * 100.0;

            // --- Death Score (tidak mati = terbaik, linear 0–6) ---
            // 0 kematian → 100% | 6 kematian → 0%
            // Contoh: Death=3 → ~50%
            if (d.Death <= 0) d.DeathScore = 100.0;
            else if (d.Death >= 6) d.DeathScore = 0.0;
            else d.DeathScore = (1.0 - (double)d.Death / 6.0) * 100.0;

            // --- Level Score (INVERS: level rendah = skor besar) ---
            // Rumus: (1 - (Level - 1) / 9) × 100
            // Contoh: Level=1 → 100% | Level=5 → ~55.6% | Level=10 → 0%
            //  Desain ini menganggap pemain level rendah yang clear dungeon = lebih hebat
            int level = ClampInt(d.PlayerLevel, 1, 10);
            d.LevelScore = (1.0 - (double)(level - 1) / (10 - 1)) * 100.0;

            // --- Performance Percent: rata-rata dari 5 faktor (equal weight) ---
            d.PerformancePercent = (d.HPScore + d.PotionScore + d.TimeScore + d.DeathScore + d.LevelScore) / 5.0;

            // --- Mapping Performance → Category + CF ---
            // Rule R2: PerformancePercent ≥ 80 → High   (CF = 0.80)
            // Rule R3: PerformancePercent ≥ 60 → Medium (CF = 0.60)
            // Rule R4: PerformancePercent  < 60 → Low    (CF = 0.40)
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

        // =========================================================
        // METHOD: CombineCF
        // Menggabungkan beberapa nilai CF menggunakan rumus MYCIN (iteratif).
        //
        // Rumus: CF_combined = CF_prev + CF_new × (1 - CF_prev)
        //
        // Sifat: Semakin banyak evidence positif, CF semakin mendekati 1.0
        // namun tidak pernah melebihinya (asymptotic).
        //
        // Contoh untuk 3 CF: [0.80, 0.60, 0.40]
        //   Step 1: result = 0.80
        //   Step 2: result = 0.80 + 0.60 × (1 - 0.80) = 0.92
        //   Step 3: result = 0.92 + 0.40 × (1 - 0.92) = 0.952 → dibulatkan 0.95
        // =========================================================
        static double CombineCF(List<double> cfs)
        {
            if (cfs == null || cfs.Count == 0) return 0.0;

            double result = cfs[0]; // Mulai dengan CF pertama sebagai basis
            for (int i = 1; i < cfs.Count; i++)
            {
                // Gabungkan CF berikutnya secara iteratif
                result = result + cfs[i] * (1 - result);
            }

            return Math.Round(result, 2); // Bulatkan ke 2 desimal
        }

        // =========================================================
        // METHOD: FinalDecision
        // Menghitung distribusi kategori dari semua dungeon,
        // menerapkan majority rule untuk menentukan difficulty akhir,
        // dan menampilkan combined CF sebagai ukuran keyakinan sistem.
        //
        // Rule R5: High ≥ 2   → HARD (Dragon)
        // Rule R6: Medium ≥ 2 → NORMAL (Golem)
        // Rule R7: High=1, Medium=1, Low=1 → NORMAL (Golem)  [tie-breaker]
        // Rule R8: Lainnya    → EASY (Goblin)
        // =========================================================
        static void FinalDecision(List<DungeonData> dungeons)
        {
            int high = 0, medium = 0, low = 0;
            List<double> cfs = new List<double>();

            // Hitung distribusi kategori dan kumpulkan nilai CF tiap dungeon
            foreach (var d in dungeons)
            {
                if (d.Category == "High") high++;
                else if (d.Category == "Medium") medium++;
                else low++;

                cfs.Add(d.CF);
            }

            // Gabungkan CF dari semua evidence menggunakan rumus MYCIN
            double finalCF = CombineCF(cfs);

            Console.WriteLine("\n=== FINAL DECISION ===");

            // Terapkan majority rule untuk menentukan difficulty
            string difficulty;
            if (high >= 2)
                difficulty = "HARD (Dragon)";           // Mayoritas performa tinggi
            else if (medium >= 2)
                difficulty = "NORMAL (Golem)";          // Mayoritas performa menengah
            else if (high == 1 && medium == 1 && low == 1)
                difficulty = "NORMAL (Golem)";          // Tie-breaker: pilih tengah
            else
                difficulty = "EASY (Goblin)";           // Mayoritas performa rendah

            Console.WriteLine("Selected Difficulty : " + difficulty);
            Console.WriteLine("Combined Certainty  : " + finalCF);
        }

        // =========================================================
        // METHOD: ClampInt
        // Helper sederhana untuk memastikan nilai integer berada
        // dalam rentang [min, max]. Digunakan sebelum kalkulasi skor
        // agar tidak terjadi hasil di luar 0–100%.
        // =========================================================
        static int ClampInt(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
    }
}
```

---

##  Author / Pembuat

> Project ini dibuat sebagai implementasi konsep Sistem Pakar dengan metode Forward Chaining dan Certainty Factor (MYCIN) dalam konteks game dungeon.

---

*README ini dibuat untuk keperluan dokumentasi akademik / This README was created for academic documentation purposes.*
