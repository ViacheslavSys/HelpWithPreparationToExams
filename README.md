# 🧠 HelpWithTestPreparation

An educational WPF application that turns Word documents into interactive test training sessions. Useful for students, teachers, or anyone preparing for exams with multiple-choice questions.

---

## 📌 Features

- 📄 **Import DOCX questions**: Parses questions and answers from `.docx` Word files.
- 🅱️ **Auto-detect correct answers**: Correct options are identified using **bold formatting**.
- 🔁 **Randomized practice**: Questions and answer options are shown in random order.
- ✅ **Progress tracking**: Correctly answered questions are removed from the pool.
- 🎨 **Theme switching**: Switch between light and dark modes using MahApps.Metro.
- 💾 **JSON export**: Automatically saves extracted questions to a `.json` file.

---

## 🗂️ Word File Format

Your input `.docx` file should follow this structure:


What is the capital of France?<br>
a) Berlin<br>
b) Madrid<br>
**c) Paris**


✔️ Correct answers should be **bolded**.  
📎 Options should begin with `a)`–`l)` (case-insensitive).

---

## 🛠️ Built With

- [.NET 8](https://dotnet.microsoft.com/)
- [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/windows/communitytoolkit/mvvm/)
- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro)
- [Open XML SDK](https://github.com/OfficeDev/Open-XML-SDK)

---

## 🚀 Getting Started
**You can use this app in two ways:**
### 1. 💻 For developers

a. Clone the repository:

```bash
git clone https://github.com/your-username/HelpWithTestPreparation.git
```
b. Open the solution in Visual Studio 2022+.<br>
c. Build and run the application.
### 2. 🧰 For users: 
1. Download the installer: `HelpWithTestPreparationInstaller.exe`  
2. Run the installer and follow the instructions.  
3. The application will appear in your Start menu after installation.

