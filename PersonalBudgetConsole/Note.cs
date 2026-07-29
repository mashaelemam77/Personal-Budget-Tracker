using System.Text.Json;

namespace PersonalBudgetConsole
{
    // Note class to represent individual notes
    public class Note
    {
        public DateTime Date { get; set; }
        public string Content { get; set; } = string.Empty;

        // Constructor
        public Note(DateTime date, string content)
        {
            Date = date;
            Content = content;
        }


        // Method to display note details
        public override string ToString()
        {
            return "Date: " + Date.ToString("dd/MM/yyyy") + " ----- Note: " + Content;
        }

        // Method to get formatted date string
        public string GetFormattedDate()
        {
            return Date.ToString("dd/MM/yyyy");
        }
    }

    // NoteManager class to handle all note operations
    public class NoteManager
    {
        private List<Note> notes;
        private string notesFilePath;

        // Constructor
        public NoteManager(string filePath = "notes_data.json")
        {
            notes = new List<Note>();
            notesFilePath = filePath;
            LoadNotes();
        }

        // Property to get notes (read-only)
        public List<Note> Notes => notes;

        // Add a new note
        public void AddNote(Note note)
        {
            notes.Add(note);
            SaveNotes();
        }

        // Get notes for a specific date
        public List<Note> GetNotesForDate(DateTime date)
        {
            List<Note> result = new List<Note>();
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Date.Date == date.Date)
                {
                    result.Add(notes[i]);
                }
            }
            return result;
        }

        // Get notes within a date range
        public List<Note> GetNotesByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Note> result = new List<Note>();
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].Date >= startDate && notes[i].Date <= endDate)
                {
                    result.Add(notes[i]);
                }
            }
            return result;
        }

        // Get all notes
        public List<Note> GetAllNotes()
        {
            return notes;
        }

        // Save notes to JSON file
        public void SaveNotes()
        {
            try
            {
                var data = new
                {
                    Notes = notes
                };

                string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(notesFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving notes: " + ex.Message);
            }
        }

        // Load notes from JSON file
        public void LoadNotes()
        {
            try
            {
                if (File.Exists(notesFilePath))
                {
                    string jsonString = File.ReadAllText(notesFilePath);
                    var data = JsonSerializer.Deserialize<JsonElement>(jsonString);

                    if (data.TryGetProperty("Notes", out var notesElement))
                    {
                        notes = JsonSerializer.Deserialize<List<Note>>(notesElement.GetRawText()) ?? new List<Note>();
                    }
                }
                else
                {
                    Console.WriteLine("No existing notes file found. Starting with empty notes.");
                    notes = new List<Note>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading notes: " + ex.Message);
                Console.WriteLine("Starting with empty notes.");
                notes = new List<Note>();
            }
        }

        // Clear all notes (for testing purposes)
        public void ClearAllNotes()
        {
            notes.Clear();
            SaveNotes();
        }
    }
}
