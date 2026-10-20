using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneButtonGame
{
    // ONLY job: read and write the high score to a text file, so it survives closing the game.
    // It doesn't decide WHEN to save or WHAT counts as a high score; GameManager does that.
    public class HighScoreFile
    {
        private string folder;
        private string filePath;

        public HighScoreFile()
        {
            // Windows: C:\Users\<you>\AppData\Local\KirbyWaveJump\highscore.txt
            folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KirbyWaveJump");
            filePath = Path.Combine(folder, "highscore.txt");
        }

        // Returns the saved high score, or 0 if there isn't one yet
        public int Load()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return 0;   // first time playing, no file yet
                }

                string text = File.ReadAllText(filePath).Trim();

                int score;
                if (int.TryParse(text, out score))   // is the file actually a number?
                {
                    return score;
                }
                return 0;   // the file has something weird in it, so start fresh
            }
            catch (Exception)
            {
                return 0;   // couldn't read the file; don't crash the game over a high score
            }
        }

        public void Save(int score)
        {
            try
            {
                Directory.CreateDirectory(folder);   // makes the folder if it's missing (does nothing if it exists)
                File.WriteAllText(filePath, score.ToString());
            }
            catch (Exception)
            {
                // couldn't save (no permission, disk full, etc.), so just keep playing
            }
        }
    }
}
